// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using ManagedIrbis;
using ManagedIrbis.Gbl.Infrastructure;
using ManagedIrbis.Gbl.Infrastructure.Ast;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Gbl.Infrastructure.Ast;

[TestClass]
public class GblAddTest
    : CommonGblAstTest
{
    [TestMethod]
    public void GblAdd_Construction_1()
    {
        var add = new GblAdd();
        Assert.IsNotNull (add);
    }

    [TestMethod]
    public void GblAdd_Execute_1()
    {
        var context = new GblContext
        {
            CurrentRecord = new Record()
        };
        var add = new GblAdd();
        add.Execute (context);
    }

    [TestMethod]
    public void GblAdd_Verify_1()
    {
        var add = new GblAdd();
        Assert.IsTrue (add.Verify (false));
    }
}
