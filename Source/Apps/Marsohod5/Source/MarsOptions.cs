// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* MarsOptions.cs -- настройки обмена с МАРС
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace Marsohod5
{
    /// <summary>
    /// Настройки обмена с МАРС.
    /// </summary>
    public sealed class MarsOptions
    {
        #region Properties

        /// <summary>
        /// Строка подключения к серверу ИРБИС64.
        /// </summary>
        public string? ConnectionString { get; set; }

        /// <summary>
        /// Метка поля, содержащего код журнала в МАРС.
        /// </summary>
        public int MarsCode { get; set; } = 883;

        /// <summary>
        /// Метка поля, содержащего флаг, управляющий импортом.
        /// </summary>
        public int MarsFlag { get; set; } = 884;

        /// <summary>
        /// Шаблон имени файла с записями, полученными из проекта МАРС.
        /// </summary>
        public string? Pattern { get; set; } = "*.iso";

        /// <summary>
        /// Имя файла, содержащего таблицу преобразования из МАРС в ИРБИС.
        /// </summary>
        public string? Fst { get; set; } = "marc_irb.fst";

        /// <summary>
        /// Удалять ISO-файл после успешного импорта.
        /// </summary>
        public bool DeleteAfterImport { get; set; } = false;

        /// <summary>
        /// Минимальный год, подлежащий импорту (включительно).
        /// </summary>
        public int MinimalYear { get; set; } = 0;

        /// <summary>
        /// Максимальный год, подлежащий импорту (включительно).
        /// </summary>
        public int MaximalYear { get; set; } = 9999;

        /// <summary>
        /// Действие, предпринимаемое, если статья уже существует
        /// в каталоге.
        /// </summary>
        public string? ExistingArticle { get; set; } = "skip";

        #endregion
    }
}
