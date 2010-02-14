using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Weather
{
    public enum WeatherAnimations { FlipToBack, FlipToData };

    public partial class DisplayWeather : UserControl
    {
        public bool AnimationRunning { get; set; }
        public WeatherAnimations AnimationState { get; set; }
        public List<WeatherAnimations> AnimationQueue = new List<WeatherAnimations>();

        public DisplayWeather()
        {
            InitializeComponent();
            //FlipToBack.Begin();
            this.AnimationState = WeatherAnimations.FlipToBack;
            this.AnimationRunning = false;

            FlipToBack.Completed += new EventHandler(AnimationComplete);
            FlipToData.Completed += new EventHandler(AnimationComplete);
        }

        void AnimationComplete(object sender, EventArgs e)
        {
            this.AnimationRunning = false;

            if (recBorder1_Copy.Opacity == 1)
                this.AnimationState = WeatherAnimations.FlipToBack;
            else
                this.AnimationState = WeatherAnimations.FlipToData;

            DoAnimate();
        }

        public void DoAnimate()
        {
            if (!this.AnimationRunning)
                if (AnimationQueue.Count >= 1)
                {
                    WeatherAnimations Action = AnimationQueue[0];
                    AnimationQueue.RemoveAt(0);

                    switch (Action)
                    {
                        case WeatherAnimations.FlipToBack:
                            if (this.AnimationState == WeatherAnimations.FlipToData)
                            {
                                this.AnimationRunning = true;
                                FlipToBack.Begin();
                            }
                            else
                                this.DoAnimate();
                            break;

                        case WeatherAnimations.FlipToData:
                            if (this.AnimationState == WeatherAnimations.FlipToBack)
                            {
                                this.AnimationRunning = true;
                                FlipToData.Begin();
                            }
                            else
                                this.DoAnimate();
                            break;
                    }
                }
        }
        public void DoAnimate(WeatherAnimations Action)
        {
            AnimationQueue.Add(Action);
            this.DoAnimate();
        }


        public string Day
        {
            get { return txtDay.Text; }
            set { txtDay.Text = value; }
        }

        public string Forecast
        {
            get { return txtForcast.Text; }
            set { txtForcast.Text = value; 
                  imaForecast.Source = new BitmapImage(
                                           new Uri("/Weather;component/Images/" + this.txtForcast.Text + ".png", UriKind.RelativeOrAbsolute)
                                          );
                }
        }

        public string High
        {
            get { return txtHigh.Text; }
            set { txtHigh.Text = value + " F"; }
        }

        public string Low
        {
            get { return txtLow.Text; }
            set { txtLow.Text = value + " F"; }
        }

    }
}
