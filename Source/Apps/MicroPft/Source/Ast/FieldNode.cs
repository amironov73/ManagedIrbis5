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
using System.IO;
using System.Text;

using AM;

using ManagedIrbis;

#endregion

#nullable enable

namespace MicroPft.Ast;

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

    #region Private members

    private string?[]? _prepared;

    /// <summary>
    /// Подготовка к расформатированию.
    /// </summary>
    internal int Prepare
        (
            PftContext context
        )
    {
        var fields = context.Record.Fields.GetField (Tag);

        if (fields.Length == 0)
        {
            _prepared = Array.Empty<string>();
            return 0;
        }

        var prepared = new List<string?> (fields.Length);
        foreach (var field in fields)
        {
            if (Code == '\0')
            {
                prepared.Add (field.ToText());
            }
            else
            {
                prepared.Add (field.GetSubFieldValue (Code));
            }
        }

        _prepared = prepared.ToArray();

        return _prepared.Length;
    }

    private void _Execute
        (
            PftContext context
        )
    {
        var prepared = _prepared.ThrowIfNull();
        var value = prepared.SafeAt (context.CurrentRepeat);
        if (string.IsNullOrEmpty (value))
        {
            if (Command == 'n')
            {
                foreach (var node in LeftHand)
                {
                    node.Execute (context);
                }
            }
        }
        else
        {
            if (Command is 'v' or 'd')
            {
                foreach (var node in LeftHand)
                {
                    node.Execute (context);
                }
            }

            if (Command == 'v')
            {
                context.Write (value);
            }

            if (Command == 'v')
            {
                foreach (var node in RightHand)
                {
                    node.Execute (context);
                }
            }
        }
    }

    #endregion

    #region PftNode members

    /// <inheritdoc cref="PftNode.Execute"/>
    public override void Execute
        (
            PftContext context
        )
    {
        var group = context.CurrentGroup;
        var record = context.Record;

        if (group is null)
        {
            context.RepeatCount = Prepare (context);

            if (context.RepeatCount == 0 && Command == 'n')
            {
                foreach (var node in LeftHand)
                {
                    node.Execute (context);
                }
            }

            for (context.CurrentRepeat = 0;
                 context.CurrentRepeat < context.RepeatCount;
                 context.CurrentRepeat++)
            {
                _Execute(context);
            }
        }
        else
        {
            _Execute (context);
        }
    }

    #endregion

    #region MereSerializer members

    /// <inheritdoc cref="PftNode.MereSerialize"/>
    public override void MereSerialize
        (
            BinaryWriter writer
        )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="PftNode.MereDeserialize"/>
    public override void MereDeserialize
        (
            BinaryReader reader
        )
    {
        throw new NotImplementedException();
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
