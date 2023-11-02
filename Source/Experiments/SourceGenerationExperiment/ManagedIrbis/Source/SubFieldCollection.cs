// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global

using System.Collections.ObjectModel;

namespace ManagedIrbis;

public class SubFieldCollection
    : Collection<SubField>
{
    public Field? Field { get; internal set; }
}
