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

/* SyncProviderUtility.cs -- вспомогательные методы для синхронного провайдера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using ManagedIrbis.Fst;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;

#endregion

#nullable enable

namespace ManagedIrbis.Providers
{
    /// <summary>
    /// Вспомогательные методы для синхронного провайдера.
    /// </summary>
    public static class SyncProviderUtility
    {
        #region Public methods

        /// <summary>
        /// Чтение FST-файла как текстового.
        /// </summary>
        public static FstFile? ReadFstFile
            (
                this ISyncProvider connection,
                FileSpecification specification
            )
        {
            var content = connection.ReadTextFile(specification);
            if (content is null)
            {
                return default;
            }

            using var reader = new StringReader(content);

            return FstFile.ParseStream(reader);

        } // method ReadFstFile

        /// <summary>
        /// Чтение меню как текстового файла.
        /// </summary>
        public static MenuFile? ReadMenuFile
            (
                this ISyncProvider provider,
                FileSpecification specification
            )
        {
            var content = provider.ReadTextFile(specification);
            if (content is null)
            {
                return default;
            }

            using var reader = new StringReader(content);

            return MenuFile.ParseStream(reader);

        } // method ReadMenuFile

        /// <summary>
        /// Чтение параметрического файла как текстового.
        /// </summary>
        public static ParFile? ReadParFile
            (
                this ISyncProvider provider,
                FileSpecification specification
            )
        {
            var text = provider.ReadTextFile(specification);
            if (text is null)
            {
                return default;
            }

            using var reader = new StringReader(text);

            return ParFile.ParseText(reader);

        } // method ReadParFile

        /// <summary>
        /// Чтение записи с сервера.
        /// </summary>
        public static Record? ReadRecord
            (
                this ISyncProvider connection,
                int mfn
            )
        {
            var parameters = new ReadRecordParameters
            {
                Database = connection.Database,
                Mfn = mfn
            };

            return connection.ReadRecord(parameters);

        } // method ReadRecord

        /// <summary>
        /// Поиск с последующим чтением одной записи.
        /// </summary>
        public static Record? SearchReadOneRecord
            (
                this ISyncProvider provider,
                string expression
            )
        {
            throw new NotImplementedException();
        }

        #endregion

    } // class SyncProviderUtility

} // namespace ManagedIrbis.Providers
