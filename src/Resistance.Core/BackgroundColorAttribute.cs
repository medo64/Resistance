namespace ResiCalc;

using System;
using System.Globalization;

/// <summary>
/// Unit of measurement
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class BackgroundColorAttribute : Attribute {

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public BackgroundColorAttribute()
        : this(string.Empty) {
    }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <param name="hex">Hex color.</param>
    public BackgroundColorAttribute(string hex) {
        if (hex is not null) {
            Hex = hex;
            if (hex.Length == 6) {
                if (byte.TryParse(hex.AsSpan(0, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var red)
                    && byte.TryParse(hex.AsSpan(2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var green)
                    && byte.TryParse(hex.AsSpan(4, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var blue)) {
                    Red = red;
                    Green = green;
                    Blue = blue;
                }
            } else if (hex.Length == 3) {
                if (byte.TryParse(new string(hex[0], 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var red)
                    && byte.TryParse(new string(hex[1], 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var green)
                    && byte.TryParse(new string(hex[2], 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var blue)) {
                    Red = red;
                    Green = green;
                    Blue = blue;
                }
            }
        } else {
            Hex = "";
        }
    }


    /// <summary>
    /// Gets hex value.
    /// </summary>
    public string Hex { get; }

    /// <summary>
    /// Gets red component.
    /// </summary>
    public byte Red { get; }

    /// <summary>
    /// Gets green component.
    /// </summary>
    public byte Green { get; }

    /// <summary>
    /// Gets blue component.
    /// </summary>
    public byte Blue { get; }

}
