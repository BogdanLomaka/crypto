using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Globalization;

namespace cryptoProject {
    public partial class MainWindow : Window {
        class crypto {
            public string price { get; set; }
        }

        public async void getCourse() {
            var privateKey = "f40ada9ce4f2f5769bdcf871d15238a3f06f9122";
            var ids = "BTC,ETH,XRP";
            var index = Combo.SelectedIndex;
            var convert = "";

            if (index == 0)
                convert = "USD";
            else if (index == 1)
                convert = "RUB";
            else if (index == 2)
                convert = "UAH";

            var client = new RestClient($"https://api.nomics.com/v1/currencies/ticker?key={privateKey}&ids={ids}&convert={convert}");
            client.Timeout = -1;

            var request = new RestRequest(Method.GET);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var response = await client.ExecuteAsync(request);

            var data = JsonConvert.DeserializeObject<List<crypto>>(response.Content);

            btc_price.Content = (Math.Round(double.Parse(UsernameTextBox_Main.Text, CultureInfo.InvariantCulture) / double.Parse(data[0].price, CultureInfo.InvariantCulture) * 10000) / 10000).ToString();
            eth_price.Content = (Math.Round(double.Parse(UsernameTextBox_Main.Text, CultureInfo.InvariantCulture) / double.Parse(data[1].price, CultureInfo.InvariantCulture) * 10000) / 10000).ToString();
            xpr_price.Content = (Math.Round(double.Parse(UsernameTextBox_Main.Text, CultureInfo.InvariantCulture) / double.Parse(data[2].price, CultureInfo.InvariantCulture) * 10000) / 10000).ToString();
        }
        public MainWindow() {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            getCourse();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e) {
            UsernamePlaceholder_Main.Visibility = Visibility.Collapsed;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e) {
            if (sender is TextBox) {
                var textbox = sender as TextBox;
                if (string.IsNullOrEmpty(textbox.Text)) {
                    UsernamePlaceholder_Main.Visibility = Visibility.Visible;
                }
            }
        }

        private void UsernameTextBox_Main_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == System.Windows.Input.Key.Enter)
                getCourse();
        }
    }
}
