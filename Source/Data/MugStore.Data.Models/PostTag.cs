using MugStore.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MugStore.Data.Models
{
    public class PostTag : BaseModel<int>
    {
        public PostTag()
        {
            this.Posts = new HashSet<PostTagPost>();
        }

        [Required]
        [MaxLength(GlobalConstants.PostTagMaxlength)]
        public string Name { get; set; }

        [Required]
        [MaxLength(GlobalConstants.PostTagAcronymMaxlength)]
        public string Acronym { get; set; }

        [Required]
        public bool Active { get; set; }

        public virtual ICollection<PostTagPost> Posts { get; set; }
    }
}
