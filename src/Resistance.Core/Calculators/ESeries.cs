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
            _E3d = new Measurement(Value, NumberSeries.E3, NumberSeriesRounding.Down, -2);
            _E6d = new Measurement(Value, NumberSeries.E6, NumberSeriesRounding.Down, -2);
            _E12d = new Measurement(Value, NumberSeries.E12, NumberSeriesRounding.Down, -2);
            _E24d = new Measurement(Value, NumberSeries.E24, NumberSeriesRounding.Down, -2);
            _E48d = new Measurement(Value, NumberSeries.E48, NumberSeriesRounding.Down, -3);
            _E96d = new Measurement(Value, NumberSeries.E96, NumberSeriesRounding.Down, -3);
            _E192d = new Measurement(Value, NumberSeries.E192, NumberSeriesRounding.Down, -3);
            _E3n = new Measurement(Value, NumberSeries.E3, NumberSeriesRounding.Nearest, -2);
            _E6n = new Measurement(Value, NumberSeries.E6, NumberSeriesRounding.Nearest, -2);
            _E12n = new Measurement(Value, NumberSeries.E12, NumberSeriesRounding.Nearest, -2);
            _E24n = new Measurement(Value, NumberSeries.E24, NumberSeriesRounding.Nearest, -2);
            _E48n = new Measurement(Value, NumberSeries.E48, NumberSeriesRounding.Nearest, -3);
            _E96n = new Measurement(Value, NumberSeries.E96, NumberSeriesRounding.Nearest, -3);
            _E192n = new Measurement(Value, NumberSeries.E192, NumberSeriesRounding.Nearest, -3);
            _E3u = new Measurement(Value, NumberSeries.E3, NumberSeriesRounding.Up, -2);
            _E6u = new Measurement(Value, NumberSeries.E6, NumberSeriesRounding.Up, -2);
            _E12u = new Measurement(Value, NumberSeries.E12, NumberSeriesRounding.Up, -2);
            _E24u = new Measurement(Value, NumberSeries.E24, NumberSeriesRounding.Up, -2);
            _E48u = new Measurement(Value, NumberSeries.E48, NumberSeriesRounding.Up, -3);
            _E96u = new Measurement(Value, NumberSeries.E96, NumberSeriesRounding.Up, -3);
            _E192u = new Measurement(Value, NumberSeries.E192, NumberSeriesRounding.Up, -3);
            base.StoreWrite(nameof(Value));
        }
    }


    private Measurement _E3d;
    [Category("Rounded down")]
    [DisplayName("E3 (±40%)")]
    [MeasurementUnit("")]
    public Measurement E3d {
        get { return _E3d; }
    }

    private Measurement _E6d;
    [Category("Rounded down")]
    [DisplayName("E6 (±20%)")]
    [MeasurementUnit("")]
    public Measurement E6d {
        get { return _E6d; }
    }

    private Measurement _E12d;
    [Category("Rounded down")]
    [DisplayName("E12 (±10%)")]
    [MeasurementUnit("")]
    public Measurement E12d {
        get { return _E12d; }
    }

    private Measurement _E24d;
    [Category("Rounded down")]
    [DisplayName("E24 (±5%)")]
    [MeasurementUnit("")]
    public Measurement E24d {
        get { return _E24d; }
    }

    private Measurement _E48d;
    [Category("Rounded down")]
    [DisplayName("E48 (±2%)")]
    [MeasurementUnit("")]
    public Measurement E48d {
        get { return _E48d; }
    }

    private Measurement _E96d;
    [Category("Rounded down")]
    [DisplayName("E96 (±1%)")]
    [MeasurementUnit("")]
    public Measurement E96d {
        get { return _E96d; }
    }

    private Measurement _E192d;
    [Category("Rounded down")]
    [DisplayName("E192 (±0.5%)")]
    [MeasurementUnit("")]
    public Measurement E192d {
        get { return _E192d; }
    }


    private Measurement _E3n;
    [Category("Rounded to nearest")]
    [DisplayName("E3 (±40%)")]
    [MeasurementUnit("")]
    public Measurement E3n {
        get { return _E3n; }
    }

    private Measurement _E6n;
    [Category("Rounded to nearest")]
    [DisplayName("E6 (±20%)")]
    [MeasurementUnit("")]
    public Measurement E6n {
        get { return _E6n; }
    }

    private Measurement _E12n;
    [Category("Rounded to nearest")]
    [DisplayName("E12 (±10%)")]
    [MeasurementUnit("")]
    public Measurement E12n {
        get { return _E12n; }
    }

    private Measurement _E24n;
    [Category("Rounded to nearest")]
    [DisplayName("E24 (±5%)")]
    [MeasurementUnit("")]
    public Measurement E24n {
        get { return _E24n; }
    }

    private Measurement _E48n;
    [Category("Rounded to nearest")]
    [DisplayName("E48 (±2%)")]
    [MeasurementUnit("")]
    public Measurement E48n {
        get { return _E48n; }
    }

    private Measurement _E96n;
    [Category("Rounded to nearest")]
    [DisplayName("E96 (±1%)")]
    [MeasurementUnit("")]
    public Measurement E96n {
        get { return _E96n; }
    }

    private Measurement _E192n;
    [Category("Rounded to nearest")]
    [DisplayName("E192 (±0.5%)")]
    [MeasurementUnit("")]
    public Measurement E192n {
        get { return _E192n; }
    }


    private Measurement _E3u;
    [Category("Rounded up")]
    [DisplayName("E3 (±40%)")]
    [MeasurementUnit("")]
    public Measurement E3u {
        get { return _E3u; }
    }

    private Measurement _E6u;
    [Category("Rounded up")]
    [DisplayName("E6 (±20%)")]
    [MeasurementUnit("")]
    public Measurement E6u {
        get { return _E6u; }
    }

    private Measurement _E12u;
    [Category("Rounded up")]
    [DisplayName("E12 (±10%)")]
    [MeasurementUnit("")]
    public Measurement E12u {
        get { return _E12u; }
    }

    private Measurement _E24u;
    [Category("Rounded up")]
    [DisplayName("E24 (±5%)")]
    [MeasurementUnit("")]
    public Measurement E24u {
        get { return _E24u; }
    }

    private Measurement _E48u;
    [Category("Rounded up")]
    [DisplayName("E48 (±2%)")]
    [MeasurementUnit("")]
    public Measurement E48u {
        get { return _E48u; }
    }

    private Measurement _E96u;
    [Category("Rounded up")]
    [DisplayName("E96 (±1%)")]
    [MeasurementUnit("")]
    public Measurement E96u {
        get { return _E96u; }
    }

    private Measurement _E192u;
    [Category("Rounded up")]
    [DisplayName("E192 (±0.5%)")]
    [MeasurementUnit("")]
    public Measurement E192u {
        get { return _E192u; }
    }


    #region Elements

    /// <inheritdoc/>
    public override ReadOnlyCollection<string> GetMeasurementNames() {
        return new ReadOnlyCollection<string>([
            nameof(Value),
            nameof(E3d),
            nameof(E6d),
            nameof(E12d),
            nameof(E24d),
            nameof(E48d),
            nameof(E96d),
            nameof(E192d),
            nameof(E3n),
            nameof(E6n),
            nameof(E12n),
            nameof(E24n),
            nameof(E48n),
            nameof(E96n),
            nameof(E192n),
            nameof(E3u),
            nameof(E6u),
            nameof(E12u),
            nameof(E24u),
            nameof(E48u),
            nameof(E96u),
            nameof(E192u),
        ]);
    }

    #endregion Elements

}
