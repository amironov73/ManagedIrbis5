// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* ReaderManager.cs -- менеджер базы читателей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Istu.OldModel.Interfaces;

#endregion

#nullable enable

namespace Istu.OldModel.Implementation
{
    /// <summary>
    /// Менеджер базы читателей для книговыдачи.
    /// </summary>
    public sealed class ReaderManager
        : IReaderManager
    {
        #region IReaderManager members

        /// <inheritdoc cref="IReaderManager.CreateReader"/>
        public int CreateReader(Reader info)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IReaderManager.GetReaderByTicket"/>
        public Reader GetReaderByTicket(string ticket)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IReaderManager.GetReaderByTicketAndPassword"/>
        public Reader GetReaderByTicketAndPassword(string ticket, string password)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IReaderManager.GetReaderByBarcode"/>
        public Reader GetReaderByBarcode(string barcode)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IReaderManager.GetReaderByIstuID"/>
        public Reader GetReaderByIstuID(int id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IReaderManager.GetReaderByRfid"/>
        public Reader GetReaderByRfid(string rfid)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IReaderManager.UpdateReaderInfo"/>
        public void UpdateReaderInfo(Reader info)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IReaderManager.Reregister"/>
        public void Reregister(string ticket)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IReaderManager.DeleteReader"/>
        public void DeleteReader(string ticket)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IReaderManager.CheckExistence"/>
        public bool CheckExistence(string ticket)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IReaderManager.ValidateTicketString"/>
        public bool ValidateTicketString(string ticket)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IReaderManager.ValidateNameString"/>
        public bool ValidateNameString(string ticket)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IReaderManager.VerifyPassword"/>
        public bool VerifyPassword(string ticket, string password)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IReaderManager.FindReaders"/>
        public Reader[] FindReaders(ReaderSearchCriteria criteria, string mask, int max)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IReaderManager.GetPhoto"/>
        public object GetPhoto(string ticket)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IReaderManager.SetPhoto"/>
        public void SetPhoto(string ticket, object photo)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IReaderManager.ExportPhoto"/>
        public void ExportPhoto(string path)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IReaderManager.GetDopplers"/>
        public string[] GetDopplers()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion

    } // class ReaderManager

} // namespace Istu.OldModel.Implementation
