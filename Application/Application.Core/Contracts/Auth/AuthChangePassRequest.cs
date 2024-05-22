namespace Application.Core.Contracts
{
    public class AuthChangePassRequest
    {
        public string current_password { get; set; }
        public string new_password { get; set; }
    }
}
