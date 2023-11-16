using DevExpress.Compression;
using Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdapterOPH
{
    public class SimpleReport : IReport
    {
        public SimpleReport(ReportDefinition report, DateTime start, DateTime end)
        {
            this.Report = report;
            this.ReportChanged += Report_ReportChanged;
            this.Start = start;
            this.End = end;
        }
        
        Logger log = LogManager.GetCurrentClassLogger();

        public delegate void ReportEvent(object sender, ReportEventArgs e);
        public event IReport.ReportEvent ReportChanged;

        public void State(int perc, int? level, string? message)//new ReportEventArgs(arguments)
        {
            ReportChanged?.Invoke(this, new ReportEventArgs(perc, level, message));
            this.Progress = perc;
        }

        public bool test = true;
        int TotalSamples = 1;
        public int Progress { get; set; }
        public ReportDefinition Report { get; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool ConsoleMode { get; set; }
        public bool MultiSheets { get; set; }

        // barStaticItemInfo.Caption = $"{sender.reportname} c {sender.Start.ToString()} по {sender.End.ToString()}: {e.Message}";

        public bool GetAttr()
        {
            var points = Report.HistPoints.FirstOrDefault(s => s.pointname.Contains("."));
            if (points == null) return false;
            var units= Report.HistPoints.GroupBy(g => g.pointname.Split('.'));
            
            foreach (var unit in units)
            {
                if (unit.Key.Length<2) continue;
                if (string.IsNullOrEmpty(unit.Key[1]) || string.IsNullOrWhiteSpace(unit.Key[1])) continue;
                State(Progress, null, $"Getting point attributes: {unit.Key[1]}");

                var ip = Report.Historians.FirstOrDefault(f => f.UnitNet == unit.Key[1])?.IP;
                Connection connection = new Connection(Report);

                var conn = connection.Open(ip, this);
                if (conn == null) return false;
                var client = new OvHNetDataClient();
                client.Connect(conn);

                int sk = 0;
                int p = 0;
                while (p < unit.Count())
                {
                    p = p + 800;
                    string[] pointNames = unit.Select(s => s.pointname).Skip(sk).Take(p - sk).ToArray();
                    uint count = (uint)pointNames.Length;
                    uint maxItems = count;
                    int[] ovhErrors = new int[maxItems];
                    uint[] items = new uint[count];
                    var collection = new PointConfigurationCollection();
                    var attributeCollection = new PointAttributeCollection();
                    int handles = client.GetHandles(count, pointNames, items, ovhErrors);
                    int config = client.GetPointConfig(pointNames.Length, pointNames, collection, ovhErrors);
                    int attrib = client.GetPointAttributes(new HistTimeStamp(), new HistTimeStamp(), count, items, ref maxItems, attributeCollection, ovhErrors);
                    List<PointAttribute> listattr = new List<PointAttribute>();
                    for (int i = 0; i < attributeCollection.Count; i++)
                    {
                        listattr.Add(attributeCollection.Item(i));
                    }

                    for (int i = 0; i < pointNames.Length; i++)
                    {
                        HistPoint h = unit.Skip(sk).Take(p - sk).ToList()[i];
                        h.Handle = items[i];
                        h.Type = collection.Item(i).DataType;
                        h.Scan_msec = collection.Item(i).scanMsecs;

                        if (h.Handle > 0)
                        {
                            PointAttribute pointAttribute = listattr.FirstOrDefault(f => f.hPoint == h.Handle);
                            h.description = pointAttribute.lpszDesc;
                            h.Units = pointAttribute.lpszEngUnits;
                        }
                        else
                        {
                            h.description = "INVALID POINT NAME";
                        }
                        if (h.Type == "P") h.description += " [" + h.bitnumber.ToString() + "]";

                    }
                    sk = p;
                }

                State(Progress + 1, 2, $"Get attributes...{unit.Key}");
                State(Progress, null, $"Point names received {unit.Key}");
                connection.Close();
            }
            State(Progress, null, $"Names of {Report.HistPoints.Count} points received.");
            return true;
        }

        public Task<bool> Generate()
        {
            var h = string.Format("Generate report from {0} to {1}", Start.ToString("G"), End.ToString("G"));
            State(0, 2, h);//LogLevel.Info

            TimeSpan period = HelpersAdapter.DateCalc(new DateTime(), Report.timeperiodinfo, Report.timeformatid).Subtract(new DateTime());
            int TotalSeconds = (int)End.Subtract(Start).TotalSeconds;
            if (Report.sampletimeformatid == 6)
            {
                TotalSamples = Convert.ToInt32(Report.HistPoints.Count * (TotalSeconds / 0.1));
            }
            else
            {
                TotalSamples = Report.HistPoints.Count * (TotalSeconds / (int)TimeSpan.Parse(Report.sampletimeperiodinfo).TotalSeconds);
            }


            var units = Report.HistPoints.GroupBy(g => g.pointname.Split('.')[1]);
            if (!GetAttr()) return Task.FromResult(false);

            foreach (var unit in units)
            {
                var ip = Report.Historians.FirstOrDefault(f => f.UnitNet == unit.Key)?.IP;
                Connection conn = new Connection(Report);
                var client = new OvHNetDataClient();
                client.Connect(conn.Open(ip, this));


                for (DateTime reportPeriod = Start; reportPeriod
                    < End; reportPeriod = HelpersAdapter.DateCalc(reportPeriod, Report.timeperiodinfo, Report.timeformatid))
                {
                   // if (HelpersAdapter.DateCalc(reportPeriod, report.timeperiodinfo, report.timeformatid) > end) end = end;
                    TimeSpan step = new TimeSpan(24, 0, 0);
                    uint pointCount = (uint)Report.HistPoints.Count;
                    uint maxItems = 10000;
                    int[] ovhErrors = new int[maxItems];
                    TimeVal val = new TimeVal();
                    switch (Report.sampletimeformatid)
                    {
                        case 1:
                            val.tv_sec = Convert.ToInt32(TimeSpan.Parse(Report.sampletimeperiodinfo).TotalSeconds);
                            break;
                        case 6:
                            double d_nsec = double.Parse(Report.sampletimeperiodinfo, CultureInfo.InvariantCulture);
                            var nsec = Convert.ToInt32(1000000000 * d_nsec);
                            val.tv_nsec = nsec;
                            break;
                        default:
                            val.tv_sec = 86400;
                            break;
                    }

                    DateTime reportStart = Start.ToUniversalTime();
                    DateTime reportEnd = End.ToUniversalTime();
                    HistTimeStamp reportStartHist = OvHNetHelper.DateTimeToHistTime(reportStart);
                    HistTimeStamp reportEndHist = new HistTimeStamp();
                    string s =
                        $"Getting data from {Start.ToString("G")} to {End.ToString("G")}";
                    State(Progress++, 2, s);
                    // while (reportStart < reportEnd)
                    //{
                    //reportEndHist = OvHNetHelper.DateTimeToHistTime(reportStart.Add(step));
                    //if (reportStart.Add(step) > report.End.Value.ToUniversalTime().AddSeconds(-val.tv_sec))
                    //{
                    reportEndHist = OvHNetHelper.DateTimeToHistTime(reportEnd);
                    //}
                    reportStartHist = OvHNetHelper.DateTimeToHistTime(reportStart);
                    HelpersAdapter.ReadProcessed(client, unit, reportStartHist, reportEndHist, val, pointCount, maxItems, ovhErrors,Report, TotalSamples);
                    //   reportStart = reportStart.Add(step);
                    //var percents = Convert.ToInt32(reportStart.Subtract(report.Start.Value).TotalSeconds * 75 / reportEnd.Subtract(report.Start.Value).TotalSeconds);
                    //percents += report.Progress;
                    //report.State(percents, LogLevel.Info, $"{percents}%");
                    // }

                    //SyncReadProcessed(HistPoints, histstart, histend, val, pointCount, maxItems, ovhErrors);

                    //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!await SaveFile(Report.HistPoints, histstart, histend, shiftday);
                    if (Report.arhive)
                    {
                        string tempPath = "";
                        using (ZipArchive archive = new ZipArchive())
                        {
                            string filename = Path.GetFullPath(Report.destinationinfo);
                            archive.AddFile(tempPath, "/");
                            archive.Save(Path.GetDirectoryName(tempPath) + "/" +
                                         Path.GetFileNameWithoutExtension(tempPath) + ".zip");
                            File.Delete(tempPath);
                        }
                    }

                    if (Report.arhive)
                    {
                        // DateTime start = OvHNetHelper.HistTimeToDateTime(histstart).ToLocalTime();
                        string source = Path.GetDirectoryName(Report.destinationinfo) + @"\" +
                                        Start.ToLocalTime().ToString("yyyy");
                        string destination = Path.GetDirectoryName(Report.destinationinfo) + @"\" +
                                             Start.ToLocalTime().ToString("yyyyMMdd") + ".zip ";

                        using (ZipArchive archive = new ZipArchive())
                        {
                            archive.AddDirectory(source, Start.ToString("yyyy"));
                            archive.Save(destination);
                            Directory.Delete(source, true);
                        }
                    }
                }

                if (client.IsConnected()) client.Disconnect();

            }

            this.ReportChanged -= Report_ReportChanged;
            var count = Report.HistPoints.Select(s => s.F_Values.Count).Sum();
            if (count > 0)
            {
                //ExportFile.Export(report);
                foreach (HistPoint item in Report.HistPoints)
                {
                    item.F_Values = new SortedDictionary<DateTime, float>();
                }
            }
            else
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);

            

        }

        public bool ToFile()
        {
            throw new NotImplementedException();
        }

        private void Report_ReportChanged(object sender, ReportEventArgs e)
        {


                Console.WriteLine("\r{0}%   {1}", e.Value, e.Message);

                switch (e.Level)
                {
                    case 0: //Trace
                    {
                        log.Trace(e.Message);
                        break;
                    }
                    case 1: //Debug
                    {
                        log.Debug(e.Message);
                        break;
                    }
                    case 2: //Info
                    {
                        log.Info(Report.reportname + ":" + e.Message);
                        break;
                    }
                    case 3: //Warn
                    {
                        log.Warn(e.Message);
                        break;
                    }
                    case 4: //Error
                    {
                        log.Error(Report.reportname + ":" + e.Message);
                        break;
                    }
                    case 5: //Fatal
                    {
                        log.Fatal(e.Message);
                        break;
                    }
                }
            //}
        }

    }
}
