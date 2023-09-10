// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

/* TextParameters.cs -- параметры полнотекстового поиска.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.Text;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Параметры полнотекстового поиска для ИРБИС64+.
    /// </summary>
    [DebuggerDisplay ("{" + nameof (Request) + "}")]
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
            builder.Append (Request);
            builder.Append (Delimiter);
            builder.Append
                (
                    Words is not null
                        ? string.Join (" ", Words)
                        : string.Empty
                );
            builder.Append (Delimiter);
            builder.Append (Morphology ? "1" : "0");
            builder.Append (Delimiter);
            builder.Append (Prefix);
            builder.Append (Delimiter);
            builder.Append (MaxDistanse.ToInvariantString());
            builder.Append (Delimiter);
            builder.Append (Context);
            builder.Append (Delimiter);
            builder.Append (MaxCount.ToInvariantString());
            builder.Append (Delimiter);
            builder.Append (CellType);
            builder.Append (Delimiter);
            builder.Append (CellCount.ToInvariantString());

            return builder.ToString();
        }

        /// <summary>
        /// Кодирование в пользовательский запрос.
        /// </summary>
        /// <param name="connection">Подключение.</param>
        /// <param name="query">Пользовательский запрос.</param>
        public void Encode
            (
                SyncConnection connection,
                SyncQuery query
            )
        {
            query.AddUtf (Encode());
        }

        #endregion
    }
}
