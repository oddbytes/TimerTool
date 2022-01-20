/*
 * Created by SharpDevelop.
 * User: Tebjan Halm
 * Date: 21.01.2014
 * Time: 16:55
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using System.Globalization;
using System.Diagnostics;

namespace ProGamer
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{

        private bool startMinimized = false;
        private bool dragging = false;
        private Point startPoint ;
        public MainForm(string[] args)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();


            //ResourceManager resources = new ResourceManager(typeof(MainForm));
            this.Resize += MainForm_Resize;


            notifyIcon1.BalloonTipTitle = "Pro Gamer";
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon1.Text = "Pro Gamer";
        
            notifyIcon1.Click += NotifyIcon1_Click;
         


            //apply commandline args
          
            var i = 0;
            foreach(var arg in args)
            {
                
                if(arg == "-t")
                {
                    if (args.Length > (i + 1))
                    {
                        double val;
                        if (double.TryParse(args[i+1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out val))
                        {
                            WinApiCalls.SetTimerResolution((uint)(val * 10000));
                        }
                    }
                }

                if(arg == "-minimized")
                {
                    startMinimized = true;

                 
                }

                i++;
            }
            
      
        }



  

        private void NotifyIcon1_Click(object sender, EventArgs e)
        {
           
            if (this.WindowState != FormWindowState.Normal)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
            }
            

        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                this.Hide();
                notifyIcon1.Visible = true;
            }
            else
                notifyIcon1.Visible = false;
           
        }





        void MainFormLoad(object sender, EventArgs e)
		{
            this.WindowState =startMinimized? FormWindowState.Minimized:FormWindowState.Normal;

            DisplayTimerCaps();
		}

        protected override void OnPaint(PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Red, ButtonBorderStyle.Solid);
        }

        private void DisplayTimerCaps()
		{
			var caps = WinApiCalls.QueryTimerResolution();
			CurrentLabel.Text =  (caps.PeriodCurrent/10000.0) + " ms";
			MinLabel.Text = (caps.PeriodMin/10000.0) + " ms";
			MaxLabel.Text = (caps.PeriodMax/10000.0) + " ms";
		}
		
		void Timer1Tick(object sender, EventArgs e)
		{
			DisplayTimerCaps();
		}

        private void lblSite_Click(object sender, EventArgs e)
        {
            Process.Start("https://www.remotocreativo.com");
        }

     

        private void control_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }

        private void control_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void imgCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void imgMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void mainForm_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            startPoint = new Point(e.X, e.Y);

        }

        private void mainForm_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void mainForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this.startPoint.X, p.Y - this.startPoint.Y);

            }

        }
    }
}
