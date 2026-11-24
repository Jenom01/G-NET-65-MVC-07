using GymManagementSystem_BLL.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GymManagementSystem_PL.Extensions
{
    public static class ModelStateExtensions
    {
        public static void AddError(this ModelStateDictionary modelState, Error error)
            => modelState.AddModelError(error.Field ?? string.Empty, error.Message);
    }
}
