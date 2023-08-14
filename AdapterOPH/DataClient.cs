using DevExpress.Compression;
using DevExpress.Spreadsheet;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Formatting = Newtonsoft.Json.Formatting;

namespace AdapterOPH
{

    public class DataClient : IDisposable
    {
        public ReportDefinition report { get; set; }

        public DataClient(ReportDefinition reportDefinition)
        {
            report = reportDefinition;
        }
        public static bool Test { get; set; } = false;
        Logger log = LogManager.GetCurrentClassLogger();
        int TotalSamples = 1;
        public OvHNetClientConnection ConnectionOpen()
        {
            DataContext db = new DataContext();

            string ip = db.Historians.FirstOrDefault(f => f.UnitNet == report.AdditionalParams)?.IP;
            report.Connection = new OvHNetClientConnection();
            report.State(report.Progress, LogLevel.Info, $"Connecting to: {ip}");
            Task<int> task = new Task<int>(() => report.Connection.Open(ip, "", "", "ReportManager"));
            task.Start();
            string c = $"Connecting to: {ip}";
            int sec = 0;
            while (!task.IsCompleted)
            {
                report.State(report.Progress, c + $" - {sec} sec");
                report.State(report.Progress, LogLevel.Off, c + $" - {sec} sec");

                Task.Delay(1000).Wait();
                sec++;
            }
            int errConnection = task.Result;
            if (errConnection < 0)
            {
                string err = ip + ": " + new OvHNetClientErrors().GetErrString(errConnection);
                report.State(0, LogLevel.Error, err);
                return null;
            }
            return report.Connection;
        }
        public void GetDescriptions()
        {

            var units = report.HistPoints.GroupBy(g => g.PointName.Split('.')[1]);
            foreach (var unit in units)
            {
                report.AdditionalParams = unit.Key;
                var client = new OvHNetDataClient();
                var conn = ConnectionOpen();
                if (conn is null)
                {
                    return;
                }
                client.Connect(conn);
                report.State(report.Progress, $"Getting Point names...{unit.Key}");
                GetAttributes(unit, client);
                report.State(report.Progress, $"Point names received {unit.Key}");
                client.Disconnect();
            }
            report.State(report.Progress, $"Names of {report.HistPoints.Count} points received.");

        }
        public void GetAttributes(IEnumerable<HistPoint> points, OvHNetDataClient client)
        {


            if (!Test)
            {
                int sk = 0;
                int p = 0;
                while (p < points.Count())
                {
                    p = p + 800;
                    string[] pointNames = points.Select(s => s.PointName).Skip(sk).Take(p - sk).ToArray();
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
                        HistPoint h = points.Skip(sk).Take(p - sk).ToList()[i];
                        h.Handle = items[i];
                        h.Type = collection.Item(i).DataType;
                        h.Scan_msec = collection.Item(i).scanMsecs;

                        if (h.Handle > 0)
                        {
                            PointAttribute pointAttribute = listattr.FirstOrDefault(f => f.hPoint == h.Handle);
                            h.Description = pointAttribute.lpszDesc;
                            h.Units = pointAttribute.lpszEngUnits;
                        }
                        else
                        {
                            h.Description = "INVALID POINT NAME";
                            h.Units = String.Empty;
                        }
                        if (h.Type == "P") h.Description += " [" + h.BitNumber.ToString() + "]";

                    }
                    sk = p;
                }
            }
            //  return "OK";
        }
        public Task<bool> GenerateReport()
        {
            report.ReportChanged += Report_ReportChanged;
            var h = string.Format("Generate report from {0} to {1}", report.Start.Value.ToString("G"), report.End.Value.ToString("G"));
            report.State(1, LogLevel.Info, h);
            TimeSpan period = report.DateCalc(new DateTime()).Subtract(new DateTime());
            DateTime start = report.Start.Value;
            DateTime end = report.End.Value;
            int TotalSeconds = (int)report.End.Value.Subtract(report.Start.Value).TotalSeconds;
            if (report.SampleTimeFormatID == 6)
            {
                TotalSamples = Convert.ToInt32(report.HistPoints.Count * (TotalSeconds / 0.1));
            }
            else
            {
                TotalSamples = report.HistPoints.Count * (TotalSeconds / (int)TimeSpan.Parse(report.SampleTimePeriodInfo).TotalSeconds);
            }

            var units = report.HistPoints.GroupBy(g => g.PointName.Split('.')[1]);
            if (units.Count() > 1) { report.MultiSheets = true; }
            foreach (var unit in units)
            {

                report.AdditionalParams = unit.Key;
                var client = new OvHNetDataClient();
                var conn = ConnectionOpen();
                if (conn is null)
                {
                    continue;
                }
                client.Connect(conn);
                report.State(report.Progress + 1, LogLevel.Info, $"Get attributes...{unit.Key}");
                GetAttributes(unit, client);

                //for (DateTime reportPeriod = start; reportPeriod < end; reportPeriod=report.DateCalc(reportPeriod))
                //{
                //report.Start = reportPeriod;
                //report.End = report.DateCalc(reportPeriod);
                // if (report.DateCalc(reportPeriod) > end) report.End = end;



                TimeSpan step = new TimeSpan(24, 0, 0);
                uint pointCount = (uint)report.HistPoints.Count;
                uint maxItems = 10000;
                int[] ovhErrors = new int[maxItems];
                TimeVal val = new TimeVal();
                switch (report.SampleTimeFormatID)
                {
                    case 1:
                        val.tv_sec = Convert.ToInt32(TimeSpan.Parse(report.SampleTimePeriodInfo).TotalSeconds);
                        break;
                    case 6:
                        double d_nsec = double.Parse(report.SampleTimePeriodInfo, CultureInfo.InvariantCulture);
                        var nsec = Convert.ToInt32(1000000000 * d_nsec);
                        val.tv_nsec = nsec;
                        break;
                    default:
                        val.tv_sec = 86400;
                        break;
                }

                //if (end_report.Subtract(start) < step) step = end_report.Subtract(start);
                DateTime reportStart = report.Start.Value.ToUniversalTime();
                DateTime reportEnd = report.End.Value.ToUniversalTime();
                HistTimeStamp reportStartHist = OvHNetHelper.DateTimeToHistTime(reportStart);
                HistTimeStamp reportEndHist = new HistTimeStamp();
                string s = $"Getting data from {report.Start.Value.ToString("G")} to {report.End.Value.ToString("G")}";
                report.State(report.Progress++, LogLevel.Info, s);
                // while (reportStart < reportEnd)
                //{
                //reportEndHist = OvHNetHelper.DateTimeToHistTime(reportStart.Add(step));
                //if (reportStart.Add(step) > report.End.Value.ToUniversalTime().AddSeconds(-val.tv_sec))
                //{
                reportEndHist = OvHNetHelper.DateTimeToHistTime(reportEnd);
                //}
                reportStartHist = OvHNetHelper.DateTimeToHistTime(reportStart);
                ReadProcessed(client, unit, reportStartHist, reportEndHist, val, pointCount, maxItems, ovhErrors);
                //   reportStart = reportStart.Add(step);
                //var percents = Convert.ToInt32(reportStart.Subtract(report.Start.Value).TotalSeconds * 75 / reportEnd.Subtract(report.Start.Value).TotalSeconds);
                //percents += report.Progress;
                //report.State(percents, LogLevel.Info, $"{percents}%");
                // }

                //SyncReadProcessed(HistPoints, histstart, histend, val, pointCount, maxItems, ovhErrors);

                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!await SaveFile(Report.HistPoints, histstart, histend, shiftday);
                if (report.Arhive)
                {
                    string tempPath = "";
                    using (ZipArchive archive = new ZipArchive())
                    {
                        string filename = Path.GetFullPath(report.DestinationInfo);
                        archive.AddFile(tempPath, "/");
                        archive.Save(Path.GetDirectoryName(tempPath) + "/" + Path.GetFileNameWithoutExtension(tempPath) + ".zip");
                        File.Delete(tempPath);
                    }
                }

                if (report.Arhive)
                {
                    // DateTime start = OvHNetHelper.HistTimeToDateTime(histstart).ToLocalTime();
                    string source = Path.GetDirectoryName(report.DestinationInfo) + @"\" + report.Start.Value.ToLocalTime().ToString("yyyy");
                    string destination = Path.GetDirectoryName(report.DestinationInfo) + @"\" + report.Start.Value.ToLocalTime().ToString("yyyyMMdd") + ".zip ";

                    using (ZipArchive archive = new ZipArchive())
                    {
                        archive.AddDirectory(source, report.Start.Value.ToString("yyyy"));
                        archive.Save(destination);
                        Directory.Delete(source, true);
                    }
                }
                // }
                if (client.IsConnected()) client.Disconnect();

            }
            report.ReportChanged -= Report_ReportChanged;
            var count = report.HistPoints.Select(s => s.F_Values.Count).Sum();
            if (count > 0)
            {
                ExportToFile exportToFile = new ExportToFile();
                exportToFile.ExportReadProcessed(report);
                foreach (HistPoint item in report.HistPoints)
                {
                    item.F_Values = new SortedDictionary<DateTime, float>();
                }
            }
            else
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);

            //switch (report.ReportTypeID)
            //  {
            //      case 1:

            //          break;
            //      case 2://events
            //          break;
            //      case 3://alarm
            //          break;
            //      case 4://raw
            //             //Report.State(0, Report.WhereClause);
            //          int skip = 0;
            //          int take = 50;
            //          List<HistPoint> points = new List<HistPoint>();
            //          string jsonpoints = JsonConvert.SerializeObject(report.HistPoints, Formatting.Indented, new JsonSerializerSettings
            //          {
            //              ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //          });
            //          points.AddRange(JsonConvert.DeserializeObject<List<HistPoint>>(jsonpoints));
            //          //while (skip < Report.HistPoints.Count)
            //          int c = points.Count;
            //          HistTimeStamp histstart = OvHNetHelper.DateTimeToHistTime(report.Start.Value.ToUniversalTime());
            //          HistTimeStamp histend = OvHNetHelper.DateTimeToHistTime(report.End.Value.ToUniversalTime());
            //          while (c > 0)
            //          {
            //              //IEnumerable<HistPoint> histPoints = Report.HistPoints.Skip(skip).Take(take);
            //              IEnumerable<HistPoint> histPoints = points.Take(take);
            //              ReadRaw(client, histPoints, histstart, histend);



            //              Stopwatch stopwatch = new Stopwatch();
            //              stopwatch.Start();
            //              //Console.WriteLine("start saveRaw = " + stopwatch.ElapsedMilliseconds);
            //              //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! string path = await SaveFile(Report, histstart, histend, shiftday);
            //              // Report.ValueProgressBar = Convert.ToInt32((skip+take) * 1.0 / Report.HistPoints.Count * 100);
            //              // Report.OnJump(Report.ValueProgressBar);
            //              stopwatch.Stop();
            //              //Console.WriteLine("end saveFiles = " + stopwatch.ElapsedMilliseconds);
            //              skip += take;
            //              List<string> names = histPoints.Select(s => s.PointName).ToList();

            //              //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //              //foreach (string item in names)
            //              //{
            //              //    if (File.Exists(path + item.Split('.')[0] + Path.GetExtension(Report.DestinationInfo)))
            //              //    {
            //              //        points.Remove(points.FirstOrDefault(f => f.PointName == item));
            //              //    }
            //              //}
            //              if (take > points.Count) take = points.Count;
            //              c = points.Count;
            //          }
            //          if (report.Arhive)
            //          {
            //              // DateTime start = OvHNetHelper.HistTimeToDateTime(histstart).ToLocalTime();
            //              string source = Path.GetDirectoryName(report.DestinationInfo) + @"\" + report.Start.Value.ToLocalTime().ToString("yyyy");
            //              string destination = Path.GetDirectoryName(report.DestinationInfo) + @"\" + report.Start.Value.ToLocalTime().ToString("yyyyMMdd") + ".zip ";

            //              using (ZipArchive archive = new ZipArchive())
            //              {
            //                  archive.AddDirectory(source, report.Start.Value.ToString("yyyy"));
            //                  archive.Save(destination);
            //                  Directory.Delete(source, true);
            //              }
            //          }
            //          report.State(0, LogLevel.Info, $"Сохранение файла {report.ReportName}: {report.DestinationInfo}");
            //          break;
            //      default:
            //          break;
            //  }

        }

        private void Report_ReportChanged(ReportDefinition sender, ReportEventArgs e)
        {
            if (e.Level.Ordinal == 6)
            {
                Console.Write("\r{0}%   {1}", e.Value, e.Message);
            }
            else
            {
                Console.WriteLine(e.Message);
            }


            switch (e.Level.Ordinal)
            {
                case 0://Trace
                    {
                        log.Trace(e.Message);
                        break;
                    }
                case 1://Debug
                    {
                        log.Debug(e.Message);
                        break;
                    }
                case 2://Info
                    {
                        log.Info(report.ReportName + ":" + e.Message);
                        break;
                    }
                case 3://Warn
                    {
                        log.Warn(e.Message);
                        break;
                    }
                case 4://Error
                    {
                        log.Error(e.Message);
                        break;
                    }
                case 5://Fatal
                    {
                        log.Fatal(e.Message);
                        break;
                    }
            }
        }

        void ReadProcessed(OvHNetDataClient client, IEnumerable<HistPoint> _histPoints, HistTimeStamp _histstart, HistTimeStamp _histend, TimeVal _val, uint pointCount, uint maxItems, int[] ovhErrors)
        {
            if (Test) { Thread.Sleep(1000); return; }
            HistSummaryCollection _histSummary = new HistSummaryCollection();
            int pr = 0;
            int count = 0;
            HistItemCollection _histItems = new HistItemCollection();
            PointParamsCollection pointParams = PointParamsCollectionInit(_histPoints, false);
            pr = client.SyncReadProcessed(_histstart, _histend, _val, (uint)_histPoints.Count(), pointParams, _histSummary, ref maxItems, _histItems, ovhErrors);
            if (pr < 0) { throw new Exception(new OvHNetClientErrors().GetErrString(pr)); }
            ParseData(_histItems, _histPoints, report.SampleTimeFormatID);
            string first = _histPoints.First().F_Values.First().Key.ToLocalTime().ToString("G");
            string last = _histPoints.First().F_Values.Last().Key.ToLocalTime().ToString("G");
            string s = $"Getting data from {first} to {last}";
            count += _histItems.Count;
            var percs = count * 75 / TotalSamples;
            if (report.Progress < percs) report.State(percs, LogLevel.Off, s);
            if (_val.tv_sec == 0) _val.tv_sec = 1;
            while (pr > 0)
            {
                _histItems = new HistItemCollection();
                pr = client.SyncReadNextProcessed(ref maxItems, _histItems, ovhErrors);
                ParseData(_histItems, _histPoints, report.SampleTimeFormatID);
                count += _histItems.Count;
                percs = count * 75 / TotalSamples;
                first = _histPoints.First().F_Values.First().Key.ToLocalTime().ToString("G");
                last = _histPoints.First().F_Values.Last().Key.ToLocalTime().ToString("G");
                s = $"Getting data from {first} to {last}";
                if (report.Progress < percs) report.State(percs, LogLevel.Off, s);
            }

        }

        void ReadRaw(OvHNetDataClient client, IEnumerable<HistPoint> _histPoints, HistTimeStamp _histstart, HistTimeStamp _histend)
        {

            int take = _histPoints.Count();
            Stopwatch stopwatch = new Stopwatch();
            //uint maxItems = 0;
            int skip = 0;

            while (skip < _histPoints.Count())
            {
                IEnumerable<HistPoint> histPoints = _histPoints.Skip(skip).Take(take);

                HistItemCollection _histItems = new HistItemCollection();
                HistSummaryCollection _histSummary = new HistSummaryCollection();
                int[] ovhErrors = new int[take];
                TimeVal val = new TimeVal()
                {
                    tv_sec = 1
                };
                DateTime tempStart = OvHNetHelper.HistTimeToDateTime(_histstart).AddSeconds(1);
                HistTimeStamp histTsStart1s = OvHNetHelper.StringToHistTime($"{tempStart:MM'/'dd'/'yyyy HH:mm:ss}");

                stopwatch.Restart();
                ReadProcessed(client, histPoints, _histstart, histTsStart1s, val, (uint)take, (uint)take, ovhErrors);
                stopwatch.Stop();

                Console.WriteLine("end Procced 1s = " + stopwatch.ElapsedMilliseconds);


                uint _maxItems = Convert.ToUInt32(take) * 2000;
                int[] ovhErrorsRaw = new int[_maxItems];
                PointParamsCollection _pointParams = PointParamsCollectionInit(histPoints, true);
                stopwatch.Restart();
                var r = -1;
                if (Test) { r = 1; Thread.Sleep(1000); }
                else
                {

                    r = client.SyncReadRaw(_histstart, _histend, false, (uint)histPoints.Count(), _pointParams, _histSummary, ref _maxItems, _histItems, ovhErrorsRaw);

                }

                Console.WriteLine("end Raw = " + stopwatch.ElapsedMilliseconds);

                if (r < 0)
                {

                    if (take == 1) throw new Exception(new OvHNetClientErrors().GetErrString(r));
                    take = take / 2;

                    foreach (HistPoint item in histPoints)
                    {
                        item.F_Values = new SortedDictionary<DateTime, float>();
                    }
                    continue;
                }
                skip = skip + take;
                stopwatch.Restart();
                //Console.WriteLine("Start ParseRaw; ID=" + Task.CurrentId);
                if (Test) { Thread.Sleep(600); }
                else
                    ParseData(_histItems, histPoints);
                stopwatch.Stop();
                Console.WriteLine("End ParseRaw = " + stopwatch.ElapsedMilliseconds);
                int t = 0;
                while (r > 0)
                {

                    HistItemCollection histItems = new HistItemCollection();
                    _maxItems = Convert.ToUInt32(take) * 2000;
                    ovhErrorsRaw = new int[_maxItems];
                    _histSummary = new HistSummaryCollection();
                    stopwatch.Restart();
                    r = client.SyncReadNextRaw(ref _maxItems, histItems, ovhErrorsRaw);
                    stopwatch.Stop();
                    Console.WriteLine("End NextRaw = " + stopwatch.ElapsedMilliseconds);
                    if (r < 0)
                    {
                        if (Test)
                        {
                            r = 1;
                            t++;
                            if (t >= 6) break;
                        }
                        else
                        {
                            if (take == 1) throw new InvalidDataException(new OvHNetClientErrors().GetErrString(r));
                            take = take / 2;
                            foreach (HistPoint item in histPoints)
                            {
                                item.F_Values = new SortedDictionary<DateTime, float>();
                            }
                        }
                    }


                    stopwatch.Restart();
                    //Console.WriteLine("start ParseNextRaw; ID=" + Task.CurrentId);
                    if (Test) { Thread.Sleep(50); }
                    else
                        ParseData(histItems, histPoints);
                    stopwatch.Stop();
                    Console.WriteLine("end ParseNextRaw = " + stopwatch.ElapsedMilliseconds);

                }
                //int value = Convert.ToInt32(histPoints.LastOrDefault().PointPosn * 1.0 / Report.HistPoints.Count * 100.0);
                //Report.OnJump(value);
            }
        }

        public void ParseData(HistItemCollection histItemCollection, IEnumerable<HistPoint> histPoints, int sampleTimeFormatID = 1)
        {

            //   Dictionary<uint, HistPoint> dictHistPoints = histPoints.ToDictionary(d => d.Handle, s => s);

            List<HistItem> items = new List<HistItem>();
            foreach (HistItem item in histItemCollection)
            {
                items.Add(item);
            }

            Parallel.ForEach(histPoints, hp =>
            {

                IEnumerable<HistItem> histItems = items.Where(w => w.hPoint == hp.Handle);
                int decim = 2;
                if (hp.Format.Contains("."))
                {
                    string[] format = hp.Format.Split('.');
                    if (format.Length > 1) decim = format[1].Length;
                }

                foreach (HistItem i in histItems)
                {


                    DateTime d;
                    if (i.dwNSec > 0)
                    {
                        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        //
                        d = sampleTimeFormatID == 6 ? OvHNetHelper.HistTimeToDateTime(i.tsTimeStamp).AddTicks(i.dwNSec / 100) : OvHNetHelper.HistTimeToDateTime(i.tsTimeStamp).AddSeconds(1);
                    }
                    else
                    {
                        d = OvHNetHelper.HistTimeToDateTime(i.tsTimeStamp);
                    }

                    switch (hp.Type)
                    {
                        case "R":
                            hp.F_Values[d] = (float)Math.Round(i.flValue, decim);
                            break;
                        case "D":
                            // uint bit = (i.dwStatus >> 1) & 1;
                            hp.F_Values[d] = Convert.ToSingle(GetBit(i.dwStatus, 0));
                            break;
                        case "P":
                            hp.F_Values[d] = Convert.ToSingle(GetBit(i.dwRawValue, hp.BitNumber));
                            break;
                        default:
                            break;
                    }


                    if (i.dwIndicator == 0)
                    {
                        //  LogWrite(String.Join(";", Points[i.hPoint].PointName, i.tsTimeStamp.ToString(), i.dwSampFlags, i.dwIndicator, i.dwStatus));
                    }
                    //if (GetBit(i.dwStatus, 8) == true && OnlyGP)
                    //{
                    //    // LogWrite(String.Join(";", Points[i.hPoint].PointName, i.tsTimeStamp.ToString(), i.dwSampFlags, i.dwIndicator, i.dwStatus, "BAD"));
                    //    continue;
                    //}

                }
            });
            items = null;
        }

        static bool GetBit(uint b, int bitNumber)
        {
            return (b & (1 << bitNumber)) != 0;
        }

        PointParamsCollection PointParamsCollectionInit(IEnumerable<HistPoint> _histPoints, bool RAW)
        {
            PointParamsCollection pointParamsCollection = new PointParamsCollection();
            foreach (var item in _histPoints)
            {
                PointParams pointParams = new PointParams()
                {
                    dwBitPosition = Convert.ToUInt32(item.BitNumber),
                    flConstant = Convert.ToSingle(item.IntegConst, new CultureInfo("en-US")),
                    hPoint = item.Handle,
                    dwOptions = 1
                };
                pointParams.dwAggregate = !RAW ? Convert.ToUInt32(item.ProcType) : (uint)4096;
                pointParamsCollection.Add(pointParams);
            }
            return pointParamsCollection;
        }

        public void Dispose()
        {
        }
        private string CheckPath(ReportDefinition report)
        {
            try
            {
                var path = report.DestinationInfo;
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                { Directory.CreateDirectory(Path.GetDirectoryName(path) ?? string.Empty); }
                return "";
            }
            catch (Exception ex)
            {
                log.Error($"Can't created {report.DestinationInfo}");
                return ex.Message;
            }
        }

    }
}