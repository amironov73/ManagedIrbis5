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

namespace UnitTests.Scripting.Barsik
{
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
            interpreter.Execute ("z = x + y;");
            var actual = (int) (object) variables["z"]!;
            Assert.AreEqual (3, actual);
        }

        [TestMethod]
        [Description ("Блок if-then-else")]
        public void Interpreter_Execute_2()
        {
            var variables = new Dictionary<string, dynamic?>();
            var output = new StringWriter();
            var interpreter = new Interpreter (variables, output);
            variables.Add ("x", 1);
            variables.Add ("y", 2);
            interpreter.Execute (@"if (x < y) { print ""x is less""; } else { print ""y is less""; }");
            var actual = output.ToString();
            Assert.AreEqual ("x is less", actual);
        }

        [TestMethod]
        [Description ("Сравнение на неравенство")]
        public void Interpreter_Execute_3()
        {
            var variables = new Dictionary<string, dynamic?>();
            var output = new StringWriter();
            var interpreter = new Interpreter (variables, output);
            variables.Add ("x", 1);
            variables.Add ("y", 2);
            interpreter.Execute (@"if (x != y) { print ""x is not equal y""; } else { print ""x is equal to y""; }");
            var actual = output.ToString();
            Assert.AreEqual ("x is not equal y", actual);
        }

        [TestMethod]
        [Description ("Директивы")]
        public void Interpreter_Execute_4()
        {
            var interpreter = new Interpreter();
            interpreter.Execute (@"#u Namespace1

#u Namespace2

x = 1;
");
            Assert.AreEqual (2, interpreter.Context.Namespaces.Count);
            var actual = (double) (object) interpreter.Context.Variables["x"]!;
            Assert.AreEqual (1.0, actual);
        }

        [TestMethod]
        [Description ("Комментарии")]
        public void Interpreter_Exectute_5()
        {
            var interpreter = new Interpreter();
            interpreter.Execute (@"// opening comment
x = 1;
// closing comment
");
            var actual = (double) (object) interpreter.Context.Variables["x"]!;
            Assert.AreEqual (1.0, actual);
        }

        [TestMethod]
        [Description ("Комментарии")]
        public void Interpreter_Exectute_6()
        {
            var interpreter = new Interpreter();
            interpreter.Execute ("x /* c1 */ = /* c2 */ 1 /* c3 */;");
            var actual = (double) (object) interpreter.Context.Variables["x"]!;
            Assert.AreEqual (1.0, actual);
        }

        [TestMethod]
        [Description ("Вызов функции")]
        public void Interpreter_Execute_7()
        {
            var interpreter = new Interpreter();
            interpreter.Execute ("l = list();");
            var actual = (List<dynamic?>)interpreter.Context.Variables["l"]!;
            Assert.AreEqual (0, actual.Count);
        }
    }
}
