namespace ResiCalc;

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using Medo.Configuration;

internal static class BrushHelpers {

    public static ISolidColorBrush SystemBaseMediumHighColor => GetBrush("SystemBaseMediumHighColor", Brushes.DarkGray, Brushes.LightGray);


    private static ISolidColorBrush GetBrush(string name, ISolidColorBrush lightDefault, ISolidColorBrush darkDefault) {
        var variant = Application.Current?.ActualThemeVariant ?? ThemeVariant.Light;
        if (Application.Current?.Styles[0] is IResourceProvider provider && provider.TryGetResource(name, variant, out var resource)) {
            if (resource is Color color) {
                return new SolidColorBrush(color);
            }
        }
        return (variant == ThemeVariant.Light) ? lightDefault : darkDefault;
    }

}
