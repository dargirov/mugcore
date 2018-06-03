using MugStore.Data;
using MugStore.Data.Models;
using System.Linq;

namespace MugStore.Services.Data
{
    public class ImagesService : IImagesService
    {
        private readonly IDbRepository<Image> images;
        private readonly IDbRepository<ProductImage> productImages;

        public ImagesService(IDbRepository<Image> images, IDbRepository<ProductImage> productImages)
        {
            this.images = images;
            this.productImages = productImages;
        }

        public void Add(Image image)
        {
            this.images.Add(image);
            this.images.Save();
        }

        public Image Get(string name)
        {
            return this.images.All().Where(i => i.Name == name).FirstOrDefault();
        }

        public void Save()
        {
            this.images.Save();
        }

        public ProductImage GetProductImage(string name)
        {
            return this.productImages.All().Where(i => i.Name == name).FirstOrDefault();
        }

        public IQueryable<Image> Get()
        {
            return this.images.All();
        }
    }
}
