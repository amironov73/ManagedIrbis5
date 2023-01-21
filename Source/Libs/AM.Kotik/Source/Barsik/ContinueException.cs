// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ContinueException.cs -- исключение, используемое для досрочного завершения текущей итерации цикла
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Исключение, используемое для досрочного завершения текущей итерации цикла.
/// </summary>
public sealed class ContinueException
    : Exception
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ContinueException()
        : base ("Досрочное завершение итерации")
    {
        // пустое тело конструктора
    }

    #endregion
}
