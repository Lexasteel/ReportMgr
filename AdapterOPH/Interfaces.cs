using Models;
using System;
using System.Threading.Tasks;

namespace AdapterOPH
{
    public interface IReport
    {
        ReportDefinition Report { get; }
        DateTime Start { get; set; }
        DateTime End { get; set; }
        bool ConsoleMode { get; set; }
        bool GetAttr();
        Task<bool> Generate();
        bool ToFile();
       int Progress { get; set; }
       bool MultiSheets { get; set; }
       void State(int perc, int? level, string? message);
       delegate void ReportEvent(object sender, ReportEventArgs e);
        event ReportEvent ReportChanged;
    }
}
