namespace ResiCalc;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

/// <summary>
/// Voltage divider calculator.
/// </summary>
public class VoltageDivider : Calculator {

    public VoltageDivider()
        : base("Voltage Divider", "Calculate voltage divider resistors.") {

        _VRef = new Measurement(StoreRead(nameof(VRef), 2.048m), 3);
        _VMax = StoreRead(nameof(VMax), 5);
        _R1 = StoreRead(nameof(R1), 30000);
        _R2 = StoreRead(nameof(R2), 10000);
        _AdcBits = new Measurement(StoreRead(nameof(AdcBits), 12), 0);
        UpdateRatio();
        UpdateOther();
    }


    private Measurement _VRef;
    [Category("Input Properties")]
    [DisplayName("Reference Voltage")]
    [MeasurementUnit("V")]
    public Measurement VRef {
        get { return _VRef; }
        set {
            _VRef = new Measurement(value, _VRef.DigitCount);
            UpdateRatio();
            UpdateOther();
            base.StoreWrite(nameof(VRef));
        }
    }

    private Measurement _VMax;
    [Category("Input Properties")]
    [DisplayName("Maximum Voltage")]
    [MeasurementUnit("V")]
    public Measurement VMax {
        get { return _VMax; }
        set {
            _VMax = value;
            UpdateRatio();
            UpdateOther();
            base.StoreWrite(nameof(VMax));
        }
    }


    private Measurement _R1;
    [Category("~Resistor Values")]
    [DisplayName("R1")]
    [MeasurementUnit("Ω")]
    public Measurement R1 {
        get { return _R1; }
        set {
            _R1 = value;
            var ratio = (R1 + R2) / R2;
            _VMax = VRef * ratio;
            UpdateOther();
            base.StoreWrite(nameof(R1));
        }
    }

    private Measurement _R2;
    [Category("~Resistor Values")]
    [DisplayName("R2")]
    [MeasurementUnit("Ω")]
    public Measurement R2 {
        get { return _R2; }
        set {
            _R2 = value;
            UpdateRatio();
            UpdateOther();
            base.StoreWrite(nameof(R2));
        }
    }


    private Measurement _AdcBits;
    [Category("~ADC Properties")]
    [DisplayName("ADC Resolution")]
    [MeasurementUnit("bit")]
    public Measurement AdcBits {
        get { return _AdcBits; }
        set {
            _AdcBits = new Measurement(Math.Min(Math.Max(value, 4), 32), _AdcBits.DigitCount);
            UpdateOther();
            base.StoreWrite(nameof(AdcBits));
        }
    }

    private Measurement _AdcLsb;
    [Category("~ADC Properties")]
    [DisplayName("ADC LSB Resolution")]
    [MeasurementUnit("V")]
    public Measurement AdcLsb {
        get { return _AdcLsb; }
    }

    private Measurement _Impedance;
    [Category("~ADC Properties")]
    [DisplayName("Impedance")]
    [MeasurementUnit("Ω")]
    public Measurement Impedance {
        get { return _Impedance; }
    }


    private void UpdateRatio() {
        var ratio = VMax / VRef;
        if (ratio > 1) {
            if (R2.IsNull) { _R2 = 10000; }
            _R1 = R2 * (ratio - 1);
            _VMax = VRef * (R1 + R2) / R2;
        } else {
            _R1 = null;
            _R2 = null;
            _VMax = VRef;
        }
    }

    private void UpdateOther() {
        _AdcLsb = new Measurement(VMax / (decimal)Math.Pow(2, (int)AdcBits), -4);
        _Impedance = (R1 * R2) / (R1 + R2);
    }


    #region Elements

    /// <inheritdoc/>
    public override ReadOnlyCollection<string> GetMeasurementNames() {
        return new ReadOnlyCollection<string>([
            nameof(VRef),
            nameof(VMax),
            nameof(R1),
            nameof(R2),
            nameof(AdcBits),
            nameof(AdcLsb),
            nameof(Impedance),
        ]);
    }

    #endregion Elements

}
