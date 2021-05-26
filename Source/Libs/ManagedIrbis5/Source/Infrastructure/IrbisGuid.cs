// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* IrbisGuid.cs -- GUID-поле в записях
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    //
    // Поле GUID - уникальный идентификатор записи.
    //
    // Поле GUID не показывается с помощью команды V,
    // отсутствует с точки зрения функций P и A
    // Не показывается &uf('0'), &uf('1'), &uf('A'),
    // &uf('P'), &uf('+4') и &uf('++0').
    // Но показывается &uf('+0').
    //

    /// <summary>
    /// GUID-поле в записях,
    /// появившееся в недавних версиях ИРБИС64.
    /// </summary>
    public static class IrbisGuid
    {
        #region Constants

        /// <summary>
        /// Метка поля для GUID.
        /// </summary>
        public const int Tag = 2147483647;

        /// <summary>
        /// Метка поля для GUID (строка).
        /// </summary>
        public const string TagString = "2147483647";

        #endregion

        #region Public methods

        /// <summary>
        /// Get GUID from the <see cref="Record"/>.
        /// </summary>
        public static string? Get
            (
                Record? record
            )
        {
            if (ReferenceEquals(record, null))
            {
                return null;
            }

            var text = record.FM(Tag);
            if (text.IsEmpty())
            {
                return null;
            }

            var guid = Parse(text);

            // TODO почему тут "D" ?

            return guid.ToString("D").ToUpperInvariant();

        } // method Get

        /// <summary>
        /// Create new GUID in IRBIS64 format.
        /// </summary>
        // TODO почему тут "B" ?
        public static string NewGuid() =>
            Guid.NewGuid().ToString("B").ToUpperInvariant();

        /// <summary>
        /// Parse the text.
        /// </summary>
        public static Guid Parse (ReadOnlySpan<char> text) => Guid.Parse(text);

        /// <summary>
        /// Parse the record.
        /// </summary>
        public static Guid? Parse
            (
                Record record
            )
        {
            var text = record.FM(Tag);
            if (text.IsEmpty())
            {
                return null;
            }

            return Parse(text);
        } // method Parse

        #endregion

    } // class IrbisGuid

} // namespace ManagedIrbis.Infrastructure
