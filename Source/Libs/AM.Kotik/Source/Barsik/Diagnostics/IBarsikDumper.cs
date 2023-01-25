// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* IBarsikDumper.cs -- интерфейс дампа контекста
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Kotik.Barsik.Diagnostics;

/// <summary>
/// Интерфейс дампа контекста интерпретатора.
/// </summary>
public interface IBarsikDumper
{
    /// <summary>
    /// Дамп контекста интерпретатора (включая вложенные).
    /// </summary>
    void DumpContext (Context context);
}
