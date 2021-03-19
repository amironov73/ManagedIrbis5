// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* FlcStatus.cs -- статус формально-логической проверки записи
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis.Flc
{
    /// <summary>
    /// Статус формально-логической проверки записи.
    /// Первый символ результата форматирования определяет
    /// результат ФЛК.
    /// </summary>
    public enum FlcStatus
    {
        /// <summary>
        /// Положительный результат контроля.
        /// </summary>
        OK = 0,

        /// <summary>
        /// Отрицательный результат. Обнаруженные ошибки
        /// считаются непреодолимыми, т.е. подлежат
        /// обязательному устранению.
        /// </summary>
        Error = 1,

        /// <summary>
        /// Отрицательный результат, но при этом ошибки
        /// считаются преодолимыми, т.е. их можно не исправлять.
        /// </summary>
        Warning = 2

    } // enum FlcStatus

} // namespace ManagedIrbis.Flc
