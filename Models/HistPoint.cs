using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace Models
{
    public class HistPoint
    {
        [Key]
        public int histpointid { get; set; }
        public int pointposn { get; set; }
        public string pointname { get; set; }=String.Empty;
        public int bitnumber { get; set; }
        public int proctype { get; set; }
        public string integconst { get; set; }
        public int glitchdetect { get; set; }
        public int summaryenable { get; set; }
        public int reportdefinitionid { get; set; } = 0;
        
        public string description { get; set; } = string.Empty;
        public string format { get; set; } = String.Empty;
        [Write(false)]
        public ReportDefinition ReportDefinition { get; set; }
        [Write(false)]
        public string Type { get; set; }
        [Write(false)]
        public string Units { get; set; } =string.Empty;
        [Write(false)]
        public int Scan_msec { get; set; }
        [Write(false)]
        public int Significant_Digits { get; set; }
        [Write(false)]
        public SortedDictionary<DateTime, float> F_Values { get; set; } = new SortedDictionary<DateTime, float>();

        [Write(false)]
        public uint Handle { get; set; }
    }

    //public class PointFilter
    //{
    //    [Key]
    //    public int ID { get; set; }
    //    public string Name { get; set; }
    //}
}
