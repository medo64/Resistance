namespace ResiCalc;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

/// <summary>
/// LDO power calculator.
/// </summary>
public class MicrochipPicPwm : Calculator {

    public MicrochipPicPwm()
        : base("Microchip PIC PWM") {

        _ClockFrequency = new Measurement(StoreRead(nameof(ClockFrequency), 48000000), digitCount: 1, useSI: true, minValue: 0, maxValue: null);
        _Pr2Register = new Measurement(StoreRead(nameof(PR2Register), 255), digitCount: 1, useSI: true, minValue: 0, maxValue: 255);
        _Tmr2Prescaler = new Measurement(StoreRead(nameof(Tmr2Prescaler), 4), digitCount: 1, useSI: false, minValue: 1, maxValue: 4);
        _PwmResolutionBits = new Measurement(null, digitCount: 3, useSI: true);
        _PwmFrequency = new Measurement(null, digitCount: 3, useSI: true);
        _PwmPeriod = new Measurement(null, digitCount: 3, useSI: true);
        _InputVoltage = new Measurement(StoreRead(nameof(InputVoltage), 5), digitCount: 3, useSI: true);
        _PwmResolutionVolts = new Measurement(null, digitCount: 3, useSI: true);
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

    private Measurement _Pr2Register;
    [Category("PIC Settings")]
    [DisplayName("PR2 Register Value")]
    [MeasurementUnit("")]
    public Measurement PR2Register {
        get { return _Pr2Register; }
        set {
            _Pr2Register.Adjust(value);
            UpdateValues();
            base.StoreWrite(nameof(PR2Register));
        }
    }

    private Measurement _Tmr2Prescaler;
    [Category("PIC Settings")]
    [DisplayName("TMR2 Prestaler")]
    [MeasurementUnit(":1")]
    public Measurement Tmr2Prescaler {
        get { return _Tmr2Prescaler; }
        set {
            if (value <= 1) {
                _Tmr2Prescaler.Adjust(1);
            } else {
                _Tmr2Prescaler.Adjust(4);
            }
            UpdateValues();
            base.StoreWrite(nameof(Tmr2Prescaler));
        }
    }


    private Measurement _PwmResolutionBits;
    [Category("~PWM Properties")]
    [DisplayName("PWM Resolution")]
    [MeasurementUnit("bit")]
    public Measurement PwmResolutionBits {
        get { return _PwmResolutionBits; }
    }

    private Measurement _PwmFrequency;
    [Category("~PWM Properties")]
    [DisplayName("PWM Frequency")]
    [MeasurementUnit("Hz")]
    public Measurement PwmFrequency {
        get { return _PwmFrequency; }
    }

    private Measurement _PwmPeriod;
    [Category("~PWM Properties")]
    [DisplayName("PWM Period")]
    [MeasurementUnit("s")]
    public Measurement PwmPeriod {
        get { return _PwmPeriod; }
    }


    private Measurement _InputVoltage;
    [Category("~Averaged DC Properties")]
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

    private Measurement _PwmResolutionVolts;
    [Category("~Averaged DC Properties")]
    [DisplayName("PWM Resolution")]
    [MeasurementUnit("V")]
    public Measurement PwmResolutionVolts {
        get { return _PwmResolutionVolts; }
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
    [DisplayName("1:1")]
    public void CmdPrescaler1() { Tmr2Prescaler = 1; }

    [Category("Prestaler Values")]
    [DisplayName("4:1")]
    public void CmdPrescaler4() { Tmr2Prescaler = 4; }

    [Category("Prestaler Values")]
    [DisplayName("128:1")]
    public void CmdPrescaler128() { Tmr2Prescaler = 128; }


    [Category("~PR2 Values")]
    [DisplayName("255")]
    public void CmdPR2255() { Tmr2Prescaler = 255; }

    [Category("~PR2 Values")]
    [DisplayName("191")]
    public void CmdPR2191() { Tmr2Prescaler = 191; }

    [Category("~PR2 Values")]
    [DisplayName("127")]
    public void CmdPR2127() { Tmr2Prescaler = 127; }

    [Category("~PR2 Values")]
    [DisplayName("63")]
    public void CmdPR2063() { Tmr2Prescaler = 63; }

    [Category("~PR2 Values")]
    [DisplayName("0")]
    public void CmdPR2000() { Tmr2Prescaler = 0; }


    [Category("Input Voltages")]
    [DisplayName("3.3 V")]
    public void CmdInputVoltage33() { InputVoltage = 3.3m; }

    [Category("Input Voltages")]
    [DisplayName("5 V")]
    public void CmdInputVoltage50() { InputVoltage = 5; }


    private void UpdateValues() {
        if (!PR2Register.IsNull) {
            _PwmResolutionBits.Adjust((decimal)Math.Floor(Math.Log(4 * (double)((decimal)PR2Register + 1)) / Math.Log(2)));
            _PwmResolutionVolts.Adjust(InputVoltage / Math.Pow(2, (double)(decimal)PwmResolutionBits));
        } else {
            PwmResolutionBits.Adjust(null);
            PwmResolutionVolts.Adjust(null);
        }
        _PwmPeriod.Adjust((PR2Register + 1) * 4 * (1 / ClockFrequency) * Tmr2Prescaler);
        _PwmFrequency.Adjust(1 / PwmPeriod);

    }


    #region Elements

    /// <inheritdoc/>
    public override ReadOnlyCollection<string> GetElementNames() {
        return new ReadOnlyCollection<string>([
            nameof(ClockFrequency), nameof(PR2Register), nameof(Tmr2Prescaler),
            nameof(PwmResolutionBits), nameof(PwmFrequency), nameof(PwmPeriod),
            nameof(InputVoltage), nameof(PwmResolutionVolts),
            nameof(CmdFrequency00032), nameof(CmdFrequency04000), nameof(CmdFrequency08000), nameof(CmdFrequency16000), nameof(CmdFrequency32000), nameof(CmdFrequency48000), nameof(CmdFrequency64000),
            nameof(CmdPrescaler1), nameof(CmdPrescaler4),
            nameof(CmdPR2000), nameof(CmdPR2063), nameof(CmdPR2127), nameof(CmdPR2191), nameof(CmdPR2255),
            nameof(CmdInputVoltage33), nameof(CmdInputVoltage50),
        ]);
    }

    #endregion Elements

}
