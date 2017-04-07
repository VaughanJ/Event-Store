// <copyright file="WithdrawnMoney.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Application.Model.Events
{
    using System;
    using Infrastructure;
    using ValueObjects;

    internal class MoneyBoxCreated : DomainEvent
    {
        public MoneyBoxCreated(Guid aggregateId)
            : base(aggregateId)
        {
        }
    }
}