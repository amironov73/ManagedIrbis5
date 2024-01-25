// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ReplaceSliceWithRangeIndexer
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* ValueField.cs -- поле библиографической записи, оформленное как структура
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis;

/// <summary>
/// Поле библиографической записи, оформленное как структура.
/// </summary>
public readonly struct ValueField
    : IVerifiable
{
    #region Constants

    /// <summary>
    /// Специальный код, зарезервированный для
    /// значения поля до первого разделителя.
    /// </summary>
    private const char ValueCode = '\0';

    /// <summary>
    /// Нет тега, т. е. тег ещё не присвоен.
    /// </summary>
    public const int NoTag = 0;

    /// <summary>
    /// Разделитель подполей.
    /// </summary>
    public const char Delimiter = '^';

    /// <summary>
    /// Количество индикаторов поля.
    /// </summary>
    public const int IndicatorCount = 2;

    #endregion

    #region Properties

    /// <summary>
    /// Метка поля.
    /// </summary>
    [XmlAttribute ("tag")]
    [JsonPropertyName ("tag")]
    public readonly int Tag;

    /// <summary>
    /// Подполя.
    /// </summary>
    public readonly Memory<ValueSubField> Subfields;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="tag">Метка поля.</param>
    /// <param name="subfields">Подполя.</param>
    public ValueField
        (
            int tag,
            Memory<ValueSubField> subfields
        )
    {
        Tag = tag;
        Subfields = subfields;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Деконструкция поля.
    /// </summary>
    public void Deconstruct
        (
            out int tag,
            out Memory<ValueSubField> subfields
        )
    {
        tag = Tag;
        subfields = Subfields;
    }

    /// <summary>
    /// Перечисление подполей с указанным кодом.
    /// </summary>
    public IEnumerable<ValueSubField> EnumerateSubFields
        (
            char code
        )
    {
        if (code == ValueCode)
        {
            var value = GetValueSubField();
            if (!value.IsEmpty)
            {
                yield return value.Span[0];
            }

            yield break;
        }

        if (code == '*')
        {
            if (Subfields.IsEmpty)
            {
                yield return Subfields.Span[0];
            }

            yield break;
        }

        for (var i = 0; i < Subfields.Length; i++)
        {
            if (Subfields.Span[i].Code.SameChar (code))
            {
                yield return Subfields.Span[i];
            }
        }
    }

    /// <summary>
    /// Получение первого подполя с указанным кодом.
    /// </summary>
    public Memory<ValueSubField> GetFirstSubField
        (
            char code
        )
    {
        if (code == ValueCode)
        {
            if (Subfields.IsEmpty)
            {
                return null;
            }

            if (Subfields.Span[0].Code == ValueCode)
            {
                return Subfields[..1];
            }

            // дальше первого элемента искать не имеет смысла

            return null;
        }

        if (code == '*')
        {
            if (Subfields.IsEmpty)
            {
                return null;
            }

            return Subfields[..1];
        }

        for (var i = 0; i < Subfields.Length; i++)
        {
            if (Subfields.Span[i].Code.SameChar (code))
            {
                return Subfields[i .. (i + 1)];
            }
        }

        return null;
    }

    /// <summary>
    /// Выдает указанное повторение подполя с данным кодом.
    /// </summary>
    /// <param name="code">Искомый код подполя.</param>
    /// <param name="occurrence">Номер повторения.</param>
    /// <returns>Найденное повторение либо пустой диапазон.</returns>
    public Memory<ValueSubField> GetSubField
        (
            char code,
            int occurrence = 0
        )
    {
        if (code == ValueCode)
        {
            if (occurrence != 0)
            {
                return null;
            }

            return GetValueSubField();
        }

        if (code == '*')
        {
            if (occurrence != 0 || Subfields.IsEmpty)
            {
                return null;
            }

            return Subfields[..1];
        }

        if (occurrence < 0)
        {
            // отрицательные индексы отсчитываются от конца
            //occurrence = Subfields.Span.Count (sf => sf.Code.SameChar (code)) + occurrence;
            //if (occurrence < 0)
            //{
                return null;
            //}
        }

        for (var i = 0; i < Subfields.Length; i++)
        {
            if (Subfields.Span[i].Code.SameChar (code))
            {
                if (occurrence == 0)
                {
                    return Subfields[i .. (i + 1)];
                }

                --occurrence;
            }
        }

        return null;
    }

    /// <summary>
    /// Получение текста указанного подполя.
    /// </summary>
    /// <param name="code">Искомый код подполя.</param>
    /// <param name="occurrence">Номер повторения.
    /// Нумерация начинается с нуля.
    /// Отрицательные индексы отсчитываются с конца массива.</param>
    /// <returns>Текст найденного подполя или <c>null</c>.</returns>
    public Memory<char> GetSubFieldValue
        (
            char code,
            int occurrence = 0
        )
    {
        var subfield = GetSubField (code, occurrence);

        return subfield.IsEmpty ? Memory<char>.Empty : subfield.Span[0].Value;
    }

    /// <summary>
    /// Поиск подполя '*' (имитация ИРБИС).
    /// </summary>
    public Memory<char> GetValueOrFirstSubField()
    {
        return Subfields.IsEmpty ? Memory<char>.Empty : Subfields.Span[0].Value;
    }

    /// <summary>
    /// Получаем подполе, выделенное для хранения
    /// значения поля до первого разделителя.
    /// </summary>
    public Memory<ValueSubField> GetValueSubField()
    {
        if (Subfields.IsEmpty)
        {
            return null;
        }

        if (Subfields.Span[0].Code == ValueCode)
        {
            return Subfields[..1];
        }

        return null;
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<ValueField> (this, throwOnError);

        verifier.Positive (Tag);
        verifier.Positive (Subfields.Length);
        foreach (var subfield in Subfields.Span)
        {
            subfield.Verify (throwOnError);
        }

        if (verifier.Result)
        {
            for (var i = 1; i < Subfields.Length; i++)
            {
                if (Subfields.Span[i].Code == ValueCode)
                {
                    verifier.Failure ("Value field is not first");
                }
            }
        }

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.Append (Tag.ToInvariantString())
            .Append ('#');
        for (var i = 0; i < Subfields.Length; i++)
        {
            builder.Append (Subfields.Span[i].ToString());
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
