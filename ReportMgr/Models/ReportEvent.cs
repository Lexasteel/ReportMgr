using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReportMgr
{
    //public class ReportEvent
    //{
    //    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    //    /// <summary>
    //    /// Unique ID
    //    /// </summary>
    //    public int ReportEventID { get; set; }
    //  ///  public virtual List<ReportGenQueue> ReportGenQueues { get; set; }
    //    public virtual List<ReportEventTask> ReportEventTasks { get; set; }
    //    /// <summary>
    //    /// User assigned event name
    //    /// </summary>
    //    public string EventName { get; set; }
    //    /// <summary>
    //    /// Event Type
    //    /// </summary>
    //    public int EventTypeID { get; set; }
    //    public virtual EventType EventType { get; set; }
    //    /// <summary>
    //    /// Enable/Disable Event processing
    //    /// </summary>
    //    public int Enable { get; set; }
    //    /// <summary>
    //    /// Time Format specification for timed event
    //    /// </summary>
    //    public int TimeFormatID { get; set; }
    //    //public virtual TimeFormat TimeFormat { get; set; }
    //    /// <summary>
    //    /// Time period specification
    //    /// </summary>
    //    public string TimePeriodInfo { get; set; }
    //    /// <summary>
    //    /// Offset Time Info (HH:MM:SS only)
    //    /// </summary>
    //    public string OffsetTimeInfo { get; set; }
    //    /// <summary>
    //    /// Time at which next event should occur
    //    /// </summary>
    //    public string EventDueDate { get; set; }
    //    [NotMapped]
    //    public DateTime EventDueDateTime { get
    //        {
    //            return DateTime.Parse(EventDueDate);
    //        } }
    //}
}
