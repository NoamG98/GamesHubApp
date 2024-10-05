using System.Windows;

namespace GamesHubApp
{
    public partial class CountriesProjectLandingPage : Window
    {
        public CountriesProjectLandingPage()
        {
            InitializeComponent();
        }

        private void OpenCountriesProject(object sender, RoutedEventArgs e)
        {
            var countriesProjectWindow = new CountriesProject();
            countriesProjectWindow.ShowDialog();
        }


        private void BackToGamesPage(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
