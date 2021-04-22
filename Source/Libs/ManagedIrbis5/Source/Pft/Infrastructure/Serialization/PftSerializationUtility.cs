// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftSerializationUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Serialization
{
    /// <summary>
    ///
    /// </summary>
    public static class PftSerializationUtility
    {
        #region Public methods

        /// <summary>
        /// Compare two lists of nodes.
        /// </summary>
        public static void CompareLists
            (
                IList<PftNode> left,
                IList<PftNode> right
            )
        {
            if (left.Count != right.Count)
            {
                throw new PftSyntaxException();
            }

            for (int i = 0; i < left.Count; i++)
            {
                CompareNodes
                    (
                        left[i],
                        right[i]
                    );
            }
        }

        /// <summary>
        /// Compare two lists of <see cref="FieldSpecification"/>.
        /// </summary>
        public static void CompareLists
            (
                IList<FieldSpecification> left,
                IList<FieldSpecification> right
            )
        {
            if (left.Count != right.Count)
            {
                throw new PftSyntaxException();
            }

            for (int i = 0; i < left.Count; i++)
            {
                if (!FieldSpecification.Compare(left[i], right[i]))
                {
                    throw new PftSerializationException();
                }
            }
        }

        /// <summary>
        /// Compare two nodes.
        /// </summary>
        public static void CompareNodes
            (
                PftNode? left,
                PftNode? right
            )
        {
            bool result;

            if (ReferenceEquals(left, null))
            {
                result = ReferenceEquals(right, null);
            }
            else
            {
                result = !ReferenceEquals(right, null)
                    && ReferenceEquals
                    (
                        left.GetType(),
                        right.GetType()
                    );
                if (result)
                {
                    left.CompareNode(right!);
                }
            }

            if (!result)
            {
                throw new PftSerializationException();
            }
        }

        /// <summary>
        /// Compare two strings.
        /// </summary>
        public static bool CompareStrings
            (
                string? left,
                string? right
            )
        {
            if (ReferenceEquals(left, null))
            {
                return ReferenceEquals(right, null);
            }

            if (ReferenceEquals(right, null))
            {
                return false;
            }

            return string.CompareOrdinal(left, right) == 0;
        }

        /// <summary>
        /// Verify deserialized <see cref="PftProgram"/>.
        /// </summary>
        public static void VerifyDeserializedProgram
            (
                PftProgram ethalon,
                PftProgram deserialized
            )
        {
            ethalon.CompareNode(deserialized);
        }

        #endregion
    }
}
