// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* DbCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Mx.Commands
{
    /// <summary>
    ///
    /// </summary>
    public sealed class DbCommand
        : MxCommand
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public DbCommand()
            : base("db")
        {
        }

        #endregion

        #region MxCommand members

        /// <inheritdoc cref="MxCommand.Execute" />
        public override bool Execute
            (
                MxExecutive executive,
                MxArgument[] arguments
            )
        {
            OnBeforeExecute();

            if (!executive.Provider.Connected)
            {
                executive.WriteLine("Not connected");
                return false;
            }

            var saveDatabase = executive.Provider.Database;
            string? dbName = null;
            if (arguments.Length != 0)
            {
                dbName = arguments[0].Text;
            }

            if (!string.IsNullOrEmpty(dbName))
            {
                executive.Provider.Database = dbName;
            }

            try
            {
                var maxMfn = executive.Provider.GetMaxMfn() - 1;
                executive.WriteMessage($"DB={executive.Provider.Database}, Max MFN={maxMfn}");
            }
            catch
            {
                executive.WriteError($"Error changing DB, restoring to {saveDatabase}");
                executive.Provider.Database = saveDatabase;
            }

            OnAfterExecute();

            return true;
        }

        #endregion

    } // class DbCommand

} // namespace ManagedIrbis.Mx.Commands
