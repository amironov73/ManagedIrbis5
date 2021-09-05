// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* OperatorManager.cs -- менеджер операторов книговыдачи
   Ars Magna project, http://library.istu.edu/am */

#region Using directives

using System;

using Istu.OldModel.Interfaces;

#endregion

#nullable enable

namespace Istu.OldModel.Implementation
{
    /// <summary>
    /// Менеджер операторов книговыдачи.
    /// </summary>
    public sealed class OperatorManager
        : IOperatorManager
    {
        #region IOperatorManager members

        /// <inheritdoc cref="IOperatorManager.GetOperatorCount"/>
        public int GetOperatorCount()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IOperatorManager.GetOperatorByBarcode"/>
        public Operator GetOperatorByBarcode(string barcode)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IOperatorManager.GetOperatorByID"/>
        public Operator GetOperatorByID(int id)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IOperatorManager.GetAllOperators"/>
        public Operator[] GetAllOperators()
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

    } // class OperatorManager

} // namespace Istu.OldModel.Implementation
