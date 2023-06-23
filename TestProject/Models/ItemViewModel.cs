namespace TestProject.Models
{
    internal class ItemViewModel
    {
        public string Role { get; set; }
        public PaginatedList<Item> Paginations { get; set; }
    }
}