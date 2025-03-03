namespace ResiCalc;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

/// <summary>
/// Ohm law calculator.
/// </summary>
public class ParallelAndSeries : Calculator {

    public ParallelAndSeries()
        : base("Parallel and Series") {

        _RPT = new Measurement(null, digitCount: -3, useSI: true, minValue: null, maxValue: null);
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

        _RST = new Measurement(null, digitCount: -3, useSI: true, minValue: null, maxValue: null);
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

        _CPT = new Measurement(null, digitCount: -3, useSI: true, minValue: null, maxValue: null);
        _CP0 = new Measurement(StoreRead(nameof(CP0), 0.000010m), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _CP1 = new Measurement(StoreRead(nameof(CP1), 0.000010m), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _CP2 = new Measurement(StoreRead(nameof(CP2), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _CP3 = new Measurement(StoreRead(nameof(CP3), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _CP4 = new Measurement(StoreRead(nameof(CP4), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _CP5 = new Measurement(StoreRead(nameof(CP5), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _CP6 = new Measurement(StoreRead(nameof(CP6), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _CP7 = new Measurement(StoreRead(nameof(CP7), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _CP8 = new Measurement(StoreRead(nameof(CP8), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _CP9 = new Measurement(StoreRead(nameof(CP9), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);

        _CST = new Measurement(null, digitCount: -3, useSI: true, minValue: null, maxValue: null);
        _CS0 = new Measurement(StoreRead(nameof(CS0), 0.000010m), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _CS1 = new Measurement(StoreRead(nameof(CS1), 0.000010m), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _CS2 = new Measurement(StoreRead(nameof(CS2), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _CS3 = new Measurement(StoreRead(nameof(CS3), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _CS4 = new Measurement(StoreRead(nameof(CS4), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _CS5 = new Measurement(StoreRead(nameof(CS5), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _CS6 = new Measurement(StoreRead(nameof(CS6), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _CS7 = new Measurement(StoreRead(nameof(CS7), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _CS8 = new Measurement(StoreRead(nameof(CS8), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _CS9 = new Measurement(StoreRead(nameof(CS9), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);

        _LPT = new Measurement(null, digitCount: -3, useSI: true, minValue: null, maxValue: null);
        _LP0 = new Measurement(StoreRead(nameof(LP0), 1000), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _LP1 = new Measurement(StoreRead(nameof(LP1), 1000), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _LP2 = new Measurement(StoreRead(nameof(LP2), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _LP3 = new Measurement(StoreRead(nameof(LP3), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _LP4 = new Measurement(StoreRead(nameof(LP4), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _LP5 = new Measurement(StoreRead(nameof(LP5), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _LP6 = new Measurement(StoreRead(nameof(LP6), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _LP7 = new Measurement(StoreRead(nameof(LP7), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _LP8 = new Measurement(StoreRead(nameof(LP8), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _LP9 = new Measurement(StoreRead(nameof(LP9), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);

        _LST = new Measurement(null, digitCount: -3, useSI: true, minValue: null, maxValue: null);
        _LS0 = new Measurement(StoreRead(nameof(LS0), 1000), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _LS1 = new Measurement(StoreRead(nameof(LS1), 1000), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _LS2 = new Measurement(StoreRead(nameof(LS2), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _LS3 = new Measurement(StoreRead(nameof(LS3), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _LS4 = new Measurement(StoreRead(nameof(LS4), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _LS5 = new Measurement(StoreRead(nameof(LS5), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _LS6 = new Measurement(StoreRead(nameof(LS6), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _LS7 = new Measurement(StoreRead(nameof(LS7), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _LS8 = new Measurement(StoreRead(nameof(LS8), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _LS9 = new Measurement(StoreRead(nameof(LS9), Measurement.Null), digitCount: -3, useSI: true, minValue: 0, maxValue: null);

        Update();
    }


    private Measurement _RPT;
    [Category("Parallel Resitors")]
    [DisplayName("Resistance (total)")]
    [MeasurementUnit("Ω")]
    public Measurement RPT {
        get { return _RPT; }
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


    private Measurement _RST;
    [Category("Series Resitors")]
    [DisplayName("Resistance (total)")]
    [MeasurementUnit("Ω")]
    public Measurement RST {
        get { return _RST; }
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


    private Measurement _CPT;
    [Category("Parallel Capacitors")]
    [DisplayName("Capacitance (total)")]
    [MeasurementUnit("F")]
    public Measurement CPT {
        get { return _CPT; }
    }

    private Measurement _CP0;
    [Category("Parallel Capacitors")]
    [DisplayName("Capacitance")]
    [MeasurementUnit("F")]
    public Measurement CP0 {
        get { return _CP0; }
        set {
            _CP0.Adjust(value);
            Update();
        }
    }

    private Measurement _CP1;
    [Category("Parallel Capacitors")]
    [DisplayName("Capacitance")]
    [MeasurementUnit("F")]
    public Measurement CP1 {
        get { return _CP1; }
        set {
            _CP1.Adjust(value);
            Update();
        }
    }

    private Measurement _CP2;
    [Category("Parallel Capacitors")]
    [DisplayName("Capacitance")]
    [MeasurementUnit("F")]
    public Measurement CP2 {
        get { return _CP2; }
        set {
            _CP2.Adjust(value);
            Update();
        }
    }

    private Measurement _CP3;
    [Category("Parallel Capacitors")]
    [DisplayName("Capacitance")]
    [MeasurementUnit("F")]
    public Measurement CP3 {
        get { return _CP3; }
        set {
            _CP3.Adjust(value);
            Update();
        }
    }

    private Measurement _CP4;
    [Category("Parallel Capacitors")]
    [DisplayName("Capacitance")]
    [MeasurementUnit("F")]
    public Measurement CP4 {
        get { return _CP4; }
        set {
            _CP4.Adjust(value);
            Update();
        }
    }

    private Measurement _CP5;
    [Category("Parallel Capacitors")]
    [DisplayName("Capacitance")]
    [MeasurementUnit("F")]
    public Measurement CP5 {
        get { return _CP5; }
        set {
            _CP5.Adjust(value);
            Update();
        }
    }

    private Measurement _CP6;
    [Category("Parallel Capacitors")]
    [DisplayName("Capacitance")]
    [MeasurementUnit("F")]
    public Measurement CP6 {
        get { return _CP6; }
        set {
            _CP6.Adjust(value);
            Update();
        }
    }

    private Measurement _CP7;
    [Category("Parallel Capacitors")]
    [DisplayName("Capacitance")]
    [MeasurementUnit("F")]
    public Measurement CP7 {
        get { return _CP7; }
        set {
            _CP7.Adjust(value);
            Update();
        }
    }

    private Measurement _CP8;
    [Category("Parallel Capacitors")]
    [DisplayName("Capacitance")]
    [MeasurementUnit("F")]
    public Measurement CP8 {
        get { return _CP8; }
        set {
            _CP8.Adjust(value);
            Update();
        }
    }

    private Measurement _CP9;
    [Category("Parallel Capacitors")]
    [DisplayName("Capacitance")]
    [MeasurementUnit("F")]
    public Measurement CP9 {
        get { return _CP9; }
        set {
            _CP9.Adjust(value);
            Update();
        }
    }


    private Measurement _CST;
    [Category("Series Capacitors")]
    [DisplayName("Capacitance (total)")]
    [MeasurementUnit("F")]
    public Measurement CST {
        get { return _CST; }
    }

    private Measurement _CS0;
    [Category("Series Capacitors")]
    [DisplayName("Capacitance")]
    [MeasurementUnit("F")]
    public Measurement CS0 {
        get { return _CS0; }
        set {
            _CS0.Adjust(value);
            Update();
        }
    }

    private Measurement _CS1;
    [Category("Series Capacitors")]
    [DisplayName("Capacitance")]
    [MeasurementUnit("F")]
    public Measurement CS1 {
        get { return _CS1; }
        set {
            _CS1.Adjust(value);
            Update();
        }
    }

    private Measurement _CS2;
    [Category("Series Capacitors")]
    [DisplayName("Capacitance")]
    [MeasurementUnit("F")]
    public Measurement CS2 {
        get { return _CS2; }
        set {
            _CS2.Adjust(value);
            Update();
        }
    }

    private Measurement _CS3;
    [Category("Series Capacitors")]
    [DisplayName("Capacitance")]
    [MeasurementUnit("F")]
    public Measurement CS3 {
        get { return _CS3; }
        set {
            _CS3.Adjust(value);
            Update();
        }
    }

    private Measurement _CS4;
    [Category("Series Capacitors")]
    [DisplayName("Capacitance")]
    [MeasurementUnit("F")]
    public Measurement CS4 {
        get { return _CS4; }
        set {
            _CS4.Adjust(value);
            Update();
        }
    }

    private Measurement _CS5;
    [Category("Series Capacitors")]
    [DisplayName("Capacitance")]
    [MeasurementUnit("F")]
    public Measurement CS5 {
        get { return _CS5; }
        set {
            _CS5.Adjust(value);
            Update();
        }
    }

    private Measurement _CS6;
    [Category("Series Capacitors")]
    [DisplayName("Capacitance")]
    [MeasurementUnit("F")]
    public Measurement CS6 {
        get { return _CS6; }
        set {
            _CS6.Adjust(value);
            Update();
        }
    }

    private Measurement _CS7;
    [Category("Series Capacitors")]
    [DisplayName("Capacitance")]
    [MeasurementUnit("F")]
    public Measurement CS7 {
        get { return _CS7; }
        set {
            _CS7.Adjust(value);
            Update();
        }
    }

    private Measurement _CS8;
    [Category("Series Capacitors")]
    [DisplayName("Capacitance")]
    [MeasurementUnit("F")]
    public Measurement CS8 {
        get { return _CS8; }
        set {
            _CS8.Adjust(value);
            Update();
        }
    }

    private Measurement _CS9;
    [Category("Series Capacitors")]
    [DisplayName("Capacitance")]
    [MeasurementUnit("F")]
    public Measurement CS9 {
        get { return _CS9; }
        set {
            _CS9.Adjust(value);
            Update();
        }
    }


    private Measurement _LPT;
    [Category("Parallel Inductors")]
    [DisplayName("Inductance (total)")]
    [MeasurementUnit("H")]
    public Measurement LPT {
        get { return _LPT; }
    }

    private Measurement _LP0;
    [Category("Parallel Inductors")]
    [DisplayName("Inductance")]
    [MeasurementUnit("H")]
    public Measurement LP0 {
        get { return _LP0; }
        set {
            _LP0.Adjust(value);
            Update();
        }
    }

    private Measurement _LP1;
    [Category("Parallel Inductors")]
    [DisplayName("Inductance")]
    [MeasurementUnit("H")]
    public Measurement LP1 {
        get { return _LP1; }
        set {
            _LP1.Adjust(value);
            Update();
        }
    }

    private Measurement _LP2;
    [Category("Parallel Inductors")]
    [DisplayName("Inductance")]
    [MeasurementUnit("H")]
    public Measurement LP2 {
        get { return _LP2; }
        set {
            _LP2.Adjust(value);
            Update();
        }
    }

    private Measurement _LP3;
    [Category("Parallel Inductors")]
    [DisplayName("Inductance")]
    [MeasurementUnit("H")]
    public Measurement LP3 {
        get { return _LP3; }
        set {
            _LP3.Adjust(value);
            Update();
        }
    }

    private Measurement _LP4;
    [Category("Parallel Inductors")]
    [DisplayName("Inductance")]
    [MeasurementUnit("H")]
    public Measurement LP4 {
        get { return _LP4; }
        set {
            _LP4.Adjust(value);
            Update();
        }
    }

    private Measurement _LP5;
    [Category("Parallel Inductors")]
    [DisplayName("Inductance")]
    [MeasurementUnit("H")]
    public Measurement LP5 {
        get { return _LP5; }
        set {
            _LP5.Adjust(value);
            Update();
        }
    }

    private Measurement _LP6;
    [Category("Parallel Inductors")]
    [DisplayName("Inductance")]
    [MeasurementUnit("H")]
    public Measurement LP6 {
        get { return _LP6; }
        set {
            _LP6.Adjust(value);
            Update();
        }
    }

    private Measurement _LP7;
    [Category("Parallel Inductors")]
    [DisplayName("Inductance")]
    [MeasurementUnit("H")]
    public Measurement LP7 {
        get { return _LP7; }
        set {
            _LP7.Adjust(value);
            Update();
        }
    }

    private Measurement _LP8;
    [Category("Parallel Inductors")]
    [DisplayName("Inductance")]
    [MeasurementUnit("H")]
    public Measurement LP8 {
        get { return _LP8; }
        set {
            _LP8.Adjust(value);
            Update();
        }
    }

    private Measurement _LP9;
    [Category("Parallel Inductors")]
    [DisplayName("Inductance")]
    [MeasurementUnit("H")]
    public Measurement LP9 {
        get { return _LP9; }
        set {
            _LP9.Adjust(value);
            Update();
        }
    }


    private Measurement _LST;
    [Category("Series Inductors")]
    [DisplayName("Inductance (total)")]
    [MeasurementUnit("H")]
    public Measurement LST {
        get { return _LST; }
    }

    private Measurement _LS0;
    [Category("Series Inductors")]
    [DisplayName("Inductance")]
    [MeasurementUnit("H")]
    public Measurement LS0 {
        get { return _LS0; }
        set {
            _LS0.Adjust(value);
            Update();
        }
    }

    private Measurement _LS1;
    [Category("Series Inductors")]
    [DisplayName("Inductance")]
    [MeasurementUnit("H")]
    public Measurement LS1 {
        get { return _LS1; }
        set {
            _LS1.Adjust(value);
            Update();
        }
    }

    private Measurement _LS2;
    [Category("Series Inductors")]
    [DisplayName("Inductance")]
    [MeasurementUnit("H")]
    public Measurement LS2 {
        get { return _LS2; }
        set {
            _LS2.Adjust(value);
            Update();
        }
    }

    private Measurement _LS3;
    [Category("Series Inductors")]
    [DisplayName("Inductance")]
    [MeasurementUnit("H")]
    public Measurement LS3 {
        get { return _LS3; }
        set {
            _LS3.Adjust(value);
            Update();
        }
    }

    private Measurement _LS4;
    [Category("Series Inductors")]
    [DisplayName("Inductance")]
    [MeasurementUnit("H")]
    public Measurement LS4 {
        get { return _LS4; }
        set {
            _LS4.Adjust(value);
            Update();
        }
    }

    private Measurement _LS5;
    [Category("Series Inductors")]
    [DisplayName("Inductance")]
    [MeasurementUnit("H")]
    public Measurement LS5 {
        get { return _LS5; }
        set {
            _LS5.Adjust(value);
            Update();
        }
    }

    private Measurement _LS6;
    [Category("Series Inductors")]
    [DisplayName("Inductance")]
    [MeasurementUnit("H")]
    public Measurement LS6 {
        get { return _LS6; }
        set {
            _LS6.Adjust(value);
            Update();
        }
    }

    private Measurement _LS7;
    [Category("Series Inductors")]
    [DisplayName("Inductance")]
    [MeasurementUnit("H")]
    public Measurement LS7 {
        get { return _LS7; }
        set {
            _LS7.Adjust(value);
            Update();
        }
    }

    private Measurement _LS8;
    [Category("Series Inductors")]
    [DisplayName("Inductance")]
    [MeasurementUnit("H")]
    public Measurement LS8 {
        get { return _LS8; }
        set {
            _LS8.Adjust(value);
            Update();
        }
    }

    private Measurement _LS9;
    [Category("Series Inductors")]
    [DisplayName("Inductance")]
    [MeasurementUnit("H")]
    public Measurement LS9 {
        get { return _LS9; }
        set {
            _LS9.Adjust(value);
            Update();
        }
    }


    private void Update() {
        var parResistors = new List<decimal>();
        if (_RP0 > 0) { parResistors.Add(_RP0); }
        if (_RP1 > 0) { parResistors.Add(_RP1); }
        if (_RP2 > 0) { parResistors.Add(_RP2); }
        if (_RP3 > 0) { parResistors.Add(_RP3); }
        if (_RP4 > 0) { parResistors.Add(_RP4); }
        if (_RP5 > 0) { parResistors.Add(_RP5); }
        if (_RP6 > 0) { parResistors.Add(_RP6); }
        if (_RP7 > 0) { parResistors.Add(_RP7); }
        if (_RP8 > 0) { parResistors.Add(_RP8); }
        if (_RP9 > 0) { parResistors.Add(_RP9); }
        if (parResistors.Count > 0) {
            var totalMul = 1m;
            var totalSum = 0m;
            foreach (var value in parResistors) {
                totalMul *= value;
                totalSum += value;
            }
            _RPT.Adjust(totalMul / totalSum);
        } else {
            _RPT = Measurement.Null;
        }

        var serResistors = new List<decimal>();
        if (_RS0 > 0) { serResistors.Add(_RS0); }
        if (_RS1 > 0) { serResistors.Add(_RS1); }
        if (_RS2 > 0) { serResistors.Add(_RS2); }
        if (_RS3 > 0) { serResistors.Add(_RS3); }
        if (_RS4 > 0) { serResistors.Add(_RS4); }
        if (_RS5 > 0) { serResistors.Add(_RS5); }
        if (_RS6 > 0) { serResistors.Add(_RS6); }
        if (_RS7 > 0) { serResistors.Add(_RS7); }
        if (_RS8 > 0) { serResistors.Add(_RS8); }
        if (_RS9 > 0) { serResistors.Add(_RS9); }
        if (serResistors.Count > 0) {
            var total = 0m;
            foreach (var value in serResistors) {
                total += value;
            }
            _RST.Adjust(total);
        } else {
            _RST = Measurement.Null;
        }

        var parCapacitors = new List<decimal>();
        if (_CP0 > 0) { parCapacitors.Add(_CP0); }
        if (_CP1 > 0) { parCapacitors.Add(_CP1); }
        if (_CP2 > 0) { parCapacitors.Add(_CP2); }
        if (_CP3 > 0) { parCapacitors.Add(_CP3); }
        if (_CP4 > 0) { parCapacitors.Add(_CP4); }
        if (_CP5 > 0) { parCapacitors.Add(_CP5); }
        if (_CP6 > 0) { parCapacitors.Add(_CP6); }
        if (_CP7 > 0) { parCapacitors.Add(_CP7); }
        if (_CP8 > 0) { parCapacitors.Add(_CP8); }
        if (_CP9 > 0) { parCapacitors.Add(_CP9); }
        if (parCapacitors.Count > 0) {
            var total = 0m;
            foreach (var value in parCapacitors) {
                total += value;
            }
            _CPT.Adjust(total);
        } else {
            _CPT = Measurement.Null;
        }

        var serCapacitors = new List<decimal>();
        if (_CS0 > 0) { serCapacitors.Add(_CS0); }
        if (_CS1 > 0) { serCapacitors.Add(_CS1); }
        if (_CS2 > 0) { serCapacitors.Add(_CS2); }
        if (_CS3 > 0) { serCapacitors.Add(_CS3); }
        if (_CS4 > 0) { serCapacitors.Add(_CS4); }
        if (_CS5 > 0) { serCapacitors.Add(_CS5); }
        if (_CS6 > 0) { serCapacitors.Add(_CS6); }
        if (_CS7 > 0) { serCapacitors.Add(_CS7); }
        if (_CS8 > 0) { serCapacitors.Add(_CS8); }
        if (_CS9 > 0) { serCapacitors.Add(_CS9); }
        if (serCapacitors.Count > 0) {
            var totalMul = 1m;
            var totalSum = 0m;
            foreach (var value in serCapacitors) {
                totalMul *= value;
                totalSum += value;
            }
            _CST.Adjust(totalMul / totalSum);
        } else {
            _CST = Measurement.Null;
        }

        var parInductors = new List<decimal>();
        if (_LP0 > 0) { parInductors.Add(_LP0); }
        if (_LP1 > 0) { parInductors.Add(_LP1); }
        if (_LP2 > 0) { parInductors.Add(_LP2); }
        if (_LP3 > 0) { parInductors.Add(_LP3); }
        if (_LP4 > 0) { parInductors.Add(_LP4); }
        if (_LP5 > 0) { parInductors.Add(_LP5); }
        if (_LP6 > 0) { parInductors.Add(_LP6); }
        if (_LP7 > 0) { parInductors.Add(_LP7); }
        if (_LP8 > 0) { parInductors.Add(_LP8); }
        if (_LP9 > 0) { parInductors.Add(_LP9); }
        if (parInductors.Count > 0) {
            var totalMul = 1m;
            var totalSum = 0m;
            foreach (var value in parInductors) {
                totalMul *= value;
                totalSum += value;
            }
            _LPT.Adjust(totalMul / totalSum);
        } else {
            _LPT = Measurement.Null;
        }

        var serInductors = new List<decimal>();
        if (_LS0 > 0) { serInductors.Add(_LS0); }
        if (_LS1 > 0) { serInductors.Add(_LS1); }
        if (_LS2 > 0) { serInductors.Add(_LS2); }
        if (_LS3 > 0) { serInductors.Add(_LS3); }
        if (_LS4 > 0) { serInductors.Add(_LS4); }
        if (_LS5 > 0) { serInductors.Add(_LS5); }
        if (_LS6 > 0) { serInductors.Add(_LS6); }
        if (_LS7 > 0) { serInductors.Add(_LS7); }
        if (_LS8 > 0) { serInductors.Add(_LS8); }
        if (_LS9 > 0) { serInductors.Add(_LS9); }
        if (serInductors.Count > 0) {
            var total = 0m;
            foreach (var value in serInductors) {
                total += value;
            }
            _LST.Adjust(total);
        } else {
            _LST = Measurement.Null;
        }
    }


    #region Elements

    /// <inheritdoc/>
    public override ReadOnlyCollection<string> GetElementNames() {
        return new ReadOnlyCollection<string>([
            nameof(RPT), nameof(RP0), nameof(RP1), nameof(RP2), nameof(RP3), nameof(RP4), nameof(RP5), nameof(RP6), nameof(RP7), nameof(RP8), nameof(RP9),
            nameof(RST), nameof(RS0), nameof(RS1), nameof(RS2), nameof(RS3), nameof(RS4), nameof(RS5), nameof(RS6), nameof(RS7), nameof(RS8), nameof(RS9),
            nameof(CPT), nameof(CP0), nameof(CP1), nameof(CP2), nameof(CP3), nameof(CP4), nameof(CP5), nameof(CP6), nameof(CP7), nameof(CP8), nameof(CP9),
            nameof(CST), nameof(CS0), nameof(CS1), nameof(CS2), nameof(CS3), nameof(CS4), nameof(CS5), nameof(CS6), nameof(CS7), nameof(CS8), nameof(CS9),
            nameof(LPT), nameof(LP0), nameof(LP1), nameof(LP2), nameof(LP3), nameof(LP4), nameof(LP5), nameof(LP6), nameof(LP7), nameof(LP8), nameof(LP9),
            nameof(LST), nameof(LS0), nameof(LS1), nameof(LS2), nameof(LS3), nameof(LS4), nameof(LS5), nameof(LS6), nameof(LS7), nameof(LS8), nameof(LS9),
        ]);
    }

    #endregion Elements

}
