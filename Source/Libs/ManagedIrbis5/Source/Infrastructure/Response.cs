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
// ReSharper disable UnusedParameter.Local

/* Response.cs -- ответ сервера ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

using AM;
using AM.Collections;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Ответ сервера ИРБИС64.
    /// </summary>
    public sealed class Response
        : IDisposable
    {
        #region Properties

        /// <summary>
        /// Код команды.
        /// </summary>
        public string? Command { get; private set; }

        /// <summary>
        /// Идентификатор клиента.
        /// </summary>
        public int ClientId { get; private set; }

        /// <summary>
        /// Порядковый номер запроса.
        /// </summary>
        public int QueryId { get; private set; }

        /// <summary>
        /// Код возврвата (не для всех запросов).
        /// </summary>
        public int ReturnCode { get; private set; }

        /// <summary>
        /// Размер запроса в байтах
        /// (не всегда присылвается сервером).
        /// </summary>
        public int AnswerSize { get; private set; }

        /// <summary>
        /// Версия сервера (присылается в ответ на запрос A,
        /// т. е. регистрацию клиента).
        /// </summary>
        public string? ServerVersion { get; private set; }

        /// <summary>
        /// Достигнут конец текста?
        /// </summary>
        public bool EOT { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Response()
        {
            _memory = new List<ArraySegment<byte>>();
        }

        #endregion

        #region Private members

        private readonly List<ArraySegment<byte>> _memory;
        private ArraySegment<byte> _currentChunk;
        private int _currentIndex, _currentOffset;

        /// <summary>
        /// IRBIS_BINARY_DATA
        /// </summary>
        private static readonly byte[] BinaryDataPreamble =
            { 73, 82, 66, 73, 83, 95, 66, 73, 78, 65, 82, 89, 95, 68, 65, 84, 65 };

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление сегмента данных.
        /// </summary>
        public void Add
            (
                ArraySegment<byte> chunk
            )
        {
            _memory.Add(chunk);
        }

        /// <summary>
        /// Проверка кода возврата.
        /// </summary>
        public bool CheckReturnCode() => GetReturnCode() >= 0;

        /// <summary>
        /// Проверка кода возврата.
        /// </summary>
        public bool CheckReturnCode
            (
                params int[] goodCodes
            )
        {
            if (GetReturnCode() < 0)
            {
                if (Array.IndexOf(goodCodes, ReturnCode) < 0)
                {
                    // throw new IrbisException(ReturnCode);
                    return false;
                }
            }

            return true;

        } // method CheckReturnCode

        /// <summary>
        /// Ищем преамбулу сырых бинарных данных.
        /// </summary>
        public bool FindPreamble()
        {
            var preambleLength = BinaryDataPreamble.Length;

            while (!EOT)
            {
                var found = true;
                for (var i = 0; i < preambleLength; i++)
                {
                    var b = ReadByte();
                    if (b != BinaryDataPreamble[i])
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    return true;
                }
            }

            return false;

        } // method FindPreamble

        /// <summary>
        /// Начальный разбор ответа сервера.
        /// </summary>
        public void Parse()
        {
            if (_memory.Count == 0)
            {
                EOT = true;
            }
            else
            {
                EOT = false;
                _currentChunk = _memory.First();
                _currentIndex = 0;
                _currentOffset = 0;

                Command = ReadAnsi();
                ClientId = ReadInteger();
                QueryId = ReadInteger();
                AnswerSize = ReadInteger();
                ServerVersion = ReadAnsi();
                ReadAnsi();
                ReadAnsi();
                ReadAnsi();
                ReadAnsi();
                ReadAnsi();
            }

        } // method Parse

        /// <summary>
        ///
        /// </summary>
        public byte Peek()
        {
            if (EOT)
            {
                return 0;
            }

            if (_currentOffset >= _currentChunk.Count)
            {
                _currentOffset = 0;
                _currentIndex++;
                if (_currentIndex >= _memory.Count)
                {
                    EOT = true;
                    return 0;
                }

                _currentChunk = _memory[_currentIndex];
            }

            return _currentChunk[_currentOffset];

        } // method Peek

        /// <summary>
        ///
        /// </summary>
        public byte ReadByte()
        {
            if (EOT)
            {
                return 0;
            }

            if (_currentOffset >= _currentChunk.Count)
            {
                _currentOffset = 0;
                _currentIndex++;
                if (_currentIndex >= _memory.Count)
                {
                    EOT = true;
                    return 0;
                }

                _currentChunk = _memory[_currentIndex];
            }

            var result = _currentChunk[_currentOffset];
            _currentOffset++;

            if (_currentOffset > _currentChunk.Count)
            {
                _currentOffset = 0;
                _currentIndex++;
                if (_currentIndex >= _memory.Count)
                {
                    EOT = true;
                }
                else
                {
                    _currentChunk = _memory[_currentIndex];
                }
            }

            return result;

        } // method ReadByte

        /// <summary>
        ///
        /// </summary>
        public byte[] ReadLine()
        {
            using var result = new MemoryStream();
            while (true)
            {
                var one = ReadByte();
                if (one == 0)
                {
                    break;
                }

                if (one == 13)
                {
                    if (Peek() == 10)
                    {
                        ReadByte();
                    }

                    break;
                }

                if (one == 10)
                {
                    break;
                }

                result.WriteByte(one);
            }

            return result.ToArray();

        } // method ReadLine

        /// <summary>
        ///
        /// </summary>
        public string ReadLine
            (
                Encoding encoding
            )
        {
            var bytes = ReadLine();
            if (bytes.Length == 0)
            {
                return string.Empty;
            }

            return encoding.GetString(bytes);

        } // method ReadLine

        /// <summary>
        ///
        /// </summary>
        public byte[] RemainingBytes()
        {
            if (EOT)
            {
                return Array.Empty<byte>();
            }

            var length = _currentChunk.Count - _currentOffset;

            for (var i = _currentIndex + 1; i < _memory.Count; i++)
            {
                length += _memory[i].Count;
            }

            if (length == 0)
            {
                EOT = true;

                return Array.Empty<byte>();
            }

            var result = new byte[length];
            var offset = 0;
            _currentChunk.Slice(_currentOffset).CopyTo(result);
            offset += _currentChunk.Count - _currentOffset;
            for (var i = _currentIndex + 1; i < _memory.Count; i++)
            {
                var chunk = _memory[i];
                chunk.CopyTo(result, offset);
                offset += chunk.Count;
            }

            return result;

        } // method RemainingBytes

        /// <summary>
        ///
        /// </summary>
        public string RemainingText
            (
                Encoding encoding
            )
        {
            var bytes = RemainingBytes();
            if (bytes.Length == 0)
            {
                return string.Empty;
            }

            return encoding.GetString(bytes);

        } // method RemainingText

        /// <summary>
        /// Debug dump.
        /// </summary>
        public void Debug
            (
                TextWriter? writer = null
            )
        {
            writer ??= Console.Out;

            foreach (var memory in _memory)
            {
                foreach (var b in memory)
                {
                    writer.Write($" {b:X2}");
                }
            }

        } // method Debug

        /// <summary>
        /// Debug dump.
        /// </summary>
        public void DebugUtf
            (
                TextWriter? writer = null
            )
        {
            writer ??= Console.Out;

            foreach (var memory in _memory)
            {
                writer.Write(IrbisEncoding.Utf8.GetString(memory));
            }

        } // method DebugUtf

        /// <summary>
        /// Debug dump.
        /// </summary>
        public void DebugAnsi
            (
                TextWriter? writer = null
            )
        {
            writer ??= Console.Out;

            foreach (var memory in _memory)
            {
                writer.Write(IrbisEncoding.Ansi.GetString(memory));
            }

        } // method DebugAnsi

        /// <summary>
        ///
        /// </summary>
        public int GetReturnCode()
        {
            ReturnCode = ReadInteger();

            return ReturnCode;

        } // method GetReturnCode

        /// <summary>
        ///
        /// </summary>
        public string ReadAnsi() => ReadLine(IrbisEncoding.Ansi);

        /// <summary>
        ///
        /// </summary>
        public int ReadInteger() => ReadLine(IrbisEncoding.Ansi).SafeToInt32();

        /// <summary>
        ///
        /// </summary>
        public string[]? ReadAnsiStrings
            (
                int count
            )
        {
            Sure.Positive(count, nameof(count));

            var result = new List<string>(count);
            for (var i = 0; i < count; i++)
            {
                var line = ReadAnsi();
                if (string.IsNullOrEmpty(line))
                {
                    return null;
                }

                result.Add(line);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Get array of ANSI strings.
        /// </summary>
        /// <returns><c>null</c>if there is no lines in
        /// the server response, otherwise missing lines will
        /// be added (as empty lines).</returns>
        public string[]? ReadAnsiStringsPlus
            (
                int count
            )
        {
            Sure.Positive(count, nameof(count));

            var result = new List<string>(count);
            int index = 0;
            string line;
            for (; index < 1; index++)
            {
                line = ReadAnsi();
                if (string.IsNullOrEmpty(line))
                {
                    return null;
                }
                result.Add(line);
            }
            for (; index < count; index++)
            {
                line = ReadAnsi();
                result.Add(line);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Require ANSI-encoded line.
        /// </summary>
        public string RequireAnsi()
        {
            var result = ReadAnsi();
            if (string.IsNullOrEmpty(result))
            {
                throw new IrbisException();
            }

            return result;
        }

        /// <summary>
        /// Require UTF8-encoded line.
        /// </summary>
        public string RequireUtf()
        {
            var result = ReadUtf();
            if (string.IsNullOrEmpty(result))
            {
                throw new IrbisException();
            }

            return result;
        }

        /// <summary>
        /// Require integer value.
        /// </summary>
        public int RequireInteger()
        {
            var line = ReadAnsi();
            var result = int.Parse(line);
            return result;
        }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<string> EnumRemainingAnsiLines()
        {
            while (!EOT)
            {
                string line;
                try
                {
                    line = ReadAnsi();
                }
                catch (Exception exception)
                {
                    System.Diagnostics.Debug.WriteLine(exception.Message);
                    continue;
                }

                yield return line;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<string> EnumRemainingNonNullAnsiLines()
        {
            while (!EOT)
            {
                string line;
                try
                {
                    line = ReadAnsi();
                }
                catch (Exception exception)
                {
                    System.Diagnostics.Debug.WriteLine(exception.Message);
                    continue;
                }

                yield return line;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<string> EnumRemainingUtfLines()
        {
            while (!EOT)
            {
                string line;
                try
                {
                    line = ReadUtf();
                }
                catch (Exception exception)
                {
                    System.Diagnostics.Debug.WriteLine(exception.Message);
                    continue;
                }

                yield return line;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<string> EnumRemainingNonNullUtfLines()
        {
            while (!EOT)
            {
                string line;
                try
                {
                    line = ReadUtf();
                }
                catch (Exception exception)
                {
                    System.Diagnostics.Debug.WriteLine(exception.Message);
                    continue;
                }

                yield return line;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IEnumerable<byte[]> EnumRemainingBinaryLines()
        {
            while (!EOT)
            {
                byte[] line;
                try
                {
                    line = ReadLine();
                }
                catch (Exception exception)
                {
                    System.Diagnostics.Debug.WriteLine(exception.Message);
                    continue;
                }

                yield return line;
            }
        }

        /// <summary>
        /// Чтение нескольких строк в кодировке ANSI.
        /// </summary>
        public string[]? GetAnsiStrings
            (
                int lineCount
            )
        {
            var result = new LocalList<string>();

            for (var i = 0; i < lineCount; i++)
            {
                if (EOT)
                {
                    return null;
                }

                var line = ReadAnsi();
                result.Add(line);
            }

            return result.ToArray();

        } // method GetAnsiStrings

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public string[] ReadRemainingAnsiLines()
        {
            var result = new LocalList<string>();

            while (!EOT)
            {
                var line = ReadAnsi();
                result.Add(line);
            }

            return result.ToArray();

        } // method ReadRemainingAnsiLines

        /// <summary>
        ///
        /// </summary>
        public string ReadRemainingAnsiText() => RemainingText(IrbisEncoding.Ansi);

        /// <summary>
        ///
        /// </summary>
        public string[] ReadRemainingUtfLines()
        {
            LocalList<string> result = new LocalList<string>();

            while (!EOT)
            {
                string line = ReadLine(IrbisEncoding.Utf8);
                result.Add(line);
            }

            return result.ToArray();
        }

        /// <summary>
        ///
        /// </summary>
        public string ReadRemainingUtfText() => RemainingText(IrbisEncoding.Utf8);

        /// <summary>
        /// Чтение строки в кодировке UTF-8.
        /// </summary>
        public string ReadUtf() => ReadLine(IrbisEncoding.Utf8);

        /// <summary>
        /// Чтение целого числа.
        /// </summary>
        public int RequireInt32()
        {
            if (EOT)
            {
                throw new IrbisException("Unexpected end of response");
            }

            var line = ReadLine(IrbisEncoding.Ansi);

            return int.Parse(line, CultureInfo.InvariantCulture);

        } // method RequireInt32

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            // TODO: проверять на повторный вызов Dispose
            // Nothing yet
        }

        #endregion

    } // class Response

} // namespace ManagedIrbis.Infrastructure
