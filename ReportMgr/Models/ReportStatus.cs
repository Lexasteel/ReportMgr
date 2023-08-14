using System.ComponentModel.DataAnnotations;

namespace ReportMgr
{
    public class ReportStatus
    {
        /// <summary>
        /// Unique ID
        /// </summary>
        [Key]
        public int ReportStatusID { get; set; }
        // public virtual ICollection<ReportGenQueue> ReportGenQueues { get; set; }
        /// <summary>
        /// Report Status Conditions
        /// </summary>
        public string Status { get; set; }
    }
}
