using System;
using System.Collections.Generic;

namespace Data.Entities;

public partial class RefreshToken
{
    public int RefreshTokenId { get; set; }

    public DateTime ExpiredAt { get; set; }

    public string Token { get; set; } = null!;

    public int? AccountId { get; set; }

    public virtual Account? Account { get; set; }
}
