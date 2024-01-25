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
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult<List<Item>> GetItems()
		{
			return _repo.GetAllItems().ToList();
		}

		///<summary> 
		///API to get all items with name longer than 5 characters
		///</summary>
		[HttpGet("GetFilteredItems")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult<List<Item>> GetFilteredItems()
		{
			var data = _repo.GetAllItems(a =>
			{
				return a.Where(b => b.Name.Length > 5);
			}).ToList();
			return data;
		}

		///<summary> 
		///API to get item by id
		///</summary>
		[HttpGet("GetItemById/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult<Item> GetItemById(int id)
		{
			var item = _repo.GetItemById(id);
			if (item != null) {
				return item;
			}
			return NotFound();
		}

		///<summary> 
		///API to create new item
		///</summary>
		[HttpPost("CreateItem")]
		[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Item))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult<Item> CreateItem(Item item)
		{
			if (ModelState.IsValid) {
				return _repo.CreateItem(item);
			}

			return BadRequest();

		}

		///<summary> 
		///API to edit existing item
		///</summary>
		[HttpPut("UpdateItem/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult<Item> UpdateItem(int id, [FromBody] Item item)
		{
			if(!ModelState.IsValid) {
				return BadRequest();
			}

			item.ID = id;
			var result = _repo.EditItem(item);

			if (result == null) {
				return NotFound();
			}
			return result;
		}

		///<summary> 
		///API to delete item by id
		///</summary>
		[HttpDelete("DeleteItem/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult DeleteItem(int id)
		{
			var result = _repo.Delete(id);
			if (result == null)
			{
				return NotFound();
			}
			return Ok();
		}
	}

}
