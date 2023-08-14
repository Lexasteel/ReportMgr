using System.Data.Entity;
using System.Data.Entity.Infrastructure.Design;
using System.Data.SQLite;

namespace AdapterOPH
{
    public class DataContext : DbContext
    {
        public DataContext() :
            base(new SQLiteConnection()
            {
                ConnectionString = new SQLiteConnectionStringBuilder() { 
                    DataSource = "rpt.db", ForeignKeys = true }.ConnectionString
            }, true)
        {
        }

        // public DbSet<EventType> EventTypes { get; set; }
        public DbSet<ReportDefinition> ReportDefinitions { get; set; }
        //public DbSet<ReportEventTask> ReportEventTasks { get; set; }
        //public DbSet<ReportEvent> ReportEvents { get; set; }

        public DbSet<HistPoint> HistPoints { get; set; }
        public DbSet<Historian> Historians { get; set; }
        // public DbSet<PointFilter> PointFilters { get; set; }
    
    }
}
