// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* BbkSection.cs -- раздел в ББК, соответствующий той или иной науке
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Systematization;

/// <summary>
/// Раздел в ББК, соответствующий той или иной науке (области знаний).
/// </summary>
public sealed class BbkSection
{
    #region Static properties

    /// <summary>
    /// Взаимное соответствие между таблицами для научных библиотек (с буквами)
    /// и таблицами для массовых библиотек (с цифрами).
    /// </summary>
    public static readonly BbkSection[] Conformity =
    {
        new ("1", "А", "Общенаучное и междисциплинарное знание"),

        // new ("2", "Б/Е", "Естественные науки"),
        new ("20", "Б", "Естественные науки в целом"),
        new ("22", "В", "Физико-математические науки"),
        new ("24", "Г", "Химические науки"),
        new ("26", "Д", "Науки о Земле (геодезические, геофизические, геологические и географические науки"),
        new ("28", "Е", "Биологические науки"),

        // new ("3", "Ж/О", "Техника. Технические науки"),
        new ("30", "Ж", "Техника и технические науки в целом"),
        new ("31.1", "З1", "Энергетика в целом"),
        new ("31.2", "З2", "Электроэнергетика. Электротехника"),
        new ("31.3", "З3", "Теплоэнергетика. Теплотехника"),
        new ("31.4", "З4", "Ядерная (атомная) энергетика"),
        new ("31.5", "З5", "Гидроэнергетика"),
        new ("31.6", "З6", "Альтернативная энергетика"),
        new ("31.7", "З7", "Техника сжатых и разреженных газов"),
        new ("32.8", "З8", "Техническая кибернетика. Общая радиотехника. Электроника. Электроакустика. Электрическая связь"),
        new ("32.9", "З9", "Телевидение. Радио- и оптическая локация. Радионавигация. Автоматика и телемеханика. Вычислительная техника. Программирование. Другие отрасли радиоэлектроники"),
        new ("33", "И", "Горное дело"),
        new ("34", "К", "Технология металлов. Машиностроение. Приборостроение"),
        new ("35", "Л", "Химическая технология. Химические производства. Пищевые производства"),
        new ("36", "Л", "Пищевые производства"),
        new ("37", "М", "Технология древесины. Производства легкой промышленности. Домоводство. Бытовые услуги. Полиграфическое производство. Фотокинотехника"),
        new ("38", "Н", "Строительство"),
        new ("39", "О", "Транспорт"),

        new ("4", "П", "Сельское и лесное хозяйство. Сельскохозяйственные и лесохозяйственные науки"),

        new ("5", "Р", "Здравоохранение. Медицинские науки"),

        // new ("6", "С/Ц", "Общественные и гуманитарные науки"),
        new ("60", "С", "Общественные науки в целом"),
        new ("63", "Т", "История. Исторические науки"),
        new ("65", "У", "Экономика. Экономические науки"),
        new ("66", "Ф", "Политика. Политическая наука"),
        new ("67", "Х", "Право. Юридические науки"),
        new ("68", "Ц", "Военное дело. Военная наука"),

        // new ("70/79", "Ч", "Культура. Наука. Просвещение"),
        new ("71", "Ч1", "Культура. Культурология"),
        new ("72", "Ч2", "Наука. Науковедение"),
        new ("74", "Ч4", "Образование. Педагогические науки"),
        new ("75", "Ч5", "Физическая культура и спорт"),
        new ("76", "Ч6", "Средства массовой информации. Книжное дело"),
        new ("77", "Ч7", "Культурно-досуговая деятельность"),
        new ("78", "Ч8", "Библиотечная, библиографическая и научно-информационная деятельность"),
        new ("79", "Ч9", "Охрана памятников истории и культуры. Музейное дело. Выставочное дело. Архивное дело"),

        // new ("80/84", "Ш", "Филологические науки. Художественная литература"),
        new ("85", "Щ", "Искусство"),
        new ("86", "Э", "Религия. Мистика. Свободомыслие"),

        // new ("87", "Ю0/8", "Философия"),
        new ("87.0", "Ю0", "Философия в целом"),
        new ("87.1", "Ю1", "Метафизика. Онтология"),
        new ("87.2", "Ю2", "Гносеология (эпистемиология). Философия науки"),
        new ("87.3", "Ю3", "История философии"),
        new ("87.4", "Ю4", "Логика"),
        new ("87.5", "Ю5", "Философская антропология. Аксиология"),
        new ("87.6", "Ю6", "Социальная философия"),
        new ("87.7", "Ю7", "Этика"),
        new ("87.8", "Ю8", "Эстетика"),
        new ("88", "Ю9", "Психология"),

        new ("9", "Я", "Литература универсального содержания"),
    };

    #endregion

    #region Properties

    /// <summary>
    /// Деление в ББК для массовых библиотек (они же средние таблицы) (арабские цифры).
    /// </summary>
    public string? PublicLibrary { get; set; }

    /// <summary>
    /// Деление в ББК для научных библиотек (они же полные таблицы) (кириллица).
    /// </summary>
    public string? ScienceLibrary { get; set; }

    /// <summary>
    /// Формулировка.
    /// </summary>
    public string? Wording { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="publicLibrary">ББК для массовых библиотек (арабские цифры).</param>
    /// <param name="scienceLibrary">ББК для научных библиотек (кириллица).</param>
    /// <param name="wording">Формулировка.</param>
    internal BbkSection
        (
            string publicLibrary,
            string scienceLibrary,
            string wording
        )
    {
        Sure.NotNullNorEmpty (publicLibrary);
        Sure.NotNullNorEmpty (scienceLibrary);
        Sure.NotNullNorEmpty (wording);

        PublicLibrary = publicLibrary;
        ScienceLibrary = scienceLibrary;
        Wording = wording;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Поиск раздела, соответствующего индексу
    /// ББК для массовых библиотек (с цифрами).
    /// </summary>
    /// <remarks>Внимание! Метод чувствителен к регистру символов!</remarks>
    public static BbkSection? FindPublicSection
        (
            string index
        )
    {
        Sure.NotNullNorEmpty (index);

        foreach (var section in Conformity)
        {
            var prefix = section.PublicLibrary.ThrowIfNullOrEmpty();
            if (index.StartsWith (prefix))
            {
                return section;
            }
        }

        return null;
    }

    /// <summary>
    /// Поиск раздела, соответствующего индексу
    /// ББК для научных библиотек (с буквами).
    /// </summary>
    /// <remarks>Внимание! Метод чувствителен к регистру символов!</remarks>
    public static BbkSection? FindScienceSection
        (
            string index
        )
    {
        Sure.NotNullNorEmpty (index);

        foreach (var section in Conformity)
        {
            var prefix = section.ScienceLibrary.ThrowIfNullOrEmpty();
            if (index.StartsWith (prefix))
            {
                return section;
            }
        }

        return null;
    }

    /// <summary>
    /// Проверяет, является ли индекс ББК для массовых библиотек (с цифрами).
    /// </summary>
    /// <remarks>Внимание! Метод чувствителен к регистру символов!</remarks>
    public static bool IsPublicLibrary
        (
            string index
        )
    {
        Sure.NotNullNorEmpty (index);

        foreach (var section in Conformity)
        {
            var prefix = section.PublicLibrary.ThrowIfNullOrEmpty();
            if (index.StartsWith (prefix))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Проверяет, является ли индекс ББК для научных библиотек (с буквами).
    /// </summary>
    /// <remarks>Внимание! Метод чувствителен к регистру символов!</remarks>
    public static bool IsScienceLibrary
        (
            string index
        )
    {
        Sure.NotNullNorEmpty (index);

        foreach (var section in Conformity)
        {
            var prefix = section.ScienceLibrary.ThrowIfNullOrEmpty();
            if (index.StartsWith (prefix))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Преобразование ББК для научных библиотек (с буквами)
    /// в ББК для массовых библиотек (с цифрами).
    /// </summary>
    /// <remarks>Внимание! Метод чувствителен к регистру символов!</remarks>
    public static string ToPublic
        (
            string index
        )
    {
        Sure.NotNullNorEmpty (index);

        foreach (var section in Conformity)
        {
            var prefix = section.ScienceLibrary.ThrowIfNullOrEmpty();
            if (index.StartsWith (prefix))
            {
                return section.PublicLibrary + index[prefix.Length..];
            }
        }

        throw new ArgumentOutOfRangeException (index, nameof (index));
    }

    /// <summary>
    /// Преобразование ББК для массовых библиотек (с цифрами)
    /// в ББК для массовых библиотек (с буквами).
    /// </summary>
    /// <remarks>Внимание! Метод чувствителен к регистру символов!</remarks>
    public static string ToScience
        (
            string index
        )
    {
        Sure.NotNullNorEmpty (index);

        foreach (var section in Conformity)
        {
            var prefix = section.PublicLibrary.ThrowIfNullOrEmpty();
            if (index.StartsWith (prefix))
            {
                return section.PublicLibrary + index[prefix.Length..];
            }
        }

        throw new ArgumentOutOfRangeException (index, nameof (index));
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"{PublicLibrary} - {ScienceLibrary}: {Wording}";
    }

    #endregion
}
