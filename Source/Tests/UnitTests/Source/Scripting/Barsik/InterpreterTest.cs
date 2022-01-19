// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable UnusedMember.Local

using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Scripting.Barsik;

#nullable enable

namespace UnitTests.Scripting.Barsik;

[TestClass]
public sealed class InterpreterTest
    : Common.CommonUnitTest
{
    /// <summary>
    /// Специальный тип событий для проверки подписки из скрипта.
    /// </summary>
    private sealed class CanaryEventArgs
        : EventArgs
    {
        public string? AdditionalData { get; }

        public CanaryEventArgs
            (
                string? additionalData
            )
        {
            AdditionalData = additionalData;
        }
    }

    /// <summary>
    /// Подопытный класс, к событиям которого мы будем подписываться
    /// из Barsik-скрипта.
    /// </summary>
    private sealed class Canary
    {
        public event EventHandler? OneChanged;
        public event EventHandler<CanaryEventArgs>? TwoChanged;

        private int _one;
        private string? _two;

        public int One
        {
            get => _one;
            set
            {
                _one = value;
                OneChanged?.Invoke (this, EventArgs.Empty);
            }
        }

        public string? Two
        {
            get => _two;
            set
            {
                _two = value;
                var eventArgs = new CanaryEventArgs (value);
                TwoChanged?.Invoke (this, eventArgs);
            }
        }

        public int Addition (int first, int second)
        {
            return first + second;
        }
    }

    [TestMethod]
    [Description ("Простое сложение двух чисел")]
    public void Interpreter_Execute_1()
    {
        var interpreter = new Interpreter();
        var variables = interpreter.Context.Variables;
        variables.Add ("x", 1);
        variables.Add ("y", 2);
        interpreter.Execute ("z = x + y");
        var actual = (int) (object) variables["z"]!;
        Assert.AreEqual (3, actual);
    }

         [TestMethod]
         [Description ("Блок if-then-else")]
         public void Interpreter_Execute_2()
         {
             var input = TextReader.Null;
             var output = new StringWriter();
             var interpreter = new Interpreter (input, output);
             var variables = interpreter.Context.Variables;
             variables.Add ("x", 1);
             variables.Add ("y", 2);
             interpreter.Execute (@"if (x < y) { print (""x is less""); } else { print (""y is less""); }");
             var actual = output.ToString();
             Assert.AreEqual ("x is less", actual);
         }

         [TestMethod]
         [Description ("Сравнение на неравенство")]
         public void Interpreter_Execute_3()
         {
             var input = TextReader.Null;
             var output = new StringWriter();
             var interpreter = new Interpreter (input, output);
             var variables = interpreter.Context.Variables;
             variables.Add ("x", 1);
             variables.Add ("y", 2);
             interpreter.Execute (@"if (x != y) { print (""x is not equal y""); } else { print (""x is equal to y""); }");
             var actual = output.ToString();
             Assert.AreEqual ("x is not equal y", actual);
         }

    [TestMethod]
    [Description ("Комментарии")]
    public void Interpreter_Execute_5()
    {
        var interpreter = new Interpreter();
        interpreter.Execute (@"// opening comment
x = 1;
// closing comment
");
        var actual = (int) (object) interpreter.Context.Variables["x"]!;
        Assert.AreEqual (1, actual);
    }

    [TestMethod]
    [Description ("Комментарии")]
    public void Interpreter_Execute_6()
    {
        var interpreter = new Interpreter();
        interpreter.Execute ("x /* c1 */ = /* c2 */ 1 /* c3 */;");
        var actual = (int) (object) interpreter.Context.Variables["x"]!;
        Assert.AreEqual (1, actual);
    }

    [TestMethod]
    [Description ("Создание списка")]
    public void Interpreter_Execute_7()
    {
        var interpreter = new Interpreter();
        interpreter.Execute ("l = [];");
        var actual = (List<dynamic?>) interpreter.Context.Variables["l"]!;
        Assert.AreEqual (0, actual.Count);
    }

    [TestMethod]
    [Description ("Скобки")]
    public void Interpreter_Execute_8()
    {
        var interpreter = new Interpreter();
        var variables = interpreter.Context.Variables;
        variables.Add ("x", 1);
        variables.Add ("y", 2);
        variables.Add ("z", 3);
        interpreter.Execute ("r = (x + y) * z;");
        var actual = (int) (object) interpreter.Context.Variables["r"]!;
        Assert.AreEqual (9, actual);
    }

    [TestMethod]
    [Description ("Внешний код")]
    public void Interpreter_External_1()
    {
        void Handler (Context context, ExternalNode node)
        {
            // самый простой нетривиальный обработчик, который я смог придумать
            context.Output.Write (node.Code);
        }

        var input = TextReader.Null;
        var output = new StringWriter();
        var interpreter = new Interpreter (input, output);
        interpreter.ExternalCodeHandler = Handler;
        var sourceCode = "print ('{'); `Hello from inner code` print ('}');";
        interpreter.Execute (sourceCode);
        var actual = output.ToString();
        Assert.AreEqual ("{Hello from inner code}", actual);
    }

    [TestMethod]
    [Description ("Позиционирование стейтментов: каждый стейтмент на своей строке")]
    public void Interpreter_Positioned_1()
    {
        const string sourceCode = @"x = 1;
y = 2;
z = 3;";
       var program = Interpreter.ParseProgram (sourceCode);
       Assert.AreEqual (1, program.Statements[0].StartPosition.Line);
       Assert.AreEqual (2, program.Statements[1].StartPosition.Line);
       Assert.AreEqual (3, program.Statements[2].StartPosition.Line);
    }

    [TestMethod]
    [Description ("Позиционирование стейтментов: несколько стейтментов на одной строке")]
    public void Interpreter_Positioned_2()
    {
        const string sourceCode = @"x = 1; y = 2;
z = 3;";
       var program = Interpreter.ParseProgram (sourceCode);
       Assert.AreEqual (1, program.Statements[0].StartPosition.Line);
       Assert.AreEqual (1, program.Statements[0].StartPosition.Column);
       Assert.AreEqual (1, program.Statements[1].StartPosition.Line);
       Assert.AreEqual (8, program.Statements[1].StartPosition.Column);
       Assert.AreEqual (2, program.Statements[2].StartPosition.Line);
    }

    [TestMethod]
    [Description ("Позиционирование стейтментов: учет комментариев")]
    public void Interpreter_Positioned_3()
    {
        const string sourceCode = @"x = 1;
// комментарий
y = 2;
/* комментарий */
z = 3;";
       var program = Interpreter.ParseProgram (sourceCode);
       Assert.AreEqual (1, program.Statements[0].StartPosition.Line);
       Assert.AreEqual (3, program.Statements[1].StartPosition.Line);
       Assert.AreEqual (5, program.Statements[2].StartPosition.Line);
    }

    [TestMethod]
    [Description ("32=битное целое со знаком")]
    public void Interpreter_Int32_1()
    {
        var interpreter = new Interpreter();
        interpreter.Execute ("x = 123");
        var actual = (int) interpreter.Context.Variables["x"]!;
        Assert.AreEqual (123, actual);
    }

    [TestMethod]
    [Description ("64-битное целое со знаком")]
    public void Interpreter_Int64_1()
    {
        var interpreter = new Interpreter();
        interpreter.Execute ("x = 1L");
        var actual = (long) interpreter.Context.Variables["x"]!;
        Assert.AreEqual (1L, actual);

        interpreter.Execute ("x = 2l");
        actual = (long) interpreter.Context.Variables["x"]!;
        Assert.AreEqual (2L, actual);
    }

    [TestMethod]
    [Description ("32-битное целое без знака")]
    public void Interpreter_UInt32_1()
    {
        var interpreter = new Interpreter();
        interpreter.Execute ("x = 1U");
        var actual = (uint) interpreter.Context.Variables["x"]!;
        Assert.AreEqual (1U, actual);

        interpreter.Execute ("x = 2u");
        actual = (uint) interpreter.Context.Variables["x"]!;
        Assert.AreEqual (2U, actual);
    }

    [TestMethod]
    [Description ("64-битное целое без знака")]
    public void Interpreter_UInt64_1()
    {
        var interpreter = new Interpreter();
        interpreter.Execute ("x = 1UL");
        var actual = (ulong) interpreter.Context.Variables["x"]!;
        Assert.AreEqual (1UL, actual);

        interpreter.Execute ("x = 2ul");
        actual = (ulong) interpreter.Context.Variables["x"]!;
        Assert.AreEqual (2UL, actual);
    }

    [TestMethod]
    [Description ("32-битное шестнадцатеричное целое")]
    public void Interpreter_HexInt32_1()
    {
        var interpreter = new Interpreter();
        interpreter.Execute ("x = 0x123");
        var actual = (int) interpreter.Context.Variables["x"]!;
        Assert.AreEqual (0x123, actual);
    }

    [TestMethod]
    [Description ("64-битное шестнадцатеричное целое")]
    public void Interpreter_HexInt64_1()
    {
        var interpreter = new Interpreter();
        interpreter.Execute ("x = 0x123L");
        var actual = (long) interpreter.Context.Variables["x"]!;
        Assert.AreEqual (0x123L, actual);

        interpreter.Execute ("x = 0x123l");
        actual = (long) interpreter.Context.Variables["x"]!;
        Assert.AreEqual (0x123L, actual);
    }

    [TestMethod]
    [Description ("64-битное шестнадцатеричное целое")]
    public void Interpreter_Float_1()
    {
        var interpreter = new Interpreter();
        interpreter.Execute ("x = 123.4F");
        var actual = (float) interpreter.Context.Variables["x"]!;
        Assert.AreEqual (123.4f, actual);

        interpreter.Execute ("x = 123.4f");
        actual = (float) interpreter.Context.Variables["x"]!;
        Assert.AreEqual (123.4f, actual);
    }

    [TestMethod]
    [Description ("Постфиксный инкремент")]
    public void Interpreter_PostfixIncrement_1()
    {
        var interpreter = new Interpreter();
        var variables = interpreter.Context.Variables;
        variables.Add ("x", 1);
        interpreter.Execute ("y = x++");
        var actualX = (int) (object) variables["x"]!;
        Assert.AreEqual (2, actualX);
        var actualY = (int) (object) variables["y"]!;
        Assert.AreEqual (1, actualY);
    }

    [Ignore]
    [TestMethod]
    [Description ("Префиксный инкремент")]
    public void Interpreter_PrefixIncrement_1()
    {
        var interpreter = new Interpreter();
        var variables = interpreter.Context.Variables;
        variables.Add ("x", 1);
        interpreter.Execute ("y = ++x");
        var actualX = (int) (object) variables["x"]!;
        Assert.AreEqual (2, actualX);
        var actualY = (int) (object) variables["y"]!;
        Assert.AreEqual (2, actualY);
    }

    [TestMethod]
    [Description ("Многократное присваивание")]
    public void Interpreter_MultipleAssignment_1()
    {
        var interpreter = new Interpreter();
        var variables = interpreter.Context.Variables;
        interpreter.Execute ("z = y = 1");
        var actualY = (int) (object) variables["y"]!;
        Assert.AreEqual (1, actualY);
        var actualZ = (int) (object) variables["z"]!;
        Assert.AreEqual (1, actualZ);
    }

    [TestMethod]
    [Description ("Многократное сложное присваивание")]
    public void Interpreter_MultipleAssignment_2()
    {
        var interpreter = new Interpreter();
        var variables = interpreter.Context.Variables;
        variables["y"] = 1;
        variables["z"] = 10;
        interpreter.Execute ("z += y += 2 * (3 + 4)");
        var actualY = (int) (object) variables["y"]!;
        Assert.AreEqual (15, actualY);
        var actualZ = (int) (object) variables["z"]!;
        Assert.AreEqual (25, actualZ);
    }

    [TestMethod]
    [Description ("Вычисление условных выражений")]
    public void Interpreter_BooleanExpression_1()
    {
        var interpreter = new Interpreter();
        var variables = interpreter.Context.Variables;
        variables["a"] = 1;
        variables["b"] = 10;
        interpreter.Execute ("z = a != 0 and b != 0");
        var actualZ = (bool) (object) variables["z"]!;
        Assert.AreEqual (true, actualZ);
    }

    [TestMethod]
    [Description ("Обращение к свойствам объекта")]
    public void Interpreter_Properting_1()
    {
        var interpreter = new Interpreter();
        var variables = interpreter.Context.Variables;
        variables["a"] = "hello";
        interpreter.Execute ("z = a.Length");
        var actualZ = (int) (object) variables["z"]!;
        Assert.AreEqual (5, actualZ);
    }

    [TestMethod]
    [Description ("Обращение к элементу по индексу")]
    public void Interpreter_Indexing_1()
    {
        var interpreter = new Interpreter();
        var variables = interpreter.Context.Variables;
        variables["a"] = new [] { 1, 2, 3, 4, 5 };
        interpreter.Execute ("z = a[2]");
        var actualZ = (int) (object) variables["z"]!;
        Assert.AreEqual (3, actualZ);
    }

    [TestMethod]
    [Description ("Подписка к событию типа EventHandler")]
    public void Interpreter_Subscribe_1()
    {
        var input = TextReader.Null;
        var output = new StringWriter();
        var interpreter = new Interpreter (input, output).WithStdLib();
        var variables = interpreter.Context.Variables;
        var canary = new Canary();
        variables["canary"] = canary;
        const string script = @"lambda = func (sender, ea) { print (sender.One) }
pad = subscribe (canary, ""OneChanged"", lambda)
canary.One = 123
unsubscribe (pad)
canary.One = 321
";
        interpreter.Execute (script);
        Assert.AreEqual ("123", output.ToString());
    }

    [TestMethod]
    [Description ("Подписка к событию типа EventHandler<T>")]
    public void Interpreter_Subscribe_2()
    {
        var input = TextReader.Null;
        var output = new StringWriter();
        var interpreter = new Interpreter (input, output).WithStdLib();
        var variables = interpreter.Context.Variables;
        var canary = new Canary();
        variables["canary"] = canary;
        const string script = @"lambda = func (sender, ea) { print (sender.Two) }
pad = subscribe (canary, ""TwoChanged"", lambda)
canary.Two = ""сто двадцать три""
unsubscribe (pad)
canary.Two = ""триста двадцать один""
";
        interpreter.Execute (script);
        Assert.AreEqual ("сто двадцать три", output.ToString());
    }

    [TestMethod]
    [Description ("Оператор new")]
    public void Interpreter_New_1()
    {
        var interpreter = new Interpreter();
        var variables = interpreter.Context.Variables;
        const string script = "x = new \"System.Text.StringBuilder\" ()";
        interpreter.Execute (script);
        var actual = (object?) variables ["x"];
        Assert.IsNotNull (actual);
    }

    [TestMethod]
    [Description ("Оператор new")]
    public void Interpreter_New_2()
    {
        var interpreter = new Interpreter();
        interpreter.Context.Namespaces.Add ("System.Text", null);
        var variables = interpreter.Context.Variables;
        const string script = "x = new StringBuilder ()";
        interpreter.Execute (script);
        var actual = (object?) variables ["x"];
        Assert.IsNotNull (actual);
    }

    [TestMethod]
    [Description ("Оператор new")]
    public void Interpreter_New_3()
    {
        var interpreter = new Interpreter();
        var variables = interpreter.Context.Variables;
        variables["t"] = "System.Text.StringBuilder";
        const string script = "x = new t ()";
        interpreter.Execute (script);
        var actual = (object?) variables ["x"];
        Assert.IsNotNull (actual);
    }

    [TestMethod]
    [Description ("Оператор new")]
    public void Interpreter_Using_1()
    {
        var interpreter = new Interpreter();
        var variables = interpreter.Context.Variables;
        const string script = "using (x = new \"System.IO.MemoryStream\" ()) {}";
        interpreter.Execute (script);
        // using помещает переменную во временный контекст
        Assert.IsFalse (variables.ContainsKey ("x"));
    }
}
