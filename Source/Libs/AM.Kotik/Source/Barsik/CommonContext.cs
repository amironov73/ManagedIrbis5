// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* CommonContext.cs -- общая часть контекста исполнения скрипта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

#endregion

#nullable enable


namespace AM.Kotik.Barsik;

/// <summary>
/// Общая часть контекста исполнения скрипта.
/// </summary>
public sealed class CommonContext
{
    #region Properties

    /// <summary>
    /// Выходной поток, ассоциированный с интерпретатором.
    /// </summary>
    public TextWriter? Output { get; set; }

    /// <summary>
    /// Поток ошибок, ассоциированный с интерпретатором.
    /// </summary>
    public TextWriter? Error { get; set; }

    /// <summary>
    /// Входной поток, ассоциированный с интерпретатором.
    /// </summary>
    public TextReader? Input { get; set; }

    #endregion
}
