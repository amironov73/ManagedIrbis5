// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable StringLiteralTypo

using System;
using System.Linq;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis;
using ManagedIrbis.Magazines;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Magazines;

[TestClass]
public class QuarterlyOrderInfoTest
    : Common.CommonUnitTest
{
    private Field _GetField()
    {
        return new Field
            (
                QuarterlyOrderInfo.Tag,
                "^Q2017/0^Y775.30^Dh^X4^E1=193.82;^N4^A1^B4"
            );
    }

    private QuarterlyOrderInfo _GetOrderInfo()
    {
        return new QuarterlyOrderInfo
        {
            Period = "2017/0",
            NumberOfIssues = "4",
            FirstIssue = "1",
            LastIssue = "4",
            TotalPrice = "775.30",
            IssuePrice = "1=193.82;",
            PeriodicityCode = "h",
            PeriodicityNumber = "4",
            UnknownSubfields = Array.Empty<SubField>()
        };
    }

    private Record _GetRecord()
    {
        var result = new Record();
        result.Fields.Add (new Field (101, "rus"));
        result.Fields.Add (new Field (102, "RU"));
        result.Fields.Add (new Field (110, "^Ta^Ba^Dh^X4^F541^G111^K11^Z16+"));
        result.Fields.Add (new Field (903, "С521"));
        result.Fields.Add (new Field (920, "J"));
        result.Fields.Add (new Field (210, "^AМосква^D1982^9№ "));
        result.Fields.Add (new Field (938, "^Q2004/1^Y86.43^Dh^X4^E43.22^N2^A1^B2"));
        result.Fields.Add (new Field (938, "^Q2004/2^Y87.30^Dh^X4^E43.65^N2^A3^B4"));
        result.Fields.Add (new Field (938, "^Q2006/2^Y108.34^Dh^X4^E1=54.17;^N2^A3^B4"));
        result.Fields.Add (new Field (938, "^Q2006/2^Y108.34^Dh^X4^E1=54.17;^N2^A3^B4"));
        result.Fields.Add (new Field (938, "^Q2007/0^Y222.60^Dh^X4^E1=55.65;^N4^A1^B4"));
        result.Fields.Add (new Field (938, "^Q2008/1^Y104.28^Dh^X4^E1=52.14;^N2^A1^B2"));
        result.Fields.Add (new Field (938, "^Q2008/2^Y159.56^Dh^X4^E1=79.78;^N2^A3^B4"));
        result.Fields.Add (new Field (938, "^Q2009/1^Y204.66^Dh^X4^E1=102.33;^N2^A1^B2"));
        result.Fields.Add (new Field (934, "2017"));
        result.Fields.Add (new Field (938, "^Q2009/2^Y80^Dh^X4^E1=40.00;^N2^A3^B4"));
        result.Fields.Add (new Field (938, "^Q2010/0^Y406.46^Dh^X4^E1=101.61;^N4^A1^B4"));
        result.Fields.Add (new Field (11, "^A0207-7698"));
        result.Fields.Add (new Field (621, "84(0)"));
        result.Fields.Add (new Field (60, "10"));
        result.Fields.Add (new Field (964, "17"));
        result.Fields.Add (new Field (606, "^AДраматургия^BЖурналы"));
        result.Fields.Add (new Field (938, "^Q2011/0^Y596.64^Dh^X4^E1=149.16;^N4^A1^B4"));
        result.Fields.Add (new Field (938, "^Q2012/0^Y613.39^Dh^X4^E1=153.35;^N4^A1^B4"));
        result.Fields.Add (new Field (883, "sdra"));
        result.Fields.Add (new Field (884, "1"));
        result.Fields.Add (new Field (938, "^Q2013/0^Y567.50^Dh^X4^E1=141.88;^N4^A1^B4"));
        result.Fields.Add (new Field (938, "^Q2014/0^Y504.86^Dh^X4^E1=126.22;^N4^A1^B4"));
        result.Fields.Add (new Field (938, "^Q2015/0^Y587.48^Dh^X4^E1=146.87;^N4^A1^B4"));
        result.Fields.Add (new Field (907, "^CКР^A20140306^Bпанюшкинатн"));
        result.Fields.Add (new Field (938, "^Q2016/0^Y732.92^Dh^X4^E1=183.23;^N4^A1^B4"));
        result.Fields.Add (new Field (200,
            "^AСовременная драматургия^Eлитературно-художественный журнал^LСоврем. драматургия^FМ-во культуры РФ, Регион. благотворит. обществ. Фонд развития и поощрения драматургии, Редакция журнала"));
        result.Fields.Add (new Field (938, "^Q2017/0^Y775.30^Dh^X4^E1=193.82;^N4^A1^B4"));
        result.Fields.Add (new Field (907, "^CКТ^A20170803^BГанинаЕА"));
        result.Fields.Add (new Field (907, "^CКТ^A20170804^BГанинаЕА"));
        result.Fields.Add (new Field (907, "^CКТ^A20170915^BМануйловаТС"));
        result.Fields.Add (new Field (907, "^CКР^A20171012^Bвасилевскаяпю"));
        result.Fields.Add (new Field (907, "^CКР^A20171028^Bвасилевскаяпю"));
        result.Fields.Add (new Field (907, "^CКР^A20171101^Bвасилевскаяпю"));
        result.Fields.Add (new Field (907, "^CКР^A20171103^Bвасилевскаяпю"));
        result.Fields.Add (new Field (901, "^Q2004^A0^B1^DОЛИ^KОЛИ - экз.1"));
        result.Fields.Add (new Field (901, "^Q2005^A0^B1^DЧЗП^FМ3^KЧЗП - экз.1"));
        result.Fields.Add (new Field (901, "^Q2006^A0^B1^DАБ^FАИФ^KАБ - экз.1"));
        result.Fields.Add (new Field (901, "^Q2007/0^A0^B1^DАБ^FМ3^KАБ - экз.1"));
        result.Fields.Add (new Field (901, "^Q2008/1^A0^B1^DАБ^FМ3^KАБ - экз.1"));
        result.Fields.Add (new Field (901, "^Q2008/2^A0^B1^DАБ^FУрал-Пресс^KАБ - экз.1"));
        result.Fields.Add (new Field (901, "^Q2009/1^A0^B1^DАБ^FУрал-Пресс^KАБ - экз.1"));
        result.Fields.Add (new Field (901, "^Q2009/2^A0^B1^DАБ^FУрал-Пресс^KАБ - экз.1"));
        result.Fields.Add (new Field (901, "^Q2010/0^A0^B1^DАБ^FУрал-Пресс^KАБ - экз.1"));
        result.Fields.Add (new Field (901, "^Q2011/0^A0^B1^DАБ^FУрал-Пресс^KАБ - экз.1"));
        result.Fields.Add (new Field (901, "^Q2012/0^A0^B1^DАБ^FУрал-Пресс^KАБ - экз.1"));
        result.Fields.Add (new Field (901, "^Q2013/0^A0^B1^DФ304^FУ2^KФ304 - экз.1"));
        result.Fields.Add (new Field (901, "^Q2014/0^A0^B1^DФ304^FУ2^E126.22^KФ304 - экз.1"));
        result.Fields.Add (new Field (901, "^Q2015/0^A0^B1^DФ304^FВ1^E146.87^KФ304 - экз.1"));
        result.Fields.Add (new Field (901, "^Q2016/0^A0^B1^DФ304^FЕ2^E183.23^KФ304 - экз.1"));
        result.Fields.Add (new Field (901, "^Q2017/0^A0^B1^DФ304^FЕ2^E193.82^KФ304 - экз.1"));
        result.Fields.Add (new Field (910, "^A0^B1^FЕ2^DФ304^E193.82^G2017/0"));
        result.Fields.Add (new Field (909, "^Q2017^DФ304^H1-4^k1"));
        result.Fields.Add (new Field (909, "^Q2016^DФ304^H1-4^k1"));
        result.Fields.Add (new Field (909, "^Q2015^DФ304^H1-4^k1"));
        result.Fields.Add (new Field (909, "^Q2014^DФ304^H1-4^k1"));
        result.Fields.Add (new Field (909, "^Q2013^DФ304^H1-4^k1"));
        result.Fields.Add (new Field (909, "^Q2012^DФП^H1-4^k1"));
        result.Fields.Add (new Field (909, "^Q2011^DФП^H1-4^k1"));
        result.Fields.Add (new Field (909, "^Q2010^DФП^H1-4^k1"));
        result.Fields.Add (new Field (909, "^Q2009^DФП^H1-3^k1"));
        result.Fields.Add (new Field (909, "^Q2008^DФП^H1,3,4^k1"));
        result.Fields.Add (new Field (909, "^Q2007^DФП^H1^k1"));
        result.Fields.Add (new Field (909, "^Q2006^DФП^H3,4^k1"));
        result.Fields.Add (new Field (909, "^Q2004^DФП^H1-4^k1"));
        result.Fields.Add (new Field (909, "^Q2003^DФ506^H1-4^k1"));
        result.Fields.Add (new Field (909, "^Q2002^DФ506^H1-4^k1"));
        result.Fields.Add (new Field (909, "^Q2001^DФ506^H2-4^k1"));
        result.Fields.Add (new Field (909, "^Q2001^DФ506^H1^kЖ82580"));
        result.Fields.Add (new Field (909, "^Q2000^DФП^H1-4^k1"));
        result.Fields.Add (new Field (909, "^Q1999^DФП^H1-4^k1"));
        result.Fields.Add (new Field (909, "^Q1998^DФП^H1-4^k1"));
        result.Fields.Add (new Field (909, "^Q1997^DФП^H1-4^k1"));
        result.Fields.Add (new Field (909, "^Q1996^DФП^H1-4^k1"));
        result.Fields.Add (new Field (909, "^Q1995^DФП^H1-4^k1"));
        result.Fields.Add (new Field (909, "^Q1994^DФП^H1-4^k1"));
        result.Fields.Add (new Field (909, "^Q1993^DФП^H1-4^k1"));
        result.Fields.Add (new Field (909, "^Q1992^DФП^H1,2,5,6^k1"));
        result.Fields.Add (new Field (909, "^Q1991^DФП^H1-6^k1"));
        result.Fields.Add (new Field (909, "^Q1990^DФП^H1-6^k1"));
        result.Fields.Add (new Field (909, "^Q1989^DФП^H1,2,4-6^k1"));
        result.Fields.Add (new Field (909, "^Q1988^DФП^H1-4^k1"));
        result.Fields.Add (new Field (909, "^Q1987^DФП^H4^k1456793"));
        result.Fields.Add (new Field (909, "^Q1987^DФП^H4^k1456792"));
        result.Fields.Add (new Field (909, "^Q1987^DФП^H2^k1444367"));
        result.Fields.Add (new Field (909, "^Q1987^DФП^H1^k1429362"));
        result.Fields.Add (new Field (909, "^Q1987^DФП^H1^k1389512"));
        result.Fields.Add (new Field (909, "^Q1986^DФП^H4^k1420507"));
        result.Fields.Add (new Field (909, "^Q1986^DФП^H3^k1411249"));
        result.Fields.Add (new Field (909, "^Q1986^DФП^H3^k1411247"));
        result.Fields.Add (new Field (909, "^Q1986^DФП^H2^k1400082"));
        result.Fields.Add (new Field (909, "^Q1985^DФП^H4^k1389510"));
        result.Fields.Add (new Field (909, "^Q1985^DФП^H2^k1375178"));
        result.Fields.Add (new Field (909, "^Q1985^DФП^H2^k1363675"));
        result.Fields.Add (new Field (909, "^Q1985^DФП^H1^k1354244"));
        result.Fields.Add (new Field (909, "^Q1984^DФП^H4^k1350269"));
        result.Fields.Add (new Field (909, "^Q1984^DФП^H3^k1342200"));
        result.Fields.Add (new Field (909, "^Q1984^DФП^H2^k1337855"));
        result.Fields.Add (new Field (909, "^Q1984^DФП^H1^kЖ59136"));
        result.Fields.Add (new Field (909, "^Q1983^DФП^H4^k1320252"));
        result.Fields.Add (new Field (909, "^Q1983^DФП^H3^k1"));
        result.Fields.Add (new Field (909, "^Q1983^DФП^H2^k1318056"));
        result.Fields.Add (new Field (909, "^Q1983^DФП^H1^k1318053"));
        result.Fields.Add (new Field (905, "^J1^S1^21^D1^I1^F2"));

        return result;
    }

    private void _Compare
        (
            QuarterlyOrderInfo first,
            QuarterlyOrderInfo second
        )
    {
        Assert.AreEqual (first.Period, second.Period);
        Assert.AreEqual (first.NumberOfIssues, second.NumberOfIssues);
        Assert.AreEqual (first.FirstIssue, second.FirstIssue);
        Assert.AreEqual (first.LastIssue, second.LastIssue);
        Assert.AreEqual (first.TotalPrice, second.TotalPrice);
        Assert.AreEqual (first.IssuePrice, second.IssuePrice);

        // TODO: fix
        //Assert.AreEqual(first.Currency, second.Currency);
        Assert.AreEqual (first.PeriodicityCode, second.PeriodicityCode);
        Assert.AreEqual (first.PeriodicityNumber, second.PeriodicityNumber);
        if (ReferenceEquals (first.UnknownSubfields, null))
        {
            Assert.IsNull (second.UnknownSubfields);
        }
        else
        {
            Assert.IsNotNull (second.UnknownSubfields);
            Assert.AreEqual (first.UnknownSubfields.Length, second.UnknownSubfields!.Length);
            for (var i = 0; i < first.UnknownSubfields.Length; i++)
            {
                Assert.AreEqual (first.UnknownSubfields[i].Code, second.UnknownSubfields[i].Code);
                Assert.AreEqual (first.UnknownSubfields[i].Value, second.UnknownSubfields[i].Value);
            }
        }
    }

    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void QuarterlyOrderInfo_Construction_1()
    {
        var order = new QuarterlyOrderInfo();
        Assert.IsNull (order.Period);
        Assert.IsNull (order.NumberOfIssues);
        Assert.IsNull (order.FirstIssue);
        Assert.IsNull (order.LastIssue);
        Assert.IsNull (order.TotalPrice);
        Assert.IsNull (order.IssuePrice);
        Assert.IsNull (order.Currency);
        Assert.IsNull (order.PeriodicityCode);
        Assert.IsNull (order.PeriodicityNumber);
        Assert.IsNull (order.UnknownSubfields);
        Assert.IsNull (order.Field);
        Assert.IsNull (order.UserData);
    }

    [TestMethod]
    [Description ("Разбор указанного поля библиографической записи")]
    public void QuarterlyOrderInfo_ParseField_1()
    {
        var field = _GetField();

        //foreach (SubField subField in field.SubFields)
        //{
        //    TestContext.WriteLine("{0}", subField.ToString());
        //}
        var actual = QuarterlyOrderInfo.ParseField (field);

        //Assert.IsNotNull(actual.UnknownSubfields);
        //TestContext.WriteLine("{0}", actual.UnknownSubfields.Length);
        //foreach (SubField subField in actual.UnknownSubfields)
        //{
        //    TestContext.WriteLine("{0}", subField.ToString());
        //}
        _Compare (_GetOrderInfo(), actual);
    }

    [TestMethod]
    [Description ("Разбор указанной библиографической записи")]
    public void QuarterlyOrderInfo_ParseRecord_1()
    {
        var record = _GetRecord();
        var orders = QuarterlyOrderInfo.ParseRecord (record);
        Assert.AreEqual (17, orders.Length);
        var year2017 = orders.FirstOrDefault (o => o.Period!.Contains ("2017"));
        Assert.IsNotNull (year2017);
    }

    [TestMethod]
    [Description ("Преобразование информации в поле библиографической записи")]
    public void QuarterlyOrderInfo_ToField_1()
    {
        var expected = _GetField();
        var order = _GetOrderInfo();
        var actual = order.ToField();
        foreach (var code in QuarterlyOrderInfo.KnownCodes)
        {
            Assert.AreEqual (expected.GetFirstSubFieldValue (code),
                actual.GetFirstSubFieldValue (code));
        }
    }

    [TestMethod]
    [Description ("Применение информации к указанному полю библиографической записи")]
    public void QuarterlyOrderInfo_ApplyToField_1()
    {
        var expected = _GetField();
        var actual = new Field (QuarterlyOrderInfo.Tag)
            .Add ('a', "555")
            .Add ('b', "666");
        var order = _GetOrderInfo();
        order.ApplyTo (actual);
        foreach (var code in "ab")
        {
            Assert.AreEqual (expected.GetFirstSubFieldValue (code), actual.GetFirstSubFieldValue (code));
        }
    }

    private void _TestSerialization
        (
            QuarterlyOrderInfo first
        )
    {
        var bytes = first.SaveToMemory();
        var second = bytes.RestoreObjectFromMemory<QuarterlyOrderInfo>();
        Assert.IsNotNull (second);
        _Compare (first, second);
        Assert.IsNull (second.Field);
        Assert.IsNull (second.UserData);
    }

    [TestMethod]
    [Description ("Сериализация")]
    public void QuarterlyOrderInfo_Serialization_1()
    {
        var order = new QuarterlyOrderInfo();
        _TestSerialization (order);

        order = _GetOrderInfo();
        order.UserData = "User data";
        order.Field = new Field();
        _TestSerialization (order);
    }

    [TestMethod]
    [Description ("Верификация")]
    public void QuarterlyOrderInfo_Verify_1()
    {
        var order = new QuarterlyOrderInfo();
        Assert.IsFalse (order.Verify (false));

        order = _GetOrderInfo();
        Assert.IsTrue (order.Verify (false));
    }

    [TestMethod]
    [Description ("XML-представление")]
    public void QuarterlyOrderInfo_ToXml_1()
    {
        var order = new QuarterlyOrderInfo();
        Assert.AreEqual ("<quarterly />", XmlUtility.SerializeShort (order));

        order = _GetOrderInfo();
        Assert.AreEqual ("<quarterly period=\"2017/0\" issues=\"4\" first=\"1\" last=\"4\" totalPrice=\"775.30\" issuePrice=\"1=193.82;\" code=\"h\" periodicity=\"4\" />",
            XmlUtility.SerializeShort (order));
    }

    [TestMethod]
    [Description ("JSON-представление")]
    public void QuarterlyOrderInfo_ToJson_1()
    {
        var order = new QuarterlyOrderInfo();
        Assert.AreEqual ("{}", JsonUtility.SerializeShort (order));

        order = _GetOrderInfo();
        Assert.AreEqual ("{\"period\":\"2017/0\",\"issues\":\"4\",\"first\":\"1\",\"last\":\"4\",\"totalPrice\":\"775.30\",\"issuePrice\":\"1=193.82;\",\"code\":\"h\",\"periodicity\":\"4\",\"unknown\":[]}",
            JsonUtility.SerializeShort (order));
    }

    [TestMethod]
    [Description ("Плоское текстовое представление")]
    public void QuarterlyOrderInfo_ToString_1()
    {
        var order = new QuarterlyOrderInfo();
        Assert.AreEqual ("(null)", order.ToString());

        order = _GetOrderInfo();
        Assert.AreEqual ("2017/0", order.ToString());
    }
}
