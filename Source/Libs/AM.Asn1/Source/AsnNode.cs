// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable VirtualMemberCallInConstructor

/* AsnNode.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Text;

#endregion

#nullable enable

namespace AM.Asn1;

/// <summary>
///
/// </summary>
public class AsnNode
{
    #region Properties

    /// <summary>
    /// Parent node.
    /// </summary>
    public AsnNode? Parent { get; internal set; }

    /// <summary>
    /// Breakpoint.
    /// </summary>
    public bool Breakpoint { get; set; }

    /// <summary>
    /// Список потомков. Может быть пустым.
    /// </summary>
    public virtual IList<AsnNode> Children
    {
        get => _children ??= new AsnNodeCollection (this);
        protected set
        {
            var collection = (AsnNodeCollection)value;
            collection.Parent = this;
            collection.EnsureParent();
            _children = collection;
        }
    }

    /// <summary>
    /// Column number.
    /// </summary>
    public int Column { get; set; }

    /// <summary>
    /// Номер строки, на которой в скрипте расположена
    /// соответствующая конструкция языка.
    /// </summary>
    public int LineNumber { get; set; }

    /// <summary>
    /// Text.
    /// </summary>
    public virtual string? Text { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public AsnNode()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public AsnNode
        (
            AsnToken token
        )
    {
        LineNumber = token.Line;
        Column = token.Column;
        Text = token.Text;
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    public AsnNode
        (
            params AsnNode[] children
        )
    {
        foreach (var child in children)
        {
            Children.Add (child);
        }
    }

    #endregion

    #region Private members

    private AsnNodeCollection? _children;

    #endregion

    #region Public methods

    #endregion

    #region Object members

    /// <inheritdoc cref="object.Equals(object)" />
    public override bool Equals
        (
            object? other
        )
    {
        return ReferenceEquals (this, other);
    }

    /// <inheritdoc cref="object.GetHashCode" />
    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = Column;
            hashCode = (hashCode * 397) ^ LineNumber;
            hashCode = (hashCode * 397) ^
                       (
                           Text != null
                               ? Text.GetHashCode()
                               : 0
                       );

            return hashCode;
        }
    }

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var result = new StringBuilder();

        foreach (var child in Children)
        {
            result.Append (child);
        }

        return result.ToString();
    }

    #endregion
}
