// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ExternalCodeHandler.cs -- обработчик внешнего кода
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using AM.Kotik.Barsik.Ast;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Обработчик внешнего кода.
/// </summary>
public delegate void ExternalCodeHandler
    (
        Context context,
        ExternalNode node
    );
