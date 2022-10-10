// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable ReplaceSliceWithRangeIndexer

/* IInteractivityProvider.cs -- некая абстракция над простейшей интерактивностью
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using AM.Collections;

#endregion

#nullable enable

namespace AM.Interactivity;

/// <summary>
/// Некая абстракция над простейшей интерактивностью.
/// </summary>
public interface IInteractivityProvider
{
    /// <summary>
    /// Показ простого сообщения.
    /// </summary>
    void ShowMessage (string messageText);

    /// <summary>
    /// Простой запрос на ввод значения.
    /// </summary>
    string? QueryValue
        (
            string prompt,
            string? defaultValue = null
        );
}
