// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* AdditionaFilesOptions.cs -- опции для дополнительных файлов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Microsoft.CodeAnalysis;

#endregion

namespace AM.SourceGeneration
{
    /// <summary>
    /// Опции для дополнительных файлов.
    /// </summary>
    internal sealed class AdditionalFilesOptions
    {
        public string Type { get; set; }

        public AdditionalText AdditionalText { get; set; }
    }
}
