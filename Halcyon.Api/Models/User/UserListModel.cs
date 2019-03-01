namespace Halcyon.Api.Models.User
{
    public class UserListModel : PaginatedList<UserModel>
    {
        public string Search { get; set; }

        public string Sort { get; set; }
    }
}