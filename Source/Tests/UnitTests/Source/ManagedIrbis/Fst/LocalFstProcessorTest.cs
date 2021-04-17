// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System.IO;

using ManagedIrbis;
using ManagedIrbis.Fst;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Fst
{
    [TestClass]
    public class LocalFstProcessorTest
        : Common.CommonUnitTest
    {
        [Ignore]
        [TestMethod]
        public void LocalFstProcessor_Construction_1()
        {
            var rootPath = Irbis64RootPath;
            using var processor = new LocalFstProcessor(rootPath, "IBIS");
            Assert.IsNotNull(processor.Provider);
        }

        [Ignore]
        [TestMethod]
        public void LocalFstProcessor_ReadFile_1()
        {
            var rootPath = Irbis64RootPath;
            var fileName = Path.Combine
                (
                    rootPath,
                    "Datai\\IBIS\\dumb.fst"
                );
            using var processor = new LocalFstProcessor(rootPath, "IBIS");
            var fst = processor.ReadFile(fileName);
            Assert.IsNotNull(fst);
            Assert.AreEqual("dumb.fst", fst!.FileName);
        }

        [Ignore]
        [TestMethod]
        public void LocalFstProcessor_ReadFile_2()
        {
            var rootPath = Irbis64RootPath;
            var fileName = Path.Combine
                (
                    rootPath,
                    "Datai\\IBIS\\empty.fst"
                );
            using var processor = new LocalFstProcessor(rootPath, "IBIS");
            var fst = processor.ReadFile(fileName);
            Assert.IsNull(fst);
        }

        [Ignore]
        [TestMethod]
        public void LocalFstProcessor_ReadFile_2a()
        {
            var rootPath = Irbis64RootPath;
            var fileName = Path.Combine
                (
                    rootPath,
                    "Datai\\IBIS\\empty2.fst"
                );
            using (var processor
                = new LocalFstProcessor(rootPath, "IBIS"))
            {
                var fst = processor.ReadFile(fileName);
                Assert.IsNull(fst);
            }
        }

        [Ignore]
        [TestMethod]
        public void LocalFstProcessor_TransformRecord_1()
        {
            var rootPath = Irbis64RootPath;
            var fileName = Path.Combine
                (
                    rootPath,
                    "Datai\\IBIS\\dumb.fst"
                );
            using var processor = new LocalFstProcessor(rootPath, "IBIS");
            var fst = processor.ReadFile(fileName);
            Assert.IsNotNull(fst);
            var source = new Record();
            var target = processor.TransformRecord(source, fst!);
            Assert.AreEqual(0, target.Fields.Count);
        }

        [Ignore]
        [TestMethod]
        public void LocalFstProcessor_TransformRecord_2()
        {
            var rootPath = Irbis64RootPath;
            var fileName = Path.Combine
                (
                    rootPath,
                    "Datai\\IBIS\\dumb.fst"
                );
            using var processor = new LocalFstProcessor(rootPath, "IBIS");
            var fst = processor.ReadFile(fileName);
            Assert.IsNotNull(fst);
            var source = processor.Provider.ReadRecord(1);
            Assert.IsNotNull(source);
            var target = processor.TransformRecord(source!, fst!);
            Assert.AreEqual(1, target.Fields.Count);
        }
    }
}
