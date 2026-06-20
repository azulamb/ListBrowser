using System.Windows;
using System.Windows.Input;

namespace ListBrowser;

public partial class AddUrlDialog : Window
{
    public string EnteredUrl { get; private set; } = string.Empty;

    public AddUrlDialog()
    {
        InitializeComponent();
        Loaded += (_, _) => UrlTextBox.Focus();
    }

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        EnteredUrl = UrlTextBox.Text;
        DialogResult = true;
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }

    private void UrlTextBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            EnteredUrl = UrlTextBox.Text;
            DialogResult = true;
        }
    }
}
