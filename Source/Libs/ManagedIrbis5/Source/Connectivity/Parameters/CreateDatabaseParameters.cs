// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* CreateDatabaseParameters.cs -- параметры создания базы данных на ИРБИС-сервере
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Параметры создания базы данных на ИРБИС-сервере
    /// </summary>
    public sealed class CreateDatabaseParameters
    {
        #region Properties

        /// <summary>
        /// Имя создаваемой базы данных (обязательно).
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// Описание создаваемой базы данных в произвольной форме
        /// (опционально).
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Сделать базу данных видимой для АРМ "Читатель"?
        /// </summary>
        public bool ReaderAccess { get; set; }

        /// <summary>
        /// Имя базы данных-шаблона (опционально).
        /// </summary>
        public string? Template { get; set; }

        #endregion
    }
}
