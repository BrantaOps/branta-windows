using Branta.ViewModels;
using DragEventArgs = System.Windows.DragEventArgs;
using UserControl = System.Windows.Controls.UserControl;

namespace Branta.Views;

public partial class InstallerVerificationView : UserControl
{
    public InstallerVerificationView()
    {
        InitializeComponent();
    }

    private void VerifyInstaller_Drop(object sender, DragEventArgs e)
    {
        var viewModel = (InstallerVerificationViewModel)DataContext;

        viewModel.DropFilesCommand.Execute(e);
    }
}
