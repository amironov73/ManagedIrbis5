// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftA.cs --
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System.IO;

using AM;

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
    public sealed class PftA
        : PftCondition
    {
        #region Properties

        /// <summary>
        /// Field.
        /// </summary>
        public PftField? Field { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftA()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftA
            (
                PftToken token
            )
            : base(token)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftA
            (
                string text
            )
        {
            Field = new PftV(text);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftA
            (
                int tag
            )
        {
            Field = new PftV(tag);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftA
            (
                int tag,
                char code
            )
        {
            Field = new PftV(tag, code);
        }

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="PftNode.Clone" />
        public override object Clone()
        {
            var result = (PftA) base.Clone();

            if (!ReferenceEquals(Field, null))
            {
                result.Field = (PftField) Field.Clone();
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
                    ((PftA)otherNode).Field
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
                            "bool flag = !PftP.HaveGlobal(Context, {0}, "
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
                    .WriteLine("return flag;"); //-V3010
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
                    .WriteLine("return !flag;"); //-V3010
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
                        "PftA::Execute: "
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
                Value = !PftP.HaveGlobal
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

                    Value = !PftP.HaveRepeat
                        (
                            record,
                            tag.SafeToInt32(),
                            Field.SubField,
                            index
                        );

                    // Само по себе обращение к A крутит группу
                    // при наличии повторения поля

                    if (PftP.HaveRepeat
                        (
                            record,
                            tag.SafeToInt32(),
                            SubField.NoCode,
                            index
                        ))
                    {
                        context.OutputFlag = true;
                    }
                }
            }
            else
            {
                Magna.Error
                    (
                        "PftA::Execute: "
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

        /// <inheritdoc cref="PftNode.PrettyPrint" />
        public override void PrettyPrint
            (
                PftPrettyPrinter printer
            )
        {
            // Обрамляем пробелами
            printer
                .SingleSpace()
                .Write("a(");
            Field?.PrettyPrint(printer);
            printer.Write(')');
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

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        protected internal override bool ShouldSerializeText() => false;

        #endregion

        #region Object members

        /// <inheritdoc cref="PftNode.ToString" />
        public override string ToString() => "a(" + Field + ")";

        #endregion
    }
}
