using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;


namespace AdapterOPH
{
    //Custom event args class for my event
    public class ReportEventArgs
    {
        public int Value { get;}
        public string Message { get;}
        public LogLevel Level { get; }

        public bool WriteLog;
        public ReportEventArgs(int value, LogLevel level, string message)
        {
            Value = value;
            Message = message;
            Level = level;
        }
        public ReportEventArgs(int value, string message)
        {
            Value = value;
            Message = message;
            Level = LogLevel.Off;
        }
        public ReportEventArgs(int value)
        {
            Value = value;
            Message = "";
            Level = LogLevel.Off;
        }

    }
    
    [Table("ReportDefinitions")]
    public class ReportDefinition:IDisposable
    {
        public delegate void ReportHandler(ReportDefinition sender, ReportEventArgs e);
        #nullable enable
        public event ReportHandler? ReportChanged;
        //public event EventHandler<ReportEventArgs> ;

        public void State(int perc, LogLevel level, string message)
        {
            ReportChanged?.Invoke(this, new ReportEventArgs(perc, level, message));
            this.Progress=perc;
        }
        public void State(int perc, string message)
        {
            ReportChanged?.Invoke(this, new ReportEventArgs(perc, message));
            this.Progress = perc;
        }
        public void State(int perc)
        {
            ReportChanged?.Invoke(this, new ReportEventArgs(perc));
            this.Progress = perc;
        }

        public static readonly IDictionary<int, string> DestinationType = new Dictionary<int, string>()
        {
            //{ 1, "Окно" },
            //{ 2, "Printer" },
            { 3, "Файл" },
            //{ 4, "Email" },
            //{ 5, "Exchange" },
            //{ 6, "File at Historian" }
        };
        public static readonly IDictionary<int, string> ReportType = new Dictionary<int, string>()
        {
            { 1, "Простой" },
            { 2, "Оператор" },
            //{ 3, "Point-ExpressCalc" },
            { 3, "Алармы" },
            { 4, "RAW" },
            //{ 6, "SOE" },
            //{ 7, "Text" }
            //{8,"Unknown" }
        };
        public static readonly IDictionary<int, string> Filters = new Dictionary<int, string>()
        {
            { 0, "Actual" },
            { 1, "Actual Bit" },
            { 2, "Time Average" },
            { 3, "Maximum Value" },
            { 4, "Minimum Value" },
            { 5, "Time of Maximum" },
            { 6, "Time of Minimum" },
            { 7, "Integration" },
            { 8, "Toggle" },
            { 9, "Toggle Set" },
            { 10, "Toggle Reset" },
            { 11, "Time Set" },
            { 12, "Time Reset" },
            //{ 13, "" },
            { 14, "Total" },
            { 15, "Average" },
            { 16, "Count" },
            { 17, "Standard Deviation" },
            { 18, "End Value" },
            //{ 19, "" },
            //{ 20, "" },
            //{ 21, "" },
            { 22, "Variance" },
            { 23, "Range" },
            { 24, "Duration Good" },
            { 25, "Duration Bad" },
            { 26, "Percent Good" },
            { 27, "Percent Bad" },
            //{ 28, "" },
            { 29, "Delta" },
            { 30, "Start Value" },
            { 31, "Average Rate Of Change" },
        };
        public static readonly IDictionary<int, string> TimeType = new Dictionary<int, string>()
        {
            { 1, "Час:Мин:Сек" },
            { 2, "День" },
            { 3, "Месяц" },
            //{ 5, "No. of Intervals" },
            { 6, "Часть секунды" },
        };

        [Key]
        /// <summary>
        /// Unique ID
        /// </summary>
        /// 
        public int ReportDefinitionID { get; set; }
        /// <summary>
        /// User assigned report name
        /// </summary>
        public string ReportName { get; set; } = "Отчет по блоку";
        /// <summary>
        /// Unique ID of ReportType table entry
        /// </summary>
        public int ReportTypeID { get; set; } = 1;
        /// <summary>
        /// Unique ID of ReportsDestination table entry
        /// </summary>
        public int ReportDestID { get; set; } = 3;
        public string? DestinationInfo { get; set; }
        /// <summary>
        /// Report Header 1 String - Used inside RPT files via DLL
        /// </summary>
        public bool Arhive { get; set; }
        /// <summary>
        /// Report Header 2 String - Used inside RPT files via DLL
        /// </summary>
        public string Header2 { get; set; } = "2,3";
        /// <summary>
        /// Where clause for database row selection
        /// </summary>
        //public string WhereClause { get; set; }
        /// <summary>
        /// Time period specification
        /// </summary>
        public int TimeFormatID { get; set; } = 2;
        /// <summary>
        /// Time period specification for report data
        /// </summary>
        public string TimePeriodInfo { get; set; } = "1";
        /// <summary>
        /// Sample Time period specification format
        /// </summary>
        public int SampleTimeFormatID { get; set; } = 1;
        /// <summary>
        /// Sample Interval as Number or HH:MM:SS
        /// </summary>
        public string SampleTimePeriodInfo { get; set; } = "0:0:0";
        /// <summary>
        /// InitialValue=1;
        /// </summary>
        public string? AdditionalParams { get; set; }
        /// <summary>
        /// Number of Unit
        /// </summary>
        public int Unit { get; set; }
        public string OffSet { get; set; } = new TimeSpan(0, 0, 0).ToString();
        [NotMapped]
        public DateTime? Start { get; set; }
        [NotMapped]
        public DateTime? End { get; set; }
        public DateTime? NextEvent { get; set; }

        public DateTime? LastUsed { get; set; }
        public bool Enable { get; set; }

        public virtual List<HistPoint>? HistPoints { get; set; }

        #region ///NotMapped
        //[NotMapped]
        //public static IDictionary<int, string>? IpAddress { get; set; }
       // [NotMapped]

        //public CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        //CancellationToken token;
     //   [NotMapped]
     //   public bool AutoCycle { get; set; }

        [NotMapped]
        public OvHNetClientConnection? Connection { get; set; }
        [NotMapped]
        public bool MultiSheets { get; set; }
        [NotMapped]
        public int Progress { get; set; }
       // [NotMapped]
       // public DateTime CreateDate { get; set; }
       // [NotMapped]
       // public int ReportStatusID { get; set; }
       // [NotMapped]
      //  public string? ErrorStr { get; set; }
        [NotMapped]
        public bool ModeGUI { get; set; } = true;
        #endregion



        public DateTime DateCalc(DateTime date, bool back = false)
        {
            int days = int.Parse(TimePeriodInfo.Split(':')[0]);
            switch (TimeFormatID)
            {
                //timespan
                case 1:
                    TimeSpan ts = TimeSpan.Parse(TimePeriodInfo);
                    if (back) return date.Subtract(ts);
                    else return date.Add(ts);
                //day
                case 2:
                    if (back) return date.AddDays(-days);
                    else return date.AddDays(days);
                //month
                case 3:
                    if (back) return date.AddMonths(-days);
                    else return date.AddMonths(days);
                default: return new DateTime();
            }
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }

}

