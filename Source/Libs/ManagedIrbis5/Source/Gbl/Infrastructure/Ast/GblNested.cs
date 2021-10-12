// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* GblNested.cs -- вложенное пакетное задание
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl.Infrastructure.Ast
{
    //
    // Начиная с 2016.1
    //
    // Введена новая конструкция для пакетных заданий
    // (только для АРМа Каталогизатор), которая позволяет использовать
    // ВЛОЖЕННЫЕ пакетные задания с помощью ФОРМАТА.
    // Новая конструкция имеет вид команды:
    // @<имя формата>
    // В результате вместо данной конструкции в пакетное задание
    // будут вставляться строки, которые формируются в результате
    // форматирования ТЕКУЩЕЙ записи по соответствующему формату.
    //

    /// <summary>
    /// Вложенное пакетное задание
    /// </summary>
    public sealed class GblNested
        : GblNode
    {
        #region Constants

        /// <summary>
        /// Command mnemonic.
        /// </summary>
        public const string Mnemonic = "@";

        #endregion

        #region Properties

        #endregion

        #region Construction

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
            Sure.NotNull (context, nameof (context));

            OnBeforeExecution(context);

            // Nothing to do here

            OnAfterExecution (context);

        } // method Execute

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => Mnemonic;

        #endregion
    }
}
