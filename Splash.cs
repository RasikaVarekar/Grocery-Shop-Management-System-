﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grocery_management_System
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        int startPos = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            startPos += 1;
            Myprogress.Value = startPos;
            PercentageLbl.Text = startPos + "%";
            if (Myprogress.Value == 100)
            {
                Myprogress.Value = 0;
                timer1.Stop();
                
                Login log = new Login();
                log.Show();
                this.Hide();
            }

        }

        private void Splash_Load(object sender, EventArgs e)
        {
            timer1.Start();

        }

        private void Myprogress_Click(object sender, EventArgs e)
        {

        }
    }
}
