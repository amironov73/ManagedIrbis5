// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* GblAdd.cs -- добавление нового повторения поля или подполя
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl.Infrastructure.Ast;

//
// Official documentation:
// http://sntnarciss.ru/irbis/spravka/pril00704010000.htm
//
// Добавление нового повторения поля или подполя
// в заданное существующее поле.
//
// При этом выполняются следующие правила:
//
// * Если задана МЕТКА ПОЛЯ и не задано подполе, то:
//   * столбец повторения поля блокируется как не имеющий смысла,
//     соответствующая строка в файле задания заполняется
//     символом-заполнителем;
//   * все строки, сформированные ФОРМАТОМ 1,
//     записываются как новые повторения поля.
//
// * Если заданы МЕТКА ПОЛЯ с обозначением подполя,
// то первая строка, которая формируется ФОРМАТОМ 1,
// записывается как подполе в заданное повторение поля.
//
// * Если заданного повторения нет в записи,
// то формируется повое повторение метки с заданным подполем.
//
// * Если ПОВТОРЕНИЕ задано признаком F, то:
// * ФОРМАТ 1 формирует строки добавляемых данных
// * номер строки определяет номер того повторения,
// в которое будет добавлено заданное подполе с данными строки.
//
// * Если повторений поля в записи меньше,
// чем сформатированных строк, то лишние строки
// не используются, если повторений больше,
// чем строк, то лишние повторения не корректируются.
//
// Во всех случаях ФОРМАТ 2 не используется
// и соответствующие строки в файле задания заполняются
// символом-заполнителем.
//
// Оператор не позволяет приписывать данные в конец поля/подполя.
// Для этого можно воспользоваться оператором CHA.
//

/// <summary>
/// Добавление нового повторения поля или подполя
/// в заданное существующее поле.
/// </summary>
public sealed class GblAdd
    : GblNode
{
    #region Constants

    /// <summary>
    /// Мнемоническое обозначение команды.
    /// </summary>
    public const string Mnemonic = "ADD";

    #endregion

    #region GblNode members

    /// <inheritdoc cref="GblNode.Execute"/>
    public override void Execute
        (
            GblContext context
        )
    {
        Sure.NotNull (context);

        OnBeforeExecution (context);

        // TODO implement

        OnAfterExecution (context);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return Mnemonic;
    }

    #endregion
}
