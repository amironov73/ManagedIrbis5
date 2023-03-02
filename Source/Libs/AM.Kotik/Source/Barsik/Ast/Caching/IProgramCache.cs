// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* IProgramCache.cs -- интерфейс кеша программ
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast.Caching;

/// <summary>
/// Интерфейс кеша программ.
/// </summary>
[PublicAPI]
public interface IProgramCache
{
    /// <summary>
    /// Добавление синтаксического дерева для указанного скрипта в кеш.
    /// </summary>
    void Add 
        (
            string sourceCode,
            ProgramNode program
        );

    /// <summary>
    /// Очистка кеша.
    /// </summary>
    void Clear();

    /// <summary>
    /// Попытка извлечения из кеша программы,
    /// соответствующая указанному исходному коду.
    /// </summary>
    bool TryGetProgram
        (
            string sourceCode,
            out ProgramNode? program
        );
}
