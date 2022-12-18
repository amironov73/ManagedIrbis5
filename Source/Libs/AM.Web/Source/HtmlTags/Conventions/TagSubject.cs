// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* TagSubject.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

using System;

namespace AM.HtmlTags.Conventions;

/// <summary>
///
/// </summary>
public class TagSubject
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public string Profile { get; }

    /// <summary>
    ///
    /// </summary>
    public ElementRequest Subject { get; }

    #endregion

    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="profile"></param>
    /// <param name="subject"></param>
    public TagSubject
        (
            string? profile,
            ElementRequest subject
        )
    {
        Sure.NotNull (subject);

        Profile = profile ?? TagConstants.Default;
        Subject = subject;
    }

    #endregion

    /// <summary>
    ///
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals (TagSubject other)
    {
        if (ReferenceEquals (null, other))
        {
            return false;
        }

        if (ReferenceEquals (this, other))
        {
            return true;
        }

        return Equals (other.Profile, Profile) && Equals (other.Subject, Subject);
    }

    #region Object members

    /// <inheritdoc cref="object.Equals(object?)"/>
    public override bool Equals
        (
            object? obj
        )
    {
        if (ReferenceEquals (null, obj))
        {
            return false;
        }

        if (ReferenceEquals (this, obj))
        {
            return true;
        }

        if (obj.GetType() != typeof (TagSubject))
        {
            return false;
        }

        return Equals ((TagSubject)obj);
    }

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode()
    {
        return HashCode.Combine (Profile, Subject);
    }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"Profile: {Profile}, Subject: {Subject}";

    #endregion
}
