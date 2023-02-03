// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftNodeInfo.cs -- информация об узле синтаксического дерева
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Collections;
using AM.Text.Output;

#endregion

#nullable enable

namespace AM.Kotik.Ast;

/// <summary>
/// Информация об узле синтаксического дерева.
/// </summary>
public sealed class AstNodeInfo
{
    #region Properties

    /// <summary>
    /// Дочерние узлы (если есть).
    /// </summary>
    public NonNullCollection<AstNodeInfo> Children { get; } = new();

    /// <summary>
    /// Имя (для человека).
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Ссылка на сам узел.
    /// </summary>
    public AstNode? Node { get; set; }

    /// <summary>
    /// Описание узла (для человека).
    /// </summary>
    public string? Description { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public AstNodeInfo()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AstNodeInfo (AstNode? node)
    {
        Node = node;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Dump the node info (include children).
    /// </summary>
    public static void Dump
        (
            AbstractOutput output,
            AstNodeInfo nodeInfo,
            int level
        )
    {
        output.Write (new string(' ', level));
        output.Write (nodeInfo.ToString());
        output.WriteLine (string.Empty);
        foreach (var child in nodeInfo.Children)
        {
            Dump (output, child, level+1);
        }
    }

    /// <summary>
    /// Задание описания для узла.
    /// </summary>
    public AstNodeInfo WithDescription
        (
            string? description
        )
    {
        Description = description;

        return this;
    }

    /// <summary>
    /// Присвоение имени узлу.
    /// </summary>
    public AstNodeInfo WithName
        (
            string name
        )
    {
        Sure.NotNullNorEmpty (name);

        Name = name;

        return this;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString() =>
        string.IsNullOrEmpty (Description)
            ? Name.ToVisibleString()
            : Name.ToVisibleString() + ": " + Description.ToVisibleString();

    #endregion
}
