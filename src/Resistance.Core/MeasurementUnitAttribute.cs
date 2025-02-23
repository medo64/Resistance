namespace ResiCalc;

using System;

/// <summary>
/// Unit of measurement
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public sealed class MeasurementUnitAttribute : Attribute {

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public MeasurementUnitAttribute()
        : this(string.Empty) {
    }

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <param name="unit">Unit.</param>
    public MeasurementUnitAttribute(string unit) {
        Unit = unit;
    }

    /// <summary>
    /// Gets unit.
    /// </summary>
    public string Unit { get; }

}
