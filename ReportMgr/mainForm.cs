using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraBars.Ribbon;


namespace ReportMgr
{
    public partial class mainForm : RibbonForm
    {
        public mainForm()
        {
            InitializeComponent();
        }

        int period = 10;
        int cycle = 0;
        bool enbl = false;
        DataContext db = new DataContext();
        List<ReportDefinition> reportDefinitions = new List<ReportDefinition>(); 

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (enbl) return;
            if (cycle <= period)
            {
                barStaticItemTime.Caption = (period - cycle).ToString();
                cycle++;
            }
            else
            {
                cycle = 0;
            }
            if (cycle == 0)
            {
                barStaticItemInfo.Caption = "Generate report....";
                enbl = true;
                worker.RunWorkerAsync();
                
            }
            
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 1; i <= 10; i++)
            {
                System.Threading.Thread.Sleep(1000);
                worker.ReportProgress(i);
            }
           
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            barPb.EditValue = e.ProgressPercentage*10;
            repositoryItemProgressBar1.Maximum = 100;

            
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            barStaticItemInfo.Caption = "Generate report.... DONE!";
            enbl = false;
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            timer1.Start();
        }


        private void mainForm_Load(object sender, EventArgs e)
        {
            barStaticItemInfo.Caption = "";
            BindDefinitions();
           
            
        }

        private void BindDefinitions()
        {
            reportDefinitions = db.ReportDefinitions.ToList();
            gridControl1.DataSource = reportDefinitions;
           
        }

    }
}
