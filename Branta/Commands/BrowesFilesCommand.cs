using System.Windows.Forms;

namespace Branta.Commands;

public class BrowseFilesCommand : BaseCommand
{
    private readonly Action<List<string>> _action;

    public BrowseFilesCommand(Action<List<string>> action)
    {
        _action = action;
    }

    public override void Execute(object parameter)
    {
        var openFileDialog = new OpenFileDialog
        {
            Multiselect = true
        };

        openFileDialog.ShowDialog();

        var files = openFileDialog.FileNames.ToList();

        _action.Invoke(files);
    }
}
