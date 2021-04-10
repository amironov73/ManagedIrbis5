// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* TypeCommand.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Mx.Commands
{
    /// <summary>
    ///
    /// </summary>
    public sealed class TypeCommand
        : MxCommand
    {
        #region Properties

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TypeCommand()
            : base("type")
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

            string? fileName = null;
            if (arguments.Length != 0)
            {
                fileName = arguments[0].Text;
            }

            if (!string.IsNullOrEmpty(fileName))
            {
                var specification = new FileSpecification
                {
                    Database = executive.Provider.Database,
                    Path = IrbisPath.MasterFile,
                    FileName = fileName
                };
                var result = executive.Provider.ReadTextFile(specification)
                    ?? string.Empty;
                executive.WriteLine(result);
            }

            OnAfterExecute();

            return true;
        }

        #endregion

    }
}
