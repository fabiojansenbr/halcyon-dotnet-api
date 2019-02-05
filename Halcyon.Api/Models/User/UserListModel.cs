namespace Halcyon.Api.Models.User
{
    public class UserListModel : PaginatedList<UserSummaryModel>
    {
        public string Search { get; set; }

        public string Sort { get; set; }
    }
}