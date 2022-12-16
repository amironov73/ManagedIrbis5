// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/*
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.HtmlTags.Conventions;

public class TagSubject
{
    public TagSubject (string profile, ElementRequest subject)
    {
        Profile = profile ?? TagConstants.Default;
        Subject = subject;
    }

    public string Profile { get; }

    public ElementRequest Subject { get; }

    public bool Equals (TagSubject other)
    {
        if (ReferenceEquals (null, other)) return false;
        if (ReferenceEquals (this, other)) return true;
        return Equals (other.Profile, Profile) && Equals (other.Subject, Subject);
    }

    public override bool Equals (object obj)
    {
        if (ReferenceEquals (null, obj)) return false;
        if (ReferenceEquals (this, obj)) return true;
        if (obj.GetType() != typeof (TagSubject)) return false;
        return Equals ((TagSubject)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return ((Profile?.GetHashCode() ?? 0) * 397) ^
                   (Subject?.GetHashCode() ?? 0);
        }
    }

    public override string ToString() => $"Profile: {Profile}, Subject: {Subject}";
}
