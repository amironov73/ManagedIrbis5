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

/* SupplierRole.cs -- статус поставщика изданий
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Reflection;

#endregion

#nullable enable

namespace ManagedIrbis.Onix
{
    /// <summary>
    /// Статус поставщика изданий.
    /// </summary>
    public static class SupplierRole
    {
        #region Constants

        /// <summary>
        /// Не определено.
        /// </summary>
        public const string Undefined = "00";

        /// <summary>
        /// Издатель как поставщик для розничной торговли.
        /// </summary>
        public const string Publisher = "01";

        /// <summary>
        /// Оптовый поставщик для розничной торговли.
        /// </summary>
        public const string WholesaleSupplier = "02";

        /// <summary>
        /// Торговый агент (посредник).
        /// </summary>
        public const string Agent = "03";

        #endregion

        #region Public methods

        /// <summary>
        /// Получение массива значений констант.
        /// </summary>
        public static string[] ListValues()
        {
            return ReflectionUtility.ListConstantValues<string> (typeof (SupplierRole));
        }

        #endregion
    }
}
