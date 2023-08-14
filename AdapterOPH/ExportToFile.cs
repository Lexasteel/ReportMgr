using DevExpress.Spreadsheet;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace AdapterOPH
{
    public class ExportToFile
    {
        //private DataImportOptions dataImportOptions;
        private ReportDefinition report { get; set; } = new ReportDefinition();
        public void ExportReadProcessed(ReportDefinition reportDefinition)
        {
            report = reportDefinition;
            report.State(report.Progress, LogLevel.Info, $"File creation {report.DestinationInfo}");

            var DateStart = report.Start.Value;
            var DateEnd = report.End.Value;
            string fname = Path.GetFileNameWithoutExtension(report.DestinationInfo);
            string filename = report.DestinationInfo.Replace(fname, fname + DateStart.ToString("_yyyyMMdd"));
            if (DateStart.TimeOfDay.TotalSeconds > 0) filename = report.DestinationInfo.Replace(fname, fname + DateStart.ToString("_yyyyMMdd_HHmmss"));

            Workbook workbook = new Workbook();


            workbook.Unit = DevExpress.Office.DocumentUnit.Point;
            workbook.Styles[BuiltInStyleId.Normal].Font.Name = "Calibri";
            workbook.Styles[BuiltInStyleId.Normal].Font.Size = 10;
            workbook.Options.Culture = new CultureInfo("en-US");
            //dataImportOptions = new DataImportOptions()
            //{
            //    FormulaCulture = CultureInfo.GetCultureInfo("en-US")
            //};
            string[] headers = report.Header2.Split(',');

            if (report.MultiSheets)
            {
                var groups = report.HistPoints.GroupBy(g => g.PointName.Split('.')[1]);
                foreach (var group in groups)
                {
                    string SheetName = group.First().PointName.Split('.')[1];
                    Worksheet worksheet = workbook.Worksheets.Add(SheetName);
                    SheetShaping(group.ToList(), worksheet);
                }
                workbook.Worksheets.RemoveAt(0);
                using (DataContext db = new DataContext())
                {
                    foreach (var sh in workbook.Worksheets)
                    {
                        string oldName = sh.Name;
                        Historian historian = db.Historians.FirstOrDefault(f => f.UnitNet == oldName);
                        string sheetName = "Unit" + historian.Unit;
                        workbook.Worksheets[oldName].Name = sheetName;
                    }
                }
            }

            else
            {
                Worksheet worksheet = workbook.Worksheets[0];
                SheetShaping(report.HistPoints, worksheet);
            }


            workbook.Worksheets.ActiveWorksheet = workbook.Worksheets[0];
            report.State(99, LogLevel.Info, $"Saving the file: {filename}");
            string tempPath = filename;
            //bool locked = false;
            switch (report.ReportDestID)
            {
                case 1: //window
                        //OutputWindow outputWindow = new OutputWindow();

                    break;
                case 3: //file
                    switch (Path.GetExtension(filename))
                    {
                        case ".xlsx":
                            workbook.SaveDocument(filename, DocumentFormat.Xlsx);
                            break;
                        case ".xlsb":
                            //while (locked)
                            //{
                            //    locked = IsFileLocked(new FileInfo(filename));
                                
                                workbook.SaveDocument(filename, DocumentFormat.Xlsb);
                            //}
                            break;
                        case ".csv":
                            workbook.SaveDocument(filename + "_", DocumentFormat.Csv);

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
            report.State(100, LogLevel.Info, $"File saved: {filename}");
        }

        public void SheetShaping(List<HistPoint> pointsList, Worksheet worksheet)
        {
            try
            {
            var OrderedPoints = pointsList.OrderBy(o => o.PointPosn).ToList();
            var count = OrderedPoints.Count;
            //int row = 0;
            string[] headers = report.Header2.Split(',');
            
            var dateTimes = OrderedPoints.First().F_Values.Keys.Select(s => s.ToLocalTime());

            int dateCount = dateTimes.Count();
            worksheet.Import(dateTimes, headers.Length, 0, true);

            if (report.SampleTimeFormatID != 6)
            {
                worksheet.Range.FromLTRB(0, headers.Length, 0, dateCount + 1).NumberFormat = "dd.mm.yy hh:mm:ss";
            }
            else
            {
                worksheet.Range.FromLTRB(0, headers.Length, 0, dateCount + 1).NumberFormat = "hh:mm:ss.000";
            }
            int col = 1;
            foreach (var point in OrderedPoints)
            {
                int row = 0;
                foreach (string h in headers)
                {
                    switch (h.Trim())
                    {
                        case "1"://kks
                            SetCellValue(worksheet.Cells[row, col], point.PointName);
                            row++;
                            break;
                        case "2"://desc
                            SetCellValue(worksheet.Cells[row, col], point.Description);
                            row++;
                            break;
                        case "3"://eng units
                            SetCellValue(worksheet.Cells[row, col], point.Units);
                            row++;
                            break;
                    }
                }
                worksheet.Import(point.F_Values.Values, row, col, true);
                
                if (worksheet.Range.FromLTRB(col, row, col, dateCount + 1).Sum(s => s.Value.NumericValue) == 0)
                {
                    worksheet.Range.FromLTRB(col, row, col, dateCount + 1).NumberFormat = "0";
                }
                else
                {
                    worksheet.Range.FromLTRB(col, row, col, dateCount + 1).NumberFormat = point.Format;
                }
                CellRange cells = worksheet.Range.FromLTRB(1, row, count, dateCount + 1);
                cells.RowHeight = 14.4;
                cells.ColumnWidth = 38;
                var percents= col * 25 / count+75;
                if (percents > 99) percents = 99;
                report.State(percents,LogLevel.Off, $"File creation {report.DestinationInfo}");
                col++;
            }
            worksheet.Columns[0].AutoFit();

            }
            catch (Exception e)
            {
                report.State(report.Progress,LogLevel.Error, $"SheetShaping: {e.Message}");    
            }

        }

        private void SetCellValue(Cell cell, string value)
        {
            cell.SetValueFromText(value);
            cell.Alignment.Horizontal = SpreadsheetHorizontalAlignment.Center;
            cell.Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
            cell.Alignment.WrapText = true;
            cell.Font.Name = "Calibri";
            cell.Font.Size = 10;
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
        protected virtual bool IsFileLocked(FileInfo file)
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }

    }
}


