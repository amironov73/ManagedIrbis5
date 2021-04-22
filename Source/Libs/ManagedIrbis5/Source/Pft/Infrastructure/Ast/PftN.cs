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

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Fake field reference.
    /// </summary>
    public sealed class PftN
        : PftField
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftN()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftN
            (
                PftToken token
            )
            : base(token)
        {
            token.MustBe(PftTokenKind.V);

            var specification = ((FieldSpecification?) token.UserData).ThrowIfNull("token.UserData");
            Apply(specification);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftN
            (
                string text
            )
        {
            var specification = new FieldSpecification();
            if (!specification.Parse(text))
            {
                Magna.Error
                    (
                        "PftN::Constructor: "
                        + "syntax error at:"
                        + text.ToVisibleString()
                    );

                throw new PftSyntaxException();
            }

            Apply(specification);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftN
            (
                int tag,
                char code
            )
        {
            var specification = new FieldSpecification(tag, code)
            {
                Command = 'n'
            };
            Apply(specification);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftN
            (
                int tag
            )
        {
            var specification = new FieldSpecification(tag)
            {
                Command = 'n'
            };
            Apply(specification);
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

                var value = GetValue(context);
                if (string.IsNullOrEmpty(value))
                {
                    context.Execute(LeftHand);
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
            return string.IsNullOrEmpty(value);
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (!ReferenceEquals(context.CurrentField, null))
            {
                Magna.Error
                    (
                        "PftN::Execute: "
                        + "nested field detected"
                    );

                throw new PftSemanticException("nested field");
            }

            if (LeftHand.Count == 0)
            {
                Magna.Warning
                    (
                        "PftN::Execute: "
                        + "no left hand nodes"
                    );
            }

            if (!ReferenceEquals(context.CurrentGroup, null))
            {
                _Execute(context);
            }
            else
            {
                var childContext = new PftContext(context)
                {
                    FieldOutputMode = context.FieldOutputMode,
                    UpperMode = context.UpperMode,
                    Output = context.Output
                };

                childContext.DoRepeatableAction(_Execute, 1);
            }

            OnAfterExecution(context);
        }

        #endregion
    }
}
