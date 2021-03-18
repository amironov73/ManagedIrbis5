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

/* SubFieldCode.cs -- subfield code related routines
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;

using AM;
using AM.Collections;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Subfield code related routines.
    /// </summary>
    public static class SubFieldCode
    {
        #region Constants

        /// <summary>
        /// Begin of valid codes range.
        /// </summary>
        public const char DefaultFirstCode = '!';

        /// <summary>
        /// End of valid codes range (including!).
        /// </summary>
        public const char DefaultLastCode = '~';

        #endregion

        #region Properties

        /// <summary>
        /// Throw exception on verification error.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public static bool ThrowOnVerification { get; set; }

        /// <summary>
        /// <see cref="CharSet"/> of valid codes.
        /// </summary>
        public static CharSet ValidCodes => _validCodes;

        #endregion

        #region Construction

        /// <summary>
        /// Static constructor.
        /// </summary>
        static SubFieldCode()
        {
            _validCodes = new CharSet();
            _validCodes.AddRange(DefaultFirstCode, DefaultLastCode);
            _validCodes.Remove('^');
        } // static constructor

        #endregion

        #region Private members

        private static readonly CharSet _validCodes;

        #endregion

        #region Public methods

        /// <summary>
        /// Whether the code valid.
        /// </summary>
        public static bool IsValidCode (char code) => ValidCodes.Contains(code);

        /// <summary>
        /// Code normalization.
        /// </summary>
        public static char Normalize (char code) => char.ToLowerInvariant(code);

        /// <summary>
        /// Verify subfield code.
        /// </summary>
        public static bool Verify
            (
                char code,
                bool throwOnError
            )
        {
            var result = IsValidCode(code);

            if (!result)
            {
                Magna.Debug
                    (
                        nameof(SubFieldCode) + "::" + nameof(Verify)
                        + ": bad code='" + code + "'"
                    );

                if (throwOnError)
                {
                    throw new ArgumentOutOfRangeException();
                }
            }

            return result;
        } // method Verify

        /// <summary>
        /// Verify subfield code.
        /// </summary>
        public static bool Verify (char code) => Verify(code, ThrowOnVerification);

        #endregion

    } // class SubFieldCode

} // namespace ManagedIrbis
