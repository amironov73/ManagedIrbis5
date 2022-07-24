// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftV.cs -- ссылка на поле
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using ManagedIrbis.Infrastructure;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast;

/// <summary>
/// Ссылка на поле.
/// </summary>
public sealed class PftV
    : PftField
{
    #region Properties

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public PftV()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftV
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
                    nameof (PftV) + "::Constructor"
                    + ": text={Text}",
                    text.ToVisibleString()
                );

            throw new PftSyntaxException();
        }

        Apply (specification);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftV
        (
            PftToken token
        )
        : base (token)
    {
        Sure.NotNull (token);

        token.MustBe (PftTokenKind.V);

        var specification = ((FieldSpecification?) token.UserData).ThrowIfNull();
        Apply (specification);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftV
        (
            int tag,
            char code
        )
    {
        Sure.Positive (tag);

        var specification = new FieldSpecification (tag, code)
        {
            Command = 'v'
        };
        Apply (specification);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public PftV
        (
            int tag
        )
    {
        Sure.Positive (tag);

        var specification = new FieldSpecification (tag)
        {
            Command = 'v'
        };
        Apply (specification);
    }

    #endregion

    #region Private members

    private int _count;

    private void _Execute
        (
            PftContext context
        )
    {
        Sure.NotNull (context);

        try
        {
            context.CurrentField = this;

            context.Execute (LeftHand);

            var value = GetValue (context);
            if (!string.IsNullOrEmpty (value))
            {
                if (Indent != 0
                    && IsFirstRepeat (context))
                {
                    value = new string (' ', Indent) + value;
                }

                if (context.UpperMode)
                {
                    value = IrbisText.ToUpper (value);
                }

                context.Write (this, value);
            }

            if (HaveRepeat (context))
            {
                context.OutputFlag = true;
                context.VMonitor = true;
            }

            context.Execute (RightHand);
        }
        finally
        {
            context.CurrentField = null;
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Число повторений _поля_ в записи.
    /// </summary>
    public int GetCount
        (
            PftContext context
        )
    {
        Sure.NotNull (context);

        var record = context.Record;
        if (ReferenceEquals (record, null)
            || string.IsNullOrEmpty (Tag))
        {
            return 0;
        }

        var result = record.Fields.GetField (Tag.SafeToInt32()).Length;

        return result;
    }

    #endregion

    #region PftField members

    /// <inheritdoc cref="PftField.IsLastRepeat" />
    public override bool IsLastRepeat
        (
            PftContext context
        )
    {
        Sure.NotNull (context);

        return context.Index >= _count - 1;
    }

    #endregion

    #region PftNode members

    /// <inheritdoc cref="PftNode.Execute" />
    public override void Execute
        (
            PftContext context
        )
    {
        Sure.NotNull (context);

        OnBeforeExecution (context);

        if (Tag != IrbisGuid.TagString)
        {
            // Поле GUID не выводится

            if (!ReferenceEquals (context.CurrentField, null))
            {
                Magna.Logger.LogError
                    (
                        nameof (PftV) + "::" + nameof (Execute)
                        + ": nested field detected"
                    );

                throw new PftSemanticException ("nested field");
            }

            if (!ReferenceEquals (context.CurrentGroup, null))
            {
                if (IsFirstRepeat (context))
                {
                    _count = GetCount (context);
                }

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

                _count = GetCount (childContext);
                childContext.DoRepeatableAction (_Execute);
            }
        }

        OnAfterExecution (context);
    }

    #endregion
}
