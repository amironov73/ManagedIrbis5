// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace ManagedIrbis;

public class SubField
{
    public char Code { get; set; }

    public string? Value { get; set; }

    public override string ToString() => $"^{Code}{Value}";
}
