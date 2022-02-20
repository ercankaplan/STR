using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using STR.Data.Models.Ef;
using STR.Reporting.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace STR.Reporting
{
    public static class QueueConsumer
    {
        public static void Consume(IModel channel, IReportsService reportsService)
        {
            channel.QueueDeclare(queue: "Reporting",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());
                Console.WriteLine(message);

                ReportRequest reportRequest = JsonConvert.DeserializeObject<ReportRequest>(message);

                reportsService.UpdateReportStatusAsync(reportRequest.Id, Data.Models.EnumReportStatus.Processing);

                reportsService.ExecuteReportAsync(reportRequest.Id);

            };
            channel.BasicConsume(queue: "Reporting",autoAck: true,consumer: consumer);
            Console.WriteLine("Consumer started");
            Console.ReadLine();
        }


    }
}
