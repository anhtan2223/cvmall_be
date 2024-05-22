namespace Application.Core.Contracts
{
    public class AuthLoginResponse
    {
        public string? access_token { get; set; }
        public string? refresh_token { get; set; }
    }
}
