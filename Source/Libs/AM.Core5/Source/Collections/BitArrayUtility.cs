// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

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
        /// Сравнение двух <see cref="BitArray"/>s.
        /// </summary>
        public static bool AreEqual
            (
                BitArray left,
                BitArray right
            )
        {
            Sure.NotNull (left);
            Sure.NotNull (right);

            if (left.Length != right.Length)
            {
                return false;
            }

            for (int i = 0; i < left.Length; i++)
            {
                if (left[i] != right[i])
                {
                    return false;
                }
            }

            return true;

        } // method AreEqual

        #endregion

    } // class BitArrayUtility

} // namespace AM.Collections
