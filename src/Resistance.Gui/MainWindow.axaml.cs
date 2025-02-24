namespace ResiCalc;

using System;
using System.Runtime.Intrinsics.X86;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Styling;
using Medo.Configuration;

internal partial class MainWindow : Window {
    public MainWindow() {
        InitializeComponent();

        foreach (var calculator in Calculator.AllCalculators) {
            lsbCalculators.Items.Add(calculator);
        }
        lsbCalculators.SelectedItem = lsbCalculators.Items[0];
    }


    protected override void OnLoaded(RoutedEventArgs e) {
        var splitPosition = Config.Read("SplitPosition", 224.0);
        if (splitPosition > Width / 2) { splitPosition = Width / 2; }
        grdMain.ColumnDefinitions[0].Width = new GridLength(splitPosition, GridUnitType.Pixel);
        base.OnLoaded(e);
    }

    protected override void OnClosed(EventArgs e) {
        Config.Write("SplitPosition", grdMain.ColumnDefinitions[0].Width.Value);
        base.OnClosed(e);
    }


    private void lsbCalculators_SelectionChanged(object? sender, SelectionChangedEventArgs e) {
        pnlMeasurements.Children.Clear();
        if (lsbCalculators.SelectedItem is not Calculator calculator) { return; }

        var blockUpdates = true;

        var lastCategory = "";
        var pnlGroup = new StackPanel() { };
        pnlMeasurements.Children.Add(pnlGroup);

        foreach (var measurementName in calculator.GetMeasurementNames()) {
            var category = Calculator.GetGuiCategory(calculator, measurementName);
            if (category != lastCategory) {
                pnlGroup = new StackPanel() { };
                pnlMeasurements.Children.Add(pnlGroup);

                var categoryTextBlock = new TextBlock() {
                    Foreground = BrushHelpers.SystemBaseMediumHighColor,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new(0, 16, 0, 0),
                    Text = "- " + category + " -",
                };
                pnlGroup.Children.Add(categoryTextBlock);

                lastCategory = category;
            }

            var displayName = Calculator.GetGuiDisplayName(calculator, measurementName) + ":";
            var displayNameTextBlock = new TextBlock() {
                Foreground = BrushHelpers.SystemBaseMediumHighColor,
                HorizontalAlignment = HorizontalAlignment.Left,
                Text = displayName,
            };
            pnlGroup.Children.Add(displayNameTextBlock);

            var value = Calculator.GetGuiValue(calculator, measurementName);
            var valueTextBox = new TextBox() {
                FontSize = FontSize * 1.3f,
                Margin = new Thickness(0, 0, 0, 16),
                Text = value,
                TextAlignment = TextAlignment.Right,
                Tag = measurementName,
            };
            valueTextBox.LostFocus += (sender, e) => {
                if (blockUpdates) { return; }
                Calculator.SetGuiValue(calculator, measurementName, valueTextBox.Text);
                UpdateAll(ref blockUpdates, calculator);
            };
            pnlGroup.Children.Add(valueTextBox);
        }

        blockUpdates = false;
    }

    private void UpdateAll(ref bool blockUpdates, Calculator calculator) {
        blockUpdates = true;

        foreach (var child in pnlMeasurements.Children) {
            if (child is StackPanel group) {
                foreach (var element in group.Children) {
                    if (element is TextBox textBox) {
                        if (textBox.Tag is not string measurementName) { continue; }
                        var value = Calculator.GetGuiValue(calculator, measurementName);
                        if (textBox.Text != value) { textBox.Text = value; }
                    }
                }
            }

        }
        blockUpdates = false;
    }
}
