// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo

using System.Collections.ObjectModel;

namespace ManagedIrbis;

public class MapFieldCollection
    : Collection<Field>
{
    public Record? Record { get; internal set; }
}
