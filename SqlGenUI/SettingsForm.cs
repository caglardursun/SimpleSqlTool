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
        private AppSettings appSettings;
        private const string appName = "SqlGenUI";
        private const string settingFileName = "Settings.json";
        private string fileSettingsPath;

        public SettingsForm()
        {
            InitializeComponent();
            appSettings = new AppSettings();
            Initialize();
        }

        private void Initialize()
        {
            string folderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), appName);
            if(!Directory.Exists(folderPath))            
                Directory.CreateDirectory(folderPath);
            fileSettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), appName, "Settings.json");


            if (File.Exists(fileSettingsPath))                            
                JsonConvert.DeserializeObject<AppSettings>(fileSettingsPath);

            textBoxApiPath.Text = appSettings.APIPath.EmptyCheck();
            textBoxDB.Text = appSettings.DefaultDB.EmptyCheck();
            textBoxPassword.Text = appSettings.Password.EmptyCheck();
            textBoxUserName.Text = appSettings.UserName.EmptyCheck();
            textBoxServer.Text = appSettings.ServerName.EmptyCheck();
            

        }


        private void buttonOpenFolderDialog_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if(folderBrowserDialog.ShowDialog() == DialogResult.OK)
                appSettings.APIPath = textBoxApiPath.Text = folderBrowserDialog.SelectedPath;                
            
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            appSettings.APIPath = textBoxApiPath.Text;
            appSettings.DefaultDB= textBoxDB.Text;
            appSettings.Password= textBoxPassword.Text;
            appSettings.UserName= textBoxUserName.Text;
            appSettings.ServerName= textBoxServer.Text;

            string output = JsonConvert.SerializeObject(appSettings);
            using (StreamWriter sw = new StreamWriter(fileSettingsPath))
            {
                sw.Write(output);
            }
            Close();
        }
    }
}
