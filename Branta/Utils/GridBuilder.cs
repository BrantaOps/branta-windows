using System.Windows;
using Branta.Enums;
using System.Windows.Controls;

namespace Branta.Utils;

public class GridBuilder
{
    private readonly Grid _grid;
    private int _column = 0;

    public GridBuilder(Color backgroundColor)
    {
        _grid = new Grid()
        {
            Background = backgroundColor.Brush()
        };
    }

    public GridBuilder AddColumnDefinition(int width, TextBlock element)
    {
        var columnDefinition = new ColumnDefinition
        {
            Width = new GridLength(width)
        };

        return AddColumnDefinition(columnDefinition, element);
    }

    public GridBuilder AddColumnDefinition(ColumnDefinition columnDefinition, TextBlock element)
    {
        _grid.ColumnDefinitions.Add(columnDefinition);

        Grid.SetColumn(element, _column);
        _grid.Children.Add(element);

        _column++;
        return this;
    }

    public Grid Build()
    {
        return _grid;
    }
}