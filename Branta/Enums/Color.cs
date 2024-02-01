using System.Windows.Media;

namespace Branta.Enums;

public static class Color
{
    public const string Gold = "#B1914A";
    public const string Red = "#944545";

    public static Brush Brush(string color)
    {
        return (SolidColorBrush)new BrushConverter().ConvertFrom(color);
    }
}