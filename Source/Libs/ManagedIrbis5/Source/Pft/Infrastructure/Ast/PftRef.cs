// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftRef.cs -- ref() function
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using AM;
using AM.Text;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;
using ManagedIrbis.Providers;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast;

//
// Функция REF позволяет извлечь данные из альтернативной
// записи файла документов (той же самой БД).
//
// * Первый аргумент - это числовое выражение,
// дающее MFN альтернативной записи, которая должна быть выбрана,
//
// * второй аргумент - это формат, который должен быть применен
// к этой записи.
//
// Если значение выражения не соответствует MFN ни одной
// из записей базы данных, то функция REF возвратит пустую строку.
//
// Функция REF - очень мощное средство, поскольку позволяет
// объединить данные, хранимые в различных записях базы данных,
//в один выводимый документ.
//
// В большинстве случаев связывание записей непосредственно
// через MFN может оказаться неудобным.
//
// Более удобным является использование возможности функции L.
// Напомним, что функция L находит MFN, соответствующий
// термину доступа. Поэтому можно использовать ее для преобразования
// символьной строки в MFN. Для корректного использования функции
// L нужно установить однозначное соответствие между символьной
// строкой и соответствующим ей MFN. Инвертированный файл
// предоставляет возможность установить такое соответствие.
//
// Система не делает никаких предположений относительно природы
// связей, существующих между записями. Она просто предоставляет
// механизм связывания записей. При конкретном практическом
// применении пользователь сам определяет смысл связей посредством
// использования языка форматирования и специального проектирования
// базы данных.
//
// Например, если библиографическая запись описания статьи должна
// быть связана с записью соответствующего номера журнала,
// то необходимо поле для отражения природы этой связи
// (шифр номера журнала).
//
// Так как второй аргумент функции REF является форматом,
// то имеется возможность использовать данную функцию рекурсивно
// с установлением многоуровневой иерархический связи.
//

/// <summary>
/// ref() function
/// </summary>
public sealed class PftRef
    : PftNode
{
    #region Properties

    /// <summary>
    /// MFN.
    /// </summary>
    public PftNumeric? Mfn { get; set; }

    /// <summary>
    /// Format.
    /// </summary>
    public PftNodeCollection Format { get; private set; }

    /// <inheritdoc cref="PftNode.Children" />
    public override IList<PftNode> Children
    {
        get
        {
            if (ReferenceEquals (_virtualChildren, null))
            {
                _virtualChildren = new VirtualChildren();
                var nodes = new List<PftNode>();
                if (!ReferenceEquals (Mfn, null))
                {
                    nodes.Add (Mfn);
                }

                nodes.AddRange (Format);
                _virtualChildren.SetChildren (nodes);
            }

            return _virtualChildren;
        }
        protected set
        {
            Magna.Logger.LogError
                (
                    nameof (PftRef) + "::" + nameof (Children)
                    + ": set value={Value}",
                    value.ToVisibleString()
                );
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PftRef()
    {
        // Нельзя делать конструктор приватным! Упадет сериализация!
        Format = new PftNodeCollection (this);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftRef
        (
            PftToken token
        )
        : base (token)
    {
        token.MustBe (PftTokenKind.Ref);

        Format = new PftNodeCollection (this);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftRef
        (
            PftNumeric mfn,
            params PftNode[] format
        )
        : this()
    {
        Mfn = mfn;
        foreach (var node in format)
        {
            Format.Add (node);
        }
    }

    #endregion

    #region Private members

    private VirtualChildren? _virtualChildren;

    #endregion

    #region ICloneable members

    /// <inheritdoc cref="ICloneable.Clone" />
    public override object Clone()
    {
        var result = (PftRef) base.Clone();

        if (Mfn is null)
        {
            result.Mfn = (PftNumeric)Mfn.Clone();
        }

        result.Format = Format.CloneNodes (result).ThrowIfNull();

        return result;
    }

    #endregion

    #region PftNode members

    /// <inheritdoc cref="PftNode.CompareNode" />
    internal override void CompareNode
        (
            PftNode otherNode
        )
    {
        Sure.NotNull (otherNode);

        base.CompareNode (otherNode);

        var otherRef = (PftRef)otherNode;
        PftSerializationUtility.CompareNodes
            (
                Mfn,
                otherRef.Mfn
            );
        PftSerializationUtility.CompareLists
            (
                Format,
                otherRef.Format
            );
    }

    /// <inheritdoc cref="PftNode.Compile" />
    public override void Compile
        (
            PftCompiler compiler
        )
    {
        Sure.NotNull (compiler);

        if (Mfn is null)
        {
            throw new PftCompilerException();
        }

        if (Format.Count == 0)
        {
            throw new PftCompilerException();
        }

        Mfn.Compile (compiler);
        compiler.CompileNodes (Format);

        compiler.StartMethod (this);

        compiler
            .WriteIndent()
            .Write ("int mfn = (int)")
            .CallNodeMethod (Mfn);

        compiler
            .WriteIndent()
            .WriteLine ("MarcRecord record = Context.Provider.ReadRecord(mfn);")
            .WriteIndent()
            .WriteLine ("if (!ReferenceEquals(record, null))")
            .WriteIndent()
            .WriteLine ("{")
            .IncreaseIndent()
            .WriteIndent();

        compiler
            .WriteLine ("using (new PftContextSaver(Context, true))")
            .WriteIndent()
            .WriteLine ("{")
            .IncreaseIndent()
            .WriteIndent()
            .WriteLine ("Context.Record = record;")
            .CallNodes (Format)
            .DecreaseIndent()
            .WriteIndent()
            .WriteLine ("}");

        compiler
            .DecreaseIndent()
            .WriteIndent()
            .WriteLine ("}");

        compiler.EndMethod (this);
        compiler.MarkReady (this);
    }

    /// <inheritdoc cref="PftNode.Deserialize" />
    protected internal override void Deserialize
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        base.Deserialize (reader);

        Mfn = (PftNumeric?)PftSerializer.DeserializeNullable (reader);
        PftSerializer.Deserialize (reader, Format);
    }

    /// <inheritdoc cref="PftNode.Execute" />
    public override void Execute
        (
            PftContext parentContext
        )
    {
        Sure.NotNull (parentContext);

        OnBeforeExecution (parentContext);

        if (Mfn is not null)
        {
            parentContext.Evaluate (Mfn);
            var newMfn = (int) Mfn.Value;
            var oldMfn = 0;
            var record = parentContext.Record;
            if (record is not null)
            {
                oldMfn = record.Mfn;
            }

            if (newMfn != oldMfn)
            {
                // TODO some caching

                record = parentContext.Provider.ReadRecord (newMfn);
            }

            if (record is not null)
            {
                var nestedContext = new PftContext (parentContext)
                {
                    Record = record,
                    Output = parentContext.Output
                };

                // формат вызывается в контексте без повторений
                nestedContext.Reset();

                nestedContext.Execute (Format);
            }
        }

        OnAfterExecution (parentContext);
    }

    /// <inheritdoc cref="PftNode.GetNodeInfo" />
    public override PftNodeInfo GetNodeInfo()
    {
        var result = new PftNodeInfo
        {
            Node = this,
            Name = SimplifyTypeName (GetType().Name)
        };

        if (!ReferenceEquals (Mfn, null))
        {
            var mfnInfo = new PftNodeInfo
            {
                Node = Mfn,
                Name = "Mfn"
            };
            result.Children.Add (mfnInfo);
            mfnInfo.Children.Add (Mfn.GetNodeInfo());
        }

        var formatInfo = new PftNodeInfo
        {
            Name = "Format"
        };
        foreach (var node in Format)
        {
            formatInfo.Children.Add (node.GetNodeInfo());
        }

        result.Children.Add (formatInfo);

        return result;
    }

    /// <inheritdoc cref="PftNode.Optimize" />
    public override PftNode? Optimize()
    {
        if (Mfn is null)
        {
            return null;
        }

        Mfn = (PftNumeric?) Mfn.Optimize();
        Format.Optimize();

        if (Format.Count == 0)
        {
            return null;
        }

        return this;
    }

    /// <inheritdoc cref="PftNode.PrettyPrint" />
    public override void PrettyPrint
        (
            PftPrettyPrinter printer
        )
    {
        Sure.NotNull (printer);

        printer
            .SingleSpace()
            .Write ("ref(");
        Mfn?.PrettyPrint (printer);
        printer.Write (", ")
            .WriteNodes (Format)
            .Write (')');
    }

    /// <inheritdoc cref="PftNode.Serialize" />
    protected internal override void Serialize
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        base.Serialize (writer);

        PftSerializer.SerializeNullable (writer, Mfn);
        PftSerializer.Serialize (writer, Format);
    }

    /// <inheritdoc cref="PftNode.ShouldSerializeText" />
    protected internal override bool ShouldSerializeText() => false;

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.Append ("ref(");
        if (!ReferenceEquals (Mfn, null))
        {
            builder.Append (Mfn);
        }

        builder.Append (',');
        PftUtility.NodesToText (builder, Format);
        builder.Append (')');

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
} // class PftRef

// namespace ManagedIrbis.Pft.Infrastructure.Ast
