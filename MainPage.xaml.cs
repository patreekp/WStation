using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using patacchiola.patrik._5h.WStation;
using System.Net.Http;
using Newtonsoft.Json;
using Windows.UI.Xaml.Media.Imaging;
// Il modello di elemento Pagina vuota è documentato all'indirizzo https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x410

namespace Weather
{
    /// <summary>
    /// Pagina vuota che può essere usata autonomamente oppure per l'esplorazione all'interno di un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            DispatcherTimer clock = new Windows.UI.Xaml.DispatcherTimer();
            clock.Tick += clock_Tick;
            clock.Interval = new TimeSpan(0, 0, 1);
            clock.Start();

            DispatcherTimer refresh = new Windows.UI.Xaml.DispatcherTimer();
            refresh.Tick += refresh_Tick;
            refresh.Interval = new TimeSpan(1, 0, 0);
            refresh.Start();

            AggiornaForm();

        }
        private void clock_Tick(object sender, object e)
        {
            txtData.Text = DateTime.Now.DayOfWeek.ToString() + ", " + DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString() + "          " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();
        }

        private void refresh_Tick(object sender, object e)
        {
            AggiornaForm();
        }

        public async void AggiornaForm()
        {
            HttpClient client = new HttpClient();
            string result = await client.GetStringAsync(new Uri(@"http://api.wunderground.com/api/1381805c071d61f2/conditions/q/IT/" + "Rimini" + ".json"));
            string result2 = await client.GetStringAsync(new Uri(@"http://api.wunderground.com/api/1381805c071d61f2/forecast/q/IT/" + "Rimini" + ".json"));
            double dopodomani = 2, dopodomaniancora = 3;
            JsonMeteo.RootObject Meteo = JsonConvert.DeserializeObject<JsonMeteo.RootObject>(result);
            JsonForecast.RootObject Forecast = JsonConvert.DeserializeObject<JsonForecast.RootObject>(result2);
            txtTemp.Text = Meteo.current_observation.temp_c.ToString() + " ˚C";
            txtWindChill.Text = Meteo.current_observation.windchill_c.ToString() + " ˚C";
            txtWindSpeed.Text = Meteo.current_observation.wind_kph.ToString() + " km/h";
            txtDirection.Text = Meteo.current_observation.wind_dir.ToString();
            txtPrec.Text = Meteo.current_observation.precip_today_string.ToString();
            txtHumidity.Text = Meteo.current_observation.relative_humidity.ToString();

            lblDopoDomani.Text = DateTime.Now.AddDays(dopodomani).DayOfWeek.ToString();
            lblDopoDomaniAncora.Text = DateTime.Now.AddDays(dopodomaniancora).DayOfWeek.ToString();

            imgOggi.Source = new BitmapImage(new Uri(Meteo.current_observation.icon_url));
            imgDomani.Source = new BitmapImage(new Uri(Forecast.forecast.simpleforecast.forecastday[1].icon_url));
            imgGiorno1.Source = new BitmapImage(new Uri(Forecast.forecast.simpleforecast.forecastday[2].icon_url));
            imgGiorno2.Source = new BitmapImage(new Uri(Forecast.forecast.simpleforecast.forecastday[3].icon_url));

            txtCondOggi.Text = Meteo.current_observation.weather.ToString();
            txtCondDomani.Text = Forecast.forecast.simpleforecast.forecastday[1].conditions.ToString();
            txtCondDopoDomani.Text = Forecast.forecast.simpleforecast.forecastday[2].conditions.ToString();
            txtCondDopoDomaniAncora.Text = Forecast.forecast.simpleforecast.forecastday[3].conditions.ToString();

            txtMaxMinOggi.Text = Forecast.forecast.simpleforecast.forecastday[0].low.celsius.ToString() + " ˚C / " + Forecast.forecast.simpleforecast.forecastday[0].high.celsius.ToString() + " ˚C";
            txtMaxMinDomani.Text = Forecast.forecast.simpleforecast.forecastday[1].low.celsius.ToString() + " ˚C / " + Forecast.forecast.simpleforecast.forecastday[1].high.celsius.ToString() + " ˚C";
            txtMaxMinDopoDomani.Text = Forecast.forecast.simpleforecast.forecastday[2].low.celsius.ToString() + " ˚C / " + Forecast.forecast.simpleforecast.forecastday[2].high.celsius.ToString() + " ˚C";
            txtMaxMinDopoDomaniAncora.Text = Forecast.forecast.simpleforecast.forecastday[3].low.celsius.ToString() + " ˚C / " + Forecast.forecast.simpleforecast.forecastday[3].high.celsius.ToString() + " ˚C";

        }
    }
}
