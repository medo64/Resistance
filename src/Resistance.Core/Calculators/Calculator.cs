namespace ResiCalc;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using Medo.Configuration;

/// <summary>
/// Calculator app interface.
/// </summary>
public abstract class Calculator {

    protected Calculator(string name) {
        Name = name;
    }


    /// <summary>
    /// Gets name for the calculator.
    /// </summary>
    public string Name { get; }


    /// <inheritdoc/>
    public override string ToString() {
        return Name;
    }


    #region Elements

    /// <summary>
    /// Returns all measurements for the calculator.
    /// </summary>
    public abstract ReadOnlyCollection<string> GetElementNames();

    private readonly ReadOnlyCollection<KeyValuePair<string, string>> _EmptyExamples = new([]);
    /// <summary>
    /// Returns all example images.
    /// </summary>
    public virtual ReadOnlyCollection<KeyValuePair<string, string>> GetExampleImageResources() {
        return _EmptyExamples;
    }

    #endregion Elements


    #region Store

    private readonly List<string> RecentlyUsed = [];

    /// <summary>
    /// Returns previously stored value.
    /// </summary>
    protected Measurement StoreRead(string measurementName, Measurement defaultValue) {
        var calculatorName = GetType().Name;
        var key = calculatorName + "." + measurementName;

        var value = Config.Read(key, "");
        if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result)) {
            return result;
        } else {
            return defaultValue;
        }
    }

    protected void StoreWrite(string measurementName) {
        RecentlyUsed.Remove(measurementName);
        RecentlyUsed.Add(measurementName);

        var calculatorType = GetType();
        var calculatorName = calculatorType.Name;
        var key = calculatorName + "." + measurementName;

        var valueProperty = calculatorType.GetProperty(measurementName);
        var value = (Measurement)valueProperty?.GetValue(this)!;

        if (value.IsNull) {
            Config.Write(calculatorName + "." + measurementName, "");
        } else {
            Config.Write(calculatorName + "." + measurementName, value.ToString());
        }
    }

    /// <summary>
    /// Returns if second item was more recently changed than the first one.
    /// </summary>
    protected bool IsSecondMoreRecentlyChanged(string measurement1Name, string measurement2Name) {
        var index1 = RecentlyUsed.IndexOf(measurement1Name);
        var index2 = RecentlyUsed.IndexOf(measurement2Name);
        return (index2 > index1);
    }

    #endregion Store


    #region Gui

    public static string GetGuiCategory(Calculator calculator, string elementName) {
        ArgumentNullException.ThrowIfNull(calculator);
        ArgumentNullException.ThrowIfNull(elementName);

        var calculatorType = calculator.GetType();
        var propertyInfo = calculatorType.GetProperty(elementName);
        var methodInfo = calculatorType.GetMethod(elementName);

        if (propertyInfo is not null) {
            foreach (var attr in propertyInfo.GetCustomAttributes(typeof(CategoryAttribute), false)) {
                if (attr is CategoryAttribute categoryAttr) {
                    return categoryAttr.Category;
                }
            }
        }

        if (methodInfo is not null) {
            foreach (var attr in methodInfo.GetCustomAttributes(typeof(CategoryAttribute), false)) {
                if (attr is CategoryAttribute categoryAttr) {
                    return categoryAttr.Category;
                }
            }
        }

        return "";
    }

    public static string GetGuiDisplayName(Calculator calculator, string elementName) {
        ArgumentNullException.ThrowIfNull(calculator);
        ArgumentNullException.ThrowIfNull(elementName);

        var calculatorType = calculator.GetType();
        var propertyInfo = calculatorType.GetProperty(elementName);
        var methodInfo = calculatorType.GetMethod(elementName);

        if (propertyInfo is not null) {
            foreach (var attr in propertyInfo.GetCustomAttributes(typeof(DisplayNameAttribute), false)) {
                if (attr is DisplayNameAttribute displayNameAttr) {
                    if (propertyInfo.GetValue(calculator) is Measurement measurement) {
                        return displayNameAttr.DisplayName
                            .Replace("{SeriesText}", measurement.SeriesText, StringComparison.InvariantCulture);
                    } else {
                        return displayNameAttr.DisplayName;
                    }
                }
            }
        }

        if (methodInfo is not null) {
            foreach (var attr in methodInfo.GetCustomAttributes(typeof(DisplayNameAttribute), false)) {
                if (attr is DisplayNameAttribute displayNameAttr) {
                    return displayNameAttr.DisplayName;
                }
            }
        }

        return elementName;
    }

    public static bool GetGuiIsText(Calculator calculator, string elementName) {
        ArgumentNullException.ThrowIfNull(calculator);
        ArgumentNullException.ThrowIfNull(elementName);

        var calculatorType = calculator.GetType();
        var measurementProperty = calculatorType.GetProperty(elementName);

        return (measurementProperty is not null);
    }

    public static bool GetGuiIsCommand(Calculator calculator, string elementName) {
        ArgumentNullException.ThrowIfNull(calculator);
        ArgumentNullException.ThrowIfNull(elementName);

        var calculatorType = calculator.GetType();
        var commandMethod = calculatorType.GetMethod(elementName);

        return (commandMethod is not null);
    }

    public static bool GetGuiIsReadonly(Calculator calculator, string elementName) {
        ArgumentNullException.ThrowIfNull(calculator);
        ArgumentNullException.ThrowIfNull(elementName);

        var calculatorType = calculator.GetType();
        var measurementProperty = calculatorType.GetProperty(elementName);
        if (measurementProperty is null) { return true; }

        return !measurementProperty.CanWrite;
    }

    public static string GetGuiValue(Calculator calculator, string measurementName) {
        ArgumentNullException.ThrowIfNull(calculator);
        ArgumentNullException.ThrowIfNull(measurementName);

        var calculatorType = calculator.GetType();
        var measurementProperty = calculatorType.GetProperty(measurementName);
        if (measurementProperty is null) { return ""; }

        var valueObject = measurementProperty.GetValue(calculator);
        if (valueObject is Measurement value) {
            var unit = GetGuiUnit(calculator, measurementName);
            return value.ToString(CultureInfo.CurrentCulture, unit);
        } else if (valueObject is string text) {
            return text;
        } else {
            return "";
        }
    }

    public static void SetGuiValue(Calculator calculator, string measurementName, string text) {
        ArgumentNullException.ThrowIfNull(calculator);
        ArgumentNullException.ThrowIfNull(measurementName);
        ArgumentNullException.ThrowIfNull(text);

        var calculatorType = calculator.GetType();
        var measurementProperty = calculatorType.GetProperty(measurementName);
        if (measurementProperty is null) { return; }

        if (string.IsNullOrWhiteSpace(text)) {
            measurementProperty.SetValue(calculator, Measurement.Null);
            return;
        }

        var isValue = true;
        var sbSuffix = new StringBuilder();
        var sbValue = new StringBuilder();
        foreach (var ch in text) {
            if (isValue) {
                if (char.IsAsciiDigit(ch) || (ch == '.') || (ch == ',')) {
                    sbValue.Append(ch);
                } else if (!char.IsWhiteSpace(ch)) {
                    isValue = false;
                    sbSuffix.Append(ch);
                }
            } else if (!char.IsWhiteSpace(ch)) {
                sbSuffix.Append(ch);
            }
        }

        if (decimal.TryParse(sbValue.ToString(), NumberStyles.Any, CultureInfo.CurrentCulture, out var value)) {
            var suffix = sbSuffix.ToString();
            var unit = GetGuiUnit(calculator, measurementName);
            if (suffix.Length >= 1) {
                var suffixIndex = suffix.IndexOf(unit, 1, StringComparison.Ordinal);
                if (suffixIndex > 0) { suffix = suffix[..suffixIndex]; }
            } else {
                suffix = " ";
            }

            var multiplier = suffix[0] switch {
                'R' => 1_000_000_000_000_000_000_000_000_000m,
                'Y' => 1_000_000_000_000_000_000_000_000m,
                'Z' => 1_000_000_000_000_000_000_000m,
                'E' => 1_000_000_000_000_000_000m,
                'P' => 1_000_000_000_000_000m,
                'T' => 1_000_000_000_000m,
                'G' => 1_000_000_000m,
                'M' => 1_000_000m,
                'k' => 1_000m,
                'm' => 0.001m,
                'μ' or 'u' => 0.000_001m,
                'n' => 0.000_000_001m,
                'p' => 0.000_000_000_001m,
                'f' => 0.000_000_000_000_001m,
                'a' => 0.000_000_000_000_000_001m,
                'z' => 0.000_000_000_000_000_000_001m,
                'y' => 0.000_000_000_000_000_000_000_001m,
                'r' => 0.000_000_000_000_000_000_000_000_001m,
                _ => 1,
            };

            var newValue = value * multiplier;
            var oldValue = (decimal)(Measurement)measurementProperty.GetValue(calculator)!;
            if (newValue != oldValue) {
                try {
                    measurementProperty.SetValue(calculator, (Measurement)newValue);
                } catch (Exception) { }  // TODO: readonly
            }
        }
    }

    public static string GetGuiUnit(Calculator calculator, string measurementName) {
        ArgumentNullException.ThrowIfNull(calculator);
        ArgumentNullException.ThrowIfNull(measurementName);

        var calculatorType = calculator.GetType();
        var measurementProperty = calculatorType.GetProperty(measurementName);

        if (measurementProperty is not null) {
            foreach (var attr in measurementProperty.GetCustomAttributes(typeof(MeasurementUnitAttribute), false)) {
                if (attr is MeasurementUnitAttribute unitAttr) {
                    return unitAttr.Unit ?? "";
                }
            }
        }

        return "";
    }

    public static MethodInfo? GetGuiMethodInfo(Calculator calculator, string measurementName) {
        ArgumentNullException.ThrowIfNull(calculator);
        ArgumentNullException.ThrowIfNull(measurementName);

        var calculatorType = calculator.GetType();
        var methodInfo = calculatorType.GetMethod(measurementName);

        if (methodInfo is not null) {
            return methodInfo;
        }

        return null;
    }

    public static (byte red, byte green, byte blue)? GetGuiBackgroundColor(Calculator calculator, string measurementName) {
        ArgumentNullException.ThrowIfNull(calculator);
        ArgumentNullException.ThrowIfNull(measurementName);

        var calculatorType = calculator.GetType();
        var methodInfo = calculatorType.GetMethod(measurementName);

        if (methodInfo is not null) {
            foreach (var attr in methodInfo.GetCustomAttributes(typeof(BackgroundColorAttribute), false)) {
                if (attr is BackgroundColorAttribute colorAttr) {
                    return (colorAttr.Red, colorAttr.Green, colorAttr.Blue);
                }
            }
        }

        return null;
    }

    #endregion Gui


    #region Static

    private readonly static ReadOnlyCollection<Calculator> _AllCalculators = new([
        new ESeries(),
        new LdoPower(),
        new Led(),
        new LM117(),
        new LM317(),
        new MicrochipPicPwm(),
        new MicrochipPicTmr0(),
        new OhmLaw(),
        new ParallelAndSeries(),
        new VoltageDivider(),
    ]);

    /// <summary>
    /// Returns all available calculators
    /// </summary>
    public static ReadOnlyCollection<Calculator> AllCalculators => _AllCalculators;

    #endregion Static

}
