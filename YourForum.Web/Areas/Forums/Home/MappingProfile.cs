using System;
using YourForum.Web.Models;
using AutoMapper;

namespace YourForum.Web.Areas.Forums.Home
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Tenant, Index.Forum>();
        }
    }
}
