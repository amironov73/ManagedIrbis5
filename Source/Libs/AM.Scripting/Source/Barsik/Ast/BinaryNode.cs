// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement

/* BinaryNode.cs -- бинарная операция, например, сложение или сравнение двух чисел
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Бинарная операция, например, сложение или сравнение двух чисел.
/// </summary>
internal sealed class BinaryNode
    : AtomNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BinaryNode
        (
            AtomNode left,
            AtomNode right,
            string op
        )
    {
        _left = left;
        _right = right;
        _op = op;
    }

    #endregion

    #region Private members

    private readonly AtomNode _left, _right;
    private readonly string _op;

    /// <summary>
    /// Оператор сравнения.
    /// </summary>
    private static dynamic? Shuttle
        (
            Context context,
            dynamic? left,
            dynamic? right
        )
    {
        context.NotUsed();

        return OmnipotentComparer.Default.Compare (left, right);
    }

    private static dynamic? Same
        (
            Context context,
            dynamic? left,
            dynamic? right
        )
    {
        context.NotUsed();

        if (left is null)
        {
            return right is null;
        }

        if (left is char leftChar && right is char rightChar)
        {
            return leftChar.SameChar (rightChar);
        }

        if (left is string leftString && right is string rightString)
        {
            return leftString.SameString (rightString);
        }

        throw new NotImplementedException();
    }

    /// <summary>
    /// Расширенная операция сложения.
    /// </summary>
    private static dynamic? Addition
        (
            Context context,
            dynamic? left,
            dynamic? right
        )
    {
        context.NotUsed();

        if (left is IDictionary leftDictionary)
        {
            if (right is IDictionary rightDictionary)
            {
                // добавляем к левому словарю отсутствующие в нем элементы из правого
                var result = new BarsikDictionary();
                foreach (DictionaryEntry entry in leftDictionary)
                {
                    result.Add (entry.Key, entry.Value);
                }

                foreach (DictionaryEntry entry in rightDictionary)
                {
                    if (!result.ContainsKey (entry.Key))
                    {
                        result.Add (entry.Key, entry.Value);
                    }
                }

                return result;
            }
        }

        if (left is IList leftList)
        {
            // как в питоне
            var result = new BarsikList();
            result.EnsureCapacity (leftList.Count);

            foreach (var item in leftList)
            {
                result.Add (item);
            }

            if (right is IList rightList)
            {
                result.EnsureCapacity (leftList.Count + rightList.Count);

                foreach (var item in rightList)
                {
                    result.Add (item);
                }

                return result;
            }

            // TODO сомнительно!

            result.Add (right);

            return result;
        }

        return left + right;
    }

    /// <summary>
    /// Расширенная операция умножения.
    /// </summary>
    private static dynamic? Multiplication
        (
            Context context,
            dynamic? left,
            dynamic? right
        )
    {
        context.NotUsed();

        if (left is char chr)
        {
            if (right is int length)
            {
                // как в питоне
                return new string (chr, length);
            }
        }

        if (left is string str)
        {
            if (right is int length)
            {
                // как в питоне
                var builder = new StringBuilder();
                for (var i = 0; i < length; i++)
                {
                    builder.Append (str);
                }

                return builder.ToString();
            }
        }

        return left * right;
    }

    /// <summary>
    /// Расширенная операция сдвига влево.
    /// </summary>
    private static dynamic? LeftShift
        (
            Context context,
            dynamic? left,
            dynamic? right
        )
    {
        context.NotUsed();

        return left << right;
    }

    /// <summary>
    /// Расширенная операция сдвига вправо.
    /// </summary>
    private static dynamic? RightShift
        (
            Context context,
            dynamic? left,
            dynamic? right
        )
    {
        context.NotUsed();

        return left >> right;
    }

    /// <summary>
    /// Проверка приводимости типа.
    /// </summary>
    private static dynamic Is
        (
            Context context,
            dynamic? left,
            dynamic? right
        )
    {
        if (left is null)
        {
            return false;
        }

        var leftType = ((object) left).GetType();

        Type? rightType = null;
        if (right is Type alreadyHaveType)
        {
            rightType = alreadyHaveType;
        }

        if (right is string typeName)
        {
            rightType = context.FindType (typeName);
        }

        if (rightType is null)
        {
            return false;
        }

        if (leftType == rightType)
        {
            return true;
        }

        return leftType.IsAssignableTo (rightType);
    }

    /// <summary>
    /// Расширенная операция сравнение на равенство.
    /// </summary>
    private static dynamic? Equality
        (
            Context context,
            dynamic? left,
            dynamic? right
        )
    {
        context.NotUsed();

        return OmnipotentComparer.Default.Compare (left, right) == 0;
    }

    /// <summary>
    /// Расширенная операция "логическое ИЛИ".
    /// </summary>
    private static dynamic? Or
        (
            Context context,
            dynamic? left,
            dynamic? right
        )
    {
        context.NotUsed();

        return BarsikUtility.ToBoolean (left) || BarsikUtility.ToBoolean (right);
    }


    /// <summary>
    /// Расширенная операция "логическое И".
    /// </summary>
    private static dynamic? And
        (
            Context context,
            dynamic? left,
            dynamic? right
        )
    {
        context.NotUsed();

        return BarsikUtility.ToBoolean (left) && BarsikUtility.ToBoolean (right);
    }

    /// <summary>
    /// Расширенная операция "В".
    /// </summary>
    /// <param name="context">Контекст.</param>
    /// <param name="left">Что ищем.</param>
    /// <param name="right">Где ищем.</param>
    private static dynamic? In
        (
            Context context,
            dynamic? left,
            dynamic? right
        )
    {
        context.NotUsed();

        if (left is null || right is null)
        {
            return false;
        }

        if (right is string text)
        {
            if (left is char chr)
            {
                return text.Contains (chr);
            }

            if (left is string sub)
            {
                return text.Contains (sub);
            }
        }

        if (right is Array array)
        {
            return Array.IndexOf (array, (object) left) >= 0;
        }

        if (right is IDictionary dictionary)
        {
            return dictionary.Contains ((object) left);
        }

        if (right is IList list)
        {
            return list.Contains ((object) left);
        }

        return false;
    }

    /// <summary>
    /// Равенство адресов в памяти.
    /// </summary>
    private static dynamic? StrictEquality
        (
            Context context,
            dynamic? left,
            dynamic? right
        )
    {
        context.NotUsed();

        return ReferenceEquals (left, right);
    }

    /// <summary>
    /// Проверка на совпадение строки с шаблоном.
    /// </summary>
    private static dynamic RegexMatch
        (
            Context context,
            dynamic? left,
            dynamic? right
        )
    {
        context.NotUsed();

        var input = BarsikUtility.ToString ((object?) left);
        if (right is Regex regex)
        {
            return regex.IsMatch (input);
        }

        var pattern = BarsikUtility.ToString ((object?) right);

        return Regex.IsMatch (input, pattern);
    }

    private static dynamic Less
        (
            Context context,
            dynamic? left,
            dynamic? right
        )
    {
        context.NotUsed();

        if (left is string leftString && right is string rightString)
        {
            return string.Compare (leftString, rightString) < 0;
        }

        return left < right;
    }

    private static dynamic LessOrEqual
        (
            Context context,
            dynamic? left,
            dynamic? right
        )
    {
        context.NotUsed();

        if (left is string leftString && right is string rightString)
        {
            return string.Compare (leftString, rightString) <= 0;
        }

        return left <= right;
    }

    private static dynamic More
        (
            Context context,
            dynamic? left,
            dynamic? right
        )
    {
        context.NotUsed();

        if (left is string leftString && right is string rightString)
        {
            return string.Compare (leftString, rightString) > 0;
        }

        return left > right;
    }

    private static dynamic MoreOrEqual
        (
            Context context,
            dynamic? left,
            dynamic? right
        )
    {
        context.NotUsed();

        if (left is string leftString && right is string rightString)
        {
            return string.Compare (leftString, rightString) >= 0;
        }

        return left >= right;
    }

    #endregion

    #region AtomNode members

    /// <inheritdoc cref="AtomNode.Compute"/>
    public override dynamic Compute
        (
            Context context
        )
    {
        var left = _left.Compute (context);
        var right = _right.Compute (context);

        return _op switch
        {
            "+" => Addition (context, left, right),
            "-" => left - right,
            "*" => Multiplication (context, left, right),
            "/" => left / right,
            "%" => left % right,
            "<<" => LeftShift (context, left, right),
            "<" => Less (context, left, right),
            "<=" => LessOrEqual (context, left, right),
            ">>" => RightShift (context, left, right),
            ">" => More (context, left, right),
            ">=" => MoreOrEqual (context, left, right),
            "==" => Equality (context, left, right),
            "!=" => left != right,
            "===" => StrictEquality (context, left, right),
            "||" or "or" => Or (context, left, right),
            "|" => left | left,
            "&&" or "and" => And (context, left, right),
            "&" => left & right,
            "^^" => BarsikUtility.ToBoolean (left) != BarsikUtility.ToBoolean (right),
            "^" => left ^ right,
            "~" => RegexMatch (context, left, right),
            "is" => Is (context, left, right),
            "in" => In (context, left, right),
            "same" => Same (context, left, right),
            "@" or "<=>" or "<:>" => Shuttle (context, left, right),
            _ => throw new Exception ($"Unknown operation '{_op}'")
        };
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"binary ({_left} {_op} {_right})";
    }

    #endregion
}
