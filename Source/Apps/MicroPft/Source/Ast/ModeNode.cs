// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ModeNode.cs -- переключение режима вывода поля/подполя
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

#endregion

#nullable enable

namespace MicroPft.Ast;

/*

   Система может выводить данные в трех различных режимах:

   * Режим проверки - в этом режиме поля выводятся в том виде,
     в каком они хранятся в записи. При этом система не обеспечивает
     никаких разделителей между полями или экземплярами повторяющихся полей.
     Пользователь должен обеспечить адекватное разделение полей с помощью
     команд размещения, литералов или повторяющихся групп. Режим обычно
     используется для вывода записей с целью проверки правильности введенных
     данных;

   * Режим заголовка - этот режим обычно используется для печати заголовков
     при выводе указателей и таблиц. Все управляющие символы, введенные вместе
      с данными, такие как разделители терминов (< и >) игнорируются (за
      исключением указанных ниже случаев), а разделители подполей заменяются
      знаками пунктуации;

   * Режим данных - этот режим похож на режим заголовка, но дополнительно
     после каждого поля автоматически ставится точка (.), за которой следуют
     два пробела (или просто два пробела, если поле заканчивается каким-либо
     знаком пунктуации). Отметим, однако, что эта автоматическая пунктуация
     подавляется, если за командой вывода поля следует суффикс-литерал.

 */

/// <summary>
/// Переключение режима вывода поля/подполя.
/// </summary>
internal sealed class ModeNode
    : PftNode
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ModeNode
        (
            char mode,
            bool upper
        )
    {
        _mode = mode;
        _upper = upper;
    }

    #endregion

    #region Private members

    private readonly char _mode;
    private readonly bool _upper;

    #endregion

    #region PftNode members

    /// <inheritdoc cref="PftNode.Execute"/>
    public override void Execute
        (
            PftContext context
        )
    {
        if (context.CurrentRepeat == 0)
        {
            context.Mode = _mode;
            context.Upper = _upper;
        }
    }

    #endregion

    #region MereSerializer members

    /// <inheritdoc cref="PftNode.MereSerialize"/>
    public override void MereSerialize
        (
            BinaryWriter writer
        )
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc cref="PftNode.MereDeserialize"/>
    public override void MereDeserialize
        (
            BinaryReader reader
        )
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"mode_{_mode}_{_upper}";
    }

    #endregion
}
