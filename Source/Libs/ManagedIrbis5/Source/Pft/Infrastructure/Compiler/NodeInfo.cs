// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* NodeInfo.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Compiler
{
    /// <summary>
    ///
    /// </summary>
    internal sealed class NodeInfo
    {
        #region Properties

        /// <summary>
        /// Node.
        /// </summary>
        public PftNode Node { get; private set; }

        /// <summary>
        /// Node identifier.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Ready?
        /// </summary>
        public bool Ready { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public NodeInfo
            (
                int id,
                PftNode node
            )
        {
            Id = id;
            Node = node;
        }

        #endregion

        #region Object members

        private bool Equals
            (
                NodeInfo other
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

            var info = obj as NodeInfo;

            return !ReferenceEquals(info, null)
                && Equals(info);
        }

        /// <inheritdoc cref="object.GetHashCode" />
        public override int GetHashCode() => Id;

        #endregion

    } // class MethodInfo

} // namespace ManagedIrbis.Pft.Infrastructure.Compiler
