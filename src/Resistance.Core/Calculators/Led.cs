namespace ResiCalc;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;

/// <summary>
/// LED resistor calculator.
/// </summary>
public class Led : Calculator {

    public Led()
        : base("LED", "LED resistor calculator.") {

        _SourceVoltage = new Measurement(StoreRead(nameof(SourceVoltage), 5), digitCount: 3, useSI: true, minValue: 0, maxValue: null);
        _ForwardVoltage = new Measurement(StoreRead(nameof(ForwardVoltage), 1.8), digitCount: 3, useSI: true, minValue: 0, maxValue: null);
        _ForwardCurrent = new Measurement(StoreRead(nameof(ForwardCurrent), 0.001), digitCount: 3, useSI: true, minValue: 0, maxValue: null);
        _ResistorValue = new Measurement(null, digitCount: -2, NumberSeries.E24, NumberSeriesRounding.Up);

        _ResistorVoltageDrop = new Measurement(null, digitCount: -3, useSI: true);
        _ResistorCurrentFlow = new Measurement(null, digitCount: -3, useSI: true);
        _ResistorPowerDissipation = new Measurement(null, digitCount: -3, useSI: true);

        UpdateResistance();
    }


    private Measurement _SourceVoltage;
    [Category("Source Properties")]
    [DisplayName("Source Voltage")]
    [MeasurementUnit("V")]
    public Measurement SourceVoltage {
        get { return _SourceVoltage; }
        set {
            _SourceVoltage = _SourceVoltage.Adjust(value);
            UpdateResistance();
            base.StoreWrite(nameof(SourceVoltage));
        }
    }


    private Measurement _ForwardVoltage;
    [Category("~LED Properties")]
    [DisplayName("Forward Voltage")]
    [MeasurementUnit("V")]
    public Measurement ForwardVoltage {
        get { return _ForwardVoltage; }
        set {
            _ForwardVoltage = _ForwardVoltage.Adjust(value);
            UpdateResistance();
            base.StoreWrite(nameof(ForwardVoltage));
        }
    }

    private Measurement _ForwardCurrent;
    [Category("~LED Properties")]
    [DisplayName("Forward Current")]
    [MeasurementUnit("A")]
    public Measurement ForwardCurrent {
        get { return _ForwardCurrent; }
        set {
            _ForwardCurrent = _ForwardCurrent.Adjust(value);
            UpdateResistance();
            base.StoreWrite(nameof(ForwardCurrent));
        }
    }


    private Measurement _ResistorValue;
    [Category("~Resistor Properties")]
    [DisplayName("Resistance")]
    [MeasurementUnit("Ω")]
    public Measurement ResistorValue {
        get { return _ResistorValue; }
    }

    private Measurement _ResistorPowerDissipation;
    [Category("~Resistor Properties")]
    [DisplayName("Power Dissipation")]
    [MeasurementUnit("W")]
    public Measurement ResistorPowerDissipation {
        get { return _ResistorPowerDissipation; }
    }

    private string _ResistorPowerRating;
    [Category("~Resistor Properties")]
    [DisplayName("Power Rating")]
    [MeasurementUnit("W")]
    public string ResistorPowerRating {
        get { return _ResistorPowerRating; }
    }

    private Measurement _ResistorVoltageDrop;
    [Category("~Resistor Properties")]
    [DisplayName("Voltage Drop")]
    [MeasurementUnit("V")]
    public Measurement ResistorVoltageDrop {
        get { return _ResistorVoltageDrop; }
    }

    private Measurement _ResistorCurrentFlow;
    [Category("~Resistor Properties")]
    [DisplayName("Current Flow")]
    [MeasurementUnit("A")]
    public Measurement ResistorCurrentFlow {
        get { return _ResistorCurrentFlow; }
    }


    [Category("Common 20 mA LEDs")]
    [DisplayName("Infrared")]
    public void CmdInfrared20() { ForwardVoltage = 1.6; ForwardCurrent = 0.020; }

    [Category("Common 20 mA LEDs")]
    [DisplayName("Red")]
    [BackgroundColor("f44")]
    public void CmdRed20() { ForwardVoltage = 1.8; ForwardCurrent = 0.020; }

    [Category("Common 20 mA LEDs")]
    [DisplayName("Orange")]
    [BackgroundColor("f70")]
    public void CmdOrange20() { ForwardVoltage = 2.1; ForwardCurrent = 0.020; }

    [Category("Common 20 mA LEDs")]
    [DisplayName("Yellow")]
    [BackgroundColor("ee0")]
    public void CmdYellow20() { ForwardVoltage = 2.1; ForwardCurrent = 0.020; }

    [Category("Common 20 mA LEDs")]
    [DisplayName("Green")]
    [BackgroundColor("7f0")]
    public void CmdGreen20() { ForwardVoltage = 3.0; ForwardCurrent = 0.020; }

    [Category("Common 20 mA LEDs")]
    [DisplayName("Blue")]
    [BackgroundColor("55f")]
    public void CmdBlue20() { ForwardVoltage = 3.1; ForwardCurrent = 0.020; }

    [Category("Common 20 mA LEDs")]
    [DisplayName("Purple")]
    [BackgroundColor("a0a")]
    public void CmdPurple20() { ForwardVoltage = 3.1; ForwardCurrent = 0.020; }

    [Category("Common 20 mA LEDs")]
    [DisplayName("Pink")]
    [BackgroundColor("f7f")]
    public void CmdPink20() { ForwardVoltage = 3.3; ForwardCurrent = 0.020; }

    [Category("Common 20 mA LEDs")]
    [DisplayName("Violet")]
    [BackgroundColor("70f")]
    public void CmdViolet20() { ForwardVoltage = 3.4; ForwardCurrent = 0.020; }

    [Category("Common 20 mA LEDs")]
    [DisplayName("Ultraviolet")]
    public void CmdUltraviolet20() { ForwardVoltage = 3.8; ForwardCurrent = 0.020; }

    [Category("Common 20 mA LEDs")]
    [DisplayName("White")]
    [BackgroundColor("ffffff")]
    public void CmdWhite20() { ForwardVoltage = 3.5; ForwardCurrent = 0.020; }


    [Category("Common 10 mA LEDs")]
    [DisplayName("Infrared")]
    public void CmdInfrared10() { ForwardVoltage = 1.6; ForwardCurrent = 0.010; }

    [Category("Common 10 mA LEDs")]
    [DisplayName("Red")]
    [BackgroundColor("ff4040")]
    public void CmdRed10() { ForwardVoltage = 1.8; ForwardCurrent = 0.010; }

    [Category("Common 10 mA LEDs")]
    [DisplayName("Orange")]
    [BackgroundColor("f70")]
    public void CmdOrange10() { ForwardVoltage = 2.1; ForwardCurrent = 0.010; }

    [Category("Common 10 mA LEDs")]
    [DisplayName("Yellow")]
    [BackgroundColor("ee0")]
    public void CmdYellow10() { ForwardVoltage = 2.1; ForwardCurrent = 0.010; }

    [Category("Common 10 mA LEDs")]
    [DisplayName("Green")]
    [BackgroundColor("7f0")]
    public void CmdGreen10() { ForwardVoltage = 3.0; ForwardCurrent = 0.010; }

    [Category("Common 10 mA LEDs")]
    [DisplayName("Blue")]
    [BackgroundColor("55f")]
    public void CmdBlue10() { ForwardVoltage = 3.1; ForwardCurrent = 0.010; }

    [Category("Common 10 mA LEDs")]
    [DisplayName("Purple")]
    [BackgroundColor("a0a")]
    public void CmdPurple10() { ForwardVoltage = 3.1; ForwardCurrent = 0.010; }

    [Category("Common 10 mA LEDs")]
    [DisplayName("Pink")]
    [BackgroundColor("f7f")]
    public void CmdPink10() { ForwardVoltage = 3.3; ForwardCurrent = 0.010; }

    [Category("Common 10 mA LEDs")]
    [DisplayName("Violet")]
    [BackgroundColor("70f")]
    public void CmdViolet10() { ForwardVoltage = 3.4; ForwardCurrent = 0.010; }

    [Category("Common 10 mA LEDs")]
    [DisplayName("Ultraviolet")]
    public void CmdUltraviolet10() { ForwardVoltage = 3.8; ForwardCurrent = 0.010; }

    [Category("Common 10 mA LEDs")]
    [DisplayName("White")]
    [BackgroundColor("ffffff")]
    public void CmdWhite10() { ForwardVoltage = 3.5; ForwardCurrent = 0.010; }


    [Category("Common 1 mA LEDs")]
    [DisplayName("Infrared")]
    public void CmdInfrared01() { ForwardVoltage = 1.6; ForwardCurrent = 0.001; }

    [Category("Common 1 mA LEDs")]
    [DisplayName("Red")]
    [BackgroundColor("f44")]
    public void CmdRed01() { ForwardVoltage = 1.8; ForwardCurrent = 0.001; }

    [Category("Common 1 mA LEDs")]
    [DisplayName("Orange")]
    [BackgroundColor("f70")]
    public void CmdOrange01() { ForwardVoltage = 2.1; ForwardCurrent = 0.001; }

    [Category("Common 1 mA LEDs")]
    [DisplayName("Yellow")]
    [BackgroundColor("ee0")]
    public void CmdYellow01() { ForwardVoltage = 2.1; ForwardCurrent = 0.001; }

    [Category("Common 1 mA LEDs")]
    [DisplayName("Green")]
    [BackgroundColor("7f0")]
    public void CmdGreen01() { ForwardVoltage = 3.0; ForwardCurrent = 0.001; }

    [Category("Common 1 mA LEDs")]
    [DisplayName("Blue")]
    [BackgroundColor("55f")]
    public void CmdBlue01() { ForwardVoltage = 3.1; ForwardCurrent = 0.001; }

    [Category("Common 1 mA LEDs")]
    [DisplayName("Purple")]
    [BackgroundColor("a0a")]
    public void CmdPurple01() { ForwardVoltage = 3.1; ForwardCurrent = 0.001; }

    [Category("Common 1 mA LEDs")]
    [DisplayName("Pink")]
    [BackgroundColor("f7f")]
    public void CmdPink01() { ForwardVoltage = 3.3; ForwardCurrent = 0.001; }

    [Category("Common 1 mA LEDs")]
    [DisplayName("Violet")]
    [BackgroundColor("70f")]
    public void CmdViolet01() { ForwardVoltage = 3.4; ForwardCurrent = 0.001; }

    [Category("Common 1 mA LEDs")]
    [DisplayName("Ultraviolet")]
    public void CmdUltraviolet01() { ForwardVoltage = 3.8; ForwardCurrent = 0.001; }

    [Category("Common 1 mA LEDs")]
    [DisplayName("White")]
    [BackgroundColor("ffffff")]
    public void CmdWhite01() { ForwardVoltage = 3.5; ForwardCurrent = 0.001; }


    [Category("Source Voltage")]
    [DisplayName("3.3 V")]
    public void CmdSourceVoltage033() { SourceVoltage = 3.3; }

    [Category("Source Voltage")]
    [DisplayName("5 V")]
    public void CmdSourceVoltage050() { SourceVoltage = 5.0; }

    [Category("Source Voltage")]
    [DisplayName("9 V")]
    public void CmdSourceVoltage090() { SourceVoltage = 9; }

    [Category("Source Voltage")]
    [DisplayName("12 V")]
    public void CmdSourceVoltage120() { SourceVoltage = 12; }

    [Category("Source Voltage")]
    [DisplayName("15 V")]
    public void CmdSourceVoltage150() { SourceVoltage = 15; }

    [Category("Source Voltage")]
    [DisplayName("20 V")]
    public void CmdSourceVoltage200() { SourceVoltage = 20; }

    [Category("Source Voltage")]
    [DisplayName("24 V")]
    public void CmdSourceVoltage240() { SourceVoltage = 24; }


    private void UpdateResistance() {
        _ResistorVoltageDrop = _ResistorVoltageDrop.Adjust(SourceVoltage - ForwardVoltage);
        _ResistorValue = _ResistorValue.Adjust(ResistorVoltageDrop / ForwardCurrent);
        _ResistorCurrentFlow = _ResistorCurrentFlow.Adjust(ResistorVoltageDrop / ResistorValue);
        _ResistorPowerDissipation = _ResistorPowerDissipation.Adjust(ResistorVoltageDrop * ResistorCurrentFlow);
        if (ResistorPowerDissipation.IsNull) {
            _ResistorPowerRating = "";
        } else {
            _ResistorPowerRating = (decimal)ResistorPowerDissipation switch {
                < 1m / 16 => "1/16 W",
                < 1m / 10 => "1/10 W",
                < 1m / 8 => "1/8 W",
                < 1m / 4 => "1/4 W",
                < 1m / 2 => "1/2 W",
                < 1000m => Math.Ceiling(ResistorPowerDissipation).ToString("0", CultureInfo.CurrentCulture) + " W",
                _ => "Ludicrous",
            };
        }
    }


    #region Elements

    /// <inheritdoc/>
    public override ReadOnlyCollection<string> GetElementNames() {
        return new ReadOnlyCollection<string>([
            nameof(SourceVoltage),
            nameof(ForwardVoltage), nameof(ForwardCurrent),
            nameof(ResistorValue), nameof(ResistorPowerDissipation), nameof(ResistorPowerRating), nameof(ResistorVoltageDrop), nameof(ResistorCurrentFlow),
            nameof(CmdInfrared20), nameof(CmdRed20), nameof(CmdOrange20), nameof(CmdYellow20), nameof(CmdGreen20), nameof(CmdBlue20), nameof(CmdPurple20), nameof(CmdPink20), nameof(CmdViolet20), nameof(CmdUltraviolet20), nameof(CmdWhite20),
            nameof(CmdInfrared10), nameof(CmdRed10), nameof(CmdOrange10), nameof(CmdYellow10), nameof(CmdGreen10), nameof(CmdBlue10), nameof(CmdPurple10), nameof(CmdPink10), nameof(CmdViolet10), nameof(CmdUltraviolet10), nameof(CmdWhite10),
            nameof(CmdInfrared01), nameof(CmdRed01), nameof(CmdOrange01), nameof(CmdYellow01), nameof(CmdGreen01), nameof(CmdBlue01), nameof(CmdPurple01), nameof(CmdPink01), nameof(CmdViolet01), nameof(CmdUltraviolet01), nameof(CmdWhite01),
            nameof(CmdSourceVoltage033), nameof(CmdSourceVoltage050), nameof(CmdSourceVoltage090), nameof(CmdSourceVoltage120), nameof(CmdSourceVoltage150), nameof(CmdSourceVoltage200), nameof(CmdSourceVoltage240),
        ]);
    }

    /// <inheritdoc/>
    public override ReadOnlyCollection<KeyValuePair<string, string>> GetExampleImageResources() {
        return new ReadOnlyCollection<KeyValuePair<string, string>>([
            new KeyValuePair<string,string>("Led.png", "Example circuit"),
        ]);
    }

    #endregion Elements

}
