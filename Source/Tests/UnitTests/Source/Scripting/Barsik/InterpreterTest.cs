// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local

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
        [Description ("Директивы")]
        public void Interpreter_Directive_1()
        {
            var interpreter = new Interpreter();
            interpreter.Execute ("#u Namespace1\nx = 1");
            Assert.AreEqual (1, interpreter.Context.Namespaces.Count);
            var count = interpreter.Context.Namespaces.Count;
            Assert.AreEqual (1, count);
            var actual = (int) (object) interpreter.Context.Variables["x"]!;
            Assert.AreEqual (1, actual);
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
             var interpreter = new Interpreter(input, output)
             {
                 Context =
                 {
                     ExternalCodeHandler = Handler
                 }
             };
             var sourceCode = "print ('{'); <Hello from inner code> print ('}');";
             interpreter.Execute (sourceCode);
             var actual = output.ToString();
             Assert.AreEqual ("{Hello from inner code}", actual);
         }
//
//         [TestMethod]
//         [Description ("Позиционирование стейтментов")]
//         public void Interpreter_Positioned_1()
//         {
//             const string sourceCode = @"x = 1;
// y = 2;
// z = 3;";
//             var program = Interpreter.Parse (sourceCode);
//             Assert.AreEqual (1, program.Statements[0].StartPosition!.Line);
//             Assert.AreEqual (2, program.Statements[1].StartPosition!.Line);
//             Assert.AreEqual (3, program.Statements[2].StartPosition!.Line);
//         }

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
}
