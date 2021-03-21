// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftHave.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using AM.IO;

using ManagedIrbis.Pft.Infrastructure.Diagnostics;
using ManagedIrbis.Pft.Infrastructure.Serialization;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Ast
{
    /// <summary>
    ///
    /// </summary>
    public sealed class PftHave
        : PftCondition
    {
        #region Properties

        /// <summary>
        /// Variable reference.
        /// </summary>
        public PftVariableReference? Variable { get; set; }

        /// <summary>
        /// Function or procedure name.
        /// </summary>
        public string? Identifier { get; set; }

        /// <inheritdoc cref="PftNode.ExtendedSyntax" />
        public override bool ExtendedSyntax => true;

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftHave()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftHave
            (
                PftToken token
            )
            : base(token)
        {
            token.MustBe(PftTokenKind.Have);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftHave
            (
                string name,
                bool variable
            )
        {
            if (variable)
            {
                Variable = new PftVariableReference(name);
            }
            else
            {
                Identifier = name;
            }
        }

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="PftNode.Clone" />
        public override object Clone()
        {
            PftHave result = (PftHave) base.Clone();

            if (!ReferenceEquals(Variable, null))
            {
                result.Variable = (PftVariableReference) Variable.Clone();
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

            PftHave otherHave = (PftHave)otherNode;
            PftSerializationUtility.CompareNodes
                (
                    Variable,
                    otherHave.Variable
                );
            PftSerializationUtility.CompareStrings
                (
                    Identifier,
                    otherHave.Identifier
                );
        }

        /// <inheritdoc cref="PftNode.Deserialize" />
        protected internal override void Deserialize
            (
                BinaryReader reader
            )
        {
            base.Deserialize(reader);

            Variable = (PftVariableReference?)
                PftSerializer.DeserializeNullable(reader);
            Identifier = reader.ReadNullableString();
        }

        /// <inheritdoc cref="PftNode.Execute" />
        public override void Execute
            (
                PftContext context
            )
        {
            OnBeforeExecution(context);

            Value = false;

            string? name;

            if (!ReferenceEquals(Variable, null))
            {
                name = Variable.Name;
                if (!string.IsNullOrEmpty(name))
                {
                    Value = !ReferenceEquals
                        (
                            context.Variables.GetExistingVariable(name),
                            null
                        );
                }
            }
            else
            {
                name = Identifier;
                if (!string.IsNullOrEmpty(name))
                {
                    Value = context.Functions.HaveFunction(name)
                        || context.Procedures.HaveProcedure(name);
                }
            }

            OnAfterExecution(context);
        }

        /// <inheritdoc cref="PftNode.GetNodeInfo" />
        public override PftNodeInfo GetNodeInfo()
        {
            PftNodeInfo result = new PftNodeInfo
            {
                Node = this,
                Name = SimplifyTypeName(GetType().Name)
            };

            if (!ReferenceEquals(Variable, null))
            {
                result.Children.Add(Variable.GetNodeInfo());
            }

            if (!ReferenceEquals(Identifier, null))
            {
                result.Children.Add(new PftNodeInfo
                {
                    Name = Identifier
                });
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

            PftSerializer.SerializeNullable(writer, Variable);
            writer.WriteNullable(Identifier);
        }

        /// <inheritdoc cref="PftNode.ShouldSerializeText" />
        protected internal override bool ShouldSerializeText() => false;

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            if (!ReferenceEquals(Variable, null))
            {
                return "have(" + Variable + ")";
            }

            return "have(" + Identifier + ")";
        }

        #endregion
    }
}
