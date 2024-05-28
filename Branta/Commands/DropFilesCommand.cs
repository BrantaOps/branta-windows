using System.Windows.Forms;

namespace Branta.Commands;

public class DropFilesCommand : BaseCommand
{
    private readonly Action<List<string>> _action;

    public DropFilesCommand(Action<List<string>> action)
    {
        _action = action;
    }

    public override void Execute(object parameter)
    {
        var dragEventArgs = (System.Windows.DragEventArgs) parameter;

        if (!dragEventArgs.Data.GetDataPresent(DataFormats.FileDrop))
        {
            return;
        }

        var files = (string[])dragEventArgs.Data.GetData(DataFormats.FileDrop);

        if (files == null)
        {
            return;
        }

        _action.Invoke(files.ToList());
    }
}
