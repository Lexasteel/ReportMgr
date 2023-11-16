using Models;
using System.Threading.Tasks;

namespace AdapterOPH
{
    public interface IConnection
    {
        OvHNetClientConnection Open(string? ip, IReport report);
        void Close();
    }
    public class Connection : IConnection
    {
        private readonly ReportDefinition report;
        private readonly OvHNetClientConnection connection;
        public Connection(ReportDefinition report)
        {
            this.report = report;
            connection = new OvHNetClientConnection();
        }
        public void Close()
        {
            connection.Close();
        }

        public OvHNetClientConnection? Open(string? ip, IReport report)
        {
            // var ip = _historians.FirstOrDefault(f => f.UnitNet == report.additionalparams)?.IP;
            if (ip == null) return null;
            //report.State(report.Progress, 2, $"Connecting to: {ip}");
            var task = new Task<int>(() => connection.Open(ip, "", "", "ReportManager"));
            task.Start();
            string c = $"Connecting to: {ip}";
            int sec = 0;
            while (!task.IsCompleted)
            {
                report.State(report.Progress, null, c + $" - {sec} sec");
                Task.Delay(1000).Wait();
                sec++;
            }
            int errConnection = task.Result;
            if (errConnection < 0)
            {
                string err = ip + ": " + new OvHNetClientErrors().GetErrString(errConnection);
                report.State(0, 4, err);

                return null;
            }

            return connection;
        }
    }
}
