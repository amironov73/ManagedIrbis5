// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Gbl;
using ManagedIrbis.Providers;

#nullable enable

namespace UnitTests.ManagedIrbis.Gbl;

[TestClass]
public sealed class GblBuilderTest
{
    [TestMethod]
    [Description ("Конструктор")]
    public void GblBuilder_Construction_1()
    {
        var builder = new GblBuilder();
        var statements = builder.ToStatements();
        Assert.IsNotNull (statements);
        Assert.AreEqual (0, statements.Length);
    }

    [TestMethod]
    [Description ("Добавление произвольной команды")]
    public void GblBuilder_AddStatement_1()
    {
        var builder1 = new GblBuilder();
        var builder2 = builder1.AddStatement
            (
                "ARBITRARY",
                "Parameter1",
                "Parameter2",
                "Format1",
                "Format2"
            );
        Assert.AreSame (builder1, builder2);

        var statements = builder1.ToStatements();
        Assert.IsNotNull (statements);
        Assert.AreEqual (1, statements.Length);
        Assert.AreEqual ("ARBITRARY", statements[0].Command);
        Assert.AreEqual ("Parameter1", statements[0].Parameter1);
        Assert.AreEqual ("Parameter2", statements[0].Parameter2);
        Assert.AreEqual ("Format1", statements[0].Format1);
        Assert.AreEqual ("Format2", statements[0].Format2);
    }

    [TestMethod]
    [Description ("Команда ADD: только значение")]
    public void GblBuilder_Add_1()
    {
        var builder = new GblBuilder();
        builder.Add ("910", "^a0^b1");
        var statements = builder.ToStatements();
        Assert.IsNotNull (statements);
        Assert.AreEqual (1, statements.Length);
        Assert.AreEqual ("ADD", statements[0].Command);
        Assert.AreEqual ("910", statements[0].Parameter1);
        Assert.AreEqual ("^a0^b1", statements[0].Format1);
    }

    [TestMethod]
    [Description ("Команда ADD: повторение и значение")]
    public void GblBuilder_Add_2()
    {
        var builder = new GblBuilder();
        builder.Add ("910", "1", "^a0^b1");
        var statements = builder.ToStatements();
        Assert.IsNotNull (statements);
        Assert.AreEqual (1, statements.Length);
        Assert.AreEqual ("ADD", statements[0].Command);
        Assert.AreEqual ("910", statements[0].Parameter1);
        Assert.AreEqual ("1", statements[0].Parameter2);
        Assert.AreEqual ("^a0^b1", statements[0].Format1);
    }

    [TestMethod]
    [Description ("Команда CHA: только значения")]
    public void GblBuilder_Change_1()
    {
        var builder = new GblBuilder();
        builder.Change ("910", "^a0^b1", "^a9^b1");
        var statements = builder.ToStatements();
        Assert.IsNotNull (statements);
        Assert.AreEqual (1, statements.Length);
        Assert.AreEqual ("CHA", statements[0].Command);
        Assert.AreEqual ("910", statements[0].Parameter1);
        Assert.AreEqual ("^a0^b1", statements[0].Format1);
        Assert.AreEqual ("^a9^b1", statements[0].Format2);
    }

    [TestMethod]
    [Description ("Команда CHA: повторение и значения")]
    public void GblBuilder_Change_2()
    {
        var builder = new GblBuilder();
        builder.Change ("910", "1", "^a0^b1", "^a9^b1");
        var statements = builder.ToStatements();
        Assert.IsNotNull (statements);
        Assert.AreEqual (1, statements.Length);
        Assert.AreEqual ("CHA", statements[0].Command);
        Assert.AreEqual ("910", statements[0].Parameter1);
        Assert.AreEqual ("1", statements[0].Parameter2);
        Assert.AreEqual ("^a0^b1", statements[0].Format1);
        Assert.AreEqual ("^a9^b1", statements[0].Format2);
    }

    [TestMethod]
    [Description ("Команда DEL: только повторение")]
    public void GblBuilder_Delete_1()
    {
        var builder = new GblBuilder();
        builder.Delete ("910", "1");
        var statements = builder.ToStatements();
        Assert.IsNotNull (statements);
        Assert.AreEqual (1, statements.Length);
        Assert.AreEqual ("DEL", statements[0].Command);
        Assert.AreEqual ("910", statements[0].Parameter1);
        Assert.AreEqual ("1", statements[0].Parameter2);
    }

    [TestMethod]
    [Description ("Команда DEL: без повторения")]
    public void GblBuilder_Delete_2()
    {
        var builder = new GblBuilder();
        builder.Delete ("910");
        var statements = builder.ToStatements();
        Assert.IsNotNull (statements);
        Assert.AreEqual (1, statements.Length);
        Assert.AreEqual ("DEL", statements[0].Command);
        Assert.AreEqual ("910", statements[0].Parameter1);
        Assert.AreEqual ("*", statements[0].Parameter2);
    }

    [TestMethod]
    [Description ("Команда DELR")]
    public void GblBuilder_DeleteRecord_1()
    {
        var builder = new GblBuilder();
        builder.DeleteRecord();
        var statements = builder.ToStatements();
        Assert.IsNotNull (statements);
        Assert.AreEqual (1, statements.Length);
        Assert.AreEqual ("DELR", statements[0].Command);
    }

    [TestMethod]
    [Description ("Выполнение глобальной корректировки на указанной базе")]
    public void GblBuilder_Execute_1()
    {
        var provider = new NullProvider();
        var builder = new GblBuilder();
        builder.Nop();

        var result = builder.Execute (provider, "IBIS");
        Assert.IsNotNull (result);
        Assert.IsFalse (result.Canceled);
    }

    [TestMethod]
    [Description ("Выполнение глобальной корректировки на текущей базе")]
    public void GblBuilder_Execute_2()
    {
        var provider = new NullProvider();
        var builder = new GblBuilder();
        builder.Nop();

        var result = builder.Execute (provider);
        Assert.IsNotNull (result);
        Assert.IsFalse (result.Canceled);
    }

    [TestMethod]
    [Description ("Выполнение глобальной корректировки на результатах поиска")]
    public void GblBuilder_Execute_3()
    {
        var provider = new NullProvider();
        var builder = new GblBuilder();
        provider.SetConnected (true);
        builder.Nop();

        var result = builder.Execute (provider, "IBIS", "A=NONE");
        Assert.IsNotNull (result);
        Assert.IsFalse (result.Canceled);
    }

    [TestMethod]
    [Description ("Выполнение глобальной корректировки на заданном интервале")]
    public void GblBuilder_Execute_4()
    {
        var provider = new NullProvider();
        var builder = new GblBuilder();
        builder.Nop();

        var result = builder.Execute (provider, "IBIS", 100, 200);
        Assert.IsNotNull (result);
        Assert.IsFalse (result.Canceled);
    }

    [TestMethod]
    [Description ("Выполнение глобальной корректировки на заданном интервале")]
    public void GblBuilder_Execute_5()
    {
        var provider = new NullProvider();
        var builder = new GblBuilder();
        builder.Nop();

        var result = builder.Execute (provider, 100, 200);
        Assert.IsNotNull (result);
        Assert.IsFalse (result.Canceled);
    }

    [TestMethod]
    [Description ("Выполнение глобальной корректировки для заданного набора записей")]
    public void GblBuilder_Execute_6()
    {
        var provider = new NullProvider();
        var builder = new GblBuilder();
        builder.Nop();

        var recordset = new[] { 1, 2, 3 };
        var result = builder.Execute (provider, "IBIS", recordset);
        Assert.IsNotNull (result);
        Assert.IsFalse (result.Canceled);
    }

    [TestMethod]
    [Description ("Выполнение глобальной корректировки для заданного набора записей")]
    public void GblBuilder_Execute_7()
    {
        var provider = new NullProvider();
        var builder = new GblBuilder();
        builder.Nop();

        var recordset = new[] { 1, 2, 3 };
        var result = builder.Execute (provider, recordset);
        Assert.IsNotNull (result);
        Assert.IsFalse (result.Canceled);
    }

    [TestMethod]
    [Description ("Команда IF: пустая")]
    public void GblBuilder_If_1()
    {
        var builder = new GblBuilder();
        builder.If ("condition");
        var statements = builder.ToStatements();
        Assert.IsNotNull (statements);
        Assert.AreEqual (1, statements.Length);
        Assert.AreEqual ("IF", statements[0].Command);
        Assert.AreEqual ("condition", statements[0].Parameter1);
    }

    [TestMethod]
    [Description ("Команда IF: с телом")]
    public void GblBuilder_If_2()
    {
        var body = new[]
        {
            new GblStatement { Command = "ONE" },
            new GblStatement { Command = "TWO" },
            new GblStatement { Command = "THREE" }
        };

        var builder = new GblBuilder();
        builder.If ("condition", body);
        var statements = builder.ToStatements();
        Assert.IsNotNull (statements);
        Assert.AreEqual (5, statements.Length);
        Assert.AreEqual ("IF", statements[0].Command);
        Assert.AreEqual ("condition", statements[0].Parameter1);
        Assert.AreEqual ("ONE", statements[1].Command);
        Assert.AreEqual ("TWO", statements[2].Command);
        Assert.AreEqual ("THREE", statements[3].Command);
        Assert.AreEqual ("FI", statements[4].Command);
    }

    [TestMethod]
    [Description ("Команда IF: с вложенным построителем")]
    public void GblBuilder_If_3()
    {
        var builder = new GblBuilder();
        builder.If
            (
                "condition",
                new GblBuilder()
                    .Add ("100", "value")
                    .Change ("200", "from", "to")
                    .Delete ("300")
            );
        var statements = builder.ToStatements();
        Assert.IsNotNull (statements);
        Assert.AreEqual (5, statements.Length);
        Assert.AreEqual ("IF", statements[0].Command);
        Assert.AreEqual ("condition", statements[0].Parameter1);
        Assert.AreEqual ("ADD", statements[1].Command);
        Assert.AreEqual ("CHA", statements[2].Command);
        Assert.AreEqual ("DEL", statements[3].Command);
        Assert.AreEqual ("FI", statements[4].Command);
    }

    [TestMethod]
    [Description ("Команда-комментарий: пустая")]
    public void GblBuilder_Nop_1()
    {
        var builder = new GblBuilder();
        builder.Nop();
        var statements = builder.ToStatements();
        Assert.IsNotNull (statements);
        Assert.AreEqual (1, statements.Length);
        Assert.AreEqual ("//", statements[0].Command);
    }

    [TestMethod]
    [Description ("Команда-комментарий: однострочный комментарий")]
    public void GblBuilder_Nop_2()
    {
        var builder = new GblBuilder();
        builder.Nop ("one");
        var statements = builder.ToStatements();
        Assert.IsNotNull (statements);
        Assert.AreEqual (1, statements.Length);
        Assert.AreEqual ("//", statements[0].Command);
        Assert.AreEqual ("one", statements[0].Parameter1);
    }

    [TestMethod]
    [Description ("Команда-комментарий: двустрочный комментарий")]
    public void GblBuilder_Nop_3()
    {
        var builder = new GblBuilder();
        builder.Nop ("one", "two");
        var statements = builder.ToStatements();
        Assert.IsNotNull (statements);
        Assert.AreEqual (1, statements.Length);
        Assert.AreEqual ("//", statements[0].Command);
        Assert.AreEqual ("one", statements[0].Parameter1);
        Assert.AreEqual ("two", statements[0].Parameter2);
    }

    [TestMethod]
    [Description ("Команда REP: повторение и значение")]
    public void GblBuilder_Replace_1()
    {
        var builder = new GblBuilder();
        builder.Replace ("900", "repeat", "value");
        var statements = builder.ToStatements();
        Assert.IsNotNull (statements);
        Assert.AreEqual (1, statements.Length);
        Assert.AreEqual ("REP", statements[0].Command);
        Assert.AreEqual ("900", statements[0].Parameter1);
        Assert.AreEqual ("repeat", statements[0].Parameter2);
        Assert.AreEqual ("value", statements[0].Format1);
    }

    [TestMethod]
    [Description ("Команда REP: только значение")]
    public void GblBuilder_Replace_2()
    {
        var builder = new GblBuilder();
        builder.Replace ("900", "value");
        var statements = builder.ToStatements();
        Assert.IsNotNull (statements);
        Assert.AreEqual (1, statements.Length);
        Assert.AreEqual ("REP", statements[0].Command);
        Assert.AreEqual ("900", statements[0].Parameter1);
        Assert.AreEqual ("*", statements[0].Parameter2);
        Assert.AreEqual ("value", statements[0].Format1);
    }

    [TestMethod]
    [Description ("Команда UNDOR")]
    public void GblBuilder_Undo_1()
    {
        var builder = new GblBuilder();
        builder.Undo ("1");
        var statements = builder.ToStatements();
        Assert.IsNotNull (statements);
        Assert.AreEqual (1, statements.Length);
        Assert.AreEqual ("UNDOR", statements[0].Command);
        Assert.AreEqual ("1", statements[0].Parameter1);
    }

    [TestMethod]
    [Description ("Верификация кода команды")]
    public void GblBuilder_VerifyCode_1()
    {
        var builder = new GblBuilder();
        const string expected = "ADD";
        var actual = builder.VerifyCode (expected);
        Assert.AreEqual (expected, actual);
    }

    [TestMethod]
    [Description ("Верификация спецификации поля/подполя")]
    public void GblBuilder_VerifyField_1()
    {
        var builder = new GblBuilder();
        const string expected = "910^a";
        var actual = builder.VerifyField (expected);
        Assert.AreEqual (expected, actual);
    }

    [TestMethod]
    [Description ("Верификация спецификации формата")]
    public void GblBuilder_VerifyFormat_1()
    {
        var builder = new GblBuilder();
        const string expected = "'v910^a'";
        var actual = builder.VerifyFormat (expected);
        Assert.AreEqual (expected, actual);
    }

    [TestMethod]
    [Description ("Верификация параметра для команды")]
    public void GblBuilder_VerifyParameter_1()
    {
        var builder = new GblBuilder();
        const string expected = "1";
        var actual = builder.VerifyParameter (expected);
        Assert.AreEqual (expected, actual);
    }

    [TestMethod]
    [Description ("Верификация спецификации повторения")]
    public void GblBuilder_VerifyRepeat_1()
    {
        var builder = new GblBuilder();
        const string expected = "1";
        var actual = builder.VerifyRepeat (expected);
        Assert.AreEqual (expected, actual);
    }

    [TestMethod]
    [Description ("Верификация значения для команды")]
    public void GblBuilder_VerifyValue_1()
    {
        var builder = new GblBuilder();
        const string expected = "1";
        var actual = builder.VerifyValue (expected);
        Assert.AreEqual (expected, actual);
    }

}
