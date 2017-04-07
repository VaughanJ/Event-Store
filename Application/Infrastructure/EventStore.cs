// <copyright file="EventStore.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Application.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using global::EventStore.ClientAPI;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class EventStore : IEventStore
    {
        private const string EventClrTypeHeader = "EventClrTypeName";

        private IEventStoreConnection eventStoreConnection;

        public EventStore(IEventStoreConnection eventStoreConnection)
        {
            this.eventStoreConnection = eventStoreConnection;
        }

        public void AddSnapshot<T>(string streamName, T snapshot)
        {
            var stream = streamName + "-snapshots";
            var snapshotAsEvent = this.MapToEventStoreFormat(snapshot, Guid.NewGuid(), Guid.NewGuid());

            this.eventStoreConnection.AppendToStreamAsync(stream, ExpectedVersion.Any, snapshotAsEvent);
        }

        /// <inheritdoc/>
        public void AppendEventsToStream(string streamName, IEnumerable<DomainEvent> domainEvents, int? expectedVersion)
        {
            var commitId = Guid.NewGuid();

            var eventsFormatted = domainEvents.Select(e => this.MapToEventStoreFormat(e, commitId, e.Id));

            this.eventStoreConnection.AppendToStreamAsync(streamName, expectedVersion ?? ExpectedVersion.Any, eventsFormatted).Wait();
        }

        private EventData MapToEventStoreFormat(object @event, Guid commitId, Guid eventId)
        {
            var headers = new Dictionary<string, object>
            {
                { "CommitId", commitId },
                { EventClrTypeHeader, @event.GetType().AssemblyQualifiedName},
            };

            var data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));

            var metaData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(headers));

            return new EventData(eventId, @event.GetType().Name, true, data, metaData);
        }

        /// <inheritdoc/>
        public void CreateNewStream(string streamName, IEnumerable<DomainEvent> domainEvents)
        {
            this.AppendEventsToStream(streamName, domainEvents, null);
        }

        public T GetLatestSnapshot<T>(string streamName) where T : class
        {
            var eventStream = this.eventStoreConnection.ReadStreamEventsBackwardAsync(streamName, StreamPosition.End, 1, false);

            if (eventStream.Result.Events.Any())
            {
                return (T)this.RebuildEvent(eventStream.Result.Events.Single());
            }
            else
            {
                return null;
            }
        }

        private object RebuildEvent(ResolvedEvent eventStoreEvent)
        {
            var metaData = eventStoreEvent.OriginalEvent.Metadata;
            var data = eventStoreEvent.OriginalEvent.Data;
            var domainEventClassType =
                JObject.Parse(Encoding.UTF8.GetString(metaData)).Property(EventClrTypeHeader).Value;

            var @event = JsonConvert.DeserializeObject(
                Encoding.UTF8.GetString(data),
                Type.GetType((string)domainEventClassType));

            return @event;
        }

        /// <inheritdoc/>
        public IEnumerable<DomainEvent> GetStream(string streamName, int fromVersion, int toVersion)
        {
            int versionsToFetch = (toVersion - fromVersion) + 1;

            var domainEvents = this.eventStoreConnection.ReadStreamEventsForwardAsync(
                streamName,
                fromVersion,
                versionsToFetch,
                false);

            return domainEvents.Result.Events.Select(e => (DomainEvent)this.RebuildEvent(e));
        }
    }

    public interface IEventStore
    {
        void CreateNewStream(string streamName, IEnumerable<DomainEvent> domainEvents);

        void AppendEventsToStream(string streamName, IEnumerable<DomainEvent> domainEvents, int? expectedVersion);

        IEnumerable<DomainEvent> GetStream(string streamName, int fromVersion, int toVersion);

        void AddSnapshot<T>(string streamName, T snapshot);

        T GetLatestSnapshot<T>(string streamName) where T : class;
    }
}
