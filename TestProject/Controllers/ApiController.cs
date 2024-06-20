using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestProject.Models;
using TestProject.Services;

namespace TestProject.Controllers
{
	[Route("api")]
	[ApiController]
	[Authorize]
	public class ApiController : ControllerBase
	{
		private readonly IDataService<Item> _repo;
		private ITokenService _tokenService;

		public ApiController(IDataService<Item> repo, ITokenService tokenService)
		{
			_repo = repo;
			_tokenService = tokenService;
		}
		///<summary> 
		///API to get JWT token
		///</summary>
		[HttpGet("token")]
		[AllowAnonymous]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult<string> token(string email, string password)
		{
			var (token, error) = _tokenService.GenerateTokenAsync(email, password).Result;
			if (error != null)
			{
				return BadRequest(error);
			}
			return Ok(token);
		}

		///<summary> 
		///API to get all items
		///</summary>
		[HttpGet("getItems")]
		[Authorize(Roles = "Admin,User")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult<List<Item>> GetItems([FromQuery] string? t)
		{
			return Ok(_repo.GetAllItems().ToList());
		}

		///<summary> 
		///API to get all items with name longer than 5 characters
		///</summary>
		[HttpGet("getFilteredItems")]
		[Authorize(Roles = "Admin,User")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult<List<Item>> GetFilteredItems([FromQuery] string? t)
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
		[HttpGet("getItemById/{id}")]
		[Authorize(Roles = "Admin,User")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult<Item> GetItemById([FromQuery] string? t, int id)
		{
			var item = _repo.GetItemById(id);
			if (item != null)
			{
				return item;
			}
			return NotFound();
		}

		///<summary> 
		///API to create new item
		///</summary>
		[HttpPost("createItem")]
		[Authorize(Roles = "Admin")]
		[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Item))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult<Item> CreateItem([FromQuery] string? t, Item item)
		{
			if (ModelState.IsValid && item != null)
			{
				return _repo.CreateItem(item);
			}

			return BadRequest();

		}

		///<summary> 
		///API to edit existing item
		///</summary>
		[HttpPut("updateItem/{id}")]
		[Authorize(Roles = "Admin")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult<Item> UpdateItem([FromQuery] string? t, int id, [FromBody] Item item)
		{
			if (!ModelState.IsValid || item == null)
			{
				return BadRequest();
			}

			item.ID = id;
			var result = _repo.EditItem(item);

			if (result == null)
			{
				return NotFound();
			}
			return result;
		}

		///<summary> 
		///API to delete item by id
		///</summary>
		[HttpDelete("deleteItem/{id}")]
		[Authorize(Roles = "Admin")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		[ProducesResponseType(StatusCodes.Status403Forbidden)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult DeleteItem([FromQuery] string? t, int id)
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
