using DragEventArgs = System.Windows.DragEventArgs;
using UserControl = System.Windows.Controls.UserControl;

namespace Branta.Features.InstallerVerification;

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
