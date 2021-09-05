// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* IstuUtility.cs -- вспомогательные методы для работы с базой книговыдачи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using LinqToDB.Mapping;

#endregion

#nullable enable

namespace Istu.OldModel
{
    /// <summary>
    /// Вспомогательные методы для работы с базой книговыдачи.
    /// </summary>
    public static class IstuUtility
    {
        #region Public methods

        /// <summary>
        /// Перечень известных событий книговыдачи.
        /// </summary>
        public static string[] KnownAttendanceEvents() => new[]
        {
            "Возврат",
            "Выдача",
            "Посещение",
            "Продление",
            "Приписка штрих-кода",
            "Регистрация",
            "СМС",
            "Списание",
        };

        /// <summary>
        /// Перевод кода типа посещения в человеко-читаемый формат.
        /// См. поле <c>type</c> в таблице <c>Attendance</c>.
        /// </summary>
        public static string TranslateAttendanceCode (char code) => code switch
            {
                'a' or 'A' => "Посещение",
                'g' or 'G' => "Выдача",
                'r' or 'R' => "Возврат",
                'p' or 'P' => "Продление",
                'w' or 'W' => "Приписка штрих-кода",
                'd' or 'D' => "Списание",
                '1' => "Регистрация",
                's' or 'S' => "СМС",
                _ => code.ToString()

            }; // switch

        #endregion

    } // class IstuUtility

} // namespace Istu.OldModel
