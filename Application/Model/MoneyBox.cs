// <copyright file="MoneyBox.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Application.Model
{
    using System;

    using Events;
    using Infrastructure;
    using ValueObjects;

    public class MoneyBox : EventSourcedAggregate
    {
        private Money balance = new Money(0);

        public int? InitialVersion { get; private set; }

        public MoneyBox(MoneyBoxSnapShot snapshot)
        {
            this.Version = snapshot.Version;
            this.balance = new Money(snapshot.Balance);
            this.InitialVersion = snapshot.Version;
        }

        public MoneyBox()
        {
        }

        /// <inheritdoc/>
        public override void Apply(DomainEvent @event)
        {
            When((dynamic)@event);
            this.Version += 1;
        }

        private void When(DepositedMoney moneyAdded)
        {
            // Todo : Add balance check here !
            this.balance = this.balance.Add(moneyAdded.Amount);
        }

        private void When(WithdrawnMoney moneyWithdrawn)
        {
            this.balance = this.balance.Subtract(moneyWithdrawn.Amount);
        }

        private void When(MoneyBoxCreated moneyBoxCreated)
        {
            this.Id = moneyBoxCreated.Id;
        }

        public void Deposit(Money add)
        {
            this.Causes(new DepositedMoney(this.Id, add));
        }

        public void Create(Guid Id)
        {
            this.Causes(new MoneyBoxCreated(Id));
        }

        public void Withdraw(Money remove)
        {
            this.Causes(new WithdrawnMoney(this.Id, remove));
        }

        private void Causes(DomainEvent @event)
        {
            this.Changes.Add(@event);
            this.Apply(@event);
        }

        private MoneyBoxSnapShot GetSnapshot()
        {
            return new MoneyBoxSnapShot() {Balance = this.balance.Amount, Version = this.Version};
        }
    }
}
