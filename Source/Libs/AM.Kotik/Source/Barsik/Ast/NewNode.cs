// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* NewNode.cs -- оператор new
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Оператор `new`.
/// </summary>
public sealed class NewNode
    : AtomNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public NewNode
        (
            string typeName,
            AtomNode[] constructorArguments
        )
    {
        _typeName = typeName;
        _constructorArguments = constructorArguments;
    }

    #endregion

    #region Private members

    private readonly string _typeName;
    private readonly AtomNode[] _constructorArguments;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="Compute"/>
    public override dynamic? Compute
        (
            Context context
        )
    {
        var type = Type.GetType (_typeName, true)!;
        var result = Activator.CreateInstance (type);

        return result;
    }

    #endregion
}
