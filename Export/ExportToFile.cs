using DevExpress.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CellRange = DevExpress.Spreadsheet.CellRange;
using Worksheet = DevExpress.Spreadsheet.Worksheet;

namespace ReportMgr
{
    public class ExportToFile
    {

        public async Task ExportReadProcessedAsync(ReportDefinitions report, DateTime start, DateTime end, int _shiftday, bool single = false)
        {

            string fname = Path.GetFileNameWithoutExtension(report.DestinationInfo);
            string filename = report.DestinationInfo.Replace(fname, fname + start.ToString("_yyyyMMdd"));
            if (start.TimeOfDay.TotalSeconds > 0) filename = report.DestinationInfo.Replace(fname, fname + start.ToString("_yyyyMMdd_HHmmss"));
            int row = 0;



            Workbook workbook = new Workbook();



            Worksheet worksheet = workbook.Worksheets[0];
            workbook.Unit = DevExpress.Office.DocumentUnit.Point;
            //worksheet.DefaultColumnWidthInPixels = 50;
            //worksheet.DefaultRowHeight = 20;
            workbook.Styles[BuiltInStyleId.Normal].Font.Name = "Arial";
            workbook.Styles[BuiltInStyleId.Normal].Font.Size = 8;
            workbook.Options.Culture = new CultureInfo("en-US");
            //workbook.Styles[BuiltInStyleId.Normal].Borders.SetAllBorders(System.Drawing.Color.Black, BorderLineStyle.Thick);
            DataImportOptions dataImportOptions = new DataImportOptions()
            {
                FormulaCulture = CultureInfo.GetCultureInfo("en-US")
            };
            //if (GetBit((uint)Report.Header2, 2))//kks
            //{
            //    List<string> kks = Report.HistPoints.Select(s => s.PointName).ToList();

            //    worksheet.Import(kks, row, 1, false);
            //    row++;
            //}

            //if (GetBit((uint)Report.Header2, 1))//desc
            //{
            //    List<string> desc = Report.HistPoints.Select(s => s.Description).ToList();
            //    worksheet.Import(desc, row, 1, false, dataImportOptions);
            //    Row rowDesc = worksheet.Rows[row];
            //    rowDesc.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
            //    rowDesc.Alignment.WrapText = true;
            //    rowDesc.AutoFitRows();
            //    row++;
            //}

            //if (GetBit((uint)Report.Header2, 0))//engunits
            //{
            //    List<string> units = Report.HistPoints.Select(s => s.Units).ToList();
            //    worksheet.Import(units, row, 1, false);
            //    Row rowUnits = worksheet.Rows[row];
            //    rowUnits.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
            //    row++;
            //}

            int header = row;
            int rows = 0;

            List<DateTime> dateTimes = new List<DateTime>();
            TimeSpan span = new TimeSpan();
            switch (report.SampleTimeFormatID)
            {
                case 1:
                    span = TimeSpan.Parse(report.SampleTimePeriodInfo);
                    break;
                case 6:
                    span = new TimeSpan(0, 0, 0, 0, Convert.ToInt32(double.Parse(report.SampleTimePeriodInfo, CultureInfo.InvariantCulture) * 1000));
                    break;
                default:
                    break;
            }

            for (DateTime dateTime = start; dateTime < end; dateTime = dateTime.Add(span))
            {
                dateTimes.Add(dateTime);
                rows++;
            }

            worksheet.Import(dateTimes, header, 0, true);

            if (report.SampleTimeFormatID != 6)
            {
                worksheet.Range.FromLTRB(0, header, 0, dateTimes.Count + 1).NumberFormat = "dd.mm.yy hh:mm:ss";
            }
            else
            {
                worksheet.Range.FromLTRB(0, header, 0, dateTimes.Count + 1).NumberFormat = "hh:mm:ss.000";
            }

            //  workbook.SaveDocument(filename);

            int count = report.HistPoints.Count;
            for (int i = 0; i < report.HistPoints.Count; i++)
            {
                worksheet.Import(report.HistPoints.ElementAt(i).F_Values.Values, header, i + 1, true);


                if (worksheet.Range.FromLTRB(i + 1, header, i + 1, dateTimes.Count + 1).Sum(s => s.Value.NumericValue) == 0)
                {
                    worksheet.Range.FromLTRB(i + 1, header, i + 1, dateTimes.Count + 1).NumberFormat = "0";
                }
                else
                {
                    worksheet.Range.FromLTRB(i + 1, header, i + 1, dateTimes.Count + 1).NumberFormat = report.HistPoints.ElementAt(i).Format;
                }

                if (i % (count * 0.1) == 0)
                {
                    //ValueProgressBar++;
                    //OnJump();
                }
                report.HistPoints.ElementAt(i).F_Values = null;
                report.HistPoints.ElementAt(i).F_Values = new SortedDictionary<DateTime, float>();
            }


            //worksheet.GetUsedRange().Borders.SetAllBorders(System.Drawing.Color.Black, BorderLineStyle.Thin);
            //ValueProgressBar++;
            //OnJump();
            CellRange cells = worksheet.Range.FromLTRB(0, header, report.HistPoints.Count, dateTimes.Count + 1);
            //ValueProgressBar++;
            //OnJump();
            cells.RowHeight = 15;
            cells.ColumnWidth = 38;

            worksheet.Columns[0].AutoFit();


            //ValueProgressBar++;
            //OnJump();

            string tempPath = filename;
            switch (report.ReportDestID)
            {
                case 1: //window
                        //OutputWindow outputWindow = new OutputWindow();

                    break;
                case 3: //file
                    switch (Path.GetExtension(filename))
                    {
                        case ".xlsx":
                            await workbook.SaveDocumentAsync(filename, DocumentFormat.Xlsx);
                            break;
                        case ".csv":
                            await workbook.SaveDocumentAsync(filename + "_", DocumentFormat.Csv);

                            var list = File.ReadLines(filename + "_");
                            List<string> vs = new List<string>();
                            foreach (var item in list)
                            {
                                vs.Add(item.Replace(',', ';'));
                            }

                            File.WriteAllLines(filename, vs, Encoding.GetEncoding("Windows-1251"));
                            File.Delete(filename + "_");
                            break;
                        default:
                            break;
                    }

                    break;
                default:
                    break;
            }


        }

        public void ExportEvents()
        {
        }
        public void ExportAlarms()
        {
        }
        public void ExportRaw()
        {
            //    tempPath = Path.GetDirectoryName(Report.DestinationInfo) + @"\" + start.ToString("yyyy") + @"\" + start.ToString("MM") + @"\" + start.ToString("dd") + @"\";
            //    if (!Directory.Exists(tempPath)) Directory.CreateDirectory(tempPath);
            //    Parallel.ForEach(histPoints, item =>
            //    {
            //        string filename = tempPath + item.PointName.Split('.')[0];
            //        switch (Path.GetExtension(Report.DestinationInfo))
            //        {
            //            case ".xlsx":
            //                break;
            //            case ".csv":
            //                filename += ".csv";
            //                switch (item.Type)
            //                {

            //                    case "R":
            //                        File.WriteAllLines(filename, item.F_Values.Select(x => String.Join(";", x.Key.ToLocalTime().ToString("dd.MM.yyyy HH:mm:ss"), x.Value.ToString("F", new CultureInfo("en-US")))));
            //                        //ChangeAttrFile(filename, shiftday);
            //                        break;
            //                    case "D":
            //                        File.WriteAllLines(filename, item.F_Values.Select(x => String.Join(";", x.Key.ToLocalTime().ToString("dd.MM.yyyy HH:mm:ss"), x.Value.ToString("0", new CultureInfo("en-US")))));
            //                        //ChangeAttrFile(filename, shiftday);
            //                        break;
            //                    case "P":
            //                        File.WriteAllLines(filename, item.F_Values.Select(x => String.Join(";", x.Key.ToLocalTime().ToString("dd.MM.yyyy HH:mm:ss"), x.Value.ToString("0", new CultureInfo("en-US")))));
            //                        break;
            //                    default:
            //                        File.WriteAllLines(filename, item.F_Values.Select(x => String.Join(";", x.Key.ToLocalTime().ToString("dd.MM.yyyy HH:mm:ss"), x.Value.ToString("F", new CultureInfo("en-US")))));
            //                        //ChangeAttrFile(filename, shiftday);
            //                        break;
            //                }
            //                break;
            //            default:
            //                break;
            //        }
            //        item.F_Values = null;
            //        item.F_Values = new SortedDictionary<DateTime, float>();

            //    }
            //     );

            //}
        }
    }
}

  //  public async Task<string> SaveFile(IEnumerable<HistPoint> histPoints, HistTimeStamp start_ts, HistTimeStamp end_ts, int _shiftday, bool single = false)
  //  {
        //Task t = Task.Run(() =>
        //{


    //    if (Report.ReportTypeID == 4)//RAW
    //    {



    //    }
    //    if (Report.ReportTypeID == 1)
    //    {

    //    return tempPath;
    //    //    });

    //    //await Task.WhenAll(new[] { t });
    //}


