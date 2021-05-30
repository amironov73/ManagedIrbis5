// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

using ManagedIrbis.Fields;
using ManagedIrbis.Providers;

#nullable enable

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class CommonInfoTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void CommonInfo_ParseRecord_1()
        {
            using var provider = GetProvider();
            var record = provider.ReadRecord(2).ThrowIfNull();

            var array = CommonInfo.ParseRecord(record);
            Assert.IsNotNull(array);
            Assert.AreEqual(1, array.Length);
            var info = array[0];
            Assert.AreEqual("Управление банком", info.Title);
            Assert.IsNull(info.Specific);
            Assert.IsNull(info.General);
            Assert.IsNull(info.Subtitle);
            Assert.AreEqual("З. М. Акулова [и др.]", info.Responsibility);
            Assert.AreEqual("Наука", info.Publisher);
            Assert.AreEqual("М.; СПб.", info.City);
            Assert.AreEqual("1990", info.BeginningYear);
            Assert.IsNull(info.EndingYear);
            Assert.IsNull(info.Isbn);
            Assert.IsNull(info.Issn);
            Assert.IsNull(info.Translation);
            Assert.IsNull(info.FirstAuthor);
            Assert.IsNull(info.Collective);
            Assert.IsNull(info.TitleVariant);
            Assert.IsNull(info.SecondLevelNumber);
            Assert.IsNull(info.SecondLevelTitle);
            Assert.IsNull(info.ThirdLevelNumber);
            Assert.IsNull(info.ThirdLevelTitle);
            Assert.IsNull(info.ParallelTitle);
            Assert.AreEqual("Деньги, кредит, финансирование, серия", info.SeriesTitle);
            Assert.IsNull(info.PreviousTitle);
            Assert.IsNotNull(info.Field461);
            Assert.IsNotNull(info.Field46);
        }
    }
}
