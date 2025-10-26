namespace IPB1.MvcApp.Models
{
    public class BlogCreateRequestModel
    {
        public string BlogTitle { get; set; }
        public string BlogAuthor { get; set; }
        public string BlogContent { get; set; }
    }

    public class BlogCreateResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class BlogEditRequestModel
    {
        public int BlogId { get; set; }
        public string BlogTitle { get; set; }
        public string BlogAuthor { get; set; }
        public string BlogContent { get; set; }
    }

    public class BlogDeleteResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
