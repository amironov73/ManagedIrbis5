// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global

/* BinaryNode.cs -- бинарная операция, например, сложение или сравнение двух чисел
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;

using AM.Text;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Бинарная операция, например, сложение или сравнение двух чисел.
    /// </summary>
    sealed class BinaryNode : AtomNode
    {
        private readonly AtomNode _left, _right;
        private readonly string _op;

        public BinaryNode (AtomNode left, AtomNode right, string op)
        {
            _left = left;
            _right = right;
            _op = op;
        }

        public override dynamic Compute (Context context)
        {
            var left = _left.Compute (context);
            var right = _right.Compute (context);

            return _op switch
            {
                "+" => left + right,
                "-" => left - right,
                "*" => left * right,
                "/" => left / right,
                "%" => left % right,
                "^" => left ^ right,
                "<" => left < right,
                "<=" => left <= right,
                ">" => left > right,
                ">=" => left >= right,
                "==" => left == right,
                "!=" => left != right,
                _ => throw new Exception ($"Unknown operation '{_op}'")
            };
        }

        public override string ToString() => $"binary ({_left} {_op} {_right})";
    }
}
