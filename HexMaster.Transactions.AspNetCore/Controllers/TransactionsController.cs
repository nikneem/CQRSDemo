using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HexMaster.Transactions.AspNetCore.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace HexMaster.Transactions.AspNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private const string QUEUE_NAME = "transactions";
        private QueueClient _queueClient;


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateTransactionDto dto)
        {
            if (ModelState.IsValid)
            {
                var messageBody = JsonConvert.SerializeObject(dto);
                var message = new Message(Encoding.UTF8.GetBytes(messageBody));
                await _queueClient.SendAsync(message);
                return Accepted(dto);
            }

            return BadRequest();
        }

        public TransactionsController(IConfiguration configuration)
        {
            _queueClient = new QueueClient(
                configuration.GetConnectionString("ServiceBusQueueConnectionString"),
                QUEUE_NAME);
        }

    }
}