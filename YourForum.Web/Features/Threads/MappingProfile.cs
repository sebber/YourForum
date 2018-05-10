using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourForum.Core.Models;

namespace YourForum.Web.Features.Threads
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Post, Index.Response.Thread>()
                .ForMember(t => t.ReplyCount, opt => opt.MapFrom(p => p.Replies.Count()));

            CreateMap<Post, Details.Response.Post>();
            CreateMap<Account, Details.Response.Author>();

            CreateMap<Create.Command, Post>(MemberList.Source)
                .ForSourceMember(c => c.Id, opt => opt.Ignore());

            CreateMap<Reply.Command, Post>(MemberList.Source)
                .ForSourceMember(c => c.Id, opt => opt.Ignore());
        }
    }
}
