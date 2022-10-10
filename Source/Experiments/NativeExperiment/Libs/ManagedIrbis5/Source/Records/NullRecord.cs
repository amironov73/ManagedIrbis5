// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMethodReturnValue.Global

/* NullRecord.cs -- пустая запись
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Direct;
using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Records;

/// <summary>
/// Пустая запись (например, для тестирования).
/// </summary>
public sealed class NullRecord
    : IRecord
{
    #region Properties

    /// <inheritdoc cref="IRecord.Database"/>
    public string? Database { get; set; }

    /// <inheritdoc cref="IRecord.Mfn"/>
    public int Mfn { get; set; }

    /// <inheritdoc cref="IRecord.Version"/>
    public int Version { get; set; }

    /// <inheritdoc cref="IRecord.Status"/>
    public RecordStatus Status { get; set; }

    #endregion

    #region IRecord members

    /// <inheritdoc cref="IRecord.Decode(Response)"/>
    public void Decode (Response _)
    {
        // пустое тело метода
    }

    /// <inheritdoc cref="IRecord.Decode(MstRecord64)"/>
    public void Decode (MstRecord64 _)
    {
        // пустое тело метода
    }

    /// <inheritdoc cref="IRecord.Encode(string)"/>
    public string Encode (string? delimiter = IrbisText.IrbisDelimiter) => string.Empty;

    /// <inheritdoc cref="IRecord.Encode(MstRecord64)"/>
    public void Encode (MstRecord64 _)
    {
        // пустое тело метода
    }

    /// <inheritdoc cref="IRecord.FM(int)"/>
    public string? FM (int _) => null;

    /// <inheritdoc cref="IRecord.FM(int,char)"/>
    public string? FM (int tag, char code) => null;

    #endregion
}
