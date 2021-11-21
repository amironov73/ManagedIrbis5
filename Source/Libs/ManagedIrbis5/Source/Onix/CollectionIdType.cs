// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* CollectionIdType.cs -- вид идентификационного номера многочастного издания
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Reflection;

#endregion

#nullable enable

namespace ManagedIrbis.Onix
{
    /// <summary>
    /// Вид идентификационного номера многочастного издания.
    /// </summary>
    public static class CollectionIdType
    {
        #region Constants

        /// <summary>
        /// Издательский номер многочастного издания в целом.
        /// </summary>
        public const string PublishingNumber = "01";

        /// <summary>
        /// Международный номер сериального издания (ISSN).
        /// </summary>
        public const string Issn = "02";

        /// <summary>
        /// Международный стандартный книжный номер (ISBN) издания в целом.
        /// </summary>
        public const string Isbn = "15";

        #endregion

        #region Public methods

        /// <summary>
        /// Получение массива значений констант.
        /// </summary>
        public static string[] ListValues()
        {
            return ReflectionUtility.ListConstantValues<string> (typeof (CollectionIdType));
        }

        #endregion
    }
}
