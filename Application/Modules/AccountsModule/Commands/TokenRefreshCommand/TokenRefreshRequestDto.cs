namespace Application.Modules.AccountsModule.Commands.TokenRefreshCommand
{
    public class TokenRefreshRequestDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
