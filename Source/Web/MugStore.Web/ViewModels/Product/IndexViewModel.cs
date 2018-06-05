using AutoMapper;
using MugStore.Data.Models;
using MugStore.Web.Infrastructure.Mapping;
using System.Collections.Generic;
using System.Linq;

namespace MugStore.Web.ViewModels.Product
{
    public class IndexViewModel : IMapFrom<Data.Models.Product>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Acronym { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }

        public virtual Category Category { get; set; }

        public string PreviewData { get; set; }

        public virtual IEnumerable<ProductImage> Images { get; set; }

        public virtual IEnumerable<ProductTag> Tags { get; set; }

        public string PageTitle { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Data.Models.Product, IndexViewModel>()
                .ForMember(x => x.Tags, x => x.MapFrom(y => y.Tags.Select(t => t.ProductTag)));
        }
    }
}
