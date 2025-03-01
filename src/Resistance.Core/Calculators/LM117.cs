namespace ResiCalc;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

/// <summary>
/// Ohm law calculator.
/// </summary>
public class LM117 : Calculator {

    public LM117()
        : base("LM117", "Calculate settings for LM117 IC.") {

        _InputVoltage = new Measurement(StoreRead(nameof(InputVoltage), 12), digitCount: 3, useSI: true, minValue: 0, maxValue: null);
        _DesiredVoltage = new Measurement(StoreRead(nameof(DesiredVoltage), 5), digitCount: 3, useSI: true, minValue: 0, maxValue: null);
        _DesiredCurrent = new Measurement(StoreRead(nameof(DesiredCurrent), 0.1m), digitCount: 3, useSI: true, minValue: 0, maxValue: null);
        _R1 = new Measurement(StoreRead(nameof(R1), 240), digitCount: -3, NumberSeries.E24, NumberSeriesRounding.Down, minValue: 0, maxValue: null);
        _R2 = new Measurement(null, digitCount: -3, NumberSeries.E24, NumberSeriesRounding.Down);
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
    [DisplayName("R2")]
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
        _OutputVoltage.Adjust(1.25m + (R2 / R1));
        _VoltageDrop.Adjust(InputVoltage - OutputVoltage);
        _PowerDissipation.Adjust(VoltageDrop * DesiredCurrent);
        _MinimumLoad.Adjust(1.25m / R1);
    }


    [Category("Input Voltages")]
    [DisplayName("5 V")]
    public void CmdInputVoltage05() { InputVoltage = 5; }

    [Category("Input Voltages")]
    [DisplayName("9 V")]
    public void CmdInputVoltage09() { InputVoltage = 9; }

    [Category("Input Voltages")]
    [DisplayName("12 V")]
    public void CmdInputVoltage12() { InputVoltage = 12; }


    [Category("~Output Voltages")]
    [DisplayName("3.3 V")]
    public void CmdOutputVoltage33() { DesiredVoltage = 3.3m; }

    [Category("~Output Voltages")]
    [DisplayName("5 V")]
    public void CmdOutputVoltage50() { DesiredVoltage = 5m; }


    [Category("Resistors")]
    [DisplayName("240 Ω")]
    public void CmdR1() { R1 = 240; }


    #region Elements

    /// <inheritdoc/>
    public override ReadOnlyCollection<string> GetElementNames() {
        return new ReadOnlyCollection<string>([
            nameof(InputVoltage), nameof(DesiredVoltage), nameof(DesiredCurrent),
            nameof(R1), nameof(R2),
            nameof(OutputVoltage), nameof(MinimumLoad), nameof(VoltageDrop), nameof(PowerDissipation),
            nameof(CmdInputVoltage05), nameof(CmdInputVoltage05), nameof(CmdInputVoltage12),
            nameof(CmdOutputVoltage33), nameof(CmdOutputVoltage50),
            nameof(CmdR1),
        ]);
    }

    /// <inheritdoc/>
    public override ReadOnlyCollection<KeyValuePair<string, string>> GetExampleImageResources() {
        return new ReadOnlyCollection<KeyValuePair<string, string>>([
            new KeyValuePair<string,string>("Lm117.png", "Example circuit"),
        ]);
    }

    #endregion Elements

}
