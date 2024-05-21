using Branta.Classes;

namespace Branta.Commands;

public class HelpCommand : BaseCommand
{
    public override void Execute(object parameter)
    {
        Helper.OpenLink("https://branta.pro/docs");
    }
}