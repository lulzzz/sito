using Maddalena.Mongo;

namespace Maddalena.Identity.Stores
{
    public class TwoFactorRecoveryCode : DBObject<TwoFactorRecoveryCode>
    {
        public string UserId { get; set; }

        public string Code { get; set; }

        public bool Redeemed { get; set; }
    }
}
