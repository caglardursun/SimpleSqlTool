using SqlGen;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace SqlGenUI
{
    public partial class MainForm : Form
    {
        List<Table> _allTables;

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            List<string> connectionStrings = new List<string>();
            connectionStrings.Add(AppSettings.Instance.ServerName);



            //serverToolStripMenuItem.DropDownItems.AddRange(
            //        //ConfigurationManager.ConnectionStrings
            //        //    .Cast<ConnectionStringSettings>()
            //        //    .Select(cs => new ToolStripMenuItem(cs.Name, null, ServerChanged) { Tag = cs, Checked = cs.Name == "local" })
            //        //    .ToArray<ToolStripItem>()

            //        connectionStrings
            //        .Select(cs => new ToolStripMenuItem(cs.ToString() ,null, ServerChanged) { Tag = cs, Checked = cs.ToString() == "local" })
            //        .ToArray<ToolStripItem>()

            //);

            ResizeListHeaders();
            RefreshCodeGenerators();

            RefreshFromDb();
        }

        private void ResizeListHeaders()
        {
            tableList.Columns[0].Width = tableList.Width - 4 - SystemInformation.VerticalScrollBarWidth;
            fkList.Columns[0].Width = fkList.Width - 4 - SystemInformation.VerticalScrollBarWidth;
            codeList.Columns[0].Width = codeList.Width - 4 - SystemInformation.VerticalScrollBarWidth;
        }



        private void RefreshCodeGenerators()
        {
            GC.KeepAlive(typeof(Generator));

            var generators = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(asm => asm.GetTypes())
                .Where(t => !t.IsAbstract && typeof(Generator).IsAssignableFrom(t))
                .Select(t => Activator.CreateInstance(t))
                .Select(gen => new ListViewItem { Text = gen.ToString(), Tag = gen })
                .OrderBy(lvi => lvi.Text);

            codeList.Items.Clear();
            codeList.Items.AddRange(generators.ToArray());
        }

        private void RefreshFromDb()
        {
            Cursor = Cursors.AppStarting;

            tableList.Items.Clear();
            var database = CheckedDatabase;
            var _settings = AppSettings.Instance;

            if (database == null)
            {


                _ = RunLoadDatabasesAsync(_settings.ConnectionString);
                _ = RunLoadTablesAsync(_settings.ConnectionString);
            }
            else
            {
                var b = new SqlConnectionStringBuilder(_settings.ConnectionString);
                b["Database"] = database;
                _ = RunLoadTablesAsync(b.ToString());
            }

            toolStripStatusLabel1.Text = new SqlConnectionStringBuilder(_settings.ConnectionString) { Password = "xxx" }.ToString();
        }

        async Task RunLoadDatabasesAsync(string connectionString)
        {
            _ = Task.Yield();

            using (var cnn = new SqlConnection(connectionString))
            {
                var da = new TableDataAccess(cnn);
                var databases = await da.ListDatabases();
                BeginInvoke((Action<List<string>, string>)PopulateDatabases, databases, await da.CurrentDatabase());
            }
        }

        void PopulateDatabases(List<string> databases, string currrentDB)
        {
            databaseToolStripMenuItem.DropDownItems.Clear();
            databaseToolStripMenuItem.DropDownItems.AddRange(databases.Select(db => new ToolStripMenuItem(db, null, database_OnClick) { Checked = db == currrentDB }).ToArray());
        }

        void database_OnClick(object sender, EventArgs args)
        {
            CheckedDatabase = ((ToolStripMenuItem)sender).Text;
            RefreshFromDb(/*CheckedConnectionString()*/);
        }

        async Task RunLoadTablesAsync(string connectionString)
        {
            Task.Yield();

            using (var cnn = new SqlConnection(connectionString))
            {
                var da = new TableDataAccess(cnn);
                var tables = await da.LoadNonAuditTable();
                BeginInvoke((Action<List<Table>>)PopulateTableList, tables);
            }
        }

        void PopulateTableList(List<Table> tables)
        {
            _allTables = tables;
            var filter = CheckedSchema;
            var schemas = tables.Select(t => t.Schema).Distinct();
            schemaToolStripMenuItem.DropDownItems.Clear();
            schemaToolStripMenuItem.DropDownItems.AddRange(schemas.Select(s => new ToolStripMenuItem(s, null, schema_OnClick) { Checked = s == filter }).ToArray());
            PopulateTableList();
            sqlTextBox.Text = "";
            Cursor = Cursors.Default;
        }

        private void PopulateTableList()
        {
            var visible = CheckedSchema == null ? _allTables : _allTables.Where(t => t.Schema == CheckedSchema);
            tableList.Items.Clear();
            tableList.Items.AddRange(visible.Select(t => new ListViewItem(t.ToString()) { Tag = t }).ToArray());
            tableList.EnsureVisible(0);
        }

        void schema_OnClick(object sender, EventArgs args)
        {
            CheckedSchema = ((ToolStripMenuItem)sender).Text;
            PopulateTableList();
        }

        private void GenerateSql()
        {
            sqlTextBox.Text = "";
            Cursor = Cursors.AppStarting;
            try
            {
                var gen = new MultiGenerator(CurrentConnectionStringAndDatabase())
                {
                    Options = new GeneratorOptions
                    {
                        Alter = alterStoredProcsToolStripMenuItem.Checked,
                        Grant = addGrantToolStripMenuItem.Checked,
                        Audit = includeAuditColumnsToolStripMenuItem.Checked
                    }
                };
                sqlTextBox.Text = gen.Generate(SelectedTables(), SelectedKeys(), SelectedCodeGenerators());
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private string CurrentConnectionStringAndDatabase()
        {
            if (CheckedDatabase == null)
            {
                return AppSettings.Instance.ConnectionString;
            }

            var b = new SqlConnectionStringBuilder(AppSettings.Instance.ConnectionString);
            b["Database"] = CheckedDatabase;
            return b.ToString();
        }

        string CheckedDatabase
        {
            get { return databaseToolStripMenuItem.DropDownItems.OfType<ToolStripMenuItem>().FirstOrDefault(mi => mi.Checked)?.Text; }
            set
            {
                foreach (var mi in databaseToolStripMenuItem.DropDownItems.OfType<ToolStripMenuItem>())
                {
                    mi.Checked = mi.Text == value ? !mi.Checked : false;
                }
            }
        }

        string CheckedSchema
        {
            get { return schemaToolStripMenuItem.DropDownItems.OfType<ToolStripMenuItem>().FirstOrDefault(mi => mi.Checked)?.Text; }
            set
            {
                foreach (var mi in schemaToolStripMenuItem.DropDownItems.OfType<ToolStripMenuItem>())
                {
                    mi.Checked = mi.Text == value ? !mi.Checked : false;
                }
            }
        }

        private IEnumerable<Generator> SelectedCodeGenerators() => codeList.SelectedItems.Cast<ListViewItem>().Select(lvi => (Generator)lvi.Tag);

        private IEnumerable<Table> SelectedTables() => tableList.SelectedItems.Cast<ListViewItem>().Select(lvi => (Table)lvi.Tag);

        private IEnumerable<TableKey> SelectedKeys() => fkList.SelectedItems.Cast<ListViewItem>().Select(lvi => (TableKey)lvi.Tag);



        private void sqlTextBox_MouseDown(object sender, MouseEventArgs e)
        {
            sqlTextBox.DoDragDrop(sqlTextBox.Text, DragDropEffects.Copy);
        }

        private void List_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender == tableList)
            {
                _ = LoadForeignKeysAsync(SelectedTables().FirstOrDefault());
            }

            GenerateSql();
        }

        private async Task LoadForeignKeysAsync(Table table)
        {
            Cursor = Cursors.AppStarting;
            fkList.Items.Clear();
            if (table == null)
            {
                return;
            }

            await Task.Yield();
            // ConnectionStringSettings connectionSettings = CheckedConnectionString();
            table.EnsureFullyPopulated(AppSettings.Instance.ConnectionString);
            BeginInvoke((Action<Table>)PopulateForeignKeyList, table);
        }

        void PopulateForeignKeyList(Table table)
        {
            fkList.Items.Clear();
            fkList.Items.Add(new ListViewItem { Text = table.PrimaryKey.ConstraintName, Tag = table.PrimaryKey });
            fkList.Items.AddRange(table.ForeignKeys.Select(fk => new ListViewItem { Text = fk.ConstraintName, Tag = fk }).ToArray());
            Cursor = Cursors.Default;
        }

        private void addGrantToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addGrantToolStripMenuItem.Checked = !addGrantToolStripMenuItem.Checked;
            GenerateSql();
        }

        private void tableLayoutPanel1_SizeChanged(object sender, EventArgs e)
        {
            ResizeListHeaders();
        }

        //private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    var menu = serverToolStripMenuItem.DropDownItems.OfType<ToolStripMenuItem>().Single(mi => mi.Checked);
        //    RefreshFromDb(/*(ConnectionStringSettings)menu.Tag*/);
        //}

        private void codeList_MouseDown(object sender, MouseEventArgs e)
        {
            sqlTextBox.DoDragDrop(sqlTextBox.Text, DragDropEffects.Copy);
        }

        private void alterStoredProcsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            alterStoredProcsToolStripMenuItem.Checked = !alterStoredProcsToolStripMenuItem.Checked;
            GenerateSql();
        }

        private void includeAuditColumnsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            includeAuditColumnsToolStripMenuItem.Checked = !includeAuditColumnsToolStripMenuItem.Checked;
            GenerateSql();
        }

        private string GetTableName()
        {
            return tableList.SelectedItems[0].Text.Split('.')[1];
        }


        private void Save([CallerMemberName] string memberName = "",
                        [CallerFilePath] string sourceFilePath = "",
                        [CallerLineNumber] int sourceLineNumber = 0)
        {
            var selected_item = codeList.SelectedItems[0].Text;

            var selected_Table = tableList.SelectedItems[0].Text;


            string tablename = GetTableName();
            string str = sqlTextBox.Text;
            var settings = AppSettings.Instance;
            string sfd = "";

            switch (selected_item.Trim())
            {
                case "Data Entity Generator":
                    //Sbu.Ubys.Ebs.Entities
                    sfd = Path.Combine(settings.APIPath, settings.Namespace);
                    sfd = Path.Combine(sfd, $"{settings.Namespace}.Entities\\Concrete\\{tablename}.cs");
                    break;
                case "Ef Repository Generator":
                    //Sbu.Ubys.Ebs.Entities
                    sfd = Path.Combine(settings.APIPath, settings.Namespace);
                    sfd = Path.Combine(sfd, $"{settings.Namespace}.DataAccess\\Concrete\\EntityFramework\\{tablename}Repository.cs");
                    break;
                case "DTO Generator":
                    sfd = Path.Combine(settings.APIPath, settings.Namespace);
                    Directory.CreateDirectory(Path.Combine(sfd, $"{settings.Namespace}.Entities\\RequestDtos\\{tablename}"));
                    sfd = Path.Combine(sfd, $"{settings.Namespace}.Entities\\RequestDtos\\{tablename}\\{tablename}RequestDto.cs");
                    break;
                case "Validation Generator":
                    //Sbu.Ubys.Ebs.Entities
                    sfd = Path.Combine(settings.APIPath, settings.Namespace);
                    Directory.CreateDirectory(Path.Combine(sfd, $"{settings.Namespace}.Business\\Handlers\\{tablename}Handlers\\ValidationRules"));
                    sfd = Path.Combine(sfd, $"{settings.Namespace}.Business\\Handlers\\{tablename}Handlers\\ValidationRules\\{tablename}Validator.cs");
                    break;

                case "Create Command Handler Generator":
                    sfd = Path.Combine(settings.APIPath, settings.Namespace);
                    Directory.CreateDirectory(Path.Combine(sfd, $"{settings.Namespace}.Business\\Handlers\\{tablename}Handlers\\Commands"));
                    sfd = Path.Combine(sfd, $"{settings.Namespace}.Business\\Handlers\\{tablename}Handlers\\Commands\\Create{tablename}Command.cs");
                    break;
                case "Update Command Handler Generator":
                    sfd = Path.Combine(settings.APIPath, settings.Namespace);
                    Directory.CreateDirectory(Path.Combine(sfd, $"{settings.Namespace}.Business\\Handlers\\{tablename}Handlers\\Commands"));
                    sfd = Path.Combine(sfd, $"{settings.Namespace}.Business\\Handlers\\{tablename}Handlers\\Commands\\Update{tablename}Command.cs");
                    break;
                case "Delete Command Handler Generator":
                    sfd = Path.Combine(settings.APIPath, settings.Namespace);
                    Directory.CreateDirectory(Path.Combine(sfd, $"{settings.Namespace}.Business\\Handlers\\{tablename}Handlers\\Commands"));
                    sfd = Path.Combine(sfd, $"{settings.Namespace}.Business\\Handlers\\{tablename}Handlers\\Commands\\Delete{tablename}Command.cs");
                    break;
                case "Get List Query Generator":
                    sfd = Path.Combine(settings.APIPath, settings.Namespace);
                    Directory.CreateDirectory(Path.Combine(sfd, $"{settings.Namespace}.Business\\Handlers\\{tablename}Handlers\\Queries"));
                    sfd = Path.Combine(sfd, $"{settings.Namespace}.Business\\Handlers\\{tablename}Handlers\\Queries\\Get{tablename}ListQuery.cs");
                    break;
                case "Get By Id Query Generator":
                    sfd = Path.Combine(settings.APIPath, settings.Namespace);
                    Directory.CreateDirectory(Path.Combine(sfd, $"{settings.Namespace}.Business\\Handlers\\{tablename}Handlers\\Queries"));
                    sfd = Path.Combine(sfd, $"{settings.Namespace}.Business\\Handlers\\{tablename}Handlers\\Queries\\Get{tablename}byIdQuery.cs");
                    break;
                case "WebAPI Generator":
                    sfd = Path.Combine(settings.APIPath, settings.Namespace);
                    sfd = Path.Combine(sfd, $"{settings.Namespace}.WebAPI\\Controllers\\{tablename}Controller.cs");
                    break;
                case "Postman Generator":
                    //Check the directory if doesn't exists create one ... then write
                    sfd = Path.Combine(settings.APIPath, settings.Namespace);
                    if (!Directory.Exists($"{sfd}\\Postman"))
                    {
                        Directory.CreateDirectory($"{sfd}\\Postman");
                    }

                    sfd = Path.Combine(sfd, $"Postman\\{tablename}Collections.json");
                    break;
                //case "Delete Command Generator":
                //    break;
                //case "Delete Command Generator":
                //    break;
                //case "Delete Command Generator":
                //    break;
                default:

                    break;
            }


            using (StreamWriter writer = File.CreateText(sfd))
            {
                writer.Write(str);
            }
            MessageBox.Show(string.Format("File saved in the path {0}", sfd), "Mention");

        }

   

        private void postgresToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void msSqlToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm();
            settingsForm.Show();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tableList.SelectedItems.Count > 0 && codeList.SelectedItems.Count > 0)
            {
                Save();
            }
            else
            {
                MessageBox.Show($"You must select both table and generate command", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


    }
}
