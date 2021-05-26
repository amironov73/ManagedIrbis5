// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftG.cs -- обращение к глобальной переменной
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM;
using AM.IO;

using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Serialization;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    /// Обращение к глобальной переменной.
    /// </summary>
    public sealed class PftG
        : PftField
    {
        #region Properties

        /// <summary>
        /// Number of the variable.
        /// </summary>
        public int Number { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftG()
        {
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftG
            (
                PftToken token
            )
            : base(token)
        {
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftG
            (
                string text
            )
        {
            var specification = new FieldSpecification();
            if (!specification.Parse(text))
            {
                throw new PftSyntaxException();
            }

            Apply(specification);

            Number = Tag.ThrowIfNull("Tag").ParseInt32();
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftG
            (
                int number
            )
        {
            Command = 'g';
            Number = number;
            Tag = number.ToInvariantString();
        } // constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftG
            (
                int number,
                char subField
            )
        {
            Command = 'g';
            Number = number;
            Tag = number.ToInvariantString();
            SubField = subField;
        } // constructor

        #endregion

        #region Private members

        private int _count;

        private void _Execute
            (
                PftContext context
            )
        {
            try
            {
                context.CurrentField = this;

                context.Execute(LeftHand);

                var value = GetValue(context);
                if (!string.IsNullOrEmpty(value))
                {
                    if (Indent != 0
                        && IsFirstRepeat(context))
                    {
                        value = new string(' ', Indent) + value;
                    }

                    context.Write(this, value);
                }

                if (HaveRepeat(context))
                {
                    context.OutputFlag = true;
                    context.VMonitor = true;
                }

                context.Execute(RightHand);
            }
            finally
            {
                context.CurrentField = null;
            }
        }// method _Execute

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
            var result = context.Globals.Get(Number).Length;

            return result;
        } // method GetCount

        /// <summary>
        /// Get value.
        /// </summary>
        public override string? GetValue
            (
                PftContext context
            )
        {
            if (_count == 0)
            {
                return null;
            }

            var index = context.Index;

            var fields = context.Globals.Get(Number);
            if (fields.Length == 0)
            {
                return null;
            }

            fields = PftUtility.GetArrayItem
                (
                    context,
                    fields,
                    FieldRepeat
                );
            if (fields.Length == 0)
            {
                return null;
            }

            var field = fields.GetOccurrence(index);
            if (ReferenceEquals(field, null))
            {
                return null;
            }

            string? result;

            if (SubField == NoSubField)
            {
                result = field.FormatField
                    (
                        context.FieldOutputMode,
                        context.UpperMode
                    );
            }
            else if (SubField == '*')
            {
                result = field.GetValueOrFirstSubField();
            }
            else
            {
                result = field.GetFirstSubFieldValue(SubField);
            }

            result = LimitText(result);

            return result;
        } // method GetValue

        /// <summary>
        /// Have value?
        /// </summary>
        public override bool HaveRepeat(PftContext context) =>
            context.Index < GetCount(context);

        #endregion

        #region PftField members

        /// <inheritdoc cref="PftField.IsLastRepeat" />
        public override bool IsLastRepeat(PftContext context) =>
            context.Index >= _count - 1;

        #endregion

        #region PftNode members

        /// <inheritdoc cref="PftNode.CompareNode" />
        internal override void CompareNode
            (
                PftNode otherNode
            )
        {
            base.CompareNode(otherNode);

            if (Number != ((PftG) otherNode).Number)
            {
                throw new PftSerializationException();
            }
        } // method CompareNode

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            if (Number == 0)
            {
                Number = Tag.ThrowIfNull("Tag").ParseInt32();
            }

            var info = compiler.CompileField(this);

            compiler.CompileNodes(LeftHand);
            compiler.CompileNodes(RightHand);

            var leftHand = compiler.CompileAction(LeftHand) ?? "null";
            var rightHand = compiler.CompileAction(RightHand) ?? "null";

            compiler.StartMethod(this);

            compiler
                .WriteIndent()
                .WriteLine("Action leftHand = {0};", leftHand);

            compiler
                .WriteIndent()
                .WriteLine("Action rightHand = {0};", rightHand);

            compiler
                .WriteIndent()
                .WriteLine
                    (
                        "DoGlobal({0}, {1}, leftHand, rightHand);",
                        Number.ToInvariantString(),
                        info.Reference
                    );

            compiler.EndMethod(this);
            compiler.MarkReady(this);
        } // method Compile

        /// <inheritdoc cref="PftNode.Deserialize" />
        protected internal override void Deserialize
            (
                BinaryReader reader
            )
        {
            base.Deserialize(reader);

            Number = reader.ReadPackedInt32();
        } // method Deserialize

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
                        nameof(PftG) + "::" + nameof(Execute)
                        + ": nested field detected"
                    );

                throw new PftSemanticException("nested field");
            }

            if (Number == 0)
            {
                Number = Tag.ThrowIfNull("Tag").ParseInt32();
            }

            if (!ReferenceEquals(context.CurrentGroup, null))
            {
                if (IsFirstRepeat(context))
                {
                    _count = GetCount(context);
                }

                _Execute(context);
            }
            else
            {
                _count = GetCount(context);

                context.DoRepeatableAction(_Execute);
            }

            OnAfterExecution(context);
        } // method Execute

        /// <inheritdoc cref="PftField.GetAffectedFields" />
        public override int[] GetAffectedFields() => Array.Empty<int>();

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            writer.WritePackedInt32(Number);
        } // method Serialize

        #endregion

    } // class PftG

} // namespace ManagedIrbis.Pft.Infrastructure.Ast
