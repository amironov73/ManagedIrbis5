// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* GblProtocolLine.cs -- строка в протоколе выполнения глобальной корректировки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Processing
{
    //
    // Типичная строка с положительным результатом:
    // DBN=IBIS#MFN=2#AUTOIN=#UPDATE=0#STATUS=8#UPDUF=0#
    //
    // Типичная строка с отрицательным результатом
    // DBN=IBIS#MFN=4#GBL_ERROR=-605
    //

    /// <summary>
    /// Строка в протоколе выполнения глобальной корректировки.
    /// </summary>
    public sealed class ProtocolLine
    {
        #region Properties

        /// <summary>
        /// Общий признак успеха.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Имя базы данных
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// MFN записи
        /// </summary>
        public int Mfn { get; set; }

        /// <summary>
        /// Результат Autoin.gbl
        /// </summary>
        public string? Autoin { get; set; }

        /// <summary>
        /// UPDATE=
        /// </summary>
        public string? Update { get; set; }

        /// <summary>
        /// STATUS=
        /// </summary>
        public string? Status { get; set; }

        /// <summary>
        /// Код ошибки, если есть
        /// </summary>
        public string? Error { get; set; }

        /// <summary>
        /// UPDUF=
        /// </summary>
        public string? UpdUf { get; set; }

        /// <summary>
        /// Исходный текст (до парсинга)
        /// </summary>
        public string? Text { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse one text line.
        /// </summary>
        public void Decode
            (
                string line
            )
        {
            Sure.NotNullNorEmpty (line, nameof (line));

            Text = line;
            Success = true;
            var parts = line.Split('#');
            foreach (var part in parts)
            {
                var p = part.Split('=');
                if (p.Length > 0)
                {
                    var name = p[0].ToUpper();
                    var value = string.Empty;
                    if (p.Length > 1)
                    {
                        value = p[1];
                    }

                    switch (name)
                    {
                        case "DBN":
                            Database = value;
                            break;

                        case "MFN":
                            Mfn = value.SafeToInt32();
                            break;

                        case "AUTOIN":
                            Autoin = value;
                            break;

                        case "UPDATE":
                            Update = value;
                            break;

                        case "STATUS":
                            Status = value;
                            break;

                        case "UPDUF":
                            UpdUf = value;
                            break;

                        case "GBL_ERROR":
                            Error = value;
                            Success = false;
                            break;

                    } // switch

                } // if

            } // foreach

        } // method Decode

        /// <summary>
        /// Parse server response.
        /// </summary>
        public static ProtocolLine[] Decode
            (
                Response response
            )
        {
            var result = new List<ProtocolLine>();

            while (true)
            {
                var line = response.ReadAnsi();
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                var item = new ProtocolLine();
                item.Decode(line);
                result.Add(item);
            } // while

            return result.ToArray();

        } // method Decode

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => Text.ToVisibleString();  // method ToString

        #endregion

    } // class ProtocolLine

} // namespace ManagedIrbis.Processing
