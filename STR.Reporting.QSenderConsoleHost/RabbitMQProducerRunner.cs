using System;
using System.Collections.Generic;
using System.Text;

namespace STR.Reporting.QSenderConsoleHost
{
    internal static class RabbitMQProducerRunner
    {
        private static bool isProcessing = false;
        private static Timer timer;

        public static void Run() { }

        static RabbitMQProducerRunner()
        {
            timer = new Timer(15 * 1000);
            timer.Elapsed += timer_Elapsed;
            timer.Enabled = true;
            timer.Start();
        }

        static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (isProcessing)
                return;

            try
            {
                isProcessing = true;
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("Codebase.OsosRunner", "Error in host => " + ex.ToString(), System.Diagnostics.EventLogEntryType.Error);
            }
            finally
            {
                isProcessing = false;
            }

        }
    }
}
