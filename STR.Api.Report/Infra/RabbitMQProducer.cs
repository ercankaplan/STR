using Newtonsoft.Json;
using RabbitMQ.Client;
using STR.Data.Models.Ef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STR.Api.Report.Infra
{
    internal class RabbitMQProducer
    {
        public async Task PostAsync(ReportRequest model)
        {

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "Reporting",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var stocData = JsonConvert.SerializeObject(model);
                var body = Encoding.UTF8.GetBytes(stocData);

                channel.BasicPublish(exchange: "",
                                     routingKey: "Reporting",
                                     basicProperties: null,
                                     body: body);


            }


        }
    }
}
