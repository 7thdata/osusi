namespace wppBacklog.Models
{
    public class ViewBaseModel
    {
        public string? Culture { get; set; }
        public int RCode { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public CurrentUserModel? User { get; set; }
    }

    public class CurrentUserModel
    {
        public string? Id { get; set; }  
        public string? Name { get; set; }
        public string? OrganizationId { get; set; }
        public string? ProjectId { get; set; }

    }
}
