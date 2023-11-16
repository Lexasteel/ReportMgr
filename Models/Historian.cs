using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Historian
    {
        [Key]
        public int HistorianID { get; set; }
        public string IP { get; set; } = String.Empty;
        public int Unit { get; set; }
        public string? UnitNet { get; set; }
        [NotMapped]
        public string Password { get; set; }
    }
}
