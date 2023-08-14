using AdapterOPH;
using DevExpress.Office.Utils;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using Newtonsoft.Json;
using NLog;
using ReportMgr.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
//using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReportMgr
{
    public partial class FormMain : RibbonForm
    {
        public FormMain()
        {
            InitializeComponent();

            EditColor = Color.Tomato;
            gridConrolMain.ProcessGridKey += gridControl_ProcessGridKey;
            gViewMain.RowStyle += GwMain_RowStyle;

            this.FormClosing += mainFrm_FormClosing;
            bbNew.ItemClick += BbNew_ItemClick;
            bbEdit.ItemClick += GwMain_DoubleClick;
            gViewMain.DoubleClick += GwMain_DoubleClick;
            bbGen.ItemClick += OnGenerateRowClick;
            bbStartTimer.ItemClick += BbStartTimer_ItemClick;
            bbCopy.ItemClick += OnCopyRowClick;
            bbDelete.ItemClick += OnDeleteRowClick;
            bbStop.ItemClick += BbStop_ItemClick;
            gViewMain.RowClick += GwMain_RowClick;

            bbImport.ItemClick += BbImport_ItemClick;
            bbExport.ItemClick += BbExport_ItemClick;
            bbRefresh.ItemClick += BbRefresh_ItemClick;
            bbCompactDB.ItemClick += BbCompactDB_ItemClick;
            btnAddPoint.Click += BtnAddPoint_Click;
            btnRemovePoint.Click += BtnRemovePoint_Click;
            btnUpPoint.Click += BtnUpPoint_Click;
            btnDownPoint.Click += BtnDownPoint_Click;
            btnImportPoints.Click += BtnImportPoints_Click;
            btnExportPoints.Click += BtnExportPoints_Click;
            beditDestinationInfo.ButtonClick += BeditDestinationInfo_ButtonClick;
            //System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
            //System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru-RU");


            _currentSkin = DevExpress.Skins.CommonSkins.GetSkin(LookAndFeel.ActiveLookAndFeel);
            _skinElementNameDisable = DevExpress.Skins.CommonColors.DisabledControl;
            _disabledColor = _currentSkin.Colors[_skinElementNameDisable];

            gridViewLog.CustomDrawCell += (s, e) =>
            {
                //e.Appearance.TextOptions.HAlignment = HorzAlignment.Center;
                //e.Appearance.Options.UseTextOptions = true;
                //e.DefaultDraw();
                switch (gridViewLog.GetRowCellValue(e.RowHandle, "ReportStatusID"))
                {
                    case 0:
                        e.Appearance.BackColor = Color.DarkSeaGreen;
                        break;
                    case 1:
                        e.Appearance.BackColor = Color.LightPink;
                        break;
                    case 2:
                        e.Appearance.BackColor = Color.Silver;
                        break;
                }
            };

            gViewDetail.InitNewRow += (s, e) =>
            {
                GridView view = s as GridView;
                view.SetRowCellValue(view.FocusedRowHandle, view.Columns["Format"], "0.00");
                view.SetRowCellValue(view.FocusedRowHandle, view.Columns["IntegConst"], "1.0");
                view.SetRowCellValue(view.FocusedRowHandle, view.Columns["ReportDefinitionID"], report.ReportDefinitionID);
                if (report.HistPoints == null) report.HistPoints = new List<HistPoint>();
                if (report.HistPoints.Count == 0)
                {
                    view.SetRowCellValue(view.FocusedRowHandle, view.Columns["PointPosn"], 1);
                }
                else
                    view.SetRowCellValue(view.FocusedRowHandle, view.Columns["PointPosn"], report.HistPoints.Max(m => m.PointPosn) + 1);
            };
            chCmbHeader.Properties.Items.Add(1, "KKS");
            chCmbHeader.Properties.Items.Add(2, "Description");
            chCmbHeader.Properties.Items.Add(3, "Eng.Units");



        }
        string pathImport = Application.StartupPath + @"\Import\";
        string pathExport = Application.StartupPath + @"\Export\";
        private void BtnExportPoints_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            foreach (var p in report.HistPoints)
            {
                list.Add(String.Join(";",
                    p.PointName,
                    p.BitNumber,
                    p.ProcType,
                    p.IntegConst,
                    p.GlitchDetect,
                    p.SummaryEnable,
                    p.Description,
                    p.Format));
            }
            if (!Directory.Exists(pathExport)) Directory.CreateDirectory(pathExport);
            string filename = pathExport + $"Points of {report.ReportName}.txt";
            File.WriteAllLines(filename, list);
            barStaticItemInfo.Caption = $"Exported {filename}";
        }

        private void BtnImportPoints_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            var pointposn = 1;
            if (report.HistPoints.Count>0)
            {
                pointposn = report.HistPoints.Max(m => m.PointPosn);
            }
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var file = File.ReadAllLines(dialog.FileName);
                foreach (var row in file)
                {
                    var s = row.Split(';');
                    HistPoint p = new HistPoint();
                    p.PointName = s[0];
                    p.BitNumber = int.Parse(s[1]);
                    p.ProcType = int.Parse(s[2]);
                    p.IntegConst = s[3];
                    p.GlitchDetect = int.Parse(s[4]);
                    p.SummaryEnable = int.Parse(s[5]);
                    p.Description = s[6];
                    p.Format = s[7];
                    p.ReportDefinitionID = report.ReportDefinitionID;
                    p.PointPosn = pointposn+=1;
                    db.HistPoints.Add(p);
                }
               
            }
            db.SaveChanges();
            gViewDetail.RefreshData();
            barStaticItemInfo.Caption = $"Imported {dialog.FileName}";
        }

        private void BbRefresh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gViewMain.RefreshData();
            gViewDetail.RefreshData();
        }

        private void BbCompactDB_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {

                using (DataContext context = new DataContext())
                {
                    var reports = context.HistPoints.GroupBy(g => g.ReportDefinitionID);
                    var count = reports.Count();
                    log.Info($"Reports: {count}.");
                    if (count == 0) return;
                    var points_count = 0;
                    int s = 0;
                    foreach (var item in reports)
                    {
                        //log.Info($"{item.Key}: {item.Count()}");
                        int reportId = item.Key;
                        if (context.ReportDefinitions.FirstOrDefault(f => f.ReportDefinitionID == reportId) is null)
                        {
                            context.HistPoints.RemoveRange(item);
                            points_count += item.Count();
                        }
                        this.Invoke(new Action(() => barPb.EditValue = (s++ / count * 100)));
                    }
                    context.SaveChanges();
                    using (SQLiteConnection sqlite_conn = new SQLiteConnection("Data Source=Rpt.db;Version=3;New=true;Compress=true;"))
                    {
                        sqlite_conn.Open();
                        using (SQLiteCommand command = sqlite_conn.CreateCommand())
                        {
                            command.CommandText = "vacuum;";
                            command.ExecuteNonQuery();
                        }
                        sqlite_conn.Close();
                    }
                    barStaticItemInfo.Caption = $"Deleted {points_count} points.";
                    log.Info($"Deleted {points_count} points.");

                }

            }
            catch (Exception ex)
            {

                log.Error($"CompactDB:{ex.Message}");
            }
        }


        // ReportDefinition _reportGenerating = new ReportDefinition();
        int CycleSet = 5;
        int CycleTimer = 0;
        static bool Busy = false;

        DataContext db = new DataContext();
        Stack<ReportDefinition> _reports = new Stack<ReportDefinition>();
        string _fileName = Application.StartupPath + "\\Grid_Layout.xml";
        ReportDefinition report = new ReportDefinition();
        //ReportDefinition BuildReport;

        string _skinElementNameDisable;
        DevExpress.Skins.Skin _currentSkin;
        Color _disabledColor;
        public Color EditColor { get; private set; }
        Logger log = LogManager.GetCurrentClassLogger();
        List<int> RowIndexes = new List<int>();
        String StateReport = String.Empty;

        BindingSource bindingSourceMain = new BindingSource();
        BindingSource bindingSourceDetail = new BindingSource();

        static CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
        CancellationToken token = cancelTokenSource.Token;

        private void BbExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var js = JsonConvert.SerializeObject(report, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.FileName = Path.ChangeExtension(report.DestinationInfo, "txt");
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(dialog.FileName, js);
            }
        }

        private void BtnDownPoint_Click(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = gViewDetail;
            view.GridControl.Focus();
            int index = view.FocusedRowHandle;
            if (index >= view.DataRowCount - 1) return;

            HistPoint row1 = (HistPoint)view.GetRow(index);
            HistPoint row2 = (HistPoint)view.GetRow(index + 1);
            int val1 = row1.PointPosn;
            int val2 = row2.PointPosn;
            row1.PointPosn = val2;
            row2.PointPosn = val1;
            //  bsHispPoints.MoveNext();
            view.RefreshData();
            view.FocusedRowHandle = index + 1;
        }

        private void BtnUpPoint_Click(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = gViewDetail;
            view.GridControl.Focus();
            int index = view.FocusedRowHandle;
            if (index <= 0) return;

            HistPoint row1 = (HistPoint)view.GetRow(index);
            HistPoint row2 = (HistPoint)view.GetRow(index - 1);
            int val1 = row1.PointPosn;
            int val2 = row2.PointPosn;
            row1.PointPosn = val2;
            row2.PointPosn = val1;
            //  bsHispPoints.MovePrevious();
            view.RefreshData();
            view.FocusedRowHandle = index - 1;
        }

        private void BtnRemovePoint_Click(object sender, EventArgs e)
        {
            var h = (HistPoint)bindingSourceDetail.Current;
            gViewDetail.DeleteRow(gViewDetail.FocusedRowHandle);
            db.HistPoints.Remove(h);
        }

        private void BtnAddPoint_Click(object sender, EventArgs e)
        {
            gViewDetail.AddNewRow();
            //gwMain.FocusedRowHandle = gwMain.RowCount - 1;

            //DevExpress.XtraGrid.Views.Grid.GridView view = sender as DevExpress.XtraGrid.Views.Grid.GridView;
            //view.SetRowCellValue(gwMain.FocusedRowHandle, view.Columns["Format"], "0.00");
            //view.SetRowCellValue(gwMain.FocusedRowHandle, view.Columns["IntegConst"], "1.0");
            //view.SetRowCellValue(gwMain.FocusedRowHandle, view.Columns["ReportDefinitionID"], report.ReportDefinitionID);
            //view.SetRowCellValue(gwMain.FocusedRowHandle, view.Columns["PointPosn"], 10 + 1);
            // gwMain.ShowEditForm();
            // gwMain.ShowPopupEditForm();
        }

        private void GwMain_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView view = sender as GridView;
            var r = view.GetDataSourceRowIndex(e.RowHandle);

            if (RowIndexes.Contains(r))
            {
                e.Appearance.BackColor = EditColor;
            }
            if ((view.FocusedRowHandle == e.RowHandle) && (RowIndexes.Contains(r)))
            {
                view.Appearance.FocusedRow.BackColor = EditColor;
            }
            else
            {
                view.Appearance.FocusedRow.BackColor = new Color();
            }
        }

        private void GwMain_DoubleClick(object sender, EventArgs e)
        {
            GridView view = sender as GridView;
            var i = view.GetFocusedDataSourceRowIndex();
            if (!RowIndexes.Contains(i))
            {
                RowIndexes.Add(i);
                view.Appearance.FocusedRow.BackColor = EditColor;
            }
            else
            {
                RowIndexes.Remove(i);
                view.Appearance.FocusedRow.BackColor = Color.Aquamarine;
            }
            StateSwitch();

        }

        private void GwMain_RowClick(object sender, RowClickEventArgs e)
        {
            if (e.RowHandle < 0) return;
            int id = 0;
            var RowText = gViewMain.GetRowCellDisplayText(e.RowHandle, "ReportDefinitionID");
            id = int.Parse(RowText);
            //report = _db.ReportDefinitions.Include("HistPoints").FirstOrDefault(f => f.ReportDefinitionID == id);
            report = (ReportDefinition)bindingSourceMain.Current;
            Select_Detail(false);

        }

        #region Detail
        private void Select_Detail(bool State = false)
        {

            //if (id == 0)
            //{
            //    var report = new ReportDefinition();
            //    _db.ReportDefinitions.Add(report);
            //    State = true;
            //}
            //else
            //{
            //    
            //}
            if (State)
            {
                StateSwitch();
            }


            txtTitle.DataBindings.Clear();
            txtTitle.DataBindings.Add("EditValue", report, "ReportName", true, DataSourceUpdateMode.OnPropertyChanged);

            lkpReportType.DataBindings.Clear();
            lkpReportType.Properties.DataSource = ReportDefinition.ReportType;
            lkpReportType.Properties.DropDownRows = ReportDefinition.ReportType.Count;
            lkpReportType.DataBindings.Add("EditValue", report, "ReportTypeID", true);

            lkpReportDest.DataBindings.Clear();
            lkpReportDest.Properties.DataSource = ReportDefinition.DestinationType;
            lkpReportDest.Properties.DropDownRows = ReportDefinition.DestinationType.Count;
            lkpReportDest.DataBindings.Add("EditValue", report, "ReportDestID", true);

            chCmbHeader.DataBindings.Clear();
            chCmbHeader.DataBindings.Add("EditValue", report, "Header2");
            chCmbHeader.SetEditValue(report.Header2);


            switch (report.SampleTimeFormatID)
            {
                case 1:
                    spinSampleHours.EditValue = report.SampleTimePeriodInfo.Split(':')[0];
                    spinSampleMins.EditValue = report.SampleTimePeriodInfo.Split(':')[1];
                    spinSampleSecs.EditValue = report.SampleTimePeriodInfo.Split(':')[2];
                    break;
                case 6:
                    cmbFractionOfSec.EditValue = report.SampleTimePeriodInfo;
                    break;
                default:
                    spinSampleHours.EditValue = report.SampleTimePeriodInfo.Split(':')[0];
                    break;
            }

            switch (report.TimeFormatID)
            {
                case 1:
                    spinTimeHours.EditValue = report.TimePeriodInfo.Split(':')[0];
                    spinTimeMinutes.EditValue = report.TimePeriodInfo.Split(':')[1];
                    spinTimeSecs.EditValue = report.TimePeriodInfo.Split(':')[2];
                    break;
                default:
                    spinTimeHours.EditValue = report.TimePeriodInfo.Split(':')[0];
                    break;
            }
            lookUpTimeFormat.EditValueChanged += LookUpTimeFormat_EditValueChanged;
            lookUpSample.EditValueChanged += LookUpSample_EditValueChanged;


            Dictionary<int, string> timeformat = ReportDefinition.TimeType.Where(w => w.Key < 5).ToDictionary(f => f.Key, v => v.Value);
            lookUpTimeFormat.DataBindings.Clear();
            lookUpTimeFormat.Properties.DataSource = timeformat;
            lookUpTimeFormat.Properties.DropDownRows = timeformat.Count;
            lookUpTimeFormat.DataBindings.Add("EditValue", report, "TimeFormatID", true);

            lookUpSample.DataBindings.Clear();
            lookUpSample.Properties.DataSource = ReportDefinition.TimeType;
            lookUpSample.Properties.DropDownRows = ReportDefinition.TimeType.Count;
            lookUpSample.DataBindings.Add("EditValue", report, "SampleTimeFormatID", true);

            beditDestinationInfo.DataBindings.Clear();
            beditDestinationInfo.DataBindings.Add("EditValue", report, "DestinationInfo", true);

            deNextEvent.DataBindings.Clear();
            deNextEvent.DataBindings.Add("EditValue", report, "NextEvent", true);

            chkEnable.DataBindings.Clear();
            chkEnable.DataBindings.Add("EditValue", report, "Enable", true);

            tsOffSet.DataBindings.Clear();
            tsOffSet.DataBindings.Add("EditValue", report, "OffSet", true);

            chZip.DataBindings.Clear();
            chZip.DataBindings.Add("EditValue", report, "Arhive", true);




            gridControlDetail.DataSource = bindingSourceDetail;

        }
        private void StateSwitch()
        {


            grpGeneral.Enabled = !grpGeneral.Enabled;
            grpPeriod.Enabled = grpGeneral.Enabled;
            grpInterval.Enabled = grpGeneral.Enabled;
            //layControlGrpScheduler.Enabled = !layControlGrpScheduler.Enabled;
            gViewDetail.OptionsBehavior.Editable = !gViewDetail.OptionsBehavior.Editable;
            btnSave.Enabled = !btnSave.Enabled;
            btnCancel.Enabled = !btnCancel.Enabled;
            bbEdit.Enabled = !bbEdit.Enabled;
            btnGetDesc.Enabled = !btnGetDesc.Enabled;
            btnAddPoint.Enabled = !btnAddPoint.Enabled;
            btnRemovePoint.Enabled = !btnRemovePoint.Enabled;
            btnUpPoint.Enabled = !btnUpPoint.Enabled;
            btnDownPoint.Enabled = !btnDownPoint.Enabled;
            btnImportPoints.Enabled = !btnImportPoints.Enabled;
            btnExportPoints.Enabled = !btnExportPoints.Enabled;

        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (db.ChangeTracker.HasChanges())
            {
                DialogResult = MessageBox.Show("Save changes?", "Report Manager", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (DialogResult == DialogResult.Yes)
                {
                    bindingSourceDetail.EndEdit();
                    db.SaveChanges();
                }
                if (DialogResult == DialogResult.No)
                {
                    Select_Detail(false);
                    db.Entry(report).State = EntityState.Unchanged;
                    db.Entry(report).Reload();
                    txtTitle.DataBindings[0].ReadValue();
                    lkpReportDest.DataBindings[0].ReadValue();
                    lkpReportType.DataBindings[0].ReadValue();
                    lookUpSample.DataBindings[0].ReadValue();
                    lookUpTimeFormat.DataBindings[0].ReadValue();
                    beditDestinationInfo.DataBindings[0].ReadValue();
                    deNextEvent.DataBindings[0].ReadValue();
                    chkEnable.DataBindings[0].ReadValue();
                    tsOffSet.DataBindings[0].ReadValue();
                    chZip.DataBindings[0].ReadValue();
                    chCmbHeader.SetEditValue(report.Header2);
                }
            }
            StateSwitch();
            var r = gViewMain.GetFocusedDataSourceRowIndex();
            RowIndexes.Remove(r);

        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            switch (report.TimeFormatID)
            {
                case 1:
                    string[] info = { spinTimeHours.EditValue.ToString(), spinTimeMinutes.EditValue.ToString(), spinTimeSecs.EditValue.ToString() };
                    report.TimePeriodInfo = String.Join(":", info);
                    break;
                default:
                    report.TimePeriodInfo = spinTimeHours.EditValue.ToString();
                    break;
            }

            switch (report.SampleTimeFormatID)
            {
                case 1:
                    string[] info = { spinSampleHours.EditValue.ToString(), spinSampleMins.EditValue.ToString(), spinSampleSecs.EditValue.ToString() };
                    report.SampleTimePeriodInfo = String.Join(":", info);
                    break;
                case 6:
                    report.SampleTimePeriodInfo = cmbFractionOfSec.EditValue.ToString();
                    break;
                default:
                    report.SampleTimePeriodInfo = spinSampleHours.EditValue.ToString();
                    break;
            }
            var f = db.HistPoints.ToList();

            bindingSourceDetail.EndEdit();
            bindingSourceMain.EndEdit();

            db.SaveChanges();
            StateSwitch();
            //
            var r = gViewMain.GetFocusedDataSourceRowIndex();
            RowIndexes.Remove(r);
        }
        private void btnGetDesc_Click(object sender, EventArgs e)
        {

            try
            {

                ReportDefinition report = (ReportDefinition)bindingSourceMain.Current;
                DataClient dataClient = new DataClient(report);
                report.ReportChanged += BuildReport_ReportChanged;
                dataClient.GetDescriptions();
                gViewDetail.RefreshData();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }



        private void BeditDestinationInfo_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            string file = beditDestinationInfo.EditValue.ToString();
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = @"Excel Workbook(*.xlsx)|*.xlsx|Excel binary Workbook(*.xlsb)|*.xlsb|CSV files(*.csv)|*.csv|All files(*.*)|*.*",
                // RestoreDirectory=true
            };

            if (!String.IsNullOrEmpty(file))
            {
                saveFileDialog.FileName = Path.GetFileName(file);
                saveFileDialog.InitialDirectory = Path.GetDirectoryName(file);
            }
            switch (Path.GetExtension(file))
            {
                case ".xlsx":
                    saveFileDialog.FilterIndex = 1;
                    break;
                case ".xlsb":
                    saveFileDialog.FilterIndex = 2;
                    break;
                case ".csv":
                    saveFileDialog.FilterIndex = 3;
                    break;
                default:
                    saveFileDialog.FilterIndex = 4;
                    break;
            }


            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                beditDestinationInfo.Text = saveFileDialog.FileName;
            }
        }
        private void LookUpTimeFormat_EditValueChanged(object sender, EventArgs e)
        {
            int timeType = Convert.ToInt32(((LookUpEdit)sender).EditValue);

            switch (timeType)
            {
                case 1:
                    spinTimeMinutes.Visible = true;
                    spinTimeSecs.Visible = true;
                    break;
                default:
                    spinTimeMinutes.Visible = false;
                    spinTimeSecs.Visible = false;

                    break;
            }



        }
        private void LookUpSample_EditValueChanged(object sender, EventArgs e)
        {
            int timeType = Convert.ToInt32(((LookUpEdit)sender).EditValue);
            var arr = VisibilitySampleIntervals(timeType);
            spinSampleHours.Visible = arr[0];
            spinSampleMins.Visible = arr[1];
            spinSampleSecs.Visible = arr[2];
            cmbFractionOfSec.Visible = arr[3];

            switch (timeType)
            {
                case 6:
                    cmbFractionOfSec.SelectedIndex = 0;
                    break;
                default:
                    // spinSampleHours.EditValue = 0;
                    break;
            }
        }
        public static bool[] VisibilitySampleIntervals(int timeType)
        {
            bool showFracOfSec = true;
            bool showHours = true;
            bool showMins = true;
            bool showSecs = true;
            switch (timeType)
            {
                case 1:
                    showFracOfSec = false;
                    break;
                case 6:
                    showHours = false;
                    showMins = false;
                    showSecs = false;
                    break;
                default:
                    showMins = false;
                    showSecs = false;
                    showFracOfSec = false;
                    break;
            }
            bool[] array = new bool[] { showHours, showMins, showSecs, showFracOfSec };
            return array;
        }
        #endregion

        #region Timer

        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Busy) return;
            if (CycleSet >= CycleTimer)
            {
                barStaticItemTime.Caption = $"Цикл: {(CycleSet - CycleTimer)} сек";
                CycleTimer++;
            }
            else
            {
                using (DataContext context = new DataContext())
                {
                    var now = DateTime.Now;
                    var list = context.ReportDefinitions.Where(w => w.Enable.Equals(true) && w.NextEvent.Value < now).OrderBy(o=>o.ReportName).Include("HistPoints");
                    foreach (ReportDefinition item in list)
                    {
                        _reports.Push(item);
                    }
                    CycleTimer = 0;
                    list = null;
                }
            }
            if (_reports.Count > 0)
            {
                Busy = true;
                Task.Run(() =>
                {
                    Monitoring();
                    Busy = false;
                }, token);

            }

        }
        void Monitoring()
        {
            while (_reports.Count > 0)
            {
                using (var BuildReport = _reports.Pop())
                {

                    BuildReport.ReportChanged += BuildReport_ReportChanged;

                    if (BuildReport.End is null)
                    {
                        BuildReport.End = BuildReport.NextEvent.Value;
                        BuildReport.Start = BuildReport.DateCalc(BuildReport.NextEvent.Value, true);
                    }
                    using (DataClient dataClient = new DataClient(BuildReport))
                    {
                        bool res = dataClient.GenerateReport().Result;
                        if (res)
                        {

                            DateTime NextEvent = BuildReport.DateCalc(BuildReport.NextEvent.Value);
                            int id = BuildReport.ReportDefinitionID;
                            var r = db.ReportDefinitions.FirstOrDefault(f => f.ReportDefinitionID == id);
                            r.NextEvent = NextEvent;
                            var now = DateTime.Now;
                            now = now.AddMilliseconds(-now.Millisecond);
                            r.LastUsed = now;
                            db.SaveChanges();
                            this.Invoke(new Action(() => gViewMain.RefreshData()));
                        }
                        else
                        {
                            //BuildReport.State(99, ex.Message);
                            //BuildReport.ErrorStr = ex.Message;
                            //if (ex.HResult == -2146233029)
                            //    BuildReport.ReportStatusID = 2;
                            //else
                            //    BuildReport.ReportStatusID = 1;
                        }
                    }
                    //finally
                    //{
                    //ReportLog reportLog = new ReportLog();
                    //reportLog.ReportDefinitionID = _reportGenerating.ReportDefinitionID;
                    //reportLog.ReportName = _reportGenerating.ReportName;
                    //reportLog.CreateDate = _reportGenerating.CreateDate;
                    //reportLog.ExecuteDate = DateTime.Now;
                    //reportLog.ReportDestID = _reportGenerating.ReportDestID;
                    //reportLog.DestinationInfo = _reportGenerating.DestinationInfo;
                    //reportLog.StartDate = _reportGenerating.Start;
                    //reportLog.EndDate = _reportGenerating.End;
                    //reportLog.ReportStatusID = _reportGenerating.ReportStatusID;
                    //reportLog.ErrorStr = _reportGenerating.ErrorStr;
                    //if (_reportGenerating.Unit > 0)
                    //    reportLog.HistorianServer = _db.Historians.FirstOrDefault(f => f.Unit == _reportGenerating.Unit).IP;

                    //}
                }
            }

        }
        private void BuildReport_ReportChanged(ReportDefinition sender, ReportEventArgs e)
        {
            if (e.Value > 100)
                repositoryItemProgressBar1.Maximum = e.Value;
            barPb.EditValue = e.Value;
            barStaticItemInfo.Caption = $"{sender.ReportName} c {sender.Start.ToString()} по {sender.End.ToString()}: {e.Message}";

        }

        #region Форма
        private void Form1_Shown(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {

            gridConrolMain.ForceInitialize();
            // Restore the previously saved layout 

            barStaticItemInfo.Caption = "";
            barStaticItemTime.Caption = "";
            InitgViewMain();
            InitgViewDetail();

            await LoadFromDb();
            gridConrolMain.DataSource = bindingSourceMain;
            //InitLog();
            gViewMain.PopupMenuShowing += gridViewMain_PopupMenuShowing;
            CycleSet = (int)Settings.Default["Period"];
            barPeriod.EditValue = CycleSet;
            bbStartTimer.Enabled = false;
            gridConrolMain.ForceInitialize();
            //rpgSkins.Text = "Оформление";
            rpMain.Text = "Главная";
            //if (File.Exists(fileName))
            //    gControl.MainView.RestoreLayoutFromXml(fileName, GetLayoutSettings());
            gViewMain.Columns["LastUsed"].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
            gViewMain.FocusedRowHandle = 0;
            //List<Header> headers = new List<Header>();
            //headers.Add(new Header() { Id = 1, Value = "KKS" });
            //headers.Add(new Header() { Id = 2, Value = "Description" });
            //headers.Add(new Header() { Id = 3, Value = "Eng.Units" });


        }
        private async Task LoadFromDb()
        {
            await db.ReportDefinitions.LoadAsync().ContinueWith(loadTask =>
            {
                bindingSourceMain.DataSource = db.ReportDefinitions.Local.ToBindingList();
                bindingSourceDetail.DataSource = bindingSourceMain;
                bindingSourceDetail.DataMember = "HistPoints";
            }, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());


        }
        //private class Header
        //{
        //    public int Id { get; set; }
        //    public string Value { get; set; }
        //}
        private void mainFrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialog = XtraMessageBox.Show("Желаете свернуть программу?", "Менеджер отчетов", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            switch (dialog)
            {
                case DialogResult.Yes:
                    this.WindowState = FormWindowState.Minimized;
                    e.Cancel = true;
                    return;

                case DialogResult.No:
                    this.Dispose();
                    break;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    return;
                default:
                    return;

            }

            notifyIcon1.Dispose();
        }

        #endregion

        #region Контексное меню
        private void gridViewMain_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            GridView view = sender as GridView;
            if (e.MenuType == GridMenuType.Row)
            {
                int rowHandle = e.HitInfo.RowHandle;
                e.Menu.Items.Clear();
                e.Menu.Items.Add(CreateSubMenuRows("Генерировать отчет", rowHandle, OnGenerateRowClick));
                e.Menu.Items.Add(CreateSubMenuRows("Открыть в проводнике ...", rowHandle, OnExplorerRowClick));
                e.Menu.Items.Add(CreateSubMenuRows("Копировать отчет", rowHandle, OnCopyRowClick));
                e.Menu.Items.Add(CreateSubMenuRows("Удалить отчет", rowHandle, OnDeleteRowClick));
                //e.Menu.Items.Add(CreateSubMenuRows("Test", rowHandle, OnTestRowClick));

            }
        }
        private DXMenuItem CreateSubMenuRows(string caption, int rowHandle, EventHandler eventHandler)
        {
            DXMenuItem menuItem = new DXMenuItem(caption, eventHandler);
            menuItem.Tag = rowHandle;
            return menuItem;
        }

        void OnExplorerRowClick(object sender, EventArgs e)
        {
            var path = bindingSourceMain.Current as ReportDefinition;
            ImportExport.ReportExplorer(path.DestinationInfo);
        }

        void OnCopyRowClick(object sender, EventArgs e)
        {
            using (DataContext context = new DataContext())
            {
                report = (ReportDefinition)bindingSourceMain.Current;
                var rd = (ReportDefinition)bindingSourceMain.Current;
                int id = rd.ReportDefinitionID;
                var originalEntity = context.ReportDefinitions.Include("HistPoints").FirstOrDefault(f => f.ReportDefinitionID == id);
                var newentity = new ReportDefinition();
                //var histPoints_originalEntity = originalEntity.HistPoints;
                context.ReportDefinitions.Add(newentity);  //Create and add new entity object to context before setting its values
                var originalEntityValues = context.Entry(originalEntity).CurrentValues;
                context.Entry(newentity).CurrentValues.SetValues(originalEntityValues);  //Copy values from original entity to new entity
                newentity.ReportName += " - Копия";
                context.SaveChanges();
                var points = context.HistPoints.Where(w => w.ReportDefinitionID == originalEntity.ReportDefinitionID);
                foreach (var item in points)
                {
                    var originalItemValues = context.Entry(item).CurrentValues;
                    HistPoint point = new HistPoint();
                    context.HistPoints.Add(point);
                    context.Entry(point).CurrentValues.SetValues(originalItemValues);
                    point.ReportDefinitionID = newentity.ReportDefinitionID;
                }
                //_db.HistPoints.AddRange(histPoints);
                context.SaveChanges();
            }
            gViewMain.RefreshData();
        }

        void OnGenerateRowClick(object sender, EventArgs e)
        {
            DataContext context = new DataContext();
            formInputDates frmInput = new formInputDates();
            int id = (int)gViewMain.GetFocusedRowCellValue("ReportDefinitionID");
            ReportDefinition definition = (ReportDefinition)bindingSourceMain.Current;
            DateTime date = DateTime.Now;
            frmInput.DateEnd = new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0);
            frmInput.DateStart = frmInput.DateEnd.Subtract(TimeSpan.Parse(definition.TimePeriodInfo));
            if (frmInput.ShowDialog() == DialogResult.OK)
            {
                definition.Start = frmInput.DateStart;
                definition.End = frmInput.DateEnd;
                definition.MultiSheets = frmInput.SingleFile;
            }
            frmInput.Dispose();
            definition.ReportChanged += BuildReport_ReportChanged;
            var now = DateTime.Now;
            now = now.AddMilliseconds(-now.Millisecond);
            definition.LastUsed = DateTime.Now;
            Busy = true;
            Task.Run(() =>
            {
                using (DataClient dataClient = new DataClient(definition))
                {
                    bool res = dataClient.GenerateReport().Result;
                    if (res)
                    {
                        bindingSourceMain.EndEdit();
                        context.SaveChanges();
                        this.Invoke(new Action(() => gViewMain.RefreshData()));
                    }
                }
                Busy = false;
            }, token);


        }
        void OnDeleteRowClick(object sender, EventArgs e)
        {

            var handles = gViewMain.GetSelectedRows();
            var text = "Удаляем: ";
            foreach (var item in handles)
            {
                text += gViewMain.GetRowCellValue(item, "ReportName");
                if (item != handles.Last())
                {
                    text += ", ";
                }
            }
            text += "?";
            DialogResult dialogResult = XtraMessageBox.Show(text, "Подтверждение удаления", MessageBoxButtons.YesNo);
            List<HistPoint> points = new List<HistPoint>();
            if (dialogResult != DialogResult.Yes)
            {
                return;
            }
            else
            {
                var rows = gViewMain.GetSelectedRows();
                using (DataContext context = new DataContext())
                {
                    foreach (var item in rows)
                    {
                        var name = gViewMain.GetRowCellValue(item, "ReportName");
                        log.Info($"Отчет {name} удален!");
                        int id = (int)gViewMain.GetRowCellValue(item, "ReportDefinitionID");
                        points.AddRange(context.HistPoints.Where(w => w.ReportDefinitionID == id));
                    }
                    gViewMain.DeleteSelectedRows();
                    context.HistPoints.RemoveRange(points);
                    context.SaveChanges();
                }
            }

        }
        #endregion
        private void InitgViewMain()
        {
            gViewMain.OptionsBehavior.AutoPopulateColumns = false;
            gViewMain.OptionsBehavior.Editable = false;
            gViewMain.OptionsView.ShowIndicator = false;
            gViewMain.OptionsSelection.EnableAppearanceFocusedCell = false;
            gViewMain.FocusRectStyle = DrawFocusRectStyle.RowFocus;
            gViewMain.OptionsDetail.EnableMasterViewMode = false;
            gViewMain.OptionsSelection.MultiSelect = true;
            gViewMain.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;

            gViewMain.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "ReportDefinitionID",
                FieldName = "ReportDefinitionID",
                Caption = "ID",
                Visible = false,
            });
            gViewMain.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "Unit",
                FieldName = "Unit",
                Caption = "Блок",
                Visible = true,
                Width = 30,


            });
            gViewMain.Columns["Unit"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gViewMain.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "ReportName",
                FieldName = "ReportName",
                Caption = "Название отчета",
                Visible = true,
                Width = 120,
            });

            //RepositoryItemLookUpEdit lookUpEditReportDestId = new RepositoryItemLookUpEdit()
            //{
            //    DisplayMember = "DestName",
            //    ValueMember = "ReportDestID",
            //};
            //gViewMain.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            //{
            //    Name = "ReportDestID",
            //    FieldName = "ReportDestID",
            //    Caption = "Тип",
            //    Visible = false,
            //    Width = 30,
            //    ColumnEdit = new RepositoryItemLookUpEdit()
            //    {
            //        DataSource = ReportDefinition.DestinationType,
            //        ShowHeader = false,
            //        ShowFooter = false,
            //        PopupFormMinSize = new Size(1, 1),
            //        DropDownRows = ReportDefinition.DestinationType.Count
            //    }
            //});
            gViewMain.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "DestinationInfo",
                FieldName = "DestinationInfo",
                Caption = "Расположение файла",
                Visible = true,
                Width = 150

            });

            gViewMain.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "ReportTypeID",
                FieldName = "ReportTypeID",
                Caption = "Вид отчета",
                Visible = true,
                Width = 40,
                ColumnEdit = new RepositoryItemLookUpEdit()
                {
                    DataSource = ReportDefinition.ReportType,
                    ShowHeader = false,
                    ShowFooter = false,
                    PopupFormMinSize = new Size(1, 1),
                    DropDownRows = ReportDefinition.ReportType.Count
                }
            });

            string formatString = "dd.MM.yy HH:mm";

            RepositoryItemDateEdit editNextEvent = new RepositoryItemDateEdit();
            editNextEvent.DisplayFormat.FormatType = FormatType.DateTime;
            editNextEvent.DisplayFormat.FormatString = formatString;
            editNextEvent.EditFormat.FormatType = FormatType.DateTime;
            editNextEvent.EditFormat.FormatString = formatString;
            editNextEvent.EditMask = formatString;
            editNextEvent.MinValue = new DateTime(2008, 1, 1);
            gViewMain.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "NextEvent",
                FieldName = "NextEvent",
                Caption = "Следущая дата",
                Visible = true,
                ColumnEdit = editNextEvent
            });

            //gViewMain.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            //{
            //    Name = "Start",
            //    FieldName = "Start",
            //    Caption = "Начальная дата",
            //    Visible = false

            //});
            //gViewMain.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            //{
            //    Name = "End",
            //    FieldName = "End",
            //    Caption = "Конечная дата",
            //    Visible = false,
            //});


            gViewMain.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "Enable",
                FieldName = "Enable",
                Caption = "Авто",
                Visible = true,
                Width = 30,
                ColumnEdit = new RepositoryItemCheckEdit()
            });

            //RepositoryItemDateEdit editEnd = new RepositoryItemDateEdit();
            //editEnd.DisplayFormat.FormatType = FormatType.DateTime;
            //editEnd.DisplayFormat.FormatString = formatString;
            //editEnd.EditFormat.FormatType = FormatType.DateTime;
            //editEnd.EditFormat.FormatString = formatString;
            //editEnd.EditMask = formatString;
            //editEnd.MinValue = new DateTime(2008, 1, 1);
            gViewMain.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "End",
                FieldName = "LastUsed",
                Caption = "Последняя дата форм.",
                Visible = true,
                //ColumnEdit = editEnd,
            });
        }
        private void InitgViewDetail()
        {
            gViewDetail.OptionsBehavior.AutoPopulateColumns = false;
            // gridControl.ForceInitialize();


            gViewDetail.OptionsBehavior.Editable = false;
            gViewDetail.OptionsView.ShowIndicator = false;
            gViewDetail.OptionsSelection.EnableAppearanceFocusedCell = false;
            gViewDetail.FocusRectStyle = DrawFocusRectStyle.RowFocus;
            //gwMain.OptionsView.NewItemRowPosition = NewItemRowPosition.Top;
            gViewDetail.OptionsBehavior.EditingMode = GridEditingMode.Inplace;
            gViewDetail.OptionsBehavior.EditorShowMode = EditorShowMode.Click;
            gViewDetail.OptionsEditForm.EditFormColumnCount = 1;
            gViewDetail.OptionsEditForm.PopupEditFormWidth = 300;

            //gwMain.OptionsBehavior.ImmediateUpdateRowPosition = true;

            #region !!!!!!!!!!!!!!!!!!Columns!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! 

            DevExpress.XtraGrid.Columns.GridColumn colPoinPosn = new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "PointPosn",
                FieldName = "PointPosn",
                Caption = "№ п/п",
                Visible = true,
                Width = 20,
            };
            colPoinPosn.OptionsColumn.ImmediateUpdateRowPosition = DevExpress.Utils.DefaultBoolean.True;
            colPoinPosn.OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;

            gViewDetail.Columns.Add(colPoinPosn);
            gViewDetail.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
             new DevExpress.XtraGrid.Columns.GridColumnSortInfo(colPoinPosn, DevExpress.Data.ColumnSortOrder.Ascending)});


            gViewDetail.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "HistPointID",
                FieldName = "HistPointID",
                Caption = "ID"

            });

            gViewDetail.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "ReportDefinitionID",
                FieldName = "ReportDefinitionID",
                Caption = "ReportID"

            });

            gViewDetail.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "PointName",
                FieldName = "PointName",
                Caption = "Код сигнала",
                Visible = true,

            });

            gViewDetail.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "Description",
                FieldName = "Description",
                Caption = "Описание сигнала",
                Visible = true,

            });
            gViewDetail.Columns["Description"].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;

            gViewDetail.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "Format",
                FieldName = "Format",
                Caption = "Формат",
                Visible = true,
                Width = 20
            });


            gViewDetail.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "ProcType",
                FieldName = "ProcType",
                Caption = "Тип выбоки",
                Visible = false,
                Width = 30,
                ColumnEdit = new RepositoryItemLookUpEdit()
                {
                    DataSource = ReportDefinition.Filters,
                    ShowHeader = false,
                    ShowFooter = false,
                    PopupFormMinSize = new Size(1, 1),
                    DropDownRows = ReportDefinition.Filters.Count
                }
            });

            gViewDetail.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "IntegConst",
                FieldName = "IntegConst",
                Caption = "Интегр.",
                Visible = false,
                Width = 20

            });
            RepositoryItemComboBox boxBitNumber = new RepositoryItemComboBox();
            for (int i = 0; i < 32; i++)
            {
                boxBitNumber.Items.Add(i);
            }

            gViewDetail.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "BitNumber",
                FieldName = "BitNumber",
                Caption = "Бит",
                Visible = false,
                Width = 10,
                ColumnEdit = boxBitNumber

            });
            #endregion
        }
        private void InitLog()
        {
            gridViewLog.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "ID",
                FieldName = "ID",
                Caption = "ID"
            });

            gridViewLog.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "ReportDefinitionID",
                FieldName = "ReportDefinitionID",
                Caption = "ReportDefinitionID",

            });
            gridViewLog.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "ReportName",
                FieldName = "ReportName",
                Caption = "ReportName",
                Visible = true,

            });

            DevExpress.XtraGrid.Columns.GridColumn colCreateDate = new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "CreateDate",
                FieldName = "CreateDate",
                Caption = "CreateDate",
                Visible = true,
                SortOrder = DevExpress.Data.ColumnSortOrder.Descending,
            };
            colCreateDate.OptionsColumn.ImmediateUpdateRowPosition = DevExpress.Utils.DefaultBoolean.True;
            gridViewLog.Columns.Add(colCreateDate);
            gridViewLog.SortInfo.AddRange(new[] {
             new DevExpress.XtraGrid.Columns.GridColumnSortInfo(colCreateDate, DevExpress.Data.ColumnSortOrder.Descending)});


            gridViewLog.Columns["CreateDate"].DisplayFormat.FormatString = "dd.MM.yy HH:mm:ss";
            gridViewLog.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "ExecuteDate",
                FieldName = "ExecuteDate",
                Caption = "ExecuteDate",
                Visible = true,

            });
            gridViewLog.Columns["ExecuteDate"].DisplayFormat.FormatString = "dd.MM.yy HH:mm:ss";
            gridViewLog.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "ReportDestID",
                FieldName = "ReportDestID",
                Caption = "Тип",
                Visible = true,
                Width = 30,
                ColumnEdit = new RepositoryItemLookUpEdit()
                {
                    DataSource = ReportDefinition.DestinationType,
                    ShowHeader = false,
                    ShowFooter = false,
                    PopupFormMinSize = new Size(1, 1),
                    DropDownRows = ReportDefinition.DestinationType.Count
                }
            });

            gridViewLog.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "DestinationInfo",
                FieldName = "DestinationInfo",
                Caption = "DestinationInfo",
                Visible = true,

            });

            gridViewLog.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "StartDate",
                FieldName = "StartDate",
                Caption = "StartDate",
                Visible = true


            });

            gridViewLog.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "EndDate",
                FieldName = "EndDate",
                Caption = "EndDate",
                Visible = true,

            });
            gridViewLog.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "ErrorStr",
                FieldName = "ErrorStr",
                Caption = "ErrorStr",
                Visible = true,

            });

            //gridViewLog.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            //{
            //    Name = "ReportStatusID",
            //    FieldName = "ReportStatusID",
            //    Caption = "Статус",
            //    Visible = true,
            //    Width = 30,
            //    ColumnEdit = new RepositoryItemLookUpEdit()
            //    {
            //        DataSource = ReportLog.ReportStatus,
            //        ShowHeader = false,
            //        ShowFooter = false,
            //        PopupFormMinSize = new Size(1, 1),
            //        DropDownRows = ReportLog.ReportStatus.Count
            //    }
            //});

            gridViewLog.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "HistorianServer",
                FieldName = "HistorianServer",
                Caption = "HistorianServer",
                Visible = true,

            });

            //List<ReportDefinition> reports = db.ReportDefinitions.ToList();
            gridViewLog.OptionsBehavior.AutoPopulateColumns = false;

            gridViewLog.OptionsBehavior.Editable = false;
            gridViewLog.OptionsView.ShowIndicator = false;
            gridViewLog.OptionsSelection.EnableAppearanceFocusedCell = false;
            gridViewLog.FocusRectStyle = DrawFocusRectStyle.RowFocus;
            gridViewLog.OptionsBehavior.ImmediateUpdateRowPosition = true;

        }

        private void gridControl_ProcessGridKey(object sender, KeyEventArgs e)
        {
            var grid = sender as GridControl;
            var view = grid.FocusedView as GridView;
            if (e.KeyData == Keys.Delete)
            {
                view.DeleteSelectedRows();
                e.Handled = true;
            }
        }
        private OptionsLayoutGrid GetLayoutSettings()
        {
            OptionsLayoutGrid oLayoutOptions = new OptionsLayoutGrid();
            oLayoutOptions.Columns.StoreAllOptions = true;
            oLayoutOptions.StoreAppearance = true;
            oLayoutOptions.StoreVisualOptions = true;
            oLayoutOptions.StoreDataSettings = true;
            oLayoutOptions.Columns.StoreAppearance = true;
            return oLayoutOptions;
        }

        #region Ribbon Bar
        private void BbNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gViewMain.AddNewRow();
            report = (ReportDefinition)bindingSourceMain.Current;
            Select_Detail(true);
        }
        private void bbSaveOptions_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gridConrolMain.MainView.SaveLayoutToXml(_fileName);
                if ((int)Settings.Default["Period"] != int.Parse(barPeriod.EditValue.ToString()))
                {
                    CycleSet = int.Parse(barPeriod.EditValue.ToString());
                    CycleTimer = 0;
                    Settings.Default["Period"] = CycleSet;
                }
                Settings.Default.Save();
            }
            catch
            {
                XtraMessageBox.Show("Что-то не так с настройками.", "Ошибка", DefaultBoolean.True);
            }
        }
        private void BbImport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                ImportExport.ReportImport(dialog.FileName);
                gViewMain.RefreshData();
            }
        }
        private void BbStartTimer_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!timer1.Enabled)
            {
                timer1.Start();
                bbStartTimer.Enabled = false;
                bbStop.Enabled = true;
                barStaticItemInfo.Caption = string.Empty;
                barStaticItemInfo.ItemAppearance.Normal.Options.UseBackColor = false;
            }
        }
        private void BbStop_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Stop();
                bbStop.Enabled = false;
                bbStartTimer.Enabled = true;
                barStaticItemInfo.Caption = "Таймер остановлен!!!";
                barStaticItemInfo.ItemAppearance.Normal.BackColor = Color.Tomato;
                barStaticItemInfo.ItemAppearance.Normal.Options.UseBackColor = true;

            }
            //cancelTokenSource.Cancel();
            //  if (DataClient.cancellationTokenSource != null)
            //      DataClient.cancellationTokenSource.Cancel();
            //    if (MessageClient.cancellationTokenSource != null)
            //        MessageClient.cancellationTokenSource.Cancel();
        }
        private void barButtonItem2_ItemClick_1(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            timer1.Start();
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            timer1.Stop();
        }

        #endregion

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized) this.WindowState = FormWindowState.Normal;
            this.Activate();
        }

        #region UPDATING!!!!!
        //private void bbNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)

        //}

        #endregion

        private void tabPane1_SelectedPageIndexChanged(object sender, EventArgs e)
        {
            if (tabPane1.SelectedPageIndex != 1) return;

        }

        private void gridView_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (!btnCancel.Enabled)
            {
                e.Appearance.BackColor = _disabledColor;
            }
            else
            {
                e.Appearance.Reset();
            }
        }


    }
}
