using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using YourForum.Web.Models;

namespace YourForum.Web.Infrastructure
{
    public class EntityModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            return typeof(Entity).IsAssignableFrom(context.Metadata.ModelType) ? new EntityModelBinder() : null;
        }
    }
}
