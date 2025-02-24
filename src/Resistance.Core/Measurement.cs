namespace ResiCalc;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

/// <summary>
/// Measurement instance.
/// </summary>
public readonly struct Measurement : IEquatable<Measurement> {

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public Measurement() { }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <param name="value">Value.</param>
    public Measurement(decimal? value) {
        Value = value;
    }

    private readonly decimal? Value;


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

    public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format) {
        return (Value is not null) ? Value.Value.ToString(format, NumberFormatInfo.CurrentInfo) : string.Empty;
    }

    public string ToString(IFormatProvider? provider) {
        return (Value is not null) ? Value.Value.ToString(provider) : string.Empty;
    }

    public string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? provider) {
        return (Value is not null) ? Value.Value.ToString(format, provider) : string.Empty;
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

}
