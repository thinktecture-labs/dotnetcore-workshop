using System.Collections.Generic;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace GrpcService
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        private readonly IAuthorizationService _authorizationService;

        public GreeterService(ILogger<GreeterService> logger, IAuthorizationService authorizationService)
        {
            _logger = logger;
            _authorizationService = authorizationService;
        }

        public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {

            var authResult =
                await _authorizationService.AuthorizeAsync(context.GetHttpContext().User, "username_longer_5");
            if (authResult.Succeeded)
            {
                throw new RpcException(new Status(StatusCode.PermissionDenied, "You shall not pass!"));
            }

            /*if (!context.GetHttpContext().User.Identity.IsAuthenticated)
            {
                throw new RpcException(new Status(StatusCode.Unauthenticated), "You must authenticate!");
            }*/

            return new HelloReply
            {
                Message = "Hello " + request.Name
            };
        }
    }

    public interface IGreeterGenerator
    {
        string Generate(string name);
    }

    public class GreeterGenerator : IGreeterGenerator
    {
        public string Generate(string name) => "$Hello {name}";
    }

    public class Helper
    {
        public void Dosomething(ILogger logger)
        {
        }
    }

    public interface IStrategy
    {
        string Type { get; set; }
    }


    public class TestStrategy : IStrategy
    {
        public string Type { get; set; }
    }

}