namespace ResiCalc;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

/// <summary>
/// Ohm law calculator.
/// </summary>
public class OhmLaw : Calculator {

    public OhmLaw()
        : base("Ohm's Law", "Calculate voltage, current, or resistance using Ohm's Law.") {

        _Voltage = StoreRead(nameof(Voltage), 5);
        _Current = StoreRead(nameof(Current), 1);
        _Resistance = _Voltage / _Current;
        _Power = _Voltage * _Current;
    }


    private Measurement _Voltage;
    [Category("")]
    [DisplayName("Voltage")]
    [MeasurementUnit("V")]
    public Measurement Voltage {
        get { return _Voltage; }
        set {
            _Voltage = value;
            if (base.IsSecondMoreRecentlyChanged(nameof(Current), nameof(Resistance))) {
                _Resistance = Voltage / Current;
            } else {
                _Current = Voltage / Resistance;
            }
            _Power = Voltage * Current;
            base.StoreWrite(nameof(Voltage));
        }
    }

    private Measurement _Current;
    [Category("")]
    [DisplayName("Current")]
    [MeasurementUnit("A")]
    public Measurement Current {
        get { return _Current; }
        set {
            _Current = value;
            if (base.IsSecondMoreRecentlyChanged(nameof(Voltage), nameof(Resistance))) {
                _Voltage = Current * Resistance;
            } else {
                _Resistance = Voltage / Current;
            }
            _Power = Voltage * Current;
            base.StoreWrite(nameof(Current));
        }
    }

    private Measurement _Resistance;
    [Category("")]
    [DisplayName("Resistance")]
    [MeasurementUnit("Ω")]
    public Measurement Resistance {
        get { return _Resistance; }
        set {
            _Resistance = value;
            if (base.IsSecondMoreRecentlyChanged(nameof(Voltage), nameof(Current))) {
                _Voltage = Current * Resistance;
            } else {
                _Current = Voltage / Resistance;
            }
            _Power = Voltage * Current;
            base.StoreWrite(nameof(Resistance));
        }
    }

    private Measurement _Power;
    [Category("")]
    [DisplayName("Power")]
    [MeasurementUnit("W")]
    public Measurement Power {
        get { return _Power; }
        set {
            _Power = value;
            if (base.IsSecondMoreRecentlyChanged(nameof(Voltage), nameof(Current))) {
                _Voltage = Power / Current;
            } else {
                _Current = Power / Voltage;
            }
            _Resistance = Voltage / Current;
            base.StoreWrite(nameof(Power));
        }
    }


    #region Elements

    /// <inheritdoc/>
    public override ReadOnlyCollection<string> GetElementNames() {
        return new ReadOnlyCollection<string>([
            nameof(Voltage),
            nameof(Current),
            nameof(Resistance),
            nameof(Power),
        ]);
    }

    #endregion Elements

}
