using AutoMapper;
using MugStore.Common;
using MugStore.Data.Models;
using MugStore.Web.Infrastructure.Mapping;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MugStore.Web.Areas.Admin.ViewModels.Blog
{
    public class CreateViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.MaxBlogPostTitleLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(GlobalConstants.MaxBlogPostTitleLength)]
        public string PageTitle { get; set; }

        [Required]
        [MaxLength(GlobalConstants.MaxBlogPostTitleLength)]
        public string PageDescription { get; set; }

        [Required]
        [MaxLength(GlobalConstants.MaxBlogPostAcronymLength)]
        public string Acronym { get; set; }

        [Required]
        public string ShortDescription { get; set; }

        [Required]
        public string FullDescription { get; set; }

        public IEnumerable<PostTag> Tags { get; set; }

        public IEnumerable<PostTag> AllTags { get; set; }

        public bool Active { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Data.Models.Post, CreateViewModel>()
                .ForMember(x => x.Tags, x => x.MapFrom(y => y.Tags.Select(t => t.PostTag)));
        }
    }
}