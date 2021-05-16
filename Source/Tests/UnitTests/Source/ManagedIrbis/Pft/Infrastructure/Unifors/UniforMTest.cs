// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforMTest
        : CommonUniforTest
    {
        private void Check
            (
                Record record,
                int tag,
                char code,
                int length,
                bool ascending
            )
        {
            var values = record.FMA(tag, code);
            Assert.AreEqual(length, values.Length);
            if (length < 2)
            {
                return;
            }

            var previousValue = values[0].ToString();
            for (var i = 1; i < values.Length; i++)
            {
                var nextValue = values[i].ToString();
                var comparisonResult = string.CompareOrdinal
                    (
                        nextValue,
                        previousValue
                    );
                if (ascending)
                {
                    if (comparisonResult < 0)
                    {
                        Assert.Fail();
                    }
                }
                else
                {
                    if (comparisonResult > 0)
                    {
                        Assert.Fail();
                    }
                }
                previousValue = nextValue;
            }
        }

        [TestMethod]
        public void UniforM_Sort_1()
        {
            Record? record = null;
            Execute(record, 0, "MI1^A", "");
            Execute(record, 0, "MD1^A", "");

            record = new Record();
            Execute(record, 0, "MI1^A", "");
            Check(record, 1, 'A', 0, true);
            Execute(record, 0, "MD1^A", "");
            Check(record, 1, 'A', 0, false);

            record.Fields.Add(new Field(1, "^AZZZ"));
            Execute(record, 0, "MI1^A", "");
            Check(record, 1, 'A', 1, true);
            Execute(record, 0, "MD1^A", "");
            Check(record, 1, 'A', 1, false);

            record.Fields.Add(new Field(1, "^AAAA"));
            Execute(record, 0, "MI1^A", "");
            Check(record, 1, 'A', 2, true);
            Execute(record, 0, "MD1^A", "");
            Check(record, 1, 'A', 2, false);

            record.Fields.Add(new Field(1, "^ACCC"));
            Execute(record, 0, "MI1^A", "");
            Check(record, 1, 'A', 3, true);
            Execute(record, 0, "MD1^A", "");
            Check(record, 1, 'A', 3, false);

            record.Fields.Add(new Field(1, "^AAAA"));
            Execute(record, 0, "MI1^A", "");
            Check(record, 1, 'A', 4, true);
            Execute(record, 0, "MD1^A", "");
            Check(record, 1, 'A', 4, false);
        }

        [TestMethod]
        public void UniforM_Sort_2()
        {
            // Обработка ошибок
            Execute("M", "");
            Execute("MI", "");
            Execute("MQ", "");
            Execute("MI1", "");
            Execute("MI1^", "");
        }
    }
}
