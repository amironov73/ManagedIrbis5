// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis;
using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Providers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft
{
    [TestClass]
    public class PftFormatterTest
        : Common.CommonUnitTest
    {
        private Record _GetRecord()
        {
            var result = new Record();

            var field = new Field(700)
            {
                {'a', "Иванов"},
                {'b', "И. И."}
            };
            result.Fields.Add(field);

            field = new Field(701)
            {
                {'a', "Петров"},
                {'b', "П. П."}
            };
            result.Fields.Add(field);

            field = new Field(200)
            {
                {'a', "Заглавие"},
                {'e', "подзаголовочное"},
                {'f', "И. И. Иванов, П. П. Петров"}
            };
            result.Fields.Add(field);

            field = new Field(300, "Первое примечание");
            result.Fields.Add(field);
            field = new Field(300, "Второе примечание");
            result.Fields.Add(field);
            field = new Field(300, "Третье примечание");
            result.Fields.Add(field);

            return result;
        }

        private PftProgram _GetProgram()
        {
            return new PftProgram
            {
                Children =
                {
                    new PftUnconditionalLiteral("Title is: "),
                    new PftV("v200^a"),
                    new PftComma(),
                    new PftV("v200^e")
                    {
                        LeftHand =
                        {
                            new PftConditionalLiteral(" : ", false)
                        }
                    },
                    new PftComma(),
                    new PftV("v200^f")
                    {
                        LeftHand =
                        {
                            new PftConditionalLiteral(" / ", false)
                        }
                    }
                }
            };
        }

        [TestMethod]
        public void PftFormatter_Construction_1()
        {
            var formatter = new PftFormatter();
            Assert.IsNotNull(formatter.Context);
            Assert.IsNull(formatter.Program);
            Assert.IsNotNull(formatter.Output);
            Assert.IsNotNull(formatter.Error);
            Assert.IsNotNull(formatter.Warning);
            Assert.IsFalse(formatter.HaveError);
            Assert.IsFalse(formatter.HaveWarning);
            Assert.AreEqual(0L, formatter.Elapsed.Ticks);
        }

        [TestMethod]
        public void PftFormatter_Construction_2()
        {
            var context = new PftContext(null);
            var formatter = new PftFormatter(context);
            Assert.AreSame(context, formatter.Context);
            Assert.IsNull(formatter.Program);
            Assert.IsNotNull(formatter.Output);
            Assert.IsNotNull(formatter.Error);
            Assert.IsNotNull(formatter.Warning);
            Assert.IsFalse(formatter.HaveError);
            Assert.IsFalse(formatter.HaveWarning);
            Assert.AreEqual(0L, formatter.Elapsed.Ticks);
        }

        [TestMethod]
        public void PftFormatter_FormatRecord_1()
        {
            using var provider = GetProvider();
            var context = new PftContext(null);
            context.SetProvider(provider);
            using var formatter = new PftFormatter(context)
            {
                Program = _GetProgram()
            };
            var record = _GetRecord();
            var expected = "Title is: Заглавие : подзаголовочное / И. И. Иванов, П. П. Петров";
            var actual = formatter.FormatRecord(record);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(PftException))]
        public void PftFormatter_FormatRecord_1a()
        {
            using var provider = GetProvider();
            var context = new PftContext(null);
            context.SetProvider(provider);
            using var formatter = new PftFormatter(context);
            var record = _GetRecord();
            formatter.FormatRecord(record);
        }

        [TestMethod]
        public void PftFormatter_FormatRecord_2()
        {
            using var provider = GetProvider();
            var context = new PftContext(null);
            context.SetProvider(provider);
            using var formatter = new PftFormatter(context)
            {
                Program = _GetProgram()
            };
            var expected = "Title is: Куда пойти учиться? : Информ. - реклам. справ / З. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]";
            var actual = formatter.FormatRecord(1);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PftFormatter_FormatRecords_1()
        {
            using var provider = GetProvider();
            var context = new PftContext(null);
            context.SetProvider(provider);
            using var formatter = new PftFormatter(context)
            {
                Program = _GetProgram()
            };
            var expected = "Title is: Куда пойти учиться? : Информ. - реклам. справ / З. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]";
            int[] mfns = { 1 };
            var actual = formatter.FormatRecords(mfns);
            Assert.AreEqual(1, actual.Length);
            Assert.AreEqual(expected, actual[0]);
        }

        [TestMethod]
        public void PftFormatter_ParseProgram_1()
        {
            using var formatter = new PftFormatter();
            var source = "'Title is: ' v200^a, \" : \" v200^e, \" / \" v200^f";
            formatter.ParseProgram(source);
            var expected = _GetProgram();
            var actual = formatter.Program;
            Assert.IsNotNull(actual);
            PftSerializationUtility.VerifyDeserializedProgram(expected, actual!);
        }

        [TestMethod]
        public void PftFormatter_SetProvider_1()
        {
            using var formatter = new PftFormatter();
            var provider = new NullProvider();
            formatter.SetProvider(provider);
            Assert.AreSame(provider, formatter.Context.Provider);
        }
    }
}
