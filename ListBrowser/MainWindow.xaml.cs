using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ListBrowser;

public partial class MainWindow : Window
{
    private readonly ObservableCollection<string> _urls = [];

    public MainWindow()
    {
        InitializeComponent();
        UrlListBox.ItemsSource = _urls;
        _ = WebView.EnsureCoreWebView2Async();
    }

    private void AddUrl_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new AddUrlDialog { Owner = this };
        if (dialog.ShowDialog() == true && !string.IsNullOrWhiteSpace(dialog.EnteredUrl))
        {
            var url = dialog.EnteredUrl.Trim();
            if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                url = "https://" + url;
            _urls.Add(url);
        }
    }

    private void DeleteUrl_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string url)
            _urls.Remove(url);
    }

    private void UrlListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (UrlListBox.SelectedItem is string url)
        {
            CurrentUrlText.Text = url;
            WebView.Source = new Uri(url);
        }
    }
}
