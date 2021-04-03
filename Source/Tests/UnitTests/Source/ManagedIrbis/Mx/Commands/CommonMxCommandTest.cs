// ReSharper disable CheckNamespace
// ReSharper disable EqualExpressionComparison
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Mx;

#nullable enable

namespace UnitTests.ManagedIrbis.Mx.Commands
{
    public class CommonMxCommandTest
        : Common.CommonUnitTest
    {
        protected MxExecutive GetExecutive()
        {
            var result = new MxExecutive();

            return result;
        }
    }
}
