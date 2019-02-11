using Grundlov.App.Models;
using Grundlov.App.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.AspNetCore.Blazor.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Grundlov.App.Pages.Account
{
    public class RegisterComponent : BlazorComponent
    {
        [Inject] private SignInManager<ApplicationUser> _signInManager { get; set; }
        [Inject] private UserManager<ApplicationUser> _userManager { get; set; }
        [Inject] private EmailSender _emailSender { get; set; }
        [Inject] private IUriHelper UriHelper { get; set; }

        protected List<string> ErrorMessages = new List<string>();

        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public string ReturnUrl { get; set; }

        protected string Email;
        protected string EmailError;
        protected string Password;
        protected string PasswordError;
        protected string ConfirmPassword;
        protected string ConfirmPasswordError;

        protected override async Task OnInitAsync()
        {
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            var uri = new Uri(UriHelper.GetAbsoluteUri());
            ReturnUrl = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query).TryGetValue("returnurl", out var type) ? type.First() : "";
        }

        protected async Task RegisterAccount()
        {
            ErrorMessages.Clear();

            if (string.IsNullOrWhiteSpace(Email))
                EmailError = "Du skal indtaste en email.";

            if (string.IsNullOrWhiteSpace(Password))
                PasswordError = "Du skal indtaste et kodeord.";

            if (string.IsNullOrWhiteSpace(ConfirmPassword))
                ConfirmPasswordError = "Du skal bekræfte dit kodeord.";

            if (Password != ConfirmPassword)
                ConfirmPasswordError = "Begge kodeord skal være ens.";



            var user = new ApplicationUser { UserName = Email, Email = Email };
            var result = await _userManager.CreateAsync(user, Password);
            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var callbackUrl = UriHelper.ToAbsoluteUri("/account/confirmemail").ToString();

                var encodedCallback = HtmlEncoder.Default.Encode(callbackUrl);
                await _emailSender.SendEmailAsync(Email, "Bekræft din email",
                    $"<p><strong>Velkommen til grundlovsspillet!</strong></p>" +
                    "<p>For at komme igang skal du bekrlfte din email adresse. Det gør du ved at besøge <a href='{encodedCallback}'>{encodedCallback}</a>." +
                    "<br />Hvis din email klient ikke tillader links, kan du kopiere den ind i din browser adresse-bar.</p>" +
                    "<p>Vi glæder os til at se, hvad du finder på!</p>" +
                    "<p>Mange hilsener," +
                    "<br />Grundlovsspilsteamet</p>"
                );

                await _signInManager.SignInAsync(user, isPersistent: false);

                if (!string.IsNullOrWhiteSpace(ReturnUrl))
                    UriHelper.NavigateTo(ReturnUrl);
                else
                    UriHelper.NavigateTo("/");
            }
            else
            {
                ErrorMessages.AddRange(result.Errors.Select(e => e.Description));
                StateHasChanged();
            }
        }
    }
}
