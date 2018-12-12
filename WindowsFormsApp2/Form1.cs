using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.Common;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {

        private TextBox textBox;

        public TextBox TextBox {get =>textBox;}

        public Form1()
        {
            InitializeComponent();

            textBox = new TextBox();
            textBox.Size = new Size(120, 10);
            textBox.Location = new Point(165, 18);

            Label label = new Label();
            label.Location = new Point(10, 20);
            label.AutoSize = true;
            label.Text = "Введите название сервера: ";

            this.Controls.Add(label);
            this.Controls.Add(textBox);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Button btn = new Button();
            btn.Text = "Ок";
            btn.Size = new Size(80, 25);
            btn.Location = new Point(this.Width / 3,60);
            btn.Click += Btn_Click;

            this.Controls.Add(btn);
        }

        private void Btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
