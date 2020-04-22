using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlGenUI
{

    public partial class SettingsForm : Form
    {                

        public SettingsForm()
        {
            InitializeComponent();
            //appSettings = new AppSettings();
            Initialize();
        }

        private void Initialize()
        {
         

            var settings = AppSettings.Instance;
            textBoxApiPath.Text = settings.APIPath?.EmptyCheck();
            textBoxDB.Text = settings.DefaultDB.EmptyCheck();
            textBoxPassword.Text = settings.Password.EmptyCheck();
            textBoxUserName.Text = settings.UserName.EmptyCheck();
            textBoxServer.Text = settings.ServerName.EmptyCheck();

            comboBoxDB.SelectedIndex = 0;
        }


        private void buttonOpenFolderDialog_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            
            if(folderBrowserDialog.ShowDialog() == DialogResult.OK)
                AppSettings.Instance.APIPath = textBoxApiPath.Text = folderBrowserDialog.SelectedPath;                
            
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            var settings = AppSettings.Instance;
            settings.APIPath = textBoxApiPath.Text;
            settings.DefaultDB= textBoxDB.Text;
            settings.Password= textBoxPassword.Text;
            settings.UserName= textBoxUserName.Text;
            settings.ServerName= textBoxServer.Text;
            settings.DBType = (string) comboBoxDB.SelectedItem;
            settings.Save();
            
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
