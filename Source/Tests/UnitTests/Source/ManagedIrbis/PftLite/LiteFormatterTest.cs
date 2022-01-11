// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.Text;

using ManagedIrbis;
using ManagedIrbis.PftLite;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.PftLite;

[TestClass]
public class LiteFormatterTest
{
    private Record _GetRecord()
    {
        return new Record()
        {
            { 700, "^aПервый^bА. А.^gАвтор Авторович^cхимик^f1900-1990" },
            { 701, "^aВторой^bБ. Б.^gБавтор Бавторович^cгеолог^f1910-1991" },
            { 701, "^aТретий^bТ. Т.^gТавтор Тавторович^cастроном^f1911-1992" }
        };
    }

    [TestMethod]
    [Description ("Дамп разобранного скрипта: пустой скрипт")]
    public void LiteFormatter_Dump_1()
    {
        var formatter = new LiteFormatter();
        formatter.SetFormat (string.Empty);
        var actual = formatter.Dump();
        Assert.AreEqual (string.Empty, actual);
    }

    [TestMethod]
    [Description ("Дамп разобранного скрипта: безусловный литерал")]
    public void LiteFormatter_Dump_2()
    {
        var formatter = new LiteFormatter();
        formatter.SetFormat ("'hello'");
        var actual = formatter.Dump().DosToUnix();
        Assert.AreEqual ("unconditional: \"hello\"\n", actual);

        formatter.SetFormat ("'hello' 'world'");
        actual = formatter.Dump().DosToUnix();
        Assert.AreEqual ("unconditional: \"hello\"\nunconditional: \"world\"\n", actual);
    }

    [TestMethod]
    [Description ("Дамп разобранного скрипта: вывод поля")]
    public void LiteFormatter_Dump_3()
    {
        var formatter = new LiteFormatter();
        formatter.SetFormat ("v123");
        var actual = formatter.Dump().DosToUnix();
        Assert.AreEqual ("field: v_123\n", actual);

        formatter.SetFormat ("v123^4");
        actual = formatter.Dump().DosToUnix();
        Assert.AreEqual ("field: v_123_^4\n", actual);

        formatter.SetFormat ("v123^4*5");
        actual = formatter.Dump().DosToUnix();
        Assert.AreEqual ("field: v_123_^4_*5\n", actual);

        formatter.SetFormat ("v123^4*5.6");
        actual = formatter.Dump().DosToUnix();
        Assert.AreEqual ("field: v_123_^4_*5_.6\n", actual);

        formatter.SetFormat ("v123*5");
        actual = formatter.Dump().DosToUnix();
        Assert.AreEqual ("field: v_123_*5\n", actual);

        formatter.SetFormat ("v123.6");
        actual = formatter.Dump().DosToUnix();
        Assert.AreEqual ("field: v_123_.6\n", actual);

        formatter.SetFormat ("v123.6*7");
        actual = formatter.Dump().DosToUnix();
        Assert.AreEqual ("field: v_123_*7_.6\n", actual);

        formatter.SetFormat ("v123 v345");
        actual = formatter.Dump().DosToUnix();
        Assert.AreEqual ("field: v_123\nfield: v_345\n", actual);
    }

    [TestMethod]
    [Description ("Дамп разобранного скрипта: группа")]
    public void LiteFormatter_Dump_4()
    {
        var formatter = new LiteFormatter();
        formatter.SetFormat ("(v1)");
        var actual = formatter.Dump().DosToUnix();
        Assert.AreEqual ("<group<field: v_1>>\n", actual);

        formatter.SetFormat ("(v1) v2");
        actual = formatter.Dump().DosToUnix();
        Assert.AreEqual ("<group<field: v_1>>\nfield: v_2\n", actual);
    }

    [TestMethod]
    [Description ("Дамп разобранного скрипта: условный литерал")]
    public void LiteFormatter_Dump_5()
    {
        var formatter = new LiteFormatter();
        formatter.SetFormat ("\"c1\" v1 \"c2\"");
        var actual = formatter.Dump().DosToUnix();
        Assert.AreEqual ("field: v_1 [left: conditional: \"c1\" True] [right: conditional: \"c2\" False]\n", actual);

        formatter.SetFormat ("\"c1\" v1, \"c2\"");
        actual = formatter.Dump().DosToUnix();
        Assert.AreEqual ("field: v_1 [left: conditional: \"c1\" True]\n", actual);
    }

    [TestMethod]
    [Description ("Дамп разобранного скрипта: повторяющийся литерал")]
    public void LiteFormatter_Dump_6()
    {
        var formatter = new LiteFormatter();
        formatter.SetFormat ("|c1| v1 |c2|");
        var actual = formatter.Dump().DosToUnix();
        Assert.AreEqual ("field: v_1 [left: repeating: \"c1\" False] [right: repeating: \"c2\" False]\n", actual);

        formatter.SetFormat ("|c1| v1, |c2|");
        actual = formatter.Dump().DosToUnix();
        Assert.AreEqual ("field: v_1 [left: repeating: \"c1\" False]\n", actual);
    }

    [Ignore]
    [TestMethod]
    [Description ("Дамп разобранного скрипта: переключение режимов")]
    public void LiteFormatter_Dump_7()
    {
        var formatter = new LiteFormatter();
        formatter.SetFormat ("mdh v1, mpl v2");
        var actual = formatter.Dump().DosToUnix();
        Assert.AreEqual ("field: v_1 [left: repeating: \"c1\" False] [right: repeating: \"c2\" False]\n", actual);
    }

    [TestMethod]
    [Description ("Расформатирование записей: пустой формат")]
    public void LiteFormatter_FormatRecord_1()
    {
        var formatter = new LiteFormatter();
        formatter.SetFormat (string.Empty);
        var actual = formatter.FormatRecord (_GetRecord());
        Assert.AreEqual (string.Empty, actual);
    }

    [TestMethod]
    [Description ("Расформатирование записей: безусловный литерал")]
    public void LiteFormatter_FormatRecord_2()
    {
        var formatter = new LiteFormatter();
        formatter.SetFormat ("'hello'");
        var actual = formatter.FormatRecord (_GetRecord());
        Assert.AreEqual ("hello", actual);
    }

    [TestMethod]
    [Description ("Расформатирование записей: вывод поля")]
    public void LiteFormatter_FormatRecord_3()
    {
        var formatter = new LiteFormatter();
        formatter.SetFormat ("v700^a, ', ', v700^b");
        var actual = formatter.FormatRecord (_GetRecord());
        Assert.AreEqual ("Первый, А. А.", actual);
    }

    [TestMethod]
    [Description ("Расформатирование записей: группа")]
    public void LiteFormatter_FormatRecord_4()
    {
        var formatter = new LiteFormatter();
        formatter.SetFormat ("(v701^a, ', ', v701^b, '_')");
        var actual = formatter.FormatRecord (_GetRecord());
        Assert.AreEqual ("Второй, Б. Б._Третий, Т. Т._", actual);
    }

    [TestMethod]
    [Description ("Расформатирование записей: повторяющийся литерал")]
    public void LiteFormatter_FormatRecord_5()
    {
        var formatter = new LiteFormatter();
        formatter.SetFormat ("v701^a|,|");
        var actual = formatter.FormatRecord (_GetRecord());
        Assert.AreEqual ("Второй,Третий,", actual);

        formatter.SetFormat ("v701^a+|,|");
        actual = formatter.FormatRecord (_GetRecord());
        Assert.AreEqual ("Второй,Третий", actual);

        formatter.SetFormat ("|>|v701^a");
        actual = formatter.FormatRecord (_GetRecord());
        Assert.AreEqual (">Второй>Третий", actual);

        formatter.SetFormat ("|>|+v701^a");
        actual = formatter.FormatRecord (_GetRecord());
        Assert.AreEqual ("Второй>Третий", actual);
    }

    [TestMethod]
    [Description ("Расформатирование записей: условный литерал")]
    public void LiteFormatter_FormatRecord_6()
    {
        var formatter = new LiteFormatter();
        formatter.SetFormat ("\"Авторы:\" v701^a");
        var actual = formatter.FormatRecord (_GetRecord());
        Assert.AreEqual ("Авторы:ВторойТретий", actual);

        formatter.SetFormat ("v701^a\"-авторы\"");
        actual = formatter.FormatRecord (_GetRecord());
        Assert.AreEqual ("ВторойТретий-авторы", actual);

        formatter.SetFormat ("\"Авторы: \" v701^a+|, | \".\"");
        actual = formatter.FormatRecord (_GetRecord());
        Assert.AreEqual ("Авторы: Второй, Третий.", actual);
    }

    [TestMethod]
    [Description ("Расформатирование записей: псевдолитерал")]
    public void LiteFormatter_FormatRecord_7()
    {
        var formatter = new LiteFormatter();
        formatter.SetFormat ("\"Авторы: \"d701");
        var actual = formatter.FormatRecord (_GetRecord());
        Assert.AreEqual ("Авторы: ", actual);

        formatter.SetFormat ("\"Авторы: \"d703");
        actual = formatter.FormatRecord (_GetRecord());
        Assert.AreEqual (string.Empty, actual);

        formatter.SetFormat ("\"Без авторов\"n703");
        actual = formatter.FormatRecord (_GetRecord());
        Assert.AreEqual ("Без авторов", actual);

        formatter.SetFormat ("\"Без авторов\"n701");
        actual = formatter.FormatRecord (_GetRecord());
        Assert.AreEqual (string.Empty, actual);
    }

    [TestMethod]
    [Description ("Расформатирование записей: табуляция в указанную позицию")]
    public void LiteFormatter_FormatRecord_8()
    {
        var formatter = new LiteFormatter();
        formatter.SetFormat ("c4 v700^a");
        var actual = formatter.FormatRecord (_GetRecord());
        Assert.AreEqual ("    Первый", actual);

        formatter.SetFormat ("v700^a, c4 v701^a+|, |");
        actual = formatter.FormatRecord (_GetRecord());
        Assert.AreEqual ("Первый\n    Второй, Третий", actual);
    }

    [TestMethod]
    [Description ("Расформатирование записей: вставка пробелов")]
    public void LiteFormatter_FormatRecord_9()
    {
        var formatter = new LiteFormatter();
        formatter.SetFormat ("x4 v700^a");
        var actual = formatter.FormatRecord (_GetRecord());
        Assert.AreEqual ("    Первый", actual);

        formatter.SetFormat ("x4 v701^a");
        actual = formatter.FormatRecord (_GetRecord());
        Assert.AreEqual ("    ВторойТретий", actual);
    }
}
