using SullaClient_base.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SullaClient_base
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static List<Product> products = new();

        private static HttpClient client = new()
        {
            BaseAddress = new("https://jwt.sulla.hu/")
        };

        string token = "";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var response = client.PostAsync("login", new StringContent(
                $"{{\"username\":\"{tbxUName.Text}\",\"password\":\"{tbxPass.Password}\"}}",
                Encoding.UTF8, "application/json"));

                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Login successful");
                    string resp = response.Result.Content.ReadAsStringAsync().Result;
                    token = resp.Split('"')[3];
                    MessageBox.Show(token);
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                }
                else
                {
                    MessageBox.Show("Login failed");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private async void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<Product> response = await client.GetFromJsonAsync<List<Product>>("termekek");
                foreach (var item in response)
                {
                    tbkStatus.Text += $"{item.Id} {item.Name} {item.Price}\n";
                    products.Add(item);
                }
                dgrData.ItemsSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
             
        }
    }
}