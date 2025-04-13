namespace MiddlewareService.Domain.Models.Response{
    public class ResponseBase <T>
    {
         public T? Data { get; set; }
         public string? Code { get; set; }
         public string? Message { get; set; }
    }
}