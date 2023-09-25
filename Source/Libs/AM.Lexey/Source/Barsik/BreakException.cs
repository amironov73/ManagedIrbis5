// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* BreakException.cs -- исключение, используемое для досрочного завершения цикла
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Lexey.Barsik;

/// <summary>
/// Исключение, используемое для досрочного завершения цикла.
/// </summary>
[PublicAPI]
public sealed class BreakException
    : ApplicationException
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public BreakException()
        : base ("Досрочное завершение цикла")
    {
        // пустое тело конструктора
    }

    #endregion
}
