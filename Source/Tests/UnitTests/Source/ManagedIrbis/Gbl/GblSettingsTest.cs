// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System.Linq;
using AM.Runtime;

using ManagedIrbis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Gbl;

using Moq;

#nullable enable

namespace UnitTests.ManagedIrbis.Gbl
{
    [TestClass]
    public class GblSettingsTest
    {
        [TestMethod]
        public void GblSettingsTest_Construction_1()
        {
            var settings = new GblSettings();
            Assert.AreEqual(true, settings.Actualize);
            Assert.AreEqual(false, settings.Autoin);
            Assert.AreEqual(null, settings.Database);
            Assert.AreEqual(null, settings.FileName);
            Assert.AreEqual(1, settings.FirstRecord);
            Assert.AreEqual(false, settings.FormalControl);
            Assert.AreEqual(0, settings.MaxMfn);
            Assert.AreEqual(null, settings.MfnList);
            Assert.AreEqual(0, settings.MinMfn);
            Assert.AreEqual(0, settings.NumberOfRecords);
            Assert.AreEqual(null, settings.SearchExpression);
            Assert.IsNotNull(settings.Statements);
        }

        [TestMethod]
        public void GblSettingsTest_Construction_2()
        {
            var mock = new Mock<ISyncConnection>();
            var connection = mock.Object;
            var settings = new GblSettings(connection);
            Assert.AreEqual(true, settings.Actualize);
            Assert.AreEqual(false, settings.Autoin);
            Assert.AreEqual(connection.Database, settings.Database);
            Assert.AreEqual(null, settings.FileName);
            Assert.AreEqual(1, settings.FirstRecord);
            Assert.AreEqual(false, settings.FormalControl);
            Assert.AreEqual(0, settings.MaxMfn);
            Assert.AreEqual(null, settings.MfnList);
            Assert.AreEqual(0, settings.MinMfn);
            Assert.AreEqual(0, settings.NumberOfRecords);
            Assert.AreEqual(null, settings.SearchExpression);
            Assert.IsNotNull(settings.Statements);
        }

        [TestMethod]
        public void GblSettingsTest_Construction_3()
        {
            var mock = new Mock<ISyncConnection>();
            var connection = mock.Object;
            GblStatement[] statements =
            {
                new (),
                new (),
                new ()
            };
            var settings = new GblSettings
                (
                    connection,
                    statements
                );
            Assert.AreEqual(true, settings.Actualize);
            Assert.AreEqual(false, settings.Autoin);
            Assert.AreEqual(connection.Database, settings.Database);
            Assert.AreEqual(null, settings.FileName);
            Assert.AreEqual(1, settings.FirstRecord);
            Assert.AreEqual(false, settings.FormalControl);
            Assert.AreEqual(0, settings.MaxMfn);
            Assert.AreEqual(null, settings.MfnList);
            Assert.AreEqual(0, settings.MinMfn);
            Assert.AreEqual(0, settings.NumberOfRecords);
            Assert.AreEqual(null, settings.SearchExpression);
            Assert.IsNotNull(settings.Statements);
            Assert.AreEqual(statements.Length, settings.Statements.Count);
        }

        [TestMethod]
        public void GblSettings_ForInterval_1()
        {
            const int minMfn = 100, maxMfn = 200;
            var mock = new Mock<ISyncConnection>();
            var connection = mock.Object;
            GblStatement[] statements =
            {
                new (),
                new (),
                new ()
            };
            var settings = GblSettings.ForInterval
                (
                    connection,
                    minMfn,
                    maxMfn,
                    statements
                );
            Assert.AreEqual(minMfn, settings.MinMfn);
            Assert.AreEqual(maxMfn, settings.MaxMfn);
            Assert.AreEqual(null, settings.MfnList);
        }

        [TestMethod]
        public void GblSettings_ForInterval_2()
        {
            const int minMfn = 100, maxMfn = 200;
            const string database = "ISTU";
            var mock = new Mock<ISyncConnection>();
            var connection = mock.Object;
            GblStatement[] statements =
            {
                new (),
                new (),
                new ()
            };
            var settings = GblSettings.ForInterval
                (
                    connection,
                    database,
                    minMfn,
                    maxMfn,
                    statements
                );
            Assert.AreEqual(database, settings.Database);
            Assert.AreEqual(minMfn, settings.MinMfn);
            Assert.AreEqual(maxMfn, settings.MaxMfn);
            Assert.AreEqual(null, settings.MfnList);
        }

        [TestMethod]
        public void GblSettings_ForList_1()
        {
            var mock = new Mock<ISyncConnection>();
            var connection = mock.Object;
            var mfnList = Enumerable.Range(100, 100).ToArray();
            GblStatement[] statements =
            {
                new (),
                new (),
                new ()
            };
            var settings = GblSettings.ForList
                (
                    connection,
                    mfnList,
                    statements
                );
            Assert.AreEqual(0, settings.MinMfn);
            Assert.AreEqual(0, settings.MaxMfn);
            Assert.IsNotNull(settings.MfnList);
            Assert.AreEqual(mfnList.Length, settings.MfnList!.Length);
        }

        [TestMethod]
        public void GblSettings_ForList_2()
        {
            var mock = new Mock<ISyncConnection>();
            var connection = mock.Object;
            var mfnList = Enumerable.Range(100, 100).ToArray();
            const string database = "ISTU";
            GblStatement[] statements =
            {
                new (),
                new (),
                new ()
            };
            var settings = GblSettings.ForList
                (
                    connection,
                    database,
                    mfnList,
                    statements
                );
            Assert.AreEqual(database, settings.Database);
            Assert.AreEqual(0, settings.MinMfn);
            Assert.AreEqual(0, settings.MaxMfn);
            Assert.IsNotNull(settings.MfnList);
            Assert.AreEqual(mfnList.Length, settings.MfnList!.Length);
        }

        [TestMethod]
        public void GblSettings_ForSearchExpression_1()
        {
            const string searchExpression = "A=AUTHOR$";
            var mock = new Mock<ISyncConnection>();
            var connection = mock.Object;
            GblStatement[] statements =
            {
                new (),
                new (),
                new ()
            };
            var settings = GblSettings.ForSearchExpression
                (
                    connection,
                    searchExpression,
                    statements
                );
            Assert.AreEqual(0, settings.MinMfn);
            Assert.AreEqual(0, settings.MaxMfn);
            Assert.AreEqual(null, settings.MfnList);
            Assert.AreEqual(searchExpression, settings.SearchExpression);
        }

        [TestMethod]
        public void GblSettings_ForSearchExpression_2()
        {
            const string searchExpression = "A=AUTHOR$";
            const string database = "ISTU";
            var mock = new Mock<ISyncConnection>();
            var connection = mock.Object;
            GblStatement[] statements =
            {
                new (),
                new (),
                new ()
            };
            var settings = GblSettings.ForSearchExpression
                (
                    connection,
                    database,
                    searchExpression,
                    statements
                );
            Assert.AreEqual(searchExpression, settings.SearchExpression);
            Assert.AreEqual(0, settings.MinMfn);
            Assert.AreEqual(0, settings.MaxMfn);
            Assert.AreEqual(null, settings.MfnList);
            Assert.AreEqual(searchExpression, settings.SearchExpression);
        }

        [TestMethod]
        public void GblSettings_SetFileName_1()
        {
            const string fileName = "fileName";
            var settings = new GblSettings();
            settings.SetFileName(fileName);
            Assert.AreEqual(fileName, settings.FileName);
        }

        [TestMethod]
        public void GblSettings_SetRange_1()
        {
            const int firstRecord = 100;
            const int numberOfRecords = 123;
            var settings = new GblSettings();
            settings.SetRange(firstRecord, numberOfRecords);
            Assert.AreEqual(firstRecord, settings.FirstRecord);
            Assert.AreEqual(numberOfRecords, settings.NumberOfRecords);
        }

        [TestMethod]
        public void GblSettings_SetSearchExpression_1()
        {
            const string searchExpression = "A=AUTHOR$";
            var settings = new GblSettings();
            settings.SetSearchExpression(searchExpression);
            Assert.AreEqual(searchExpression, settings.SearchExpression);
        }

        private void _TestSerialize
            (
                GblSettings first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<GblSettings>();

            Assert.IsNotNull(second);
            Assert.AreEqual(first.Actualize, second!.Actualize);
            Assert.AreEqual(first.Autoin, second.Autoin);
            Assert.AreEqual(first.Database, second.Database);
            Assert.AreEqual(first.FileName, second.FileName);
            Assert.AreEqual(first.FirstRecord, second.FirstRecord);
            Assert.AreEqual(first.FormalControl, second.FormalControl);
            Assert.AreEqual(first.MaxMfn, second.MaxMfn);
            Assert.AreEqual(first.MinMfn, second.MinMfn);
            Assert.AreEqual(first.NumberOfRecords, second.NumberOfRecords);
            Assert.AreEqual(first.SearchExpression, second.SearchExpression);
            Assert.AreEqual(first.Statements.Count, second.Statements.Count);
        }

        [TestMethod]
        public void GblSettings_Serialize_1()
        {
            var mock = new Mock<ISyncConnection>();
            var connection = mock.Object;
            var settings = new GblSettings();
            _TestSerialize(settings);

            GblStatement[] statements =
            {
                new (),
                new (),
                new ()
            };
            settings = new GblSettings
                (
                    connection,
                    statements
                );
            settings
                .SetSearchExpression("A=AUTHOR$")
                .SetRange(100, 200)
                .SetFileName("hello.gbl");
            _TestSerialize(settings);
        }

        [TestMethod]
        public void GblSettings_Verify_1()
        {
            var mock = new Mock<ISyncConnection>();
            var connection = mock.Object;
            var settings = new GblSettings();
            Assert.AreEqual(false, settings.Verify(false));

            GblStatement[] statements =
            {
                new (),
                new (),
                new ()
            };
            settings = new GblSettings
                (
                    connection,
                    statements
                );
            settings.Database = "IBIS";
            settings
                .SetSearchExpression("A=AUTHOR$")
                .SetRange(100, 200)
                .SetFileName("hello.gbl");
            Assert.AreEqual(true, settings.Verify(false));
        }
    }
}
