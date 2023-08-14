using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AdapterOPH
{
    [Serializable]
    public class HistPoint
    {
        /// <summary>
        /// Unique ID
        /// </summary>
        [Key]
        public int HistPointID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int PointPosn { get; set; }
        /// <summary>
        ///
        /// </summary>
        public string PointName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int BitNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ProcType { get; set; }
        [NotMapped]
        public HistItemProcTypeEnum ProcTypeEnum => (HistItemProcTypeEnum)ProcType;

        /// <summary>
        /// Report Type
        /// </summary>
        public string IntegConst { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int GlitchDetect { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int SummaryEnable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ReportDefinitionID { get; set; }
        public virtual ReportDefinition ReportDefinition { get; set; }
        public string Format { get; set; } = String.Empty;

        [NotMapped]
        public string Type { get; set; }

        public string Description { get; set; }=String.Empty;
        [NotMapped]
        public string Units { get; set; }
        [NotMapped]
        public int Scan_msec { get; set; }
        [NotMapped]
        public int Significant_Digits { get; set; }
        [NotMapped]
        public SortedDictionary<DateTime, float> F_Values { get; set; }
        [NotMapped]
        public uint Handle { get; set; }
        public HistPoint()
        {
            F_Values = new SortedDictionary<DateTime, float>();
        }

        static public List<HistPoint> GetPoints(int reportDefinitionID)
        {
            using (DataContext db = new DataContext())
            {
                var plist = db.HistPoints.Where(w => w.ReportDefinitionID == reportDefinitionID).ToList();
                return plist;
            }
        }


    }

    //public class PointFilter
    //{
    //    [Key]
    //    public int ID { get; set; }
    //    public string Name { get; set; }
    //}
}
