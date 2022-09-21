using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Threading.Tasks;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Newtonsoft.Json;
using System;

using Data = Google.Apis.Sheets.v4.Data;

using Google.Apis.Discovery;
using Google.Apis.Sheets;
using Google.Apis.Sheets.v4.Data;
using GData.Spreadsheets;
using System.Net.Http;
using Google.Apis.Auth;
using System.IO;
using System.Collections;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.FileIO;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using ConsoleApp2;
using System.Threading;

namespace Names
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string[] c1 = { "A", "C", "E", "G", "I", "K", "M", "O", "Q", "S", "U", "W", "Y" };
        public static string[] c2 = { "B", "D", "F", "H", "J", "L", "N", "P", "R", "T", "V", "X", "Z" };
        List<IList<object>> listTrack = new List<IList<object>>();
        List<Tuple<int, int>> tupTrack = new List<Tuple<int, int>>();
        List<object> track = new List<object>();

        public static int ef = 0;




        public MainWindow()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        List<string> StockNames = new List<string>(10);
        List<int> stockIndexes = new List<int>(10);

        private async void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //Ensure that text box contains text
            //Ensure that text entered does not already exist
            //Add name to list box

            if (!string.IsNullOrWhiteSpace(txtName1.Text))
            {
                string stockName = txtName1.Text;

                //List<string> strings;


                using (var reader = new StreamReader(@"C:\Users\12149\source\repos\Names\nasdaq_screener_1663524082856.csv"))
                {
                    using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        List<int> x = new List<int>();
                        List<int> y = new List<int>();
                        List<string> z = new List<string>();

                        var records = csvReader.GetRecords<StockTypes>().ToList();
                        var names = records.Select(x => x.Name).ToArray();
                        var symbols = records.Select(x => x.Symbol).ToArray();
                        //var b = names.Any(s => s.Contains(stockName));
                        List<Tuple<int, int>> indexes = new List<Tuple<int, int>>();

                        for (int i = 0; i < names.Length; i++)
                        {
                            int index = names[i].IndexOf(stockName);
                            if (index != -1)
                                indexes.Add(new Tuple<int, int>(i, index)); //where "i" is the index of the string, while "index" is the index of the substring
                        }
                        tupTrack = indexes;
                        int c;
                        for (c = 0; c < indexes.Count; c++)
                        {


                            int d;
                            int f;
                            x.Add(indexes[c].Item1);
                            y.Add(indexes[c].Item2);
                            //MessageBox.Show(c.ToString());
                        }
                        for (c = 0; c < x.Count; c++)
                        {
                            z.Add(symbols[x[c]]);
                            //MessageBox.Show(z[c]);
                        }
                        //view names where int location = i and length of string equals the substring length
                        //get symbol of stock
                        //a = Array.FindIndex(names, t=> t.Equals(stockName, StringComparison.InvariantCultureIgnoreCase));
                        //MessageBox.Show(a.ToString());
                        //if(names.Contains(stockName))
                        //{
                        //    MessageBox.Show(stockName);
                        //}
                        for (c = 0; c < z.Count; c++)
                        {
                            ChoosableList.Items.Add(z[c]);
                        }
                        records.Clear();
                        indexes.Clear();
                        names = null;
                        symbols = null;
                        x.Clear();
                        y.Clear();
                        z.Clear();
                        //clear all vars;
                    }



                }


                //what do i want this to do?
                //receive input from textbox
                //go through, check to ensure that stock name is valid and exists in csv
                //if doesn't exist, "invalid name"
                //if does exist, add to list of stock names
                //go through the list of stock names and make a search for each one, appending its codename to the base uri string
                //nasdaq only
                //write to another list/arry/google sheet/database with the stock price, stock name, and date when the infromation was recorded
                //plot a dot to a graph with position
                //after the next value is acquired, start drawing lines based on the difference between the values
                //force application to sleep for 24 hours, restart again
                //write data to external database to keep a long term storage
            }
            else
            {
                MessageBox.Show("Incorrect Input");
            }
        }

        private void txtName1_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ChoosableList_TextChanged()
        {

        }

        private void ChoosableList_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void PickBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ChoosableList != null)
            {
                List<object> stockFinal = new List<object>();
                List<IList<object>> stocks = new List<IList<object>>();
                int c = 1;
                int d = 1;

                if (ChoosableList.SelectedValue != null)
                {
                    string stockSymbol = ChoosableList.SelectedValue.ToString();
                    var stockPrice = StockScraper.NASDAQStockScraper(stockSymbol);
                    if (stockPrice.Contains("</"))
                    {
                        stockPrice = stockPrice.Replace(@"</", "");
                    }
                    else
                    {
                    }
                    string Ranges = (c1[ef] + c + ":" + c2[ef] + d);
                    stockFinal.Add(stockSymbol);
                    stockFinal.Add(stockPrice);

                    track.AddRange(stockFinal);
                    stocks.Add(stockFinal);
                    //listTrack.Add(stockFinal);

                    StockApp.SSheetConnector.UpdateData(stocks, Ranges);
                    //c++;
                    //d++;
                    ef++;
                    ChoosableList.Items.Clear();
                }
                else
                {
                    MessageBox.Show("Select the right stock name.");
                }



            }
            else
            {
                MessageBox.Show("Pick a stock.");
            }

        }
        public static int inc = 0;
        public static int df = 0;
        public static int h = 0;
        public static int f = 2;
        private void StockTracker_Click(object sender, RoutedEventArgs e)
        {
            if(track.Count>13)
            {
                MessageBox.Show("Unfortunately the app cannot handle more than 13 stocks at a time. Please restart and change your selection.");
            }else if(track.Count<13)
            {
                this.Close();
                //Thread.Sleep(60000);
                
                for (int i = 0; i < track.Count(); i++)
                {
                    string s = StockScraper.NASDAQStockScraper(track[i].ToString());
                    track[(i + 1)] = s;
                    i = i + 1;
                }



                //for (int abc = 0; abc < track.Count(); abc++)
                //{
                    for (int xyz = 0; xyz < 10; xyz++)
                    {

                    listTrack.Add(track);

                    List<IList<object>> stocks = new List<IList<object>>();
                        //for(int z = 0; z < listTrack.Count(); z++)
                        //{

                        //}
                        //i could do this more efficiently^, instead make a separate method that updates the stock of the price rather than this method.
                        stocks = listTrack;
                        df = track.Count();
                        
                            string Ranges = (c1[h] + f + ":" + c2[df] + f);
                            StockApp.SSheetConnector.UpdateData(stocks, Ranges);

                    stocks.Clear();
                    for (int i = 0; i < track.Count(); i++)
                    {
                        string s = StockScraper.NASDAQStockScraper(track[i].ToString());
                        track[(i + 1)] = s;
                        i = i + 1;

                    }
                    f++;

                    Thread.Sleep(86400000);
                    //}
                }
            }
        }
    }
}
