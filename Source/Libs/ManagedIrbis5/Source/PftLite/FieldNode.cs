// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* FieldNode.cs -- команда вывода поля/подполя
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.PftLite;

/// <summary>
/// Команда вывода поля.
/// </summary>
sealed class FieldNode
    : PftNode
{
    #region Properties

    /// <summary>
    /// Узлы левой руки.
    /// </summary>
    public List<PftNode> LeftHand { get; }

    /// <summary>
    /// Узлы правой руки.
    /// </summary>
    public List<PftNode> RightHand { get; }

    /// <summary>
    /// Команда вывода поля: <c>v, n, d</c>.
    /// </summary>
    public char Command { get; }

    /// <summary>
    /// Метка поля.
    /// </summary>
    public int Tag { get; }

    /// <summary>
    /// Код подполя.
    /// </summary>
    public char Code { get; }

    /// <summary>
    /// Смещение.
    /// </summary>
    public int Offset { get; }

    /// <summary>
    /// Длина.
    /// </summary>
    public int Length { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="command">Команда: <c>v, n, d</c>.</param>
    /// <param name="tag">Метка поля.</param>
    /// <param name="code">Код подполя (опционально).</param>
    /// <param name="offset">Смещение (опционально).</param>
    /// <param name="length">Длина (опционально).</param>
    public FieldNode
        (
            char command,
            int tag,
            char code = '\0',
            int offset = 0,
            int length = 0
        )
    {
        Sure.Positive (tag);
        Sure.NonNegative (offset);
        Sure.NonNegative (length);

        code = char.ToLowerInvariant (code);
        command = char.ToLowerInvariant (command);
        if (command != 'v' && command != 'n' && command != 'd')
        {
            throw new ArgumentOutOfRangeException (nameof (command));
        }

        LeftHand = new ();
        RightHand = new ();
        Command = command;
        Tag = tag;
        Code = code;
        Offset = offset;
        Length = length;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        var result = new StringBuilder();
        result.Append ("field: ");
        result.Append (Command);
        result.Append ('_');
        result.Append (Tag);

        if (Code != '\0')
        {
            result.Append ('_');
            result.Append ('^');
            result.Append (Code);
        }

        if (Offset > 0)
        {
            result.Append ('_');
            result.Append ('*');
            result.Append (Offset);
        }

        if (Length > 0)
        {
            result.Append ('_');
            result.Append ('.');
            result.Append (Length);
        }

        if (LeftHand.Count != 0)
        {
            result.Append (" [left:");
            foreach (var node in LeftHand)
            {
                result.Append (' ');
                result.Append (node);
            }
            result.Append (']');
        }

        if (RightHand.Count != 0)
        {
            result.Append (" [right:");
            foreach (var node in RightHand)
            {
                result.Append (' ');
                result.Append (node);
            }
            result.Append (']');
        }

        return result.ToString();
    }

    #endregion
}
