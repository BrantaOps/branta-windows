using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using Branta.Enums;

namespace Branta.Views;

public partial class HelpWindow : Window
{
    public HelpWindow()
    {
        InitializeComponent();

        TbLink.Foreground = Color.Gold.Brush();

        TbVersion.Text = GetBrantaVersionWithoutCommitHash();
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

    private static string GetBrantaVersionWithoutCommitHash()
    {
        var version = Assembly.GetEntryAssembly()!.GetCustomAttribute<AssemblyInformationalVersionAttribute>()!.InformationalVersion;

        return "v" + version.Split("+")[0];
    }
}
