﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Compression;
using Models;
using NLog;

namespace AdapterOPH
{
    public class RawReport : IReport
    {
        public ReportDefinition Report { get; }
        public RawReport(ReportDefinition report, DateTime start, DateTime end)
        {
            this.Report = report;
            this.Start = start;
            this.End = end;
        }

        public int Progress { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool ConsoleMode { get; set; }
        public bool MultiSheets { get; set; }

        public event IReport.ReportEvent ReportChanged;

        //public event ReportHandler ReportChanged;

        public Task<bool> Generate()
        {
        
                    int skip = 0;
                    int take = 50;
                    List<HistPoint> points = new List<HistPoint>(Report.HistPoints);
                    //while (skip < Report.HistPoints.Count)
                    int c = points.Count;
                    HistTimeStamp histstart = OvHNetHelper.DateTimeToHistTime(Start.ToUniversalTime());
                    HistTimeStamp histend = OvHNetHelper.DateTimeToHistTime(End.ToUniversalTime());
                    while (c > 0)
                    {
                        //IEnumerable<HistPoint> histPoints = Report.HistPoints.Skip(skip).Take(take);
                        IEnumerable<HistPoint> histPoints = points.Take(take);
                //?????????????????????????????????????????????????
                       // ReadRaw(client, histPoints, histstart, histend);



                      
                        //Console.WriteLine("start saveRaw = " + stopwatch.ElapsedMilliseconds);
                        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! string path = await SaveFile(Report, histstart, histend, shiftday);
                        // Report.ValueProgressBar = Convert.ToInt32((skip+take) * 1.0 / Report.HistPoints.Count * 100);
                        // Report.OnJump(Report.ValueProgressBar);
                        
                        //Console.WriteLine("end saveFiles = " + stopwatch.ElapsedMilliseconds);
                        skip += take;
                        List<string> names = histPoints.Select(s => s.pointname).ToList();

                        //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        //foreach (string item in names)
                        //{
                        //    if (File.Exists(path + item.Split('.')[0] + Path.GetExtension(Report.destinationinfo)))
                        //    {
                        //        points.Remove(points.FirstOrDefault(f => f.pointname == item));
                        //    }
                        //}
                        if (take > points.Count) take = points.Count;
                        c = points.Count;
                    }
                    if (Report.arhive)
                    {
                        // DateTime start = OvHNetHelper.HistTimeToDateTime(histstart).ToLocalTime();
                        string source = Path.GetDirectoryName(Report.destinationinfo) + @"\" + Start.ToLocalTime().ToString("yyyy");
                        string destination = Path.GetDirectoryName(Report.destinationinfo) + @"\" + Start.ToLocalTime().ToString("yyyyMMdd") + ".zip ";

                        using (ZipArchive archive = new ZipArchive())
                        {
                            archive.AddDirectory(source, Start.ToString("yyyy"));
                            archive.Save(destination);
                            Directory.Delete(source, true);
                        }
                    }
                    //???????????????????????????????????????????????
                    //State(0, LogLevel.Info, $"Сохранение файла {report.reportname}: {report.destinationinfo}");
                    return Task.FromResult(true);

        }

        

        public bool GetAttr()
        {
            throw new NotImplementedException();
        }

        public void State(int perc, int? level, string message)
        {
            throw new NotImplementedException();
        }

        public bool ToFile()
        {
            throw new NotImplementedException();
        }

        void ReadRaw(OvHNetDataClient client, IEnumerable<HistPoint> _histPoints, HistTimeStamp _histstart, HistTimeStamp _histend)
        {

            int take = _histPoints.Count();
            
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
                
                HelpersAdapter.ReadProcessed(client, histPoints, _histstart, histTsStart1s, val, (uint)take, (uint)take, ovhErrors, Report, 0);



                uint _maxItems = Convert.ToUInt32(take) * 2000;
                int[] ovhErrorsRaw = new int[_maxItems];
                PointParamsCollection _pointParams = HelpersAdapter.PointParamsCollectionInit(histPoints, true);
                var r = -1;
               

                    r = client.SyncReadRaw(_histstart, _histend, false, (uint)histPoints.Count(), _pointParams, _histSummary, ref _maxItems, _histItems, ovhErrorsRaw);

                


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
                Console.WriteLine("Start ParseRaw; ID=" + Task.CurrentId);
                
                    HelpersAdapter.ParseData(_histItems, histPoints, Report.sampletimeformatid);
                int t = 0;
                while (r > 0)
                {

                    HistItemCollection histItems = new HistItemCollection();
                    _maxItems = Convert.ToUInt32(take) * 2000;
                    ovhErrorsRaw = new int[_maxItems];
                    _histSummary = new HistSummaryCollection();
                    r = client.SyncReadNextRaw(ref _maxItems, histItems, ovhErrorsRaw);
                    if (r < 0)
                    {
                        
                        
                            if (take == 1) throw new InvalidDataException(new OvHNetClientErrors().GetErrString(r));
                            take = take / 2;
                            foreach (HistPoint item in histPoints)
                            {
                                item.F_Values = new SortedDictionary<DateTime, float>();
                            }
                        
                    }


                    //Console.WriteLine("start ParseNextRaw; ID=" + Task.CurrentId);
                    
                        HelpersAdapter.ParseData(histItems, histPoints, Report.sampletimeformatid);

                }
                //int value = Convert.ToInt32(histPoints.LastOrDefault().pointposn * 1.0 / Report.HistPoints.Count * 100.0);
                //Report.OnJump(value);
            }
        }

    }
}