// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* NamingContext.cs -- контекст переименования файлов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace NamerCommon;

/// <summary>
/// Контекст переименования файлов.
/// </summary>
[PublicAPI]
public sealed class NamingContext
{
    #region Constants

    private const string IncludeFileName = "include.txt";
    private const string ExcludeFileName = "exclude.txt";

    #endregion

    #region Properties

    /// <summary>
    /// Фильтры.
    /// </summary>
    public List<FileFilter> Filters { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public NamingContext()
    {
        Filters = new();
    }

    #endregion

    #region Public methods

    public void LoadDefaultIncludeExclude()
    {
        var fileName = Path.Combine (AppContext.BaseDirectory, IncludeFileName);
        if (File.Exists (fileName))
        {
            foreach (var line in File.ReadLines (fileName))
            {
                if (!string.IsNullOrWhiteSpace (line))
                {
                    Filters.Add (new IncludeFilter (line.Trim()));
                }
            }
        }

        fileName = Path.Combine (AppContext.BaseDirectory, ExcludeFileName);
        if (File.Exists (fileName))
        {
            foreach (var line in File.ReadLines (fileName))
            {
                if (!string.IsNullOrWhiteSpace (line))
                {
                    Filters.Add (new ExcludeFilter (line.Trim()));
                }
            }
        }
    }

    /// <summary>
    /// Проверка файла на прохождение через фильтры.
    /// </summary>
    public bool CanPass
        (
            FileInfo fileInfo
        )
    {
        Sure.NotNull (fileInfo);

        foreach (var filter in Filters)
        {
            if (!filter.CanPass (fileInfo))
            {
                return false;
            }
        }

        return true;
    }

    #endregion
}
