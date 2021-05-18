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

using System.IO;

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
        /// Чтение меню как текстового файла.
        /// </summary>
        public static MenuFile? ReadMenuFile
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

            return MenuFile.ParseStream(reader);

        } // method ReadMenuFile

        #endregion

    } // class SyncProviderUtility

} // namespace ManagedIrbis.Providers
