﻿using System;
using System.IO;
using System.Reflection;

//using ManagedIrbis;
//using ManagedIrbis.Client;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

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
                var assembly = typeof(CommonUnitTest).Assembly;
                var codeBase = assembly.Location;
                var uri = new UriBuilder(codeBase!);
                var asmPath = Uri.UnescapeDataString(uri.Path);
                var result = Path.GetDirectoryName(asmPath);
                Assert.IsNotNull(result, "UnitTestDllPath");

                return result!;
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

        //[NotNull]
        //protected virtual IrbisProvider GetProvider()
        //{
        //    string rootPath = Irbis64RootPath;
        //    LocalProvider result = new LocalProvider(rootPath)
        //    {
        //        Database = "IBIS",
        //        PlatformAbstraction = new TestingPlatformAbstraction()
        //    };

        //    return result;
        //}

        //protected static string GatherCodes
        //    (
        //        [NotNull] RecordField field
        //    )
        //{
        //    char[] codes = field.SubFields.Select(sf => sf.Code)
        //        .OrderBy(c => c)
        //        .ToArray();

        //    return new string(codes);
        //}

        //protected static void CompareFields
        //    (
        //        [NotNull] RecordField expected,
        //        [NotNull] RecordField actual
        //    )
        //{
        //    string expectedCodes = GatherCodes(expected);
        //    string actualCodes = GatherCodes(actual);
        //    Assert.AreEqual(expectedCodes, actualCodes, true);
        //    foreach (char code in expectedCodes)
        //    {
        //        SubField[] expectedSubFields = expected.GetSubField(code);
        //        SubField[] actualSubFields = actual.GetSubField(code);
        //        Assert.AreEqual(expectedSubFields.Length, actualSubFields.Length);
        //        for (int i = 0; i < expectedSubFields.Length; i++)
        //        {
        //            Assert.AreEqual(expectedSubFields[i].Value, actualSubFields[i].Value);
        //        }
        //    }
        //}
    }
}
