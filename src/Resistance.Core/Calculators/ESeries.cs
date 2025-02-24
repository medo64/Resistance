namespace ResiCalc;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

/// <summary>
/// Ohm law calculator.
/// </summary>
public class ESeries : Calculator {

    public ESeries()
        : base("E-series", "Round to nearest E-series value") {

        Value = StoreRead(nameof(Value), 1000);
    }


    private Measurement _Value;
    [Category("")]
    [DisplayName("Value")]
    [MeasurementUnit("")]
    public Measurement Value {
        get { return _Value; }
        set {
            _Value = value;
            _E6 = new Measurement(Value, NumberSeries.E6, NumberSeriesRounding.Nearest, -2);
            _E12 = new Measurement(Value, NumberSeries.E12, NumberSeriesRounding.Nearest, -2);
            _E24 = new Measurement(Value, NumberSeries.E24, NumberSeriesRounding.Nearest, -2);
            _E48 = new Measurement(Value, NumberSeries.E48, NumberSeriesRounding.Nearest, -3);
            _E96 = new Measurement(Value, NumberSeries.E96, NumberSeriesRounding.Nearest, -3);
            _E192 = new Measurement(Value, NumberSeries.E192, NumberSeriesRounding.Nearest, -3);
            base.StoreWrite(nameof(Value));
        }
    }

    private Measurement _E6;
    [Category("Rounded")]
    [DisplayName("E6 (±20%)")]
    [MeasurementUnit("")]
    public Measurement E6 {
        get { return _E6; }
    }

    private Measurement _E12;
    [Category("Rounded")]
    [DisplayName("E12 (±10%)")]
    [MeasurementUnit("")]
    public Measurement E12 {
        get { return _E12; }
    }

    private Measurement _E24;
    [Category("Rounded")]
    [DisplayName("E24 (±5%)")]
    [MeasurementUnit("")]
    public Measurement E24 {
        get { return _E24; }
    }

    private Measurement _E48;
    [Category("Rounded")]
    [DisplayName("E48 (±2%)")]
    [MeasurementUnit("")]
    public Measurement E48 {
        get { return _E48; }
    }

    private Measurement _E96;
    [Category("Rounded")]
    [DisplayName("E96 (±1%)")]
    [MeasurementUnit("")]
    public Measurement E96 {
        get { return _E96; }
    }

    private Measurement _E192;
    [Category("Rounded")]
    [DisplayName("E192 (±0.5%)")]
    [MeasurementUnit("")]
    public Measurement E192 {
        get { return _E192; }
    }


    #region Elements

    /// <inheritdoc/>
    public override ReadOnlyCollection<string> GetMeasurementNames() {
        return new ReadOnlyCollection<string>([
            nameof(Value),
            nameof(E6),
            nameof(E12),
            nameof(E24),
            nameof(E48),
            nameof(E96),
            nameof(E192),
        ]);
    }

    #endregion Elements

}
