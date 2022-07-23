// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* SearchScenario.cs -- search scenario
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis;

//
// Для описания одного вида поиска по словарю служат следующие параметры
// Имя параметра Содержание
// ItemAdvNN правила автоматического расширения поиска
// ItemDictionTypeNN тип словаря
// ItemF8ForNN имя формата (без расширения)
// ItemHintNN текст подсказки/предупреждения
// ItemLogicNN логические операторы
// ItemMenuNN имя файла справочника
// ItemModByDicAutoNN не задействован
// ItemModByDicNN способ корректировки по словарю
// ItemNameNN название поиска
// ItemNumb общее количество поисков по словарю
// ItemPftNN имя формата показа документов
// ItemPrefNN префикс инверсии
// ItemTrancNN исходное положение переключателя Усечение
//
// где NN - порядковый номер вида поиска по словарю
// в общем списке (начиная с 0).

/// <summary>
/// Search scenario
/// </summary>
[XmlRoot ("search")]
[DebuggerDisplay ("{Prefix} {Name}")]
public sealed class SearchScenario
    : IHandmadeSerializable,
        IVerifiable
{
    #region Properties

    /// <summary>
    /// Название поиска.
    /// </summary>
    /// <remarks>Например: ItemName5=Заглавие</remarks>
    [XmlAttribute ("name")]
    [JsonPropertyName ("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Префикс для соответствующих терминов в словаре.
    /// </summary>
    /// <remarks>Например: ItemPref5=Т=</remarks>
    [XmlAttribute ("prefix")]
    [JsonPropertyName ("prefix")]
    public string? Prefix { get; set; }

    /// <summary>
    /// Тип словаря для соответствующего поиска.
    /// </summary>
    /// <remarks>Например: ItemDictionType8=1</remarks>
    [XmlAttribute ("type")]
    [JsonPropertyName ("type")]
    public DictionaryType DictionaryType { get; set; }

    /// <summary>
    /// Имя файла справочника.
    /// </summary>
    /// <remarks>Например: ItemMenu8=str.mnu</remarks>
    [XmlAttribute ("menu")]
    [JsonPropertyName ("menu")]
    public string? MenuName { get; set; }

    /// <summary>
    /// Имя формата (без расширения).
    /// </summary>
    /// <remarks>Параметр  служит для указания имени формата
    /// (без расширения), который используется при показе
    /// термина словаря полностью (Приложение 4 п.13).
    /// Используется только в ИРБИС32, т.е. в ИРБИС64
    /// не используется.
    /// Используется для длинных терминов (больше 30 символов).
    /// Например: ItemF8For5=!F8TIT.
    /// </remarks>
    [XmlAttribute ("old")]
    [JsonPropertyName ("old")]
    public string? OldFormat { get; set; }

    /// <summary>
    /// Способ Корректировки по словарю.
    /// </summary>
    [XmlAttribute ("correction")]
    [JsonPropertyName ("correction")]
    public string? Correction { get; set; }

    /// <summary>
    /// Исходное положение переключателя "Усечение".
    /// </summary>
    /// <remarks>Параметр  определяет исходное положение
    /// переключателя "Усечение" для данного вида поиска
    /// (0 - нет; 1 - да) - действует только в АРМе "Каталогизатор".
    /// </remarks>
    [XmlAttribute ("truncation")]
    [JsonPropertyName ("truncation")]
    public bool Truncation { get; set; }

    /// <summary>
    /// Текст подсказки/предупреждения.
    /// </summary>
    [XmlAttribute ("hint")]
    [JsonPropertyName ("hint")]
    public string? Hint { get; set; }

    /// <summary>
    /// Параметр пока не задействован.
    /// </summary>
    [XmlAttribute ("mod")]
    [JsonPropertyName ("mod")]
    public string? ModByDicAuto { get; set; }

    /// <summary>
    /// Логические операторы.
    /// </summary>
    [XmlAttribute ("logic")]
    [JsonPropertyName ("logic")]
    public SearchLogicType Logic { get; set; }

    /// <summary>
    /// Правила автоматического расширения поиска
    /// на основе авторитетного файла или тезауруса.
    /// </summary>
    /// <remarks>Параметр имеет следующую структуру:
    /// Dbname,Prefix,Format.
    /// </remarks>
    [XmlAttribute ("advance")]
    [JsonPropertyName ("advance")]
    public string? Advance { get; set; }

    /// <summary>
    /// Имя формата показа документов.
    /// </summary>
    [XmlAttribute ("format")]
    [JsonPropertyName ("format")]
    public string? Format { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Clone the <see cref="SearchScenario"/>.
    /// </summary>
    public SearchScenario Clone() => (SearchScenario)MemberwiseClone();

    /// <summary>
    /// Parse INI file for scenarios.
    /// </summary>
    public static SearchScenario[] ParseIniFile
        (
            IniFile iniFile
        )
    {
        Sure.NotNull (iniFile);

        var section = iniFile["SEARCH"];
        if (ReferenceEquals (section, null))
        {
            return Array.Empty<SearchScenario>();
        }

        int count = section.GetValue ("ItemNumb", 0);
        if (count == 0)
        {
            return Array.Empty<SearchScenario>();
        }

        List<SearchScenario> result = new List<SearchScenario> (count);

        for (int i = 0; i < count; i++)
        {
            var name = section.GetValue
                (
                    "ItemName" + i,
                    null
                );
            if (string.IsNullOrEmpty (name))
            {
                Magna.Logger.LogError
                    (
                        nameof (SearchScenario) + "::" + nameof (ParseIniFile)
                        + ": item name not set: {Item}",
                        i
                    );

                throw new IrbisException ("Item name not set: " + i);
            }

            var scenario = new SearchScenario
            {
                Name = name,

                Prefix = section.GetValue
                    (
                        "ItemPref" + i,
                        string.Empty
                    ),

                DictionaryType = (DictionaryType)section.GetValue
                    (
                        "ItemDictionType" + i,
                        0
                    ),

                Advance = section.GetValue
                    (
                        "ItemAdv" + i,
                        null
                    ),

                Format = section.GetValue
                    (
                        "ItemPft" + i,
                        null
                    ),

                Hint = section.GetValue
                    (
                        "ItemHint" + i,
                        null
                    ),

                Logic = (SearchLogicType)section.GetValue
                    (
                        "ItemLogic" + i,
                        0
                    ),

                MenuName = section.GetValue
                    (
                        "ItemMenu" + i,
                        null
                    ),

                ModByDicAuto = section.GetValue
                    (
                        "ItemModByDicAuto" + i,
                        null
                    ),

                Correction = section.GetValue
                    (
                        "ModByDic" + i,
                        null
                    ),

                Truncation = Convert.ToBoolean
                    (
                        section.GetValue ("ItemTranc" + i, 0)
                    )
            };

            result.Add (scenario);
        }

        return result.ToArray();
    }

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        Name = reader.ReadNullableString();
        Prefix = reader.ReadNullableString();
        DictionaryType = (DictionaryType)reader.ReadPackedInt32();
        MenuName = reader.ReadNullableString();
        OldFormat = reader.ReadNullableString();
        Correction = reader.ReadNullableString();
        Hint = reader.ReadNullableString();
        ModByDicAuto = reader.ReadNullableString();
        Logic = (SearchLogicType)reader.ReadPackedInt32();
        Advance = reader.ReadNullableString();
        Format = reader.ReadNullableString();
        Truncation = reader.ReadBoolean();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer
            .WriteNullable (Name)
            .WriteNullable (Prefix)
            .WritePackedInt32 ((int)DictionaryType)
            .WriteNullable (MenuName)
            .WriteNullable (OldFormat)
            .WriteNullable (Correction)
            .WriteNullable (Hint)
            .WriteNullable (ModByDicAuto)
            .WritePackedInt32 ((int)Logic)
            .WriteNullable (Advance)
            .WriteNullable (Format)
            .Write (Truncation);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify" />
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<SearchScenario>
            (
                this,
                throwOnError
            );

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString() =>
        $"{Prefix.ToVisibleString()} {Name.ToVisibleString()}";

    #endregion
}
