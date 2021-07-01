// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable VirtualMemberCallInConstructor

/* SiberianPropertyColumn.cs -- колонка, привязанная к свойству объекта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Reflection;
using System.Windows.Forms;

using AM.Reflection;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Колонка, привязанная к свойству объекта.
    /// </summary>
    public class SiberianPropertyColumn
        : SiberianColumn
    {
        #region Properties

        /// <summary>
        /// Имя свойства.
        /// </summary>
        public override string? Member
        {
            get => base.Member;
            set
            {
                if (value != base.Member)
                {
                    base.Member = value;
                    _getter = null;
                    _setter = null;
                }
            }
        } // property Member

        #endregion

        #region Private members

        private Func<object, object?>? _getter;
        private Action<object, object?>? _setter;

        #endregion

        #region Public methods

        /// <summary>
        /// Получение значения.
        /// </summary>
        public object? GetValue
            (
                object? obj
            )
        {
            if (obj is null || string.IsNullOrEmpty(Member))
            {
                return null;
            }

            if (_getter is null)
            {
                var type = obj.GetType();
                var propertyInfo = type.GetProperty
                    (
                        Member,
                        BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic
                    );
                if (propertyInfo is null)
                {
                    return null;
                }

                _getter = ReflectionUtility.CreateUntypedGetter(propertyInfo);
            }

            return _getter(obj);

        } // method GetValue

        /// <summary>
        /// Установка значения.
        /// </summary>
        public void SetValue
            (
                object? obj,
                object? value
            )
        {
            if (obj is null || string.IsNullOrEmpty(Member))
            {
                return;
            }

            if (_setter is null)
            {
                var type = obj.GetType();
                var propertyInfo = type.GetProperty
                    (
                        Member,
                        BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic
                    );
                if (propertyInfo is null)
                {
                    return;
                }

                _setter = ReflectionUtility.CreateUntypedSetter(propertyInfo);
            }

            _setter(obj, value);

        } // method SetValue

        #endregion

        #region SiberianColumn members

        /// <inheritdoc cref="SiberianColumn.CreateCell" />
        public override SiberianCell CreateCell() => new SiberianPropertyCell { Column = this };

        /// <inheritdoc cref="SiberianColumn.CreateEditor" />
        public override Control? CreateEditor (SiberianCell cell, bool edit, object? state) => default;

        /// <inheritdoc cref="SiberianColumn.GetData" />
        public override void GetData (object? theObject, SiberianCell cell) =>
            ((SiberianPropertyCell)cell).Value = GetValue(theObject);

        /// <inheritdoc cref="SiberianColumn.PutData" />
        public override void PutData (object? theObject, SiberianCell cell) =>
            SetValue(theObject, ((SiberianPropertyCell)cell).Value);

        #endregion

    } // class SiberianPropertyColumn

} // namespace ManagedIrbis.WinForms.Grid
