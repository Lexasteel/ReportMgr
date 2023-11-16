using Models;
using System.Threading.Tasks;

namespace ReportMgr.Models
{
    class OvHNetMessageClient
    { }
    class MessageClient : OvHNetMessageClient
    {
        //public static bool test { get; set; } = false;
        //public ReportDefinition Report;
        //public static OvHNetClientConnection Connection;
        //public static CancellationTokenSource cancellationTokenSource;
        public MessageClient(ReportDefinition report) : base()
        {
            //cancellationTokenSource = new CancellationTokenSource();
            //if (!test)
            //{
            //    Report = report;
            //    if (Connection == null) Connection = new OvHNetClientConnection();
            //    string ip;
            //    using (DataContext db = new DataContext())
            //    {
            //        ip = db.Historians.Where(w => w.unit == report.unit).FirstOrDefault().IP;
            //    }
            //    int errConnection = Connection.Open(ip, "", "", "ReportManager");
            //    Console.WriteLine("create connection");
            //    if (errConnection < 0) throw new Exception(new OvHNetClientErrors().GetErrString(errConnection));
            //    this.Connect(Connection);
            //}
        }
        //DateTime dateStartCycle;
        public async Task GenerateReport()
        {
            //try
            //{
            //    if (!Directory.Exists(Path.GetDirectoryName(Report.destinationinfo))) Directory.CreateDirectory(Path.GetDirectoryName(Report.destinationinfo));
            //}
            //catch (Exception ex)
            //{

            //    throw ex;
            //}

            //bool cycle = true;
            //DateTime dateTime;
            //dateStartCycle = Report.Start.Value;
            //while (cycle)
            //{
            //    dateTime = Report.DateCalc(dateStartCycle);
            //    if (dateTime > Report.End) dateTime = Report.End.Value;
            //    HistTimeStamp histstart = OvHNetHelper.StringToHistTime(string.format("{0:MM'/'dd'/'yyyy HH:mm:ss}", dateStartCycle.ToUniversalTime()));
            //    HistTimeStamp histend = OvHNetHelper.StringToHistTime(string.format("{0:MM'/'dd'/'yyyy HH:mm:ss}", dateTime.ToUniversalTime()));

            //    //try
            //    //{
            //    //Events
            //    if (Report.reporttypeid == 2)
            //    {
            //        Task task = Task.Run(() => GetMessageFromHist(histstart, histend), cancellationTokenSource.Token);
            //        await Task.WhenAll(new[] { task });
            //    }
            //    //Alarms
            //    if (Report.reporttypeid == 3)
            //    {
            //        Task task = Task.Run(() => GetAlarmFromHist(histstart, histend), cancellationTokenSource.Token);
            //        await Task.WhenAll(new[] { task });
            //    }
            //    //}
            //    //catch (Exception ex)
            //    //{

            //    //    throw new Exception(ex.Message);
            //    //}

            //    dateStartCycle = dateTime;
            //    if (dateTime == Report.End) break;
            //}
            await Task.Delay(1000);

        }

        //#region ///GetAlarm

        //public void GetAlarmFromHist(HistTimeStamp histstart, HistTimeStamp histend)
        //{



        //    HistAlarmItemEntityCollection histAlarm = new HistAlarmItemEntityCollection();
        //    uint maxItems = 10000;
        //    List<string> list = new List<string>();
        //    Report.OnJump(0);

        //    int h = ReadAlarms(histstart, histend, -1, -1, 0, String.Empty, -1, 5242879, 255, ref maxItems, histAlarm);//Record Type: 1-LA(91), 2-LD(141), 4-LP(226), 8-DU(6), 16-RM(48) 
        //    long total_ticks = OvHNetHelper.HistTimeToDateTime(histend).Subtract(OvHNetHelper.HistTimeToDateTime(histstart)).Ticks;
        //    foreach (HistAlarmItemEntity item in histAlarm)
        //    {
        //        var d = OvHNetHelper.DecodeAlarm(item);
        //        Alarm alarm = new Alarm()
        //        {
        //            AlarmType = d.m_lpszAlarmType,
        //            Limits = d.limitBits.ad_pts.limit_string,
        //            PointDesc = d.m_lpszEnggDesc,
        //            pointname = d.m_lpszPointName,
        //            Priority = d.m_cPriority,
        //            TimeStamp = OvHNetHelper.HistTimeToDateTime(d.m_htTimeStamp),
        //            Units = d.limitBits.ad_pts.units_string,
        //            Value = d.m_lpszValue,
        //            PointType = d.m_cRecordType,
        //            Quality = d.m_usQualityIndex.ToString()
        //        };
        //        list.Add(strJoin(alarm));

        //    }
        //    long cur_ticks = 0;
        //    DateTime last_date;
        //    if (histAlarm.Count > 0)
        //    {
        //        last_date = OvHNetHelper.HistTimeToDateTime(histAlarm.Item(histAlarm.Count - 1).TimeStamp);
        //        cur_ticks = last_date.Subtract(OvHNetHelper.HistTimeToDateTime(histstart)).Ticks;
        //    }
        //    Report.OnJump(Convert.ToInt32(cur_ticks * 1.0 / total_ticks * 100));




        //    while (h > 0)
        //    {
        //        h = ReadNextAlarms(ref maxItems, histAlarm);



        //        foreach (HistAlarmItemEntity item in histAlarm)
        //        {
        //            var d = OvHNetHelper.DecodeAlarm(item);
        //            Alarm alarm = new Alarm()
        //            {
        //                AlarmType = d.m_lpszAlarmType,
        //                Limits = d.limitBits.ad_pts.limit_string,
        //                PointDesc = d.m_lpszEnggDesc,
        //                pointname = d.m_lpszPointName,
        //                Priority = d.m_cPriority,
        //                TimeStamp = OvHNetHelper.HistTimeToDateTime(d.m_htTimeStamp),
        //                Units = d.limitBits.ad_pts.units_string,
        //                Value = d.m_lpszValue,
        //                PointType = d.m_cRecordType
        //            };
        //            list.Add(strJoin(alarm));

        //        }
        //        last_date = OvHNetHelper.HistTimeToDateTime(histAlarm.Item(histAlarm.Count - 1).TimeStamp);
        //        cur_ticks = last_date.Subtract(OvHNetHelper.HistTimeToDateTime(histstart)).Ticks;
        //        Report.OnJump(Convert.ToInt32(cur_ticks * 1.0 / total_ticks * 100));

        //    }
        //    string HeaderAlarms = String.Join(";", "Date/Time", "Alarm Type", "Point Name", "Point description", "Priority", "Value", "Quality", "Units/State", "Limits", "PointType");
        //    WriteFile(list, HeaderAlarms);


        //    string strJoin(Alarm x)
        //    {
        //        return String.Join(";", x.TimeStamp.ToLocalTime().ToString("dd.MM.yyyy HH:mm:ss"), x.AlarmType, x.pointname, x.PointDesc, x.Priority, x.Value, x.Qtext, x.Units, x.Limits, x.TypeTxt);//), Encoding.GetEncoding("Windows-1251"));
        //    }
        //}
        //#endregion

        //#region ///GetMessage
        //public int GetMessageFromHist(HistTimeStamp histstart, HistTimeStamp histend)
        //{
        //    Report.OnJump(0);

        //    HistTxtItemEntityCollection xcollection = new HistTxtItemEntityCollection();
        //    //HistSoeItemEntityCollection histSoe = new HistSoeItemEntityCollection();
        //    uint maxItems = 10000;
        //    ushort[] ush = new ushort[0];
        //    //int s = client.ReadSOE(histstart, histend, -1, -1, 0, String.Empty, ref maxItems, histSoe);
        //    //int h = client.ReadAlarms(histstart, histend, -1, -1, 0, String.Empty, -1, 5242879, 255, ref maxItems, histAlarm);//Record Type: 1-LA(91), 2-LD(141), 4-LP(226), 8-DU(6), 16-RM(48) 
        //    List<OperEvent> OperEvents = new List<OperEvent>();
        //    List<string> list = new List<string>();
        //    long total_ticks = OvHNetHelper.HistTimeToDateTime(histend).Subtract(OvHNetHelper.HistTimeToDateTime(histstart)).Ticks;

        //    int h = ReadTextMsgs(histstart, histend, -1, -1, 0, "OPEVENT", String.Empty, 0, ush, ref maxItems, xcollection);
        //    foreach (HistTxtItemEntity item in xcollection)
        //    {
        //        OperEvent oper = new OperEvent()
        //        {
        //            TimeStamp = OvHNetHelper.HistTimeToDateTime(item.TimeStamp),
        //            Domain = item.source_id.ToString(),
        //            Drop = item.node.ToString(),
        //            Message = item.prim_text,
        //            UserName = item.ind.ToString(),
        //        };
        //        OpEvInfoEntity opEv = new OpEvInfoEntity();
        //        opEv = OvHNetHelper.DecodeOpEv(item);
        //        TextInfoEntity textInfo = new TextInfoEntity();
        //        textInfo = OvHNetHelper.DecodeText(item);
        //        list.Add(strJoin(oper));
        //    }

        //    DateTime last_date = OvHNetHelper.HistTimeToDateTime(xcollection.Item(xcollection.Count - 1).TimeStamp);
        //    long cur_ticks = last_date.Subtract(OvHNetHelper.HistTimeToDateTime(histstart)).Ticks;
        //    Report.OnJump(Convert.ToInt32(cur_ticks * 1.0 / total_ticks * 100));

        //    while (h > 0)
        //    {
        //        h = ReadNextTextMsgs(ref maxItems, xcollection);

        //        foreach (HistTxtItemEntity item in xcollection)
        //        {
        //            OperEvent oper = new OperEvent()
        //            {
        //                TimeStamp = OvHNetHelper.HistTimeToDateTime(item.TimeStamp),
        //                Domain = item.source_id.ToString(),
        //                Drop = item.node.ToString(),
        //                Message = item.prim_text,
        //                UserName = item.ind.ToString(),
        //            };
        //            OpEvInfoEntity opEv = new OpEvInfoEntity();
        //            opEv = OvHNetHelper.DecodeOpEv(item);
        //            TextInfoEntity textInfo = new TextInfoEntity();
        //            textInfo = OvHNetHelper.DecodeText(item);
        //            list.Add(strJoin(oper));
        //        }
        //        last_date = OvHNetHelper.HistTimeToDateTime(xcollection.Item(xcollection.Count - 1).TimeStamp);
        //        cur_ticks = last_date.Subtract(OvHNetHelper.HistTimeToDateTime(histstart)).Ticks;
        //        Report.OnJump(Convert.ToInt32(cur_ticks * 1.0 / total_ticks * 100));
        //    }
        //    string HeaderMessages = String.Join(";", "Date/Time", "Drop", "Message");
        //    WriteFile(list, HeaderMessages);

        //    return 0;

        //    string strJoin(OperEvent x)
        //    {
        //        return String.Join(";", x.TimeStamp.ToLocalTime().ToString("dd.MM.yyyy HH:mm:ss"), x.Drop, x.Message);
        //    }
        //}
        //#endregion

        //string CreateFileName()
        //{
        //    string file = Report.destinationinfo;
        //    file = file.Replace(" ", "_");
        //    string fname = Path.GetFileNameWithoutExtension(file);

        //    switch (Path.GetExtension(Report.destinationinfo))
        //    {
        //        case ".xlsx":
        //            throw new NotImplementedException();
        //        case ".csv":


        //            switch (Report.timeformatid)
        //            {
        //                case 1: return file.Replace(fname, fname + dateStartCycle.ToString("_yyyyMMdd_HHmmss"));
        //                case 2: return file.Replace(fname, fname + dateStartCycle.ToString("_yyyyMMdd"));
        //                case 3: return file.Replace(fname, fname + dateStartCycle.ToString("_yyyyMM"));
        //                default: throw new NotImplementedException();
        //            }
        //        default:
        //            throw new NotImplementedException();
        //    }
        //}

        //void WriteFile(IEnumerable<string> list, string header)
        //{

        //    string file = CreateFileName();
        //    if (File.Exists(file)) File.Delete(file);
        //    File.AppendAllLines(file, new string[] { header }, Encoding.GetEncoding("Windows-1251"));
        //    File.WriteAllLines(file, list, Encoding.GetEncoding("Windows-1251"));
        //    if (Report.arhive)
        //    {
        //        string dest = Path.GetDirectoryName(Report.destinationinfo) + @"\" + Report.reportname + ".zip";
        //        dest = dest.Replace(" ", "_");
        //        AddFileToArchive(file, dest, true);
        //    }
        //}


        //public void AddFileToArchive(string source, string destination, bool Delete = false)
        //{
        //    MemoryStream stream = new MemoryStream();
        //    string[] sourcefiles = new string[] { source };
        //    string pathToZipArchive = destination;
        //    ZipArchive archive;
        //    if (File.Exists(pathToZipArchive))
        //    {
        //        using (FileStream fs = File.Open(pathToZipArchive, FileMode.Open))
        //        {
        //            fs.CopyTo(stream);
        //            fs.Close();
        //        }
        //        stream.Seek(0, SeekOrigin.Begin);
        //        archive = ZipArchive.Read(stream, System.Text.Encoding.Default, false);
        //    }
        //    else
        //    {
        //        archive = new ZipArchive();
        //    }
        //    foreach (string sfile in sourcefiles)
        //    {
        //        archive.AddFile(sfile, "/");
        //    }
        //    archive.Save(pathToZipArchive);
        //    if (Delete) File.Delete(source);
        //}


    }
}
