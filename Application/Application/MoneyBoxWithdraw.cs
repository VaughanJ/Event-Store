namespace Application.Application
{
    using System;
    using Infrastructure;
    using Model.ValueObjects;

    public class MoneyBoxWithdraw
    {
        private IMoneyBoxRepository moneyBoxRepository;

        public MoneyBoxWithdraw(IMoneyBoxRepository moneyBoxRepository)
        {
            this.moneyBoxRepository = moneyBoxRepository;
        }

        public void Execute(Guid id, decimal amount)
        {
            var moneyBox = this.moneyBoxRepository.FindBy(id);

            if (moneyBox.Id == Guid.Empty)
            {
                moneyBox.Create(id);
            }

            var money = new Money(amount);

            moneyBox.Withdraw(money);

            this.moneyBoxRepository.Save(moneyBox);
        }
    }
}
