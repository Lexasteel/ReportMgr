using AdapterOPH;
using Dapper;
using Dapper.Contrib.Extensions;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using Models;
using Npgsql;
using ReportMgr.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ReportMgr
{
    public partial class FrmMain : XtraForm
    {
        public FrmMain()
        {
            InitializeComponent();
            this.Load += FrmMain_Load;
            this.FormClosing += FrmMain_FormClosing;
            timer1.Tick += timer1_Tick;
            gViewMain.FocusedRowChanged += GViewMain_FocusedRowChanged;
            gViewMain.DoubleClick += GwMain_DoubleClick;
            barButtonNew.ItemClick += barButtonNew_ItemClick;
            barButtonEdit.ItemClick += GwMain_DoubleClick;

            barButtonCopy.ItemClick += OnCopyRowClick;
            barButtonDelete.ItemClick += OnDeleteRowClick;

            barButtonGenerate.ItemClick += OnGenerateRowClick;

            barButtonTimerStart.ItemClick += barButtonTimerStart_ItemClick;
            barButtonTimerStop.ItemClick += barButtonTimerStop_ItemClick;

            barButtonSaveOptions.ItemClick += BarButtonSaveOptions_ItemClick;

            btnEditDestinationInfo.ButtonClick += BeditDestinationInfo_ButtonClick;
            lookUpTimeFormat.EditValueChanged += LookUpTimeFormat_EditValueChanged;
            lookUpSample.EditValueChanged += LookUpSample_EditValueChanged;

            btnAddPoint.Click += BtnAddPoint_Click;
            btnRemovePoint.Click += BtnRemovePoint_Click;
            btnUpPoint.Click += BtnUpPoint_Click;
            btnDownPoint.Click += BtnDownPoint_Click;
            btnImportPoints.Click += BtnImportPoints_Click;
            btnExportPoints.Click += BtnExportPoints_Click;

            btnGetDesc.Click += BtnGetDesc_Click;
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
            _reports = new List<ReportDefinition>();
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Reports_Connection"].ConnectionString;
            _connection = new NpgsqlConnection(connectionString);

            _bindingSourceMain.CurrentChanged += _bindingSourceMain_CurrentChanged;
            _bindingSourceMain.CurrentItemChanged += _bindingSourceMain_CurrentItemChanged;
        }


        private void _bindingSourceMain_CurrentItemChanged(object sender, EventArgs e)
        {
        }

        private void _bindingSourceMain_CurrentChanged(object sender, EventArgs e)
        {
        }

        private List<ReportDefinition> _reports;
        private readonly NpgsqlConnection _connection;
        private const string _version = "Менеджер отчетов 3.1";
        private int _cycleSet = 5;
        private int _cycleTimer = 0;
        private static bool _busy = false;
        private static readonly Stack<ReportDefinition> Reports = new Stack<ReportDefinition>();
        private readonly string _fileName = Application.StartupPath + "\\Grid_Layout.xml";
        private enum State
        {
            View,
            Edit,
            New
        }
        BindingSource _bindingSourceMain = new BindingSource();
        BindingSource _bindingSourceDetail = new BindingSource();
        static CancellationTokenSource _cancelTokenSource = new CancellationTokenSource();
        CancellationToken _token = _cancelTokenSource.Token;
        private void FrmMain_Load(object sender, EventArgs e)
        {
            InitgViewMain();
            InitgViewDetail();
            GetData();
            txtReportName.DataBindings.Add("EditValue", _bindingSourceMain, "reportname");
            cmbUnit.DataBindings.Add("EditValue", _bindingSourceMain, "unit");
            btnEditDestinationInfo.DataBindings.Add("EditValue", _bindingSourceMain, "destinationinfo");
            deNextEvent.DataBindings.Add("EditValue", _bindingSourceMain, "nextevent", true);
            chkEnable.DataBindings.Add("EditValue", _bindingSourceMain, "enable", true);
            tsOffSet.DataBindings.Add("EditValue", _bindingSourceMain, "offset", true);
            chZip.DataBindings.Add("EditValue", _bindingSourceMain, "arhive", true);


            LookUpEdit_DataBinding(lkpReportType, Dictionares.ReportType, "reporttypeid");
            LookUpEdit_DataBinding(lkpReportDest, Dictionares.DestinationType, "reportdestid");
            LookUpEdit_DataBinding(lookUpTimeFormat, Dictionares.TimeType, "timeformatid");
            LookUpEdit_DataBinding(lookUpSample, Dictionares.TimeType, "sampletimeformatid");



            foreach (var header in Dictionares.Headers)
            {
                var item = new CheckedListBoxItem(header.Id)
                {
                    Description = header.Value
                };
                chCmbHeader.Properties.Items.Add(item);
            }
            chCmbHeader.Properties.DropDownRows = Dictionares.Headers.Count;
            chCmbHeader.Properties.PopupFormMinSize = new Size(10, 10);
            chCmbHeader.Properties.PopupWidthMode = PopupWidthMode.ContentWidth;
            chCmbHeader.Properties.EditValueType = EditValueTypeCollection.CSV;
            chCmbHeader.DataBindings.Add("EditValue", _bindingSourceMain, "header2");


            this.Text = _version;
            var h = (int)Settings.Default["Heigth"];
            var w = (int)Settings.Default["Width"];
            this.ClientSize = new Size(w, h);
            this.StartPosition = FormStartPosition.CenterScreen;
            _cycleSet = (int)Settings.Default["Period"];
            barSpinCycle.EditValue = _cycleSet;



            gridConrolMain.ForceInitialize();

            barStaticInfo.Caption = "";
            barStaticTime.Caption = "";

            gViewMain.PopupMenuShowing += gridViewMain_PopupMenuShowing;
            splitContainerControl1.FixedPanel = SplitFixedPanel.Panel2;
            var panel2Width = 518;
            splitContainerControl1.Panel2.MinSize = panel2Width;
            splitContainerControl1.Panel2.Size = new Size(panel2Width, splitContainerControl1.Panel2.Size.Height);
            barButtonTimerStart.Enabled = false;
            barProgress.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
            int id = (int)gViewMain.GetFocusedRowCellValue("reportdefinitionid");
            Select_Detail(id, State.View);
        }

        private void LookUpEdit_DataBinding(LookUpEdit sender, List<Item> items, string dataMember = null)
        {
            sender.Properties.DataSource = items;
            sender.Properties.DropDownRows = items.Count;
            sender.Properties.DisplayMember = "Value";
            sender.Properties.ValueMember = "Id";
            if (dataMember != null)
            {
                sender.DataBindings.Add(new Binding("EditValue", _bindingSourceMain, dataMember));
            }
            sender.Properties.PopulateColumns();
            sender.Properties.Columns[0].Visible = false;
            sender.Properties.ShowHeader = false;
            sender.Properties.ShowFooter = false;
            sender.Properties.PopupFormMinSize = new Size(10, 10);
            sender.Properties.PopupWidthMode = PopupWidthMode.ContentWidth;
        }

        private void GetData()
        {
            //var sql = "SELECT * FROM ReportDefinitions";
            this._reports = _connection.GetAll<ReportDefinition>().ToList();
            _bindingSourceMain.DataSource = _reports.OrderBy(o => o.reportdefinitionid);
            gridConrolMain.DataSource = _bindingSourceMain;
        }

        private void GViewMain_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (e.FocusedRowHandle < 0 || e.PrevFocusedRowHandle < 0) return;
            int id = 0;
            var rowText = gViewMain.GetRowCellDisplayText(e.FocusedRowHandle, "reportdefinitionid");
            id = int.Parse(rowText);
            Select_Detail(id, State.View);
        }

        private void GwMain_DoubleClick(object sender, EventArgs e)
        {
            StateSwitch();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialog = XtraMessageBox.Show("Закрыть программу?", "Менеджер отчетов", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            switch (dialog)
            {
                case DialogResult.Yes:
                    Application.ExitThread();
                    break;
                case DialogResult.No:
                    e.Cancel = true;
                    return;
                default:
                    return;
            }
        }
        private void barButtonExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Application.ExitThread();
        }

        #region Detail
        private void Select_Detail(int id, State state)
        {
            var report = (ReportDefinition)_bindingSourceMain.Current;

            var sql = "SELECT * FROM Histpoints WHERE reportdefinitionid=@ID";
            var reps = _connection.Query<HistPoint>(sql, new { ID = id }).ToList();


            _bindingSourceDetail.DataSource = reps;
            gridControlDetail.DataSource = _bindingSourceDetail;


            report.HistPoints = state != State.New
                ? reps
                : new List<HistPoint>();

            // bindingSourceMain.ResetBindings(false);

            if (state == State.Edit || state == State.New)
            {
                StateSwitch();
            }
            GetTimeFormat(report);

        }

        private void GetTimeFormat(ReportDefinition report)
        {
            switch (report.sampletimeformatid)
            {
                case 1:
                    spinSampleHours.EditValue = report.sampletimeperiodinfo.Split(':')[0];
                    spinSampleMins.EditValue = report.sampletimeperiodinfo.Split(':')[1];
                    spinSampleSecs.EditValue = report.sampletimeperiodinfo.Split(':')[2];
                    break;
                case 6:
                    cmbFractionOfSec.EditValue = report.sampletimeperiodinfo;
                    break;
                default:
                    spinSampleHours.EditValue = report.sampletimeperiodinfo.Split(':')[0];
                    break;
            }
            switch (report.timeformatid)
            {
                case 1:
                    spinTimeHours.EditValue = report.timeperiodinfo.Split(':')[0];
                    spinTimeMinutes.EditValue = report.timeperiodinfo.Split(':')[1];
                    spinTimeSecs.EditValue = report.timeperiodinfo.Split(':')[2];
                    break;
                default:
                    spinTimeHours.EditValue = report.timeperiodinfo.Split(':')[0];
                    break;
            }

        }
        private void StateSwitch()
        {

            gridConrolMain.Enabled = !gridConrolMain.Enabled;

            grpDetail.Enabled = !grpDetail.Enabled;
            gridControlDetail.Enabled = true;
            gViewDetail.OptionsBehavior.Editable = !gViewDetail.OptionsBehavior.Editable;

            barButtonEdit.Enabled = !barButtonEdit.Enabled;
            btnSave.Enabled = !btnSave.Enabled;
            btnCancel.Enabled = !btnCancel.Enabled;
            btnGetDesc.Enabled = !btnGetDesc.Enabled;
            btnAddPoint.Enabled = !btnAddPoint.Enabled;
            btnRemovePoint.Enabled = !btnRemovePoint.Enabled;
            btnUpPoint.Enabled = !btnUpPoint.Enabled;
            btnDownPoint.Enabled = !btnDownPoint.Enabled;
            btnImportPoints.Enabled = !btnImportPoints.Enabled;
            btnExportPoints.Enabled = !btnExportPoints.Enabled;

        }
        private void BtnCancel_Click(object sender, EventArgs e)
        {

            var dialogResult = DialogResult.No;
            if (HasUnsavedChanges())
            {
                dialogResult = MessageBox.Show("Save changes?", "Report Manager", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }

            StateSwitch();
            if (dialogResult == DialogResult.Yes)
            {
                BtnSave_Click(sender, e);
            }


            //foreach (DbEntityEntry entry in _db.ChangeTracker.Entries())
            //{
            //    switch (entry.State)
            //    {
            //        case EntityState.Modified:
            //            entry.State = EntityState.Unchanged;
            //            break;
            //        case EntityState.Added:
            //            entry.State = EntityState.Detached;
            //            break;
            //        case EntityState.Deleted:
            //            entry.State = EntityState.Modified; //Revert changes made to deleted entity.
            //            entry.Reload(); //OR entry.State = EntityState.Unchanged;;
            //            break;
            //    }
            //}

            _bindingSourceMain.CancelEdit();
            _bindingSourceDetail.CancelEdit();
            var rep = (ReportDefinition)_bindingSourceMain.Current;
            //_db.Entry(rep).Reload();
            //_bindingSourceMain.ResetBindings(false);
            //_bindingSourceDetail.ResetBindings(false);
            GetTimeFormat(rep);
        }

        public bool HasUnsavedChanges()
        {
            var ret = false;
            return ret;
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            var report = (ReportDefinition)_bindingSourceMain.Current;
            var id = report.reportdefinitionid;
            switch (report.timeformatid)
            {
                case 1:
                    string[] info = { spinTimeHours.EditValue.ToString(), spinTimeMinutes.EditValue.ToString(), spinTimeSecs.EditValue.ToString() };
                    report.timeperiodinfo = String.Join(":", info);
                    break;
                default:
                    report.timeperiodinfo = spinTimeHours.EditValue.ToString();
                    break;
            }

            switch (report.sampletimeformatid)
            {
                case 1:
                    string[] info = { spinSampleHours.EditValue.ToString(), spinSampleMins.EditValue.ToString(), spinSampleSecs.EditValue.ToString() };
                    report.sampletimeperiodinfo = String.Join(":", info);
                    break;
                case 6:
                    report.sampletimeperiodinfo = cmbFractionOfSec.EditValue.ToString();
                    break;
                default:
                    report.sampletimeperiodinfo = spinSampleHours.EditValue.ToString();
                    break;
            }

            report.header2 = chCmbHeader.EditValue.ToString();
            if (id == 0)
            {
                id = Convert.ToInt32(_connection.Insert(new ReportDefinition
                {
                    reportname = report.reportname,
                    reporttypeid = report.reporttypeid,
                    reportdestid = report.reportdestid,
                    destinationinfo = report.destinationinfo,
                    arhive = report.arhive,
                    header2 = report.header2,
                    timeformatid = report.timeformatid,
                    timeperiodinfo = report.timeperiodinfo,
                    sampletimeformatid = report.sampletimeformatid,
                    sampletimeperiodinfo = report.sampletimeperiodinfo,
                    additionalparams = report.additionalparams,
                    unit = report.unit,
                    offset = report.offset,
                    nextevent = report.nextevent,
                    lastused = report.lastused,
                    enable = report.enable,
                }));
                if (report.HistPoints != null)
                    for (int i = 0; i < report.HistPoints.Count; i++)
                    {
                        _connection.Insert(new HistPoint
                        {
                            pointposn = i + 1,
                            pointname = report.HistPoints[i].pointname,
                            bitnumber = report.HistPoints[i].bitnumber,
                            proctype = report.HistPoints[i].proctype,
                            integconst = report.HistPoints[i].integconst,
                            glitchdetect = report.HistPoints[i].glitchdetect,
                            summaryenable = report.HistPoints[i].summaryenable,
                            reportdefinitionid = report.reportdefinitionid,
                            format = report.HistPoints[i].format,
                            description = report.HistPoints[i].description,
                        });
                    }
            }
            else
            {
                id = Convert.ToInt32(_connection.Update(new ReportDefinition
                {
                    reportdefinitionid = report.reportdefinitionid,
                    reportname = report.reportname,
                    reporttypeid = report.reporttypeid,
                    reportdestid = report.reportdestid,
                    destinationinfo = report.destinationinfo,
                    arhive = report.arhive,
                    header2 = report.header2,
                    timeformatid = report.timeformatid,
                    timeperiodinfo = report.timeperiodinfo,
                    sampletimeformatid = report.sampletimeformatid,
                    sampletimeperiodinfo = report.sampletimeperiodinfo,
                    additionalparams = report.additionalparams,
                    unit = report.unit,
                    offset = report.offset,
                    nextevent = report.nextevent,
                    lastused = report.lastused,
                    enable = report.enable,
                }));
                if (report.HistPoints != null)
                    for (int i = 0; i < report.HistPoints.Count; i++)
                    {
                        _connection.Update(new HistPoint
                        {
                            histpointid = report.HistPoints[i].histpointid,
                            pointposn = i + 1,
                            pointname = report.HistPoints[i].pointname,
                            bitnumber = report.HistPoints[i].bitnumber,
                            proctype = report.HistPoints[i].proctype,
                            integconst = report.HistPoints[i].integconst,
                            glitchdetect = report.HistPoints[i].glitchdetect,
                            summaryenable = report.HistPoints[i].summaryenable,
                            reportdefinitionid = report.reportdefinitionid,
                            format = report.HistPoints[i].format,
                            description = report.HistPoints[i].description,
                        });
                    }


            }



            _bindingSourceDetail.EndEdit();
            _bindingSourceMain.EndEdit();
            //gViewMain.RefreshData();
            //_bindingSourceMain.ResetBindings(false);
            //_bindingSourceDetail.ResetBindings(false);

            StateSwitch();
        }


        private void BtnGetDesc_Click(object sender, EventArgs e)
        {
            var report = (ReportDefinition)_bindingSourceMain.Current;
            if (report.reporttypeid != 1) return;
            var date = DateTime.Now;
            var target = CreateTarget(report, date, date);
            target.GetAttr();
            gViewDetail.RefreshData();
        }
        private void BeditDestinationInfo_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            string file = btnEditDestinationInfo.EditValue.ToString();
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Filter = @"Excel Workbook(*.xlsx)|*.xlsx|Excel binary Workbook(*.xlsb)|*.xlsb|CSV files(*.csv)|*.csv|All files(*.*)|*.*"
            };

            if (!string.IsNullOrEmpty(file))
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
                btnEditDestinationInfo.Text = saveFileDialog.FileName;
            }
        }
        private void LookUpTimeFormat_EditValueChanged(object sender, EventArgs e)
        {
            LookUpEdit lookUpEdit = (LookUpEdit)sender;
            if (lookUpEdit.EditValue is DBNull) return;
            int timeType = Convert.ToInt32(lookUpEdit.EditValue);

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
            LookUpEdit lookUpEdit = sender as LookUpEdit;
            if (lookUpEdit.EditValue is DBNull) return;
            int timeType = Convert.ToInt32(lookUpEdit.EditValue);
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
                    break;
            }
        }
        public static bool[] VisibilitySampleIntervals(int timeType)
        {
            var showFracOfSec = true;
            var showHours = true;
            var showMins = true;
            var showSecs = true;
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_busy) return;
            if (_cycleSet >= _cycleTimer)
            {
                barStaticTime.Caption = $"Цикл: {(_cycleSet - _cycleTimer)} сек";
                _cycleTimer++;
                return;
            }
            _cycleTimer = 0;
            var now = DateTime.Now;
            var enable = true;
            var sql = "SELECT * FROM reportdefinitions WHERE nextevent < @Now AND enable=@Enable";
            var list = _connection.Query<ReportDefinition>(sql, new { Now = now, Enable = enable }).ToList().OrderBy(o => o.reportname);

            if (!list.Any()) return;

            foreach (ReportDefinition item in list)
            {
                Reports.Push(item);
            }
            _busy = true;
            Task.Run(() =>
            {
                TaskPerform();
                _busy = false;
            }, _token);



        }
        void TaskPerform()
        {
            while (Reports.Count > 0)
            {
                var buildReport = Reports.Pop();
                if (buildReport.nextevent == null) return;
                var end = buildReport.nextevent.Value;
                var start = HelpersAdapter.DateCalc(buildReport.nextevent.Value, buildReport.timeperiodinfo, buildReport.timeformatid, true);
                var target = CreateTarget(buildReport, start, end);
                if (!target.Generate().Result) return;
                if (buildReport.nextevent != null)
                {
                    var nextEvent = HelpersAdapter.DateCalc(buildReport.nextevent.Value, buildReport.timeperiodinfo, buildReport.timeformatid);
                    _connection.Update(new ReportDefinition
                    {
                        reportdefinitionid = buildReport.reportdefinitionid,
                        nextevent = nextEvent,
                        lastused = DateTime.Now,

                    });
                }
                this.Invoke(new Action(() => gViewMain.RefreshData()));
            }
        }
        private IReport CreateTarget(ReportDefinition report, DateTime start, DateTime end)
        {
            var id = report.reportdefinitionid;

            var sql = "SELECT * FROM histpoints WHERE reportdefinitionid=@ID";
            report.HistPoints = _connection.Query<HistPoint>(sql, new { ID = id }).ToList();
            sql = "SELECT * FROM historians ";
            report.Historians = _connection.Query<Historian>(sql).ToList();

            switch (report.reporttypeid)
            {
                //case 1: //simple
                //   // return new SimpleReport(report, hist);
                //    break;
                case 2: //oper.events
                    break;
                case 3: //alarms
                    break;
                case 4: //RAW
                    var r = new RawReport(report, start, end);
                    r.ReportChanged += Report_ReportChanged;
                    return r;
            }
            var s = new SimpleReport(report, start, end);
            s.ReportChanged += Report_ReportChanged;
            return s;
        }

        private void Report_ReportChanged(object sender, ReportEventArgs e)
        {
            var report = (IReport)sender;
            Invoke(new Action(() =>
            {
                barProgress.EditValue = e.Value;
            }));
            barStaticInfo.Caption = $"{report.Report.reportname}: {report.Start.ToString()} по {report.End.ToString()}: {e.Message}";

        }

        private void barButtonNew_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            gViewMain.AddNewRow();

            Select_Detail(GridControl.NewItemRowHandle, State.New);

        }

        void OnCopyRowClick(object sender, EventArgs e)
        {

            //var rd = (ReportDefinition)_bindingSourceMain.Current;
            //int id = rd.reportdefinitionid;
            //var originalEntity = _db.ReportDefinitions.Include("HistPoints").FirstOrDefault(f => f.reportdefinitionid == id);
            //var newentity = new ReportDefinition();
            //_db.ReportDefinitions.Add(newentity);  //Create and add new entity object to context before setting its values
            //var originalEntityValues = _db.Entry(originalEntity).CurrentValues;
            //_db.Entry(newentity).CurrentValues.SetValues(originalEntityValues);  //Copy values from original entity to new entity
            //newentity.reportname += " - Копия";

            //var points = _db.HistPoints.Where(w => w.reportdefinitionid == id);
            //foreach (var item in points)
            //{
            //    var originalItemValues = _db.Entry(item).CurrentValues;
            //    HistPoint point = new HistPoint();
            //    _db.HistPoints.Add(point);
            //    _db.Entry(point).CurrentValues.SetValues(originalItemValues);
            //    point.reportdefinitionid = newentity.reportdefinitionid;
            //}
            //_db.SaveChanges();
            //gViewMain.RefreshData();
        }

        void OnGenerateRowClick(object sender, EventArgs e)
        {
            //formInputDates frmInput = new formInputDates();
            //var report = (ReportDefinition)_bindingSourceMain.Current;
            //DateTime date = DateTime.Now;
            //frmInput.DateEnd = new DateTime(date.Year, date.Month, date.Day, date.Hour, 0, 0);
            //frmInput.DateStart = frmInput.DateEnd.Subtract(TimeSpan.Parse(report.timeperiodinfo));
            //if (frmInput.ShowDialog() != DialogResult.OK) return;
            //var target = CreateTarget(report, frmInput.DateStart, frmInput.DateEnd);

            //target.MultiSheets = frmInput.SingleFile;

            //frmInput.Dispose();
            //var now = DateTime.Now;
            //report.lastused = DateTime.Now;
            //_busy = true;
            //Task.Run(() =>
            //{
            //    if (target.Generate().Result)
            //    {
            //        _bindingSourceMain.EndEdit();
            //        _db.SaveChanges();
            //        this.Invoke(new Action(() => gViewMain.RefreshData()));
            //    }

            //    _busy = false;
            //}, _token);


        }
        void OnDeleteRowClick(object sender, EventArgs e)
        {

            //var handles = gViewMain.GetSelectedRows();
            //var text = "Удаляем: ";
            //foreach (var item in handles)
            //{
            //    text += gViewMain.GetRowCellValue(item, "reportname");
            //    if (item != handles.Last())
            //    {
            //        text += ", ";
            //    }
            //}
            //text += "?";
            //DialogResult dialogResult = XtraMessageBox.Show(text, "Подтверждение удаления", MessageBoxButtons.YesNo);
            //List<HistPoint> points = new List<HistPoint>();
            //if (dialogResult != DialogResult.Yes) return;
            //var rows = gViewMain.GetSelectedRows();

            //foreach (var item in rows)
            //{
            //    var name = gViewMain.GetRowCellValue(item, "reportname");
            //    //??????????????????????????
            //    //log.Info($"Отчет {name} удален!");
            //    int id = (int)gViewMain.GetRowCellValue(item, "reportdefinitionid");
            //    points.AddRange(_db.HistPoints.Where(w => w.reportdefinitionid == id));
            //}
            //gViewMain.DeleteSelectedRows();
            //_db.HistPoints.RemoveRange(points);
            //_db.SaveChanges();


        }

        private void barButtonTimerStart_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!timer1.Enabled)
            {
                timer1.Start();
                barButtonTimerStart.Enabled = false;
                barButtonTimerStop.Enabled = true;
                barStaticInfo.Caption = string.Empty;
                barStaticInfo.ItemAppearance.Normal.Options.UseBackColor = false;
            }
        }
        private void barButtonTimerStop_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (timer1.Enabled)
            {
                timer1.Stop();
                barButtonTimerStop.Enabled = false;
                barButtonTimerStart.Enabled = true;
                barStaticInfo.Caption = "Таймер остановлен!!!";
                barStaticInfo.ItemAppearance.Normal.BackColor = Color.Tomato;
                barStaticInfo.ItemAppearance.Normal.Options.UseBackColor = true;

            }
            //cancelTokenSource.Cancel();
            //  if (DataClient.cancellationTokenSource != null)
            //      DataClient.cancellationTokenSource.Cancel();
            //    if (MessageClient.cancellationTokenSource != null)
            //        MessageClient.cancellationTokenSource.Cancel();
        }

        private void BtnDownPoint_Click(object sender, EventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.GridView view = gViewDetail;
            view.GridControl.Focus();
            int index = view.FocusedRowHandle;
            if (index >= view.DataRowCount - 1) return;

            HistPoint row1 = (HistPoint)view.GetRow(index);
            HistPoint row2 = (HistPoint)view.GetRow(index + 1);
            int val1 = row1.pointposn;
            int val2 = row2.pointposn;
            row1.pointposn = val2;
            row2.pointposn = val1;
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
            int val1 = row1.pointposn;
            int val2 = row2.pointposn;
            row1.pointposn = val2;
            row2.pointposn = val1;
            view.RefreshData();
            view.FocusedRowHandle = index - 1;
        }

        private void BtnRemovePoint_Click(object sender, EventArgs e)
        {
            //var handle = gViewDetail.FocusedRowHandle;
            //var id = (int)gViewDetail.GetRowCellValue(handle, "histpointid");
            //var item = _db.HistPoints.FirstOrDefault(f => f.histpointid == id);
            //gViewDetail.DeleteRow(handle);
            //_db.Entry(item).State = EntityState.Deleted;
        }
        private void BtnAddPoint_Click(object sender, EventArgs e)
        {
            var report = (ReportDefinition)_bindingSourceMain.Current;
            if (report.HistPoints == null) report.HistPoints = new List<HistPoint>();
            _bindingSourceDetail.DataSource = report.HistPoints;
            gViewDetail.AddNewRow();
            gViewDetail.SetRowCellValue(GridControl.NewItemRowHandle, gViewDetail.Columns["format"], "0.00");
            gViewDetail.SetRowCellValue(GridControl.NewItemRowHandle, gViewDetail.Columns["integconst"], "1.0");
            gViewDetail.SetRowCellValue(GridControl.NewItemRowHandle, gViewDetail.Columns["reportdefinitionid"], report.reportdefinitionid);

            if (!report.HistPoints.Any())
            {
                gViewDetail.SetRowCellValue(gViewDetail.FocusedRowHandle, gViewDetail.Columns["pointposn"], 1);
            }
            else
                gViewDetail.SetRowCellValue(gViewDetail.FocusedRowHandle, gViewDetail.Columns["pointposn"], report.HistPoints.Max(m => m.pointposn) + 1);

        }

        private void BtnImportPoints_Click(object sender, EventArgs e)
        {
            //var dialog = new OpenFileDialog();
            //var pointposn = 1;

            //if (dialog.ShowDialog() != DialogResult.OK) return;

            //var dialogPoints = MessageBox.Show("Заменить точки новыми?", "Импорт точек", MessageBoxButtons.YesNo,
            //    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            //var file = File.ReadAllLines(dialog.FileName);
            //var report = (ReportDefinition)_bindingSourceMain.Current;
            //if (dialogPoints == DialogResult.Yes)
            //{
            //    if (report.HistPoints != null) _db.HistPoints.RemoveRange(report.HistPoints);
            //}
            //else
            //{
            //    pointposn = report.HistPoints.Max(m => m.pointposn) + 1;
            //}
            //foreach (var row in file)
            //{
            //    var s = row.Split(';');
            //    var p = new HistPoint
            //    {
            //        pointname = s[0]
            //    };
            //    if (s.Length > 1)
            //    {
            //        p.format = s[1];
            //    }
            //    if (s.Length > 2)
            //    {
            //        p.description = s[2];
            //    }
            //    p.reportdefinitionid = report.reportdefinitionid;
            //    p.pointposn = pointposn;
            //    pointposn++;
            //    _db.HistPoints.Add(p);
            //}

            //var items = _db.HistPoints.Where(w => w.reportdefinitionid == report.reportdefinitionid).OrderBy(o => o.pointposn);
            //int max = items.Max(m => m.pointposn);
            //int count = items.Count();
            //if (max != count)
            //{
            //    int i = 1;
            //    foreach (var item in items)
            //    {
            //        item.pointposn = i;
            //        i++;
            //    }
            //}
            //gViewDetail.RefreshData();
            //barStaticInfo.Caption = $"Imported {dialog.FileName}";
        }
        private void BtnExportPoints_Click(object sender, EventArgs e)
        {
            var report = (ReportDefinition)_bindingSourceMain.Current;
            if (report.HistPoints == null) return;
            var list = report.HistPoints.Select(p => string.Join(";", p.pointname,
                    //p.bitnumber,
                    //p.proctype,
                    //p.integconst,
                    //p.glitchdetect,
                    //p.summaryenable,
                    p.format, p.description))
                .ToList();
            var dir = Path.GetDirectoryName(report.destinationinfo);
            var filename = Path.GetFileNameWithoutExtension(report.destinationinfo);
            if (dir == null) return;
            var combine = Path.Combine(dir, "Points_" + filename + ".txt");
            var dialog = new SaveFileDialog() { FileName = combine };

            if (dialog.ShowDialog() != DialogResult.OK) return;
            File.WriteAllLines(dialog.FileName, list);
            barStaticInfo.Caption = $@"Exported {dialog.FileName}";

        }

        private void BarButtonSaveOptions_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                gridConrolMain.MainView.SaveLayoutToXml(_fileName);

                _cycleSet = int.Parse(barSpinCycle.EditValue.ToString());
                _cycleTimer = 0;
                Settings.Default["Period"] = _cycleSet;
                Settings.Default["Heigth"] = this.ClientSize.Height;
                Settings.Default["Width"] = this.ClientSize.Width;

                Settings.Default.Save();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Ошибка, что-то не так с настройками.", DefaultBoolean.True);
            }
        }

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
            var path = _bindingSourceMain.Current as ReportDefinition;
            ImportExport.ReportExplorer(path.destinationinfo);
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
                Name = "reportdefinitionid",
                FieldName = "reportdefinitionid",
                Caption = "ID",
                Visible = false,
            });
            gViewMain.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "Unit",
                FieldName = "unit",
                Caption = "Блок",
                Visible = true,
                Width = 30,


            });
            gViewMain.Columns["unit"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            gViewMain.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "ReportName",
                FieldName = "reportname",
                Caption = "Название отчета",
                Visible = true,
                Width = 120,
            });

            RepositoryItemLookUpEdit lookUpEditReportDestId = new RepositoryItemLookUpEdit()
            {
                DisplayMember = "DestName",
                ValueMember = "ReportDestID",
            };
            gViewMain.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "ReportDestID",
                FieldName = "reportdestid",
                Caption = "Тип",
                Visible = false,
                Width = 30,
                ColumnEdit = new RepositoryItemLookUpEdit()
                {
                    DataSource = Dictionares.DestinationType,
                    ShowHeader = false,
                    ShowFooter = false,
                    PopupFormMinSize = new Size(1, 1),
                    DropDownRows = Dictionares.DestinationType.Count
                }
            });
            gViewMain.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "DestinationInfo",
                FieldName = "destinationinfo",
                Caption = "Расположение файла",
                Visible = true,
                Width = 150

            });

            gViewMain.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "ReportTypeID",
                FieldName = "reporttypeid",
                Caption = "Вид отчета",
                Visible = true,
                Width = 40,
                ColumnEdit = new RepositoryItemLookUpEdit()
                {
                    DataSource = Dictionares.ReportType,
                    ValueMember = "Id",
                    DisplayMember = "Value"
                }
            });


            gViewMain.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "NextEvent",
                FieldName = "nextevent",
                Caption = "Следущая дата",
                Visible = true,
            });

            gViewMain.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "Enable",
                FieldName = "enable",
                Caption = "Авто",
                Visible = true,
                Width = 30,
                ColumnEdit = new RepositoryItemCheckEdit()
            });


            gViewMain.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "LastUsed",
                FieldName = "lastused",
                Caption = "Последняя дата форм.",
                Visible = true,
            });
        }
        private void InitgViewDetail()
        {
            gViewDetail.OptionsBehavior.AutoPopulateColumns = false;


            gViewDetail.OptionsBehavior.Editable = false;
            gViewDetail.OptionsView.ShowIndicator = false;
            gViewDetail.OptionsSelection.EnableAppearanceFocusedCell = false;
            gViewDetail.FocusRectStyle = DrawFocusRectStyle.RowFocus;
            gViewDetail.OptionsBehavior.EditingMode = GridEditingMode.Inplace;
            gViewDetail.OptionsBehavior.EditorShowMode = EditorShowMode.Click;
            gViewDetail.OptionsEditForm.EditFormColumnCount = 1;
            gViewDetail.OptionsEditForm.PopupEditFormWidth = 300;


            #region !!!!!!!!!!!!!!!!!!Columns!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! 

            DevExpress.XtraGrid.Columns.GridColumn colPoinPosn = new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "pointposn",
                FieldName = "pointposn",
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
                Name = "histpointid",
                FieldName = "histpointid",
                Caption = "ID"

            });



            gViewDetail.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "pointname",
                FieldName = "pointname",
                Caption = "Код сигнала",
                Visible = true,

            });

            gViewDetail.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "description",
                FieldName = "description",
                Caption = "Описание сигнала",
                Visible = true,

            });
            gViewDetail.Columns["description"].OptionsEditForm.Visible = DevExpress.Utils.DefaultBoolean.False;

            gViewDetail.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "format",
                FieldName = "format",
                Caption = "Формат",
                Visible = true,
                Width = 20
            });


            gViewDetail.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "proctype",
                FieldName = "proctype",
                Caption = "Тип выбоки",
                Visible = false,
                Width = 30,
                ColumnEdit = new RepositoryItemLookUpEdit()
                {
                    DataSource = Dictionares.Filters,
                    DisplayMember = "Value",
                    ValueMember = "Id",
                    PopupFormMinSize = new Size(1, 1),
                    DropDownRows = Dictionares.Filters.Count
                }
            });

            gViewDetail.Columns.Add(new DevExpress.XtraGrid.Columns.GridColumn()
            {
                Name = "integconst",
                FieldName = "integconst",
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
                Name = "bitnumber",
                FieldName = "bitnumber",
                Caption = "Бит",
                Visible = false,
                Width = 10,
                ColumnEdit = boxBitNumber

            });
            #endregion
        }
    }
}
