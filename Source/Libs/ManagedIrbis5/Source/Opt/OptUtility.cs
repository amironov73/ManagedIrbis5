// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global

/* OptUtility.cs -- методы для работы с OPT-файлами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Threading.Tasks;

using AM;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Opt
{
    /// <summary>
    /// Методы для работы с OPT-файлами.
    /// </summary>
    public static class OptUtility
    {
        #region Public methods

        /// <summary>
        /// Сравнение символов с учётом подстановочного
        /// знака '+'.
        /// </summary>
        public static bool CompareChar (char left, char right) =>
            left == OptFile.Wildcard
            || char.ToUpperInvariant(left) == char.ToUpperInvariant(right);

        /// <summary>
        /// Сравнение строк с учётом подстановочного знака '+'.
        /// </summary>
        public static bool CompareString
            (
                string left,
                string? right
            )
        {
            if (string.IsNullOrEmpty(right))
            {
                if (left.ConsistOf(OptFile.Wildcard))
                {
                    return true;
                }

                return false;
            }

            var leftEnumerator = left.ToCharArray().GetEnumerator();
            var rightEnumerator = right.ToCharArray().GetEnumerator();

            while (true)
            {
                char leftChar;
                var leftNext = leftEnumerator.MoveNext();
                var rightNext = rightEnumerator.MoveNext();

                if (leftNext && !rightNext)
                {
                    leftChar = (char)leftEnumerator.Current.ThrowIfNull();
                    if (leftChar == OptFile.Wildcard)
                    {
                        while (leftEnumerator.MoveNext())
                        {
                            leftChar = (char)leftEnumerator.Current.ThrowIfNull();
                            if (leftChar != OptFile.Wildcard)
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                }

                if (leftNext != rightNext)
                {
                    return false;
                }
                if (!leftNext)
                {
                    return true;
                }

                leftChar = (char)leftEnumerator.Current.ThrowIfNull();
                var rightChar = (char)rightEnumerator.Current.ThrowIfNull();
                if (!CompareChar(leftChar, rightChar))
                {
                    return false;
                }
            }
        } // method CompareString

        /// <summary>
        /// Load from the server.
        /// </summary>
        public static OptFile? ReadOptFile
            (
                this ISyncProvider provider,
                FileSpecification specification
            )
        {
            var content = provider.ReadTextFile(specification);
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }

            using var reader = new StringReader(content);

            return OptFile.ParseText(reader);
        } // method ReadOptFile

        /// <summary>
        /// Load from the server.
        /// </summary>
        public static async Task<OptFile?> ReadOptFileAsync
            (
                this IAsyncProvider provider,
                FileSpecification specification
            )
        {
            var content = await provider.ReadTextFileAsync(specification);
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }

            using var reader = new StringReader(content);

            return OptFile.ParseText(reader);
        } // method ReadOptFileAsync

        #endregion

    } // class OptUtility

} // namespace ManagedIrbis.Opt
