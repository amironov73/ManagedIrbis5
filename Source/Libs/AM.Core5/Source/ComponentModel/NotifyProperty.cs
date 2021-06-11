// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* NotifyProperty.cs -- наивная реализация INotifyPropertyChanged
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;

#endregion

#nullable enable

namespace AM.ComponentModel
{
    /// <summary>
    /// Наивная реализация <see cref="INotifyPropertyChanged"/>.
    /// </summary>
    public class NotifyProperty
        : INotifyPropertyChanged
    {
        /// <summary>
        /// Событие возбуждается при изменении значения свойства.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Извещает подписчиков свойства об изменении значения свойства.
        /// </summary>
        public virtual void NotifyPropertyChanged (string str)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(str));

    } // class NotifyProperty

} // namespace AM.ComponentModel
