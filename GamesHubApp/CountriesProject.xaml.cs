using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace GamesHubApp
{
    public partial class CountriesProject : Window
    {
        public ObservableCollection<Country> Countries { get; set; } = new ObservableCollection<Country>();
        private ObservableCollection<Country> _allCountries = new ObservableCollection<Country>();
        public static HttpClient client = new HttpClient();

        public CountriesProject()
        {
            InitializeComponent();  // Ensure that this is called first
            _ = LoadCountriesDataAsync();
            UpdatePlaceholderVisibility();
        }

        private async Task LoadCountriesDataAsync()
        {
            try
            {
                string json = await client.GetStringAsync("https://restcountries.com/v3.1/all");

                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var countries = JsonSerializer.Deserialize<List<Country>>(json, options);
                if (countries != null)
                {
                    _allCountries = new ObservableCollection<Country>(countries);
                    UpdateCountriesCollection(_allCountries.ToList());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading countries data: {ex.Message}");
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePlaceholderVisibility();

            string searchText = SearchTextBox.Text?.ToLower() ?? string.Empty;
            var filteredCountries = _allCountries
                .Where(c => c.Name?.Common?.ToLower().Contains(searchText) == true)
                .ToList();

            UpdateCountriesCollection(filteredCountries);
        }

        private void UpdatePlaceholderVisibility()
        {
            // Correct name used here: SearchPlaceholderTextBlock
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                SearchPlaceholderTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                SearchPlaceholderTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        private void RegionFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = RegionFilterComboBox.SelectedItem as ComboBoxItem;

            if (selectedItem != null)
            {
                string selectedRegion = selectedItem.Content?.ToString() ?? string.Empty;

                if (selectedRegion == "All Regions")
                {
                    UpdateCountriesCollection(_allCountries.ToList());
                }
                else
                {
                    var filteredCountries = _allCountries
                        .Where(c => c.Region?.Equals(selectedRegion, System.StringComparison.OrdinalIgnoreCase) == true)
                        .ToList();

                    UpdateCountriesCollection(filteredCountries);
                }
            }
        }

        private void UpdateCountriesCollection(List<Country> countries)
        {
            if (CountriesDataGrid != null)
            {
                Countries.Clear();
                foreach (var country in countries)
                {
                    Countries.Add(country);
                }
                CountriesDataGrid.ItemsSource = Countries;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
