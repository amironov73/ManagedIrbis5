// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ConditionalNode.cs -- условный литерал
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.PftLite;

/// <summary>
/// Условный литерал.
/// </summary>
internal sealed class ConditionalNode
    : PftNode
{
    #region Properties

    /// <summary>
    /// Литерал находится слева от команды вывода поля.
    /// </summary>
    public bool LeftHand { get; set; }

    /// <summary>
    /// Поле, которому принадлежит литерал.
    /// </summary>
    public FieldNode? Field { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ConditionalNode
        (
            string value
        )
    {
        _value = value;
    }

    #endregion

    #region Private members

    private readonly string _value;

    #endregion

    #region PftNode members

    /// <inheritdoc cref="PftNode.Execute"/>
    public override void Execute
        (
            PftContext context
        )
    {
        if (LeftHand)
        {
            if (context.CurrentRepeat == 0)
            {
                context.Write (_value);
            }
        }
        else
        {
            if (context.CurrentRepeat == context.RepeatCount - 1)
            {
                context.Write (_value);
            }
        }
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"conditional: \"{_value}\" {LeftHand}";
    }

    #endregion
}
