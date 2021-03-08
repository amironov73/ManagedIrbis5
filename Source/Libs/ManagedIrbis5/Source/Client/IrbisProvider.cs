// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* IrbisProvider.cs -- абстрактный ИРБИС-провайдер
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

#endregion

using ManagedIrbis.Infrastructure;

#nullable enable

namespace ManagedIrbis.Client
{
    /// <summary>
    /// Абстрактный ИРБИС-провайдер.
    /// </summary>
    public abstract class IrbisProvider
    {
        /// <summary>
        /// Текущая база данных.
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// Чтение файла с сервера.
        /// </summary>
        public abstract string ReadFile(FileSpecification file);

        /// <summary>
        /// Поиск записей на сервере.
        /// </summary>
        public abstract int[] Search(string expression);

    } // class IrbisProvider

} // namespace ManagedIrbis.Client
