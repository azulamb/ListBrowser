using System.Windows;

namespace ListBrowser;

public partial class AddUrlDialog : Window
{
    public string EnteredText { get; private set; } = string.Empty;

    public AddUrlDialog()
    {
        InitializeComponent();
        Loaded += (_, _) => UrlTextBox.Focus();
    }

    private void Add_Click(object sender, RoutedEventArgs e)
    {
        EnteredText = UrlTextBox.Text;
        DialogResult = true;
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
    }
}
