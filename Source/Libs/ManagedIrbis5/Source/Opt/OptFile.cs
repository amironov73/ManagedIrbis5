// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global

/* OptFile.cs -- поддержка OPT-файлов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Linq;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Opt
{
    //
    // Оптимизированный формат – это механизм автоматического
    // переключения форматов показа документов в зависимости
    // от их вида. Переключение производится в соответствии
    // с содержанием специального файла, имя которого
    // определяется параметром PFTOPT. Данный файл оптимизации
    // является текстовым и имеет следующую структуру:
    //
    // <метка>|<формат>|@<имя_формата>
    // <длина>
    // <значение_1> <имя формата_1>
    // <значение_2> <имя формата_2>
    // <значение_3> <имя формата_3>
    // *****
    //
    // где:
    // <метка>|<формат>|@<имя_формата> - ключ, который может
    // задаваться тремя способами:
    // <метка> - метка поля, значение которого определяет
    // вид документа;
    // <формат> - непосредственный формат, с помощью которого
    // определяется значение для вида документа;
    // @<имя_формата> - имя формата с предшествующим символом @,
    // с помощью которого определяется значение для вида документа.
    // <длина> - макс.длина значения для вида документа;
    // <значение_n> <имя формата_n> - значение (вид документа)
    // и соответствующий ему формат, разделенные символом пробела.
    // При этом в элементе <значение_n> могут содержаться символы
    // маскирования «+» (означающие, что на соответствующем месте
    // может быть любой символ).
    // Для БД электронного каталога (IBIS) предлагаются два
    // оптимизационных файла:
    // PFTW.OPT – включает RTF-форматы;
    // PFTW_H.OPT – включает HTML-форматы.
    //

    //
    // В исходном состоянии системы в качестве оптимизированного
    // определены HTML-форматы (т.е. PFTOPT=PFTW_H.OPT).
    // Для перехода на RTF-форматы (в качестве оптимизированного)
    // необходимо установить PFTOPT=PFTW.OPT.
    //


    // 920
    // 5
    // PAZK  PAZK42
    // PVK   PVK42
    // SPEC  SPEC42
    // J     !RPJ51
    // NJ    !NJ31
    // NJP   !NJ31
    // NJK   !NJ31
    // AUNTD AUNTD42
    // ASP   ASP42
    // MUSP  MUSP
    // SZPRF SZPRF
    // BOUNI BOUNI
    // IBIS  IBIS
    // +++++ PAZK42
    // *****

    /// <summary>
    /// Поддержка OPT-файлов.
    /// </summary>
    public sealed class OptFile
        : IHandmadeSerializable
    {
        #region Constants

        /// <summary>
        /// Подстановочный символ.
        /// </summary>
        public const char Wildcard = '+';

        #endregion

        #region Properties

        /// <summary>
        /// Элементы списка.
        /// </summary>
        public NonNullCollection<OptLine> Lines { get; set; } = new();

        /// <summary>
        /// Length of worksheet.
        /// </summary>
        public int WorksheetLength { get; private set; }

        /// <summary>
        /// Tag that identifies worksheet.
        /// Common used: 920
        /// </summary>
        public int WorksheetTag { get; private set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Получаем рабочий лист для указанной записи.
        /// </summary>
        public string? GetWorksheet (Record record) => record.FM(WorksheetTag);

        /// <summary>
        /// Загружаем из OPT-файла.
        /// </summary>
        public static OptFile LoadFile
            (
                string filePath
            )
        {
            using var reader = TextReaderUtility.OpenRead
                (
                    filePath,
                    IrbisEncoding.Ansi
                );

            return ParseText(reader);
        }

        /// <summary>
        /// Разбор текста.
        /// </summary>
        public static OptFile ParseText
            (
                TextReader reader
            )
        {
            var result = new OptFile();

            result.SetWorksheetTag(reader.RequireLine().Trim().ParseInt32());
            result.SetWorksheetLength(int.Parse(reader.RequireLine().Trim()));

            while (true)
            {
                string line = reader.RequireLine().Trim();
                if (line.StartsWith("*"))
                {
                    break;
                }

                var item = OptLine.Parse(line);
                if (item is not null)
                {
                    result.Lines.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Выбор рабочего листа.
        /// </summary>
        public string SelectWorksheet
            (
                string? tagValue
            )
        {
            foreach (var line in Lines)
            {
                if (line.Compare(tagValue))
                {
                    return line.Value.ThrowIfNull("item.Value");
                }
            }

            Magna.Error
                (
                    "OptFile::SelectWorksheet: "
                    + "can't select worksheet for "
                    + tagValue.ToVisibleString()
                );

            throw new IrbisException("Can't select worksheet");
        }

        /// <summary>
        /// Создание OPT-файла по описанию.
        /// </summary>
        public void WriteFile
            (
                string fileName
            )
        {
            using StreamWriter writer = TextWriterUtility.Create
                (
                    fileName,
                    IrbisEncoding.Ansi
                );
            WriteOptFile(writer);
        }

        /// <summary>
        /// Создание OPT-файла по описанию.
        /// </summary>
        public void WriteOptFile
            (
                TextWriter writer
            )
        {
            writer.WriteLine(WorksheetTag);
            writer.WriteLine(WorksheetLength);
            foreach (var line in Lines)
            {
                writer.WriteLine
                    (
                        "{0} {1}",
                        line.Key.ThrowIfNull("item.Key")
                            .PadRight(WorksheetLength),
                        line.Value
                    );
            }
            writer.WriteLine("*****");
        }

        /// <summary>
        /// Установка длины названия рабочего листа.
        /// </summary>
        public void SetWorksheetLength
            (
                int length
            )
        {
            if (length <= 0)
            {
                Magna.Error
                    (
                        "OptFile::SetWorksheetLength: "
                        + "length="
                        + length
                    );

                throw new ArgumentOutOfRangeException(nameof(length));
            }

            WorksheetLength = length;
        }

        /// <summary>
        /// Установка поля для рабочего листа.
        /// </summary>
        public void SetWorksheetTag
            (
                int tag
            )
        {
            WorksheetTag = tag;
        }

        /// <summary>
        /// Проверка на валидность. OPT должен содержать
        /// одну строку с плюсами. Она должна быть последней.
        /// OPT не должен быть пустым. Длина ключей в элементах
        /// не должна превышать <see cref="WorksheetLength"/>.
        /// Не должно быть одинаковых ключей.
        /// </summary>
        public bool Validate
            (
                bool throwException
            )
        {
            var result = Lines.Count != 0;

            if (result)
            {
                var count = 0;
                foreach (var line in Lines)
                {
                    if (line.Key.ConsistOf(Wildcard))
                    {
                        count++;
                    }
                }

                result = count == 1;
            }

            if (result)
            {
                result = Lines.Last().Key.ConsistOf(Wildcard);
            }

            if (result)
            {
                result = Lines.All
                    (
                        item => item.Key.ThrowIfNull("item.Key")
                            .Length <= WorksheetLength
                    );
            }

            if (result)
            {
                result = Lines
                    .GroupBy(item => item.Key.ThrowIfNull("item.Key").ToUpper())
                    .Count(grp => grp.Count() > 1)
                    == 0;
            }

            if (!result && throwException)
            {
                throw new IrbisException("OPT not valid");
            }

            return result;
        }

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Lines = reader.ReadNonNullCollection<OptLine>();
            WorksheetLength = reader.ReadPackedInt32();
            WorksheetTag = reader.ReadPackedInt32();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer.Write(Lines);
            writer.WritePackedInt32(WorksheetLength);
            writer.WritePackedInt32(WorksheetTag);
        }

        #endregion

    } // class OptFile

} // namespace ManagedIrbis.Opt
