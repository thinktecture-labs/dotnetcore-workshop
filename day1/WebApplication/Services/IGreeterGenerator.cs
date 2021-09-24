using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace WebApplication.Services
{
    public interface IGreeterGenerator
    {
        string SayHello();
    }

    public class GreeterGenerator : IGreeterGenerator
    {
        private readonly string _from;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ILogger<GreeterGenerator> _logger;

        public GreeterGenerator(IHttpContextAccessor contextAccessor)
        {
            _from = "Workshop";
            _contextAccessor = contextAccessor;
        }

        public string SayHello()
        {
            var context = _contextAccessor.HttpContext;
            return $"Hello {context.User.Identity.Name} from {_from}";
        }
    }
}