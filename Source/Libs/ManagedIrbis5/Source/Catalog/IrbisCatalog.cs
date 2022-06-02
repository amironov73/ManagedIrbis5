// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* IrbisCatalog.cs -- общее для каталога
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Catalog;

/// <summary>
/// Common catalog-related stuff.
/// </summary>
public static class IrbisCatalog
{
    #region Constants

    /// <summary>
    /// Мастер-файл.
    /// </summary>
    public const string MasterFileExtension = "mst";

    /// <summary>
    /// Файл ссылок на записи.
    /// </summary>
    public const string CrossReferenceExtension = "xrf";

    /// <summary>
    /// Индексный (инвертированный) файл.
    /// </summary>
    public const string IndexFileExtension = "ifp";

    /// <summary>
    /// Файл узлов.
    /// </summary>
    public const string NodeFileExtension1 = "n01";

    /// <summary>
    /// Файл узлов.
    /// </summary>
    public const string NodeFileExtension2 = "n02";

    /// <summary>
    /// Файл листьев.
    /// </summary>
    public const string LeafFileExtension1 = "l01";

    /// <summary>
    /// Файл листьев.
    /// </summary>
    public const string LeafFileExtension2 = "l02";

    /// <summary>
    /// Файл выбора полей.
    /// </summary>
    public const string FileSelectionTableExtension = "fst";

    /// <summary>
    /// Таблица выбора полей.
    /// </summary>
    public const string FileSelectionInvertedTableExtension = "ifs";

    #endregion

    #region Private members

    private static string[]? _extensions;

    #endregion

    #region Public methods

    /// <summary>
    /// Перечень расширений для файлов базы данных.
    /// </summary>
    public static string[] GetExtensions()
    {
        _extensions ??= new[]
        {
            MasterFileExtension,
            CrossReferenceExtension,
            IndexFileExtension,
            NodeFileExtension1,
            NodeFileExtension2,
            LeafFileExtension1,
            LeafFileExtension2,
            FileSelectionTableExtension,
            FileSelectionInvertedTableExtension
        };

        return _extensions;
    }

    #endregion
}
