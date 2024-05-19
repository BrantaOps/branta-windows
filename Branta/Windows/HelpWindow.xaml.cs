using System.Windows;
using System.Windows.Input;
using Branta.Classes;
using Branta.Enums;

namespace Branta.Windows;

public partial class HelpWindow
{
    public HelpWindow()
    {
        InitializeComponent();

        SetLanguageDictionary();

        TbLink.Foreground = Color.Brush(Color.Gold);

        TbVersion.Text = "v" + Helper.GetBrantaVersionWithoutCommitHash();
    }

    private void Link_Click(object sender, MouseButtonEventArgs e)
    {
        try
        {
            Helper.OpenLink("https://branta.pro");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error opening webpage: {ex.Message}");
        }
    }
}