// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* BinaryNode.cs -- бинарная инфиксная операция
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

using AM.Kotik.Ast;
using AM.Text;

#endregion

#nullable enable

namespace AM.Kotik.Barsik.Ast;

/// <summary>
/// Бинарная инфиксная операция, например, сложение.
/// </summary>
public sealed class BinaryNode
    : AtomNode
{
    #region Construciton

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BinaryNode
        (
            AtomNode left,
            string operation,
            AtomNode right
        )
    {
        Sure.NotNull (left);
        Sure.NotNull (operation);
        Sure.NotNull (right);

        _left = left;
        _operation = operation;
        _right = right;
    }

    #endregion

    #region Private members

    private readonly AtomNode _left;
    private readonly string _operation;
    private readonly AtomNode _right;

    /// <summary>
    /// Оператор сравнения вида `left &lt;=&gt; right`.
    /// Возвращает 0, если операнды равны, иначе значение
    /// меньше 0 (если левый операнд меньше) или больше 0
    /// (если левый операнд больше).
    /// </summary>
    private static dynamic? Shuttle
        (
            Context context,
            dynamic? left,
            dynamic? right
        )
    {
        context.NotUsed();

        if (left is string leftString && right is string rightString)
        {
            // строки сравниваются без учета культуры
            var result = string.CompareOrdinal (leftString, rightString);
            return result > 0 ? 1 : result < 0 ? -1 : 0;
        }

        return OmnipotentComparer.Default.Compare (left, right);
    }

    /// <summary>
    /// Сравнеие текстов с точностью до регистра символов.
    /// </summary>
    private static dynamic Same
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

        if (left is string leftString)
        {
            if (right is string rightString)
            {
                return leftString.SameString (rightString);
            }

            if (right is IEnumerable enumerable)
            {
                foreach (var one in enumerable)
                {
                    if (one is string rightOne)
                    {
                        if (leftString.SameString (rightOne))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        // для всех прочих типов данных оператор не реализован
        throw new NotImplementedException();
    }

    /// <summary>
    /// Расширенная операция сложения.
    /// Включает в себя, кроме прочего, сложение
    /// списков и словарей.
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
                    result.TryAdd (entry.Key, entry.Value);
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

        // Обратите внимание: благодаря магии DLR
        // работают такие неочевидные вещи как
        // `"1" + 2` или `1 + "2"`.
        // В принципе, они работают и в C#, но вдруг
        // для кого-нибудь это будет открытием.
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
                var builder = StringBuilderPool.Shared.Get();
                for (var i = 0; i < length; i++)
                {
                    builder.Append (str);
                }

                return builder.ReturnShared();
            }
        }

        return left * right;
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
            // строки сравниваются без учета культуры
            return string.CompareOrdinal (leftString, rightString) < 0;
        }

        // return left < right;
        return OmnipotentComparer.Default.Compare (left, right) < 0;
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
            // строки сравниваются без учета культуры
            return string.CompareOrdinal (leftString, rightString) <= 0;
        }

        // return left <= right;
        return OmnipotentComparer.Default.Compare (left, right) <= 0;
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
            // строки сравниваются без учета культуры
            return string.CompareOrdinal (leftString, rightString) > 0;
        }

        // return left > right;
        return OmnipotentComparer.Default.Compare (left, right) > 0;
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
            // строки сравниваются без учета культуры
            return string.CompareOrdinal (leftString, rightString) >= 0;
        }

        // return left >= right;
        return OmnipotentComparer.Default.Compare (left, right) >= 0;
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

        if (left is string leftString && right is string rightString)
        {
            // строки сравниваются без учета культуры
            return string.CompareOrdinal (leftString, rightString) == 0;
        }

        // return left == right;
        return OmnipotentComparer.Default.Compare (left, right) == 0;
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

        // TODO дополнить полезными расширениями
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

        // TODO дополнить полезными расширениями
        return left >> right;
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

        // TODO реализовать интеллектуально
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

        var input = KotikUtility.ToString ((object?) left);
        if (right is Regex regex)
        {
            return regex.IsMatch (input);
        }

        var pattern = KotikUtility.ToString ((object?) right);
        var timeout = TimeSpan.FromSeconds (1);

        return Regex.IsMatch (input, pattern, RegexOptions.None, timeout);
    }

    // TODO задействовать для чего-нибудь другого
    // /// <summary>
    // /// Оператор слияния `1 ?? 2`.
    // /// Возвращает первый из аргументов, не выдающий ложь
    // /// с точки зрения Барсика. Если все аргументы ложны,
    // /// выдает последний из них.
    // /// </summary>
    // private static dynamic? Coalesce
    //     (
    //         Context context,
    //         dynamic? left,
    //         dynamic? right
    //     )
    // {
    //     context.NotUsed();
    //
    //     var result = KotikUtility.ToBoolean (left) ? left : right;
    //
    //     return result;
    // }

    /// <summary>
    /// Оператор "или". Работает по-питоновски:
    /// `"a" || "b"` выдает `"a"`,
    /// `"" || "b"` выдает `"b"`.
    /// </summary>
    private static dynamic? Or
        (
            Context context,
            dynamic? left,
            dynamic? right
        )
    {
        context.NotUsed();

        var result = KotikUtility.ToBoolean (left) ? left : right;

        return result;
    }

    /// <summary>
    /// Оператор "и". Работает по-питоновски:
    /// `"a" &amp;&amp; "b"` выдает `"b"`,
    /// `"" &amp;&amp; "b"` выдает `""`.
    /// </summary>
    private static dynamic? And
        (
            Context context,
            dynamic? left,
            dynamic? right
        )
    {
        context.NotUsed();

        var result = KotikUtility.ToBoolean (right) ? left : right;

        return result;
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

        var result = _operation switch
        {
            "+" => Addition (context, left, right),
            "-" => left - right,
            "*" => Multiplication (context, left, right),
            "/" => left / right,
            "%" => left % right,
            "<" => Less (context, left, right),
            ">" => More (context, left, right),
            "<=" => LessOrEqual (context, left, right),
            ">=" => MoreOrEqual (context, left, right),
            "==" => Equality (context, left, right),
            "!=" => !Equality (context, left, right),
            "===" => StrictEquality (context, left, right),
            "!==" => !StrictEquality (context, left, right),
            "<<" => LeftShift (context, left, right),
            ">>" => RightShift (context, left, right),
            "|" => left | right,
            "||" => Or (context, left, right),
            "&" => left & right,
            "&&" => And (context, left, right),
            "^" => left ^ right,
            "^^" => KotikUtility.ToBoolean (left) != KotikUtility.ToBoolean (right),
            "~" => Same (context, left, right),
            "~~" => RegexMatch (context, left, right),
            "in" => In (context, left, right),
            "<=>" => Shuttle (context, left, right),
            // "??" => Coalesce (context, left, right),
            _ => throw new InvalidOperationException ($"Unknown operator {_operation}")
        };

        // context.Output.WriteLine ($"Compute {left} {_operation} {right} = {result}");

        return result;
    }

    #endregion

    #region AstNode members

    /// <inheritdoc cref="AstNode.DumpHierarchyItem(string?,int,System.IO.TextWriter,string?)"/>
    internal override void DumpHierarchyItem
        (
            string? name,
            int level,
            TextWriter writer
        )
    {
        base.DumpHierarchyItem (name, level, writer, ToString());

        _left.DumpHierarchyItem ("Left", level + 1, writer);
        DumpHierarchyItem ("Op", level + 1, writer, _operation);
        _right.DumpHierarchyItem ("Right", level + 1, writer);
    }

    /// <inheritdoc cref="AstNode.GetNodeInfo"/>
    public override AstNodeInfo GetNodeInfo() => new (this)
        {
            Name = "binary operation",
            Children =
            {
                new AstNodeInfo
                {
                    Name = "operation",
                    Description = _operation
                },

                _left.GetNodeInfo().WithName ("left"),
                _right.GetNodeInfo().WithName ("right")
            }
        };

    #endregion
}
