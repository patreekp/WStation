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
        }

        private async Task btnCerca_ClickAsync(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            string result = await client.GetStringAsync(new Uri(@"http://api.wunderground.com/api/1381805c071d61f2/conditions/q/IT/" + txtCitta.Text + ".json"));

            JsonMeteo Meteo = JsonConvert.DeserializeObject<JsonMeteo>(result);
            
        }
    }
}
