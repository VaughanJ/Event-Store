// <copyright file="EventSourcedAggregate.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Application.Infrastructure
{
    using System.Collections.Generic;

    public abstract class EventSourcedAggregate : Entity
    {
        public List<DomainEvent> Changes { get; private set; }

        public int Version { get; protected set; }

        public EventSourcedAggregate()
        {
            Changes = new List<DomainEvent>();
        }

        public abstract void Apply(DomainEvent @event);
    }
}
