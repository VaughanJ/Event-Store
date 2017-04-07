// <copyright file="Money.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Application.Model.ValueObjects
{
    using System;
    using System.Collections.Generic;
    using Infrastructure;

    public class Money : ValueObject<Money>, IComparable<Money>
    {
        public decimal Amount { get; set; }

        public Money()
            : this(0m)
        {
        }

        public Money(decimal amount)
        {
            Validate(amount);

            Amount = amount;
        }

        private void Validate(decimal amount)
        {
            if (amount % 0.01m != 0)
                throw new ArgumentException("Amount can be 2 decmial places only.");

            if (amount < 0)
                throw new ArgumentException("Money cannot be a negative amount");
        }

        internal Money Add(object amount)
        {
            throw new NotImplementedException();
        }

        public Money Add(Money money)
        {
            return new Money(this.Amount + money.Amount);
        }

        public bool IsGreaterThanOrEqualTo(Money money)
        {
            return this.Amount >= money.Amount;
        }

        public Money Subtract(Money money)
        {
            return new Money(this.Amount - money.Amount);
        }

        public Money MultiplyBy(int number)
        {
            return new Money(this.Amount * number);
        }

        /// <inheritdoc/>
        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<Object>() { Amount };
        }

        /// <inheritdoc/>
        public int CompareTo(Money other)
        {
            return this.Amount.CompareTo(other.Amount);
        }
    }
}
