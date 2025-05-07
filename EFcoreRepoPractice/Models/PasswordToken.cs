using System;
using System.Collections.Generic;

namespace EFcoreRepoPractice.Models;

public partial class PasswordToken
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Token { get; set; } = null!;

    public DateTime ExpireAt { get; set; }

    public bool IsUsed { get; set; }

    public DateTime CreatedAt { get; set; }
}
