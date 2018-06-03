using AutoMapper;
using MugStore.Data.Models;
using MugStore.Web.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MugStore.Web.ViewModels.Blog
{
    public class PostViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public string Title { get; set; }

        public string PageTitle { get; set; }

        public string PageDescription { get; set; }

        public string Acronym { get; set; }

        public string ShortDescription { get; set; }

        public string FullDescription { get; set; }

        public DateTime CreatedOn { get; set; }

        public virtual IEnumerable<PostTag> Tags { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Data.Models.Post, PostViewModel>()
                .ForMember(x => x.Tags, x => x.MapFrom(y => y.Tags.Select(t => t.PostTag)));
        }
    }
}