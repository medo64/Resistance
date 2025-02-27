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

        _Voltage = new Measurement(StoreRead(nameof(Voltage), 5), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _Current = new Measurement(StoreRead(nameof(Current), 1), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _Resistance = new Measurement(_Voltage / _Current, digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _Power = new Measurement(_Voltage * _Current, digitCount: -3, useSI: true, minValue: 0, maxValue: null);
    }


    private Measurement _Voltage;
    [Category("")]
    [DisplayName("Voltage")]
    [MeasurementUnit("V")]
    public Measurement Voltage {
        get { return _Voltage; }
        set {
            _Voltage.Adjust(value);
            if (base.IsSecondMoreRecentlyChanged(nameof(Current), nameof(Resistance))) {
                _Resistance.Adjust(Voltage / Current);
            } else {
                _Current.Adjust(Voltage / Resistance);
            }
            _Power.Adjust(Voltage * Current);
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
            _Current.Adjust(value);
            if (base.IsSecondMoreRecentlyChanged(nameof(Voltage), nameof(Resistance))) {
                _Voltage.Adjust(Current * Resistance);
            } else {
                _Resistance.Adjust(Voltage / Current);
            }
            _Power.Adjust(Voltage * Current);
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
            _Resistance.Adjust(value);
            if (base.IsSecondMoreRecentlyChanged(nameof(Voltage), nameof(Current))) {
                _Voltage.Adjust(Current * Resistance);
            } else {
                _Current.Adjust(Voltage / Resistance);
            }
            _Power.Adjust(Voltage * Current);
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
            _Power.Adjust(value);
            if (base.IsSecondMoreRecentlyChanged(nameof(Voltage), nameof(Current))) {
                _Voltage.Adjust(Power / Current);
            } else {
                _Current.Adjust(Power / Voltage);
            }
            _Resistance.Adjust(Voltage / Current);
            base.StoreWrite(nameof(Power));
        }
    }


    #region Elements

    /// <inheritdoc/>
    public override ReadOnlyCollection<string> GetElementNames() {
        return new ReadOnlyCollection<string>([
            nameof(Voltage), nameof(Current), nameof(Resistance), nameof(Power),
        ]);
    }

    #endregion Elements

}
