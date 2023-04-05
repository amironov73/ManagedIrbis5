// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using System;

using ManagedIrbis.Batch;
using ManagedIrbis.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace UnitTests.ManagedIrbis.Batch;

[TestClass]
public sealed class ParallelRecordFormatterTest
{
    [TestMethod]
    [Description ("Конструктор")]
    public void ParallelRecordFormatter_Construction_1()
    {
        const string connectionString = "none";
        var formatter = new ParallelRecordFormatter
            (
                -1,
                connectionString,
                Array.Empty<int>(),
                IrbisFormat.Brief
            );
        Assert.AreEqual (connectionString, formatter.ConnectionString);
        Assert.IsTrue (formatter.Parallelism > 0);
        Assert.AreEqual (IrbisFormat.Brief, formatter.Format);
        // Assert.IsFalse (formatter.IsStop);
    }

}
