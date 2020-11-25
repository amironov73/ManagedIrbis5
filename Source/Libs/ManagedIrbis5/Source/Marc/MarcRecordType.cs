// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* MarcRecordType.cs -- код типа записи в формате ISO 2709
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis.Marc
{
    /// <summary>
    /// Код Типа записи в формате ISO 2709.
    /// </summary>
    public enum MarcRecordType
    {
        /// <summary>
        /// Текстовый материал.
        /// </summary>
        Text = 'a',

        /// <summary>
        /// Архивный материал, рукописи.
        /// </summary>
        ArchiveMaterial = 'b',

        /// <summary>
        /// Печатные ноты.
        /// </summary>
        PrintedMusic = 'c',

        /// <summary>
        /// Рукописные ноты.
        /// </summary>
        HandwrittenMusic = 'd',

        /// <summary>
        /// Печатные карты.
        /// </summary>
        PrintedMap = 'e',

        /// <summary>
        /// Рукописные карты.
        /// </summary>
        HandwrittenMap = 'f',

        /// <summary>
        /// Фильмокопии, видеофильмы и проч.
        /// </summary>
        Film = 'g',

        /// <summary>
        /// Немузыкальные записи.
        /// </summary>
        NonMusicRecord = 'i',

        /// <summary>
        /// Музыкальные записи.
        /// </summary>
        MusicRecord = 'j',

        /// <summary>
        /// Картины, фотографии и т.д. (двумерная графика).
        /// </summary>
        Picture = 'k',

        /// <summary>
        /// Компьютерные файлы.
        /// </summary>
        ComputerFiles = 'm',

        /// <summary>
        /// Смешанные материалы.
        /// </summary>
        MixedMaterial = 'o',

        /// <summary>
        /// Скульптуры и другие трехмерные объекты.
        /// </summary>
        Sculpture = 'r'
    }
}
