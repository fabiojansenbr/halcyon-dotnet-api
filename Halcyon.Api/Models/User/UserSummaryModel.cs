namespace Halcyon.Api.Models.User
{
    public class UserSummaryModel
    {
        public string Id { get; set; }

        public string EmailAddress { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsLockedOut { get; set; }

        public bool HasPassword { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public string Picture { get; set; }
    }
}