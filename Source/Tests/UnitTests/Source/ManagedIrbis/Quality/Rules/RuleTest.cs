// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using ManagedIrbis;
using ManagedIrbis.Quality;

using Moq;

#nullable enable

namespace UnitTests.ManagedIrbis.Quality.Rules;

public class RuleTest
    : Common.CommonUnitTest
{
    protected RuleContext GetContext()
    {
        var mock = new Mock<ISyncProvider>();
        mock.SetupGet (m => m.IsConnected).Returns (true);
        var connection = mock.Object;
        var record = new Record();
        var result = new RuleContext
        {
            Provider = connection,
            BriefFormat = "@brief",
            Record = record
        };

        return result;
    }
}
