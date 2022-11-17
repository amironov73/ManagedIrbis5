// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* Affix.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Linguistics.Hunspell;

/*

    Аффикс (лат. affixus — прикреплённый), также формант или форматив,
    — морфема, которая присоединяется к корню и служит для образования
    слов. Аффиксы могут быть словообразовательными (как английский
    -ness и pre-) и флексионными (как -s и -ed). Аффикс является
    связанной морфемой (морфема, которая не совпадает с основой хотя бы
    в одной словоформе неслужебного слова; напр., kindness, unlikely).
    Префиксы и суффиксы могут быть отделимыми аффиксами.

    Аффиксы делятся на несколько категорий в зависимости от их позиции
    относительно корня слова. Самыми распространёнными терминами
    для обозначения аффиксов являются суффикс и префикс. Реже встречаются
    термины инфикс и циркумфикс, так как инфиксы практически отсутствуют
    в европейских языках, а циркумфиксы встречаются только в некоторых
    германских.

    По функциональности

    * Непродуктивный аффикс - такой, который редко принимает участие
      в создании слов или не используется в современном языке совсем.
    * Продуктивный аффикс - такой, который широко используется для создания
      новых слов и форм. Например, в русском языке: -чик, под-
    * Словообразовательный аффикс - такой, который служит для образования
      новых слов. Например, в русском языке: пере-, -тель.
    * Формообразующий аффикс - такой, который служит для образования
      форм слов. Например, в русском языке: нос-ивш-ий, окн-а, ходи-л-а.

 */

internal sealed class Affix<TEntry>
    where TEntry : AffixEntry
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public TEntry Entry { get; }

    /// <summary>
    ///
    /// </summary>
    public FlagValue AFlag { get; }

    /// <summary>
    ///
    /// </summary>
    public AffixEntryOptions Options { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="entry"></param>
    /// <param name="aFlag"></param>
    /// <param name="options"></param>
    public Affix
        (
            TEntry entry,
            FlagValue aFlag,
            AffixEntryOptions options
        )
    {
        Sure.NotNull (entry);

        Entry = entry;
        AFlag = aFlag;
        Options = options;
    }

    #endregion

    #region Methods

    /// <summary>
    ///
    /// </summary>
    /// <param name="entry"></param>
    /// <param name="group"></param>
    internal static Affix<TEntry> Create
        (
            TEntry entry,
            AffixEntryGroup<TEntry> group
        )
    {
        return new (entry, group.AFlag, group.Options);
    }

    #endregion
}
