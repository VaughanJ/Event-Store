// <copyright file="WithdrawnMoney.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Application.Model.Events
{
    using System;
    using Infrastructure;
    using ValueObjects;

    internal class WithdrawnMoney : DomainEvent
    {
        public WithdrawnMoney(Guid aggregateId, Money amount) : base(aggregateId)
        {
            Amount = amount;
        }

        public Money Amount { get; internal set; }
    }
}