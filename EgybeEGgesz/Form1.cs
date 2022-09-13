using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EgybeEGgesz
{
    public partial class Form1 : Form
    {

        bool huzas;
        Point elhelyezkedes;
        

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {          
            string str = Directory.GetCurrentDirectory() + @"\dejavu.exe";
            Process process = new Process();
            process.StartInfo.FileName = str;
            process.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string str = Directory.GetCurrentDirectory() + @"\JetafidaScheme.exe";
            Process process = new Process();
            process.StartInfo.FileName = str;
            process.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string str = Directory.GetCurrentDirectory() + @"\PassPoint.exe";
            Process process = new Process();
            process.StartInfo.FileName = str;
            process.Start();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string str = Directory.GetCurrentDirectory() + @"\zaro.exe";


            Process process = new Process();
            process.StartInfo.FileName = str;
            process.Start();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
            
            
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            huzas = true;
            elhelyezkedes.X = e.X;
            elhelyezkedes.Y = e.Y;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            huzas = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (huzas == true)
            {
                Point formElhelyezkedes = PointToScreen(e.Location);
                Location = new Point(formElhelyezkedes.X - elhelyezkedes.X, formElhelyezkedes.Y - elhelyezkedes.Y);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
