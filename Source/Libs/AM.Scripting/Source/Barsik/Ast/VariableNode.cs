// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement

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
    #region Properties

    /// <summary>
    /// Имя переменной.
    /// </summary>
    public string Name { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public VariableNode
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        // if (Array.IndexOf (BarsikUtility.Keywords, name) >= 0)
        // {
        //     throw new BarsikException ($"Name {name} is reserved");
        // }

        Name = name;
    }

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute
        (
            Context context
        )
    {
        if (context.TryGetVariable (Name, out var value))
        {
            return value;
        }

        var type = context.FindType (Name);
        if (type is not null)
        {
            return type;
        }

        context.Error.WriteLine ($"Variable or type '{Name}' not defined");

        return null;
    }

    /// <inheritdoc cref="AtomNode.Assign"/>
    public override dynamic? Assign
        (
            Context context,
            string operation,
            dynamic? value
        )
    {
        dynamic? variableValue = null;

        if (operation != "=")
        {
            if (!context.TryGetVariable (Name, out variableValue))
            {
                context.Error.WriteLine ($"Variable {Name} not found");
            }
        }

        value = operation switch
        {
            "=" => value,
            "+=" => variableValue + value,
            "-=" => variableValue - value,
            "*=" => variableValue * value,
            "/=" => variableValue / value,
            "%=" => variableValue % value,
            "&=" => variableValue & value,
            "|=" => variableValue | value,
            "^=" => variableValue ^ value,
            "<<=" => variableValue << value,
            ">>=" => variableValue >> value,
            _ => throw new Exception()
        };

        context.SetVariable (Name, value);

        return value;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"variable '{Name}'";
    }

    #endregion
}
