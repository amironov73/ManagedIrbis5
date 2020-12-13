// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* BitArrayUtility.cs -- вспомогательные методы для BitArray
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections;

#endregion

#nullable enable

namespace AM.Collections
{
    /// <summary>
    /// Вспомогательные методы для класса <see cref="BitArray"/>.
    /// </summary>
    public static class BitArrayUtility
    {
        #region Public methods

        /// <summary>
        /// Compares two <see cref="BitArray"/>s.
        /// </summary>
        public static bool AreEqual
            (
                BitArray left,
                BitArray right
            )
        {
            Sure.NotNull(left, "left");
            Sure.NotNull(right, "right");

            if (left.Length != right.Length)
            {
                return false;
            }

            int length = left.Length;
            bool[] leftA = new bool[length];
            ICollection leftCollection = left;
            leftCollection.CopyTo(leftA, 0);
            bool[] rightA = new bool[length];
            ICollection rightCollection = right;
            rightCollection.CopyTo(rightA,0);

            for (int i = 0; i < length; i++)
            {
                if (leftA[i] != rightA[i])
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
