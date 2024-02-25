using Branta.Classes;
using Branta.Enums;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Branta.Views;

public partial class HelpWindow : BaseWindow
{
    public HelpWindow()
    {
        InitializeComponent();

        TbLink.Foreground = Color.Brush(Color.Gold);

        TbVersion.Text = "v" + Helper.GetBrantaVersionWithoutCommitHash();
    }

    private void Link_Click(object sender, MouseButtonEventArgs e)
    {
        try
        {
            Process.Start(new ProcessStartInfo("cmd", "/c start https://branta.pro") { CreateNoWindow = true });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error opening webpage: {ex.Message}");
        }
    }
}
