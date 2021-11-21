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

/* CollectionType.cs -- вид многочастного издания
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Reflection;

#endregion

#nullable enable

namespace ManagedIrbis.Onix
{
    /// <summary>
    /// Вид многочастного издания.
    /// </summary>
    public static class CollectionType
    {
        #region Constants

        /// <summary>
        /// Неопределенный.
        /// </summary>
        public const string Undefined = "00";

        /// <summary>
        /// Многочастное издание, созданное издателем
        /// (многотомное издание, издательская
        /// серия, комплектное и комбинированное издание).
        /// </summary>
        public const string ByPublisher = "10";

        /// <summary>
        /// Многочастное (комплектное) издание, собранное
        /// не издателем, а книготорговой организацией, распространителем.
        /// </summary>
        public const string ByDistributor = "20";

        #endregion

        #region Public methods

        /// <summary>
        /// Получение массива значений констант.
        /// </summary>
        public static string[] ListValues()
        {
            return ReflectionUtility.ListConstantValues<string> (typeof (CollectionType));
        }

        #endregion
    }
}
