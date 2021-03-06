﻿// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Gbl.Infrastructure;
using ManagedIrbis.Gbl.Infrastructure.Ast;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Gbl.Infrastructure.Ast
{
    [TestClass]
    public class GblDelTest
        : CommonGblAstTest
    {
        [TestMethod]
        public void GblDel_Construction_1()
        {
            GblDel del = new GblDel();
            Assert.IsNotNull(del);
        }

        [TestMethod]
        public void GblDel_Execute_1()
        {
            GblContext context = new GblContext();
            GblDel del = new GblDel();
            del.Execute(context);
        }

        [TestMethod]
        public void GblDel_Verify_1()
        {
            GblDel del = new GblDel();
            Assert.IsTrue(del.Verify(false));
        }
    }
}
