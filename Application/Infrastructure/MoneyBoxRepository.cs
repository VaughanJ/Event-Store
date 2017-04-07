// <copyright file="MoneyBoxRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Application.Infrastructure
{
    using System;

    using global::EventStore.ClientAPI;

    using Model;

    public class MoneyBoxRepository : IMoneyBoxRepository
    {
        private readonly IEventStore eventStore;

        public MoneyBoxRepository(IEventStore eventStore)
        {
            this.eventStore = eventStore;
        }

        /// <inheritdoc/>
        public MoneyBox FindBy(Guid id)
        {
            var streamName = this.StreamNameFor(id);
            var fromEventNumber = 0;

            var snapshot = this.eventStore.GetLatestSnapshot<MoneyBoxSnapShot>(streamName);
            if (snapshot != null)
            {
                fromEventNumber = snapshot.Version + 1;
            }

            var stream = this.eventStore.GetStream(streamName, fromEventNumber, 4095);

            var moneyBox = snapshot != null ? new MoneyBox(snapshot) : new MoneyBox();

            foreach (var @event in stream)
            {
                moneyBox.Apply(@event);
            }

            return moneyBox;
        }

        /// <inheritdoc/>
        public void Save(MoneyBox moneyBox)
        {
            if (moneyBox.Id == Guid.Empty)
            {
                throw new ArgumentException("Aggregate ID needs to be set."); 
            }

            var streamName = this.StreamNameFor(moneyBox.Id);

            var initVer = moneyBox.InitialVersion ?? ExpectedVersion.Any;

            this.eventStore.AppendEventsToStream(streamName, moneyBox.Changes, initVer);
        }

        /// <inheritdoc/>
        public void Add(MoneyBox moneyBox)
        {
            var streamName = this.StreamNameFor(moneyBox.Id);

            this.eventStore.CreateNewStream(streamName, moneyBox.Changes);
        }

        /// <inheritdoc/>
        public string StreamNameFor(Guid id)
        {
            return $"{typeof(MoneyBox).Name}-{id}";
        }
    }
}
