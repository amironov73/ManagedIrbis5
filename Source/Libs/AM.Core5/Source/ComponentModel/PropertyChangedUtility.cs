// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* NotifyPropertyChangedUtility.cs -- вспомогательные методы для INotifyPropertyChanged
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

#endregion

#nullable enable

namespace AM.ComponentModel
{
    /// <summary>
    /// Вспомогательные методы для <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    public static class PropertyChangedUtility
    {
        #region Public methods

        /// <summary>
        /// Borrowed from ReactiveUI
        /// </summary>
        public static TRet? RaiseAndSetIfChanged<TObj, TRet>
            (
                this TObj that,
                ref TRet? backingField,
                TRet? newValue,
                string propertyName
            )
            where TObj : INotifyPropertyChanged
        {
            if (EqualityComparer<TRet>.Default.Equals
                (
                    backingField,
                    newValue
                ))
            {
                return newValue;
            }

            //This.raisePropertyChanging(propertyName);
            backingField = newValue;
            //This.raisePropertyChanged(propertyName);

            return newValue;

        } // method RaiseAndSetIfChanged

        /// <summary>
        /// Notifies the property changed.
        /// </summary>
        public static void NotifyPropertyChanged<T, TProperty>
            (
                this T propertyChangedBase,
                Expression<Func<T, TProperty>> expression,
                object? newValue
            )
            where T : NotifyProperty
        {
            if (expression.Body is MemberExpression memberExpression)
            {
                var propertyName = memberExpression.Member.Name;
                if (newValue != null)
                {
                    typeof(T).GetProperty(propertyName)!
                        .SetValue
                        (
                            propertyChangedBase,
                            newValue,
                            null
                        );
                }

                propertyChangedBase.NotifyPropertyChanged(propertyName);
            }

        } // method NotifyPropertyChanged

        #endregion

    } // class PropertyChangedUtility

} // namespace AM.ComponentModel
