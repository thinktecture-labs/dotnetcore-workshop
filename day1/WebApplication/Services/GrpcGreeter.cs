using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using WebApplication.EfModel;

namespace WebApplication.Services
{
    public class GrpcGreeter: global::Greeter.GreeterBase
    {
        private readonly IGreeterGenerator _greeterGenerator;
        private readonly IAuthorizationService _authorizationService;

        public GrpcGreeter(IGreeterGenerator greeterGenerator, IAuthorizationService authorizationService, MyContext context)
        {
            _greeterGenerator = greeterGenerator;
            _authorizationService = authorizationService;
        }
        
        [Authorize()]
        public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            var authResult =
                await _authorizationService.AuthorizeAsync(context.GetHttpContext().User, "username_longer_5");
            
            var result = new HelloReply()
            {
                Message = _greeterGenerator.SayHello()
            };
            return result;
        }
    }
}