// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* AstNode.cs -- абстрактный узел AST
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM.Kotik.Compilation;
using AM.Runtime.Mere;

#endregion

namespace AM.Kotik.Ast;

/// <summary>
/// Абстрактный узел AST.
/// </summary>
public abstract class AstNode
    : IMereSerializable
{
    #region Public methods

    /// <summary>
    /// Прием посетителя.
    /// </summary>
    public virtual bool AcceptVisitor
        (
            AstVisitor visitor
        )
    {
        Sure.NotNull (visitor);

        return visitor.VisitNode (this);
    }

    /// <summary>
    /// Сравнение с информацией об AST-узле.
    /// Необходимо для автоматизированных тестов парсера.
    /// </summary>
    public virtual bool CompareTo
        (
            AstNodeInfo nodeInfo
        )
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Транспиляция узла в C#.
    /// </summary>
    public virtual void Compile
        (
            ScriptCompiler compiler
        )
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Получение информации об AST-узле.
    /// </summary>
    public virtual AstNodeInfo GetNodeInfo()
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Internal methods

    /// <summary>
    /// Общая реализация дампа узла.
    /// </summary>
    internal void DumpHierarchyItem
        (
            string? name,
            int level,
            TextWriter writer,
            string? value
        )
    {
        for (var i = 0; i < level; i++)
        {
            writer.Write ("| ");
        }

        writer.Write ("+ ");

        if (!string.IsNullOrEmpty (name))
        {
            writer.Write (name);
            writer.Write (": ");
        }

        writer.WriteLine (value);
    }

    /// <summary>
    /// Дамп узла как элемента иерархии.
    /// </summary>
    internal virtual void DumpHierarchyItem
        (
            string? name,
            int level,
            TextWriter writer
        )
    {
        DumpHierarchyItem (name, level, writer, GetType().Name);
    }

    #endregion

    #region IMereSerializable members

    /// <inheritdoc cref="IMereSerializable.MereSerialize"/>
    public virtual void MereSerialize
        (
            BinaryWriter writer
        )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="IMereSerializable.MereDeserialize"/>
    public virtual void MereDeserialize
        (
            BinaryReader reader
        )
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => GetType().Name;

    #endregion
}
