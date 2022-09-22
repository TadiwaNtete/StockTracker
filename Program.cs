

using ConsoleApp2;
using StockApp;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Runtime.Serialization.Formatters.Binary;
using System.Configuration;
using static System.Net.Mime.MediaTypeNames;
using System.Xml;
using Newtonsoft.Json.Linq;

namespace consoleapp5
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>        
        public static int x1 = 1;
        public static int x2 = 1;
        public static int x3 = 2;
        public static int x4 = 1;

        public static string[] c1 = { "A", "C", "E", "G", "I", "K", "M", "O", "Q", "S", "U", "W", "Y" };
        public static string[] c2 = { "B", "D", "F", "H", "J", "L", "N", "P", "R", "T", "V", "X", "Z" };
        static void Main()
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var appSettings = configFile.AppSettings.Settings;
            string sa = appSettings["DateInterval"].Value;
            //DateTime oDate = DateTime.ParseExact(sa, "yyyy-MM-dd HH:mm:ss,fff", System.Globalization.CultureInfo.InvariantCulture);
            //DateTime cdate = DateTime.Today;
            //String datediff = (cdate - oDate).TotalDays.ToString();
            //Console.WriteLine(datediff);
            //this ^ was an attempt to use the difference between dates to check which row to update to. instead, i use a date interval int i update every day
            int interval;
            interval = Convert.ToInt32(sa);
            List<object> todayStocks = new List<object>();
            List<IList<object>> todayList = new List<IList<object>>();
            string ranges;
            for (interval = 1; interval < 30; interval++)
            {
                appSettings["DateInterval"].Value = interval.ToString();
                for (x1 = 0; x1 < c2.Length; x1++)
                {
                    
                    ranges = c1[x1] + x2 + ":" + c1[x1] + x2;
                    string s = StockApp.SSheetConnector.CallData(ranges);
                    s = new String(s.Where(Char.IsLetter).ToArray());
                    if (string.IsNullOrEmpty(s) || s == "null")
                    {
                        break;

                    }
                    else
                    {
                        todayStocks.Add(s);
                        string z = StockScraper.NASDAQStockScraper(s);

                        if (z.Contains("</") || z.Contains("<"))
                        {
                            z = z.Replace(@"</", "");
                            z = z.Replace(@"/", "");
                            z = z.Replace(@"<", "");
                            todayStocks.Add(z);
                        }
                        else
                        {
                            todayStocks.Add(z);
                        }

                    }

                }
                //todayList.Add(todayStocks);
                todayList.Add(todayStocks);
                for (x4 = 0; x4 < todayStocks.Count; x4++)
                {
                    ranges = c1[x4] + interval + ":" + c2[x4] + interval;
                    StockApp.SSheetConnector.UpdateData(todayList, ranges);
                    x3++;
                }
                todayList.Clear();
                todayStocks.Clear();
                Thread.Sleep(86400000);
            }



            //int x = 0;
            //List<object> ilist1 = new List<object>();
            //List<object> ilist2 = new List<object>();
            //List<object> ilist3 = new List<object>();

            //List<IList<object>> List = new List<IList<object>>(10);


            //    //"A6:H6"

            //ilist1.Add("1");
            //ilist1.Add("9/21");
            //ilist1.Add("1");
            //ilist1.Add("9:30");
            //ilist1.Add("18:00");
            //ilist1.Add("14:30");
            //ilist1.Add("15:00");
            //ilist1.Add("9:28");
            //ilist1.Add("18:37");
            //List.Add(ilist1);

            //while(x == 0)
            //{
            //    string rangespace = "A" + x1 + ":J" + x2;
            //    var output = SSheetConnector.UpdateData(List, rangespace);
            //    x2++;
            //    x1++;
            //    Console.WriteLine(output);
            //}



        }
    }
}




