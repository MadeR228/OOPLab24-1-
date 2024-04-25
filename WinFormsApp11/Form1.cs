using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace WinFormsApp11
{
    public partial class Form1 : Form
    {
        private volatile bool stopThreads = false;
        private ManualResetEvent thread1Event = new ManualResetEvent(true);
        private ManualResetEvent thread2Event = new ManualResetEvent(true);
        private ManualResetEvent thread3Event = new ManualResetEvent(true);

        Thread thread1;
        Thread thread2;
        Thread thread3;

        public Form1()
        {
            InitializeComponent();
        }

        private void DrawRect()
        {
            try
            {
                Random rnd = new Random();
                Graphics g = panel1.CreateGraphics();
                while (!stopThreads)
                {
                    thread1Event.WaitOne();
                    Thread.Sleep(40);
                    g.DrawRectangle(Pens.Pink, 0, 0, rnd.Next(this.Width), rnd.Next(this.Height));
                }
            }
            catch (Exception ex) { }
        }

        private void DrawEllipse()
        {
            try
            {
                Random rnd = new Random();
                Graphics g = panel2.CreateGraphics();
                while (!stopThreads)
                {
                    thread2Event.WaitOne();
                    Thread.Sleep(40);
                    g.DrawEllipse(Pens.Pink, 0, 0, rnd.Next(this.Width), rnd.Next(this.Height));
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void GenerateRandomNumber()
        {
            try
            {
                Random rnd = new Random();
                while (!stopThreads)
                {
                    thread3Event.WaitOne();
                    richTextBox1.Invoke((MethodInvoker)delegate ()
                    {
                        richTextBox1.Text += rnd.Next().ToString();
                    });
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            stopThreads = false;
            thread1 = new Thread(new ThreadStart(DrawRect));
            thread2 = new Thread(new ThreadStart(DrawEllipse));
            thread3 = new Thread(new ThreadStart(GenerateRandomNumber));
            thread1.Start();
            thread2.Start();
            thread3.Start();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (thread1 == null || !thread1.IsAlive)
            {
                thread1 = new Thread(new ThreadStart(DrawRect));
                thread1.Start();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (thread2 == null || !thread2.IsAlive)
            {
                thread2 = new Thread(new ThreadStart(DrawEllipse));
                thread2.Start();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (thread3 == null || !thread3.IsAlive)
            {
                thread3 = new Thread(new ThreadStart(GenerateRandomNumber));
                thread3.Start();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            thread1Event.Reset();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            thread2Event.Reset();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            thread3Event.Reset();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            thread1Event.Reset();
            thread2Event.Reset();
            thread3Event.Reset();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            stopThreads = true;
            thread1Event.Set();
            thread2Event.Set();
            thread3Event.Set();
        }
    }
}
