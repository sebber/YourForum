using System;
using YourForum.Core.Models;
using AutoMapper;

namespace YourForum.Web.Features.Accounts
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Account, Index.Model>();
            CreateMap<Create.Command, Account>(MemberList.Source)
                .ForSourceMember(c => c.Id, opt => opt.Ignore())
                .ForSourceMember(c => c.Password, opt => opt.Ignore());
        }
    }
}
