﻿using MugStore.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MugStore.Data.Models
{
    public class Post : BaseModel<int>
    {
        public Post()
        {
            this.Tags = new HashSet<PostTagPost>();
        }

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

        public ICollection<PostTagPost> Tags { get; set; }

        [Required]
        public bool Active { get; set; }
    }
}
