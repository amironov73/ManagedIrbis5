// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* FieldTag.cs -- field tag related routines
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

using AM;
using AM.Collections;

using ManagedIrbis.Properties;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Field tag related routines.
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
            GoodCharacters = new CharSet().AddRange('0', '9');
        }

        #endregion

        #region Private members

        private static readonly CharSet GoodCharacters;

        #endregion

        #region Public methods

        /// <summary>
        /// Whether given tag is valid?
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

            var result = GoodCharacters.CheckText(tag)
                         && Normalize(tag) != "0"
                         && tag.Length < 6; // ???

            return result;
        } // method IsValidTag

        /// <summary>
        /// Normalization.
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
        /// Verify the tag value.
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
                        + Resources.FieldTag_Verify_BadTag1
                        + tag.ToVisibleString()
                    );

                if (throwOnError)
                {
                    throw new VerificationException
                        (
                            Resources.FieldTag_Verify_BadTag2
                            + tag.ToVisibleString()
                        );
                }
            }

            return result;
        } // method Verify

        /// <summary>
        /// Verify the tag value.
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
                        + Resources.FieldTag_Verify_BadTag1
                        + tag.ToInvariantString()
                    );

                if (throwOnError)
                {
                    throw new VerificationException
                    (
                        Resources.FieldTag_Verify_BadTag2
                        + tag.ToInvariantString()
                    );
                }
            }

            return result;
        } // method Verify

        /// <summary>
        /// Verify the tag value.
        /// </summary>
        public static bool Verify
            (
                int tag
            )
        {
            return Verify(tag, ThrowOnValidate);
        } // method Verify

        /// <summary>
        /// Verify the tag value.
        /// </summary>
        public static bool Verify
            (
                string? tag
            )
        {
            return Verify(tag, ThrowOnValidate);
        } // method Verify

        #endregion

    } // class FieldTag

} // namespace ManagedIrbis
