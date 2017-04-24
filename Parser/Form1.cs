using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Net;

namespace Parser
{
    public partial class Form1 : Form
    {
        public string[] Mass = new string[10];
        public string[] _Day = new string[10];
        public WebClient gm = new WebClient();

        public String Gismeteo()
        {
            String Response = gm.DownloadString("https://www.gismeteo.ru/weather-yoshkar-ola-11975/weekly/");
            string[] MaxT = new string[8];
            string[] MinT = new string[8];
            string[] Day = new string[8];
            string[] DayD = new string[8];
            string ret = "";
            int i = 0;
            // <em class="(date_of_week\s+[A-Za-z]+)">([0-9]+)</em>
            foreach (Match match in Regex.Matches(Response, @"<em class=""(date_of_week\s+[A-Za-z]+)"">([0-9]+)</em>"))
            {
                DayD[i] = match.Groups[2].Value;
                _Day[i] = DayD[i];
                i++;
            }
            i = 0;
            foreach (Match match in Regex.Matches(Response, @"class=""weather_item js_temp_graph""\s+data-max=""([-+]?[0-9]+)"" data-min=""([-+]?[0-9]+)"""))
            {
                MaxT[i] =  match.Groups[1].Value;
                Mass[i] = MaxT[i];
                MinT[i] =  match.Groups[2].Value;
                i++;
            }
            i = 0;
            foreach (Match match in Regex.Matches(Response, @"<span class=""day_of_week"">([А-Яа-я]+)</span>"))
            {
                 Day[i] = match.Groups[1].Value;
                 i++;
            }
            for (int c = 0; c < 7; c++)
            {
                ret = ret + "---------- " + Day[c] + ", " + DayD[c] + " ----------\nmax T: " + MaxT[c] + " °С | min T: " + MinT[c] + " °С\n";
            }
            return ret;
        }

        public String Yandex()
        {
            String Response = gm.DownloadString("https://yandex.ru/pogoda/yoshkar-ola");
            String Response1 = gm.DownloadString("https://yandex.ru/pogoda/yoshkar-ola/details");
            string[] MaxT = new string[10];
            string[] Day = new string[10];
            string[] DayD = new string[10];
            string ret = "";
            int i = 0;
            // <div class="forecast-brief__item-temp-day" title="Максимальная температура днём">+2 днём</div>
            foreach (Match match in Regex.Matches(Response, @"<div class=""forecast-brief__item-temp-day"" title=""Максимальная температура днём"">([−+]?[0-9]+) днём</div>"))
            {
                MaxT[i] = match.Groups[1].Value;
                Mass[i] = MaxT[i];
                i++;
            }
            foreach (Match match in Regex.Matches(Response, @"<div class=""forecast-brief__item-temp-day"" title=""Максимальная температура днём"">([−+]?[0-9]+)</div>"))
            {
                MaxT[i] = match.Groups[1].Value;
                Mass[i] = MaxT[i];
                i++;
            }
            i = 0;
            //<span class="forecast-brief__item-day-name">сегодня</span>
            //<span class="forecast-brief__item-day">([0-9]+)</span>
            //<strong class="forecast-detailed__day-number">([0-9]+)
            foreach (Match match in Regex.Matches(Response1, @"<strong class=""forecast-detailed__day-number"">([0-9]+)"))
            {
                DayD[i] = match.Groups[1].Value;
                _Day[i] = DayD[i];
                i++;
            }
            i = 0;
            foreach (Match match in Regex.Matches(Response, @"<span class=""forecast-brief__item-day-name"">([А-Яа-я]+)</span>"))
            {
                Day[i] = match.Groups[1].Value;
                i++;
            }
            for (int c = 0; c < 7; c++)
            {
                ret = ret + "------- " + Day[c] + ", " + DayD[c] + " -------\nT: " + MaxT[c] + " °С\n";
            }
            return ret;
        }

        public String Meteoinfo()
        {
            String Response = gm.DownloadString("http://www.meteoinfo.ru/forecasts5000/russia/republic-mary-el/joskar-ola");
            string[] MaxT = new string[7];
            string[] MinT = new string[7];
            string[] Day = new string[7];
            string[] DayD = new string[7];
            string ret = "";
            int i = 0;
            // <div class="forecast-brief__item-temp-day" title="Максимальная температура днём">+2 днём</div>
            foreach (Match match in Regex.Matches(Response, @"<td align=center>([А-Яа-я]+)<BR><nobr>([0-9]+)\s([А-Яа-я]+)</nobr></td>"))
            {
                Day[i] = match.Groups[1].Value;
                DayD[i] = match.Groups[2].Value;
                _Day[i] = DayD[i];
                i++;
            }
            foreach (Match match in Regex.Matches(Response, @"<td class=pogodadate>([А-Яа-я]+)<BR><nobr>([0-9]+)\s([А-Яа-я]+)</nobr></td>"))
            {
                Day[i] = match.Groups[1].Value;
                DayD[i] = match.Groups[2].Value;
                _Day[i] = DayD[i];
                i++;
            }
            i = 0;
            //<td class=pogodacell>([-+]?[0-9]) &nbsp;&frasl;&nbsp; ([-+]?[0-9])</td>
            foreach (Match match in Regex.Matches(Response, @"<td class=pogodacell>([-+]?[0-9]{1,2}) &nbsp;&frasl;&nbsp; ([-+]?[0-9]{1,2})</td>"))
            {
                MinT[i] = match.Groups[1].Value;
                MaxT[i] = match.Groups[2].Value;
                Mass[i] = MaxT[i];
                i++;
            }
            for (int c = 0; c < 7; c++)
            {
                ret = ret + "----- " + Day[c] + ", " + DayD[c] + " -----\nmax T: " + MaxT[c] + " °С | min T: " + MinT[c] + " °С\n";
            }
            return ret;
        }

        private void Draw()
        {
            //340 300
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics graph = Graphics.FromImage(bmp);
            Pen pen = new Pen(Color.Black);
            Pen penR = new Pen(Color.Red, 2);
            Pen point = new Pen(Color.Red);
            Pen os = new Pen(Color.Black);
            int[] pointsX = new int[7];
            int[] pointsY = new int[7];
            Font drawFont = new Font("Calibri", 10);
            
            SolidBrush drawBrush = new SolidBrush(Color.Black);

            int x = 5, p = 32;
            os.Width = 2;
            //graph.DrawLine(pen, 15, 20, 375, 20);
            graph.DrawLine(os, 30, 145, 325, 145);
            graph.DrawLine(os, 30, 15, 30, 295);
            for (int i = 0; i < 17; i++)
            {
                graph.DrawLine(os, 25, 20 + x, 35, 20 + x);
                if(p>0)
                    graph.DrawString("+" + p.ToString(), drawFont, drawBrush, 0, 10 + x);
                else
                    graph.DrawString(p.ToString(), drawFont, drawBrush, 0, 10 + x);
                p -= 4;
                x += 15;
            }
            x = 0;
            for (int i = 0; i < 7; i++)
            {
                graph.DrawLine(os, 30 + x, 140, 30 + x, 150);
                graph.DrawString(_Day[i], drawFont, drawBrush, 30 + x, 142);
                x += 40;
            }
            x = 0;
            for (int m = 0; m < 7; m++)
            {
                Mass[m] = Mass[m].Replace("−", "-");
                graph.FillRectangle(Brushes.Red, 27 + x, 142 - Convert.ToInt32(Mass[m])*4, 6, 6);
                pointsX[m] = 27 + x;
                pointsY[m] = 142 - Convert.ToInt32(Mass[m]) * 4;
                x += 40;
            }
            for (int i = 0; i < 6; i++)
            {
                graph.DrawLine(penR, pointsX[i] + 3, pointsY[i] + 3, pointsX[i+1] + 3, pointsY[i+1] + 3);
                x += 40;
            }
            pictureBox1.Image = bmp;
        }

        public Form1()
        {
            InitializeComponent();
            richTextBox1.SelectionAlignment = HorizontalAlignment.Center;
            this.MaximizeBox = false;
            gm.Proxy = new WebProxy("83.239.58.162:8080");
            gm.Encoding = Encoding.UTF8;            
        }

        private void Form1_Shown(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            richTextBox1.Clear();            
            richTextBox1.Text = Gismeteo();
            Draw();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox1.Text = Yandex();  
            Draw();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            richTextBox1.Clear();
            richTextBox1.Text = Meteoinfo();
            Draw();
        }
    }
}
