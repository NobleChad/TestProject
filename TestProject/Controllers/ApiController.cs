using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using TestProject.Data;
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
		private SignInManager<ApplicationUser> _manager;
		private UserManager<ApplicationUser> _userManager;
		private IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
		private KeyManager _keyManager;

		public ApiController(IDataService<Item> repo,
			SignInManager<ApplicationUser> manager,
			UserManager<ApplicationUser> userManager,
			IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
			KeyManager keyManager)
		{
			_repo = repo;
			_manager = manager;
			_userManager = userManager;
			_claimsFactory = claimsFactory;
			_keyManager = keyManager;
		}
		///<summary> 
		///API to get JWT token
		///</summary>
		[HttpGet("token")]
		[AllowAnonymous]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult> token(string email, string password)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user != null)
			{
				var result = await _manager.CheckPasswordSignInAsync(user, password, false);
				if (result.Succeeded)
				{
					var principal = await _claimsFactory.CreateAsync(user);
					var identity = principal.Identities.First();
					identity.AddClaim(new Claim("amr", "pwd"));
					identity.AddClaim(new Claim("method", "jwt"));
					var handler = new JsonWebTokenHandler();
					var key = new RsaSecurityKey(_keyManager.rsaKey);
					var token = handler.CreateToken(new SecurityTokenDescriptor()
					{
						Issuer = "https://localhost:7258",
						Subject = identity,
						SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.RsaSha256),
					});
					return Ok(token);
				}
			}
			return NotFound();
		}

		///<summary> 
		///API to get all items
		///</summary>
		[HttpGet("GetItems")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult<List<Item>> GetItems([FromQuery] string t)
		{
			return Ok(_repo.GetAllItems().ToList());
		}

		///<summary> 
		///API to get all items with name longer than 5 characters
		///</summary>
		[HttpGet("GetFilteredItems")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult<List<Item>> GetFilteredItems([FromQuery] string t)
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
		public ActionResult<Item> GetItemById([FromQuery] string t, int id)
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
		[HttpPost("CreateItem")]
		[ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Item))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult<Item> CreateItem([FromQuery] string t, Item item)
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
		[HttpPut("UpdateItem/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public ActionResult<Item> UpdateItem([FromQuery] string t, int id, [FromBody] Item item)
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
		[HttpDelete("DeleteItem/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult DeleteItem([FromQuery] string t, int id)
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
