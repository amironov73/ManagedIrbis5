// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace PdfSharpCore.Pdf.Advanced;

/// <summary>
/// Provides access to the internal PDF object data structures. This class prevents the public
/// interfaces from pollution with to much internal functions.
/// </summary>
public class PdfObjectInternals
{
    internal PdfObjectInternals (PdfObject obj)
    {
        _obj = obj;
    }

    readonly PdfObject _obj;

    /// <summary>
    /// Gets the object identifier. Returns PdfObjectID.Empty for direct objects.
    /// </summary>
    public PdfObjectID ObjectID => _obj.ObjectID;

    /// <summary>
    /// Gets the object number.
    /// </summary>
    public int ObjectNumber => _obj.ObjectID.ObjectNumber;

    /// <summary>
    /// Gets the generation number.
    /// </summary>
    public int GenerationNumber => _obj.ObjectID.GenerationNumber;

    /// <summary>
    /// Gets the name of the current type.
    /// Not a very useful property, but can be used for data binding.
    /// </summary>
    public string TypeID
    {
        get
        {
            return _obj switch
            {
                PdfArray => "array",
                PdfDictionary => "dictionary",
                _ => _obj.GetType().Name
            };
        }
    }
}
