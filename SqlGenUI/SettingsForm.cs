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

    public static class StringHelper
    {
        public static string EmptyCheck(this string variable)
        {
            return variable == null ? "" : variable;
        }
    }

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
            textBoxApiPath.Text = settings.APIPath.EmptyCheck();
            textBoxDB.Text = settings.DefaultDB.EmptyCheck();
            textBoxPassword.Text = settings.Password.EmptyCheck();
            textBoxUserName.Text = settings.UserName.EmptyCheck();
            textBoxServer.Text = settings.ServerName.EmptyCheck();

            
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
            settings.Save();
            
            Close();
        }
    }
}
