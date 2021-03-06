﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlackScreen
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            int id = 0;
            RegisterHotKey(this.Handle, id, (int)KeyModifier.Alt, Keys.A.GetHashCode()); 
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            WinKey = 8
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x0312)
            {
                this.Focus();
                blackout();
            }
        }

        void normal()
        {
            black = false;
            this.Hide();
            this.TopMost = false;
            this.WindowState = FormWindowState.Normal;
            blackOutToolStripMenuItem.Visible = true;
            showToolStripMenuItem.Visible = false;
        }

        bool black = false;

        void blackout()
        {
            black = true;
            this.Show();
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            blackOutToolStripMenuItem.Visible = false;
            showToolStripMenuItem.Visible = true;
            this.BringToFront();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            blackout();
        }

        Dictionary<string, Point> dictionary = new Dictionary<string, Point>();

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            int i = 0;
            foreach (Screen screen in Screen.AllScreens)
            {
                dictionary.Add(screen.DeviceName, screen.WorkingArea.Location);
                changeScreensToolStripMenuItem.DropDownItems.Add(screen.DeviceName, null, onClick);                
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            this.Hide();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void blackOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            blackout();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                normal();
            }
        }

        bool cursor = true;

        void showCursor(bool _c)
        {
            switch (_c)
            {
                case false: Cursor.Hide(); break;
                case true: Cursor.Show(); break;
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            cursor = !cursor;
            showCursor(cursor);
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            normal();
        }


        private void onClick(object sender, EventArgs e)
        {
            int k = 0;
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.DeviceName == sender.ToString())
                {
                    switch (black)
                    {
                        case true: normal(); this.Location = dictionary[screen.DeviceName]; blackout(); break;
                        case false: this.Location = dictionary[screen.DeviceName]; break;
                    }
                }
            }
        }
    }
}
