namespace ResiCalc;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

/// <summary>
/// Ohm law calculator.
/// </summary>
public class ParallelAndSeriesResistors : Calculator {

    public ParallelAndSeriesResistors()
        : base("Parallel and Series Resistors") {

        _RP0 = new Measurement(StoreRead(nameof(RP0), 1000), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _RP1 = new Measurement(StoreRead(nameof(RP1), 1000), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _RP2 = new Measurement(StoreRead(nameof(RP2), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _RP3 = new Measurement(StoreRead(nameof(RP3), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _RP4 = new Measurement(StoreRead(nameof(RP4), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _RP5 = new Measurement(StoreRead(nameof(RP5), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _RP6 = new Measurement(StoreRead(nameof(RP6), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _RP7 = new Measurement(StoreRead(nameof(RP7), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _RP8 = new Measurement(StoreRead(nameof(RP8), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _RP9 = new Measurement(StoreRead(nameof(RP9), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _RPT = new Measurement(null, digitCount: -3, useSI: true, minValue: null, maxValue: null);

        _RS0 = new Measurement(StoreRead(nameof(RS0), 1000), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _RS1 = new Measurement(StoreRead(nameof(RS1), 1000), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _RS2 = new Measurement(StoreRead(nameof(RS2), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _RS3 = new Measurement(StoreRead(nameof(RS3), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _RS4 = new Measurement(StoreRead(nameof(RS4), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _RS5 = new Measurement(StoreRead(nameof(RS5), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _RS6 = new Measurement(StoreRead(nameof(RS6), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _RS7 = new Measurement(StoreRead(nameof(RS7), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _RS8 = new Measurement(StoreRead(nameof(RS8), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _RS9 = new Measurement(StoreRead(nameof(RS9), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _RST = new Measurement(null, digitCount: -3, useSI: true, minValue: null, maxValue: null);

        Update();
    }


    private Measurement _RP0;
    [Category("Parallel Resitors")]
    [DisplayName("Resistance")]
    [MeasurementUnit("Ω")]
    public Measurement RP0 {
        get { return _RP0; }
        set {
            _RP0.Adjust(value);
            Update();
        }
    }

    private Measurement _RP1;
    [Category("Parallel Resitors")]
    [DisplayName("Resistance")]
    [MeasurementUnit("Ω")]
    public Measurement RP1 {
        get { return _RP1; }
        set {
            _RP1.Adjust(value);
            Update();
        }
    }

    private Measurement _RP2;
    [Category("Parallel Resitors")]
    [DisplayName("Resistance")]
    [MeasurementUnit("Ω")]
    public Measurement RP2 {
        get { return _RP2; }
        set {
            _RP2.Adjust(value);
            Update();
        }
    }

    private Measurement _RP3;
    [Category("Parallel Resitors")]
    [DisplayName("Resistance")]
    [MeasurementUnit("Ω")]
    public Measurement RP3 {
        get { return _RP3; }
        set {
            _RP3.Adjust(value);
            Update();
        }
    }

    private Measurement _RP4;
    [Category("Parallel Resitors")]
    [DisplayName("Resistance")]
    [MeasurementUnit("Ω")]
    public Measurement RP4 {
        get { return _RP4; }
        set {
            _RP4.Adjust(value);
            Update();
        }
    }

    private Measurement _RP5;
    [Category("Parallel Resitors")]
    [DisplayName("Resistance")]
    [MeasurementUnit("Ω")]
    public Measurement RP5 {
        get { return _RP5; }
        set {
            _RP5.Adjust(value);
            Update();
        }
    }

    private Measurement _RP6;
    [Category("Parallel Resitors")]
    [DisplayName("Resistance")]
    [MeasurementUnit("Ω")]
    public Measurement RP6 {
        get { return _RP6; }
        set {
            _RP6.Adjust(value);
            Update();
        }
    }

    private Measurement _RP7;
    [Category("Parallel Resitors")]
    [DisplayName("Resistance")]
    [MeasurementUnit("Ω")]
    public Measurement RP7 {
        get { return _RP7; }
        set {
            _RP7.Adjust(value);
            Update();
        }
    }

    private Measurement _RP8;
    [Category("Parallel Resitors")]
    [DisplayName("Resistance")]
    [MeasurementUnit("Ω")]
    public Measurement RP8 {
        get { return _RP8; }
        set {
            _RP8.Adjust(value);
            Update();
        }
    }

    private Measurement _RP9;
    [Category("Parallel Resitors")]
    [DisplayName("Resistance")]
    [MeasurementUnit("Ω")]
    public Measurement RP9 {
        get { return _RP9; }
        set {
            _RP9.Adjust(value);
            Update();
        }
    }

    private Measurement _RPT;
    [Category("Parallel Resitors")]
    [DisplayName("Resistance (total)")]
    [MeasurementUnit("Ω")]
    public Measurement RPT {
        get { return _RPT; }
    }


    private Measurement _RS0;
    [Category("Series Resitors")]
    [DisplayName("Resistance")]
    [MeasurementUnit("Ω")]
    public Measurement RS0 {
        get { return _RS0; }
        set {
            _RS0.Adjust(value);
            Update();
        }
    }

    private Measurement _RS1;
    [Category("Series Resitors")]
    [DisplayName("Resistance")]
    [MeasurementUnit("Ω")]
    public Measurement RS1 {
        get { return _RS1; }
        set {
            _RS1.Adjust(value);
            Update();
        }
    }

    private Measurement _RS2;
    [Category("Series Resitors")]
    [DisplayName("Resistance")]
    [MeasurementUnit("Ω")]
    public Measurement RS2 {
        get { return _RS2; }
        set {
            _RS2.Adjust(value);
            Update();
        }
    }

    private Measurement _RS3;
    [Category("Series Resitors")]
    [DisplayName("Resistance")]
    [MeasurementUnit("Ω")]
    public Measurement RS3 {
        get { return _RS3; }
        set {
            _RS3.Adjust(value);
            Update();
        }
    }

    private Measurement _RS4;
    [Category("Series Resitors")]
    [DisplayName("Resistance")]
    [MeasurementUnit("Ω")]
    public Measurement RS4 {
        get { return _RS4; }
        set {
            _RS4.Adjust(value);
            Update();
        }
    }

    private Measurement _RS5;
    [Category("Series Resitors")]
    [DisplayName("Resistance")]
    [MeasurementUnit("Ω")]
    public Measurement RS5 {
        get { return _RS5; }
        set {
            _RS5.Adjust(value);
            Update();
        }
    }

    private Measurement _RS6;
    [Category("Series Resitors")]
    [DisplayName("Resistance")]
    [MeasurementUnit("Ω")]
    public Measurement RS6 {
        get { return _RS6; }
        set {
            _RS6.Adjust(value);
            Update();
        }
    }

    private Measurement _RS7;
    [Category("Series Resitors")]
    [DisplayName("Resistance")]
    [MeasurementUnit("Ω")]
    public Measurement RS7 {
        get { return _RS7; }
        set {
            _RS7.Adjust(value);
            Update();
        }
    }

    private Measurement _RS8;
    [Category("Series Resitors")]
    [DisplayName("Resistance")]
    [MeasurementUnit("Ω")]
    public Measurement RS8 {
        get { return _RS8; }
        set {
            _RS8.Adjust(value);
            Update();
        }
    }

    private Measurement _RS9;
    [Category("Series Resitors")]
    [DisplayName("Resistance")]
    [MeasurementUnit("Ω")]
    public Measurement RS9 {
        get { return _RS9; }
        set {
            _RS9.Adjust(value);
            Update();
        }
    }

    private Measurement _RST;
    [Category("Series Resitors")]
    [DisplayName("Resistance (total)")]
    [MeasurementUnit("Ω")]
    public Measurement RST {
        get { return _RST; }
    }


    private void Update() {
        var parallel = new List<decimal>();
        if (_RP0 > 0) { parallel.Add(_RP0); }
        if (_RP1 > 0) { parallel.Add(_RP1); }
        if (_RP2 > 0) { parallel.Add(_RP2); }
        if (_RP3 > 0) { parallel.Add(_RP3); }
        if (_RP4 > 0) { parallel.Add(_RP4); }
        if (_RP5 > 0) { parallel.Add(_RP5); }
        if (_RP6 > 0) { parallel.Add(_RP6); }
        if (_RP7 > 0) { parallel.Add(_RP7); }
        if (_RP8 > 0) { parallel.Add(_RP8); }
        if (_RP9 > 0) { parallel.Add(_RP9); }
        if (parallel.Count > 0) {
            var totalMul = 1m;
            var totalSum = 0m;
            foreach (var value in parallel) {
                totalMul *= value;
                totalSum += value;
            }
            _RPT.Adjust(totalMul / totalSum);
        } else {
            _RPT = Measurement.Null;
        }

        var serial = new List<decimal>();
        if (_RS0 > 0) { serial.Add(_RS0); }
        if (_RS1 > 0) { serial.Add(_RS1); }
        if (_RS2 > 0) { serial.Add(_RS2); }
        if (_RS3 > 0) { serial.Add(_RS3); }
        if (_RS4 > 0) { serial.Add(_RS4); }
        if (_RS5 > 0) { serial.Add(_RS5); }
        if (_RS6 > 0) { serial.Add(_RS6); }
        if (_RS7 > 0) { serial.Add(_RS7); }
        if (_RS8 > 0) { serial.Add(_RS8); }
        if (_RS9 > 0) { serial.Add(_RS9); }
        if (serial.Count > 0) {
            var total = 0m;
            foreach (var value in serial) {
                total += value;
            }
            _RST.Adjust(total);
        } else {
            _RST = Measurement.Null;
        }
    }


    #region Elements

    /// <inheritdoc/>
    public override ReadOnlyCollection<string> GetElementNames() {
        return new ReadOnlyCollection<string>([
            nameof(RP0), nameof(RP1), nameof(RP2), nameof(RP3), nameof(RP4), nameof(RP5), nameof(RP6), nameof(RP7), nameof(RP8), nameof(RP9), nameof(RPT),
            nameof(RS0), nameof(RS1), nameof(RS2), nameof(RS3), nameof(RS4), nameof(RS5), nameof(RS6), nameof(RS7), nameof(RS8), nameof(RS9), nameof(RST),
        ]);
    }

    #endregion Elements

}
