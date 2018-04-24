using System;
using YourForum.Core.Models;
using AutoMapper;

namespace YourForum.Web.Features.Home
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Tenant, Index.Forum>();
        }
    }
}
