namespace MugStore.Data.Models
{
    public class PostTagPost
    {
        public int PostId { get; set; }
        public Post Post { get; set; }

        public int PostTagId { get; set; }
        public PostTag PostTag { get; set; }
    }
}
