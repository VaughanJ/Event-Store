// <copyright file="IMoneyBoxRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Application.Infrastructure
{
    using System;
    using Model;

    public interface IMoneyBoxRepository
    {
        MoneyBox FindBy(Guid id);

        void Save(MoneyBox moneyBox);

        void Add(MoneyBox moneyBox);

        string StreamNameFor(Guid id);
    }
}