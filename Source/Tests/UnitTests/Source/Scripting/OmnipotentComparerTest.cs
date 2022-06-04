// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable HeapView.BoxingAllocation
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Scripting;

#nullable enable

namespace UnitTests.Scripting;

[TestClass]
public sealed class OmnipotentComparerTest
{
    [TestMethod]
    [Description ("null всегда равен null")]
    public void OmnipotentComparer_Compare_Null_Null()
    {
        Assert.AreEqual (0, OmnipotentComparer.Default.Compare (null, null));
    }

    [TestMethod]
    [Description ("null всегда меньше не-null")]
    public void OmnipotentComparer_Compare_Null_NonNull()
    {
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (null, string.Empty));
    }

    [TestMethod]
    [Description ("не-null всегда больше null")]
    public void OmnipotentComparer_Compare_NonNull_Null()
    {
        Assert.AreEqual (1, OmnipotentComparer.Default.Compare (string.Empty, null));
    }

    [TestMethod]
    [Description ("bool")]
    public void OmnipotentComparer_Compare_Bool_1()
    {
        Assert.AreEqual (0, OmnipotentComparer.Default.Compare (true, true));
        Assert.AreEqual (0, OmnipotentComparer.Default.Compare (false, false));
        Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, false));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, true));

        Assert.AreEqual (0, OmnipotentComparer.Default.Compare (false, (byte) 0));
        Assert.AreEqual (0, OmnipotentComparer.Default.Compare (true, (byte) 1));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (byte) 1));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (byte) 2));
        Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (byte) 0));

        Assert.AreEqual (0, OmnipotentComparer.Default.Compare (false, (sbyte) 0));
        Assert.AreEqual (0, OmnipotentComparer.Default.Compare (true, (sbyte) 1));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (sbyte) 1));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (sbyte) 2));
        Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (sbyte) 0));
        Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (sbyte) -1));

        Assert.AreEqual (0, OmnipotentComparer.Default.Compare (false, (short) 0));
        Assert.AreEqual (0, OmnipotentComparer.Default.Compare (true, (short) 1));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (short) 1));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (short) 2));
        Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (short) 0));
        Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (short) -1));

        Assert.AreEqual (0, OmnipotentComparer.Default.Compare (false, (ushort) 0));
        Assert.AreEqual (0, OmnipotentComparer.Default.Compare (true, (ushort) 1));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (ushort) 1));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (ushort) 2));
        Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (ushort) 0));

        Assert.AreEqual (0, OmnipotentComparer.Default.Compare (false, 0));
        Assert.AreEqual (0, OmnipotentComparer.Default.Compare (true, 1));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, 1));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, 2));
        Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, 0));
        Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, -1));

        Assert.AreEqual (0, OmnipotentComparer.Default.Compare (false, (uint) 0));
        Assert.AreEqual (0, OmnipotentComparer.Default.Compare (true, (uint) 1));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (uint) 1));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (uint) 2));
        Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (uint) 0));

        Assert.AreEqual (0, OmnipotentComparer.Default.Compare (false, (long) 0));
        Assert.AreEqual (0, OmnipotentComparer.Default.Compare (true, (long) 1));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (long) 1));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (long) 2));
        Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (long) 0));
        Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (long) -1));

        Assert.AreEqual (0, OmnipotentComparer.Default.Compare (false, (ulong) 0));
        Assert.AreEqual (0, OmnipotentComparer.Default.Compare (true, (ulong) 1));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (ulong) 1));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (ulong) 2));
        Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (ulong) 0));

        Assert.AreEqual (0, OmnipotentComparer.Default.Compare (false, (float) 0));
        Assert.AreEqual (0, OmnipotentComparer.Default.Compare (true, (float) 1));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (float) 0.5));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (float) 1));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (float) 2));
        Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (float) 0));
        Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (float) -0.5));
        Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (float) -1));

        Assert.AreEqual (0, OmnipotentComparer.Default.Compare (false, (double) 0));
        Assert.AreEqual (0, OmnipotentComparer.Default.Compare (true, (double) 1));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, 0.5));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (double) 1));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (double) 2));
        Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (double) 0));
        Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true,  -0.5));
        Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (double) -1));

        Assert.AreEqual (0, OmnipotentComparer.Default.Compare (false, (decimal) 0));
        Assert.AreEqual (0, OmnipotentComparer.Default.Compare (true, (decimal) 1));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (decimal) 0.5));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (decimal) 1));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (decimal) 2));
        Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (decimal) 0));
        Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (decimal) -0.5));
        Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (decimal) -1));

        Assert.AreEqual (0, OmnipotentComparer.Default.Compare (false, (nint) 0));
        Assert.AreEqual (0, OmnipotentComparer.Default.Compare (true, (nint) 1));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (nint) 1));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (nint) 2));
        Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (nint) 0));

        Assert.AreEqual (0, OmnipotentComparer.Default.Compare (false, (nuint) 0));
        Assert.AreEqual (0, OmnipotentComparer.Default.Compare (true, (nuint) 1));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (nuint) 1));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (nuint) 2));
        Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (nuint) 0));

        // Assert.AreEqual (0, OmnipotentComparer.Default.Compare (false, "0"));
        // Assert.AreEqual (0, OmnipotentComparer.Default.Compare (true, "1"));
        // Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, "1"));
        // Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, "2"));
        // Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, "2"));
        // Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, "-1"));
    }

    [TestMethod]
    [Description ("byte")]
    public void OmnipotentComparer_Compare_Byte_1()
    {
        Assert.AreEqual (0, OmnipotentComparer.Default.Compare ((byte) 0, (byte) 0));
        Assert.AreEqual (0, OmnipotentComparer.Default.Compare ((byte) 1, (byte) 1));
        Assert.AreEqual (1, OmnipotentComparer.Default.Compare ((byte) 1, (byte) 0));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare ((byte) 0, (byte) 1));

        Assert.AreEqual (0, OmnipotentComparer.Default.Compare ((byte) 0, false));
        Assert.AreEqual (0, OmnipotentComparer.Default.Compare ((byte) 1, true));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare ((byte) 0, true));
        Assert.AreEqual (1, OmnipotentComparer.Default.Compare ((byte) 2, true));

        Assert.AreEqual (0, OmnipotentComparer.Default.Compare ((byte) 0, (sbyte) 0));
        Assert.AreEqual (0, OmnipotentComparer.Default.Compare ((byte) 1, (sbyte) 1));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare ((byte) 0, (sbyte) 1));
        Assert.AreEqual (-1, OmnipotentComparer.Default.Compare ((byte) 0, (sbyte) 2));
        Assert.AreEqual (1, OmnipotentComparer.Default.Compare ((byte) 1, (sbyte) 0));
        Assert.AreEqual (1, OmnipotentComparer.Default.Compare ((byte) 1, (sbyte) -1));

        // Assert.AreEqual (0, OmnipotentComparer.Default.Compare (false, (short) 0));
        // Assert.AreEqual (0, OmnipotentComparer.Default.Compare (true, (short) 1));
        // Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (short) 1));
        // Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (short) 2));
        // Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (short) 0));
        // Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (short) -1));
        //
        // Assert.AreEqual (0, OmnipotentComparer.Default.Compare (false, (ushort) 0));
        // Assert.AreEqual (0, OmnipotentComparer.Default.Compare (true, (ushort) 1));
        // Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (ushort) 1));
        // Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (ushort) 2));
        // Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (ushort) 0));
        //
        // Assert.AreEqual (0, OmnipotentComparer.Default.Compare (false, 0));
        // Assert.AreEqual (0, OmnipotentComparer.Default.Compare (true, 1));
        // Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, 1));
        // Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, 2));
        // Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, 0));
        // Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, -1));
        //
        // Assert.AreEqual (0, OmnipotentComparer.Default.Compare (false, (uint) 0));
        // Assert.AreEqual (0, OmnipotentComparer.Default.Compare (true, (uint) 1));
        // Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (uint) 1));
        // Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (uint) 2));
        // Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (uint) 0));
        //
        // Assert.AreEqual (0, OmnipotentComparer.Default.Compare (false, (long) 0));
        // Assert.AreEqual (0, OmnipotentComparer.Default.Compare (true, (long) 1));
        // Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (long) 1));
        // Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (long) 2));
        // Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (long) 0));
        // Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (long) -1));
        //
        // Assert.AreEqual (0, OmnipotentComparer.Default.Compare (false, (ulong) 0));
        // Assert.AreEqual (0, OmnipotentComparer.Default.Compare (true, (ulong) 1));
        // Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (ulong) 1));
        // Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (ulong) 2));
        // Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (ulong) 0));
        //
        // Assert.AreEqual (0, OmnipotentComparer.Default.Compare (false, (float) 0));
        // Assert.AreEqual (0, OmnipotentComparer.Default.Compare (true, (float) 1));
        // Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (float) 0.5));
        // Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (float) 1));
        // Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (float) 2));
        // Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (float) 0));
        // Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (float) -0.5));
        // Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (float) -1));
        //
        // Assert.AreEqual (0, OmnipotentComparer.Default.Compare (false, (double) 0));
        // Assert.AreEqual (0, OmnipotentComparer.Default.Compare (true, (double) 1));
        // Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, 0.5));
        // Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (double) 1));
        // Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (double) 2));
        // Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (double) 0));
        // Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true,  -0.5));
        // Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (double) -1));
        //
        // Assert.AreEqual (0, OmnipotentComparer.Default.Compare (false, (decimal) 0));
        // Assert.AreEqual (0, OmnipotentComparer.Default.Compare (true, (decimal) 1));
        // Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (decimal) 0.5));
        // Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (decimal) 1));
        // Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (decimal) 2));
        // Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (decimal) 0));
        // Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (decimal) -0.5));
        // Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (decimal) -1));
        //
        // Assert.AreEqual (0, OmnipotentComparer.Default.Compare (false, (nint) 0));
        // Assert.AreEqual (0, OmnipotentComparer.Default.Compare (true, (nint) 1));
        // Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (nint) 1));
        // Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (nint) 2));
        // Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (nint) 0));
        //
        // Assert.AreEqual (0, OmnipotentComparer.Default.Compare (false, (nuint) 0));
        // Assert.AreEqual (0, OmnipotentComparer.Default.Compare (true, (nuint) 1));
        // Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (nuint) 1));
        // Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, (nuint) 2));
        // Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, (nuint) 0));

        // Assert.AreEqual (0, OmnipotentComparer.Default.Compare (false, "0"));
        // Assert.AreEqual (0, OmnipotentComparer.Default.Compare (true, "1"));
        // Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, "1"));
        // Assert.AreEqual (-1, OmnipotentComparer.Default.Compare (false, "2"));
        // Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, "2"));
        // Assert.AreEqual (1, OmnipotentComparer.Default.Compare (true, "-1"));
    }
}
