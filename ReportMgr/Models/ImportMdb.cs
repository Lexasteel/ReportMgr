using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ReportMgr
{
    public class ImportMdb
    {
        string ConnectionReports
        {
            get
            {
                string c = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=";
                return c + Filename;

            }
        }

        public string Filename { get; set; }
        public int Unit { get; set; }

        public IEnumerable<ReportEventTask> GetReportEventTasksMdb()
        {
            List<ReportEventTask> list = new List<ReportEventTask>();
            using (OleDbConnection conBase = new OleDbConnection(ConnectionReports))
            {
                if (conBase.State == ConnectionState.Closed) conBase.Open();
                string cmdText = "SELECT * FROM tblReportEventTasks";
                using (OleDbCommand command = new OleDbCommand(cmdText.ToString(), conBase))
                {
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                list.Add(new ReportEventTask()
                                {
                                    EventTaskID = reader.GetInt32(0),
                                    ReportEventID = reader.GetInt32(1),
                                    ReportDefinitionID = reader.GetInt32(2),
                                    PrintOrder = reader.GetInt32(3)

                                });
                            }
                        }
                    }
                }
            }
            return list;

        }

        public List<ReportEvent> GetReportEventsMdb()
        {
            List<ReportEvent> list = new List<ReportEvent>();
            using (OleDbConnection conBase = new OleDbConnection(ConnectionReports))
            {
                if (conBase.State == ConnectionState.Closed) conBase.Open();
                string cmdText = "SELECT * FROM tblReportEvents a inner Join tblTimedEventParams b on a.ReportEventID=b.ReportEventID where RefEventTypeID > 1";
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmdText, conBase))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {

                        ReportEvent re = new ReportEvent()
                        {
                            ReportEventID= (int)row["a.ReportEventID"],
                            EventName = row["EventName"].ToString(),
                            EventTypeID = (int)row["RefEventTypeID"],
                            Enable = (int)row["RefEventTypeID"],
                            TimeFormatID = (int)row["RefTimeFormatID"],
                            TimePeriodInfo = row["TimePeriodInfo"].ToString(),
                            OffsetTimeInfo = row["OffsetTimeInfo"].ToString(),
                            EventDueDate = row["EventDueDate"].ToString()
                        };
                         list.Add(re);
                    }
                   



                }
            }
            return list;

        }

        public List<HistPoint> GetHistPointsMdb()
        {
            List<HistPoint> list = new List<HistPoint>();
            using (OleDbConnection conBase = new OleDbConnection(ConnectionReports))
            {
                if (conBase.State == ConnectionState.Closed) conBase.Open();
                string cmdText = "SELECT * FROM tblReportPointList";
                using (OleDbCommand command = new OleDbCommand(cmdText.ToString(), conBase))
                {
                    using (OleDbDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                list.Add(new HistPoint()
                                {
                                    HistPointID = reader.GetInt32(0),
                                    PointPosn = reader.GetInt32(1),
                                    PointName = reader.GetString(2),
                                    BitNumber = reader.GetInt32(3),
                                    ProcType = reader.GetInt32(4),
                                    IntegConst = reader.GetString(5),
                                    GlitchDetect = Convert.ToInt32(reader.GetBoolean(6)),
                                    SummaryEnable = Convert.ToInt32(reader.GetBoolean(7)),
                                    ReportDefinitionID = reader.GetInt32(8)
                                });
                            }
                        }
                    }
                }
            }
            return list;

        }

        public List<ReportDefinition> GetReportDefinitionsMdb()
        {
            try
            {
                List<ReportDefinition> reports = new List<ReportDefinition>();
                using (OleDbConnection conBase = new OleDbConnection(ConnectionReports))
                {
                    if (conBase.State == ConnectionState.Closed) conBase.Open();
                    string cmdText = "SELECT * FROM tblReportDefinitions as a inner join tblReportFormats as b on a.ReportFormatID=b.ReportFormatID";
                    using (OleDbCommand command = new OleDbCommand(cmdText, conBase))
                    {
                        DataTable dt = new DataTable();
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter(cmdText, conBase))
                        {

                            adapter.Fill(dt);
                        }
                        foreach (DataRow row in dt.Rows)
                        {
                            ReportDefinition report = new ReportDefinition
                            {
                                ReportDefinitionID = (int)row["ReportDefinitionID"],
                                ReportName = row["ReportName"].ToString(),
                                ReportTypeID = (int)row["RefReportTypeID"],
                                ReportDestID = (int)row["RefReportDestID"],
                                DestinationInfo = row["DestinationInfo"].ToString(),
                                Arhive = 0,
                                Header2 = "12",
                                WhereClause = row["WhereClause"].ToString(),
                                TimeFormatID = (int)row["RefTimeFormatID"],
                                TimePeriodInfo = row["TimePeriodInfo"].ToString(),
                                SampleTimeFormatID = (int)row["RefSampleTimeFormatID"],
                                SampleTimePeriodInfo = row["SampleTimePeriodInfo"].ToString(),
                                Unit = Unit
                            };
                            reports.Add(report);
                        }
                    }
                }
                return reports;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "GetReportDefinitions");
                return null;
            }
        }

    }
}
