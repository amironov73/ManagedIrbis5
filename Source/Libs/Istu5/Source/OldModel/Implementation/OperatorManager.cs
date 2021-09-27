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
using System.Linq;
using Istu.OldModel.Interfaces;
using LinqToDB;
using LinqToDB.Data;

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
        #region Properties

        /// <summary>
        /// Кладовка.
        /// </summary>
        public Storehouse Storehouse { get; }

        /// <summary>
        /// Таблица <c>operators</c>.
        /// </summary>
        public ITable<Operator> Operators => _GetDb().GetOperators();

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public OperatorManager
            (
                Storehouse storehouse
            )
        {
            Storehouse = storehouse;

        } // constructor

        #endregion

        #region Private members

        private DataConnection? _dataConnection;

        private DataConnection _GetDb() => _dataConnection ??= Storehouse.GetKladovka();

        #endregion

        #region IOperatorManager members

        /// <inheritdoc cref="IOperatorManager.GetOperatorByBarcode"/>
        public Operator? GetOperatorByBarcode (string barcode) =>
            Operators.FirstOrDefault (op => op.Barcode == barcode);

        /// <inheritdoc cref="IOperatorManager.GetOperatorByID"/>
        public Operator? GetOperatorByID(int id) =>
            Operators.FirstOrDefault (op => op.ID == id);

        /// <inheritdoc cref="IOperatorManager.ListAllOperators"/>
        public Operator[] ListAllOperators() => Operators.ToArray();

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            if (_dataConnection is not null)
            {
                _dataConnection.Dispose();
                _dataConnection = null;
            }

        } // method Dispose

        #endregion

    } // class OperatorManager

} // namespace Istu.OldModel.Implementation
