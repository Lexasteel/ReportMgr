
namespace ReportMgr
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
            this.bar1 = new DevExpress.XtraBars.Bar();
            this.barButtonNew = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonEdit = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonCopy = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonDelete = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonGenerate = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonTimerStart = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonTimerStop = new DevExpress.XtraBars.BarButtonItem();
            this.barSpinCycle = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemSpinEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.barButtonSaveOptions = new DevExpress.XtraBars.BarButtonItem();
            this.bar2 = new DevExpress.XtraBars.Bar();
            this.barSubItem1 = new DevExpress.XtraBars.BarSubItem();
            this.barButtonExit = new DevExpress.XtraBars.BarButtonItem();
            this.barSubItem2 = new DevExpress.XtraBars.BarSubItem();
            this.barSubItem3 = new DevExpress.XtraBars.BarSubItem();
            this.bar3 = new DevExpress.XtraBars.Bar();
            this.barStaticTime = new DevExpress.XtraBars.BarStaticItem();
            this.barStaticInfo = new DevExpress.XtraBars.BarStaticItem();
            this.barProgress = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemProgressBar1 = new DevExpress.XtraEditors.Repository.RepositoryItemProgressBar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.gridConrolMain = new DevExpress.XtraGrid.GridControl();
            this.gViewMain = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.grpDetail = new DevExpress.XtraEditors.GroupControl();
            this.lblUnit = new DevExpress.XtraEditors.LabelControl();
            this.grpPeriod = new DevExpress.XtraEditors.GroupControl();
            this.spinTimeSecs = new DevExpress.XtraEditors.SpinEdit();
            this.spinTimeMinutes = new DevExpress.XtraEditors.SpinEdit();
            this.spinTimeHours = new DevExpress.XtraEditors.SpinEdit();
            this.lookUpTimeFormat = new DevExpress.XtraEditors.LookUpEdit();
            this.chkEnable = new DevExpress.XtraEditors.CheckEdit();
            this.grpInterval = new DevExpress.XtraEditors.GroupControl();
            this.cmbFractionOfSec = new DevExpress.XtraEditors.ComboBoxEdit();
            this.spinSampleSecs = new DevExpress.XtraEditors.SpinEdit();
            this.spinSampleMins = new DevExpress.XtraEditors.SpinEdit();
            this.spinSampleHours = new DevExpress.XtraEditors.SpinEdit();
            this.lookUpSample = new DevExpress.XtraEditors.LookUpEdit();
            this.chZip = new DevExpress.XtraEditors.CheckEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.lkpReportDest = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.tsOffSet = new DevExpress.XtraEditors.TimeSpanEdit();
            this.chCmbHeader = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.deNextEvent = new DevExpress.XtraEditors.DateEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.btnEditDestinationInfo = new DevExpress.XtraEditors.ButtonEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txtReportName = new DevExpress.XtraEditors.TextEdit();
            this.cmbUnit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.lkpReportType = new DevExpress.XtraEditors.LookUpEdit();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.gridControlDetail = new DevExpress.XtraGrid.GridControl();
            this.gViewDetail = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnImportPoints = new DevExpress.XtraEditors.SimpleButton();
            this.btnGetDesc = new DevExpress.XtraEditors.SimpleButton();
            this.btnExportPoints = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnDownPoint = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnUpPoint = new DevExpress.XtraEditors.SimpleButton();
            this.btnRemovePoint = new DevExpress.XtraEditors.SimpleButton();
            this.btnAddPoint = new DevExpress.XtraEditors.SimpleButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.sqlDataSource1 = new DevExpress.DataAccess.Sql.SqlDataSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemProgressBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel1)).BeginInit();
            this.splitContainerControl1.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel2)).BeginInit();
            this.splitContainerControl1.Panel2.SuspendLayout();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridConrolMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gViewMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpDetail)).BeginInit();
            this.grpDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpPeriod)).BeginInit();
            this.grpPeriod.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spinTimeSecs.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinTimeMinutes.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinTimeHours.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpTimeFormat.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEnable.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpInterval)).BeginInit();
            this.grpInterval.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbFractionOfSec.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinSampleSecs.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinSampleMins.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinSampleHours.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpSample.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chZip.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpReportDest.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tsOffSet.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chCmbHeader.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deNextEvent.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deNextEvent.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnEditDestinationInfo.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReportName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUnit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpReportType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gViewDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // barManager1
            // 
            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.bar1,
            this.bar2,
            this.bar3});
            this.barManager1.DockControls.Add(this.barDockControlTop);
            this.barManager1.DockControls.Add(this.barDockControlBottom);
            this.barManager1.DockControls.Add(this.barDockControlLeft);
            this.barManager1.DockControls.Add(this.barDockControlRight);
            this.barManager1.Form = this;
            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barSubItem1,
            this.barSubItem2,
            this.barSubItem3,
            this.barButtonExit,
            this.barStaticTime,
            this.barStaticInfo,
            this.barProgress,
            this.barButtonNew,
            this.barButtonEdit,
            this.barButtonCopy,
            this.barButtonDelete,
            this.barButtonGenerate,
            this.barButtonTimerStart,
            this.barButtonTimerStop,
            this.barSpinCycle,
            this.barButtonSaveOptions});
            this.barManager1.MainMenu = this.bar2;
            this.barManager1.MaxItemId = 25;
            this.barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemProgressBar1,
            this.repositoryItemSpinEdit2});
            this.barManager1.StatusBar = this.bar3;
            // 
            // bar1
            // 
            this.bar1.BarName = "Tools";
            this.bar1.DockCol = 0;
            this.bar1.DockRow = 1;
            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonNew),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonEdit),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonCopy),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonDelete),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonGenerate, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonTimerStart, true),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonTimerStop),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSpinCycle),
            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonSaveOptions)});
            this.bar1.OptionsBar.AllowQuickCustomization = false;
            this.bar1.OptionsBar.DrawDragBorder = false;
            this.bar1.Text = "Tools";
            // 
            // barButtonNew
            // 
            this.barButtonNew.Caption = "New report";
            this.barButtonNew.Id = 15;
            this.barButtonNew.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonNew.ImageOptions.SvgImage")));
            this.barButtonNew.Name = "barButtonNew";
            // 
            // barButtonEdit
            // 
            this.barButtonEdit.Caption = "Edit Report";
            this.barButtonEdit.Id = 16;
            this.barButtonEdit.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonEdit.ImageOptions.SvgImage")));
            this.barButtonEdit.Name = "barButtonEdit";
            // 
            // barButtonCopy
            // 
            this.barButtonCopy.Caption = "Copy";
            this.barButtonCopy.Id = 17;
            this.barButtonCopy.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonCopy.ImageOptions.SvgImage")));
            this.barButtonCopy.Name = "barButtonCopy";
            // 
            // barButtonDelete
            // 
            this.barButtonDelete.Caption = "Delete";
            this.barButtonDelete.Id = 18;
            this.barButtonDelete.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonDelete.ImageOptions.SvgImage")));
            this.barButtonDelete.Name = "barButtonDelete";
            // 
            // barButtonGenerate
            // 
            this.barButtonGenerate.Caption = "Generate report";
            this.barButtonGenerate.Id = 19;
            this.barButtonGenerate.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonGenerate.ImageOptions.SvgImage")));
            this.barButtonGenerate.Name = "barButtonGenerate";
            // 
            // barButtonTimerStart
            // 
            this.barButtonTimerStart.Caption = "Timer start";
            this.barButtonTimerStart.Id = 20;
            this.barButtonTimerStart.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonTimerStart.ImageOptions.SvgImage")));
            this.barButtonTimerStart.Name = "barButtonTimerStart";
            // 
            // barButtonTimerStop
            // 
            this.barButtonTimerStop.Caption = "Timer stop";
            this.barButtonTimerStop.Id = 21;
            this.barButtonTimerStop.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonTimerStop.ImageOptions.SvgImage")));
            this.barButtonTimerStop.Name = "barButtonTimerStop";
            // 
            // barSpinCycle
            // 
            this.barSpinCycle.Caption = "Cycle";
            this.barSpinCycle.Edit = this.repositoryItemSpinEdit2;
            this.barSpinCycle.Id = 23;
            this.barSpinCycle.Name = "barSpinCycle";
            // 
            // repositoryItemSpinEdit2
            // 
            this.repositoryItemSpinEdit2.AutoHeight = false;
            this.repositoryItemSpinEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemSpinEdit2.Name = "repositoryItemSpinEdit2";
            // 
            // barButtonSaveOptions
            // 
            this.barButtonSaveOptions.Caption = "Save";
            this.barButtonSaveOptions.Id = 24;
            this.barButtonSaveOptions.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonSaveOptions.ImageOptions.SvgImage")));
            this.barButtonSaveOptions.Name = "barButtonSaveOptions";
            // 
            // bar2
            // 
            this.bar2.BarName = "Main menu";
            this.bar2.DockCol = 0;
            this.bar2.DockRow = 0;
            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem1),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem2),
            new DevExpress.XtraBars.LinkPersistInfo(this.barSubItem3)});
            this.bar2.OptionsBar.AllowQuickCustomization = false;
            this.bar2.OptionsBar.DrawDragBorder = false;
            this.bar2.OptionsBar.MultiLine = true;
            this.bar2.OptionsBar.UseWholeRow = true;
            this.bar2.Text = "Main menu";
            // 
            // barSubItem1
            // 
            this.barSubItem1.Caption = "&File";
            this.barSubItem1.Id = 7;
            this.barSubItem1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.Caption, this.barButtonExit, "E&xit")});
            this.barSubItem1.Name = "barSubItem1";
            // 
            // barButtonExit
            // 
            this.barButtonExit.Caption = "E&xit";
            this.barButtonExit.Id = 10;
            this.barButtonExit.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("barButtonExit.ImageOptions.SvgImage")));
            this.barButtonExit.Name = "barButtonExit";
            this.barButtonExit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonExit_ItemClick);
            // 
            // barSubItem2
            // 
            this.barSubItem2.Caption = "&Report";
            this.barSubItem2.Id = 8;
            this.barSubItem2.Name = "barSubItem2";
            // 
            // barSubItem3
            // 
            this.barSubItem3.Caption = "&Tools";
            this.barSubItem3.Id = 9;
            this.barSubItem3.Name = "barSubItem3";
            // 
            // bar3
            // 
            this.bar3.BarName = "Status bar";
            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.bar3.DockCol = 0;
            this.bar3.DockRow = 0;
            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticTime),
            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticInfo),
            new DevExpress.XtraBars.LinkPersistInfo(this.barProgress)});
            this.bar3.OptionsBar.AllowQuickCustomization = false;
            this.bar3.OptionsBar.DrawDragBorder = false;
            this.bar3.OptionsBar.UseWholeRow = true;
            this.bar3.Text = "Status bar";
            // 
            // barStaticTime
            // 
            this.barStaticTime.Caption = "barStaticTime";
            this.barStaticTime.Id = 11;
            this.barStaticTime.Name = "barStaticTime";
            this.barStaticTime.Size = new System.Drawing.Size(200, 0);
            this.barStaticTime.Width = 200;
            // 
            // barStaticInfo
            // 
            this.barStaticInfo.Caption = "barStaticInfo";
            this.barStaticInfo.Id = 12;
            this.barStaticInfo.Name = "barStaticInfo";
            // 
            // barProgress
            // 
            this.barProgress.Caption = "barProgress";
            this.barProgress.Edit = this.repositoryItemProgressBar1;
            this.barProgress.EditValue = "0";
            this.barProgress.EditWidth = 150;
            this.barProgress.Id = 13;
            this.barProgress.Name = "barProgress";
            // 
            // repositoryItemProgressBar1
            // 
            this.repositoryItemProgressBar1.FlowAnimationEnabled = true;
            this.repositoryItemProgressBar1.Name = "repositoryItemProgressBar1";
            this.repositoryItemProgressBar1.NullText = "0%";
            this.repositoryItemProgressBar1.ShowTitle = true;
            this.repositoryItemProgressBar1.Step = 1;
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Manager = this.barManager1;
            this.barDockControlTop.Size = new System.Drawing.Size(960, 50);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 551);
            this.barDockControlBottom.Manager = this.barManager1;
            this.barDockControlBottom.Size = new System.Drawing.Size(960, 20);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 50);
            this.barDockControlLeft.Manager = this.barManager1;
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 501);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(960, 50);
            this.barDockControlRight.Manager = this.barManager1;
            this.barDockControlRight.Size = new System.Drawing.Size(0, 501);
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.None;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 50);
            this.splitContainerControl1.Name = "splitContainerControl1";
            // 
            // splitContainerControl1.Panel1
            // 
            this.splitContainerControl1.Panel1.Controls.Add(this.gridConrolMain);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            // 
            // splitContainerControl1.Panel2
            // 
            this.splitContainerControl1.Panel2.Controls.Add(this.grpDetail);
            this.splitContainerControl1.Panel2.Controls.Add(this.groupControl1);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(960, 501);
            this.splitContainerControl1.SplitterPosition = 430;
            this.splitContainerControl1.TabIndex = 4;
            // 
            // gridConrolMain
            // 
            this.gridConrolMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridConrolMain.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(6);
            this.gridConrolMain.Location = new System.Drawing.Point(0, 0);
            this.gridConrolMain.MainView = this.gViewMain;
            this.gridConrolMain.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.gridConrolMain.Name = "gridConrolMain";
            this.gridConrolMain.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit1,
            this.repositoryItemCheckEdit1});
            this.gridConrolMain.Size = new System.Drawing.Size(430, 501);
            this.gridConrolMain.TabIndex = 2;
            this.gridConrolMain.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gViewMain});
            // 
            // gViewMain
            // 
            this.gViewMain.DetailHeight = 548;
            this.gViewMain.GridControl = this.gridConrolMain;
            this.gViewMain.Name = "gViewMain";
            this.gViewMain.OptionsBehavior.AutoPopulateColumns = false;
            this.gViewMain.OptionsBehavior.EditingMode = DevExpress.XtraGrid.Views.Grid.GridEditingMode.EditFormInplace;
            this.gViewMain.OptionsMenu.EnableColumnMenu = false;
            this.gViewMain.OptionsView.ShowGroupPanel = false;
            // 
            // repositoryItemLookUpEdit1
            // 
            this.repositoryItemLookUpEdit1.AutoHeight = false;
            this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // grpDetail
            // 
            this.grpDetail.Controls.Add(this.lblUnit);
            this.grpDetail.Controls.Add(this.grpPeriod);
            this.grpDetail.Controls.Add(this.chkEnable);
            this.grpDetail.Controls.Add(this.grpInterval);
            this.grpDetail.Controls.Add(this.chZip);
            this.grpDetail.Controls.Add(this.labelControl7);
            this.grpDetail.Controls.Add(this.lkpReportDest);
            this.grpDetail.Controls.Add(this.labelControl4);
            this.grpDetail.Controls.Add(this.labelControl5);
            this.grpDetail.Controls.Add(this.tsOffSet);
            this.grpDetail.Controls.Add(this.chCmbHeader);
            this.grpDetail.Controls.Add(this.labelControl6);
            this.grpDetail.Controls.Add(this.deNextEvent);
            this.grpDetail.Controls.Add(this.labelControl3);
            this.grpDetail.Controls.Add(this.btnEditDestinationInfo);
            this.grpDetail.Controls.Add(this.labelControl2);
            this.grpDetail.Controls.Add(this.labelControl1);
            this.grpDetail.Controls.Add(this.txtReportName);
            this.grpDetail.Controls.Add(this.cmbUnit);
            this.grpDetail.Controls.Add(this.lkpReportType);
            this.grpDetail.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpDetail.Enabled = false;
            this.grpDetail.Location = new System.Drawing.Point(0, 0);
            this.grpDetail.Margin = new System.Windows.Forms.Padding(4);
            this.grpDetail.Name = "grpDetail";
            this.grpDetail.Size = new System.Drawing.Size(518, 210);
            this.grpDetail.TabIndex = 2;
            this.grpDetail.Text = "Общие";
            // 
            // lblUnit
            // 
            this.lblUnit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUnit.Location = new System.Drawing.Point(27, 27);
            this.lblUnit.Margin = new System.Windows.Forms.Padding(4);
            this.lblUnit.Name = "lblUnit";
            this.lblUnit.Size = new System.Drawing.Size(28, 13);
            this.lblUnit.TabIndex = 61;
            this.lblUnit.Text = "Блок:";
            // 
            // grpPeriod
            // 
            this.grpPeriod.Controls.Add(this.spinTimeSecs);
            this.grpPeriod.Controls.Add(this.spinTimeMinutes);
            this.grpPeriod.Controls.Add(this.spinTimeHours);
            this.grpPeriod.Controls.Add(this.lookUpTimeFormat);
            this.grpPeriod.Location = new System.Drawing.Point(10, 132);
            this.grpPeriod.Margin = new System.Windows.Forms.Padding(4);
            this.grpPeriod.Name = "grpPeriod";
            this.grpPeriod.Size = new System.Drawing.Size(148, 68);
            this.grpPeriod.TabIndex = 2;
            this.grpPeriod.Text = "Период:";
            // 
            // spinTimeSecs
            // 
            this.spinTimeSecs.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinTimeSecs.Location = new System.Drawing.Point(99, 45);
            this.spinTimeSecs.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.spinTimeSecs.Name = "spinTimeSecs";
            this.spinTimeSecs.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinTimeSecs.Properties.IsFloatValue = false;
            this.spinTimeSecs.Properties.MaskSettings.Set("mask", "d2");
            this.spinTimeSecs.Properties.MaxValue = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.spinTimeSecs.Properties.UseMaskAsDisplayFormat = true;
            this.spinTimeSecs.Size = new System.Drawing.Size(38, 20);
            this.spinTimeSecs.TabIndex = 54;
            // 
            // spinTimeMinutes
            // 
            this.spinTimeMinutes.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinTimeMinutes.Location = new System.Drawing.Point(53, 45);
            this.spinTimeMinutes.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.spinTimeMinutes.Name = "spinTimeMinutes";
            this.spinTimeMinutes.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinTimeMinutes.Properties.IsFloatValue = false;
            this.spinTimeMinutes.Properties.MaskSettings.Set("mask", "d2");
            this.spinTimeMinutes.Properties.MaxValue = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.spinTimeMinutes.Properties.UseMaskAsDisplayFormat = true;
            this.spinTimeMinutes.Size = new System.Drawing.Size(38, 20);
            this.spinTimeMinutes.TabIndex = 53;
            // 
            // spinTimeHours
            // 
            this.spinTimeHours.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinTimeHours.Location = new System.Drawing.Point(8, 45);
            this.spinTimeHours.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.spinTimeHours.Name = "spinTimeHours";
            this.spinTimeHours.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinTimeHours.Properties.IsFloatValue = false;
            this.spinTimeHours.Properties.MaskSettings.Set("mask", "d2");
            this.spinTimeHours.Properties.MaxValue = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.spinTimeHours.Properties.UseMaskAsDisplayFormat = true;
            this.spinTimeHours.Size = new System.Drawing.Size(38, 20);
            this.spinTimeHours.TabIndex = 52;
            // 
            // lookUpTimeFormat
            // 
            this.lookUpTimeFormat.Location = new System.Drawing.Point(8, 22);
            this.lookUpTimeFormat.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.lookUpTimeFormat.Name = "lookUpTimeFormat";
            this.lookUpTimeFormat.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpTimeFormat.Properties.PopupWidthMode = DevExpress.XtraEditors.PopupWidthMode.ContentWidth;
            this.lookUpTimeFormat.Properties.ShowFooter = false;
            this.lookUpTimeFormat.Properties.ShowHeader = false;
            this.lookUpTimeFormat.Size = new System.Drawing.Size(129, 20);
            this.lookUpTimeFormat.TabIndex = 22;
            // 
            // chkEnable
            // 
            this.chkEnable.Location = new System.Drawing.Point(382, 146);
            this.chkEnable.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.chkEnable.Name = "chkEnable";
            this.chkEnable.Properties.AutoWidth = true;
            this.chkEnable.Properties.Caption = "Авто";
            this.chkEnable.Size = new System.Drawing.Size(46, 19);
            this.chkEnable.TabIndex = 31;
            // 
            // grpInterval
            // 
            this.grpInterval.Controls.Add(this.cmbFractionOfSec);
            this.grpInterval.Controls.Add(this.spinSampleSecs);
            this.grpInterval.Controls.Add(this.spinSampleMins);
            this.grpInterval.Controls.Add(this.spinSampleHours);
            this.grpInterval.Controls.Add(this.lookUpSample);
            this.grpInterval.Location = new System.Drawing.Point(166, 132);
            this.grpInterval.Margin = new System.Windows.Forms.Padding(4);
            this.grpInterval.Name = "grpInterval";
            this.grpInterval.Size = new System.Drawing.Size(207, 68);
            this.grpInterval.TabIndex = 3;
            this.grpInterval.Text = "Интервал";
            // 
            // cmbFractionOfSec
            // 
            this.cmbFractionOfSec.Location = new System.Drawing.Point(145, 45);
            this.cmbFractionOfSec.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.cmbFractionOfSec.MinimumSize = new System.Drawing.Size(32, 0);
            this.cmbFractionOfSec.Name = "cmbFractionOfSec";
            this.cmbFractionOfSec.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbFractionOfSec.Properties.DropDownRows = 9;
            this.cmbFractionOfSec.Properties.Items.AddRange(new object[] {
            "0.1",
            "0.2",
            "0.3",
            "0.4",
            "0.5",
            "0.6",
            "0.7",
            "0.8",
            "0.9"});
            this.cmbFractionOfSec.Properties.PopupFormSize = new System.Drawing.Size(80, 0);
            this.cmbFractionOfSec.Size = new System.Drawing.Size(50, 20);
            this.cmbFractionOfSec.TabIndex = 51;
            // 
            // spinSampleSecs
            // 
            this.spinSampleSecs.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinSampleSecs.Location = new System.Drawing.Point(99, 45);
            this.spinSampleSecs.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.spinSampleSecs.Name = "spinSampleSecs";
            this.spinSampleSecs.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinSampleSecs.Properties.IsFloatValue = false;
            this.spinSampleSecs.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.spinSampleSecs.Properties.MaskSettings.Set("mask", "d2");
            this.spinSampleSecs.Properties.MaxValue = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.spinSampleSecs.Size = new System.Drawing.Size(38, 20);
            this.spinSampleSecs.TabIndex = 50;
            // 
            // spinSampleMins
            // 
            this.spinSampleMins.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinSampleMins.Location = new System.Drawing.Point(53, 45);
            this.spinSampleMins.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.spinSampleMins.Name = "spinSampleMins";
            this.spinSampleMins.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinSampleMins.Properties.IsFloatValue = false;
            this.spinSampleMins.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.spinSampleMins.Properties.MaskSettings.Set("mask", "d2");
            this.spinSampleMins.Properties.MaxValue = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.spinSampleMins.Size = new System.Drawing.Size(38, 20);
            this.spinSampleMins.TabIndex = 49;
            // 
            // spinSampleHours
            // 
            this.spinSampleHours.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.spinSampleHours.Location = new System.Drawing.Point(8, 45);
            this.spinSampleHours.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.spinSampleHours.Name = "spinSampleHours";
            this.spinSampleHours.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.spinSampleHours.Properties.IsFloatValue = false;
            this.spinSampleHours.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.spinSampleHours.Properties.MaskSettings.Set("mask", "d2");
            this.spinSampleHours.Properties.MaxValue = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.spinSampleHours.Size = new System.Drawing.Size(38, 20);
            this.spinSampleHours.TabIndex = 48;
            // 
            // lookUpSample
            // 
            this.lookUpSample.Location = new System.Drawing.Point(8, 21);
            this.lookUpSample.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.lookUpSample.Name = "lookUpSample";
            this.lookUpSample.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lookUpSample.Properties.PopupWidth = 31;
            this.lookUpSample.Properties.PopupWidthMode = DevExpress.XtraEditors.PopupWidthMode.UseEditorWidth;
            this.lookUpSample.Properties.ShowFooter = false;
            this.lookUpSample.Properties.ShowHeader = false;
            this.lookUpSample.Size = new System.Drawing.Size(188, 20);
            this.lookUpSample.TabIndex = 24;
            // 
            // chZip
            // 
            this.chZip.Location = new System.Drawing.Point(382, 172);
            this.chZip.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.chZip.Name = "chZip";
            this.chZip.Properties.Caption = "Архив ZIP";
            this.chZip.Size = new System.Drawing.Size(71, 19);
            this.chZip.TabIndex = 57;
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(320, 106);
            this.labelControl7.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(33, 13);
            this.labelControl7.TabIndex = 59;
            this.labelControl7.Text = "Сдвиг:";
            // 
            // lkpReportDest
            // 
            this.lkpReportDest.Location = new System.Drawing.Point(362, 80);
            this.lkpReportDest.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.lkpReportDest.Name = "lkpReportDest";
            this.lkpReportDest.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkpReportDest.Properties.PopupWidth = 79;
            this.lkpReportDest.Properties.PopupWidthMode = DevExpress.XtraEditors.PopupWidthMode.ContentWidth;
            this.lkpReportDest.Properties.ShowFooter = false;
            this.lkpReportDest.Properties.ShowHeader = false;
            this.lkpReportDest.Size = new System.Drawing.Size(145, 20);
            this.lkpReportDest.TabIndex = 19;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(307, 83);
            this.labelControl4.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(45, 13);
            this.labelControl4.TabIndex = 20;
            this.labelControl4.Text = "Формат:";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(10, 83);
            this.labelControl5.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(57, 13);
            this.labelControl5.TabIndex = 56;
            this.labelControl5.Text = "Заголовки:";
            // 
            // tsOffSet
            // 
            this.tsOffSet.EditValue = System.TimeSpan.Parse("00:00:00");
            this.tsOffSet.Location = new System.Drawing.Point(362, 106);
            this.tsOffSet.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.tsOffSet.Name = "tsOffSet";
            this.tsOffSet.Properties.AllowEditDays = false;
            this.tsOffSet.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.tsOffSet.Properties.MaxDays = 0;
            this.tsOffSet.Properties.TimeEditStyle = DevExpress.XtraEditors.Repository.TimeEditStyle.SpinButtons;
            this.tsOffSet.Size = new System.Drawing.Size(145, 20);
            this.tsOffSet.TabIndex = 33;
            // 
            // chCmbHeader
            // 
            this.chCmbHeader.EditValue = "";
            this.chCmbHeader.Location = new System.Drawing.Point(82, 80);
            this.chCmbHeader.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.chCmbHeader.Name = "chCmbHeader";
            this.chCmbHeader.Properties.AllowMultiSelect = true;
            this.chCmbHeader.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.chCmbHeader.Properties.DropDownRows = 3;
            this.chCmbHeader.Properties.SelectAllItemVisible = false;
            this.chCmbHeader.Size = new System.Drawing.Size(203, 20);
            this.chCmbHeader.TabIndex = 55;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(10, 110);
            this.labelControl6.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(69, 13);
            this.labelControl6.TabIndex = 58;
            this.labelControl6.Text = "След. запуск:";
            // 
            // deNextEvent
            // 
            this.deNextEvent.EditValue = null;
            this.deNextEvent.Location = new System.Drawing.Point(82, 108);
            this.deNextEvent.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.deNextEvent.MinimumSize = new System.Drawing.Size(202, 0);
            this.deNextEvent.Name = "deNextEvent";
            this.deNextEvent.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deNextEvent.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.deNextEvent.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.deNextEvent.Properties.MaskSettings.Set("mask", "G");
            this.deNextEvent.Size = new System.Drawing.Size(202, 20);
            this.deNextEvent.TabIndex = 37;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(8, 54);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(56, 13);
            this.labelControl3.TabIndex = 18;
            this.labelControl3.Text = "Сохранить:";
            // 
            // btnEditDestinationInfo
            // 
            this.btnEditDestinationInfo.Location = new System.Drawing.Point(82, 51);
            this.btnEditDestinationInfo.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.btnEditDestinationInfo.Name = "btnEditDestinationInfo";
            this.btnEditDestinationInfo.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.btnEditDestinationInfo.Size = new System.Drawing.Size(203, 20);
            this.btnEditDestinationInfo.TabIndex = 17;
            // 
            // labelControl2
            // 
            this.labelControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelControl2.Location = new System.Drawing.Point(295, 55);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(58, 13);
            this.labelControl2.TabIndex = 16;
            this.labelControl2.Text = "Тип отчета:";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(147, 27);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(4);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(53, 13);
            this.labelControl1.TabIndex = 14;
            this.labelControl1.Text = "Название:";
            // 
            // txtReportName
            // 
            this.txtReportName.EditValue = "";
            this.txtReportName.Location = new System.Drawing.Point(209, 23);
            this.txtReportName.Margin = new System.Windows.Forms.Padding(6, 2, 6, 2);
            this.txtReportName.Name = "txtReportName";
            this.txtReportName.Size = new System.Drawing.Size(298, 20);
            this.txtReportName.TabIndex = 13;
            // 
            // cmbUnit
            // 
            this.cmbUnit.EditValue = "";
            this.cmbUnit.Location = new System.Drawing.Point(82, 23);
            this.cmbUnit.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.cmbUnit.Name = "cmbUnit";
            this.cmbUnit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbUnit.Properties.DropDownRows = 8;
            this.cmbUnit.Properties.Items.AddRange(new object[] {
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"});
            this.cmbUnit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.cmbUnit.Size = new System.Drawing.Size(45, 20);
            this.cmbUnit.TabIndex = 62;
            // 
            // lkpReportType
            // 
            this.lkpReportType.Location = new System.Drawing.Point(362, 53);
            this.lkpReportType.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.lkpReportType.Name = "lkpReportType";
            this.lkpReportType.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFit;
            this.lkpReportType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkpReportType.Size = new System.Drawing.Size(145, 20);
            this.lkpReportType.TabIndex = 15;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.gridControlDetail);
            this.groupControl1.Controls.Add(this.btnImportPoints);
            this.groupControl1.Controls.Add(this.btnGetDesc);
            this.groupControl1.Controls.Add(this.btnExportPoints);
            this.groupControl1.Controls.Add(this.btnCancel);
            this.groupControl1.Controls.Add(this.btnDownPoint);
            this.groupControl1.Controls.Add(this.btnSave);
            this.groupControl1.Controls.Add(this.btnUpPoint);
            this.groupControl1.Controls.Add(this.btnRemovePoint);
            this.groupControl1.Controls.Add(this.btnAddPoint);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupControl1.Location = new System.Drawing.Point(0, 207);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(518, 294);
            this.groupControl1.TabIndex = 74;
            // 
            // gridControlDetail
            // 
            this.gridControlDetail.AllowDrop = true;
            this.gridControlDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControlDetail.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(6);
            this.gridControlDetail.Location = new System.Drawing.Point(10, 11);
            this.gridControlDetail.MainView = this.gViewDetail;
            this.gridControlDetail.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.gridControlDetail.Name = "gridControlDetail";
            this.gridControlDetail.Size = new System.Drawing.Size(462, 235);
            this.gridControlDetail.TabIndex = 40;
            this.gridControlDetail.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gViewDetail});
            // 
            // gViewDetail
            // 
            this.gViewDetail.DetailHeight = 548;
            this.gViewDetail.GridControl = this.gridControlDetail;
            this.gViewDetail.Name = "gViewDetail";
            this.gViewDetail.OptionsBehavior.Editable = false;
            this.gViewDetail.OptionsView.ShowGroupPanel = false;
            // 
            // btnImportPoints
            // 
            this.btnImportPoints.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportPoints.Enabled = false;
            this.btnImportPoints.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnImportPoints.ImageOptions.SvgImage")));
            this.btnImportPoints.Location = new System.Drawing.Point(478, 211);
            this.btnImportPoints.Margin = new System.Windows.Forms.Padding(2);
            this.btnImportPoints.Name = "btnImportPoints";
            this.btnImportPoints.Size = new System.Drawing.Size(36, 36);
            this.btnImportPoints.TabIndex = 73;
            this.btnImportPoints.ToolTip = "Import points";
            // 
            // btnGetDesc
            // 
            this.btnGetDesc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGetDesc.Enabled = false;
            this.btnGetDesc.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnGetDesc.ImageOptions.SvgImage")));
            this.btnGetDesc.Location = new System.Drawing.Point(8, 258);
            this.btnGetDesc.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.btnGetDesc.Name = "btnGetDesc";
            this.btnGetDesc.Size = new System.Drawing.Size(166, 29);
            this.btnGetDesc.TabIndex = 62;
            this.btnGetDesc.Text = "Получить наименования";
            // 
            // btnExportPoints
            // 
            this.btnExportPoints.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportPoints.Enabled = false;
            this.btnExportPoints.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnExportPoints.ImageOptions.SvgImage")));
            this.btnExportPoints.Location = new System.Drawing.Point(478, 171);
            this.btnExportPoints.Margin = new System.Windows.Forms.Padding(2);
            this.btnExportPoints.Name = "btnExportPoints";
            this.btnExportPoints.Size = new System.Drawing.Size(36, 36);
            this.btnExportPoints.TabIndex = 72;
            this.btnExportPoints.ToolTip = "Export points";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Enabled = false;
            this.btnCancel.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnCancel.ImageOptions.SvgImage")));
            this.btnCancel.Location = new System.Drawing.Point(407, 258);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(107, 29);
            this.btnCancel.TabIndex = 64;
            this.btnCancel.Text = "Отмена";
            // 
            // btnDownPoint
            // 
            this.btnDownPoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDownPoint.Enabled = false;
            this.btnDownPoint.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnDownPoint.ImageOptions.SvgImage")));
            this.btnDownPoint.Location = new System.Drawing.Point(478, 131);
            this.btnDownPoint.Margin = new System.Windows.Forms.Padding(2);
            this.btnDownPoint.Name = "btnDownPoint";
            this.btnDownPoint.Size = new System.Drawing.Size(36, 36);
            this.btnDownPoint.TabIndex = 71;
            this.btnDownPoint.ToolTip = "Move down point";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Enabled = false;
            this.btnSave.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnSave.ImageOptions.SvgImage")));
            this.btnSave.Location = new System.Drawing.Point(295, 258);
            this.btnSave.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(107, 29);
            this.btnSave.TabIndex = 63;
            this.btnSave.Text = "Сохранить";
            // 
            // btnUpPoint
            // 
            this.btnUpPoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpPoint.Enabled = false;
            this.btnUpPoint.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnUpPoint.ImageOptions.SvgImage")));
            this.btnUpPoint.Location = new System.Drawing.Point(478, 91);
            this.btnUpPoint.Margin = new System.Windows.Forms.Padding(2);
            this.btnUpPoint.Name = "btnUpPoint";
            this.btnUpPoint.Size = new System.Drawing.Size(36, 36);
            this.btnUpPoint.TabIndex = 70;
            this.btnUpPoint.ToolTip = "Move up point";
            // 
            // btnRemovePoint
            // 
            this.btnRemovePoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemovePoint.Enabled = false;
            this.btnRemovePoint.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnRemovePoint.ImageOptions.SvgImage")));
            this.btnRemovePoint.Location = new System.Drawing.Point(478, 51);
            this.btnRemovePoint.Margin = new System.Windows.Forms.Padding(2);
            this.btnRemovePoint.Name = "btnRemovePoint";
            this.btnRemovePoint.Size = new System.Drawing.Size(36, 36);
            this.btnRemovePoint.TabIndex = 69;
            this.btnRemovePoint.ToolTip = "Remove point";
            // 
            // btnAddPoint
            // 
            this.btnAddPoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddPoint.Enabled = false;
            this.btnAddPoint.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnAddPoint.ImageOptions.SvgImage")));
            this.btnAddPoint.Location = new System.Drawing.Point(478, 11);
            this.btnAddPoint.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddPoint.Name = "btnAddPoint";
            this.btnAddPoint.Size = new System.Drawing.Size(36, 36);
            this.btnAddPoint.TabIndex = 68;
            this.btnAddPoint.ToolTip = "Add point";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.Name = "sqlDataSource1";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(960, 571);
            this.Controls.Add(this.splitContainerControl1);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("FrmMain.IconOptions.Image")));
            this.Name = "FrmMain";
            this.Text = "frmMain";
            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemProgressBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel1)).EndInit();
            this.splitContainerControl1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1.Panel2)).EndInit();
            this.splitContainerControl1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridConrolMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gViewMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpDetail)).EndInit();
            this.grpDetail.ResumeLayout(false);
            this.grpDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpPeriod)).EndInit();
            this.grpPeriod.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spinTimeSecs.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinTimeMinutes.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinTimeHours.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpTimeFormat.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkEnable.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grpInterval)).EndInit();
            this.grpInterval.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbFractionOfSec.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinSampleSecs.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinSampleMins.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spinSampleHours.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lookUpSample.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chZip.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpReportDest.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tsOffSet.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chCmbHeader.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deNextEvent.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deNextEvent.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnEditDestinationInfo.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtReportName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUnit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkpReportType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gViewDetail)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarManager barManager1;
        private DevExpress.XtraBars.Bar bar1;
        private DevExpress.XtraBars.Bar bar2;
        private DevExpress.XtraBars.Bar bar3;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarSubItem barSubItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonExit;
        private DevExpress.XtraBars.BarSubItem barSubItem2;
        private DevExpress.XtraBars.BarSubItem barSubItem3;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraGrid.GridControl gridConrolMain;
        private DevExpress.XtraGrid.Views.Grid.GridView gViewMain;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraBars.BarStaticItem barStaticTime;
        private DevExpress.XtraBars.BarStaticItem barStaticInfo;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraEditors.GroupControl grpDetail;
        private DevExpress.XtraEditors.LabelControl lblUnit;
        private DevExpress.XtraEditors.GroupControl grpPeriod;
        private DevExpress.XtraEditors.SpinEdit spinTimeSecs;
        private DevExpress.XtraEditors.SpinEdit spinTimeMinutes;
        private DevExpress.XtraEditors.SpinEdit spinTimeHours;
        private DevExpress.XtraEditors.LookUpEdit lookUpTimeFormat;
        private DevExpress.XtraEditors.CheckEdit chkEnable;
        private DevExpress.XtraEditors.GroupControl grpInterval;
        private DevExpress.XtraEditors.ComboBoxEdit cmbFractionOfSec;
        private DevExpress.XtraEditors.SpinEdit spinSampleSecs;
        private DevExpress.XtraEditors.SpinEdit spinSampleMins;
        private DevExpress.XtraEditors.SpinEdit spinSampleHours;
        private DevExpress.XtraEditors.LookUpEdit lookUpSample;
        private DevExpress.XtraEditors.CheckEdit chZip;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LookUpEdit lkpReportDest;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.TimeSpanEdit tsOffSet;
        private DevExpress.XtraEditors.CheckedComboBoxEdit chCmbHeader;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.DateEdit deNextEvent;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.ButtonEdit btnEditDestinationInfo;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txtReportName;
        private DevExpress.XtraEditors.ComboBoxEdit cmbUnit;
        private DevExpress.XtraEditors.LookUpEdit lkpReportType;
        private DevExpress.XtraGrid.GridControl gridControlDetail;
        private DevExpress.XtraGrid.Views.Grid.GridView gViewDetail;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnGetDesc;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnImportPoints;
        private DevExpress.XtraEditors.SimpleButton btnExportPoints;
        private DevExpress.XtraEditors.SimpleButton btnDownPoint;
        private DevExpress.XtraEditors.SimpleButton btnRemovePoint;
        private DevExpress.XtraEditors.SimpleButton btnAddPoint;
        private DevExpress.XtraEditors.SimpleButton btnUpPoint;
        private DevExpress.XtraBars.BarEditItem barProgress;
        private DevExpress.XtraEditors.Repository.RepositoryItemProgressBar repositoryItemProgressBar1;
        private DevExpress.XtraBars.BarButtonItem barButtonNew;
        private DevExpress.XtraBars.BarButtonItem barButtonEdit;
        private DevExpress.XtraBars.BarButtonItem barButtonCopy;
        private DevExpress.XtraBars.BarButtonItem barButtonDelete;
        private DevExpress.XtraBars.BarButtonItem barButtonGenerate;
        private DevExpress.XtraBars.BarButtonItem barButtonTimerStart;
        private DevExpress.XtraBars.BarButtonItem barButtonTimerStop;
        private DevExpress.XtraBars.BarEditItem barSpinCycle;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEdit2;
        private DevExpress.XtraBars.BarButtonItem barButtonSaveOptions;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource1;
    }
}