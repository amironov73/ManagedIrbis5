using System.ComponentModel.DataAnnotations;

#nullable enable

namespace BeriChitai.Data;

public sealed class LogonModel
{
    public string Ticket { get; set; } = null!;

    public string Email { get; set; } = null!;
}
