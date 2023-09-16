namespace TestProject.Models
{
    public class ItemViewModel
    {
        public string Role { get; set; }
        public PaginatedList<Item> Paginations { get; set; }
    }
}