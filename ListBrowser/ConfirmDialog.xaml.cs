using System.Windows;

namespace ListBrowser;

public partial class ConfirmDialog : Window
{
    public ConfirmDialog(string message)
    {
        InitializeComponent();
        MessageText.Text = message;
    }

    private void Ok_Click(object sender, RoutedEventArgs e) => DialogResult = true;
    private void Cancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
}
