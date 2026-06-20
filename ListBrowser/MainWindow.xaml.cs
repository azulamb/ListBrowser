using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Web.WebView2.Core;

namespace ListBrowser;

public partial class MainWindow : Window
{
    private readonly ObservableCollection<string> _urls = [];
    private static readonly string UserDataFolder =
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ListBrowser", "WebView2");

    public MainWindow()
    {
        InitializeComponent();
        UrlListBox.ItemsSource = _urls;
        _ = InitializeWebViewAsync();
    }

    private async Task InitializeWebViewAsync()
    {
        var env = await CoreWebView2Environment.CreateAsync(userDataFolder: UserDataFolder);
        await WebView.EnsureCoreWebView2Async(env);
        WebView.DefaultBackgroundColor = System.Drawing.Color.FromArgb(0x4d, 0x4d, 0x4d);
        WebView.CoreWebView2.NavigateToString("<html><body style='background:#4d4d4d;margin:0'></body></html>");
        WebView.Visibility = Visibility.Visible;
    }

    private void AddUrl_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new AddUrlDialog { Owner = this };
        if (dialog.ShowDialog() != true) return;

        foreach (var line in dialog.EnteredText.Split('\n'))
        {
            var raw = line.Trim();
            if (string.IsNullOrEmpty(raw)) continue;

            var url = raw.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                      raw.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
                ? raw : "https://" + raw;

            if (!Uri.TryCreate(url, UriKind.Absolute, out _)) continue;
            if (_urls.Contains(url)) continue;

            _urls.Add(url);
        }
    }

    private void DeleteUrl_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button btn || btn.Tag is not string url) return;

        var wasSelected = UrlListBox.SelectedItem is string selected && selected == url;
        var index = _urls.IndexOf(url);
        _urls.Remove(url);

        if (!wasSelected || _urls.Count == 0) return;

        UrlListBox.SelectedIndex = Math.Min(index, _urls.Count - 1);
    }

    private void UrlListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (UrlListBox.SelectedItem is string url)
        {
            CurrentUrlText.Text = url;
            WebView.Source = new Uri(url);
        }
    }

    private void CopyUrl_Click(object sender, RoutedEventArgs e)
    {
        if (UrlListBox.SelectedItem is string url)
            Clipboard.SetText(url);
    }

    private async void ResetSession_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new ConfirmDialog("Cookie・キャッシュ・ログイン情報をすべて削除します。\nよろしいですか？") { Owner = this };
        if (dialog.ShowDialog() != true) return;

        await WebView.CoreWebView2.Profile.ClearBrowsingDataAsync();
        WebView.CoreWebView2.NavigateToString("<html><body style='background:#4d4d4d;margin:0'></body></html>");
        CurrentUrlText.Text = "URLを選択してください";
        UrlListBox.SelectedItem = null;
    }
}
