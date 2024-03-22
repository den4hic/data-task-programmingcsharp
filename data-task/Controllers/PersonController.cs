using data_task.Exceptions;
using data_task.Models;
using data_task.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace data_task.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PersonController : ControllerBase
	{
		private readonly ILogger<PersonController> _logger;
		private readonly IPersonService _personService;

		public PersonController(ILogger<PersonController> logger, IPersonService personService)
		{
			_logger = logger;
			_personService = personService;
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] PersonRequestModel request)
		{
			if (string.IsNullOrEmpty(request.FirstName) || string.IsNullOrEmpty(request.LastName) || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Birthdate.ToString()))
			{
				return BadRequest(new Person() { ErrorMessage = "Bad data" });
			}

			_logger.LogInformation($"Отримано дату народження: {request.Birthdate}");

			var personModel = new PersonModel
			{
				FirstName = request.FirstName,
				LastName = request.LastName,
				Email = request.Email,
				Birthdate = DateTime.Parse(request.Birthdate)
			};
			
			try
			{
				var result = await _personService.CalculatePersonInfoAsync(personModel);
				return new JsonResult(result);
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
		}
	}
}
