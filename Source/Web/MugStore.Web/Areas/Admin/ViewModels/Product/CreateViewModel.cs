using AutoMapper;
using MugStore.Common;
using MugStore.Data.Models;
using MugStore.Web.Infrastructure.Mapping;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MugStore.Web.Areas.Admin.ViewModels.Product
{
    public class CreateViewModel : IMapFrom<Data.Models.Product>, IHaveCustomMappings
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.MaxProductTitleLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(GlobalConstants.MaxProductAcronymLength)]
        public string Acronym { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [MaxLength(GlobalConstants.MaxProductCodeLength)]
        public string Code { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public IEnumerable<Data.Models.Category> Categories { get; set; }

        public string PreviewData { get; set; }

        public virtual ICollection<ProductImage> Images { get; set; }

        public virtual IEnumerable<ProductTag> Tags { get; set; }

        public virtual IEnumerable<ProductTag> AllTags { get; set; }

        [MaxLength(GlobalConstants.LinkToProductImageMaxLength)]
        public string LinkToSource { get; set; }

        [Required]
        [MaxLength(GlobalConstants.MaxProductPageTitleLength)]
        public string PageTitle { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Data.Models.Product, CreateViewModel>()
                .ForMember(x => x.Tags, x => x.MapFrom(y => y.Tags.Select(t => t.ProductTag)));
        }
    }
}
