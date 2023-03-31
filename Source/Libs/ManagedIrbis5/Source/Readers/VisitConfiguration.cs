// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* VisitConfiguration.cs -- конфигурация поля 40
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace ManagedIrbis.Readers;

/// <summary>
/// Конфигурация поля <c>40</c> в базе данных <c>RDR</c>.
/// </summary>
[PublicAPI]
public sealed class VisitConfiguration
{
    #region Properties

    /// <summary>
    /// Поле записи в БД RDR, используемое для хранения
    /// информации.о посещениях и книговыдачи.
    /// В дистрибутиве это поле 40.
    /// </summary>
    [JsonPropertyName ("tag")]
    [XmlAttribute ("tag")]
    [DefaultValue (40)]
    [DisplayName ("Поле с посещениями и выдачами")]
    [Description ("Поле записи в БД RDR, используемое для хранения "
                  + "информации о посещениях и книговыдаче. Как правило, это поле 40.")]
    public int Tag { get; set; } = 40;

    /// <summary>
    /// Код подполя, хранящего имя БД каталога.
    /// </summary>
    public char DatabaseSubfieldCode { get; set; } = 'g';

    /// <summary>
    /// Код подполя, хранящего шифр документа.
    /// </summary>
    public char IndexSubfieldCode { get; set; } = 'a';

    /// <summary>
    /// Код подполя, хранящего инвентарный номер экземпляра.
    /// </summary>
    public char InventorySubfieldCode { get; set; } = 'b';

    /// <summary>
    /// Код подполя, хранящего штрих-код экземпляра.
    /// </summary>
    public char BarcodeSubfieldCode { get; set; } = 'h';

    /// <summary>
    /// Код подполя, хранящего место хранения экземпляра.
    /// </summary>
    public char SiglaSubfieldCode { get; set; } = 'k';

    /// <summary>
    /// Код подполя, хранящего дату выдачи.
    /// </summary>
    public char DateGivenSubfieldCode { get; set; }


    /// <summary>
    /// Код подполя, хранящего место выдачи.
    /// </summary>
    public char DepartmentSubfieldCode { get; set; } = 'y';

    /// <summary>
    /// Код подполя, хранящего дату предполагаемого возврата.
    /// </summary>
    public char DateExpectedSubfieldCode { get; set; } = 'e';

    /// <summary>
    /// Код подполя, хранящего дату фактического возврата.
    /// </summary>
    public char DateReturnedSubfieldCode { get; set; } = 'f';

    /// <summary>
    /// Код подполя, хранящего дату продления.
    /// </summary>
    public char DateProlongSubfieldCode { get; set; } = 'l';

    /// <summary>
    /// Код подполя, хранящего признак утерянного экземпляра.
    /// </summary>
    public char LostSubfieldCode { get; set; } = 'u';

    /// <summary>
    /// Код подполя, хранящего краткое библиографическое описание.
    /// </summary>
    public char DescriptionSubfieldCode { get; set; } = 'c';

    /// <summary>
    /// Код подполя, хранящего имя ответственного лица.
    /// </summary>
    public char ResponsibleSubfieldCode { get; set; } = 't';

    /// <summary>
    /// Код подполя, хранящего время начало визита в библиотеку.
    /// </summary>
    public char TimeInSubfieldCode { get; set; } = '1';

    /// <summary>
    /// Код подполя, хранящего время окончания визита в библиотеку.
    /// </summary>
    public char TimeOutSubfieldCode { get; set; } = '2';

    /// <summary>
    /// Код подполя, хранящего счетчик продлений.
    /// </summary>
    public char ProlongCountSubfieldCode { get; set; } = '4';

    #endregion

    #region Public methods

    /// <summary>
    /// Получение конфигурации по умолчанию.
    /// </summary>
    public static VisitConfiguration GetDefault() => new ();

    #endregion

    #region ICloneable members

    /// <summary>
    /// Создание клона конфигурации.
    /// </summary>
    public VisitConfiguration Clone()
        => (VisitConfiguration) MemberwiseClone();

    #endregion
}
