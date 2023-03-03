// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* FileFilter.cs -- абстрактный фильтр файлов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;

using AM;
using AM.Text;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace NamerCommon;

/// <summary>
/// Абстрактный фильтр файлов.
/// </summary>
public abstract class FileFilter
{
    /// <summary>
    /// Можно ли брать в обработку указанный файл.
    /// </summary>
    public abstract bool CanPass (FileInfo fileInfo);
}
