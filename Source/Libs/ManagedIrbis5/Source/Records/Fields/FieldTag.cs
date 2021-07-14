// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global

/* FieldTag.cs -- валидация и нормализация меток полей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

using AM;
using AM.Collections;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Валидация и нормализация меток полей.
    /// </summary>
    public static class FieldTag
    {
        #region Properties

        /// <summary>
        /// Бросать исключения при валидации?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public static bool ThrowOnValidate { get; set; }

        #endregion

        #region Construction

        static FieldTag()
        {
            _goodCharacters = new CharSet().AddRange('0', '9');
        }

        #endregion

        #region Private members

        /// <summary>
        /// Символы, которые могут встречаться в метке поля.
        /// </summary>
        private static readonly CharSet _goodCharacters;

        #endregion

        #region Public methods

        /// <summary>
        /// Проверка метки поля на валидность.
        /// </summary>
        public static bool IsValidTag
            (
                string? tag
            )
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                return false;
            }

            var result = _goodCharacters.CheckText(tag)
                         && Normalize(tag) != "0"
                         && tag.Length < 6; // ???

            return result;

        } // method IsValidTag

        /// <summary>
        /// Нормализация метки поля.
        /// Удаляет лидирующие нули, если таковые имеются.
        /// </summary>
        public static string? Normalize
            (
                string? tag
            )
        {
            if (string.IsNullOrEmpty(tag))
            {
                return tag;
            }

            var result = tag;
            while (result.Length > 1 && result.StartsWith("0"))
            {
                result = result.Substring(1);
            }

            return result;

        } // method Normalize

        /// <summary>
        /// Проверка метки поля.
        /// Может выбросить ислкючение,
        /// если <paramref name="throwOnError"/> установлено в <c>true</c>.
        /// </summary>
        public static bool Verify
            (
                string? tag,
                bool throwOnError
            )
        {
            var result = IsValidTag(tag);

            if (!result)
            {
                Magna.Debug
                    (
                        nameof(FieldTag) + "::" + nameof(Verify)
                        + ": "
                        + tag.ToVisibleString()
                    );

                if (throwOnError)
                {
                    throw new VerificationException
                        (
                            nameof(FieldTag) + "::" + nameof(Verify)
                            + ": "
                            + tag.ToVisibleString()
                        );
                }
            }

            return result;

        } // method Verify

        /// <summary>
        /// Проверка метки поля.
        /// Может выбросить ислкючение,
        /// если <paramref name="throwOnError"/> установлено в <c>true</c>.
        /// </summary>
        public static bool Verify
            (
                int tag,
                bool throwOnError
            )
        {
            var result = tag > 0;

            if (!result)
            {
                Magna.Debug
                    (
                        nameof(FieldTag) + "::" + nameof(Verify)
                        + ": "
                        + tag.ToInvariantString()
                    );

                if (throwOnError)
                {
                    throw new VerificationException
                    (
                        nameof(FieldTag) + "::" + nameof(Verify)
                        + ": "
                        + tag.ToInvariantString()
                    );
                }
            }

            return result;

        } // method Verify

        /// <summary>
        /// Проверка метки поля.
        /// Может выбросить исключение,
        /// если <see cref="ThrowOnValidate"/> установлено в <c>true</c>.
        /// </summary>
        public static bool Verify (int tag) => Verify(tag, ThrowOnValidate);

        /// <summary>
        /// Проверка метки поля.
        /// Может выбросить исключение,
        /// если <see cref="ThrowOnValidate"/> установлено в <c>true</c>.
        /// </summary>
        public static bool Verify(string? tag) => Verify(tag, ThrowOnValidate);

        #endregion

    } // class FieldTag

} // namespace ManagedIrbis
