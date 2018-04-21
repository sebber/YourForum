using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using YourForum.Web.Data;

namespace YourForum.Web.Infrastructure
{
    public class EntityModelBinder : IModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var original = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (original != ValueProviderResult.None)
            {
                var originalValue = original.FirstValue;
                if (int.TryParse(originalValue, out var id))
                {
                    var dbContext = bindingContext.HttpContext.RequestServices.GetService<YourForumContext>();
                    var entity = await dbContext.FindAsync(bindingContext.ModelType, id);

                    bindingContext.Result = ModelBindingResult.Success(entity);
                }
            }
        }
    }
}
