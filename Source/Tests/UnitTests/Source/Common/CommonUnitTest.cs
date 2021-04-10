// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo

using System;
using System.IO;
using System.Linq;

using AM.PlatformAbstraction;

using ManagedIrbis;
using ManagedIrbis.Client;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.Common
{
    public class CommonUnitTest
    {
        /// <summary>
        /// Контекст текста.
        /// </summary>
        public TestContext? TestContext { get; set; }

        /// <summary>
        /// Папка, в которой расположена UnitTests.dll.
        /// </summary>
        public string UnitTestDllPath
        {
            get
            {
                var result = AppContext.BaseDirectory;

                return result;
            }
        }

        /// <summary>
        /// Папка с данными для тестов.
        /// </summary>
        public string TestDataPath
        {
            get
            {
                var result = Path.Combine
                    (
                        UnitTestDllPath,
                        @"../../../../../../TestData"
                    );
                result = Path.GetFullPath(result);

                return result;
            }
        }

        /// <summary>
        /// Корневая папка с тестовыми данными для ИРБИС32.
        /// </summary>
        public string Irbis32RootPath
        {
            get
            {
                var result = Path.Combine
                    (
                        TestDataPath,
                        "Irbis32"
                    );

                return result;
            }
        }

        /// <summary>
        /// Корневая папка с тестовыми данными для ИРБИС64.
        /// </summary>
        public string Irbis64RootPath
        {
            get
            {
                var result = Path.Combine
                    (
                        TestDataPath,
                        "Irbis64"
                    );

                return result;
            }
        }

        protected virtual ISyncIrbisProvider GetProvider()
        {
            string rootPath = Irbis64RootPath;
            LocalProvider result = new LocalProvider(rootPath)
            {
                Database = "IBIS",
                PlatformAbstraction = new TestingPlatformAbstraction()
            };

            return result;
        }

        protected static string GatherCodes
            (
                Field field
            )
        {
            var codes = field.Subfields.Select(sf => sf.Code)
                .OrderBy(c => c)
                .ToArray();

            return new string(codes);
        }

        protected static void CompareFields
            (
                Field expected,
                Field actual
            )
        {
            var expectedCodes = GatherCodes(expected);
            var actualCodes = GatherCodes(actual);
            Assert.AreEqual(expectedCodes, actualCodes, true);
            foreach (char code in expectedCodes)
            {
                var expectedSubFields = expected.EnumerateSubFields(code).ToArray();
                var actualSubFields = actual.EnumerateSubFields(code).ToArray();
                Assert.AreEqual(expectedSubFields.Length, actualSubFields.Length);
                for (var i = 0; i < expectedSubFields.Length; i++)
                {
                    Assert.AreEqual
                        (
                            expectedSubFields[i].Value,
                            actualSubFields[i].Value
                        );
                }
            }
        }
    }
}
