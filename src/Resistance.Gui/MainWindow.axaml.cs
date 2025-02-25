namespace ResiCalc;

using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
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
        pnlGroupsPrimary.Children.Clear();
        pnlGroupsSecondary.Children.Clear();
        pnlOther.Children.Clear();
        if (lsbCalculators.SelectedItem is not Calculator calculator) { return; }

        var blockUpdates = true;

        string? lastCategory = null;
        StackPanel? pnlGroup = null;

        foreach (var elementName in calculator.GetElementNames()) {
            var category = Calculator.GetGuiCategory(calculator, elementName);
            if ((category != lastCategory) || (pnlGroup is null)) {
                lastCategory = category;
                var isContinuingCategory = category.StartsWith('~');
                var isPrimaryPanel = (pnlGroup is null);
                category = category.TrimStart('~');

                if (!isContinuingCategory || (pnlGroup is null)) {
                    pnlGroup = new StackPanel() {
                        Margin = new(8, 0, 8, 0),
                        Width = 160,
                    };
                    if (isPrimaryPanel) {
                        pnlGroupsPrimary.Children.Add(pnlGroup);
                    } else {
                        pnlGroupsSecondary.Children.Add(pnlGroup);
                    }
                }

                var categoryTextBlock = new TextBlock() {
                    FontSize = FontSize * 0.8f,
                    Foreground = BrushHelpers.SystemBaseMediumHighColor,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new(0, isContinuingCategory ? 16 : 0, 0, 4),
                    Text = !string.IsNullOrEmpty(category) ? "- " + category + " -" : "",
                };
                pnlGroup.Children.Add(categoryTextBlock);
            }

            var displayName = Calculator.GetGuiDisplayName(calculator, elementName);

            if (Calculator.GetGuiIsText(calculator, elementName)) {

                var displayNameTextBlock = new TextBlock() {
                    Foreground = BrushHelpers.SystemBaseMediumHighColor,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Text = displayName + ":",
                };
                pnlGroup.Children.Add(displayNameTextBlock);

                var isReadonly = Calculator.GetGuiIsReadonly(calculator, elementName);

                var value = Calculator.GetGuiValue(calculator, elementName);
                var valueTextBox = new TextBox() {
                    FontSize = FontSize * 1.3f,
                    IsReadOnly = isReadonly,
                    Margin = new Thickness(0, 0, 0, 16),
                    Text = value,
                    TextAlignment = TextAlignment.Right,
                    Tag = new TagBag {
                        ElementName = elementName
                    }
                };
                if (!isReadonly) {
                    valueTextBox.GotFocus += (sender, e) => {
                        var bag = (TagBag)((TextBox)sender!).Tag!;
                        bag.AnyChange = false;
                    };
                    valueTextBox.TextChanged += (sender, e) => {
                        var bag = (TagBag)((TextBox)sender!).Tag!;
                        bag.AnyChange = true;
                    };
                    valueTextBox.LostFocus += (sender, e) => {
                        if (blockUpdates) { return; }
                        var bag = (TagBag)((TextBox)sender!).Tag!;
                        if (bag.AnyChange) {
                            Calculator.SetGuiValue(calculator, elementName, valueTextBox.Text);
                            UpdateAll(ref blockUpdates, calculator);
                        }
                    };
                }
                pnlGroup.Children.Add(valueTextBox);

            } else if (Calculator.GetGuiIsCommand(calculator, elementName)) {

                var commandButton = new Button() {
                    Content = displayName,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(4),
                };
                commandButton.Click += (sender, e) => {
                    var methodInfo = Calculator.GetGuiMethodInfo(calculator, elementName);
                    if (methodInfo is not null) {
                        methodInfo.Invoke(calculator, []);
                        UpdateAll(ref blockUpdates, calculator);
                    }
                };
                pnlGroup.Children.Add(commandButton);

            }

        }

        foreach (var exampleImageResource in calculator.GetExampleImageResources()) {
            var resourceName = exampleImageResource.Key;
            var caption = exampleImageResource.Value;

            var imageStack = new StackPanel();
            pnlOther.Children.Add(imageStack);

            var captionTextBlock = new TextBlock() {
                FontSize = FontSize * 0.8f,
                Foreground = BrushHelpers.SystemBaseMediumHighColor,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new(0, 0, 0, 4),
                Text = caption,
            };
            imageStack.Children.Add(captionTextBlock);

            var bitmapUri = new Uri("avares://resistance/Assets/Examples/" + resourceName);
            var bitmap = new Bitmap(AssetLoader.Open(bitmapUri));
            var imageControl = new Image() {
                Source = bitmap,
                Stretch = Stretch.None,
            };
            imageStack.Children.Add(imageControl);
        }

        blockUpdates = false;
    }

    private void UpdateAll(ref bool blockUpdates, Calculator calculator) {
        blockUpdates = true;
        UpdateControls(calculator, pnlGroupsPrimary.Children);
        UpdateControls(calculator, pnlGroupsSecondary.Children);
        blockUpdates = false;
    }

    private void UpdateControls(Calculator calculator, Controls controls) {
        foreach (var control in controls) {
            if (control is StackPanel subcontrols) {
                UpdateControls(calculator, subcontrols.Children);
            } else if (control is TextBox textBox) {
                var bag = textBox.Tag as TagBag;
                if ((bag is not null) && (bag.ElementName is not null)) {
                    var value = Calculator.GetGuiValue(calculator, bag.ElementName);
                    if (textBox.Text != value) { textBox.Text = value; }
                }
            }

        }
    }
}
