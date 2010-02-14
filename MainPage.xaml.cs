using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using Weather.Classes;
using System.Windows;
using System.Windows.Browser;

namespace Weather
{
    public partial class MainPage : UserControl
    {
        List<DisplayWeather> weatherControls = new List<DisplayWeather>();

        public MainPage()
        {
            InitializeComponent();
            weatherControls.AddRange(new List<DisplayWeather> { displayWeather1, displayWeather2, displayWeather3, displayWeather4, displayWeather5 });

            if (HtmlPage.Document.QueryString.Keys.Contains("zip"))
            {
                textBox1.Text = HtmlPage.Document.QueryString["zip"];
                this.GetWeather(textBox1.Text);
            }
        }

        //
        // Animation
        //
        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //ViewWeather.Begin();
                for (int counter = 1; counter <= weatherControls.Count; counter++)
                    weatherControls[counter - 1].DoAnimate(WeatherAnimations.FlipToBack);

                this.GetWeather(textBox1.Text);
            }
        }

        //
        // Connect to Wunderground
        //
        public void GetWeather(string Zipcode)
        {
            string query = String.Format("http://api.wunderground.com/auto/wui/geo/ForecastXML/index.xml?query={0}", Zipcode);

            WebClient wunderground = new WebClient();
            wunderground.DownloadStringCompleted += new DownloadStringCompletedEventHandler(WeatherXML);
            wunderground.DownloadStringAsync(new Uri(query)); 
        }

        //
        // Process the incoming data and turn it into a list
        //
        private void WeatherXML(object sender, DownloadStringCompletedEventArgs e)
        {
            List<WeatherData> forecast = new List<WeatherData>();

            if (e.Error == null)
            {
                XDocument thisDoc = XDocument.Parse(e.Result);

                forecast = (from x in thisDoc.Elements("forecast").Elements("simpleforecast").Elements("forecastday")
                            select new WeatherData()
                            {
                                Text = x.Element("conditions").Value,
                                Icon = x.Element("icon").Value,
                                Weekday = (from date in x.Elements("date") select date.Element("weekday").Value).First(),
                                HighF = (from temp in x.Elements("high") select temp.Element("fahrenheit").Value).First(),
                                HighC = (from temp in x.Elements("high") select temp.Element("celsius").Value).First(),
                                LowF = (from temp in x.Elements("low") select temp.Element("fahrenheit").Value).First(),
                                LowC = (from temp in x.Elements("low") select temp.Element("celsius").Value).First()
                            }).ToList();

                if (forecast.Count >= 5)
                    for (int counter = 1; counter <= weatherControls.Count; counter++)
                    {
                        weatherControls[counter - 1].Day = forecast[counter - 1].Weekday;
                        weatherControls[counter - 1].Forecast = forecast[counter - 1].Text;
                        weatherControls[counter - 1].High = forecast[counter - 1].HighF;
                        weatherControls[counter - 1].Low = forecast[counter - 1].LowF;
                        weatherControls[counter - 1].DoAnimate(WeatherAnimations.FlipToData);
                    }
                else
                    MessageBox.Show("Invalid Zip Code");
            }
            else
                MessageBox.Show("Error, Call 911");
        }

        private void displayWeather4_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        }

        private void displayWeather5_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

        } 
    }
}
