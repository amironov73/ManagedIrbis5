// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using ManagedIrbis;
using ManagedIrbis.Quality;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Quality
{
    class TestRule : QualityRule
    {
        public override string FieldSpec => "999";

        public override RuleReport CheckRecord
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            var fields = GetFields();

            if (fields.Length == 0)
            {
                AddDefect(999, 10, "Отсутствует поле 999");
            }

            if (fields.Length != 0)
            {
                foreach (var field in fields)
                {
                    MustNotContainSubfields(field);
                    MustNotContainWhitespace(field);
                }
            }

            if (fields.Length > 1)
            {
                AddDefect(999, 10, "Много повторений поля 999");
            }

            var result = EndCheck();
            if (fields.Length == 1)
            {
                result.Bonus = 5;
            }

            return result;
        }

        public RuleReport CheckRecord2
            (
                RuleContext context
            )
        {
            BeginCheck(context);

            var fields = GetFields();
            MustBeUniqueField(fields);

            return EndCheck();
        }
    }

    [TestClass]
    public class QualityRuleTest
    {
        [TestMethod]
        public void QualityRule_Construction_1()
        {
            var rule = new TestRule();
            Assert.IsNull(rule.UserData);

            rule.UserData = "User data";
            Assert.AreEqual("User data", rule.UserData);
        }

        [TestMethod]
        public void QualityRule_CheckRecord_1()
        {
            QualityRule rule = new TestRule();
            var record = new Record();
            var context = new RuleContext
            {
                Record = record
            };
            var report = rule.CheckRecord(context);
            Assert.AreEqual(10, report.Damage);
            Assert.AreEqual(1, report.Defects.Count);
            Assert.AreEqual(0, report.Bonus);
        }

        [TestMethod]
        public void QualityRule_CheckRecord_2()
        {
            var rule = new TestRule();
            var record = new Record();
            record.Fields.Add(new Field { Tag = 999, Value = "1000" });
            var context = new RuleContext
            {
                Record = record
            };
            var report = rule.CheckRecord(context);
            Assert.AreEqual(20, report.Damage);
            Assert.AreEqual(1, report.Defects.Count);
            Assert.AreEqual(5, report.Bonus);
        }

        [TestMethod]
        public void QualityRule_CheckRecord_3()
        {
            var rule = new TestRule();
            var record = new Record();
            record.Fields.Add(new Field { Tag = 999, Value = "1000" });
            record.Fields.Add(new Field { Tag = 999, Value = "1001" });
            var context = new RuleContext
            {
                Record = record
            };
            var report = rule.CheckRecord2(context);
            Assert.AreEqual(0, report.Damage);
            Assert.AreEqual(0, report.Defects.Count);
            Assert.AreEqual(0, report.Bonus);
        }

    }
}
