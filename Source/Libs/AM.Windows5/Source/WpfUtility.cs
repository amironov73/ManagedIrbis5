// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UseStringInterpolation

/* WpfUtility.cs -- полезные методы для WPF
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

#endregion

#nullable enable

namespace AM.Windows
{
    /// <summary>
    /// Полезные методы для WPF.
    /// </summary>
    public static class WpfUtility
    {
        #region Private members

        private static void _DoNothing()
        {
            // Nothing to do here
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Analog for WinForms Apllication.DoEvents.
        /// </summary>
        /// <remarks>
        /// Borrowed from https://stackoverflow.com/questions/4502037/where-is-the-application-doevents-in-wpf
        /// </remarks>
        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke
                (
                    DispatcherPriority.Background,
                    new Action(_DoNothing)
                );
        } // method DoEvents

        /// <summary>
        /// List of (recursive) children of the given type.
        /// </summary>
        public static List<TChild> ListChildren<TChild>
            (
                DependencyObject element
            )
            where TChild : DependencyObject
        {
            var result = new List<TChild>();
            var count = VisualTreeHelper.GetChildrenCount(element);
            for (var i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);
                if (child is TChild item)
                {
                    result.Add(item);
                }

                var list = ListChildren<TChild>(child);
                result.AddRange(list);
            }

            return result;

        } // method ListChildren

        #endregion

    } // class WpfUtility

} // namespace AM.Windows
