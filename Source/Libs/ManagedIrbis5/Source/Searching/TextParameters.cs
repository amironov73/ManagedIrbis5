// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* TextParameters.cs -- параметры полнотекстового поиска для ИРБИС64+
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.Text;

using AM;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /*
        Из официальной документации:

        search_exp Full_text – Строка, содержащая седующий набор параметров, разделенных #31:

        Request - поисковый запрос на естественном языке
        Request_In - искать эти слова в найденном по request
        Morphology - учитывать морфологию слова (1) или (0) производить усечение? Умолчание 0.
        Prefix -  (умолчание KT=)
        MaxWordsDistanse  - учесть максимальное расстояние между словами. Если
        MaxWordsDistanse =-1 или пустая строка - то не учитывать. Умолчание -1.
        Context- ничего, или один или несколько тематических индексов запятую для поиска похожих.
        MaxCountResult - максимальное число ответов (умолчание 100)
        CellType – тип фасета (префикс), по умолчанию  пустота  (параметр может отсутствовать)
        CellCount – глубина выдачи (по умолчанию 5 верхних фасетов)

     */

    /// <summary>
    /// Параметры полнотекстового поиска для ИРБИС64+.
    /// </summary>
    [DebuggerDisplay("{" + nameof(Request) + "}")]
    public sealed class TextParameters
    {
        #region Constants

        /// <summary>
        /// Разделитель между элементами запроса.
        /// </summary>
        public const char Delimiter = '\x1F';

        #endregion

        #region Properties

        /// <summary>
        /// Поисковый запрос на естественном языке.
        /// </summary>
        public string? Request { get; set; }

        /// <summary>
        /// Искать эти слова в найденном по Request.
        /// </summary>
        public string[]? Words { get; set; }

        /// <summary>
        /// Использовать морфологию? По умолчанию используется стемминг
        /// и поиск по совпадению с стеммированным словом.
        /// </summary>
        public bool Morphology { get; set; }

        /// <summary>
        /// Префикс в словаре. По умолчанию "KT=".
        /// </summary>
        public string Prefix { get; set; } = "KT=";

        /// <summary>
        /// Учитывать максимальное расстояние между словами.
        /// По умолчанию = -1, что означает "не учитывать".
        /// </summary>
        public int MaxDistanse { get; set; } = -1;

        /// <summary>
        /// Тематические индексы через запятую.
        /// </summary>
        public string? Context { get; set; }

        /// <summary>
        /// Максимальное число ответов.
        /// По умолчанию 100.
        /// </summary>
        public int MaxCount { get; set; } = 100;

        /// <summary>
        /// Тип фасета (префикс).
        /// По умолчанию отсутсвует.
        /// </summary>
        public string? CellType { get; set; }

        /// <summary>
        /// Глубина выдачи.
        /// По умолчанию 5 верхних фасетов.
        /// </summary>
        public int CellCount { get; set; } = 5;

        #endregion

        #region Public methods

        /// <summary>
        /// Кодирование параметров в строку.
        /// </summary>
        public string Encode()
        {
            var builder = new StringBuilder();
            builder.Append(Request);
            builder.Append(Delimiter);
            if (Words is not null)
            {
                builder.Append(string.Join(' ', Words));
            }
            else
            {
                builder.Append(string.Empty);
            }
            builder.Append(Delimiter);
            builder.Append(Morphology ? "1" : "0");
            builder.Append(Delimiter);
            builder.Append(Prefix);
            builder.Append(Delimiter);
            builder.Append(MaxDistanse.ToInvariantString());
            builder.Append(Delimiter);
            builder.Append(Context);
            builder.Append(Delimiter);
            builder.Append(MaxCount.ToInvariantString());
            builder.Append(Delimiter);
            builder.Append(CellType);
            builder.Append(Delimiter);
            builder.Append(CellCount.ToInvariantString());

            return builder.ToString();
        } // method Encode

        /// <summary>
        /// Кодирование в пользовательский запрос.
        /// </summary>
        /// <param name="connection">Подключение.</param>
        /// <param name="query">Пользовательский запрос.</param>
        public void Encode<TQuery>
            (
                IIrbisConnectionSettings connection,
                TQuery query
            )
            where TQuery: IQuery
        {
            query.AddUtf(Encode());
        } // method Encode

        #endregion

    } // class TextParameters

} // namespace ManagedIrbis
