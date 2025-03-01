namespace ResiCalc;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

/// <summary>
/// LDO power calculator.
/// </summary>
public class MicrochipPicTmr0 : Calculator {

    public MicrochipPicTmr0()
        : base("Microchip PIC TMR0", "Calculate Microchip TMR0 settings.") {
        _ClockFrequency = new Measurement(StoreRead(nameof(ClockFrequency), 48000000), digitCount: 1, useSI: true, minValue: 0, maxValue: null);
        _Tmr0Prescaler = new Measurement(StoreRead(nameof(Tmr0Prescaler), 8), digitCount: 1, useSI: false, minValue: 1, maxValue: 256);
        _Tmr0Bits = new Measurement(StoreRead(nameof(Tmr0Bits), 8), digitCount: 1, useSI: true, minValue: 8, maxValue: 8);
        _Tmr0Frequency = new Measurement(null, digitCount: 3, useSI: true);
        _Tmr0Period = new Measurement(null, digitCount: 3, useSI: true);
        UpdateValues();
    }


    private Measurement _ClockFrequency;
    [Category("PIC Settings")]
    [DisplayName("Clock Frequency")]
    [MeasurementUnit("Hz")]
    public Measurement ClockFrequency {
        get { return _ClockFrequency; }
        set {
            _ClockFrequency.Adjust(value);
            UpdateValues();
            base.StoreWrite(nameof(ClockFrequency));
        }
    }

    private Measurement _Tmr0Prescaler;
    [Category("PIC Settings")]
    [DisplayName("TMR0 Prestaler")]
    [MeasurementUnit(":1")]
    public Measurement Tmr0Prescaler {
        get { return _Tmr0Prescaler; }
        set {
            var x = value;
            var i = 0;
            while (x > 1) {
                x /= 2;
                i++;
            }
            _Tmr0Prescaler.Adjust((decimal)Math.Pow(2, i));
            UpdateValues();
            base.StoreWrite(nameof(Tmr0Prescaler));
        }
    }

    private Measurement _Tmr0Bits;
    [Category("PIC Settings")]
    [DisplayName("TMR0 Bits")]
    [MeasurementUnit("bit")]
    public Measurement Tmr0Bits {
        get { return _Tmr0Bits; }
        set {
            _Tmr0Bits.Adjust(value);
            UpdateValues();
            base.StoreWrite(nameof(Tmr0Bits));
        }
    }


    private Measurement _Tmr0Frequency;
    [Category("~Timer Properties")]
    [DisplayName("TMR0 Frequency")]
    [MeasurementUnit("Hz")]
    public Measurement Tmr0Frequency {
        get { return _Tmr0Frequency; }
    }

    private Measurement _Tmr0Period;
    [Category("~Timer Properties")]
    [DisplayName("TMR0 Period")]
    [MeasurementUnit("s")]
    public Measurement Tmr0Period {
        get { return _Tmr0Period; }
    }


    [Category("Frequencies")]
    [DisplayName("32.768 kHz")]
    public void CmdFrequency00032() { ClockFrequency = 32768; }

    [Category("Frequencies")]
    [DisplayName("1 MHz")]
    public void CmdFrequency01000() { ClockFrequency = 1000000; }

    [Category("Frequencies")]
    [DisplayName("4 MHz")]
    public void CmdFrequency04000() { ClockFrequency = 4000000; }

    [Category("Frequencies")]
    [DisplayName("8 MHz")]
    public void CmdFrequency08000() { ClockFrequency = 8000000; }

    [Category("Frequencies")]
    [DisplayName("16 MHz")]
    public void CmdFrequency16000() { ClockFrequency = 16000000; }

    [Category("Frequencies")]
    [DisplayName("32 MHz")]
    public void CmdFrequency32000() { ClockFrequency = 32000000; }

    [Category("Frequencies")]
    [DisplayName("48 MHz")]
    public void CmdFrequency48000() { ClockFrequency = 48000000; }

    [Category("Frequencies")]
    [DisplayName("64 MHz")]
    public void CmdFrequency64000() { ClockFrequency = 64000000; }


    [Category("Prestaler Values")]
    [DisplayName("2:1")]
    public void CmdPrescaler002() { Tmr0Prescaler = 2; }

    [Category("Prestaler Values")]
    [DisplayName("4:1")]
    public void CmdPrescaler004() { Tmr0Prescaler = 4; }

    [Category("Prestaler Values")]
    [DisplayName("8:1")]
    public void CmdPrescaler008() { Tmr0Prescaler = 8; }

    [Category("Prestaler Values")]
    [DisplayName("16:1")]
    public void CmdPrescaler016() { Tmr0Prescaler = 16; }

    [Category("Prestaler Values")]
    [DisplayName("32:1")]
    public void CmdPrescaler032() { Tmr0Prescaler = 32; }

    [Category("Prestaler Values")]
    [DisplayName("64:1")]
    public void CmdPrescaler064() { Tmr0Prescaler = 64; }

    [Category("Prestaler Values")]
    [DisplayName("128:1")]
    public void CmdPrescaler128() { Tmr0Prescaler = 128; }

    [Category("Prestaler Values")]
    [DisplayName("256:1")]
    public void CmdPrescaler256() { Tmr0Prescaler = 256; }


    private void UpdateValues() {
        var maxCount = Math.Pow(2, (int)Tmr0Bits);
        _Tmr0Frequency.Adjust(ClockFrequency / 4 / Tmr0Prescaler / maxCount);
        _Tmr0Period.Adjust(1 / Tmr0Frequency);
    }

    #region Elements

    /// <inheritdoc/>
    public override ReadOnlyCollection<string> GetElementNames() {
        return new ReadOnlyCollection<string>([
            nameof(ClockFrequency), nameof(Tmr0Prescaler), nameof(Tmr0Bits), nameof(Tmr0Frequency), nameof(Tmr0Period),
            nameof(CmdFrequency00032), nameof(CmdFrequency04000), nameof(CmdFrequency08000), nameof(CmdFrequency16000), nameof(CmdFrequency32000), nameof(CmdFrequency48000), nameof(CmdFrequency64000),
            nameof(CmdPrescaler002), nameof(CmdPrescaler004), nameof(CmdPrescaler008), nameof(CmdPrescaler016), nameof(CmdPrescaler032), nameof(CmdPrescaler064), nameof(CmdPrescaler128), nameof(CmdPrescaler256),
        ]);
    }

    #endregion Elements

}
