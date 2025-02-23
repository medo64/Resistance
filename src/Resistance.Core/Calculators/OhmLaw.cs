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
        try { _Resistance = _Voltage / _Current; } catch (ArithmeticException) { _Resistance = null; }
        try { _Power = _Voltage * _Current; } catch (ArithmeticException) { _Power = null; }
    }


    private decimal? _Voltage;
    [Category("")]
    [DisplayName("Voltage")]
    [MeasurementUnit("V")]
    public decimal? Voltage {
        get { return _Voltage; }
        set {
            _Voltage = value;
            if (base.IsSecondMoreRecentlyChanged(nameof(Current), nameof(Resistance))) {
                try { _Resistance = Voltage / Current; } catch (ArithmeticException) { _Resistance = null; }
            } else {
                try { _Current = Voltage / Resistance; } catch (ArithmeticException) { _Current = null; }
            }
            try { _Power = Voltage * Current; } catch (ArithmeticException) { _Power = null; }
            base.StoreWrite(nameof(Voltage));
        }
    }

    private decimal? _Current;
    [Category("")]
    [DisplayName("Current")]
    [MeasurementUnit("A")]
    public decimal? Current {
        get { return _Current; }
        set {
            _Current = value;
            if (base.IsSecondMoreRecentlyChanged(nameof(Voltage), nameof(Resistance))) {
                try { _Voltage = Current * Resistance; } catch (ArithmeticException) { _Voltage = null; }
            } else {
                try { _Resistance = Voltage / Current; } catch (ArithmeticException) { _Resistance = null; }
            }
            try { _Power = Voltage * Current; } catch (ArithmeticException) { _Power = null; }
            base.StoreWrite(nameof(Current));
        }
    }

    private decimal? _Resistance;
    [Category("")]
    [DisplayName("Resistance")]
    [MeasurementUnit("Ω")]
    public decimal? Resistance {
        get { return _Resistance; }
        set {
            _Resistance = value;
            if (base.IsSecondMoreRecentlyChanged(nameof(Voltage), nameof(Current))) {
                try { _Voltage = Current * Resistance; } catch (ArithmeticException) { _Voltage = null; }
            } else {
                try { _Current = Voltage / Resistance; } catch (ArithmeticException) { _Current = null; }
            }
            try { _Power = Voltage * Current; } catch (ArithmeticException) { _Power = null; }
            base.StoreWrite(nameof(Resistance));
        }
    }

    private decimal? _Power;
    [Category("")]
    [DisplayName("Power")]
    [MeasurementUnit("W")]
    public decimal? Power {
        get { return _Power; }
        set {
            _Power = value;
            if (base.IsSecondMoreRecentlyChanged(nameof(Voltage), nameof(Current))) {
                try { _Voltage = Power / Current; } catch (ArithmeticException) { _Voltage = null; }
            } else {
                try { _Current = Power / Voltage; } catch (ArithmeticException) { _Current = null; }
            }
            try { _Resistance = Voltage / Current; } catch (ArithmeticException) { _Resistance = null; }
            base.StoreWrite(nameof(Power));
        }
    }


    #region Elements

    /// <inheritdoc/>
    public override ReadOnlyCollection<string> GetMeasurementNames() {
        return new ReadOnlyCollection<string>([
            nameof(Voltage),
            nameof(Current),
            nameof(Resistance),
            nameof(Power),
        ]);
    }

    #endregion Elements

}
