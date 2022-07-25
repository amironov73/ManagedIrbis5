// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftF.cs -- функция F
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using AM;
using AM.Text;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast;

//
// Official documentation
//
// Функция F(выр-1,выр-2,выр-3)
//
// Функция F преобразует числовое значение
// из его внутреннего представления с плавающей
// точкой в символьную строку.
//
// Все три аргумента являются числовыми выражениями.
// * Первый аргумент - выр-1, является числом,
// которое необходимо преобразовать.
// * Второй аргумент  - выр-2, минимальная длина
// выходной строки, выделяемая для результата,
// * Третий аргумент  - выр-3, количество десятичных цифр.
//
// Второй и третий аргументы необязательны.
// Отметим, однако, что если присутствует выр-3,
// то выр-2 не может быть опущено.
//
// Выр-2 определяет минимальную длину, т.е.
// значением функции будет символьная строка длиной
// как минимум выр-2 символов, и если преобразуемое
// числовое значение требует выр-2 символов или меньше,
// оно будет выровнено по правой границе в пределах
// этой длины. Если количество символов, требуемое
// для представления значения выр-1, больше данной
// длины, то используются дополнительные позиции.
// В этом случае выходная строка будет длиннее,
// чем выр-2 символов.
//
// Выр-3 определяет количество десятичных цифр
// дробной части Выр-1.
// Если оно опущено, то результат будет представлен
// в экспоненциальной форме. Если при этом также
// опущено выр-2, то по умолчанию длина выходной
// строки будет равна 16 символам.
// Если выр-3 присутствует, то результатом будет
// округленное представление выр-1 с фиксированной
// точкой с выр-3 цифрами после десятичной точки.
//
// Если выр-3 равно нулю, то выр-1 округляется
// до ближайшего целого числа и результатом будет
// целое число без десятичной точки.
//
// Если при преобразовании целых чисел и чисел
// с фиксированной точкой оказывается,
// что целая часть числа слишком большая для
// ее представления, то выходная строка заменяется
// последовательностью символов "*".
//
// Функция F может использоваться для выравнивания
// колонки чисел по десятичной точке путем выбора
// соответствующей длины.
//
// Примеры функции F
//
// Выражение                       Значение
// ------------------------------- ----------------
// f(1)                            1.000000000E+00
// f(1,10)                         1.000E+00
// f(-1,10,2)                      -1.00
// f(1,5,2)                        1.00
// f(1,8,2)                        1.00
// f(mfn,1,0)                      4
// f(mfn,2,0)                      4
// f(mfn,3,0)                      4
//

//
// ibatrak
// F(0.5, 0, 0) выводит 0
// F(1.5, 0, 0) выводит 2
// F(2.5, 0, 0) выводит 2
// F(3.5, 0, 0) выводит 4
// F(4.5, 0, 0) выводит 4
//

/// <summary>
/// Функция F.
/// </summary>
public sealed class PftF
    : PftNode
{
    #region Properties

    /// <summary>
    /// First argument value.
    /// </summary>
    public PftNumeric? Argument1 { get; set; }

    /// <summary>
    /// Second argument value.
    /// </summary>
    public PftNumeric? Argument2 { get; set; }

    /// <summary>
    /// Third argument value.
    /// </summary>
    public PftNumeric? Argument3 { get; set; }

    /// <inheritdoc cref="PftNode.Children" />
    public override IList<PftNode> Children
    {
        get
        {
            if (ReferenceEquals (_virtualChildren, null))
            {
                _virtualChildren = new VirtualChildren();
                var nodes = new List<PftNode>();
                if (!ReferenceEquals (Argument1, null))
                {
                    nodes.Add (Argument1);
                }

                if (!ReferenceEquals (Argument2, null))
                {
                    nodes.Add (Argument2);
                }

                if (!ReferenceEquals (Argument3, null))
                {
                    nodes.Add (Argument3);
                }

                _virtualChildren.SetChildren (nodes);
            }

            return _virtualChildren;
        }

        [ExcludeFromCodeCoverage]
        protected set
        {
            // Nothing to do here

            Magna.Logger.LogError
                (
                    nameof (PftF) + nameof (Children)
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
    public PftF()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftF
        (
            PftToken token
        )
        : base (token)
    {
    }

    #endregion

    #region Private members

    private VirtualChildren? _virtualChildren;

    #endregion

    #region ICloneable members

    /// <inheritdoc cref="ICloneable.Clone" />
    public override object Clone()
    {
        var result = (PftF)base.Clone();

        result._virtualChildren = null;

        if (Argument1 is not null)
        {
            result.Argument1 = (PftNumeric) Argument1.Clone();
        }

        if (Argument2 is not null)
        {
            result.Argument2 = (PftNumeric) Argument2.Clone();
        }

        if (Argument3 is not null)
        {
            result.Argument3 = (PftNumeric) Argument3.Clone();
        }

        return result;
    }

    #endregion

    #region PftNode members

    /// <inheritdoc cref="PftNode.CompareNode"/>
    [ExcludeFromCodeCoverage]
    internal override void CompareNode
        (
            PftNode otherNode
        )
    {
        Sure.NotNull (otherNode);

        base.CompareNode (otherNode);

        var otherF = (PftF)otherNode;
        PftSerializationUtility.CompareNodes
            (
                Argument1,
                otherF.Argument1
            );
        PftSerializationUtility.CompareNodes
            (
                Argument2,
                otherF.Argument2
            );
        PftSerializationUtility.CompareNodes
            (
                Argument3,
                otherF.Argument3
            );
    }

    /// <inheritdoc cref="PftNode.Compile" />
    public override void Compile
        (
            PftCompiler compiler
        )
    {
        Sure.NotNull (compiler);

        if (Argument1 is null)
        {
            throw new PftCompilerException();
        }

        Argument1.Compile (compiler);
        if (Argument2 is not null)
        {
            Argument2.Compile (compiler);
        }

        if (Argument3 is not null)
        {
            Argument3.Compile (compiler);
        }

        compiler.StartMethod (this);

        compiler
            .WriteIndent()
            .Write ("double value = ")
            .CallNodeMethod (Argument1);

        compiler
            .WriteIndent()
            .Write ("int minWidth = ");
        if (ReferenceEquals (Argument2, null))
        {
            compiler.WriteLine ("-1;");
        }
        else
        {
            compiler
                .Write ("(int)")
                .CallNodeMethod (Argument2);
        }

        compiler
            .WriteIndent()
            .Write ("int decimalPoints = ");
        if (ReferenceEquals (Argument3, null))
        {
            compiler.WriteLine ("-1;");
        }
        else
        {
            compiler
                .Write ("(int)")
                .CallNodeMethod (Argument3);
        }

        compiler
            .WriteIndent()
            .WriteLine ("string text = PftUtility.FormatLikeF(value, minWidth, decimalPoints);")
            .WriteIndent()
            .WriteLine ("Context.Write(null, text);");

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

        Argument1 = (PftNumeric?)PftSerializer.DeserializeNullable (reader);
        Argument2 = (PftNumeric?)PftSerializer.DeserializeNullable (reader);
        Argument3 = (PftNumeric?)PftSerializer.DeserializeNullable (reader);
    }

    /// <inheritdoc cref="PftNode.Execute" />
    public override void Execute
        (
            PftContext context
        )
    {
        Sure.NotNull (context);

        OnBeforeExecution (context);

        if (Argument1 is null)
        {
            Magna.Logger.LogError
                (
                    nameof (PftF) + "::" + nameof (Execute)
                    + ": argument1 is not set"
                );

            throw new PftException ("Argument1 is null");
        }

        using (var guard = new PftContextGuard (context))
        {
            var clone = guard.ChildContext;

            Argument1.Execute (clone);
            var value = Argument1.Value;

            var minWidth = -1;
            if (!ReferenceEquals (Argument2, null))
            {
                clone = guard.PushAgain();
                Argument2.Execute (clone);
                minWidth = (int)Argument2.Value;
            }

            var decimalPoints = -1;
            if (!ReferenceEquals (Argument3, null))
            {
                clone = guard.PushAgain();
                Argument3.Execute (clone);
                decimalPoints = (int)Argument3.Value;
            }

            var result = PftUtility.FormatLikeF
                (
                    value,
                    minWidth,
                    decimalPoints
                );
            context.Write (this, result);
        }

        // Doesn't touch context.OutputFlag

        OnAfterExecution (context);
    }

    /// <inheritdoc cref="PftNode.GetNodeInfo" />
    public override PftNodeInfo GetNodeInfo()
    {
        var result = new PftNodeInfo
        {
            Node = this,
            Name = SimplifyTypeName (GetType().Name)
        };

        if (Argument1 is not null)
        {
            var node = new PftNodeInfo
            {
                Node = Argument1,
                Name = "Argument1"
            };
            result.Children.Add (node);
            node.Children.Add (Argument1.GetNodeInfo());
        }

        if (Argument2 is not null)
        {
            var node = new PftNodeInfo
            {
                Node = Argument2,
                Name = "Argument2"
            };
            result.Children.Add (node);
            node.Children.Add (Argument2.GetNodeInfo());
        }

        if (Argument3 is not null)
        {
            var node = new PftNodeInfo
            {
                Node = Argument3,
                Name = "Argument3"
            };
            result.Children.Add (node);
            node.Children.Add (Argument3.GetNodeInfo());
        }

        return result;
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
            .Write ("f(");
        if (!ReferenceEquals (Argument1, null))
        {
            Argument1.PrettyPrint (printer);
        }

        if (!ReferenceEquals (Argument2, null))
        {
            printer.EatWhitespace();
            printer.Write (", ");
            Argument2.PrettyPrint (printer);
        }

        if (!ReferenceEquals (Argument3, null))
        {
            printer.EatWhitespace();
            printer.Write (", ");
            Argument3.PrettyPrint (printer);
        }

        printer.EatWhitespace();
        printer.Write (')');
    }

    /// <inheritdoc cref="PftNode.Serialize" />
    protected internal override void Serialize
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        base.Serialize (writer);

        PftSerializer.SerializeNullable (writer, Argument1);
        PftSerializer.SerializeNullable (writer, Argument2);
        PftSerializer.SerializeNullable (writer, Argument3);
    }

    /// <inheritdoc cref="PftNode.ShouldSerializeText" />
    protected internal override bool ShouldSerializeText() => false;

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.Append ("f(");
        if (!ReferenceEquals (Argument1, null))
        {
            builder.Append (Argument1);
        }

        if (!ReferenceEquals (Argument2, null))
        {
            builder.Append (',');
            builder.Append (Argument2);
        }

        if (!ReferenceEquals (Argument3, null))
        {
            builder.Append (',');
            builder.Append (Argument3);
        }

        builder.Append (')');

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
