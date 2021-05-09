// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.IO;

using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class PftVariableManagerTest
    {
        private PftVariableManager _GetSingleManager()
        {
            var result = new PftVariableManager(null);
            result.SetVariable("firstVar", "firstValue");
            result.SetVariable("secondVar", 3.14);

            return result;
        }

        private PftVariableManager _GetChildManager()
        {
            var parent = new PftVariableManager(null);
            parent.SetVariable("thirdVar", "thirdValue");
            parent.SetVariable("fourthVar", 2.78);
            var result = new PftVariableManager(parent);
            result.SetVariable("firstVar", "firstValue");
            result.SetVariable("secondVar", 3.14);

            return result;
        }

        [TestMethod]
        public void PftVariableManager_Construction_1()
        {
            var manager = new PftVariableManager(null);
            Assert.IsNull(manager.Parent);
            Assert.IsNotNull(manager.Registry);
            Assert.AreEqual(0, manager.Registry.Count);
        }

        [TestMethod]
        public void PftVariableManager_DumpVariables_1()
        {
            var manager = new PftVariableManager(null);
            var writer = new StringWriter();
            manager.DumpVariables(writer);
            var expected = "============================================================\n";
            var actual = writer.ToString().DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftVariableManager_DumpVariables_2()
        {
            var manager = _GetSingleManager();
            var writer = new StringWriter();
            manager.DumpVariables(writer);
            var expected = "firstVar: \"firstValue\"\nsecondVar: 3.14\n" +
                           "============================================================\n";
            var actual = writer.ToString().DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftVariableManager_DumpVariables_3()
        {
            var manager = _GetChildManager();
            var writer = new StringWriter();
            manager.DumpVariables(writer);
            var expected = "firstVar: \"firstValue\"\nsecondVar: 3.14\n" +
                           "============================================================\n" +
                           "fourthVar: 2.78\nthirdVar: \"thirdValue\"\n" +
                           "============================================================\n";
            var actual = writer.ToString().DosToUnix();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftVariableManager_GetAllVariables_1()
        {
            var manager = new PftVariableManager(null);
            var variables = manager.GetAllVariables();
            Assert.AreEqual(0, variables.Length);
        }

        [TestMethod]
        public void PftVariableManager_GetAllVariables_2()
        {
            var manager = _GetSingleManager();
            var variables = manager.GetAllVariables();
            Assert.AreEqual(2, variables.Length);
        }

        [TestMethod]
        public void PftVariableManager_GetAllVariables_3()
        {
            var manager = _GetChildManager();
            var variables = manager.GetAllVariables();
            Assert.AreEqual(4, variables.Length);
        }

        [TestMethod]
        public void PftVariableManager_GetExistingVariable_1()
        {
            var manager = new PftVariableManager(null);
            var variable = manager.GetExistingVariable("firstVar");
            Assert.IsNull(variable);
        }

        [TestMethod]
        public void PftVariableManager_GetExistingVariable_2()
        {
            var manager = _GetSingleManager();
            var variable = manager.GetExistingVariable("firstVar");
            Assert.IsNotNull(variable);
            Assert.AreEqual("firstVar", variable!.Name);

            variable = manager.GetExistingVariable("noSuchVariable");
            Assert.IsNull(variable);
        }

        [TestMethod]
        public void PftVariableManager_GetExistingVariable_3()
        {
            var manager = _GetChildManager();
            var variable = manager.GetExistingVariable("firstVar");
            Assert.IsNotNull(variable);
            Assert.AreEqual("firstVar", variable!.Name);

            variable = manager.GetExistingVariable("thirdVar");
            Assert.IsNotNull(variable);
            Assert.AreEqual("thirdVar", variable!.Name);

            variable = manager.GetExistingVariable("noSuchVariable");
            Assert.IsNull(variable);
        }

        [TestMethod]
        public void PftVariableManager_OrCreateVariable_1()
        {
            var manager = new PftVariableManager(null);
            var variable = manager.GetOrCreateVariable("noSuchVariable", false);
            Assert.IsNotNull(variable);
            Assert.AreEqual("noSuchVariable", variable.Name);
        }

        [TestMethod]
        public void PftVariableManager_GetOrCreateVariable_2()
        {
            var manager = _GetSingleManager();
            var variable = manager.GetOrCreateVariable("firstVar", false);
            Assert.IsNotNull(variable);
            Assert.AreEqual("firstVar", variable.Name);

            variable = manager.GetOrCreateVariable("noSuchVariable", false);
            Assert.IsNotNull(variable);
            Assert.AreEqual("noSuchVariable", variable.Name);
        }

        [TestMethod]
        public void PftVariableManager_GetCreateVariable_3()
        {
            var manager = _GetChildManager();
            var variable = manager.GetOrCreateVariable("firstVar", false);
            Assert.IsNotNull(variable);
            Assert.AreEqual("firstVar", variable.Name);

            variable = manager.GetOrCreateVariable("thirdVar", false);
            Assert.IsNotNull(variable);
            Assert.AreEqual("thirdVar", variable.Name);

            variable = manager.GetOrCreateVariable("noSuchVariable", false);
            Assert.IsNotNull(variable);
            Assert.AreEqual("noSuchVariable", variable.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void PftVariableManager_GetOrCreateVariable_4()
        {
            var manager = _GetSingleManager();
            manager.GetOrCreateVariable("firstVar", true);
        }

        [TestMethod]
        public void PftVariable_SetVariable_1()
        {
            var name = "firstVar";
            var value = "secondValue";
            var manager = new PftVariableManager(null);
            var variable = manager.SetVariable(name, value);
            Assert.IsNotNull(variable);
            Assert.IsFalse(variable.IsNumeric);
            Assert.AreEqual(name, variable.Name);
            Assert.AreEqual(value, variable.StringValue);
        }

        [TestMethod]
        public void PftVariable_SetVariable_2()
        {
            var name = "firstVar";
            var value = "secondValue";
            var manager = _GetSingleManager();
            var variable = manager.SetVariable(name, value);
            Assert.IsNotNull(variable);
            Assert.IsFalse(variable.IsNumeric);
            Assert.AreEqual(name, variable.Name);
            Assert.AreEqual(value, variable.StringValue);

            name = "noSuchVar";
            variable = manager.SetVariable(name, value);
            Assert.IsNotNull(variable);
            Assert.IsFalse(variable.IsNumeric);
            Assert.AreEqual(name, variable.Name);
            Assert.AreEqual(value, variable.StringValue);
        }

        [TestMethod]
        public void PftVariable_SetVariable_3()
        {
            var name = "firstVar";
            var value = "secondValue";
            var manager = _GetChildManager();
            var variable = manager.SetVariable(name, value);
            Assert.IsNotNull(variable);
            Assert.IsFalse(variable.IsNumeric);
            Assert.AreEqual(name, variable.Name);
            Assert.AreEqual(value, variable.StringValue);

            name = "thirdVar";
            variable = manager.SetVariable(name, value);
            Assert.IsNotNull(variable);
            Assert.IsFalse(variable.IsNumeric);
            Assert.AreEqual(name, variable.Name);
            Assert.AreEqual(value, variable.StringValue);

            name = "noSuchVar";
            variable = manager.SetVariable(name, value);
            Assert.IsNotNull(variable);
            Assert.IsFalse(variable.IsNumeric);
            Assert.AreEqual(name, variable.Name);
            Assert.AreEqual(value, variable.StringValue);
        }

        [TestMethod]
        public void PftVariable_SetVariable_4()
        {
            var name = "noSuchVar";
            var value = 123.45;
            var manager = new PftVariableManager(null);
            var variable = manager.SetVariable(name, value);
            Assert.IsNotNull(variable);
            Assert.IsTrue(variable.IsNumeric);
            Assert.AreEqual(name, variable.Name);
            Assert.AreEqual(value, variable.NumericValue);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void PftVariable_SetVariable_5()
        {
            var name = "firstVar";
            var value = 123.45;
            var manager = _GetSingleManager();
            manager.SetVariable(name, value);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void PftVariable_SetVariable_6()
        {
            var name = "thirdVar";
            var value = 123.45;
            var manager = _GetChildManager();
            manager.SetVariable(name, value);
        }

        [TestMethod]
        public void PftVariable_SetVariable_7()
        {
            var name = "firstVar";
            var context = new PftContext(null);
            var manager = new PftVariableManager(null);
            var index = new IndexSpecification
            {
                Kind = IndexKind.Literal,
                Literal = 1
            };
            manager.SetVariable(context, name, index, "line1");
            index.Literal = 2;
            manager.SetVariable(context, name, index, "line2");
            index.Literal = 3;
            manager.SetVariable(context, name, index, "line3");
            var variable = manager.GetExistingVariable(name);
            Assert.IsNotNull(variable);
            var expected = "line1\nline2\nline3";
            var actual = variable!.StringValue.DosToUnix();
            Assert.AreEqual(expected, actual);
        }
    }
}
