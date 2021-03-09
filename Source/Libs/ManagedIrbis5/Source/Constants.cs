// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* Constants.cs -- общие для ИРБИС64 константы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Общие для ИРБИС64 константы.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class Constants
    {
        #region Constants

        /// <summary>
        /// Имя файла, содержащего список баз данных для администратора.
        /// </summary>
        public const string AdministratorDatabaseList = "dbnam1.mnu";

        /// <summary>
        /// Имя файла, содержащего список баз данных для каталогизатора.
        /// </summary>
        public const string CatalogerDatabaseList = "dbnam2.mnu";

        /// <summary>
        /// Максимальная длина (размер полки) - ограничение формата.
        /// </summary>
        public const int MaxRecord = 32000;

        /// <summary>
        /// Максимальное количество постингов в пакете.
        /// </summary>
        public const int MaxPostings = 32758;

        /// <summary>
        /// Имя файла, содержащего список баз данных для читателя.
        /// </summary>
        public const string ReaderDatabaseList = "dbnam3.mnu";

        #endregion

    } // class Constants

} // namespace ManagedIrbis
