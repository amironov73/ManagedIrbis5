// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* VariableNode.cs -- ссыллка на переменную
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Ссылка на переменную.
/// </summary>
internal sealed class VariableNode
    : AtomNode
{
    #region Private members

    private readonly string _name;

    /// <summary>
    /// Конструктор.
    /// </summary>
    public VariableNode
        (
            string name
        )
    {
        _name = name;
    }

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute
        (
            Context context
        )
    {
        if (context.Variables.TryGetValue (_name, out var value))
        {
            return value;
        }

        context.Error.WriteLine ($"Variable '{_name}' not defined");

        return null;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"variable '{_name}'";
    }

    #endregion
}
