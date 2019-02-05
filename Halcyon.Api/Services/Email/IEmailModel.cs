namespace Halcyon.Api.Services.Email
{
    public interface IEmailModel
    {
        string ToAddress { get; }

        string Template { get; }
    }
}