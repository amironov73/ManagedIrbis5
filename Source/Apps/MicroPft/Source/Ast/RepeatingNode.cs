// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* RepeatingNode.cs -- повторяющийся литерал
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

#endregion

#nullable enable

namespace MicroPft.Ast;

/// <summary>
/// Повторяющийся литерал.
/// </summary>
internal sealed class RepeatingNode
    : PftNode
{
    #region Properties

    /// <summary>
    /// Литерал находится слева от команды вывода поля.
    /// </summary>
    public bool LeftHand { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RepeatingNode
        (
            string value,
            bool plus
        )
    {
        _value = value;
        _plus = plus;
    }

    #endregion

    #region Private members

    private readonly string _value;

    private readonly bool _plus;

    #endregion

    #region PftNode members

    /// <inheritdoc cref="PftNode.Execute"/>
    public override void Execute
        (
            PftContext context
        )
    {
        if (!_plus)
        {
            context.Write (_value);
        }
        else
        {
            if (LeftHand)
            {
                if (context.CurrentRepeat != 0)
                {
                    context.Write (_value);
                }
            }
            else
            {
                if (context.CurrentRepeat != (context.RepeatCount - 1))
                {
                    context.Write (_value);
                }
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
        return $"repeating: \"{_value}\" {_plus}";
    }

    #endregion

}
