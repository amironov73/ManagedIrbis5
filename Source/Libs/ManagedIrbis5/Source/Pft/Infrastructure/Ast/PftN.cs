// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftN.cs -- fake field reference
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast;

/// <summary>
/// Fake field reference.
/// </summary>
public sealed class PftN
    : PftField
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PftN()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftN
        (
            PftToken token
        )
        : base (token)
    {
        token.MustBe (PftTokenKind.V);

        var specification = ((FieldSpecification?) token.UserData).ThrowIfNull();
        Apply (specification);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftN
        (
            string text
        )
    {
        Sure.NotNullNorEmpty (text);

        var specification = new FieldSpecification();
        if (!specification.Parse (text))
        {
            Magna.Logger.LogError
                (
                    nameof (PftN) + "::Constructor"
                    + ": syntax error at {Token}",
                    text.ToVisibleString()
                );

            throw new PftSyntaxException (this);
        }

        Apply (specification);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftN
        (
            int tag,
            char code
        )
    {
        Sure.Positive (tag);

        var specification = new FieldSpecification (tag, code)
        {
            Command = 'n'
        };
        Apply (specification);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftN
        (
            int tag
        )
    {
        Sure.Positive (tag);

        var specification = new FieldSpecification (tag)
        {
            Command = 'n'
        };
        Apply (specification);
    }

    #endregion

    #region Private members

    private void _Execute
        (
            PftContext context
        )
    {
        try
        {
            context.CurrentField = this;

            var value = GetValue (context);
            if (string.IsNullOrEmpty (value))
            {
                context.Execute (LeftHand);
            }
        }
        finally
        {
            context.CurrentField = null;
        }
    }

    #endregion

    #region PftNode members

    /// <inheritdoc cref="PftField.CanOutput" />
    public override bool CanOutput
        (
            string? value
        )
    {
        return string.IsNullOrEmpty (value);
    }

    /// <inheritdoc cref="PftNode.Execute" />
    public override void Execute
        (
            PftContext context
        )
    {
        Sure.NotNull (context);

        OnBeforeExecution (context);

        if (context.CurrentField is not null)
        {
            Magna.Logger.LogError
                (
                    nameof (PftN) + "::" + nameof (Execute)
                    + ": nested field detected at {Token}",
                    this
                );

            throw new PftSemanticException ("nested field detected at " + this);
        }

        if (LeftHand.Count == 0)
        {
            Magna.Logger.LogWarning
                (
                    nameof (PftN) + "::" + nameof (Execute)
                    + ": no left hand nodes at {Token}",
                    this
                );
        }

        if (!ReferenceEquals (context.CurrentGroup, null))
        {
            _Execute (context);
        }
        else
        {
            var childContext = new PftContext (context)
            {
                FieldOutputMode = context.FieldOutputMode,
                UpperMode = context.UpperMode,
                Output = context.Output
            };

            childContext.DoRepeatableAction (_Execute, 1);
        }

        OnAfterExecution (context);
    }

    #endregion
}
