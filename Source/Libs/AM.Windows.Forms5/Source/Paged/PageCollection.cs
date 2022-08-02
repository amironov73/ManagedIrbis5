// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global
// ReSharper disable VirtualMemberCallInConstructor

/* PageCollection.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

#endregion

#nullable enable

namespace Manina.Windows.Forms;

public partial class PagedControl
{
    /// <summary>
    ///
    /// </summary>
    [DefaultProperty ("Item")]
    public class PageCollection
        : IList<Page>
    {
        #region Member Variables

        private readonly PagedControl _owner;
        private readonly PagedControlControlCollection _controls;

        #endregion

        #region Properties

        /// <summary>
        ///
        /// </summary>
        public Page this [int index]
        {
            get => (Page)_controls[index + _owner.FirstPageIndex];
            set
            {
                _controls.FromPageCollection = true;
                _controls.RemoveAt (index + _owner.FirstPageIndex);
                _controls.Add (value);
                _controls.SetChildIndex (value, index + _owner.FirstPageIndex);
                _controls.FromPageCollection = false;

                if (_owner.SelectedIndex == index)
                {
                    _owner.ChangePage (value, false);
                }

                _owner.UpdatePages();
                _owner.OnUpdateUIControls (EventArgs.Empty);
            }
        }

        /// <summary>
        ///
        /// </summary>
        public int Count => _owner.PageCount;

        /// <summary>
        ///
        /// </summary>
        public bool IsReadOnly => false;

        #endregion

        #region Constructor

        /// <summary>
        ///
        /// </summary>
        public PageCollection (PagedControl control)
        {
            _owner = control;
            _controls = (PagedControlControlCollection) control.Controls;
        }

        #endregion

        #region Public Methods

        /// <summary>
        ///
        /// </summary>
        public void Add (Page item)
        {
            _controls.FromPageCollection = true;
            _controls.Add (item);
            _controls.FromPageCollection = false;

            _owner.OnPageAdded (new PageEventArgs (item));

            if (Count == 1)
            {
                // Just added a page to an empty control
                // Set this page as the selected page; this event cannot be cancelled
                _owner.OnPageChanging (new PageChangingEventArgs (null, item));
                _owner.selectedPage = item;
                _owner.selectedIndex = 0;
                _owner.OnPageShown (new PageEventArgs (item));
                _owner.OnPageChanged (new PageChangedEventArgs (null, item));
            }

            _owner.UpdatePages();
            _owner.OnUpdateUIControls (EventArgs.Empty);
        }

        /// <summary>
        ///
        /// </summary>
        public void Clear()
        {
            if (Count == 0)
            {
                return;
            }

            var toRemove = new List<Page>();
            for (var i = 0; i < Count; i++)
            {
                toRemove.Add (this[i]);
            }

            var lastSelectedPage = _owner.selectedPage;

            // Set the selected page to null; this event cannot be cancelled
            _owner.OnPageChanging (new PageChangingEventArgs (lastSelectedPage, null));
            _owner.selectedPage = null;
            _owner.selectedIndex = -1;
            _owner.OnPageChanged (new PageChangedEventArgs (lastSelectedPage, null));

            _controls.FromPageCollection = true;
            foreach (var page in toRemove)
            {
                _controls.Remove (page);
                if (page.Visible)
                {
                    _owner.OnPageHidden (new PageEventArgs (page));
                }

                _owner.OnPageRemoved (new PageEventArgs (page));
            }

            _controls.FromPageCollection = false;

            _owner.UpdatePages();
            _owner.OnUpdateUIControls (EventArgs.Empty);
        }

        /// <summary>
        ///
        /// </summary>
        public bool Contains (Page item)
        {
            return _controls.Contains (item);
        }

        /// <summary>
        ///
        /// </summary>
        public void CopyTo (Page[] array, int arrayIndex)
        {
            for (var i = arrayIndex; i < array.Length; i++)
            {
                array[i] = (Page)_controls[i - arrayIndex + _owner.FirstPageIndex];
            }
        }

        /// <summary>
        ///
        /// </summary>
        public IEnumerator<Page> GetEnumerator()
        {
            for (var i = _owner.FirstPageIndex; i < _owner.FirstPageIndex + _owner.PageCount; i++)
            {
                yield return (Page)_controls[i];
            }
        }

        /// <summary>
        ///
        /// </summary>
        public int IndexOf (Page item)
        {
            return _controls.IndexOf (item) - _owner.FirstPageIndex;
        }

        /// <summary>
        ///
        /// </summary>
        public void Insert (int index, Page item)
        {
            if (_owner.PageCount == 0)
            {
                Add (item);
                return;
            }

            var insertBeforeSelected = index <= _owner.SelectedIndex;

            _controls.FromPageCollection = true;
            _controls.Add (item);
            _controls.SetChildIndex (item, index + _owner.FirstPageIndex);
            _controls.FromPageCollection = false;

            if (insertBeforeSelected)
            {
                _owner.selectedIndex += 1;
            }

            _owner.UpdatePages();
            _owner.OnUpdateUIControls (EventArgs.Empty);

            _owner.OnPageAdded (new PageEventArgs (item));
        }

        /// <summary>
        ///
        /// </summary>
        public bool Remove (Page item)
        {
            var index = _owner.Pages.IndexOf (item);
            var exists = index != -1;
            if (!exists)
            {
                throw new ArgumentException ("Page not found in collection.");
            }

            _controls.FromPageCollection = true;
            _controls.Remove (item);
            _controls.FromPageCollection = false;

            if (Count == 0)
            {
                // Just removed the last page from the collection
                // Set the selected page to null; this event cannot be cancelled
                _owner.OnPageChanging (new PageChangingEventArgs (item, null));
                _owner.selectedPage = null;
                _owner.selectedIndex = -1;
                _owner.OnPageChanged (new PageChangedEventArgs (item, null));
            }
            else if (ReferenceEquals (_owner.selectedPage, item))
            {
                // Just removed the selected page from the collection
                // Set the selected page to the page before it; this event cannot be cancelled
                var newSelectedIndex = _owner.selectedIndex == Count ? Count - 1 : _owner.selectedIndex;
                var newSelectedPage = this[newSelectedIndex];

                _owner.OnPageChanging (new PageChangingEventArgs (item, newSelectedPage));
                _owner.selectedPage = newSelectedPage;
                _owner.selectedIndex = newSelectedIndex;
                _owner.OnPageChanged (new PageChangedEventArgs (item, newSelectedPage));
            }

            _owner.OnPageHidden (new PageEventArgs (item));
            _owner.OnPageRemoved (new PageEventArgs (item));

            _owner.UpdatePages();
            _owner.OnUpdateUIControls (EventArgs.Empty);

            return exists;
        }

        /// <summary>
        ///
        /// </summary>
        public void RemoveAt (int index)
        {
            Remove (this[index]);
        }

        /// <summary>
        ///
        /// </summary>
        public void CopyTo (Array array, int index)
        {
            for (var i = index; i < array.Length; i++)
            {
                array.SetValue (this[i - index], i);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
