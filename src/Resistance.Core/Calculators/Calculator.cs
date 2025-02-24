namespace ResiCalc;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using Medo.Configuration;

/// <summary>
/// Calculator app interface.
/// </summary>
public abstract class Calculator {

    protected Calculator(string name, string description) {
        Name = name;
        Description = description;
    }


    /// <summary>
    /// Gets name for the calculator.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets description for the calculator.
    /// </summary>
    public string Description { get; }

    /// <inheritdoc/>
    public override string ToString() {
        return Name;
    }


    #region Elements

    /// <summary>
    /// Returns all measurements for the calculator.
    /// </summary>
    public abstract ReadOnlyCollection<string> GetMeasurementNames();

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
            Config.Write(calculatorName + "." + measurementName, value.ToString(CultureInfo.InvariantCulture));
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


    #region Static

    private readonly static ReadOnlyCollection<Calculator> _AllCalculators = new([new OhmLaw()]);
    /// <summary>
    /// Returns all available calculators
    /// </summary>
    public static ReadOnlyCollection<Calculator> AllCalculators => _AllCalculators;

    #endregion Static


    #region Gui

    public static string GetGuiCategory(Calculator calculator, string measurementName) {
        ArgumentNullException.ThrowIfNull(calculator);
        ArgumentNullException.ThrowIfNull(measurementName);

        var calculatorType = calculator.GetType();
        var measurementProperty = calculatorType.GetProperty(measurementName);

        if (measurementProperty is not null) {
            foreach (var attr in measurementProperty.GetCustomAttributes(typeof(CategoryAttribute), false)) {
                if (attr is CategoryAttribute categoryAttr) {
                    return categoryAttr.Category;
                }
            }
        }

        return "";
    }

    public static string GetGuiDisplayName(Calculator calculator, string measurementName) {
        ArgumentNullException.ThrowIfNull(calculator);
        ArgumentNullException.ThrowIfNull(measurementName);

        var calculatorType = calculator.GetType();
        var measurementProperty = calculatorType.GetProperty(measurementName);

        if (measurementProperty is not null) {
            foreach (var attr in measurementProperty.GetCustomAttributes(typeof(DisplayNameAttribute), false)) {
                if (attr is DisplayNameAttribute displayNameAttr) {
                    return displayNameAttr.DisplayName;
                }
            }
        }

        return measurementName;
    }

    public static string GetGuiValue(Calculator calculator, string measurementName) {
        ArgumentNullException.ThrowIfNull(calculator);
        ArgumentNullException.ThrowIfNull(measurementName);

        var calculatorType = calculator.GetType();
        var measurementProperty = calculatorType.GetProperty(measurementName);
        if (measurementProperty is null) { return ""; }

        var value = (Measurement)measurementProperty.GetValue(calculator)!;
        var unit = GetGuiUnit(calculator, measurementName);

        if (value.IsNull) {
            return "";
        } else {
            var number = (decimal)value;
            return number switch {
                >= 1_000_000_000_000_000_000_000_000_000m => (number / 1_000_000_000_000_000_000_000_000_000m).ToString("0.###", CultureInfo.CurrentCulture) + " R" + unit,
                >= 1_000_000_000_000_000_000_000_000m => (number / 1_000_000_000_000_000_000_000_000m).ToString("0.###", CultureInfo.CurrentCulture) + " Y" + unit,
                >= 1_000_000_000_000_000_000_000m => (number / 1_000_000_000_000_000_000_000m).ToString("0.###", CultureInfo.CurrentCulture) + " Z" + unit,
                >= 1_000_000_000_000_000_000m => (number / 1_000_000_000_000_000_000m).ToString("0.###", CultureInfo.CurrentCulture) + " E" + unit,
                >= 1_000_000_000_000_000m => (number / 1_000_000_000_000_000m).ToString("0.###", CultureInfo.CurrentCulture) + " P" + unit,
                >= 1_000_000_000_000m => (number / 1_000_000_000_000m).ToString("0.###", CultureInfo.CurrentCulture) + " T" + unit,
                >= 1_000_000_000m => (number / 1_000_000_000m).ToString("0.###", CultureInfo.CurrentCulture) + " G" + unit,
                >= 1_000_000m => (number / 1_000_000m).ToString("0.###", CultureInfo.CurrentCulture) + " M" + unit,
                >= 1000m => (number / 1_000m).ToString("0.###", CultureInfo.CurrentCulture) + " k" + unit,
                >= 1m => number.ToString("0.###", CultureInfo.CurrentCulture) + " " + unit,
                >= 0.001m => (number * 1_000m).ToString("0.###", CultureInfo.CurrentCulture) + " m" + unit,
                >= 0.000_001m => (number * 1_000_000m).ToString("0.###", CultureInfo.CurrentCulture) + " μ" + unit,
                >= 0.000_000_001m => (number * 1_000_000_000m).ToString("0.###", CultureInfo.CurrentCulture) + " n" + unit,
                >= 0.000_000_000_001m => (number * 1_000_000_000_000m).ToString("0.###", CultureInfo.CurrentCulture) + " p" + unit,
                >= 0.000_000_000_000_001m => (number * 1_000_000_000_000_000m).ToString("0.###", CultureInfo.CurrentCulture) + " f" + unit,
                >= 0.000_000_000_000_000_001m => (number * 1_000_000_000_000_000_000m).ToString("0.###", CultureInfo.CurrentCulture) + " a" + unit,
                >= 0.000_000_000_000_000_000_001m => (number * 1_000_000_000_000_000_000_000m).ToString("0.###", CultureInfo.CurrentCulture) + " z" + unit,
                >= 0.000_000_000_000_000_000_000_001m => (number * 1_000_000_000_000_000_000_000_000m).ToString("0.###", CultureInfo.CurrentCulture) + " y" + unit,
                _ => (number * 1_000_000_000_000_000_000_000_000_000m).ToString("0.###", CultureInfo.CurrentCulture) + " r" + unit,
            };
        }
    }

    public static void SetGuiValue(Calculator calculator, string measurementName, string text) {
        ArgumentNullException.ThrowIfNull(calculator);
        ArgumentNullException.ThrowIfNull(measurementName);
        ArgumentNullException.ThrowIfNull(text);

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

        var calculatorType = calculator.GetType();
        var measurementProperty = calculatorType.GetProperty(measurementName);
        if (measurementProperty is null) { return; }

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

            var newValue = new Measurement(value * multiplier);
            var oldValue = (Measurement)measurementProperty.GetValue(calculator)!;
            if (newValue != oldValue) {
                measurementProperty.SetValue(calculator, newValue);
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

    #endregion Gui

}
