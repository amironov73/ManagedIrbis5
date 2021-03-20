// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftProcedure.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM;
using AM.IO;

using ManagedIrbis.Pft.Infrastructure.Serialization;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Procedure.
    /// </summary>
    public sealed class PftProcedure
        : ICloneable
    {
        #region Properties

        /// <summary>
        /// Procedure body.
        /// </summary>
        public PftNodeCollection Body { get; set; }

        /// <summary>
        /// Procedure name.
        /// </summary>
        public string? Name { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PftProcedure()
        {
            // TODO fake parent?

            Body = new PftNodeCollection(null);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Deserialize the procedure.
        /// </summary>
        public void Deserialize
            (
                BinaryReader reader
            )
        {
            Name = reader.ReadNullableString();
            PftSerializer.Deserialize(reader, Body);
        }

        /// <summary>
        /// Execute the procedure.
        /// </summary>
        public void Execute
            (
                PftContext context,
                string? argument
            )
        {
            using PftContextGuard guard = new PftContextGuard(context);
            var nested = guard.ChildContext;
            nested.Output = context.Output;
            var variables = new PftVariableManager(context.Variables);
            variables.SetVariable("arg", argument);
            nested.SetVariables(variables);
            nested.Execute(Body);
        }

        /// <summary>
        /// Serialize the procedure.
        /// </summary>
        public void Serialize
            (
                BinaryWriter writer
            )
        {
            writer.WriteNullable(Name);
            PftSerializer.Serialize(writer, Body);
        }

        #endregion

        #region ICloneable members

        /// <inheritdoc cref="ICloneable.Clone" />
        public object Clone()
        {
            PftProcedure result = (PftProcedure) MemberwiseClone();

            result.Body = Body.CloneNodes(null).ThrowIfNull();

            return result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Name.ToVisibleString();
        }

        #endregion
    }
}
