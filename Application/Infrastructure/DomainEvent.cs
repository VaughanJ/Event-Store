// <copyright file="DomainEvent.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Application.Infrastructure
{
    using System;

    public abstract class DomainEvent
    {
        public DomainEvent(Guid aggregateId)
        {
            this.Id = aggregateId;
        }

        public Guid Id { get; private set; }
    }
}
