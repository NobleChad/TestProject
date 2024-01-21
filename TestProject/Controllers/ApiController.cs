using Microsoft.AspNetCore.Mvc;
using TestProject.Models;
using TestProject.Services;

namespace TestProject.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
		private readonly IDataService<Item> _repo;

		public ApiController(IDataService<Item> repo)
        {
			_repo = repo;

		}

        ///<summary> 
        ///API to get all items
        ///</summary>
        [HttpGet("GetItems")]
        public ActionResult<List<Item>> GetItems()
        {
            return _repo.GetAllItems().ToList();
        }

		///<summary> 
		///API to get all items with name longer than 5 characters
		///</summary>
		[HttpGet("GetFilteredItems")]
		public ActionResult<List<Item>> GetFilteredItems()
		{
			return _repo.GetAllItems(a => {
               return a.Where(b => b.Name.Length > 5);
            }).ToList();
		}

		///<summary> 
		///API to get item by id
		///</summary>
		[HttpGet("GetItemById/{id}")]
        public ActionResult<Item> GetItemById(int id)
        {
			return _repo.GetItemById(id);
		}

        ///<summary> 
        ///API to create new item
        ///</summary>
        [HttpPost("CreateItem")]
        public ActionResult<Item> CreateItem(Item item)
        {
            return _repo.CreateItem(item);

		}

        ///<summary> 
        ///API to edit existing item
        ///</summary>
        [HttpPut("UpdateTask/{id}")]
        public ActionResult<Item> UpdateItem(int id,[FromBody] Item item)
        {
            item.ID = id;
            return _repo.EditItem(item);
		}

		///<summary> 
		///API to delete item by id
		///</summary>
		[HttpDelete("DeleteTask/{id}")]
        public void DeleteItem(Item item)
        {
            _repo.Delete(item);
        }
    }

}
