// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System;

using AM;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis;
using ManagedIrbis.Fields;

#nullable enable

namespace UnitTests.ManagedIrbis.Fields;

[TestClass]
public class ArrivalsInfoTest
    : CommonFieldsTest
{
    private Field _GetField()
    {
        return new Field (ArrivalsInfo.Tag)
            .Add ('1', "62")
            .Add ('a', "62")
            .Add ('3', "59");
    }

    private Record _GetRecord()
    {
        return new Record()
            .Add (88, "^A2017/112^Y112^B20171205^CТов. накл. №№ 3, 4, 5, 6, 7 от 17.11.2017 г.^DК8^E64^F82^G61115")
            .Add (920, "KSU")
            .Add (907, "^C^A20171205^Bklipovatv")
            .Add (907, "^CПК^A20171205^Bklipovatv")
            .Add (44, "^IФ404^H05^J62^K82^+61115.00^A79^O62^P82^S82^882^G3")
            .Add (744, "^IФ404^H05^A62^B82^+61115.00^L59^M78^S82^682^O4^J3^43")
            .Add (145, "^A62^B82^C61115.00^G62^H82^I61115.00^J59^K79^P59^Q79")
            .Add (147, "^C3^F3")
            .Add (148, "^B59^C78^D4^G10")
            .Add (149, "^C62^D59^F82^G79^L61115.00^M62^N82^O61115.00^T82^V82^182")
            .Add (151, "^882")
            .Add (45, "^A79^382^462^582^661115.00^.62^(59^G3")
            .Add (47, "^882")
            .Add (18, "^162^259^359^762")
            .Add (17, "^162^A62^359")
            .Add (910, "^261115.00")
            .Add (20, "^A288.00^B62.00^E82.00^F82.00^G62.00")
            .Add (910, "^1И83156")
            .Add (910, "^1Б62490")
            .Add (910, "^1CD4093")
            .Add (910, "И83210")
            .Add (910, "Б62513")
            .Add (910, "CD4095");
    }

    private ArrivalsInfo _GetArrivals()
    {
        return new ()
        {
            OnBalanceWithoutPeriodicals = "62",
            TotalWithoutPeriodicals = "62",
            Educational = "59",
            UnknownSubFields = Array.Empty<SubField>()
        };
    }

    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void ArrivalsInfo_Construciton_1()
    {
        var arrivals = new ArrivalsInfo();
        Assert.IsNull (arrivals.OnBalanceWithoutPeriodicals);
        Assert.IsNull (arrivals.OffBalanceWithoutPeriodicals);
        Assert.IsNull (arrivals.TotalWithoutPeriodicals);
        Assert.IsNull (arrivals.OffBalanceWithPeriodicals);
        Assert.IsNull (arrivals.Educational);
        Assert.IsNull (arrivals.UnknownSubFields);
        Assert.IsNull (arrivals.Field);
        Assert.IsNull (arrivals.UserData);
    }

    [TestMethod]
    [Description ("Разбор поля библиографической записи")]
    public void ArrivalsInfo_ParseField_1()
    {
        var field = _GetField();
        var actual = ArrivalsInfo.ParseField (field);
        var expected = _GetArrivals();
        _Compare (expected, actual);
        Assert.IsNotNull (actual.UnknownSubFields);
        Assert.AreEqual (0, actual.UnknownSubFields?.Length);
        Assert.AreSame (field, actual.Field);
        Assert.IsNull (actual.UserData);
    }

    [TestMethod]
    [Description ("Разбор библиографической записи")]
    public void ArrivalsInfo_ParseRecord_1()
    {
        var record = _GetRecord();
        var arrivals = ArrivalsInfo.ParseRecord (record);
        Assert.AreEqual (1, arrivals.Length);
    }

    [TestMethod]
    [Description ("Преобразование в поле записи")]
    public void ArrivalsInfo_ToField_1()
    {
        var arrivals = _GetArrivals();
        var actual = arrivals.ToField();
        var expected = _GetField();
        CompareFields (expected, actual);
    }

    [TestMethod]
    [Description ("Применение данных к указанному полю записи")]
    public void ArrivalsInfo_ApplyToField_1()
    {
        var arrivals = _GetArrivals();
        var actual = new Field { Tag = ArrivalsInfo.Tag }
            .Add ('1', "100")
            .Add ('a', "200")
            .Add ('3', "90");
        arrivals.ApplyToField (actual);
        var expected = _GetField();
        CompareFields (expected, actual);
    }

    private void _Compare
        (
            ArrivalsInfo first,
            ArrivalsInfo second
        )
    {
        Assert.AreEqual (first.OnBalanceWithoutPeriodicals, second.OnBalanceWithoutPeriodicals.EmptyToNull());
        Assert.AreEqual (first.OffBalanceWithoutPeriodicals, second.OffBalanceWithoutPeriodicals.EmptyToNull());
        Assert.AreEqual (first.TotalWithoutPeriodicals, second.TotalWithoutPeriodicals.EmptyToNull());
        Assert.AreEqual (first.OnBalanceWithoutPeriodicals, second.OnBalanceWithoutPeriodicals.EmptyToNull());
        Assert.AreEqual (first.Educational, second.Educational);

        if (ReferenceEquals (first.UnknownSubFields, null))
        {
            Assert.IsNull (second.UnknownSubFields);
        }
        else
        {
            Assert.IsNotNull (second);
            Assert.IsNotNull (second.UnknownSubFields);
            Assert.AreEqual
                (
                    first.UnknownSubFields.Length,
                    second.UnknownSubFields!.Length
                );
            for (var i = 0; i < first.UnknownSubFields.Length; i++)
            {
                Assert.AreEqual (first.UnknownSubFields[i].Code, second.UnknownSubFields[i].Code);
                Assert.AreEqual (first.UnknownSubFields[i].Value, second.UnknownSubFields[i].Value);
            }
        }
    }

    private void _TestSerialization
        (
            ArrivalsInfo first
        )
    {
        var bytes = first.SaveToMemory();
        var second = bytes.RestoreObjectFromMemory<ArrivalsInfo>();
        Assert.IsNotNull (second);
        _Compare (first, second);
        Assert.IsNull (second.Field);
        Assert.IsNull (second.UserData);
    }

    [TestMethod]
    [Description ("Сериализация")]
    public void ArrivalsInfo_Serialization_1()
    {
        var arrivals = new ArrivalsInfo();
        _TestSerialization (arrivals);

        arrivals = _GetArrivals();
        arrivals.Field = new Field();
        arrivals.UserData = "User data";
        _TestSerialization (arrivals);
    }

    [TestMethod]
    [Description ("Верификация")]
    public void ArrivalsInfo_Verify_1()
    {
        var arrivals = new ArrivalsInfo();
        Assert.IsFalse (arrivals.Verify (false));

        arrivals = _GetArrivals();
        Assert.IsTrue (arrivals.Verify (false));
    }

    [TestMethod]
    [Description ("XML-представление")]
    public void ArrivalsInfo_ToXml_1()
    {
        var arrivals = new ArrivalsInfo();
        Assert.AreEqual
            (
                "<arrivals />",
                XmlUtility.SerializeShort (arrivals)
            );

        arrivals = _GetArrivals();
        Assert.AreEqual
            (
                "<arrivals onBalanceWithoutPeriodicals=\"62\" totalWithoutPeriodicals=\"62\" educational=\"59\" />",
                XmlUtility.SerializeShort (arrivals)
            );
    }

    [TestMethod]
    [Description ("JSON-представление")]
    public void ArrivalsInfo_ToJson_1()
    {
        var arrivals = new ArrivalsInfo();
        Assert.AreEqual
            (
                "{}",
                JsonUtility.SerializeShort (arrivals)
            );

        arrivals = _GetArrivals();
        Assert.AreEqual
            (
                "{\"onBalanceWithoutPeriodicals\":\"62\",\"totalWithoutPeriodicals\":\"62\",\"educational\":\"59\",\"unknown\":[]}",
                JsonUtility.SerializeShort (arrivals)
            );
    }

    [TestMethod]
    [Description ("Плоское текстовое представление")]
    public void ArrivalsInfo_ToString_1()
    {
        var arrivals = new ArrivalsInfo();
        Assert.AreEqual
            (
                "OnBalanceWithoutPeriodicals: (null), OffBalanceWithoutPeriodicals: (null), TotalWithoutPeriodicals: (null), OffBalanceWithPeriodicals: (null), Educational: (null)",
                arrivals.ToString()
            );

        arrivals = _GetArrivals();
        Assert.AreEqual
            (
                "OnBalanceWithoutPeriodicals: 62, OffBalanceWithoutPeriodicals: (null), TotalWithoutPeriodicals: 62, OffBalanceWithPeriodicals: (null), Educational: 59",
                arrivals.ToString()
            );
    }
}
