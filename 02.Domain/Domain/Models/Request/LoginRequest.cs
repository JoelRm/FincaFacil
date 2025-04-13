namespace MiddlewareService.Domain.Models.Request{
    public class LoginRequest{
        public string usr { get; set; }
        public string password { get; set; }
        public bool persistent { get; set; }
        public string client { get; set; }
    }
}