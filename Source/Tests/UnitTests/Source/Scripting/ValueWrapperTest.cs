// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Scripting;

#nullable enable

#pragma warning disable CS8602

namespace UnitTests.Scripting;

[TestClass]
public sealed class ValueWrapperTest
{
    [TestMethod]
    [Description ("Чтение целочисленнной переменной")]
    public void ValueWrapper_GetValue_1()
    {
        var valueToWrap = 123;

        var wrapper = new ValueWrapper<int> (() => valueToWrap);
        Assert.AreEqual (valueToWrap, wrapper.GetValue());

        var asInterface = wrapper as IValueWrapper;
        Assert.AreEqual (valueToWrap, asInterface.GetValue());
    }

    [TestMethod]
    [Description ("Чтение строковой переменной")]
    public void ValueWrapper_GetValue_2()
    {
        var valueToWrap = "123";

        var wrapper = new ValueWrapper<string> (() => valueToWrap);
        Assert.AreEqual (valueToWrap, wrapper.GetValue());

        var asInterface = wrapper as IValueWrapper;
        Assert.AreEqual (valueToWrap, asInterface.GetValue());
    }

    [TestMethod]
    [Description ("Чтение значения null")]
    public void ValueWrapper_GetValue_3()
    {
        string? valueToWrap = null;

        var wrapper = new ValueWrapper<string?> (() => valueToWrap);
        Assert.AreEqual (valueToWrap, wrapper.GetValue());

        var asInterface = wrapper as IValueWrapper;
        Assert.AreEqual (valueToWrap, asInterface.GetValue());
    }

    [TestMethod]
    [Description ("Установка значения целочисленнной переменной")]
    public void ValueWrapper_SetValue_1()
    {
        var valueToWrap = 123;
        const int newValue1 = 321;
        const int newValue2 = 231;

        var wrapper = new ValueWrapper<int> (() => valueToWrap,
            (v) => valueToWrap = v);
        wrapper.SetValue (newValue1);
        Assert.AreEqual (newValue1, valueToWrap);
        Assert.AreEqual (newValue1, wrapper.GetValue());

        var asInterface = wrapper as IValueWrapper;
        asInterface.SetValue (newValue2);
        Assert.AreEqual (newValue2, valueToWrap);
        Assert.AreEqual (newValue2, asInterface.GetValue());
    }

    [TestMethod]
    [Description ("Установка значения строковой переменной")]
    public void ValueWrapper_SetValue_2()
    {
        var valueToWrap = "123";
        const string? newValue1 = "321";
        const string? newValue2 = "231";

        var wrapper = new ValueWrapper<string?> (() => valueToWrap,
            (v) => valueToWrap = v);
        wrapper.SetValue (newValue1);
        Assert.AreEqual (newValue1, valueToWrap);
        Assert.AreEqual (newValue1, wrapper.GetValue());

        var asInterface = wrapper as IValueWrapper;
        asInterface.SetValue (newValue2);
        Assert.AreEqual (newValue2, valueToWrap);
        Assert.AreEqual (newValue2, asInterface.GetValue());
    }

    [TestMethod]
    [Description ("Установка значения null")]
    public void ValueWrapper_SetValue_3()
    {
        var valueToWrap = "123";
        const string? newValue = null;

        var wrapper = new ValueWrapper<string?> (() => valueToWrap,
            (v) => valueToWrap = v);
        wrapper.SetValue (newValue);
        Assert.AreEqual (newValue, valueToWrap);
        Assert.AreEqual (newValue, wrapper.GetValue());
    }

    [TestMethod]
    [Description ("Установка значения null")]
    public void ValueWrapper_SetValue_4()
    {
        var valueToWrap = "123";
        const string? newValue = null;

        var wrapper = new ValueWrapper<string?> (() => valueToWrap,
            (v) => valueToWrap = v);
        var asInterface = wrapper as IValueWrapper;
        asInterface.SetValue (newValue);
        Assert.AreEqual (newValue, valueToWrap);
        Assert.AreEqual (newValue, asInterface.GetValue());
    }
}
