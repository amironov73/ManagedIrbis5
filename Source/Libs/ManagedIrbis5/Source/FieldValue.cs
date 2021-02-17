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

/* FieldValue.cs -- field value related routines
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

using AM;

using ManagedIrbis.Properties;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Field value related routines.
    /// </summary>
    public static class FieldValue
    {
        #region Properties

        /// <summary>
        /// Throw exception on verification error.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public static bool ThrowOnVerify { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Whether the value valid.
        /// </summary>
        public static bool IsValidValue
            (
                string? value
            )
        {
            if (!string.IsNullOrEmpty(value))
            {
                foreach (var c in value)
                {
                    if (c == SubField.Delimiter || c < ' ')
                    {
                        return false;
                    }
                }
            }

            return true;
        } // method IsValidValue

        /// <summary>
        /// Field value normalization.
        /// </summary>
        public static string? Normalize
            (
                string? value
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            var result = value.Trim();

            return result;
        } // method Normalize

        /// <summary>
        /// Verify subfield value.
        /// </summary>
        public static bool Verify (string? value) => Verify(value, ThrowOnVerify);

        /// <summary>
        /// Verify subfield code.
        /// </summary>
        public static bool Verify
            (
                string? value,
                bool throwOnError
            )
        {
            var result = IsValidValue(value);

            if (!result)
            {
                Magna.Debug
                    (
                        nameof(FieldValue) + "::" + nameof(Verify)
                        + Resources.FieldValue_Verify_BadValue
                        + value.ToVisibleString()
                    );

                if (throwOnError)
                {
                    throw new VerificationException
                        (
                            Resources.FieldValue_Verify_BadFieldValue
                            + value.ToVisibleString()
                        );
                }
            }

            return result;
        } // method Verify

        #endregion

    } // class FieldValue

} // namespace ManagedIrbis
