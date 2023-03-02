// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* InMemoryProgramCache.cs -- кеш программ в памяти
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast.Caching;

/// <summary>
/// Кеш программ в оперативной памяти.
/// </summary>
[PublicAPI]
public sealed class InMemoryProgramCache
    : IProgramCache
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public InMemoryProgramCache()
    {
        _dictionary = new ();
    }

    #endregion

    #region Private members

    private readonly Dictionary<string, ProgramNode> _dictionary;

    #endregion

    #region IProgramCache members

    /// <inheritdoc cref="IProgramCache.Add"/>
    public void Add
        (
            string sourceCode,
            ProgramNode program
        )
    {
        Sure.NotNull (sourceCode);
        Sure.NotNull (program);
        
        _dictionary.Add (sourceCode, program);
    }

    /// <inheritdoc cref="IProgramCache.Clear"/>
    public void Clear()
    {
        _dictionary.Clear();
    }

    /// <inheritdoc cref="IProgramCache.TryGetProgram"/>
    public bool TryGetProgram
        (
            string sourceCode,
            out ProgramNode? program
        )
    {
        Sure.NotNull (sourceCode);

        return _dictionary.TryGetValue (sourceCode, out program);
    }

    #endregion
}
