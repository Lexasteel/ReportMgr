using AdapterOPH;
using Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapper;
using Npgsql;
using Dapper.Contrib.Extensions;

namespace ReportMgr
{
    static class Program
    {
        //[System.Runtime.InteropServices.DllImport("kernel32.dll")]
        //private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FreeConsole();

        [DllImport("kernel32", SetLastError = true)]
        static extern bool AttachConsole(int dwProcessId);
        private const int ATTACH_PARENT_PROCESS = -1;

        //[DllImport("user32.dll")]
        //static extern IntPtr GetForegroundWindow();

        //[DllImport("user32.dll", SetLastError = true)]
        //static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);


        //private static List<Historian> hist;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        static void Main(string[] args)
        {
            AttachConsole(ATTACH_PARENT_PROCESS);
            string mode = args.Length > 0 ? args[0] : "-gui"; //default to gui

            switch (mode)
            {
                case "-gui":
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new FrmMain());
                        break;
                    }
                case "-con":
                    {
                        var now = DateTime.Now;
                        var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Reports_Connection"].ConnectionString;
                        using (var _connection = new NpgsqlConnection(connectionString))
                        {
                            try
                            {


                               
                         
                                Console.WriteLine();
                                Console.Write("Connect to database");
                               
                                var t = new Task<List<ReportDefinition>>(() =>
                                {
                                   var enable = true;
                                    var sql =
                                        "SELECT * FROM reportdefinitions WHERE nextevent < @Now AND enable=@Enable";
                                    var list = _connection
                                        .Query<ReportDefinition>(sql, new { Now = now, Enable = enable }).ToList();
                                    return list;



                                });
                         
                                while (!t.IsCompleted && !t.IsFaulted)
                                {
                                    Console.Write(".");
                                    Thread.Sleep(1000);
                                    if (t.Status == TaskStatus.Created)
                                    {
                                        t.Start();
                                    }
                                }

                                Console.WriteLine("OK");
                                var reports = t.Result;
                                var count = reports.Count;
                                Console.WriteLine($"Total reports: {count}");
                                
                                foreach (var report in reports)
                                {
                                   var res = true;

                                   var sql = "SELECT * FROM histpoints WHERE reportdefinitionid=@ID";
                                   report.HistPoints = _connection.Query<HistPoint>(sql, new { ID = report.reportdefinitionid }).ToList();
                                   sql = "SELECT * FROM historians ";
                                   report.Historians = _connection.Query<Historian>(sql).ToList();

                                    while (res)
                                    {

                                        var h_count = (66 - report.reportname.Length) / 2;
                                        var header = "|" + new string(' ', h_count) + report.reportname +
                                                     new string(' ', h_count) + "|";
                                        Console.WriteLine(new string('=', header.Length));
                                        Console.WriteLine(header);
                                        Console.WriteLine(new string('=', header.Length));
                                        var end = report.nextevent.Value;
                                        var start = HelpersAdapter.DateCalc(report.nextevent.Value,
                                            report.timeperiodinfo, report.timeformatid, true);
                                        var target = CreateReport(report, start, end);
                                        target.ConsoleMode = true;
                                        res = target.Generate().Result;
                                        if (res)
                                        {
                                            DateTime NextEvent = HelpersAdapter.DateCalc(report.nextevent.Value,
                                                report.timeperiodinfo, report.timeformatid);
                                            _connection.Update(new ReportDefinition
                                            {
                                                reportdefinitionid =report.reportdefinitionid,
                                                nextevent = NextEvent,
                                                lastused = now,
                                            });
                                            if (NextEvent > now)
                                            {
                                                res = false;
                                            }
                                        }
                                        else
                                        {
                                            res = false;
                                        }
                                        //});
                                        //Task.WaitAll(t);
                                    }
                                }

                            }
                            catch (Exception e)
                            {
                                Logger log = LogManager.GetCurrentClassLogger();
                                log.Error(e.Message);
                                Console.WriteLine(e.Message);
                                Thread.Sleep(5000);
                            }
                        }

                        var gl = "GOOD LUCK!!!";
                         var fc = (66 - gl.Length) / 2;
                        Console.WriteLine(new string('=',fc)+gl+ new string('=', fc));
                        System.Windows.Forms.SendKeys.SendWait("{ENTER}");
                        //Thread.Sleep(5000);
                        FreeConsole();
                        break;
                    }
                default:
                    {
                        // AllocConsole();
                        Console.WriteLine();
                        Console.WriteLine("ReportMgr -gui or empty      window mode");
                        Console.WriteLine("ReportMgr -con               console mode");
                        System.Windows.Forms.SendKeys.SendWait("{ENTER}");
                        //Thread.Sleep(5000);
                        FreeConsole();
                        break;
                    }


            }
        }

        static IReport CreateReport(ReportDefinition report, DateTime start, DateTime end)
        {
            switch (report.reporttypeid)
            {
                case 1: //simple
                    break;
                case 2: //oper.events
                    break;
                case 3: //alarms
                    break;
                case 4: //RAW
                    return new RawReport(report, start, end);
                    

            }
            return new SimpleReport(report, start, end);
        }

       
    }
}

