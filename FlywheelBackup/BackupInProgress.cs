using io.rz.Flywheel.BackupLib.Jobs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FlywheelBackup
{
    public partial class BackupInProgress : Form
    {
        private JobExecutor executor;
        bool done = false;
        public BackupInProgress(JobExecutor executor)
        {
            this.executor = executor;
            this.executor.JobDone += executor_JobDone;
            InitializeComponent();

        }

        void executor_JobDone(object sender, EventArgs e)
        {
            done = true;
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            if (done)
            {
                lblStatus.Text = "Done";
                btnOk.Visible = true;
            }
            else
                lblStatus.Text = string.Format("Backing up {0} files",executor.RunningTasks);

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            
        }
    }
}
