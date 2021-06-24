using RestSharp;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace cryptoProject {
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window {
        public static string GetHash(string input) {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create()) {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++) {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

        public async void authorize() {
            var login = UsernameTextBox.Text;
            var password = GetHash(UsernameTextBox2.Password);

            var client = new RestClient($"http://82.146.61.12:3001/api/login?login={login}&password={password}");
            client.Timeout = -1;

            var request = new RestRequest(Method.GET);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var response = await client.ExecuteAsync(request);

            if (bool.Parse(response.Content)) {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            } else
                ErrorLabel.Visibility = Visibility.Visible;
        }
        public async void register() {
            var login = UsernameTextBox.Text;
            var password = GetHash(UsernameTextBox2.Password);

            var client = new RestClient($"http://82.146.61.12:3001/api/register?login={login}&password={password}");
            client.Timeout = -1;

            var request = new RestRequest(Method.GET);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var response = await client.ExecuteAsync(request);

            if (bool.Parse(response.Content)) {
                GoodLabel.Visibility = Visibility.Visible;
            } else ErrorCreateLabel.Visibility = Visibility.Visible;
        }
        public Authorization() {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e) {
            UsernamePlaceholder.Visibility = Visibility.Collapsed;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e) {
            if (sender is TextBox) {
                var textbox = sender as TextBox;
                if (string.IsNullOrEmpty(textbox.Text)) {
                    UsernamePlaceholder.Visibility = Visibility.Visible;
                }
            }
        }

        private void TextBox_GotFocus2(object sender, RoutedEventArgs e) {
            UsernamePlaceholder2.Visibility = Visibility.Collapsed;
        }

        private void TextBox_LostFocus2(object sender, RoutedEventArgs e) {
            if (sender is TextBox) {
                var textbox = sender as TextBox;
                if (string.IsNullOrEmpty(textbox.Text)) {
                    UsernamePlaceholder2.Visibility = Visibility.Visible;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            authorize();
        }

        private void Button_Click1(object sender, RoutedEventArgs e) {
            register();
        }

        private void UsernameTextBox2_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key == System.Windows.Input.Key.Enter) {
                if (btmLoginButton.Visibility == Visibility.Hidden)
                    authorize();
                else
                    register();
            } 
        }

        private void UsernameTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e) {
            if (e.Key == System.Windows.Input.Key.Enter) {
                if (btmLoginButton.Visibility == Visibility.Hidden)
                    authorize();
                else
                    register();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e) {
            loginButton.Visibility = Visibility.Hidden;
            registerButton.Visibility = Visibility.Visible;
            btmRegisterButton.Visibility = Visibility.Hidden;
            btmLoginButton.Visibility = Visibility.Visible;
            ErrorLabel.Visibility = Visibility.Hidden;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e) {
            loginButton.Visibility = Visibility.Visible;
            registerButton.Visibility = Visibility.Hidden;
            btmRegisterButton.Visibility = Visibility.Visible;
            btmLoginButton.Visibility = Visibility.Hidden;
            ErrorCreateLabel.Visibility = Visibility.Hidden;
        }
    }
}
