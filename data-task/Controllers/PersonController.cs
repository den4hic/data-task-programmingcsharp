using data_task.Exceptions;
using data_task.Models;
using data_task.Services;
using Microsoft.AspNetCore.Mvc;

namespace data_task.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PersonController : ControllerBase
	{
		private static List<Person> _cachedUsers;
		private static List<Person> _allUsers;
		private readonly ILogger<PersonController> _logger;
		private readonly IPersonService _personService;

		public PersonController(ILogger<PersonController> logger, IPersonService personService)
		{
			_logger = logger;
			_personService = personService;
		}

		[HttpGet("filtered")]
		public IActionResult GetFilteredUsers([FromQuery] string? searchTerm, [FromQuery] string? startDate, [FromQuery] string? endDate, [FromQuery] string? searchEmail, [FromQuery] string? searchSunSign, [FromQuery] string? searchChineseSign, [FromQuery] string? filterIsAdult, [FromQuery] string? filterIsBirthday)
		{
			try
			{
				var filteredUsers = _allUsers;

				if (!string.IsNullOrEmpty(searchTerm))
				{
					filteredUsers = _personService.SearchUsers(filteredUsers, searchTerm);
				}

				if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
				{
					filteredUsers = _personService.SearchDateRange(filteredUsers, startDate, endDate);
				}

				if (!string.IsNullOrEmpty(searchEmail))
				{
					filteredUsers = _personService.SearchEmail(filteredUsers, searchEmail);
				}

				if (!string.IsNullOrEmpty(searchSunSign))
				{
					filteredUsers = _personService.SearchSunSign(filteredUsers, searchSunSign);
				}

				if (!string.IsNullOrEmpty(searchChineseSign))
				{
					filteredUsers = _personService.SearchChineseSign(filteredUsers, searchChineseSign);
				}

				if(filterIsAdult != "any")
				{
					filteredUsers = _personService.FilterAdult(filteredUsers, filterIsAdult);
				}

				if(filterIsBirthday != "any")
				{
					filteredUsers = _personService.FilterBirthday(filteredUsers, filterIsAdult);
				}

				_cachedUsers = filteredUsers;

				return Ok(filteredUsers);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error filtering users: {ex.Message}");
				return StatusCode(500, "Internal Server Error");
			}
		}

		[HttpGet("sorted")]
		public async Task<IActionResult> Get([FromQuery] string? sortField, [FromQuery] string sortDirection = "asc")
		{
			try
			{
				if (_cachedUsers == null)
				{
					_cachedUsers = await _personService.ProcessUsersFromFileAsync("./users.json");
					_allUsers = _cachedUsers;
					return new JsonResult(_cachedUsers);
				}

				if (string.IsNullOrEmpty(sortField))
				{
					return new JsonResult(_cachedUsers);
				}

				var result = _personService.MakeSort(_cachedUsers, sortField, sortDirection);

				return new JsonResult(result.ToList());
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error getting users: {ex.Message}");
				return StatusCode(500, "Internal Server Error");
			}
			
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] PersonRequestModel request)
		{
			try
			{
				var personModel = new PersonModel
				{
					Id = _cachedUsers.Count + 1,
					FirstName = request.FirstName,
					LastName = request.LastName,
					Email = request.Email,
					Birthdate = DateTime.Parse(request.Birthdate)
				};
				
				var newUser = await _personService.CalculatePersonInfoAsync(personModel);

				if (_cachedUsers == null)
				{
					_cachedUsers = new List<Person>();
				}

				_cachedUsers.Add(newUser);

				_personService.SerializeUsersToFile(_cachedUsers, "./users.json");

				return new JsonResult(newUser);
			}
			catch (FutureBirthdateException ex)
			{
				return BadRequest(new Person() { ErrorMessage = ex.Message });
			}
			catch (AncientBirthdateException ex)
			{
				return BadRequest(new Person() { ErrorMessage = ex.Message });
			}
			catch (InvalidEmailException ex)
			{
				return BadRequest(new Person() { ErrorMessage = ex.Message });
			}
			catch (InvalidSortFieldException ex)
			{
				return BadRequest(new Person() { ErrorMessage = ex.Message });
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error adding user: {ex.Message}");
				return StatusCode(500, "Internal Server Error");
			}
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			try
			{
				var userToDelete = _cachedUsers.FirstOrDefault(user => user.Id == id);
				if (userToDelete == null)
				{
					return NotFound();
				}

				_cachedUsers.Remove(userToDelete);				

				_personService.SerializeUsersToFile(_cachedUsers, "./users.json");

				return Ok(_cachedUsers);
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error deleting user: {ex.Message}");
				return StatusCode(500, "Internal Server Error");
			}
		}
		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, [FromBody] PersonRequestModel updatedPerson)
		{
			try
			{
				var userToUpdate = _cachedUsers.FirstOrDefault(user => user.Id == id);
				if (userToUpdate == null)
				{
					return NotFound();
				}

				var personModel = new PersonModel
				{
					Id = userToUpdate.Id,
					FirstName = updatedPerson.FirstName,
					LastName = updatedPerson.LastName,
					Email = updatedPerson.Email,
					Birthdate = DateTime.Parse(updatedPerson.Birthdate)
				};

				var resultPerson = await _personService.CalculatePersonInfoAsync(personModel);

				userToUpdate.FirstName = resultPerson.FirstName;
				userToUpdate.LastName = resultPerson.LastName;
				userToUpdate.EmailAddress = resultPerson.EmailAddress;
				userToUpdate.BirthDate = resultPerson.BirthDate;
				userToUpdate.IsAdult = resultPerson.IsAdult;
				userToUpdate.SunSign = resultPerson.SunSign;
				userToUpdate.ChineseSign = resultPerson.ChineseSign;
				userToUpdate.IsBirthday = resultPerson.IsBirthday;

				_personService.SerializeUsersToFile(_cachedUsers, "./users.json");

				return new JsonResult(userToUpdate);
			}
			catch (FutureBirthdateException ex)
			{
				return BadRequest(new Person() { ErrorMessage = ex.Message });
			}
			catch (AncientBirthdateException ex)
			{
				return BadRequest(new Person() { ErrorMessage = ex.Message });
			}
			catch (InvalidEmailException ex)
			{
				return BadRequest(new Person() { ErrorMessage = ex.Message });
			}
			catch (InvalidSortFieldException ex)
			{
				return BadRequest(new Person() { ErrorMessage = ex.Message });
			}
			catch (Exception ex)
			{
				_logger.LogError($"Error updating user: {ex.Message}");
				return StatusCode(500, "Internal Server Error");
			}
		}


	}
}
