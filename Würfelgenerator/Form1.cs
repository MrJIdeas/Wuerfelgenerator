using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Würfelgenerator
{
    public partial class Form1 : Form
    {
        private BindingList<Ziehung> Ziehungen = new BindingList<Ziehung>();

        private Dictionary<int, int> Group = new Dictionary<int, int>();
        private Dictionary<int, int> Group2 = new Dictionary<int, int>();
        private Dictionary<int, int> Group3 = new Dictionary<int, int>();

        private DateTime lastupdate = DateTime.MinValue;

        private Random rand = new Random(DateTime.Now.Millisecond);

        public Form1()
        {
            InitializeComponent();
            Ziehungen.ListChanged += Ziehungen_ListChanged;
        }

        private void Ziehungen_ListChanged(object sender, ListChangedEventArgs e)
        {
            if ((DateTime.Now - lastupdate).TotalSeconds > 1)
            {
                richTextBox1.Text = Ziehungen.LastOrDefault()?.ToString();
                Group.Clear();

                double MW_Wuerfe = Ziehungen.Select(x => x.Count).Sum() / (double)Ziehungen.Count;
                double MW_Punkte = Ziehungen.Select(x => x.Punktzahl).Sum() / (double)Ziehungen.Count;

                label6.Text = Math.Round(MW_Punkte, 2).ToString();
                label7.Text = Math.Round(MW_Wuerfe, 2).ToString();

                for (int i = 0; i < Ziehungen.Count; i++)
                {
                    if (Group.Keys.Contains(Ziehungen[i].Count))
                        Group[Ziehungen[i].Count]++;
                    else
                        Group.Add(Ziehungen[i].Count, 1);
                }
                if (tabControl1.SelectedTab == tabPage2)
                {
                    chart2.Series[0].Points.Clear();
                    foreach (var item in Group)
                        chart2.Series[0].Points.AddXY(item.Key, item.Value);
                }
                for (int i = 0; i < Ziehungen.Count; i++)
                {
                    if (Group2.Keys.Contains(Ziehungen[i].Punktzahl))
                        Group2[Ziehungen[i].Punktzahl]++;
                    else
                        Group2.Add(Ziehungen[i].Punktzahl, 1);
                }
                if (tabControl1.SelectedTab == tabPage1)
                {
                    chart1.Series[0].Points.Clear();
                    foreach (var item in Group2)
                        chart1.Series[0].Points.AddXY(item.Key, item.Value);
                }

                for (int i = 0; i < Ziehungen.Count; i++)
                {
                    foreach (var thing in Ziehungen[i])
                    {
                        if (Group3.Keys.Contains(thing))
                            Group3[thing]++;
                        else
                            Group3.Add(thing, 1);
                    }
                }
                if (tabControl1.SelectedTab == tabPage3)
                {
                    chart3.Series[0].Points.Clear();
                    foreach (var item in Group3)
                        chart3.Series[0].Points.AddXY(item.Key, item.Value);
                }
                lastupdate = DateTime.Now;
            }
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            label2.Text = trackBar1.Value.ToString();
            button2.PerformClick();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Ziehungen.Clear();
            richTextBox1.ResetText();
            chart1.Series[0].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart3.Series[0].Points.Clear();
            Group.Clear();
            Group2.Clear();
            Group3.Clear();
        }

        private void Wuerfel()
        {
            Ziehung neu = new Ziehung();
            int count = 0;
            while (count < trackBar1.Value)
            {
                int erg = rand.Next((int)numericUpDown1.Value, (int)numericUpDown2.Value + 1);
                neu.Add(erg);
                count = neu.Where(x => x == erg).Count();
            }
            Ziehungen.Add(neu);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Wuerfel();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                timer1.Start();
            else
                timer1.Stop();
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            timer1.Interval = 1000 / trackBar2.Value;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Wuerfel();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            button2_Click(null, null);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            button2_Click(null, null);
        }
    }
}