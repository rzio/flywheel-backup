using io.rz.Flywheel.BackupLib;
using io.rz.Flywheel.BackupLib.Backup;
using io.rz.Flywheel.BackupLib.Backup.Processors;
using io.rz.Flywheel.BackupLib.Jobs;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FlywheelBackup
{
    public partial class InitialForm : Form
    {
        public InitialForm()
        {
            InitializeComponent();
        }

        private void btnBrowseFolder_Click(object sender, EventArgs e)
        {
            var result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtFolder.Text = folderBrowserDialog.SelectedPath;
            }


        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            string connectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", txtAccount.Text, txtKey.Text);
            BackupProcessorChain chain = new BackupProcessorChain(
                new List<Processor<BackupItem>>()
                {
                    new FileLoadingBackupProcessor(),
                    new GZipBackupProcessor(),
                    new SHA1NamingBackupProcessor(),
                    new AzureUploaderBackupProcessor(connectionString,"flywheel")
                });

            //chain.Backup(new DirectoryBackupTask(txtFolder.Text,false));

            var executor = new JobExecutor();
            executor.SubmitBackupJob(new BackupJob(@"C:\data\Resources", true,chain, "metadata.xml"));
            BackupInProgress b = new BackupInProgress(executor);
            b.ShowDialog();
            
        }



    }
}
