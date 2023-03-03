// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* RegexFilter.cs -- фильтрация файлов на основе регулярного выражения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.RegularExpressions;

using AM;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace NamerCommon;

/// <summary>
/// Фильтрация файлов на основе регулярного выражения.
/// </summary>
[PublicAPI]
public abstract class RegexFilter
    : FileFilter
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    protected RegexFilter 
        (
            string specification
        )
        : this (new Regex (specification.ThrowIfNullOrEmpty()))
    {
        // пустое тело конструктора
    }
    
    /// <summary>
    /// Конструктор.
    /// </summary>
    protected RegexFilter
        (
            Regex regex
        )
    {
        Sure.NotNull (regex);
        
        _regex = regex;
    }

    #endregion
    
    #region Private members

    private readonly Regex _regex;

    #endregion
    
    #region Protected members

    /// <summary>
    /// Проверка имени файла на соответствие регулярному выражению.
    /// </summary>
    protected bool IsMatch
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        return _regex.IsMatch (fileName);
    }

    #endregion
}
