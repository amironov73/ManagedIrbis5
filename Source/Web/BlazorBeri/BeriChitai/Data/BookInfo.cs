// ReSharper disable IdentifierTypo

namespace BeriChitai.Data;

public sealed class BookInfo
{
    public bool Selected { get; set; }

    public int Mfn { get; set; }

    public string? Description { get; set; }

    public string? Cover { get; set; }
}
