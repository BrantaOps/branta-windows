using System.Windows.Media;
using Ardalis.SmartEnum;

namespace Branta.Enums;

public sealed class Color : SmartEnum<Color>
{
    public static readonly Color Gold = new (0, "#B1914A");
    public static readonly Color Red = new (1, "#944545");
    public static readonly Color Blue = new(2, "#547AA6");

    public Color(int value, string name) : base(name, value)
    {
    }

    public Brush Brush()
    {
        return (SolidColorBrush)new BrushConverter().ConvertFrom(Name);
    }
}