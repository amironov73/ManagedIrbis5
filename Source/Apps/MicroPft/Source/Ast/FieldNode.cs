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

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace MicroPft.Ast;

/// <summary>
/// Команда вывода поля.
/// </summary>
internal sealed class FieldNode
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
    public char Command { get; set; }

    /// <summary>
    /// Метка поля.
    /// </summary>
    public int Tag { get; set; }

    /// <summary>
    /// Код подполя.
    /// </summary>
    public char Code { get; set; }

    /// <summary>
    /// Смещение.
    /// </summary>
    public int Offset { get; }

    /// <summary>
    /// Длина.
    /// </summary>
    public int Width { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="command">Команда: <c>v, n, d</c>.</param>
    /// <param name="tag">Метка поля.</param>
    /// <param name="code">Код подполя (опционально).</param>
    /// <param name="offset">Смещение (опционально).</param>
    /// <param name="width">Длина (опционально).</param>
    public FieldNode
        (
            char command,
            int tag,
            char code = '\0',
            int offset = 0,
            int width = 0
        )
    {
        Sure.Positive (tag);
        Sure.NonNegative (offset);
        Sure.NonNegative (width);

        var logger = Magna.Logger;

        code = char.ToLowerInvariant (code);
        command = char.ToLowerInvariant (command);
        if (command != 'v' && command != 'n' && command != 'd')
        {
            logger.LogError ("Unknown command: {Command}", command);
            throw new ArgumentOutOfRangeException (nameof (command));
        }

        LeftHand = new ();
        RightHand = new ();
        Command = command;
        Tag = tag;
        Code = code;
        Offset = offset;
        Width = width;
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
                switch (context.Mode)
                {
                    case 'h':
                    case 'H':
                        prepared.Add
                            (
                                PftUtility.UpperMode
                                    (
                                        context.Upper,
                                        PftUtility.HeaderMode (field.Subfields)
                                    )
                            );
                        break;

                    case 'd':
                    case 'D':
                        prepared.Add
                            (
                                PftUtility.UpperMode
                                    (
                                        context.Upper,
                                        PftUtility.DataMode
                                            (
                                                PftUtility.HeaderMode (field.Subfields)
                                            )
                                    )
                            );
                        break;

                    default:
                        prepared.Add
                            (
                                PftUtility.UpperMode
                                    (
                                        context.Upper,
                                        field.ToText()
                                    )
                            );
                        break;
                }
            }
            else
            {
                switch (context.Mode)
                {
                    case 'h':
                    case 'H':
                        prepared.Add
                            (
                                PftUtility.UpperMode
                                    (
                                        context.Upper,
                                        PftUtility.HeaderMode
                                            (
                                                field.GetSubFieldValue (Code)
                                            )
                                    )
                            );
                        break;

                    case 'd':
                    case 'D':
                        prepared.Add
                            (
                                PftUtility.UpperMode
                                    (
                                        context.Upper,
                                        PftUtility.DataMode
                                            (
                                                PftUtility.HeaderMode
                                                    (
                                                        field.GetSubFieldValue (Code)
                                                    )
                                            )
                                    )
                            );
                        break;

                    default:
                        prepared.Add
                            (
                                PftUtility.UpperMode
                                    (
                                        context.Upper,
                                        field.GetSubFieldValue (Code)
                                    )
                            );
                        break;
                }
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
                if (Offset is not 0 || Width is not 0)
                {
                    var width = 1_000_000_000;
                    if (Width is not 0)
                    {
                        width = Width;
                    }

                    value = value.SafeSubstring (Offset, width);
                }

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
        writer.Write (Command);
        writer.Write7BitEncodedInt (Tag);
        writer.Write (Code);

        writer.Write7BitEncodedInt (LeftHand.Count);
        foreach (var item in LeftHand)
        {
            PftSerializer.Serialize (writer, item);
        }

        writer.Write7BitEncodedInt (RightHand.Count);
        foreach (var item in RightHand)
        {
            PftSerializer.Serialize (writer, item);
        }
    }

    /// <inheritdoc cref="PftNode.MereDeserialize"/>
    public override void MereDeserialize
        (
            BinaryReader reader
        )
    {
        Command = reader.ReadChar();
        Tag = reader.Read7BitEncodedInt();
        Code = reader.ReadChar();

        var count = reader.Read7BitEncodedInt();
        LeftHand.Clear();
        for (var i = 0; i < count; i++)
        {
            var item = PftSerializer.Deserialize (reader);
            LeftHand.Add (item);
        }

        count = reader.Read7BitEncodedInt();
        RightHand.Clear();
        for (var i = 0; i < count; i++)
        {
            var item = PftSerializer.Deserialize (reader);
            RightHand.Add (item);
        }
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

        if (Width > 0)
        {
            result.Append ('_');
            result.Append ('.');
            result.Append (Width);
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
