namespace ResiCalc;

using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

/// <summary>
/// Measurement instance.
/// </summary>
public readonly struct Measurement : IEquatable<Measurement> {

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public Measurement() {
        Value = null;
        DigitCount = -3;
    }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <param name="value">Value.</param>
    public Measurement(decimal? value) {
        Value = value;
        DigitCount = -3;
    }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <param name="series">Series.</param>
    /// <param name="rounding">Rounding.</param>
    /// <param name="digitCount">Negative number uses rounding using significant digits, while positive number controls how many decimal places to use.</param>
    internal Measurement(decimal? value, NumberSeries series, NumberSeriesRounding rounding, int? digitCount) {
        if (series == NumberSeries.None) {
            Value = value;
        } else {
            Value = RoundToSeries(value, series, rounding);
        }
        if (digitCount is null) {
            DigitCount = null;
        } else if (digitCount >= 0) {
            DigitCount = Math.Min(Math.Max(digitCount.Value, 0), 6);  // 0 - 6 decimal digits
        } else {
            DigitCount = Math.Min(Math.Max(digitCount.Value, -6), 0);  // 1 to 6 significant digits
        }
    }

    private readonly decimal? Value;
    private readonly int? DigitCount;


    /// <summary>
    /// Gets if value is null.
    /// </summary>
    public bool IsNull => Value is null;


    #region Overrides

    public bool Equals(Measurement other) {
        return Value.Equals(other.Value);
    }

    public override bool Equals(object? obj) {
        return Value.Equals(obj);
    }

    public override int GetHashCode() {
        return Value.GetHashCode();
    }

    #endregion Overrides


    #region Formatting

    public override string ToString() {
        if (Value is null) { return string.Empty; }
        return Value.Value.ToString(CultureInfo.InvariantCulture);
    }

    public string ToString(CultureInfo? provider, string unit) {
        if (Value is null) { return ""; }
        provider ??= CultureInfo.CurrentCulture;

        var value = Value.Value;
        var number = value;
        return number switch {
            >= 1_000_000_000_000_000_000_000_000_000m => GetString(provider, number / 1_000_000_000_000_000_000_000_000_000m, 'R', unit),
            >= 1_000_000_000_000_000_000_000_000m => GetString(provider, number / 1_000_000_000_000_000_000_000_000m, 'Y', unit),
            >= 1_000_000_000_000_000_000_000m => GetString(provider, number / 1_000_000_000_000_000_000_000m, 'Z', unit),
            >= 1_000_000_000_000_000_000m => GetString(provider, number / 1_000_000_000_000_000_000m, 'E', unit),
            >= 1_000_000_000_000_000m => GetString(provider, number / 1_000_000_000_000_000m, 'P', unit),
            >= 1_000_000_000_000m => GetString(provider, number / 1_000_000_000_000m, 'T', unit),
            >= 1_000_000_000m => GetString(provider, number / 1_000_000_000m, 'G', unit),
            >= 1_000_000m => GetString(provider, number / 1_000_000m, 'M', unit),
            >= 1000m => GetString(provider, number / 1_000m, 'k', unit),
            >= 1m => GetString(provider, number, ' ', unit),
            >= 0.001m => GetString(provider, number * 1_000m, 'm', unit),
            >= 0.000_001m => GetString(provider, number * 1_000_000m, 'μ', unit),
            >= 0.000_000_001m => GetString(provider, number * 1_000_000_000m, 'n', unit),
            >= 0.000_000_000_001m => GetString(provider, number * 1_000_000_000_000m, 'p', unit),
            >= 0.000_000_000_000_001m => GetString(provider, number * 1_000_000_000_000_000m, 'f', unit),
            >= 0.000_000_000_000_000_001m => GetString(provider, number * 1_000_000_000_000_000_000m, 'a', unit),
            >= 0.000_000_000_000_000_000_001m => GetString(provider, number * 1_000_000_000_000_000_000_000m, 'z', unit),
            >= 0.000_000_000_000_000_000_000_001m => GetString(provider, number * 1_000_000_000_000_000_000_000_000m, 'y', unit),
            _ => GetString(provider, number * 1_000_000_000_000_000_000_000_000_000m, 'r', unit),
        };
    }

    private string GetString(CultureInfo provider, decimal scaledValue, char si, string unit) {
        var sb = new StringBuilder();

        if (DigitCount is null) {  // decimal digits
            sb.Append(scaledValue.ToString("0.###", provider));
        } else if (DigitCount >= 0) {  // decimal digits
            sb.AppendFormat(provider, "0." + new string('0', DigitCount.Value), scaledValue);
        } else {  // significant digits
            var index = 0;
            while (scaledValue >= 10) {
                index -= 1;
                scaledValue /= 10m;
            }
            var lastIndex = Math.Max(index + (-DigitCount.Value) - 1, 0);
            for (var i = index; i <= lastIndex; i++) {
                var digit = (i != lastIndex)
                          ? Math.Floor((decimal)scaledValue)
                          : Math.Round((decimal)scaledValue, 0, MidpointRounding.AwayFromZero);
                sb.Append(digit);
                if ((i == 0) && (i != lastIndex)) { sb.Append(provider.NumberFormat.NumberDecimalSeparator); }
                scaledValue -= digit;
                scaledValue *= 10;
            }
        }

        if (string.IsNullOrEmpty(unit)) {
            if (si != ' ') { sb.Append(si); }
        } else {
            sb.Append(" " + (si + unit).Trim());
        }
        return sb.ToString();
    }


    #endregion


    #region Operators

    public static implicit operator Measurement(decimal? value) => new(value);

    public static implicit operator Measurement(decimal value) => new(value);

    public static implicit operator decimal?(Measurement value) => value.Value;

    public static implicit operator decimal(Measurement value) => value.Value ?? 0;


    public static bool operator ==(Measurement left, Measurement right) {
        return left.Value.Equals(right.Value);
    }

    public static bool operator !=(Measurement left, Measurement right) {
        return !(left.Value == right.Value);
    }

    public static bool operator <(Measurement left, Measurement right) {
        return left.Value < right.Value;
    }

    public static bool operator >(Measurement left, Measurement right) {
        return left.Value > right.Value;
    }

    public static bool operator <=(Measurement left, Measurement right) {
        return left.Value <= right.Value;
    }

    public static bool operator >=(Measurement left, Measurement right) {
        return left.Value >= right.Value;
    }

    public static Measurement operator +(Measurement left, Measurement right) {
        try { return new(left.Value + right.Value); } catch (ArithmeticException) { return Null; }
    }

    public static Measurement operator -(Measurement left, Measurement right) {
        try { return new(left.Value - right.Value); } catch (ArithmeticException) { return Null; }
    }

    public static Measurement operator *(Measurement left, Measurement right) {
        try { return new(left.Value * right.Value); } catch (ArithmeticException) { return Null; }
    }

    public static Measurement operator /(Measurement left, Measurement right) {
        try { return new(left.Value / right.Value); } catch (ArithmeticException) { return Null; }
    }

    #endregion Operators


    #region Singletons

    /// <summary>
    /// Gets null value.
    /// </summary>
    public static Measurement Null { get; } = new();

    /// <summary>
    /// Gets 10^-27 value.
    /// </summary>
    public static Measurement Ronto { get; } = new(0.000_000_000_000_000_000_000_000_001m);

    /// <summary>
    /// Gets 10^-24 value.
    /// </summary>
    public static Measurement Yocto { get; } = new(0.000_000_000_000_000_000_000_001m);

    /// <summary>
    /// Gets 10^-21 value.
    /// </summary>
    public static Measurement Zepto { get; } = new(0.000_000_000_000_000_000_001m);

    /// <summary>
    /// Gets 10^-18 value.
    /// </summary>
    public static Measurement Atto { get; } = new(0.000_000_000_000_000_001m);

    /// <summary>
    /// Gets 10^-15 value.
    /// </summary>
    public static Measurement Femto { get; } = new(0.000_000_000_000_001m);

    /// <summary>
    /// Gets 10^-12 value.
    /// </summary>
    public static Measurement Pico { get; } = new(0.000_000_000_001m);

    /// <summary>
    /// Gets 10^-9 value.
    /// </summary>
    public static Measurement Nano { get; } = new(0.000_000_001m);

    /// <summary>
    /// Gets 10^-6 value.
    /// </summary>
    public static Measurement Micro { get; } = new(0.000_001m);

    /// <summary>
    /// Gets 10^-3 value.
    /// </summary>
    public static Measurement Milli { get; } = new(0.001m);

    /// <summary>
    /// Gets unit value.
    /// </summary>
    public static Measurement Unit { get; } = new(1m);

    /// <summary>
    /// Gets 10^3 value.
    /// </summary>
    public static Measurement Kilo { get; } = new(1_000m);

    /// <summary>
    /// Gets 10^6 value.
    /// </summary>
    public static Measurement Mega { get; } = new(1_000_000m);

    /// <summary>
    /// Gets 10^9 value.
    /// </summary>
    public static Measurement Giga { get; } = new(1_000_000_000m);

    /// <summary>
    /// Gets 10^12 value.
    /// </summary>
    public static Measurement Tera { get; } = new(1_000_000_000_000m);

    /// <summary>
    /// Gets 10^15 value.
    /// </summary>
    public static Measurement Peta { get; } = new(1_000_000_000_000_000m);

    /// <summary>
    /// Gets 10^18 value.
    /// </summary>
    public static Measurement Exa { get; } = new(1_000_000_000_000_000_000m);

    /// <summary>
    /// Gets 10^21 value.
    /// </summary>
    public static Measurement Zetta { get; } = new(1_000_000_000_000_000_000_000m);

    /// <summary>
    /// Gets 10^24 value.
    /// </summary>
    public static Measurement Yotta { get; } = new(1_000_000_000_000_000_000_000_000m);

    /// <summary>
    /// Gets 10^27 value.
    /// </summary>
    public static Measurement Ronna { get; } = new(1_000_000_000_000_000_000_000_000_000m);

    #endregion Singletons


    #region Rounding

    private static decimal? RoundToSeries(decimal? number, NumberSeries series, NumberSeriesRounding rounding) {
        if (number is null) { return null; }
        if (series == NumberSeries.None) { return number; }
        var value = number.Value;

        decimal[] numbers = series switch {
            NumberSeries.E3 => [
                                    100, 220, 470,
                               ],
            NumberSeries.E6 => [
                                    100, 150, 220, 330, 470, 680,
                               ],
            NumberSeries.E12 => [
                                    100, 120, 150, 180, 220, 270,
                                    330, 390, 470, 560, 680, 820,
                                ],
            NumberSeries.E24 => [
                                    100, 110, 120, 130, 150, 160,
                                    180, 200, 220, 240, 270, 300,
                                    330, 360, 390, 430, 470, 510,
                                    560, 620, 680, 750, 820, 910,
                                ],
            NumberSeries.E48 => [
                                    100, 105, 110, 115, 121, 127,
                                    133, 140, 147, 154, 162, 169,
                                    178, 187, 196, 205, 215, 226,
                                    237, 249, 261, 274, 287, 301,
                                    316, 332, 348, 365, 383, 402,
                                    422, 442, 464, 487, 511, 536,
                                    562, 590, 619, 649, 681, 715,
                                    750, 787, 825, 866, 909, 953,
                                ],
            NumberSeries.E96 => [
                                    100, 102, 105, 107, 110, 113,
                                    115, 118, 121, 124, 127, 130,
                                    133, 137, 140, 143, 147, 150,
                                    154, 158, 162, 165, 169, 174,
                                    178, 182, 187, 191, 196, 200,
                                    205, 210, 215, 221, 226, 232,
                                    237, 243, 249, 255, 261, 267,
                                    274, 280, 287, 294, 301, 309,
                                    316, 324, 332, 340, 348, 357,
                                    365, 374, 383, 392, 402, 412,
                                    422, 432, 442, 453, 464, 475,
                                    487, 499, 511, 523, 536, 549,
                                    562, 576, 590, 604, 619, 634,
                                    649, 665, 681, 698, 715, 732,
                                    750, 768, 787, 806, 825, 845,
                                    866, 887, 909, 931, 953, 976,
                                ],
            NumberSeries.E192 => [
                                    100, 101, 102, 104, 105, 106,
                                    107, 109, 110, 111, 113, 114,
                                    115, 117, 118, 120, 121, 123,
                                    124, 126, 127, 129, 130, 132,
                                    133, 135, 137, 138, 140, 142,
                                    143, 145, 147, 149, 150, 152,
                                    154, 156, 158, 160, 162, 164,
                                    165, 167, 169, 172, 174, 176,
                                    178, 180, 182, 184, 187, 189,
                                    191, 193, 196, 198, 200, 203,
                                    205, 208, 210, 213, 215, 218,
                                    221, 223, 226, 229, 232, 234,
                                    237, 240, 243, 246, 249, 252,
                                    255, 258, 261, 264, 267, 271,
                                    274, 277, 280, 284, 287, 291,
                                    294, 298, 301, 305, 309, 312,
                                    316, 320, 324, 328, 332, 336,
                                    340, 344, 348, 352, 357, 361,
                                    365, 370, 374, 379, 383, 388,
                                    392, 397, 402, 407, 412, 417,
                                    422, 427, 432, 437, 442, 448,
                                    453, 459, 464, 470, 475, 481,
                                    487, 493, 499, 505, 511, 517,
                                    523, 530, 536, 542, 549, 556,
                                    562, 569, 576, 583, 590, 597,
                                    604, 612, 619, 626, 634, 642,
                                    649, 657, 665, 673, 681, 690,
                                    698, 706, 715, 723, 732, 741,
                                    750, 759, 768, 777, 787, 796,
                                    806, 816, 825, 835, 845, 856,
                                    866, 876, 887, 898, 909, 919,
                                    931, 942, 953, 965, 976, 988,
                                ],
            _ => throw new ArgumentOutOfRangeException(nameof(series), "Unrecognized series."),
        };

        var valueCoefficient = value;
        int valueExponent = 0;
        if (value < 100) {
            while (valueCoefficient < 100) {
                valueCoefficient *= 10;
                valueExponent -= 1;
            }
        } else {
            while (valueCoefficient >= 1000) {
                valueCoefficient /= 10;
                valueExponent += 1;
            }
        }

        int valueIndex = -1;
        for (int i = 0; i < numbers.Length; ++i) {
            if (valueCoefficient <= numbers[i]) {
                valueIndex = i;
                break;
            }
        }
        if (valueIndex == -1) { //it is higher than highest coefficient
            valueExponent += 1;
            valueIndex = 0;
        }

        var valueHigh = numbers[valueIndex] * (decimal)Math.Pow(10, valueExponent);

        decimal valueLow;
        if (value != valueHigh) {
            if (valueIndex == 0) {
                valueLow = numbers[^1] * (decimal)Math.Pow(10, valueExponent - 1);
            } else {
                valueLow = numbers[valueIndex - 1] * (decimal)Math.Pow(10, valueExponent);
            }
        } else {
            valueLow = valueHigh;
        }

        return rounding switch {
            NumberSeriesRounding.Nearest => (Math.Abs(value - valueLow) < Math.Abs(value - valueHigh)) ? valueLow : valueHigh,
            NumberSeriesRounding.Up => valueHigh,
            NumberSeriesRounding.Down => (decimal?)valueLow,
            _ => throw new ArgumentOutOfRangeException(nameof(rounding), "Unrecognized rounding."),
        };
    }

    #endregion
}
