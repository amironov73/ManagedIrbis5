// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* OperativeMode.cs -- оперативный режим
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Workspace;

/// <summary>
/// <para>Оперативный режим.</para>
/// <para>Под ОПЕРАТИВНЫМИ РЕЖИМАМИ понимаются режимы АРМ Каталогизатор,
/// которые предлагаются пользователю ситуативно - т.е. в зависимости
/// от содержания ТЕКУЩЕГО документа - в форме плавающего окна с кнопками.</para>
/// <para>В версиях до 2012.1 включительно предлагаются "ЖЕСТКИЕ" оперативные режимы,
/// с помощью которых реализуются технологии описания периодических изданий
/// и аналитической росписи сборников .</para>
/// <para>Начиная с версии 2013.1 пользователю предоставляется возможность
/// формировать оперативные режимы по собственному усмотрению.</para>
/// </summary>
[Serializable]
public sealed class OperativeMode
    : IHandmadeSerializable,
    IVerifiable
{
    #region Properties

    /// <summary>
    /// Вид оперативного режима.
    /// </summary>
    /// <remarks>
    /// Вид оперативных режимов ("жесткие" или произвольные) определяется
    /// значением ПЕРВОЙ строки результата форматирования, которое может быть следующим:
    /// <list type="table">
    /// <item>
    ///     <term>0</term>
    ///     <description>"жесткие" оперативные режимы для СБОРНИКОВ</description>
    /// </item>
    /// <item>
    ///     <term>1</term>
    /// <description>"жесткие" оперативные режимы для ПЕРИОДИКИ</description>
    /// </item>
    /// <item>
    ///     <term>2</term>
    ///     <description>ПРОИЗВОЛЬНЫЕ оперативные режимы</description>
    /// </item>
    /// </list>
    /// </remarks>
    [XmlElement ("kind")]
    [JsonPropertyName ("kind")]
    [DisplayName ("Вид")]
    [Description ("Вид оперативного режима")]
    public string? Kind { get; set; }

    /// <summary>
    /// Надпись на кнопке (может быть пустой).
    /// </summary>
    [XmlElement ("title")]
    [JsonPropertyName ("title")]
    [DisplayName ("Надпись")]
    [Description ("Надпись на кнопке")]
    public string? Title { get; set; }

    /// <summary>
    /// Подсказка на кнопке (появляется при наведении мыши) (может быть пустой).
    /// </summary>
    [XmlElement ("hint")]
    [JsonPropertyName ("hint")]
    [DisplayName ("Подсказка")]
    [Description ("Появляется при наведении мыши")]
    public string? Hint { get; set; }

    /// <summary>
    /// Иконка для кнопки: номер в списке внутренних образов (может быть пустой).
    /// </summary>
    [XmlElement ("icon")]
    [JsonPropertyName ("icon")]
    [DisplayName ("Иконка")]
    [Description ("Номер в списке внутренних образов")]
    public string? Icon { get; set; }

    /// <summary>
    /// Код команды (на основе которой реализуется оперативный режим).
    /// </summary>
    [XmlElement ("command")]
    [JsonPropertyName ("command")]
    [DisplayName ("Команда")]
    [Description ("Код команды")]
    public string? CommandCode { get; set; }

    /// <summary>
    /// Параметры команды.
    /// </summary>
    [XmlElement ("parameters")]
    [JsonPropertyName ("parameters")]
    [DisplayName ("Параметры")]
    [Description ("Параметры команды")]
    public string? CommandParameters { get; set; }

    /// <summary>
    /// Текст финального сообщения в случае успешного завершения режима
    /// (может быть пустым).
    /// </summary>
    [XmlElement ("message")]
    [JsonPropertyName ("message")]
    [DisplayName ("Сообщение")]
    [Description ("Текст финального сообщения")]
    public string? SuccessMessage { get; set; }

    /// <summary>
    /// Зарезервировано.
    /// </summary>
    [XmlElement ("reserved")]
    [JsonPropertyName ("reserved")]
    [DisplayName ("Зарезервировано")]
    [Description ("Зарезервировано")]
    public string? Reserved { get; set; }

    /// <summary>
    /// Произвольные пользовательские данные.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    [field: NonSerialized]
    public object? UserData { get; set; }

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

        Kind = reader.ReadNullableString();
        Title = reader.ReadNullableString();
        Hint = reader.ReadNullableString();
        Icon = reader.ReadNullableString();
        CommandCode = reader.ReadNullableString();
        CommandParameters = reader.ReadNullableString();
        SuccessMessage = reader.ReadNullableString();
        Reserved = reader.ReadNullableString();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer
            .WriteNullable (Kind)
            .WriteNullable (Title)
            .WriteNullable (Hint)
            .WriteNullable (Icon)
            .WriteNullable (CommandCode)
            .WriteNullable (CommandParameters)
            .WriteNullable (SuccessMessage)
            .WriteNullable (Reserved);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<OperativeMode> (this, throwOnError);

        verifier
            .AnyNotNullNorEmpty
                (
                    Title,
                    CommandCode
                );

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return (Title ?? CommandCode).ToVisibleString();
    }

    #endregion
}
