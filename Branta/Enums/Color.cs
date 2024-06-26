﻿using System.Windows.Media;

namespace Branta.Enums;

public static class Color
{
    public const string Transparent = "Transparent";
    public const string Gold = "#B1914A";
    public const string WindowBackground = "#1B1B1B";

    public static Brush Brush(string color)
    {
        return (SolidColorBrush)new BrushConverter().ConvertFrom(color);
    }
}