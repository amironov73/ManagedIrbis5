// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global

/* LngFile.cs -- лингвистический файл
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Linq;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Infrastructure;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Lng;

//
// Лингвистические файлы расположены в директории
// cgi\irbis64r_XX, рисунки с изображениями флажков
// в папке htdocs\irbis64r_XX\images\flags.
// Изображения должны иметь расширения PNG,
// лингвистические файлы — LNG. Имена файлов
// и изображений должны соответствовать кодам языков
// в файле lng.mnu.
//
// Лингвистический файл аналогичен по структуре
// файлу справочника, но имеет кодировку UTF8
// и состоит из парных строк:
// строка текста на русском языке
// строка текста на национальном языке
//
// Для нормальной работы системы такая структура
// файла должна неукоснительно соблюдаться.
// При отсутствии перевода после строки
// на русском языке необходимо обязательно оставлять
// пустую строку.
//
// Все русскоязычные литералы (любые текстовые строки
// на русском) в фреймах, форматах и MNU файлах
// WEB ИРБИС обрамлены двойными тильдами
// (например: ~~Русский язык~~). Тильды определят
// фрагмент текста как потенциальную константу
// для замены.При переключении на альтернативный
// язык интерфейса, наличии лингвистического файла
// и перевода этот литерал заменяется на национальный
// аналог. В противном случае тильды удаляются шлюзом,
// и литерал выводится без изменений.
//

//
// Example: uk.lng
//
// Абхазский
// Абхазька
// Август
// Серпень
// Австралия
// Австралія
// Австрия
// Австрія
// Автор(ы)
// Автор(и)
// Автор, Вид издания,
// Автор, Різновид видання,
// Автор, редактор, составитель
// Автор, редактор, упорядник
//

/// <summary>
/// Лингвистический файл.
/// </summary>
public sealed class LngFile
    : IHandmadeSerializable,
        IVerifiable
{
    #region Properties

    /// <summary>
    /// Имя файла.
    /// </summary>
    public string? Name { get; set; }

    #endregion

    #region Private members

    private readonly CaseInsensitiveDictionary<string> _dictionary = new ();

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление пары "оригинал-перевод".
    /// </summary>
    public LngFile Add
        (
            string original,
            string translation
        )
    {
        Sure.NotNull (original);
        Sure.NotNull (translation);

        _dictionary.Add (original, translation);

        return this;
    }

    /// <summary>
    /// Очистка словаря.
    /// </summary>
    public void Clear() => _dictionary.Clear();

    /// <summary>
    /// Получение перевода текста.
    /// </summary>
    public string GetTranslation
        (
            string text
        )
    {
        _dictionary.TryGetValue (text, out var result);

        if (string.IsNullOrEmpty (result))
        {
            result = text;
        }

        return result;
    } // method GetTranslation

    /// <summary>
    /// Чтение из текстового потока <see cref="TextReader"/>.
    /// </summary>
    public void ParseText
        (
            TextReader reader
        )
    {
        Sure.NotNull (reader);

        while (reader.ReadLine() is { } key)
        {
            if (_dictionary.ContainsKey (key))
            {
                Magna.Logger.LogError
                    (
                        nameof (LngFile) + "::" + nameof (ParseText)
                        + ": duplicate key: {Key}",
                        key
                    );
            }

            string? value;
            if ((value = reader.ReadLine()) == null)
            {
                break;
            }

            _dictionary[key] = value;
        }

        Magna.Logger.LogTrace
            (
                nameof (LngFile) + "::" + nameof (ParseText)
                + ": keys: {Count}",
                _dictionary.Count
            );
    }

    /// <summary>
    /// Чтение из локального файла.
    /// </summary>
    public static LngFile ReadLocalFile
        (
            string fileName
        )
    {
        var result = new LngFile { Name = fileName };

        using var reader = new StreamReader (fileName, IrbisEncoding.Utf8);
        result.ParseText (reader);

        return result;
    } // method ReadLocalFile

    /// <summary>
    /// Сохранение содержимого в текстовый поток <see cref="TextWriter"/>.
    /// </summary>
    public void WriteTo
        (
            TextWriter writer
        )
    {
        Sure.NotNull (writer);

        foreach (var pair in _dictionary.OrderBy (p => p.Key))
        {
            writer.WriteLine (pair.Key);
            writer.WriteLine (pair.Value);
        }
    } // method WriteTo

    /// <summary>
    /// Сохранение содержимого в локальный файл.
    /// </summary>
    public void WriteLocalFile
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        using var writer = new StreamWriter (fileName, false, IrbisEncoding.Utf8);
        WriteTo (writer);
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
        Clear();
        while (true)
        {
            var key = reader.ReadNullableString();
            if (ReferenceEquals (key, null))
            {
                break;
            }

            var value = reader.ReadNullableString();
            _dictionary[key] = value ?? string.Empty;
        }
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

        writer.WriteNullable (Name);
        foreach (var pair in _dictionary) //-V3087
        {
            writer.WriteNullable (pair.Key);
            writer.WriteNullable (pair.Value);
        }

        writer.WriteNullable ((string?)null);
    }

    #endregion

    #region IVerifiable members

    /// <inheritdoc cref="IVerifiable.Verify"/>
    public bool Verify
        (
            bool throwOnError
        )
    {
        var verifier = new Verifier<LngFile> (this, throwOnError);

        verifier
            .NotNullNorEmpty (Name)
            .NotNullNorEmpty (_dictionary);

        return verifier.Result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return Name.ToVisibleString();
    }

    #endregion
}
