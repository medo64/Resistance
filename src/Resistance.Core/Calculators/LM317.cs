namespace ResiCalc;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

/// <summary>
/// Ohm law calculator.
/// </summary>
public class LM317 : Calculator {

    public LM317()
        : base("LM317 Voltage Regulator") {

        _InputVoltage = new Measurement(StoreRead(nameof(InputVoltage), 12), digitCount: 3, useSI: true, minValue: 0, maxValue: null);
        _DesiredVoltage = new Measurement(StoreRead(nameof(DesiredVoltage), 5), digitCount: 3, useSI: true, minValue: 0, maxValue: null);
        _DesiredCurrent = new Measurement(StoreRead(nameof(DesiredCurrent), 0.1m), digitCount: 3, useSI: true, minValue: 0, maxValue: null);
        _R1 = new Measurement(StoreRead(nameof(R1), 120), digitCount: -3, NumberSeries.E24, NumberSeriesRounding.Nearest, minValue: 110, maxValue: 130);
        _R2 = new Measurement(null, digitCount: -3, NumberSeries.E24, NumberSeriesRounding.Nearest);
        _OutputVoltage = new Measurement(null, digitCount: 2, useSI: true);
        _MinimumLoad = new Measurement(null, digitCount: 1, useSI: true);
        _VoltageDrop = new Measurement(null, digitCount: 1, useSI: true);
        _PowerDissipation = new Measurement(null, digitCount: 1, useSI: true);
        Update();
    }


    private Measurement _InputVoltage;
    [Category("Desired Properties")]
    [DisplayName("Input Voltage")]
    [MeasurementUnit("V")]
    public Measurement InputVoltage {
        get { return _InputVoltage; }
        set {
            _InputVoltage.Adjust(value);
            Update();
            base.StoreWrite(nameof(InputVoltage));
        }
    }

    private Measurement _DesiredVoltage;
    [Category("Desired Properties")]
    [DisplayName("Output Voltage")]
    [MeasurementUnit("V")]
    public Measurement DesiredVoltage {
        get { return _DesiredVoltage; }
        set {
            _DesiredVoltage.Adjust(value);
            Update();
            base.StoreWrite(nameof(DesiredVoltage));
        }
    }

    private Measurement _DesiredCurrent;
    [Category("Desired Properties")]
    [DisplayName("Output Current")]
    [MeasurementUnit("A")]
    public Measurement DesiredCurrent {
        get { return _DesiredCurrent; }
        set {
            _DesiredCurrent.Adjust(value);
            Update();
            base.StoreWrite(nameof(DesiredCurrent));
        }
    }


    private Measurement _R1;
    [Category("~Resistors")]
    [DisplayName("R1")]
    [MeasurementUnit("Ω")]
    public Measurement R1 {
        get { return _R1; }
        set {
            _R1.Adjust(value);
            Update();
            base.StoreWrite(nameof(R1));
        }
    }

    private Measurement _R2;
    [Category("~Resistors")]
    [DisplayName("R2 ({SeriesText})")]
    [MeasurementUnit("Ω")]
    public Measurement R2 {
        get { return _R2; }
    }


    private Measurement _OutputVoltage;
    [Category("~Output Properties")]
    [DisplayName("Output Voltage")]
    [MeasurementUnit("V")]
    public Measurement OutputVoltage {
        get { return _OutputVoltage; }
    }

    private Measurement _MinimumLoad;
    [Category("~Output Properties")]
    [DisplayName("Minimum Load")]
    [MeasurementUnit("A")]
    public Measurement MinimumLoad {
        get { return _MinimumLoad; }
    }

    private Measurement _VoltageDrop;
    [Category("~Output Properties")]
    [DisplayName("Voltage Drop")]
    [MeasurementUnit("V")]
    public Measurement VoltageDrop {
        get { return _VoltageDrop; }
    }

    private Measurement _PowerDissipation;
    [Category("~Output Properties")]
    [DisplayName("Power Dissipation")]
    [MeasurementUnit("W")]
    public Measurement PowerDissipation {
        get { return _PowerDissipation; }
    }


    private void Update() {
        _R2.Adjust(R1 * (DesiredVoltage - 1.25));
        _OutputVoltage.Adjust(1.25 + (R2 / R1));
        _VoltageDrop.Adjust(InputVoltage - OutputVoltage);
        _PowerDissipation.Adjust(VoltageDrop * DesiredCurrent);
        _MinimumLoad.Adjust(1.25 / R1);
    }


    [Category("Input Voltages")]
    [DisplayName("3.3 V")]
    public void CmdInputVoltage033() { InputVoltage = 3.3m; }

    [Category("Input Voltages")]
    [DisplayName("5 V")]
    public void CmdInputVoltage050() { InputVoltage = 5; }

    [Category("Input Voltages")]
    [DisplayName("9 V")]
    public void CmdInputVoltage090() { InputVoltage = 9; }

    [Category("Input Voltages")]
    [DisplayName("12 V")]
    public void CmdInputVoltage120() { InputVoltage = 12; }

    [Category("Input Voltages")]
    [DisplayName("24 V")]
    public void CmdInputVoltage240() { InputVoltage = 24; }


    [Category("Output Voltages")]
    [DisplayName("1.5 V")]
    public void CmdOutputVoltage015() { DesiredVoltage = 1.5m; }

    [Category("Output Voltages")]
    [DisplayName("1.8 V")]
    public void CmdOutputVoltage018() { DesiredVoltage = 1.8m; }

    [Category("Output Voltages")]
    [DisplayName("2.5 V")]
    public void CmdOutputVoltage025() { DesiredVoltage = 2.5m; }

    [Category("Output Voltages")]
    [DisplayName("3.3 V")]
    public void CmdOutputVoltage033() { DesiredVoltage = 3.3m; }

    [Category("Output Voltages")]
    [DisplayName("5 V")]
    public void CmdOutputVoltage050() { DesiredVoltage = 5; }


    [Category("Resistors")]
    [DisplayName("120 Ω")]
    public void CmdR1() { R1 = 120; }


    [Category("Resistance Series")]
    [DisplayName("E24")]
    public void CmdR2E24() {
        _R1.AdjustSeriesEx(NumberSeries.E24);
        _R2.AdjustSeriesEx(NumberSeries.E24);
        Update();
    }

    [Category("Resistance Series")]
    [DisplayName("E48")]
    public void CmdR2E48() {
        _R1.AdjustSeriesEx(NumberSeries.E48);
        _R2.AdjustSeriesEx(NumberSeries.E48);
        Update();
    }

    [Category("Resistance Series")]
    [DisplayName("E96")]
    public void CmdR2E96() {
        _R1.AdjustSeriesEx(NumberSeries.E96);
        _R2.AdjustSeriesEx(NumberSeries.E96);
        Update();
    }

    [Category("Resistance Series")]
    [DisplayName("E192")]
    public void CmdR2E192() {
        _R1.AdjustSeriesEx(NumberSeries.E192);
        _R2.AdjustSeriesEx(NumberSeries.E192);
        Update();
    }


    #region Elements

    /// <inheritdoc/>
    public override ReadOnlyCollection<string> GetElementNames() {
        return new ReadOnlyCollection<string>([
            nameof(InputVoltage), nameof(DesiredVoltage), nameof(DesiredCurrent),
            nameof(R1), nameof(R2),
            nameof(OutputVoltage), nameof(MinimumLoad), nameof(VoltageDrop), nameof(PowerDissipation),
            nameof(CmdInputVoltage033), nameof(CmdInputVoltage050), nameof(CmdInputVoltage090), nameof(CmdInputVoltage120), nameof(CmdInputVoltage240),
            nameof(CmdOutputVoltage015), nameof(CmdOutputVoltage018), nameof(CmdOutputVoltage025), nameof(CmdOutputVoltage033), nameof(CmdOutputVoltage050),
            nameof(CmdR1),
            nameof(CmdR2E24), nameof(CmdR2E48), nameof(CmdR2E96), nameof(CmdR2E192),
        ]);
    }

    /// <inheritdoc/>
    public override ReadOnlyCollection<KeyValuePair<string, string>> GetExampleImageResources() {
        return new ReadOnlyCollection<KeyValuePair<string, string>>([
            new KeyValuePair<string,string>("Lm317.png", "Example circuit"),
        ]);
    }

    #endregion Elements

}
