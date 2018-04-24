using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using YourForum.Core.Models;

namespace YourForum.Core.Infrastructure
{
    public class EntityModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            return typeof(Entity).IsAssignableFrom(context.Metadata.ModelType) ? new EntityModelBinder() : null;
        }
    }
}
