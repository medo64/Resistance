namespace Tests;

using ResiCalc;

[TestClass]
public sealed class OhmLawTests {

    [TestMethod]
    public void OhmLaw_Basic_VI() {
        var calc = new OhmLaw {
            Voltage = 10,
            Current = 2
        };
        Assert.AreEqual(5, calc.Resistance);
        Assert.AreEqual(20, calc.Power);
    }

    [TestMethod]
    public void OhmLaw_Basic_VR() {
        var calc = new OhmLaw {
            Voltage = 20,
            Resistance = 10
        };
        Assert.AreEqual(2, calc.Current);
        Assert.AreEqual(40, calc.Power);
    }

    [TestMethod]
    public void OhmLaw_Basic_VP() {
        var calc = new OhmLaw {
            Voltage = 20,
            Power = 100
        };
        Assert.AreEqual(5, calc.Current);
        Assert.AreEqual(4, calc.Resistance);
    }

    [TestMethod]
    public void OhmLaw_Basic_IR() {
        var calc = new OhmLaw {
            Current = 5,
            Resistance = 6
        };
        Assert.AreEqual(30, calc.Voltage);
        Assert.AreEqual(150, calc.Power);
    }

    [TestMethod]
    public void OhmLaw_Basic_IP() {
        var calc = new OhmLaw {
            Current = 5,
            Power = 200
        };
        Assert.AreEqual(40, calc.Voltage);
        Assert.AreEqual(8, calc.Resistance);
    }

}
