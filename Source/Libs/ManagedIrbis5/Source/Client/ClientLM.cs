// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ClientLM.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Globalization;
using System.Linq;
using System.Text;

using AM.IO;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Client
{
    /// <summary>
    /// Client LM.
    /// </summary>
    public sealed class ClientLM
    {
        #region Constants

        /// <summary>
        /// Default salt.
        /// </summary>
        public const string DefaultSalt = "Ассоциация ЭБНИТ";

        #endregion

        #region Properties

        /// <summary>
        /// Encoding.
        /// </summary>
        public Encoding Encoding { get; private set; }

        /// <summary>
        /// Salt.
        /// </summary>
        public string? Salt { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ClientLM()
            : this
                (
                    IrbisEncoding.Ansi,
                    DefaultSalt
                )
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="salt"></param>
        public ClientLM
            (
                Encoding encoding,
                string? salt
            )
        {
            Encoding = encoding;
            Salt = salt;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Check hash for the INI-file
        /// (both IRBIS32 and IRBIS64).
        /// </summary>
        public bool CheckHash
            (
                IniFile iniFile
            )
        {
            var user = iniFile.GetValue("Main", "User", null);
            var common = iniFile.GetValue("Main", "Common", null);

            if (string.IsNullOrEmpty(user)
                || string.IsNullOrEmpty(common))
            {
                return false;
            }

            var hash = ComputeHash(user);

            return hash == common;
        }

        /// <summary>
        /// Compute hash for the text
        /// (both IRBIS32 and IRBIS64).
        /// </summary>
        public string ComputeHash
            (
                string text
            )
        {
            var salted = Salt + text;
            var raw = Encoding.GetBytes(salted);
            unchecked
            {
                var sum = 0;
                foreach (var one in raw)
                {
                    sum += one;
                }

                raw = Encoding.GetBytes
                    (
                        sum.ToString(CultureInfo.InvariantCulture)
                    )
                    .Reverse()
                    .ToArray();

                for (var i = 0; i < raw.Length; i++)
                {
                    raw[i] += 0x6D;
                }
            }

            var result = Encoding.GetString(raw, 0, raw.Length);

            return result;
        }

        #endregion
    }
}
