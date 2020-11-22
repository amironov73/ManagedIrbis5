// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ParFile.cs -- работа с PAR-файлами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Text;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    // Official documentation:
    //
    // Каждой базе данных ИРБИС соответствует один .par-файл.
    // Этот файл содержит набор путей к файлам базы данных ИРБИС.
    // Имя .par-файла соответствует имени базы данных.
    //
    // .par-файл представляет собой текстовый файл, состоящий
    // из 11 строк. Каждая строка представляет собой путь,
    // указывающий местонахождение соответствующих файлов базы данных.
    // Примечание: до версии 2011.1 включительно .par-файлы включают
    // в себя 10 строк. 11-я строка добавлена в версии 2012.1.
    //
    // В исходном состоянии системы .par-файл содержит относительные
    // пути размещения файлов базы данных – относительно основной
    // директории системы <IRBIS_SERVER_ROOT>.
    //
    // Фактически в ИРБИС принят принцип хранения всех файлов
    // базы данных в одной папке, поэтому .par-файлы содержат
    // один и тот же путь, повторяющийся в каждой строке.

    // Как правило, PAR-файлы располагаются в подпапке DataI внутри
    // папки IRBIS64, в которую установлен сервер ИРБИС
    // (но их расположение может быть переопределено параметром
    // DataPath в irbis_server.ini).

    // Пример файла IBIS.PAR:
    //
    // 1=.\datai\ibis\
    // 2=.\datai\ibis\
    // 3=.\datai\ibis\
    // 4=.\datai\ibis\
    // 5=.\datai\ibis\
    // 6=.\datai\ibis\
    // 7=.\datai\ibis\
    // 8=.\datai\ibis\
    // 9=.\datai\ibis\
    // 10=.\datai\ibis\
    // 11=f:\webshare\

    // Параметр | Назначение
    //        1 | Путь к файлу XRF
    //        2 | MST
    //        3 | CNT
    //        4 | N01
    //        5 | N02 (только для ИРБИС32)
    //        6 | L01
    //        7 | L02 (только для ИРБИС32)
    //        8 | IFP
    //        9 | ANY
    //       10 | FDT, FST, FMT, PFT, STW, SRT
    //       11 | появился в версии 2012:
    //          | расположение внешних объектов (поле 951)


    /// <summary>
    /// PAR files handling.
    /// </summary>
    [XmlRoot("par")]
    public sealed class ParFile
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Standard extension for PAR files.
        /// </summary>
        public const string Extension = ".par";

        #endregion

        #region Properties

        /// <summary>
        /// Путь к файлу XRF
        /// </summary>
        [XmlAttribute("xrf")]
        [JsonPropertyName("xrf")]
        public string? XrfPath { get; set; }

        /// <summary>
        /// Путь к файлу MST
        /// </summary>
        [XmlAttribute("mst")]
        [JsonPropertyName("mst")]
        public string? MstPath { get; set; }

        /// <summary>
        /// Путь к файлу CNT
        /// </summary>
        [XmlAttribute("cnt")]
        [JsonPropertyName("cnt")]
        public string? CntPath { get; set; }

        /// <summary>
        /// Путь к файлу N01
        /// </summary>
        [XmlAttribute("n01")]
        [JsonPropertyName("n01")]
        public string? N01Path { get; set; }

        /// <summary>
        /// Путь к файлу N02
        /// </summary>
        [XmlAttribute("n02")]
        [JsonPropertyName("n02")]
        public string? N02Path { get; set; }

        /// <summary>
        /// Путь к файлу L01
        /// </summary>
        [XmlAttribute("l01")]
        [JsonPropertyName("l01")]
        public string? L01Path { get; set; }

        /// <summary>
        /// Путь к файлу L02
        /// </summary>
        [XmlAttribute("l02")]
        [JsonPropertyName("l02")]
        public string? L02Path { get; set; }

        /// <summary>
        /// Путь к файлу IFP
        /// </summary>
        [XmlAttribute("ifp")]
        [JsonPropertyName("ifp")]
        public string? IfpPath { get; set; }

        /// <summary>
        /// Путь к файлу ANY
        /// </summary>
        [XmlAttribute("any")]
        [JsonPropertyName("any")]
        public string? AnyPath { get; set; }

        /// <summary>
        /// Путь к файлам PFT
        /// </summary>
        [XmlAttribute("pft")]
        [JsonPropertyName("pft")]
        public string? PftPath { get; set; }

        /// <summary>
        /// Расположение внешних объектов (поле 951)
        /// </summary>
        /// <remarks>Параметр появился в версии 2012</remarks>
        [XmlAttribute("ext")]
        [JsonPropertyName("ext")]
        public string? ExtPath { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ParFile()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public ParFile
            (
                string mstPath
            )
        {
            Sure.NotNullNorEmpty(mstPath, nameof(mstPath));

            MstPath = mstPath;
            XrfPath = mstPath;
            CntPath = mstPath;
            L01Path = mstPath;
            L02Path = mstPath;
            N01Path = mstPath;
            N02Path = mstPath;
            IfpPath = mstPath;
            AnyPath = mstPath;
            PftPath = mstPath;
            ExtPath = mstPath;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор PAR-файла на строчки вида 1=.\datai\ibis.
        /// </summary>
        public static Dictionary<int, string> ReadDictionary
            (
                TextReader reader
            )
        {
            string? line;
            var result = new Dictionary<int, string>(11);
            while ((line = reader.ReadLine()) != null)
            {
                line = line.Trim();
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                var parts = line.Split(CommonSeparators.EqualSign, 2);

                if (parts.Length != 2)
                {
                    Magna.Error
                        (
                            nameof(ParseFile)
                            + "::"
                            + nameof(ReadDictionary)
                            + ": format error"
                        );

                    throw new FormatException();
                }
                var key = parts[0].Trim();
                var value = parts[1].Trim();
                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(value))
                {
                    Magna.Error
                        (
                            nameof(ParseFile)
                            + "::"
                            + nameof(ReadDictionary)
                            + ": format error"
                        );

                    throw new FormatException();
                }
                result.Add(int.Parse(key), value);
            }

            foreach (var key in Enumerable.Range(1, 10))
            {
                if (!result.ContainsKey(key))
                {
                    Magna.Error
                        (
                            nameof(ParseFile) + "::" + nameof(ReadDictionary)
                            + ": key not found: "
                            + key
                        );

                    throw new FormatException();
                }
            }

            return result;
        }

        /// <summary>
        /// Разбор файла.
        /// </summary>
        public static ParFile ParseFile
            (
                string fileName
            )
        {
            Sure.NotNullNorEmpty(fileName, nameof(fileName));

            using var reader = TextReaderUtility.OpenRead
                (
                    fileName,
                    IrbisEncoding.Ansi
                );
            return ParseText(reader);
        }

        /// <summary>
        /// Разбор текста.
        /// </summary>
        public static ParFile ParseText
            (
                TextReader reader
            )
        {
            var dictionary = ReadDictionary(reader);

            var result = new ParFile
            {
                XrfPath = dictionary[1],
                MstPath = dictionary[2],
                CntPath = dictionary[3],
                N01Path = dictionary[4],
                N02Path = dictionary[5],
                L01Path = dictionary[6],
                L02Path = dictionary[7],
                IfpPath = dictionary[8],
                AnyPath = dictionary[9],
                PftPath = dictionary[10]
            };
            if (dictionary.ContainsKey(11))
            {
                result.ExtPath = dictionary[11];
            }

            return result;
        }

        /// <summary>
        /// Преобразование в словарь.
        /// </summary>
        public Dictionary<int, string?> ToDictionary()
        {
            var result = new Dictionary<int, string?>(11)
                {
                    {1, XrfPath},
                    {2, MstPath},
                    {3, CntPath},
                    {4, N01Path},
                    {5, N02Path},
                    {6, L01Path},
                    {7, L02Path},
                    {8, IfpPath},
                    {9, AnyPath},
                    {10, PftPath},
                    {11, ExtPath}
                };

            return result;
        }

        /// <summary>
        /// Запись в файл
        /// </summary>
        public void WriteFile
            (
                string fileName
            )
        {
            Sure.NotNullNorEmpty(fileName, nameof(fileName));

            using var writer = TextWriterUtility.Create
                (
                    fileName,
                    IrbisEncoding.Ansi
                );
            WriteText(writer);
        }

        /// <summary>
        /// Запись в поток.
        /// </summary>
        public void WriteText
            (
                TextWriter writer
            )
        {
            writer.WriteLine("1={0}", XrfPath);
            writer.WriteLine("2={0}", MstPath);
            writer.WriteLine("3={0}", CntPath);
            writer.WriteLine("4={0}", N01Path);
            writer.WriteLine("5={0}", N02Path);
            writer.WriteLine("6={0}", L01Path);
            writer.WriteLine("7={0}", L02Path);
            writer.WriteLine("8={0}", IfpPath);
            writer.WriteLine("9={0}", AnyPath);
            writer.WriteLine("10={0}", PftPath);
            writer.WriteLine("11={0}", ExtPath);
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            XrfPath = reader.ReadNullableString();
            MstPath = reader.ReadNullableString();
            CntPath = reader.ReadNullableString();
            N01Path = reader.ReadNullableString();
            N02Path = reader.ReadNullableString();
            L01Path = reader.ReadNullableString();
            L02Path = reader.ReadNullableString();
            IfpPath = reader.ReadNullableString();
            AnyPath = reader.ReadNullableString();
            PftPath = reader.ReadNullableString();
            ExtPath = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(XrfPath)
                .WriteNullable(MstPath)
                .WriteNullable(CntPath)
                .WriteNullable(N01Path)
                .WriteNullable(N02Path)
                .WriteNullable(L01Path)
                .WriteNullable(L02Path)
                .WriteNullable(IfpPath)
                .WriteNullable(AnyPath)
                .WriteNullable(PftPath)
                .WriteNullable(ExtPath);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<ParFile>
                (
                    this,
                    throwOnError
                );

            verifier
                .NotNullNorEmpty(XrfPath, "XrfPath")
                .NotNullNorEmpty(MstPath, "MstPath")
                .NotNullNorEmpty(CntPath, "CntPath")
                .NotNullNorEmpty(N01Path, "N01Path")
                .NotNullNorEmpty(N02Path, "N02Path")
                .NotNullNorEmpty(L01Path, "L01Path")
                .NotNullNorEmpty(L02Path, "L02Path")
                .NotNullNorEmpty(IfpPath, "IfpPath")
                .NotNullNorEmpty(AnyPath, "AnyPath")
                .NotNullNorEmpty(PftPath, "PftPath");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return MstPath.ToVisibleString();
        }

        #endregion
    }
}

