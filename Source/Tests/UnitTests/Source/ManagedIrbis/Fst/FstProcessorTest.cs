// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using ManagedIrbis;
using ManagedIrbis.Fst;
using ManagedIrbis.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

#nullable enable

namespace UnitTests.ManagedIrbis.Fst
{
    [TestClass]
    public class FstProcessorTest
        : Common.CommonUnitTest
    {
        private new ISyncProvider GetProvider()
        {
            var mock = new Mock<ISyncProvider>();
            var provider = mock.Object;

            return provider;
        }

        private FstFile _GetFile()
        {
            var result = new FstFile
            {
                FileName = "FST file"
            };

            result.Lines.Add(new FstLine
            {
                LineNumber = 1,
                Tag = 201,
                Method = FstIndexMethod.Method0,
                Format = "(v200 /)"
            });

            return result;
        }

        [Ignore]
        [TestMethod]
        public void FstProcessor_Construction_1()
        {
            using var provider = GetProvider();
            var specification = new FileSpecification
            {
                Path = IrbisPath.MasterFile,
                Database = "IBIS",
                FileName = "dumb.fst"
            };
            var processor = new FstProcessor
                (
                    provider,
                    specification
                );
            Assert.AreSame(provider, processor.Provider);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void FstProcessor_Construction_1a()
        {
            using var provider = GetProvider();
            var specification = new FileSpecification
            {
                Path = IrbisPath.MasterFile,
                Database = "IBIS",
                FileName = "nosuchfile.fst"
            };
            var processor = new FstProcessor
                (
                    provider,
                    specification
                );
            Assert.IsNotNull(processor);
        }

        [TestMethod]
        [ExpectedException(typeof(IrbisException))]
        public void FstProcessor_Construction_1b()
        {
            using var provider = GetProvider();
            var specification = new FileSpecification
            {
                Path = IrbisPath.MasterFile,
                Database = "IBIS",
                FileName = "empty.fst"
            };
            var processor = new FstProcessor
                (
                    provider,
                    specification
                );
            Assert.IsNotNull(processor);
        }

        [TestMethod]
        public void FstProcessor_Construction_2()
        {
            using var provider = GetProvider();
            var file = _GetFile();
            var processor = new FstProcessor
                (
                    provider,
                    file
                );
            Assert.AreSame(file, processor.File);
            Assert.AreSame(provider, processor.Provider);
        }

        [TestMethod]
        public void FstProcessor_ExtractTerms_1()
        {
            using var provider = GetProvider();
            var file = _GetFile();
            var processor = new FstProcessor(provider, file);
            var record = new Record();
            var terms = processor.ExtractTerms(record);
            Assert.AreEqual(0, terms.Length);
        }

        [Ignore]
        [TestMethod]
        public void FstProcessor_ExtractTerms_2()
        {
            using var provider = GetProvider();
            var file = _GetFile();
            var processor = new FstProcessor(provider, file);
            var record = provider.ReadRecord(1);
            Assert.IsNotNull(record);
            var terms = processor.ExtractTerms(record!);
            Assert.AreEqual(1, terms.Length);
        }

        [TestMethod]
        public void FstProcessor_TransformRecord_1()
        {
            using var provider = GetProvider();
            var file = _GetFile();
            var processor = new FstProcessor(provider, file);
            var source = new Record();
            var target = processor.TransformRecord(source, file);
            Assert.AreEqual(0, target.Fields.Count);
        }

        [Ignore]
        [TestMethod]
        public void FstProcessor_TransformRecord_1a()
        {
            using var provider = GetProvider();
            var file = _GetFile();
            var processor = new FstProcessor(provider, file);
            var source = provider.ReadRecord(1);
            Assert.IsNotNull(source);
            var target = processor.TransformRecord(source!, file);
            Assert.AreEqual(1, target.Fields.Count);
        }

        [TestMethod]
        public void FstProcessor_TransformRecord_2()
        {
            using var provider = GetProvider();
            var file = _GetFile();
            var format = "mpl,'201',/,(v200 /),'\a'";
            var processor = new FstProcessor(provider, file);
            var source = new Record();
            var target = processor.TransformRecord(source, format);
            Assert.AreEqual(0, target.Fields.Count);
        }

        [Ignore]
        [TestMethod]
        public void FstProcessor_TransformRecord_2a()
        {
            using var provider = GetProvider();
            var file = _GetFile();
            var format = "mpl,'201',/,(v200 /),'\a'";
            var processor = new FstProcessor(provider, file);
            var source = provider.ReadRecord(1);
            Assert.IsNotNull(source);
            var target = processor.TransformRecord(source!, format);
            Assert.AreEqual(1, target.Fields.Count);
        }
    }
}
