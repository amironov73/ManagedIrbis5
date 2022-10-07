// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* GblStatement.cs -- оператор глобальной корректировки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl;

//
// EXTRACT FROM OFFICIAL DOCUMENTATION
//
// Файл задания на пакетную корректировку
// представляет собой текстовый файл с расширением GBL,
// содержит последовательность операторов корректировки,
// каждый из которых состоит из нескольких строк.
//
// Операторы выполняются в порядке их следования,
// причем каждый оператор использует значения полей
// и/или подполей измененных, возможно, предыдущими операторами.
//
// Первая строка файла задания – это число, задающее
// количество параметров, используемых в операторах корректировки.
//
// Последующие пары строк, число пар должно быть равно
// количеству параметров, используются программой
// глобальной корректировки.
//
// Первая строка пары - значение параметра или пусто,
// если пользователю предлагается задать его значение
// перед выполнением корректировки. В этой строке можно
// задать имя файла меню (с расширением MNU)
// или имя рабочего листа подполей (с расширением Wss),
// которые будут поданы для выбора значения параметра.
// Вторая строка пары – наименование параметра,
// которое появится в названии столбца, задающего параметр.
//
// Группы строк, описывающих операторы корректировки
// Далее следуют группы строк, описывающих операторы корректировки.
//
// Первая строка каждой группы – это имя оператора,
// которое может иметь одно из значений: ADD, REP, CHA, CHAC,
// DEL, DELR, UNDEL, CORREC, NEWMFN, END, IF, FI, ALL,
// EMPTY, REPEAT, UNTIL, //.
//
// Количество строк, описывающих оператор, зависит от его назначения.
// Операторы ADD, REP, CHA, CHAC, DEL описываются пятью строками,
// в которых задаются  следующие элементы:
// ИМЯ ОПЕРАТОРА
// МЕТКА ПОЛЯ/ПОДПОЛЯ: число, обозначающее метку поля,
// + разделитель подполя + обозначение подполя.
// Разделитель подполя с обозначением могут отсутствовать
// ПОВТОРЕНИЕ ПОЛЯ
// * - если корректируются все повторения
// F - если используется корректировка по формату
// N (число) – если корректируется N-ое повторение поля
// L – если корректируется последнее повторение поля
// L-N ( число) – если корректируется N-ое с конца повторение поля
// ФОРМАТ 1 – формат
// ФОРМАТ 2 - формат
//
// Для каждого конкретного оператора элементы ФОРМАТ 1
// и ФОРМАТ 2 имеют свое назначение. Некоторые из элементов
// могут не задаваться, когда в конкретной конфигурации
// они не имеют смысла. Тогда соответствующая строка
// в задании должна быть пустой или занята символом-заполнителем,
// как это формирует программа глобальной корректировки.
//
// Содержимое строк остальных операторов определяется
// их назначением и представлено в описании операторов.

/// <summary>
/// Оператор глобальной корректировки со всеми относящимися
/// к нему данными.
/// </summary>
[XmlRoot ("statement")]
[DebuggerDisplay ("{Command} {Parameter1} {Parameter2}")]
public sealed class GblStatement
    : IHandmadeSerializable,
    IVerifiable
{
    #region Constants

    /// <summary>
    /// Разделитель элементов в протоколе ИРБИС64.
    /// </summary>
    public const string Delimiter = "\x1F\x1E";

    #endregion

    #region Properties

    /// <summary>
    /// Команда (оператор), например, ADD или DEL.
    /// </summary>
    [XmlElement ("command")]
    [JsonPropertyName ("command")]
    public string? Command { get; set; }

    /// <summary>
    /// Первый параметр, как правило, спецификация поля/подполя.
    /// </summary>
    [XmlElement ("parameter1")]
    [JsonPropertyName ("parameter1")]
    public string? Parameter1 { get; set; }

    /// <summary>
    /// Второй параметр, как правило, спецификация повторения.
    /// </summary>
    [XmlElement ("parameter2")]
    [JsonPropertyName ("parameter2")]
    public string? Parameter2 { get; set; }

    /// <summary>
    /// Первый формат, например, выражение для замены.
    /// </summary>
    [XmlElement ("format1")]
    [JsonPropertyName ("format1")]
    public string? Format1 { get; set; }

    /// <summary>
    /// Второй формат, например, заменяющее выражение.
    /// </summary>
    [XmlElement ("format2")]
    [JsonPropertyName ("format2")]
    public string? Format2 { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Кодирование оператора глобальной корректировки
    /// для протокола обмена с сервером ИРБИС64.
    /// </summary>
    public string EncodeForProtocol()
    {
        var builder = StringBuilderPool.Shared.Get();

        builder.Append (Command);
        builder.Append (Delimiter);
        builder.Append (Parameter1);
        builder.Append (Delimiter);
        builder.Append (Parameter2);
        builder.Append (Delimiter);
        builder.Append (Format1);
        builder.Append (Delimiter);
        builder.Append (Format2);
        builder.Append (Delimiter);

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    /// <summary>
    /// Parse the stream.
    /// </summary>
    public static GblStatement? ParseStream
        (
            TextReader reader
        )
    {
        var command = reader.ReadLine();
        if (string.IsNullOrEmpty (command))
        {
            return null;
        }

        var result = new GblStatement
        {
            Command = command.Trim(),
            Parameter1 = reader.RequireLine(),
            Parameter2 = reader.RequireLine(),
            Format1 = reader.RequireLine(),
            Format2 = reader.RequireLine()
        };

        return result;
    }

    #endregion

    #region IHandmadeSerializable members

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Command = reader.ReadNullableString();
        Parameter1 = reader.ReadNullableString();
        Parameter2 = reader.ReadNullableString();
        Format1 = reader.ReadNullableString();
        Format2 = reader.ReadNullableString();
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        writer.WriteNullable (Command);
        writer.WriteNullable (Parameter1);
        writer.WriteNullable (Parameter2);
        writer.WriteNullable (Format1);
        writer.WriteNullable (Format2);
    }

    #endregion

    #region IVerifiable

    /// <inheritdoc cref="IVerifiable.Verify" />
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<GblStatement>
            (
                this,
                throwOnError
            );

        verifier
            .NotNullNorEmpty(Command, nameof (Command));

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString() =>
        $"Command: {Command},{Environment.NewLine}" + $"Parameter1: {Parameter1},{Environment.NewLine}" +
        $"Parameter2: {Parameter2},{Environment.NewLine}" + $"Format1: {Format1},{Environment.NewLine}" +
        $"Format2: {Format2}";

    #endregion

}
