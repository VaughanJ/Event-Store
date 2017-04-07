// <copyright file="MoneyBoxWithdraw.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Application.Application
{
    using System;
    using Infrastructure;
    using Model.ValueObjects;

    public class MoneyBoxDeposit
    {
        private IMoneyBoxRepository moneyBoxRepository;

        public MoneyBoxDeposit(IMoneyBoxRepository moneyBoxRepository)
        {
            this.moneyBoxRepository = moneyBoxRepository;
        }

        public void Execute(Guid id, decimal amount)
        {
            var moneyBox = this.moneyBoxRepository.FindBy(id);

            var money = new Money(amount);

            if (moneyBox.Id == Guid.Empty)
            {
                moneyBox.Create(id);
            }

            moneyBox.Deposit(money);

            this.moneyBoxRepository.Save(moneyBox);
        }
    }
}
