using Halcyon.Api.Services.Handlers;
using Halcyon.Api.Services.Providers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Halcyon.Api.Models.Token
{
    public class GetTokenModel : IValidatableObject, IHandlerRequest
    {
        [Display(Name = "Grant Type")]
        public GrantType GrantType { get; set; }

        [Display(Name = "Email Address")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Provider")]
        public Provider Provider { get; set; }

        [Display(Name = "Access Token")]
        public string AccessToken { get; set; }

        [Display(Name = "Verification Code")]
        public string VerificationCode { get; set; }

        [Display(Name = "Refresh Token")]
        public string RefreshToken { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            switch (GrantType)
            {
                case GrantType.RefreshToken:
                    if (string.IsNullOrEmpty(RefreshToken))
                    {
                        yield return new ValidationResult("The Refresh Token field is required.");
                    }

                    break;

                case GrantType.External:
                    if (string.IsNullOrEmpty(AccessToken))
                    {
                        yield return new ValidationResult("The Access Token field is required.");
                    }

                    break;

                case GrantType.TwoFactor:
                    if (string.IsNullOrEmpty(EmailAddress))
                    {
                        yield return new ValidationResult("The Email Address field is required.");
                    }

                    if (string.IsNullOrEmpty(Password))
                    {
                        yield return new ValidationResult("The Password field is required.");
                    }

                    if (string.IsNullOrEmpty(VerificationCode))
                    {
                        yield return new ValidationResult("The Verification Code field is required.");
                    }

                    break;

                case GrantType.Password:
                    if (string.IsNullOrEmpty(EmailAddress))
                    {
                        yield return new ValidationResult("The Email Address field is required.");
                    }

                    if (string.IsNullOrEmpty(Password))
                    {
                        yield return new ValidationResult("The Password field is required.");
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
