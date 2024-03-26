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
using System.Net.Http;

namespace PlayListImport
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly HttpClient client = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Import_Click(object sender, RoutedEventArgs e)
        {
            //SetPreviousInputs();

            Log.Text = "";
            StringBuilder path = new StringBuilder();

            // Add slash if not present in provided URL input
            if (Url.Text.Substring(Url.Text.Length - 1) == "/")
            {
                path.Append(Url.Text);
            }
            else
            {
                path.Append($"{Url.Text}/");
            }

            // Build path for playlist endpoint
            path.Append($"playlists/upload?sectionID={SectionId.Text}&path={PlaylistPath.Text}&X-Plex-Token={Token.Text}");

            //Log.WriteLine(path.ToString(), "Path");

            try
            {
                // Send POST request
                var response = await client.PostAsync(path.ToString(), null);

                // Await response
                var responseString = await response.Content.ReadAsStringAsync();

                // Write to debugger
                System.Diagnostics.Debug.WriteLine(response, "Response");

                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        Log.Text = "Command successfully sent to PLEX";
                        break;
                    default:
                        //result.Text = $"Unexpected result of request ({response.StatusCode})";
                        Log.Text = ($"Errors occurred while processing request. {response.StatusCode}\n{response.RequestMessage.RequestUri.AbsoluteUri}");
                        break;
                }
            }
            catch (Exception ex)
            {
                Log.Text = $"Errors occurred while processing request. {ex.Message}";
                System.Diagnostics.Debug.WriteLine(ex.ToString(), "Exception");
                //Logger.Error($"Errors occurred while processing request. {ex}");
            }

        }

    }
}