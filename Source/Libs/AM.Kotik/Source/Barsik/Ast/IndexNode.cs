// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* IndexNode.cs -- обращение к элементу по индексу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Reflection;

#endregion

#nullable enable

namespace AM.Kotik.Barsik;

/// <summary>
/// Обращение к элементу по индексу.
/// </summary>
public sealed class IndexNode
    : PostfixNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public IndexNode
        (
            AtomNode obj,
            AtomNode index
        )
    {
        Sure.NotNull (obj);
        Sure.NotNull (index);

        _obj = obj;
        _index = index;
    }

    #endregion

    #region Private members

    private readonly AtomNode _obj;
    private readonly AtomNode _index;

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic? Compute
        (
            Context context
        )
    {
        var obj = _obj.Compute (context);
        if (obj is null)
        {
            return null;
        }

        // TODO: в классе может быть больше одного индексера

        var index = _index.Compute (context);

        if (obj is Array array && index is int integerIndex)
        {
            return array.GetValue (integerIndex);
        }

        if (obj is IList list && index is int integerIndex2)
        {
            return list[integerIndex2];
        }

        var type = ((object) obj).GetType();
        ParameterInfo[]? parameters;
        PropertyInfo? indexer = null;
        foreach (var property in type.GetProperties (BindingFlags.Instance | BindingFlags.Public))
        {
            parameters = property.GetIndexParameters();
            if (parameters.Length != 0)
            {
                indexer = property;
                break;
            }
        }

        if (indexer is null)
        {
            return null;
        }

        var method = indexer.GetGetMethod();
        if (method is null)
        {
            return null;
        }

        var result = method.Invoke (obj, new object? [] { index });

        return result;
    }

    /// <inheritdoc cref="AtomNode.Assign"/>
    public override dynamic? Assign
        (
            Context context,
            string? operation,
            dynamic? value
        )
    {
        dynamic? variableValue = null;

        if (operation != "=")
        {
            variableValue = Compute (context);
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

        var index = _index.Compute (context);

        // TODO: не вычислять дважды
        var obj = _obj.Compute (context);
        if (obj is null)
        {
            return null;
        }

        if (obj is Array array && index is int integerIndex)
        {
            array.SetValue (value, integerIndex);
            return value;
        }

        if (obj is IList list && index is int integerIndex2)
        {
            list[integerIndex2] = value;
            return value;
        }

        var type = ((object) obj).GetType();
        ParameterInfo[]? parameters;
        PropertyInfo? indexer = null;
        foreach (var property in type.GetProperties (BindingFlags.Instance | BindingFlags.Public))
        {
            parameters = property.GetIndexParameters();
            if (parameters.Length != 0)
            {
                indexer = property;
                break;
            }
        }

        if (indexer is null)
        {
            return null;
        }

        var method = indexer.GetSetMethod();
        if (method is null)
        {
            return null;
        }

        // TODO почему возвращается value, а не result?
        var result = method.Invoke (obj, new object? [] { index, value });

        return value;
    }

    #endregion
}
