using System;


namespace ReportMgr
{
    //[NotMapped]
    //public class AlarmReport:ReportDefinition
    //{
    //    protected string FileName { get; set; }
    //    //public List<Alarm> Alarms { get; set; }

    //    public AlarmReport()
    //    {
    //        List<Alarm> Alarms = new List<Alarm>();
    //    }
    //    public int GetDataFromHist(BackgroundWorker worker, DoWorkEventArgs e)
    //    {
    //        worker.ReportProgress(0);
    //        using (OvHNetMessageClient client = new OvHNetMessageClient())
    //        {

    //            if (!client.IsConnected())
    //            {
    //                int c = client.Connect(Connection);
    //                if (c < 0)
    //                {
    //                    return c;
    //                }
    //            }

    //            HistTimeStamp histstart = OvHNetHelper.StringToHistTime(string.format("{0:MM'/'dd'/'yyyy HH:mm:ss}", Start.Value.ToUniversalTime()));
    //            HistTimeStamp histend = OvHNetHelper.StringToHistTime(string.format("{0:MM'/'dd'/'yyyy HH:mm:ss}", End.Value.ToUniversalTime()));

    //            client.Connect(Connection);
    //            HistAlarmItemEntityCollection histAlarm = new HistAlarmItemEntityCollection();
    //            uint maxItems = 10000;
    //            List<Alarm> Alarms = new List<Alarm>();
    //            if (!HeaderCSV()) return 0;
    //            int h = client.ReadAlarms(histstart, histend, -1, -1, 0, String.Empty, -1, 5242879, 255, ref maxItems, histAlarm);//Record Type: 1-LA(91), 2-LD(141), 4-LP(226), 8-DU(6), 16-RM(48) 

    //            foreach (HistAlarmItemEntity item in histAlarm)
    //            {
    //                var d = OvHNetHelper.DecodeAlarm(item);
    //                Alarm alarm = new Alarm()
    //                {
    //                    AlarmType = d.m_lpszAlarmType,
    //                    Limits = d.limitBits.ad_pts.limit_string,
    //                    PointDesc = d.m_lpszEnggDesc,
    //                    pointname = d.m_lpszPointName,
    //                    Priority = d.m_cPriority,
    //                    TimeStamp = OvHNetHelper.HistTimeToDateTime(d.m_htTimeStamp),
    //                    Units = d.limitBits.ad_pts.units_string,
    //                    Value = d.m_lpszValue,
    //                    PointType = d.m_cRecordType,
    //                    Quality = d.m_usQualityIndex.ToString()
    //                };
    //                Alarms.Add(alarm);

    //            }

    //            while (h > 0)
    //            {
    //                h = client.ReadNextAlarms(ref maxItems, histAlarm);
    //                foreach (HistAlarmItemEntity item in histAlarm)
    //                {
    //                    var d = OvHNetHelper.DecodeAlarm(item);
    //                    Alarm alarm = new Alarm()
    //                    {
    //                        AlarmType = d.m_lpszAlarmType,
    //                        Limits = d.limitBits.ad_pts.limit_string,
    //                        PointDesc = d.m_lpszEnggDesc,
    //                        pointname = d.m_lpszPointName,
    //                        Priority = d.m_cPriority,
    //                        TimeStamp = OvHNetHelper.HistTimeToDateTime(d.m_htTimeStamp),
    //                        Units = d.limitBits.ad_pts.units_string,
    //                        Value = d.m_lpszValue,
    //                        PointType = d.m_cRecordType
    //                    };
    //                    Alarms.Add(alarm);
    //                }

    //            }
    //            AppendCSV(Alarms);

    //        }

    //        if (arhive == true)
    //        {
    //            string source = Path.GetDirectoryName(Destination) + @"\" + reportname + @"\" + Start.Value.ToString("yyyy");
    //            string dest = Path.GetDirectoryName(Destination) + @"\" + reportname + "_" + Start.Value.ToString("yyyy") + ".zip ";
    //            CreateZip(source, dest, 0);
    //        }
    //        return 0;
    //    }
    //    private bool HeaderCSV()
    //    {
    //        switch (Path.GetExtension(Destination))
    //        {
    //            case ".xlsx":
    //                MessageBox.Show("Необходимо использовать тип файла .csv");
    //                return false;
    //            case ".csv":
    //                string newpath = Path.GetDirectoryName(Destination) + @"\" + reportname + @"\" + Start.Value.ToString("yyyy") + @"\";
    //                if (!Directory.Exists(newpath)) Directory.CreateDirectory(newpath);
    //                string file = newpath + reportname + "_";
    //                switch (timeformatid)
    //                {
    //                    case 3:
    //                        file += Start.Value.ToString("yyyyMM") + ".csv";
    //                        break;
    //                    default:
    //                        file += Start.Value.ToString("yyyyMMdd") + ".csv";
    //                        break;
    //                }

    //                if (File.Exists(file)) File.Delete(file);
    //                FileName = file;
    //                string header = String.Join(";", "Date/Time", "Alarm Type", "Point Name", "Point description", "Priority", "Value", "Quality", "Units/State", "Limits", "PointType");
    //                File.AppendAllLines(file, new string[] { header }, Encoding.GetEncoding("Windows-1251"));
    //                return true;
    //            default:
    //                return false;
    //        }
    //    }
    //    private void AppendCSV(List<Alarm> alarms)
    //    {

    //        File.WriteAllLines(FileName,
    //            alarms.Select(x => String.Join(";", x.TimeStamp.ToLocalTime().ToString("dd.MM.yyyy HH:mm:ss"), x.AlarmType, x.pointname, x.PointDesc, x.Priority, x.Value, x.Qtext, x.Units, x.Limits, x.TypeTxt)), Encoding.GetEncoding("Windows-1251"));


    //    }



    //}

    public class Alarm
    {
        public DateTime TimeStamp { get; set; }
        public string AlarmType { get; set; }
        public string Qtext { get {

                switch (Quality)
                {
                    case "0":
                        return "P";
                    case "1":
                        return "B";
                    case "2":
                        return "F";
                    default:
                        return String.Empty;
                }
            } }
        public string PointName { get; set; }
        public string PointDesc { get; set; }
        public int Priority { get; set; }
        public string Value { get; set; }
        public string Units { get; set; }
        public string Limits { get; set; }
        public byte  PointType { get; set; }
        public string TypeTxt { get {

                switch (PointType)
                {
                    case 6:
                        return "DU";
                    case 48:
                        return "RM";
                    case 49:
                        return "RN";
                    case 91:
                        return "LA";
                    case 141:
                        return "LD";
                    case 226:
                        return "LP";
                    default:
                        return String.Empty;
                }


            } }
        public string Quality { get; set; }



    }//end class
}
