using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Reflection;

namespace Presentation.AppCode.Pipeline
{
    public class HeaderBinderBehaviour<TRequest, TRespose> : IPipelineBehavior<TRequest, TRespose>
        where TRequest : IRequest<TRespose>
    {
        private readonly IHttpContextAccessor ctx;

        public HeaderBinderBehaviour(IHttpContextAccessor ctx)
        {
            this.ctx = ctx;
        }
        public Task<TRespose> Handle(TRequest request, RequestHandlerDelegate<TRespose> next, CancellationToken cancellationToken)
        {

            var properties = request.GetType()
                .GetProperties()
                .Where(m => m.GetCustomAttribute<FromHeaderAttribute>() != null);


            foreach (var property in properties)
            {

                if (!ctx.HttpContext.Request.Headers.TryGetValue(property.Name, out StringValues values))
                    continue;

                property.SetValue(request, values.FirstOrDefault());
            }

            return next();
        }
    }
}
