// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* GblNewMfn.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl.Infrastructure.Ast
{
    //
    // Official documentation:
    // http://sntnarciss.ru/irbis/spravka/pril00704080000.htm
    //
    // Создание новой записи в текущей или другой базе данных.
    //
    // Предполагается, что за этим оператором следуют операторы ADD,
    // которые будут наполнять новую запись.
    // Группа операторов ADD завершается оператором END,
    // после которого корректирующие операторы будут относиться
    // к исходной базе данных и к исходной(текущей) записи.
    // В операторах ADD форматирование по ФОРМАТ 1 происходит
    // в исходной записи исходной базы данных.
    //
    // В файле задания оператор описывается двумя строками,
    // которые содержат следующие данные:
    //
    // 1. Имя оператора – NEWMFN,
    // 2. Формат – результатом форматирования текущей записи
    // должна быть текстовая строка, задающая имя той базы данных,
    // в которой будет создана новая запись.
    // Если строка – ‘*’, то этой базой данных останется текущая.
    //

    /// <summary>
    /// Создает новую запись в текущей или другой базе данных.
    /// </summary>
    public sealed class GblNewMfn
        : GblNode
    {
        #region Constants

        /// <summary>
        /// Command mnemonic.
        /// </summary>
        public const string Mnemonic = "NEWMFN";

        #endregion

        #region Properties

        /// <summary>
        /// Children nodes.
        /// </summary>
        public GblNodeCollection Children { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public GblNewMfn()
        {
            Children = new GblNodeCollection(this);
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        #endregion

        #region GblNode members

        /// <summary>
        /// Execute the node.
        /// </summary>
        public override void Execute
            (
                GblContext context
            )
        {
            Sure.NotNull(context, nameof(context));

            OnBeforeExecution(context);

            // Nothing to do here

            OnAfterExecution(context);
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
}
