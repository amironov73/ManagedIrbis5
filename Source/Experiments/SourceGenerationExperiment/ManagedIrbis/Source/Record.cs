// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace ManagedIrbis;

public class Record
{
    public string? Database { get; set; }

    public int Mfn { get; set; }

    public int Version { get; set; }

    public RecordStatus Status { get; set; }

    public MapFieldCollection Fields { get; }

    public Record()
    {
        Fields = new MapFieldCollection { Record = this };
    }
}
