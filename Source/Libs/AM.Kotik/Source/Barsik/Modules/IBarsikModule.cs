// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UseNameofExpression

/* IBarsikModule.cs -- интерфейс Барсик-модуля
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Интерфейс Барсик-модуля.
/// </summary>
public interface IBarsikModule
{
    /// <summary>
    /// Описание в произвольной форме.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Версия модуля.
    /// </summary>
    Version Version { get; }

    /// <summary>
    /// Инициализация модуля в интерпретаторе.
    /// </summary>
    bool AttachModule (Interpreter interpreter);

    /// <summary>
    /// Освобождение (деинициализация) модуля в интерпретаторе.
    /// </summary>
    void DetachModule (Interpreter interpreter);
}
