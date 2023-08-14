using AdapterOPH;
using NLog;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReportMgr
{
    static class Program
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool FreeConsole();

        [DllImport("kernel32", SetLastError = true)]
        static extern bool AttachConsole(int dwProcessId);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        [STAThread]
        static void Main(string[] args)
        {
            string mode = args.Length > 0 ? args[0] : "gui"; //default to gui
            Logger log = LogManager.GetCurrentClassLogger();
            if (mode == "gui")
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormMain());
            }
            else if (mode == "con")
            {
                //Get a pointer to the forground window.  The idea here is that
                //IF the user is starting our application from an existing console
                //shell, that shell will be the uppermost window.  We'll get it
                //and attach to it


                IntPtr ptr = GetForegroundWindow();

                int u;

                GetWindowThreadProcessId(ptr, out u);

                Process process = Process.GetProcessById(u);


                //if (process.ProcessName == "cmd" || process.ProcessName == "devenv")    //Is the uppermost window a cmd process?
                //{
                //    AttachConsole(process.Id);
                //    Console.WriteLine(@"hello. It cmd");
                //    Console.ReadLine();
                //}



                AllocConsole();



                Console.WriteLine("Start task.");


                try
                {

                    using (DataContext db = new DataContext())
                    {
                        bool res = true;
                        var now = DateTime.Now;
                        now = now.AddMilliseconds(-now.Millisecond);
                        var reports = db.ReportDefinitions.Where(w => w.Enable.Equals(true) && w.NextEvent.Value < now).OrderBy(o=>o.ReportName).ToList();
                        var count = reports.Count();
                        Console.WriteLine($"Reports: {count}");
                        foreach (var report in reports)
                        {
                            res = true;
                            while (res)
                            {
                                Task t = Task.Run(() =>
                                {
                                    DataClient dataClient = new DataClient(report);
                                    Console.WriteLine(report.ReportName);
                                    report.LastUsed = now;
                                    report.ModeGUI = false;
                                    report.End = report.NextEvent.Value;
                                    report.Start = report.DateCalc(report.NextEvent.Value, true);
                                    res = dataClient.GenerateReport().Result;
                                    if (res)
                                    {
                                        DateTime NextEvent = report.DateCalc(report.NextEvent.Value);
                                        report.NextEvent = NextEvent;
                                        report.LastUsed = now;
                                        db.SaveChanges();
                                        if (NextEvent>now) { res = false; }
                                    }
                                    else
                                    {
                                        res = false;
                                    }
                                });
                                Task.WaitAll(t);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    log.Error(e.Message);
                    Console.WriteLine(e.Message);
                    Thread.Sleep(5000);
                }
                Console.WriteLine("End task.");
                Thread.Sleep(5000);


                //if (process.ProcessName == "cmd")    //Is the uppermost window a cmd process?
                //{
                //    AttachConsole(process.Id);
                //    Console.WriteLine(@"hello. It cmd");
                //    Console.ReadLine();
                //}
                //else
                //{
                //    //no console AND we're in console mode ... create a new console.

                //    AllocConsole();

                //    Console.WriteLine(@"hello. It looks like you double clicked me to start
                //   AND you want console mode.  Here's a new console.");
                //    Console.WriteLine("press any key to continue ...");
                //    // Console.ReadLine();

                //}

                FreeConsole();
            }

            //    if (args.Length > 0)
            //{
            //    // Command line given, display console
            //    AllocConsole();
            //    Console.WriteLine("Argument");

            //    ConsoleMain(args);
            //}
            //else
            //{

            //}
        }
        private static void ConsoleMain(string[] args)
        {
            Console.WriteLine("Command line = {0}", Environment.CommandLine);
            for (int ix = 0; ix < args.Length; ++ix)
                Console.WriteLine("Argument{0} = {1}", ix + 1, args[ix]);
            Console.ReadLine();
        }
        //static SQLiteConnection CreateConnection()
        //{
        //    SQLiteConnection sqlite_conn;
        //    sqlite_conn = new SQLiteConnection("Data Source=Rpt.db;Version=3;New=true;Compress=true;");
        //    try
        //    {
        //        sqlite_conn.Open();
        //    }
        //    catch (Exception ex)
        //    {

        //        Console.WriteLine(ex.Message);
        //    }
        //    return sqlite_conn;
        //}


    }
}
