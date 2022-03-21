using System;

namespace AM.Avalonia.Exceptions;

public class InvalidUrlException
    : Exception
{
    public InvalidUrlException(string message) : base(message)
    {
    }
}
