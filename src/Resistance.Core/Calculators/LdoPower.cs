namespace ResiCalc;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

/// <summary>
/// LDO power calculator.
/// </summary>
public class LdoPower : Calculator {

    public LdoPower()
        : base("LDO Power", "Calculate power rating for LDO.") {

        _InputVoltage = new Measurement(StoreRead(nameof(InputVoltage), 12), digitCount: 1, useSI: true, minValue: 0, maxValue: null);
        _InputTolerance = new Measurement(StoreRead(nameof(InputTolerance), 5), digitCount: 1, useSI: false, minValue: 0, maxValue: 50);
        _OutputVoltage = new Measurement(StoreRead(nameof(OutputVoltage), 3.3m), digitCount: 1, useSI: true, minValue: 0, maxValue: null);
        _OutputTolerance = new Measurement(StoreRead(nameof(OutputTolerance), 1), digitCount: 1, useSI: false, minValue: 0, maxValue: 50);
        _OutputCurrent = new Measurement(StoreRead(nameof(OutputCurrent), 0.1m), digitCount: 3, useSI: true, minValue: 0, maxValue: null);
        _ThermalResistance = new Measurement(StoreRead(nameof(ThermalResistance), 80), digitCount: 0, useSI: false, minValue: 0, maxValue: null);
        _AmbientTemperature = new Measurement(StoreRead(nameof(AmbientTemperature), 25), digitCount: 0, useSI: false);
        _JunctionTemperature = new Measurement(null, digitCount: 0, useSI: false);
        UpdateValues();
    }


    private Measurement _InputVoltage;
    [Category("Input Properties")]
    [DisplayName("Input Voltage")]
    [MeasurementUnit("V")]
    public Measurement InputVoltage {
        get { return _InputVoltage; }
        set {
            _InputVoltage.Adjust(value);
            UpdateValues();
            base.StoreWrite(nameof(InputVoltage));
        }
    }

    private Measurement _InputTolerance;
    [Category("Input Properties")]
    [DisplayName("Input Tolerance")]
    [MeasurementUnit("%")]
    public Measurement InputTolerance {
        get { return _InputTolerance; }
        set {
            _InputTolerance.Adjust(value);
            UpdateValues();
            base.StoreWrite(nameof(InputTolerance));
        }
    }


    private Measurement _OutputVoltage;
    [Category("~Output Properties")]
    [DisplayName("Output Voltage")]
    [MeasurementUnit("V")]
    public Measurement OutputVoltage {
        get { return _OutputVoltage; }
        set {
            _OutputVoltage.Adjust(value);
            UpdateValues();
            base.StoreWrite(nameof(OutputVoltage));
        }
    }

    private Measurement _OutputTolerance;
    [Category("~Output Properties")]
    [DisplayName("Output Tolerance")]
    [MeasurementUnit("%")]
    public Measurement OutputTolerance {
        get { return _OutputTolerance; }
        set {
            _OutputTolerance.Adjust(value);
            UpdateValues();
            base.StoreWrite(nameof(OutputTolerance));
        }
    }

    private Measurement _OutputCurrent;
    [Category("~Output Properties")]
    [DisplayName("Output Current")]
    [MeasurementUnit("A")]
    public Measurement OutputCurrent {
        get { return _OutputCurrent; }
        set {
            _OutputCurrent.Adjust(value);
            UpdateValues();
            base.StoreWrite(nameof(OutputCurrent));
        }
    }


    private Measurement _PowerDissipation;
    [Category("~Dissipation")]
    [DisplayName("Power Dissipation")]
    [MeasurementUnit("W")]
    public Measurement PowerDissipation {
        get { return _PowerDissipation; }
        set {
            _PowerDissipation.Adjust(value);
            UpdateValues();
            base.StoreWrite(nameof(PowerDissipation));
        }
    }

    private Measurement _ThermalResistance;
    [Category("~Dissipation")]
    [DisplayName("Thermal Resistance")]
    [MeasurementUnit("°C/W")]
    public Measurement ThermalResistance {
        get { return _ThermalResistance; }
        set {
            _ThermalResistance.Adjust(value);
            UpdateValues();
            base.StoreWrite(nameof(ThermalResistance));
        }
    }

    private Measurement _AmbientTemperature;
    [Category("~Dissipation")]
    [DisplayName("Ambient Temperature")]
    [MeasurementUnit("°C")]
    public Measurement AmbientTemperature {
        get { return _AmbientTemperature; }
        set {
            _AmbientTemperature.Adjust(value);
            UpdateValues();
            base.StoreWrite(nameof(AmbientTemperature));
        }
    }

    private Measurement _JunctionTemperature;
    [Category("~Dissipation")]
    [DisplayName("Junction Temperature")]
    [MeasurementUnit("°C")]
    public Measurement JunctionTemperature {
        get { return _JunctionTemperature; }
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


    [Category("~Input Tolerances")]
    [DisplayName("0%")]
    public void CmdInputTolerance000() { InputTolerance = 0; }

    [Category("~Input Tolerances")]
    [DisplayName("0.5%")]
    public void CmdInputTolerance005() { InputTolerance = 0.5m; }

    [Category("~Input Tolerances")]
    [DisplayName("1%")]
    public void CmdInputTolerance010() { InputTolerance = 1; }

    [Category("~Input Tolerances")]
    [DisplayName("2%")]
    public void CmdInputTolerance020() { InputTolerance = 2; }

    [Category("~Input Tolerances")]
    [DisplayName("5%")]
    public void CmdInputTolerance050() { InputTolerance = 5; }

    [Category("~Input Tolerances")]
    [DisplayName("10%")]
    public void CmdInputTolerance100() { InputTolerance = 10; }


    [Category("Output Voltages")]
    [DisplayName("1.5 V")]
    public void CmdOutputVoltage015() { OutputVoltage = 1.5m; }

    [Category("Output Voltages")]
    [DisplayName("1.8 V")]
    public void CmdOutputVoltage018() { OutputVoltage = 1.8m; }

    [Category("Output Voltages")]
    [DisplayName("2.5 V")]
    public void CmdOutputVoltage025() { OutputVoltage = 2.5m; }

    [Category("Output Voltages")]
    [DisplayName("3.3 V")]
    public void CmdOutputVoltage033() { OutputVoltage = 3.3m; }

    [Category("Output Voltages")]
    [DisplayName("5 V")]
    public void CmdOutputVoltage050() { OutputVoltage = 5; }


    [Category("~Output Tolerances")]
    [DisplayName("0%")]
    public void CmdOutputTolerance000() { InputTolerance = 0; }

    [Category("~Output Tolerances")]
    [DisplayName("0.5%")]
    public void CmdOutputTolerance005() { InputTolerance = 0.5m; }

    [Category("~Output Tolerances")]
    [DisplayName("1%")]
    public void CmdOutputTolerance010() { InputTolerance = 1; }

    [Category("~Output Tolerances")]
    [DisplayName("2%")]
    public void CmdOutputTolerance020() { InputTolerance = 2; }

    [Category("~Output Tolerances")]
    [DisplayName("5%")]
    public void CmdOutputTolerance050() { InputTolerance = 5; }


    [Category("Common Packages")]
    [DisplayName("SOT-23")]
    public void CmdPackageSot23() { ThermalResistance = 230; }

    [Category("Common Packages")]
    [DisplayName("SOT-89")]
    public void CmdPackageSot89() { ThermalResistance = 180; }

    [Category("Common Packages")]
    [DisplayName("TO-92")]
    public void CmdPackageTo92() { ThermalResistance = 160; }

    [Category("Common Packages")]
    [DisplayName("SOT-223")]
    public void CmdPackageSot223() { ThermalResistance = 150; }

    [Category("Common Packages")]
    [DisplayName("SO-8")]
    public void CmdPackageSo8() { ThermalResistance = 100; }

    [Category("Common Packages")]
    [DisplayName("TO-252")]
    public void CmdPackageTo252() { ThermalResistance = 100; }

    [Category("Common Packages")]
    [DisplayName("D-PAK")]
    public void CmdPackageDPak() { ThermalResistance = 80; }

    [Category("Common Packages")]
    [DisplayName("TO-220")]
    public void CmdPackageTo220() { ThermalResistance = 50; }

    [Category("Common Packages")]
    [DisplayName("TO-3")]
    public void CmdPackageTo3() { ThermalResistance = 40; }


    private void UpdateValues() {
        var inputMax = InputVoltage * (100 + InputTolerance) / 100;
        var outputMin = OutputVoltage * (100 - OutputTolerance) / 100;
        _PowerDissipation.Adjust((inputMax - outputMin) * OutputCurrent);
        _JunctionTemperature.Adjust(AmbientTemperature + PowerDissipation * ThermalResistance);
    }

    #region Elements

    /// <inheritdoc/>
    public override ReadOnlyCollection<string> GetElementNames() {
        return new ReadOnlyCollection<string>([
            nameof(InputVoltage), nameof(InputTolerance), nameof(OutputVoltage), nameof(OutputTolerance), nameof(OutputCurrent), nameof(PowerDissipation), nameof(ThermalResistance), nameof(AmbientTemperature), nameof(JunctionTemperature),
            nameof(CmdInputVoltage033), nameof(CmdInputVoltage050), nameof(CmdInputVoltage090), nameof(CmdInputVoltage120), nameof(CmdInputVoltage240),
            nameof(CmdInputTolerance000), nameof(CmdInputTolerance005), nameof(CmdInputTolerance010), nameof(CmdInputTolerance020), nameof(CmdInputTolerance050), nameof(CmdInputTolerance100),
            nameof(CmdOutputVoltage015), nameof(CmdOutputVoltage018), nameof(CmdOutputVoltage025), nameof(CmdOutputVoltage033), nameof(CmdOutputVoltage050),
            nameof(CmdOutputTolerance000), nameof(CmdOutputTolerance005), nameof(CmdOutputTolerance010), nameof(CmdOutputTolerance020), nameof(CmdOutputTolerance050),
            nameof(CmdPackageSot23), nameof(CmdPackageSot89), nameof(CmdPackageTo92), nameof(CmdPackageSot223), nameof(CmdPackageSo8), nameof(CmdPackageTo252), nameof(CmdPackageDPak), nameof(CmdPackageTo220), nameof(CmdPackageTo3),
        ]);
    }

    #endregion Elements

}
