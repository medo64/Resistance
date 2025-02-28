namespace ResiCalc;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

/// <summary>
/// Voltage divider calculator.
/// </summary>
public class VoltageDivider : Calculator {

    public VoltageDivider()
        : base("Voltage Divider", "Calculate voltage divider resistors.") {

        _VRef = new Measurement(StoreRead(nameof(VRef), 2.048m), digitCount: 3, useSI: true, minValue: 0, maxValue: null);
        _VMax = new Measurement(StoreRead(nameof(VMax), 2.048m), digitCount: 3, useSI: true, minValue: 0, maxValue: null);
        _Ratio = new Measurement(null, digitCount: 2, useSI: false);
        _R1 = new Measurement(StoreRead(nameof(R1), 30000), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _R2 = new Measurement(StoreRead(nameof(R2), 10000), digitCount: -3, useSI: true, minValue: 0, maxValue: null);
        _AdcBits = new Measurement(StoreRead(nameof(AdcBits), 12), digitCount: 0, useSI: false, minValue: 4, maxValue: 32);
        _AdcSteps = new Measurement(null, digitCount: 0, useSI: false);
        _AdcLsb = new Measurement(null, digitCount: -3, useSI: true);
        _Impedance = new Measurement(null, digitCount: -3, useSI: true);
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
            _VRef.Adjust(value);
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
            _VMax.Adjust(value);
            UpdateRatio();
            UpdateOther();
            base.StoreWrite(nameof(VMax));
        }
    }

    private Measurement _Ratio;
    [Category("Input Properties")]
    [DisplayName("Ratio")]
    [MeasurementUnit(":1")]
    public Measurement Ratio {
        get { return _Ratio; }
    }


    private Measurement _R1;
    [Category("~Resistor Values")]
    [DisplayName("R1")]
    [MeasurementUnit("Ω")]
    public Measurement R1 {
        get { return _R1; }
        set {
            _R1.Adjust(value);
            _Ratio.Adjust((R1 + R2) / R2);
            _VMax.Adjust(VRef * Ratio);
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
            _R2.Adjust(value);
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
            _AdcBits.Adjust(value);
            UpdateOther();
            base.StoreWrite(nameof(AdcBits));
        }
    }

    private Measurement _AdcSteps;
    [Category("~ADC Properties")]
    [DisplayName("ADC Steps")]
    [MeasurementUnit("")]
    public Measurement AdcSteps {
        get { return _AdcSteps; }
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


    [Category("Reference Voltage")]
    [DisplayName("1.024 V")]
    public void CmdVRef1024() { VRef = 1.024m; }

    [Category("Reference Voltage")]
    [DisplayName("1.2 V")]
    public void CmdVRef1200() { VRef = 1.200m; }

    [Category("Reference Voltage")]
    [DisplayName("1.25 V")]
    public void CmdVRef1250() { VRef = 1.250m; }

    [Category("Reference Voltage")]
    [DisplayName("2.048 V")]
    public void CmdVRef2048() { VRef = 2.048m; }

    [Category("Reference Voltage")]
    [DisplayName("2.5 V")]
    public void CmdVRef2500() { VRef = 2.500m; }

    [Category("Reference Voltage")]
    [DisplayName("3.3 V")]
    public void CmdVRef3300() { VRef = 3.300m; }

    [Category("Reference Voltage")]
    [DisplayName("4.096 V")]
    public void CmdVRef4096() { VRef = 4.096m; }

    [Category("Reference Voltage")]
    [DisplayName("5.0 V")]
    public void CmdVRef5000() { VRef = 5.000m; }


    [Category("Maximum Voltage")]
    [DisplayName("5 V ±10%")]
    public void CmdVMax05() { VMax = 5 * 1.1m; }

    [Category("Maximum Voltage")]
    [DisplayName("9 V ±10%")]
    public void CmdVMax09() { VMax = 9 * 1.1m; }

    [Category("Maximum Voltage")]
    [DisplayName("12 V ±10%")]
    public void CmdVMax12() { VMax = 12 * 1.1m; }

    [Category("Maximum Voltage")]
    [DisplayName("15 V ±10%")]
    public void CmdVMax15() { VMax = 15 * 1.1m; }

    [Category("Maximum Voltage")]
    [DisplayName("24 V ±10%")]
    public void CmdVMax24() { VMax = 24 * 1.1m; }

    [Category("Maximum Voltage")]
    [DisplayName("30 V ±10%")]
    public void CmdVMax30() { VMax = 30 * 1.1m; }

    [Category("Maximum Voltage")]
    [DisplayName("36 V ±10%")]
    public void CmdVMax36() { VMax = 36 * 1.1m; }

    [Category("Maximum Voltage")]
    [DisplayName("48 V ±10%")]
    public void CmdVMax48() { VMax = 48 * 1.1m; }

    [Category("Maximum Voltage")]
    [DisplayName("60 V ±10%")]
    public void CmdVMax60() { VMax = 60 * 1.1m; }


    [Category("ADC Resolution")]
    [DisplayName("8 bit")]
    public void CmdAdcBits08() { AdcBits = 8; }

    [Category("ADC Resolution")]
    [DisplayName("10 bit")]
    public void CmdAdcBits10() { AdcBits = 10; }

    [Category("ADC Resolution")]
    [DisplayName("12 bit")]
    public void CmdAdcBits12() { AdcBits = 12; }

    [Category("ADC Resolution")]
    [DisplayName("14 bit")]
    public void CmdAdcBits14() { AdcBits = 14; }

    [Category("ADC Resolution")]
    [DisplayName("16 bit")]
    public void CmdAdcBits16() { AdcBits = 16; }

    [Category("ADC Resolution")]
    [DisplayName("18 bit")]
    public void CmdAdcBits18() { AdcBits = 18; }


    [Category("~R2")]
    [DisplayName("1 kΩ")]
    public void CmdR21k() { R2 = 1000; }

    [Category("~R2")]
    [DisplayName("10 kΩ")]
    public void CmdR210k() { R2 = 10000; }


    private void UpdateRatio() {
        var ratio = VMax / VRef;
        if (ratio > 1) {
            _Ratio.Adjust(ratio);
            if (R2.IsNull) { _R2.Adjust(10000); }
            _R1.Adjust(R2 * (Ratio - 1));
            _VMax.Adjust(VRef * (R1 + R2) / R2);
        } else {
            _Ratio.Adjust(1);
            _R1.Adjust(0);
            _R2.Adjust(null);
            _VMax.Adjust(VRef);
        }
    }

    private void UpdateOther() {
        _AdcSteps.Adjust((decimal)Math.Pow(2, (int)AdcBits));
        _AdcLsb.Adjust(VMax / AdcSteps);
        _Impedance.Adjust((R1 * R2) / (R1 + R2));
    }


    #region Elements

    /// <inheritdoc/>
    public override ReadOnlyCollection<string> GetElementNames() {
        return new ReadOnlyCollection<string>([
            nameof(VRef), nameof(VMax), nameof(Ratio), nameof(R1), nameof(R2), nameof(AdcBits), nameof(AdcSteps), nameof(AdcLsb), nameof(Impedance),
            nameof(CmdVRef1024), nameof(CmdVRef1200), nameof(CmdVRef1250), nameof(CmdVRef2048), nameof(CmdVRef2500), nameof(CmdVRef3300), nameof(CmdVRef4096), nameof(CmdVRef5000),
            nameof(CmdVMax05), nameof(CmdVMax09), nameof(CmdVMax12), nameof(CmdVMax15), nameof(CmdVMax24), nameof(CmdVMax30), nameof(CmdVMax36), nameof(CmdVMax48), nameof(CmdVMax60),
            nameof (CmdAdcBits08), nameof (CmdAdcBits10), nameof (CmdAdcBits12), nameof (CmdAdcBits14), nameof (CmdAdcBits16), nameof (CmdAdcBits18),
            nameof(CmdR21k), nameof(CmdR210k),
        ]);
    }

    /// <inheritdoc/>
    public override ReadOnlyCollection<KeyValuePair<string, string>> GetExampleImageResources() {
        return new ReadOnlyCollection<KeyValuePair<string, string>>([
            new KeyValuePair<string,string>("VoltageDivider.png", "Example circuit"),
        ]);
    }

    #endregion Elements

}
