// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text.Output;

#endregion

#nullable enable

namespace UnitTests.AM.Text.Output;

[TestClass]
public class TimestampedOutputTest
{
    [TestMethod]
    public void TimestampedOutput_ToString_1()
    {
        var innerOutput = new TextOutput();
        var output = new TimestampedOutput (innerOutput);

        output.Write ("Hello");

        var actual = innerOutput.ToString();
        Assert.IsNotNull (actual);
    }
}
