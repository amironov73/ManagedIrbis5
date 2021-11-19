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

/* ProductFormCode.cs -- код формы продукции
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace ManagedIrbis.Onix
{
    /// <summary>
    /// Код формы продукции.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ProductFormCode
    {
        #region Constants

        /// <summary>
        /// Аудио.
        /// </summary>
        public const string Audio = "AA";

        /// <summary>
        /// Аудиокассета.
        /// </summary>
        public const string AudioCassette = "AB";

        /// <summary>
        /// Аудио-CD (формат CD-Audio или SACD).
        /// </summary>
        public const string AudioCD = "AC";

        /// <summary>
        /// Цифровая аудиокассета.
        /// </summary>
        public const string DigitalAudioCassette = "AD";

        /// <summary>
        /// Аудиодиск (кроме CD).
        /// </summary>
        public const string AudioDisc = "AE";

        /// <summary>
        /// Sony MiniDisc.
        /// </summary>
        public const string SonyMiniDisc = "AG";

        /// <summary>
        /// CD-EXTRA (Audio-CD плюс содержимое для компьютера).
        /// </summary>
        public const string CdExtra = "AH";

        /// <summary>
        /// DVD Audio.
        /// </summary>
        public const string DvdAudio = "AI";

        /// <summary>
        /// Запись для специальных плееров
        /// </summary>
        public const string SpecialAudioPlayer = "AK";

        /// <summary>
        /// SD-карта памяти с записью.
        /// </summary>
        public const string SdCardWithAudio = "AL";

        /// <summary>
        /// Другой аудиоформат.
        /// </summary>
        public const string OtherAudio = "AZ";

        /// <summary>
        /// Книга.
        /// </summary>
        public const string Book = "BA";

        /// <summary>
        /// Книга в твердом переплете.
        /// </summary>
        public const string HardcoverBook = "BB";

        /// <summary>
        /// Книга в мягком переплете (обложке).
        /// </summary>
        public const string SoftcoverBook = "BC";

        /// <summary>
        /// Книга с отрывными листами.
        /// </summary>
        public const string LooseLeafBook = "BD";

        /// <summary>
        /// Книга на спирали.
        /// </summary>
        public const string SpiralBook = "BE";

        /// <summary>
        /// Брошюра.
        /// </summary>
        public const string Brochure = "BF";

        /// <summary>
        /// Книга в кожаном переплете.
        /// </summary>
        public const string LeatherBook = "BG";

        /// <summary>
        /// Картонная книга для детей.
        /// </summary>
        public const string CardboardBook = "BH";

        /// <summary>
        /// Матерчатая книга для детей.
        /// </summary>
        public const string ClothBook = "BI";

        /// <summary>
        /// Книга для ванной для детей.
        /// </summary>
        public const string BookForBath = "BJ";

        /// <summary>
        /// Книга в экспериментальном формате.
        /// </summary>
        public const string ExperimentalBook = "BK";

        /// <summary>
        /// Книга-гигант (для обучения и т. п.).
        /// </summary>
        public const string GiantBook = "BM";

        /// <summary>
        /// Часть книги (для собирания).
        /// </summary>
        public const string BookPart = "BN";

        /// <summary>
        /// Лепорелло (сложенные большие листы).
        /// </summary>
        public const string Leporello = "BO";

        /// <summary>
        /// Книга в другом формате.
        /// </summary>
        public const string OtherBook = "BZ";

        /// <summary>
        /// Карта.
        /// </summary>
        public const string Map = "CA";

        /// <summary>
        /// Карта сложенная.
        /// </summary>
        public const string FoldedMap = "CB";

        /// <summary>
        /// Карта разложенная
        /// </summary>
        public const string LayedOutMap = "CC";

        /// <summary>
        /// Карта свернутая.
        /// </summary>
        public const string RolledMap = "CD";

        /// <summary>
        /// Глобус или планисфера.
        /// </summary>
        public const string Globe = "CE";

        /// <summary>
        /// Другая картографическая продукция.
        /// </summary>
        public const string OtherMap = "CZ";

        /// <summary>
        /// CD-ROM.
        /// </summary>
        public const string CdRom = "DB";

        /// <summary>
        /// CD-I (интерактивный CD).
        /// </summary>
        public const string InteractiveCd = "DC";

        /// <summary>
        /// DVD.
        /// </summary>
        public const string Dvd = "DD";

        /// <summary>
        /// Игровой картридж.
        /// </summary>
        public const string GameCartridge = "DE";

        /// <summary>
        /// Дискета.
        /// </summary>
        public const string Diskette = "DF";

        /// <summary>
        /// Электронная книга.
        /// </summary>
        public const string ElectronicBook = "DG";

        /// <summary>
        /// Онлайн-ресурс.
        /// </summary>
        public const string OnlineResource = "DH";

        /// <summary>
        /// DVD-ROM.
        /// </summary>
        public const string DvdRom = "DI";

        /// <summary>
        /// Карта памяти Secure Digital.
        /// </summary>
        public const string SecureDigital = "DJ";

        /// <summary>
        /// Карта памяти Compact Flash.
        /// </summary>
        public const string CompactFlash = "DK";

        /// <summary>
        /// Карта памяти Memory Stick.
        /// </summary>
        public const string MemoryStick = "DL";

        /// <summary>
        /// USB-Flash-диск.
        /// </summary>
        public const string UsbFlash = "DM";

        /// <summary>
        /// Двухсторонний CD/DVD (на одной стороне CD, на другой DVD).
        /// </summary>
        public const string CdDvd = "DN";

        /// <summary>
        /// Слайды.
        /// </summary>
        public const string Slides = "FC";

        /// <summary>
        /// Слайды для Overhead-проектора.
        /// </summary>
        public const string Overhead = "FD";

        /// <summary>
        /// Разная печатная продукция.
        /// </summary>
        public const string PrintingProduct = "PA";

        /// <summary>
        /// Адресная книга.
        /// </summary>
        public const string AddressBook = "PB";

        /// <summary>
        /// Календарь.
        /// </summary>
        public const string Calendar = "PC";

        /// <summary>
        /// Карты игральные.
        /// </summary>
        public const string PlayingCards = "PD";

        /// <summary>
        /// Дневник.
        /// </summary>
        public const string Diary = "PF";

        /// <summary>
        /// Набор.
        /// </summary>
        public const string Kit = "PH";

        /// <summary>
        /// Нотный лист.
        /// </summary>
        public const string SheetOfMusic = "PI";

        /// <summary>
        /// Набор (или книга) открыток.
        /// </summary>
        public const string Postcards = "PJ";

        /// <summary>
        /// Постер.
        /// </summary>
        public const string Poster = "PK";

        /// <summary>
        /// Книга для записей.
        /// </summary>
        public const string BookForNotes = "PL";

        /// <summary>
        /// Футляр или папка.
        /// </summary>
        public const string Case = "PM";

        /// <summary>
        /// Картины или фотографии.
        /// </summary>
        public const string Painting = "PN";

        /// <summary>
        /// Настенный плакат.
        /// </summary>
        public const string WallPoster = "PO";

        /// <summary>
        /// Наклейки.
        /// </summary>
        public const string Sticker = "PP";

        /// <summary>
        /// Пластинка (книжного размера)
        /// </summary>
        public const string Plate = "PQ";

        /// <summary>
        /// Другая печатная продукция
        /// </summary>
        public const string OtherPrinting = "PZ";

        /// <summary>
        /// Видео.
        /// </summary>
        public const string Video = "VA";

        /// <summary>
        /// Видеодиск.
        /// </summary>
        public const string VideoDisc = "DF";

        /// <summary>
        /// DVD-видео.
        /// </summary>
        public const string DvdVideo = "VI";

        /// <summary>
        /// VHS-кассета.
        /// </summary>
        public const string Vhs = "VJ";

        /// <summary>
        /// Betamax-кассета.
        /// </summary>
        public const string Betamax = "VK";

        /// <summary>
        /// CD-видео.
        /// </summary>
        public const string CdVideo = "VL";

        /// <summary>
        /// SVCD-диск.
        /// </summary>
        public const string Svcd = "VM";

        /// <summary>
        /// HD DVD-диск.
        /// </summary>
        public const string HdDvd = "VN";

        /// <summary>
        /// Blue-ray-видео.
        /// </summary>
        public const string BlueRay = "VO";

        /// <summary>
        /// Видеодиск UMD.
        /// </summary>
        public const string Umd = "VP";

        /// <summary>
        /// Другой видеоформат,
        /// </summary>
        public const string OtherVideo = "VZ";

        /// <summary>
        /// Dumpbin - empty.
        /// </summary>
        public const string EmptyDumpbin = "XB";

        /// <summary>
        /// Dumpbin - filled.
        /// </summary>
        public const string FilledDumpbin = "XC";

        /// <summary>
        /// Counterpack - empty.
        /// </summary>
        public const string EmptyCounterpack = "XD";

        /// <summary>
        /// Counterpack - filled.
        /// </summary>
        public const string FilledCounterpack = "XE";

        /// <summary>
        /// Транспарант.
        /// </summary>
        public const string Transparency = "XI";

        /// <summary>
        /// Игра.
        /// </summary>
        public const string Game = "ZE";

        #endregion
    }
}
