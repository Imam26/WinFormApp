using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Common;
using System.Configuration;

namespace WindowsFormsApp2
{
    public partial class Form2 : Form
    {
        DbCommand cmd = null;

        public Form2()
        {
            InitializeComponent();
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
        }

        public Form2(DbConnection cnStr):this()
        {
            if (cnStr.State == System.Data.ConnectionState.Closed)
            {
                cnStr.Open();
            }
            cmd = cnStr.CreateCommand();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.Controls.Count == 2)
               this.Controls.RemoveAt(1);
            DataGridView dataGrid = new DataGridView();
            dataGrid.Location = new Point(10, 60);
            dataGrid.AutoSize = true;

            cmd.CommandText = $"select * from {comboBox1.SelectedItem.ToString()}";
            DbDataReader reader = cmd.ExecuteReader();

            DataTable table = new DataTable();
            table.Load(reader);
            reader.Close();

            dataGrid.DataSource = table;

            this.Controls.Add(dataGrid);
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
