// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global

/* GblStatement.cs -- оператор глобальной корректировки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Text;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Оператор глобальной корректировки со всеми относящимися
    /// к нему данными.
    /// </summary>
    [DebuggerDisplay ("{Command} {Parameter1} {Parameter2}")]
    public sealed class GblStatement
    {
        #region Constants

        /// <summary>
        /// Разделитель элементов
        /// </summary>
        public const string Delimiter = "\x1F\x1E";

        #endregion

        #region Properties

        /// <summary>
        /// Команда (оператор), например, ADD или DEL.
        /// </summary>
        public string? Command { get; set; }

        /// <summary>
        /// Первый параметр, как правило, спецификация поля/подполя.
        /// </summary>
        public string? Parameter1 { get; set; }

        /// <summary>
        /// Второй параметр, как правило, спецификация повторения.
        /// </summary>
        public string? Parameter2 { get; set; }

        /// <summary>
        /// Первый формат, например, выражение для замены.
        /// </summary>
        public string? Format1 { get; set; }

        /// <summary>
        /// Второй формат, например, заменяющее выражение.
        /// </summary>
        public string? Format2 { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Encode for protocol.
        /// </summary>
        public string EncodeForProtocol()
        {
            var result = new StringBuilder();

            result.Append (Command);
            result.Append (Delimiter);
            result.Append (Parameter1);
            result.Append (Delimiter);
            result.Append (Parameter2);
            result.Append (Delimiter);
            result.Append (Format1);
            result.Append (Delimiter);
            result.Append (Format2);
            result.Append (Delimiter);

            return result.ToString();
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
            if (ReferenceEquals (command, null) || command.Length == 0)
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

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "Command: {0},{5}"
                    + "Parameter1: {1},{5}"
                    + "Parameter2: {2},{5}"
                    + "Format1: {3},{5}"
                    + "Format2: {4}",
                    Command,
                    Parameter1,
                    Parameter2,
                    Format1,
                    Format2,
                    Environment.NewLine
                );
        }

        #endregion
    }
}
