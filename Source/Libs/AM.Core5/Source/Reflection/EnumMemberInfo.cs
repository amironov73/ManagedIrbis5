// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* EnumMemberInfo.cs -- information about Enum member.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

#endregion

#nullable enable

namespace AM.Reflection
{
    /// <summary>
    /// Information about <see cref="T:System.Enum"/> member.
    /// </summary>
    public sealed class EnumMemberInfo
    {
        #region Properties

        /// <summary>
        /// Gets the display name of the enum member.
        /// </summary>
        /// <value>Display name of the enum member.</value>
        public string DisplayName { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public int Value { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="EnumMemberInfo"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="displayName">Name of the display.</param>
        /// <param name="value">The value.</param>
        public EnumMemberInfo
            (
                string name,
                string displayName,
                int value
            )
        {
            Name = name;
            DisplayName = displayName;
            Value = value;
        }

        #endregion

        #region Private members

        private enum SortBy
        {
            Name,
            FriendlyName,
            Value
        }

        private class MemberComparer
            : IComparer
        {
            private SortBy _sortBy;

            public MemberComparer(SortBy sortBy)
            {
                _sortBy = sortBy;
            }

            /// <inheritdoc cref="IComparer.Compare"/>
            public int Compare
                (
                    object? x,
                    object? y
                )
            {
                var first = (EnumMemberInfo?)x;
                var second = (EnumMemberInfo?)y;
                switch (_sortBy)
                {
                    case SortBy.Name:
                        return string.CompareOrdinal(first?.Name, second?.Name);

                    case SortBy.FriendlyName:
                        return string.CompareOrdinal(first?.DisplayName, second?.DisplayName);

                    case SortBy.Value:
                        return (first?.Value ?? default) - (second?.Value ?? default);
                }

                return 0;
            }
        }

        private static void _SortBy
            (
                EnumMemberInfo[] members,
                SortBy sortBy
            )
        {
            MemberComparer comparer = new MemberComparer(sortBy);
            Array.Sort(members, comparer);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Parses the specified enum type.
        /// </summary>
        /// <param name="enumType">Type of the enum.</param>
        public static EnumMemberInfo[] Parse
            (
                Type enumType
            )
        {
            List<EnumMemberInfo> result = new List<EnumMemberInfo>();
            if (!enumType.IsEnum)
            {
                Magna.Error
                    (
                        "EnumMemberInfo::Parse: "
                        + "type="
                        + enumType.FullName
                        + " is not enum"
                    );

                throw new ArgumentException("enumType");
            }

            Type underlyingType = Enum.GetUnderlyingType(enumType);
            switch (underlyingType.Name)
            {
                case "Byte":
                case "SByte":
                case "Int16":
                case "UInt16":
                case "Int32":
                case "UInt32":
                    break;

                default:
                    Magna.Error
                        (
                            "EnumMemberInfo::Parse: "
                            + "unexpected underlying type="
                            + underlyingType.FullName
                        );

                    throw new ArgumentException("enumType");
            }
            foreach (string name in Enum.GetNames(enumType))
            {
                FieldInfo field =
                    enumType.GetField(name
                        /*, BindingFlags.Public | BindingFlags.GetField */ );

                DisplayNameAttribute titleAttribute = ReflectionUtility
                    .GetCustomAttribute<DisplayNameAttribute>(field);

                string displayName = ReferenceEquals(titleAttribute, null)
                    ? name
                    : titleAttribute.DisplayName;

                int value = (int)Enum.Parse(enumType, name, false);
                EnumMemberInfo info = new EnumMemberInfo(name, displayName, value);
                result.Add(info);
            }

            return result.ToArray();
        }

        /// <summary>
        /// Sorts the name of the by display.
        /// </summary>
        /// <param name="members">The members.</param>
        public static void SortByDisplayName(EnumMemberInfo[] members)
        {
            _SortBy(members, SortBy.FriendlyName);
        }

        /// <summary>
        /// Sorts the name of the by.
        /// </summary>
        /// <param name="members">The members.</param>
        public static void SortByName(EnumMemberInfo[] members)
        {
            _SortBy(members, SortBy.Name);
        }

        /// <summary>
        /// Sorts the by value.
        /// </summary>
        /// <param name="members">The members.</param>
        public static void SortByValue(EnumMemberInfo[] members)
        {
            _SortBy(members, SortBy.Value);
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return DisplayName;
        }

        #endregion
    }
}
