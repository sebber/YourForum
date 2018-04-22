﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourForum.Web.Models;

namespace YourForum.Web.Features.Tenants
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Tenant, Index.Result.Tenant>();
            CreateMap<Create.Command, Tenant>(MemberList.Source).ForSourceMember(c => c.Id, opt => opt.Ignore());
            CreateMap<Tenant, Edit.Command>().ReverseMap();
        }
    }
}
