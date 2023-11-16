using System.IO;
using System.Diagnostics;
using AdapterOPH;

namespace ReportMgr
{
    internal static class ImportExport
    {
        public static void ReportImport(string filename)
        {
            //DataContext context = new DataContext();
            //string js = File.ReadAllText(filename);
            //ReportDefinition rep = JsonConvert.DeserializeObject<ReportDefinition>(js);
            //rep.reportdefinitionid = 0;
            //rep.timeperiodinfo = "1";
            //rep.header2 = "2,3";
            //foreach (HistPoint item in rep.HistPoints)
            //{
            //    item.histpointid = 0;
            //}
            //context.ReportDefinitions.Add(rep);
            //context.SaveChanges();
        }

        public static void ReportExplorer(string path)
        {
            Process.Start("explorer.exe", @Path.GetDirectoryName(path));
        }
    }
}
