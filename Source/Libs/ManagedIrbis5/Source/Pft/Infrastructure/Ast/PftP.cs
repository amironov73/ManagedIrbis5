// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftP.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using AM;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Compiler;
using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;
using ManagedIrbis.Pft.Infrastructure.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    ///
    /// </summary>
    public sealed class PftP
        : PftCondition
    {
        #region Properties

        /// <summary>
        /// Field.
        /// </summary>
        public PftField? Field { get; set; }

        /// <inheritdoc cref="PftNode.Children" />
        public override IList<PftNode> Children
        {
            get
            {
                if (ReferenceEquals(_virtualChildren, null))
                {
                    _virtualChildren = new VirtualChildren();
                    var nodes = new List<PftNode>();
                    if (!ReferenceEquals(Field, null))
                    {
                        nodes.Add(Field);
                    }
                    _virtualChildren.SetChildren(nodes);
                }

                return _virtualChildren;
            }

            [ExcludeFromCodeCoverage]
            protected set => Magna.Error
                (
                    "PftP::Children: "
                    + "set value="
                    + value.ToVisibleString()
                );
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftP()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftP
            (
                PftToken token
            )
            : base(token)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftP
            (
                string text
            )
        {
            // TODO support for G
            Field = new PftV(text);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftP
            (
                int tag
            )
        {
            Field = new PftV(tag);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftP
            (
                int tag,
                char code
            )
        {
            Field = new PftV(tag, code);
        }

        #endregion

        #region Private members

        private VirtualChildren? _virtualChildren;

        /// <summary>
        /// Limit text.
        /// </summary>
        public static string? LimitText
            (
                FieldSpecification field,
                string? text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            var offset = field.Offset;
            var length = 100000000;
            if (field.Length != 0)
            {
                length = field.Length;
            }

            var result = PftUtility.SafeSubString
                (
                    text,
                    offset,
                    length
                );

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Have the specified global repeat?
        /// </summary>
        public static bool HaveGlobal
            (
                PftContext context,
                FieldSpecification specification,
                int number,
                int index
            )
        {
            var fields = context.Globals.Get(number);
            var field = fields.GetOccurrence(index);
            if (!ReferenceEquals(field, null))
            {
                var code = specification.SubField;
                string? result;

                if (code == SubField.NoCode)
                {
                    result = field.FormatField
                    (
                        context.FieldOutputMode,
                        context.UpperMode
                    );
                }
                else if (code == '*')
                {
                    result = field.GetValueOrFirstSubField();
                }
                else
                {
                    result = field.GetFirstSubFieldValue(code);
                }

                result = LimitText(specification, result);
                return !string.IsNullOrEmpty(result);
            }

            return false;
        }

        /// <summary>
        /// Have the specified field repeat?
        /// </summary>
        public static bool HaveRepeat
            (
                Record record,
                int tag,
                char code,
                int index
            )
        {
            if (tag == IrbisGuid.Tag)
            {
                // Поле GUID всегда считается отсуствующим
                return false;
            }

            var field = record.Fields.GetField
                (
                    tag,
                    index
                );
            if (ReferenceEquals(field, null))
            {
                return false;
            }

            if (code == SubField.NoCode)
            {
                return true;
            }

            var result = field.HaveSubField(code);

            return result;
        }

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public override object Clone()
        {
            var result = (PftP)base.Clone();

            if (!ReferenceEquals(Field, null))
            {
                result.Field = (PftField)Field.Clone();
            }

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
            base.CompareNode(otherNode);

            PftSerializationUtility.CompareNodes
                (
                    Field,
                    ((PftP)otherNode).Field
                );
        }

        /// <inheritdoc cref="PftNode.Compile" />
        public override void Compile
            (
                PftCompiler compiler
            )
        {
            if (ReferenceEquals(Field, null))
            {
                throw new PftCompilerException();
            }

            var info = compiler.CompileField(Field);

            compiler.StartMethod(this);

            if (Field.Command == 'g')
            {
                var number = FastNumber.ParseInt32(Field.Tag.ThrowIfNull());
                compiler
                    .WriteIndent()
                    .WriteLine
                        (
                            "bool flag = PftP.HaveGlobal(Context, {0}, "
                            + "{1}, Context.Index);",
                            info.Reference,
                            number.ToInvariantString()
                        );

                compiler
                    .WriteIndent()
                    .WriteLine
                        (
                            "RecordField[] fields = Context.Globals.Get({0});",
                            number.ToInvariantString()
                        )
                    .WriteIndent()
                    .WriteLine("if (Context.Index <= fields.Length)")
                    .WriteIndent()
                    .WriteLine("{")
                    .IncreaseIndent()
                    .WriteIndent()
                    .WriteLine("HaveOutput()")
                    .DecreaseIndent()
                    .WriteIndent()
                    .WriteLine("}")
                    .WriteIndent()
                    .WriteLine("return flag;");
            }
            else
            {
                compiler
                    .WriteIndent()
                    .WriteLine("MarcRecord record = Context.Record;")
                    .WriteIndent()
                    .WriteLine("string tag = {0}.Tag;", info.Reference)
                    .WriteIndent()
                    .WriteLine
                        (
                            "bool flag = PftP.HaveRepeat(record, "
                            + "tag, {0}.SubField, Context.Index);",
                            info.Reference
                        )
                    .WriteIndent()
                    .WriteLine("if (PftP.HaveRepeat(record, "
                               + "tag, SubField.NoCode, Context.Index))")
                    .WriteIndent()
                    .WriteLine("{")
                    .IncreaseIndent()
                    .WriteIndent()
                    .WriteLine("HaveOutput();")
                    .DecreaseIndent()
                    .WriteIndent()
                    .WriteLine("}")
                    .WriteIndent()
                    .WriteLine("return flag;");
            }

            compiler.EndMethod(this);
            compiler.MarkReady(this);
        }

        /// <inheritdoc cref="PftNode.Deserialize" />
        protected internal override void Deserialize
            (
                BinaryReader reader
            )
        {
            base.Deserialize(reader);

            Field = (PftField?) PftSerializer.DeserializeNullable(reader);
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            if (ReferenceEquals(Field, null))
            {
                Magna.Error
                    (
                        "PftP::Execute: "
                        + "Field not specified"
                    );

                throw new PftSyntaxException(this);
            }

            var tag = Field.Tag.ThrowIfNull("Field.Tag");
            var record = context.Record;
            var index = context.Index;

            if (Field.Command == 'g')
            {
                var number = FastNumber.ParseInt32(tag);
                var fields = context.Globals.Get(number);
                Value = HaveGlobal
                    (
                        context,
                        Field.ToSpecification(),
                        number,
                        index
                    );

                if (index <= fields.Length)
                {
                    context.OutputFlag = true;
                }
            }
            else if (Field.Command == 'v')
            {
                if (!ReferenceEquals(record, null))
                {
                    // ИРБИС64 вне группы всегда проверяет
                    // на наличие лишь первое повторение поля!

                    Value = HaveRepeat
                        (
                            record,
                            tag.SafeToInt32(),
                            Field.SubField,
                            index
                        );

                    // Само по себе обращение к P крутит группу
                    // при наличии повторения поля

                    if (HaveRepeat(record, tag.SafeToInt32(), SubField.NoCode, index))
                    {
                        context.OutputFlag = true;
                    }
                }
            }
            else
            {
                Magna.Error
                    (
                        "PftP::Execute: "
                        + "unexpected command: "
                        + Field.Command
                    );

                throw new PftSyntaxException
                    (
                        "unexpected command "
                        + Field.Command
                    );
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.GetNodeInfo" />
        public override PftNodeInfo GetNodeInfo()
        {
            var result = new PftNodeInfo
            {
                Node = this,
                Name = SimplifyTypeName(GetType().Name)
            };

            if (!ReferenceEquals(Field, null))
            {
                var fieldInfo = new PftNodeInfo
                {
                    Node = Field,
                    Name = "Field"
                };
                fieldInfo.Children.Add(Field.GetNodeInfo());
                result.Children.Add(fieldInfo);
            }

            return result;
        }

        /// <inheritdoc cref="PftNode.Serialize" />
        protected internal override void Serialize
            (
                BinaryWriter writer
            )
        {
            base.Serialize(writer);

            PftSerializer.SerializeNullable(writer, Field);
        }

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            printer
                .SingleSpace()
                .Write("p(");
            Field?.PrettyPrint(printer);
            printer.Write(')');
        }

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        protected internal override bool ShouldSerializeText() => false;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => "p(" + Field + ")";

        #endregion
    }
}

