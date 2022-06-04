namespace KittenTests;

[TestClass]
public sealed class ParseStateTest
{
    [TestMethod]
    public void ParseState_Construction_1()
    {
        var state = new ParseState<char>();
        Assert.IsNotNull (state);
    }
}
