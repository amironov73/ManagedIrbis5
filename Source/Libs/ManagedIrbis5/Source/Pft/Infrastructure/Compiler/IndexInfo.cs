// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* IndexInfo.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Compiler
{
    /// <summary>
    ///
    /// </summary>
    internal sealed class IndexInfo
    {
        #region Properties

        /// <summary>
        /// Specification.
        /// </summary>
        public IndexSpecification Specification { get; }

        /// <summary>
        /// Identifier.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Reference.
        /// </summary>
        public string Reference { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public IndexInfo
            (
                IndexSpecification specification,
                int id
            )
        {
            Specification = specification;
            Id = id;
            Reference = "Index" + Id.ToInvariantString();
        }

        #endregion

        #region Object members

        private bool Equals
            (
                IndexInfo other
            )
        {
            return Id == other.Id;
        }

        /// <inheritdoc cref="object.Equals(object)" />
        public override bool Equals
            (
                object? obj
            )
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var info = obj as IndexInfo;

            return !ReferenceEquals(info, null)
                   && Equals(info);
        }

        /// <inheritdoc cref="object.GetHashCode" />
        public override int GetHashCode()
        {
            return Id;
        }

        #endregion
    }
}
