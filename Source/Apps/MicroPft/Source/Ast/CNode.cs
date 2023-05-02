// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* CNode.cs -- перемещение в указанную позицию
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

#endregion

#nullable enable

namespace MicroPft.Ast;

/*

   Команда 'Cn' устанавливает n-ю позицию в строке.
   Иными словами, это табуляция в n-ю позицию.

 */

/// <summary>
/// Перемещение в указанную позицию.
/// </summary>
internal sealed class CNode
    : PftNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public CNode
        (
            int position
        )
    {
        _position = position;
    }

    #endregion

    #region Private members

    private int _position;

    #endregion

    #region PftNode members

    /// <inheritdoc cref="PftNode.Execute"/>
    public override void Execute
        (
            PftContext context
        )
    {
        if (context.CurrentRepeat == 0)
        {
            var current = context.Output.ColumnNumber;
            if (current < _position)
            {
                context.Write (' ', _position - current);
            }
            else if (current > _position)
            {
                context.Write ('\n');
                context.Write (' ', _position);
            }
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
        writer.Write7BitEncodedInt (_position);
    }

    /// <inheritdoc cref="PftNode.MereDeserialize"/>
    public override void MereDeserialize
        (
            BinaryReader reader
        )
    {
        _position = reader.Read7BitEncodedInt();
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"C_{_position}";
    }

    #endregion
}
