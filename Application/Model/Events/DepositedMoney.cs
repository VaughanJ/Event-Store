// <copyright file="DepositedMoney.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Application.Model.Events
{
    using System;
    using Infrastructure;
    using ValueObjects;

    internal class DepositedMoney : DomainEvent
    {
        public DepositedMoney(Guid aggregateId, Money amount) : base(aggregateId)
        {
            this.Amount = amount;
        }

        public Money Amount { get; internal set; }
    }
}