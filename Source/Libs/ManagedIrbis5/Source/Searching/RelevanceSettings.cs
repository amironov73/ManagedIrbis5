// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* RelevanceSettings.cs -- настройки для оценки релевантности
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Text.Json.Serialization;

using AM;
using AM.Json;

#endregion

#nullable enable

namespace ManagedIrbis.Searching;

/// <summary>
/// Настройки для оценки релевантности.
/// </summary>
public sealed class RelevanceSettings
{
    #region Properties

    /// <summary>
    /// Коэффициенты.
    /// </summary>
    [JsonPropertyName ("coefficients")]
    public IList<RelevanceCoefficient> Coefficients { get; set; }
        = new List<RelevanceCoefficient>();

    /// <summary>
    /// Релеватность для упоминаний в посторонних полях.
    /// </summary>
    [JsonPropertyName ("extraneous")]
    public double ExtraneousRelevance { get; set; }

    /// <summary>
    /// Мультипликатор для случая полного совпадения.
    /// </summary>
    [JsonPropertyName ("multiplier")]
    public double Multiplier { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Настройки по умолчанию для базы IBIS.
    /// </summary>
    public static RelevanceSettings ForIbis()
    {
        var result = new RelevanceSettings
        {
            ExtraneousRelevance = 1,
            Multiplier = 2,
            Coefficients =
            {
                // заглавие или авторы
                new (10)
                {
                    Fields =
                    {
                        200, // основное заглавие
                        700, 701, // индивидуальные авторы
                        710, 711, 971, 972, // коллективные авторы
                        923, // выпуск, часть
                        922, // статья сборника
                        925, // несколько томов в одной книге
                        961, // индивидуальные авторы общей части
                        962, // коллективы общей части
                        461, // заглавие общей части
                        463, // издание, в котором опубликована статья
                    }
                },

                // редакторы
                new (7) { Fields = { 702 }},

                // прочие заглавия
                new (6)
                {
                    Fields =
                    {
                        510, // параллельное заглавие
                        517, // разночтение заглавия
                        541, // перевод заглавия
                        924, // "другое" заглавие
                        921, // транслитерированное заглавие
                    }
                },

                // содержание
                new (6)
                {
                    Fields =
                    {
                        330, // оглавление
                        922, // статья из журнала
                    }
                },

                // рубрики
                new (5)
                {
                    Fields =
                    {
                        606, // предметная рубрика
                        607, // географическая рубрика
                        600, 601, // персоналия
                        965, // дескриптор
                    }
                },

                // серия
                new (4) { Fields = { 225 } },

                // ключевые слова и аннотации
                new (3)
                {
                    Fields =
                    {
                        610, // ненормированное ключевое слово
                        331, // аннотация
                    }
                },
            }
        };

        return result;
    }

    /// <summary>
    /// Загрузка настроек из указанного файла.
    /// </summary>
    public static RelevanceSettings Load
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        var result = JsonUtility.ReadObjectFromFile<RelevanceSettings> (fileName);

        return result;
    }

    /// <summary>
    /// Сохранение настроек в указанный файл.
    /// </summary>
    public void Save
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        JsonUtility.SaveObjectToFile (this, fileName);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => JsonUtility.SerializeIndented (this);

    #endregion
}
