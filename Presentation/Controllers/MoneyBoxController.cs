namespace Presentation.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    using Application.Application;
    using Application.Infrastructure;

    using Autofac.Core;

    using EventStore.ClientAPI;

    public class MoneyBoxController : ApiController
    {
        private IMoneyBoxRepository moneyBoxRepository;

        public MoneyBoxController(IMoneyBoxRepository moneyBoxRepo)
        {
            this.moneyBoxRepository = moneyBoxRepo;
        }

        // GET: api/MoneyBox
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/MoneyBox/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/MoneyBox
        public HttpResponseMessage Post([FromBody]decimal amount)
        {
            var deposit = new MoneyBoxDeposit(this.moneyBoxRepository);

            deposit.Execute(Guid.NewGuid(), amount);

            return this.Request.CreateResponse(HttpStatusCode.NoContent);
        }

        // PUT: api/MoneyBox/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/MoneyBox/5
        public void Delete(int id)
        {
        }
    }
}
