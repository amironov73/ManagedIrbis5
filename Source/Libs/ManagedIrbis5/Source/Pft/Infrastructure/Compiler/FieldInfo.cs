// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FieldInfo.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using ManagedIrbis.Pft.Infrastructure.Ast;

#endregion

namespace ManagedIrbis.Pft.Infrastructure.Compiler
{
    internal sealed class FieldInfo
    {
        #region Properties

        public PftField Field { get; private set; }

        public FieldSpecification Specification { get; private set; }

        public string Text { get; private set; }

        public int Id { get; private set; }

        public string Reference { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public FieldInfo
            (
                PftField field,
                int id
            )
        {
            Field = field;
            Specification = field.ToSpecification();
            Text = Specification.ToString();
            Id = id;
            Reference = "Field" + Id.ToInvariantString();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => Text;

        #endregion
    }
}
