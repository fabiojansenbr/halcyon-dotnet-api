using Halcyon.Api.Entities;

namespace Halcyon.Api.Services.Handlers
{
    public class HandlerResult
    {
        public HandlerResult(
            User user = null,
            bool isLockedOut = false,
            bool requiresTwoFactor = false,
            bool requiresExternal = false)
        {
            User = user;
            IsLockedOut = isLockedOut;
            RequiresTwoFactor = requiresTwoFactor;
            RequiresExternal = requiresExternal;
        }

        public User User { get; }

        public bool IsLockedOut { get; }

        public bool RequiresTwoFactor { get; }

        public bool RequiresExternal { get; }

    }
}