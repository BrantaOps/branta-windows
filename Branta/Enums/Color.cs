using System.Windows.Media;
using Ardalis.SmartEnum;

namespace Branta.Enums;

public sealed class Color : SmartEnum<Color>
{
    public static readonly Color Background = new(0, "#444444");
    public static readonly Color BackgroundOffset = new(1, "#4c4c4c");
    public static readonly Color White = new(2, "#ffffff");
    public static readonly Color Gold = new (3, "#B1914A");
    public static readonly Color Red = new (4, "#944545");

    public Color(int value, string name) : base(name, value)
    {
    }

    public Brush Brush()
    {
        return (SolidColorBrush)new BrushConverter().ConvertFrom(Name);
    }
}