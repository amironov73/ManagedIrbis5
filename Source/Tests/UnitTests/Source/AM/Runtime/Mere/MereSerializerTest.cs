// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local

#region Using directives

using System;
using System.Collections;
using System.IO;

using AM;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime.Mere;

#endregion

#pragma warning disable CS0659 // GetHashCode not overriden

#nullable enable

namespace UnitTests.AM.Runtime.Mere;

[TestClass]
public sealed class MereSerializerTest
{
    private sealed class Person
        : IMereSerializable
    {
        public string? Name { get; set; }
        public int Age { get; set; }

        public void MereSerialize (BinaryWriter writer)
        {
            MereSerializer.Serialize (writer, Name);
            MereSerializer.Serialize (writer, Age);
        }

        public void MereDeserialize (BinaryReader reader)
        {
            Name = (string?) MereSerializer.Deserialize (reader);
            Age = (int) MereSerializer.Deserialize (reader)!;
        }

        public override bool Equals (object? other)
        {
            var second = (Person) other!;

            return Name == second.Name && Age == second.Age;
        }
    }

    [TestMethod]
    [Description ("null")]
    public void MereSerializer_Null_1()
    {
        var memory = new MemoryStream();
        var writer = new BinaryWriter (memory);
        MereSerializer.Serialize (writer, null);
        var bytes = memory.ToArray();
        Assert.AreEqual (1, bytes.Length);
        Assert.AreEqual ((byte) MereTypeCode.Null, bytes[0]);

        memory = new MemoryStream (bytes);
        var reader = new BinaryReader (memory);
        var actual = MereSerializer.Deserialize (reader);
        Assert.IsNull (actual);
    }

    private void _Test<T> (T sourceValue, int expectedLength, MereTypeCode typeCode)
    {
        var memory = new MemoryStream();
        var writer = new BinaryWriter (memory);
        MereSerializer.Serialize (writer, sourceValue);
        writer.Flush();
        var bytes = memory.ToArray();
        Assert.AreEqual (expectedLength, bytes.Length);
        Assert.AreEqual ((byte) typeCode, bytes[0]);

        memory = new MemoryStream (bytes);
        var reader = new BinaryReader (memory);
        var actual = (T) MereSerializer.Deserialize (reader).ThrowIfNull ();
        if (sourceValue is ICollection sourceCollection)
        {
            Assert.IsNotNull (actual);
            CollectionAssert.AreEqual (sourceCollection, (ICollection) actual);
        }
        else
        {
            Assert.AreEqual (sourceValue, actual);
        }
    }

    [TestMethod]
    [Description ("bool")]
    public void MereSerializer_Bool_1()
    {
        _Test (true, 2, MereTypeCode.Boolean);
        _Test (false, 2, MereTypeCode.Boolean);
    }

    [TestMethod]
    [Description ("byte")]
    public void MereSerializer_Byte_1()
    {
        _Test ((byte) 0, 2, MereTypeCode.Byte);
        _Test ((byte) 1, 2, MereTypeCode.Byte);
        _Test ((byte) 123, 2, MereTypeCode.Byte);
    }

    [TestMethod]
    [Description ("sbyte")]
    public void MereSerializer_SByte_1()
    {
        _Test ((sbyte) 0, 2, MereTypeCode.SByte);
        _Test ((sbyte) 1, 2, MereTypeCode.SByte);
        _Test ((sbyte) -1, 2, MereTypeCode.SByte);
    }

    [TestMethod]
    [Description ("char")]
    public void MereSerializer_Char_1()
    {
        _Test ('a', 2, MereTypeCode.Char);
        _Test ('\u0410',  3, MereTypeCode.Char);
    }

    [TestMethod]
    [Description ("short")]
    public void MereSerializer_Short_1()
    {
        _Test ((short) 0, 3, MereTypeCode.Int16);
        _Test ((short) 123, 3, MereTypeCode.Int16);
        _Test ((short) -123, 3, MereTypeCode.Int16);
    }

    [TestMethod]
    [Description ("ushort")]
    public void MereSerializer_Ushort_1()
    {
        _Test ((ushort) 0, 3, MereTypeCode.UInt16);
        _Test ((ushort) 123, 3, MereTypeCode.UInt16);
    }

    [TestMethod]
    [Description ("int")]
    public void MereSerializer_Int_1()
    {
        _Test (0, 5, MereTypeCode.Int32);
        _Test (123, 5, MereTypeCode.Int32);
        _Test (-123, 5, MereTypeCode.Int32);
    }

    [TestMethod]
    [Description ("uint")]
    public void MereSerializer_Uint_1()
    {
        _Test ((uint) 0, 5, MereTypeCode.UInt32);
        _Test ((uint) 123, 5, MereTypeCode.UInt32);
    }

    [TestMethod]
    [Description ("long")]
    public void MereSerializer_Long_1()
    {
        _Test (0L, 9, MereTypeCode.Int64);
        _Test (123L, 9, MereTypeCode.Int64);
        _Test (-123L, 9, MereTypeCode.Int64);
    }

    [TestMethod]
    [Description ("ulong")]
    public void MereSerializer_Ulong_1()
    {
        _Test (0UL, 9, MereTypeCode.UInt64);
        _Test (123UL, 9, MereTypeCode.UInt64);
    }

    [TestMethod]
    [Description ("single")]
    public void MereSerializer_Single_1()
    {
        _Test (0.0f, 5, MereTypeCode.Single);
        _Test (123.4f, 5, MereTypeCode.Single);
        _Test (-123.4f, 5, MereTypeCode.Single);
    }

    [TestMethod]
    [Description ("double")]
    public void MereSerializer_Double_1()
    {
        _Test (0.0, 9, MereTypeCode.Double);
        _Test (123.4, 9, MereTypeCode.Double);
        _Test (-123.4, 9, MereTypeCode.Double);
    }

    [TestMethod]
    [Description ("decimal")]
    public void MereSerializer_Decimal_1()
    {
        _Test (0.0m, 17, MereTypeCode.Decimal);
        _Test (123.4m, 17, MereTypeCode.Decimal);
        _Test (-123.4m, 17, MereTypeCode.Decimal);
    }

    [TestMethod]
    [Description ("DateTime")]
    public void MereSerializer_DateTime_1()
    {
        _Test (new DateTime (2021, 12, 9), 9, MereTypeCode.DateTime);
    }

    [TestMethod]
    [Description ("Array")]
    public void MereSerializer_Array_1()
    {
        _Test (Array.Empty<object>(), 5, MereTypeCode.Array);
        _Test (new object[] { 1 }, 10, MereTypeCode.Array);
        _Test (new object[] { 1, 2 }, 15, MereTypeCode.Array);
        _Test (new object[] { 1, 2, 3 }, 20, MereTypeCode.Array);
    }

    [TestMethod]
    [Description ("List")]
    public void MereSerializer_List_1()
    {
        _Test (new ArrayList(), 125, MereTypeCode.List);
        _Test (new ArrayList { 1 }, 130, MereTypeCode.List);
        _Test (new ArrayList { 1, 2 }, 135, MereTypeCode.List);
        _Test (new ArrayList { 1, 2, 3 }, 140, MereTypeCode.List);
    }

    [TestMethod]
    [Description ("Dictionary")]
    public void MereSerializer_Dictionary_1()
    {
        _Test (new Hashtable(), 125, MereTypeCode.Dictionary);
        _Test (new Hashtable { {1, "one"} }, 135, MereTypeCode.Dictionary);
        _Test (new Hashtable { {1, "one"}, {2, "two"} }, 145, MereTypeCode.Dictionary);
        _Test (new Hashtable { {1, "one"}, {2, "two"}, {3, "three"} }, 157, MereTypeCode.Dictionary);
    }

    [Ignore]
    [TestMethod]
    [Description ("Object")]
    public void MereSerializer_Object_1()
    {
        _Test (new Person(), 140, MereTypeCode.Object);
        _Test (new Person() { Name = "Alexey", Age = 48 }, 147, MereTypeCode.Object);
    }

    [TestMethod]
    [Description ("Unknown type")]
    [ExpectedException (typeof (ArsMagnaException))]
    public void MereSerializer_UnknownType_1()
    {
        _Test (new Version (1, 2, 3, 4), 0, MereTypeCode.Object);
    }
}
