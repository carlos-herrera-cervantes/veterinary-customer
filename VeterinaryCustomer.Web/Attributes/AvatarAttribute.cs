using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MongoDB.Driver;
using VeterinaryCustomer.Domain.Models;
using VeterinaryCustomer.Repositories.Repositories;

namespace VeterinaryCustomer.Web.Attributes
{
    public class AvatarAttribute : TypeFilterAttribute
    {
        public AvatarAttribute() : base(typeof(AvatarFilter)) {}
    }

    internal class AvatarFilter : IAsyncActionFilter
    {
        #region snippet_Properties

        private readonly IAvatarRepository _avatarRepository;

        #endregion

        #region snippet_Constructors

        public AvatarFilter(IAvatarRepository avatarRepository)
            => _avatarRepository = avatarRepository;

        #endregion

        #region snippet_ActionMethods

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string userId = context.HttpContext.Request.Headers["user-id"];
            var filter = Builders<Avatar>.Filter.Eq(a => a.CustomerId, userId);
            var avatarCounter = await _avatarRepository.CountAsync(filter);

            if (avatarCounter == 0)
            {
                context.Result = new NotFoundResult();
                return;
            }

            await next();
        }

        #endregion
    }
}
