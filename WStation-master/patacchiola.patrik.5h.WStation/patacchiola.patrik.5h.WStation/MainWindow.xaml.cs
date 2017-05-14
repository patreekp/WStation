using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using static patacchiola.patrik._5h.WStation.JsonMeteo;
namespace patacchiola.patrik._5h.WStation
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DispatcherTimer clock = new System.Windows.Threading.DispatcherTimer();
            clock.Tick += new EventHandler(clock_Tick);
            clock.Interval = new TimeSpan(0, 0, 1);
            clock.Start();

            DispatcherTimer refresh = new System.Windows.Threading.DispatcherTimer();
            refresh.Tick += new EventHandler(refresh_Tick);
            refresh.Interval = new TimeSpan(1, 0, 0);
            refresh.Start();

            AggiornaForm();
            
        }
        private void clock_Tick(object sender, EventArgs e)
        {
            txtData.Text = DateTime.Now.DayOfWeek.ToString() + ", " + DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString()+"          " + DateTime.Now.Hour.ToString()+":"+ DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();
        }

        private void refresh_Tick(object sender, EventArgs e)
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
          
            txtMaxMinOggi.Text = Forecast.forecast.simpleforecast.forecastday[0].low.celsius.ToString() + " ˚C / " +  Forecast.forecast.simpleforecast.forecastday[0].high.celsius.ToString() + " ˚C";
            txtMaxMinDomani.Text = Forecast.forecast.simpleforecast.forecastday[1].low.celsius.ToString() + " ˚C / " +Forecast.forecast.simpleforecast.forecastday[1].high.celsius.ToString() + " ˚C";
            txtMaxMinDopoDomani.Text = Forecast.forecast.simpleforecast.forecastday[2].low.celsius.ToString() + " ˚C / " + Forecast.forecast.simpleforecast.forecastday[2].high.celsius.ToString() + " ˚C";
            txtMaxMinDopoDomaniAncora.Text = Forecast.forecast.simpleforecast.forecastday[3].low.celsius.ToString() + " ˚C / " + Forecast.forecast.simpleforecast.forecastday[3].high.celsius.ToString() + " ˚C";

        }
    }
}
