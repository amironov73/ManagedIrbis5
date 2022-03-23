// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* RestoreFromArchiveParameters.cs -- параметры восстановления каталога из архива
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Catalog;

/// <summary>
/// Параметры восстановления каталога из архива.
/// </summary>
public sealed class RestoreFromArchiveParameters
    : IVerifiable
{
    #region Properties

    /// <summary>
    /// Имя файла архива.
    /// </summary>
    public string? ArchiveFile { get; set; }

    /// <summary>
    /// Имя эталонного каталога (<c>null</c> означает <c>IBIS</c>).
    /// </summary>
    public string? Ethalon { get;set; }

    /// <summary>
    /// Имя оригинального каталога.
    /// </summary>
    public string? Original { get; set; }

    /// <summary>
    /// Имя папки назначения.
    /// например, "IBIS_PREV".
    /// </summary>
    public string? Target { get; set; }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<RestoreFromArchiveParameters> (this, throwOnError);

        verifier
            .FileExist (ArchiveFile)
            .NotNullNorEmpty (Original)
            .NotNullNorEmpty (Target);

        return verifier.Result;
    }

    #endregion
}
