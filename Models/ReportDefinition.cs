using System;
using System.Collections.Generic;
using Dapper.Contrib.Extensions;


namespace Models
{
    public class ReportDefinition
    { 
        [Key]
        public int reportdefinitionid { get; set; }
        public string reportname { get; set; } = "Отчет по блоку";
        public int reporttypeid { get; set; } = 1;
        public int reportdestid { get; set; } = 3;
        public string? destinationinfo { get; set; }
        public bool arhive { get; set; }
        public string header2 { get; set; } = "2,3";
        public int timeformatid { get; set; } = 2;
        public string timeperiodinfo { get; set; } = "1";
        public int sampletimeformatid { get; set; } = 1;
        public string sampletimeperiodinfo { get; set; } = "0:0:0";
        public string? additionalparams { get; set; }
        public int unit { get; set; }
        public string offset { get; set; } = new TimeSpan(0, 0, 0).ToString();
        public DateTime? nextevent { get; set; }
        public DateTime? lastused { get; set; }
        public bool enable { get; set; }
        [Write(false)]
        public List<HistPoint>? HistPoints { get; set; }

        [Write(false)] 
        public List<Historian>? Historians { get; set; }
    }

}

//    [Table("Tablename")] to specify the table name
//    [Key] to mark an auto-generated key field
//    [ExplicitKey] to mark a field that isn't generated automatically
//    [Write(true / false)] to mark(non)writable properties
//    [Computed] to mark calculated properties.