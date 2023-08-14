using AdapterOPH;
using System;
using System.ComponentModel.DataAnnotations.Schema;



namespace ReportMgr
{
    [NotMapped]
    public class OperEventReport : ReportDefinition
    {
        protected string FileName { get; set; }
        //public int GetDataFromHist(BackgroundWorker worker, DoWorkEventArgs e)
        //{
        //    worker.ReportProgress(0);
        //    using (OvHNetMessageClient client = new OvHNetMessageClient())
        //    {
        //        if (!client.IsConnected())
        //        {
        //            int c = client.Connect(Connection);
        //            if (c < 0)
        //            {
        //                return c;
        //            }
        //        }
        //        HistTimeStamp histstart = OvHNetHelper.StringToHistTime(string.Format("{0:MM'/'dd'/'yyyy HH:mm:ss}", StartDate.ToUniversalTime()));
        //        HistTimeStamp histend = OvHNetHelper.StringToHistTime(string.Format("{0:MM'/'dd'/'yyyy HH:mm:ss}", EndDate.ToUniversalTime()));

        //        client.Connect(Connection);

        //        HistTxtItemEntityCollection xcollection = new HistTxtItemEntityCollection();
        //        //HistSoeItemEntityCollection histSoe = new HistSoeItemEntityCollection();

        //        uint maxItems = 500;
        //        ushort[] ush = new ushort[0];
        //        //int s = client.ReadSOE(histstart, histend, -1, -1, 0, String.Empty, ref maxItems, histSoe);
        //        //int h = client.ReadAlarms(histstart, histend, -1, -1, 0, String.Empty, -1, 5242879, 255, ref maxItems, histAlarm);//Record Type: 1-LA(91), 2-LD(141), 4-LP(226), 8-DU(6), 16-RM(48) 
        //        List<OperEvent> OperEvents = new List<OperEvent>();
        //        if (!HeaderCSV()) return 0;

        //        int h = client.ReadTextMsgs(histstart, histend, -1, -1, 0, "OPEVENT", String.Empty, 0, ush, ref maxItems, xcollection);
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
        //            OperEvents.Add(oper);
        //        }


        //        while (h > 0)
        //        {
        //            h = client.ReadNextTextMsgs(ref maxItems, xcollection);
        //            foreach (HistTxtItemEntity item in xcollection)
        //            {
        //                OperEvent oper = new OperEvent()
        //                {
        //                    TimeStamp = OvHNetHelper.HistTimeToDateTime(item.TimeStamp),
        //                    Domain = item.source_id.ToString(),
        //                    Drop = item.node.ToString(),
        //                    Message = item.prim_text,
        //                    UserName = item.ind.ToString(),
        //                };
        //                OpEvInfoEntity opEv = new OpEvInfoEntity();
        //                opEv = OvHNetHelper.DecodeOpEv(item);
        //                TextInfoEntity textInfo = new TextInfoEntity();
        //                textInfo = OvHNetHelper.DecodeText(item);
        //                OperEvents.Add(oper);
        //            }

        //        }
        //        AppendCSV(OperEvents);


        //    }
        //    if (Arhive==1)
        //    {
        //        string source = Path.GetDirectoryName(Destination) + @"\" + ReportName + @"\" + StartDate.ToString("yyyy");
        //        string dest = Path.GetDirectoryName(Destination) + @"\" + ReportName + "_" + StartDate.ToString("yyyy") + ".zip ";
        //        CreateZip(source, dest, 0);
        //    }
        //    return 0;
        //}

        //private bool HeaderCSV()
        //{
        //    switch (Path.GetExtension(Destination))
        //    {
        //        case ".xlsx":
        //            MessageBox.Show("Необходимо использовать тип файла .csv");
        //            return false;
        //        case ".csv":
        //            string newpath = Path.GetDirectoryName(Destination) + @"\" + ReportName + @"\" + Start.Value.ToString("yyyy") + @"\";
        //            if (!Directory.Exists(newpath)) Directory.CreateDirectory(newpath);
        //            string file = newpath + ReportName + "_";
        //            switch (TimeFormatID)
        //            {
        //                case 3:
        //                    file += Start.Value.ToString("yyyyMM") + ".csv";
        //                    break;
        //                default:
        //                    file += Start.Value.ToString("yyyyMMdd") + ".csv";
        //                    break;
        //            }

        //            if (File.Exists(file)) File.Delete(file);
        //            FileName = file;
        //            string header = String.Join(";", "Date/Time", "Drop", "Message");
        //            File.AppendAllLines(file, new string[] { header }, Encoding.GetEncoding("Windows-1251"));
        //            return true;
        //        default:
        //            return false;
        //    }
        //}

        //private void AppendCSV(List<OperEvent> operEvents)
        //{
        //    File.WriteAllLines(FileName,
        //        operEvents.Select(x => String.Join(";", x.TimeStamp.ToLocalTime().ToString("dd.MM.yyyy HH:mm:ss"), x.Drop, x.Message)), Encoding.GetEncoding("Windows-1251"));
        //}
    }
    [NotMapped]
    public class OperEvent
    {
        public DateTime TimeStamp { get; set; }
        public string Drop { get; set; }
        public string Domain { get; set; }
        public string UserName { get; set; }
        public string Message { get; set; }
        public string FileName { get; set; }



    }


}
