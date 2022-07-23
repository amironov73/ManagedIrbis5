// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftBreak.cs -- досрочное прерывание цикла
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

using AM;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast;

/*
 * Как работает оператор break в ИРБИС64:
 *
 * Вне повторяющейся группы он немедленно прекращает
 * выполнение текущего контекста (управление переходит
 * родительскому контексту, если таковой существует).
 * При этом неважно, находится ли он внутри операторных
 * скобок IF/FI.
 *
 * Внутри повторяющейся группы он по-разному работает
 * в зависимости от того, находится ли он внутри
 * операторных скобок IF/FI.
 *
 * Внутри IF/FI он выставляет флаг "не надо следующего
 * повторения группы". Вне IF/FI он просто прекращает
 * выполнение текущего контекста (см. выше).
 *
 * Есть маленькая тонкость: break останавливает повторяющуюся
 * группу, только если находится на первом логическом уровне
 * после IF, иначе она срабатывает как вне IF/FI.
 *
 * Пример:
 *
 * 'first',/,
 * (
 * 'second',/,
 * if 1=1 then break fi,
 * v300,/,
 * 'third',/,
 * ),
 * 'fourth',
 *
 * Выведет:
 *
 * first
 * second
 * Указ.
 * third
 * second
 * third
 * fourth
 *
 */

/// <summary>
/// Досрочное прерывание цикла.
/// </summary>
public sealed class PftBreak
    : PftNode
{
    #region Properties

    /// <inheritdoc cref="PftNode.ConstantExpression" />
    public override bool ConstantExpression => true;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PftBreak()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftBreak
        (
            PftToken token
        )
        : base (token)
    {
        token.MustBe (PftTokenKind.Break);
    }

    #endregion

    #region PftNode members

    /// <inheritdoc cref="PftNode.Compile" />
    public override void Compile
        (
            PftCompiler compiler
        )
    {
        Sure.NotNull (compiler);

        compiler.StartMethod (this);

        compiler
            .WriteIndent()
            .WriteLine ("if (InGroup)")
            .WriteIndent()
            .WriteLine ("{")
            .IncreaseIndent()
            .WriteIndent()
            .WriteLine ("if (PftConfig.BreakImmediate)")
            .WriteIndent()
            .WriteLine ("{")
            .IncreaseIndent()
            .WriteIndent()
            .WriteLine ("throw new PftBreakException(null);")
            .DecreaseIndent()
            .WriteIndent()
            .WriteLine ("}")
            .WriteIndent()
            .WriteLine ("else")
            .WriteIndent()
            .WriteLine ("{")
            .IncreaseIndent()
            .WriteIndent()
            .WriteLine ("Context.BreakFlag = true;")
            .DecreaseIndent()
            .WriteIndent()
            .WriteLine ("}")
            .WriteIndent()
            .WriteLine ("}")
            .WriteIndent()
            .WriteLine ("else")
            .WriteIndent()
            .WriteLine ("{")
            .IncreaseIndent()
            .WriteIndent()
            .WriteLine ("throw new PftBreakException(null);")
            .DecreaseIndent()
            .WriteIndent()
            .WriteLine ("}"); //-V3010

        compiler.EndMethod (this);
        compiler.MarkReady (this);
    }

    /// <inheritdoc cref="PftNode.Execute" />
    public override void Execute
        (
            PftContext context
        )
    {
        Sure.NotNull (context);

        OnBeforeExecution (context);

        if (!ReferenceEquals (context.CurrentGroup, null))
        {
            // Мы внутри группы
            var handled = false;

            if (PftConfig.BreakImmediate)
            {
                Magna.Logger.LogError
                    (
                        nameof (PftBreak) + "::" + nameof (Execute)
                        + ": break inside the group"
                    );

                throw new PftBreakException (this);
            }

            var possibleCondition = Parent;
            if (possibleCondition is PftConditionalStatement)
            {
                context.BreakFlag = true;
                handled = true;
            }

            if (!handled)
            {
                throw new PftBreakException (this);
            }
        }
        else
        {
            // Это не группа, а оператор for
            // или что-нибудь в этом роде

            Magna.Logger.LogError
                (
                    nameof (PftBreak) + "::" + nameof (Execute)
                    + ": break outside the group"
                );

            throw new PftBreakException (this);
        }

        OnAfterExecution (context);
    }

    /// <inheritdoc cref="PftNode.PrettyPrint" />
    public override void PrettyPrint
        (
            PftPrettyPrinter printer
        )
    {
        printer
            .SingleSpace()
            .Write ("break")
            .SingleSpace();
    }

    /// <inheritdoc cref="PftNode.ShouldSerializeText" />
    [ExcludeFromCodeCoverage]
    protected internal override bool ShouldSerializeText() => false;

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return "break";
    }

    #endregion
}
