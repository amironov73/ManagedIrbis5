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

using System.ComponentModel;

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
    [Description ("Имя файла")]
    public string? ArchiveFile { get; set; }

    /// <summary>
    /// Имя эталонного каталога (<c>null</c> означает <c>IBIS</c>).
    /// </summary>
    [Description ("Эталонный каталог")]
    public string? Ethalon { get;set; }

    /// <summary>
    /// Имя оригинального каталога.
    /// </summary>
    [Description ("Оригинальный каталог")]
    public string? Original { get; set; }

    /// <summary>
    /// Имя папки назначения.
    /// например, "IBIS_PREV".
    /// </summary>
    [Description ("Папка назначения")]
    public string? Target { get; set; }

    /// <summary>
    /// Создать соответствующий PAR-файл?
    /// </summary>
    [Description ("Создавать PAR?")]
    public bool CreateParFile { get; set; }

    /// <summary>
    /// Добавить базу данных в меню администратора?
    /// </summary>
    [Description ("Добавлять в меню администратора?")]
    public bool AddToAdminMenu { get; set; }

    /// <summary>
    /// Добавить базу данных в меню библиотекаря?
    /// </summary>
    [Description ("Добавлять в меню библиотекаря?")]
    public bool AddToLibrarianMenu { get; set; }

    /// <summary>
    /// Добавить базу данных в меню читателя?
    /// </summary>
    [Description ("Добавлять в меню читателя?")]
    public bool AddToReaderMenu { get; set; }

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
