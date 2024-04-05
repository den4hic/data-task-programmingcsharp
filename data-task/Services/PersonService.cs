using data_task.Exceptions;
using data_task.Models;
using Newtonsoft.Json;
using System.Globalization;
using System.Security.AccessControl;
using System.Text.Json;

namespace data_task.Services
{
	public class PersonService : IPersonService
	{
		public async Task<List<Person>> ProcessUsersFromFileAsync(string filePath)
		{
			string json = await File.ReadAllTextAsync(filePath);
			List<Person> userData = JsonConvert.DeserializeObject<List<Person>>(json);
			if (userData == null)
			{
				return new List<Person>();
			}
			return userData;
		}


		public async Task<Person> CalculatePersonInfoAsync(PersonModel model)
		{
			DateTime today = DateTime.Today;
			int age = today.Year - model.Birthdate.Year;

			if(model.Birthdate > today.AddYears(-age))
				age--;

			if (model.Birthdate > today)
			{
				throw new FutureBirthdateException("Дата народження не може бути у майбутньому.");
			}

			if (age > 135)
			{
				throw new AncientBirthdateException("Дата народження занадто давня.");
			}

			if (!IsValidEmail(model.Email))
			{
				throw new InvalidEmailException("Недійсна адреса електронної пошти.");
			}

			Task<bool> isAdultTask = IsAdultAsync(age);
			Task<string> sunSignTask = CalculateWesternZodiacSignAsync(model.Birthdate);
			Task<string> chineseSignTask = CalculateChineseZodiacSignAsync(model.Birthdate);
			Task<bool> isBirthdayTask = IsBirthdayAsync(model.Birthdate);

			await Task.WhenAll(isAdultTask, sunSignTask, chineseSignTask, isBirthdayTask);

			var birthdayInfo = new BirthdayResult
			{
				Age = age,
				IsAdult = isAdultTask.Result,
				IsBirthdayToday = isBirthdayTask.Result,
				WesternZodiacSign = sunSignTask.Result,
				ChineseZodiacSign = chineseSignTask.Result
			};

			return new Person(model.Id, model.FirstName, model.LastName, model.Email, model.Birthdate, birthdayInfo);

		}

		private async Task<bool> IsBirthdayAsync(DateTime birthdate)
		{
			return await Task.Run(() => DateTime.Now.Month == birthdate.Month && DateTime.Now.Day == birthdate.Day);
		}

		private async Task<bool> IsAdultAsync(int age)
		{
			return await Task.Run(() => age >= 18);
		}

		private async Task<string> CalculateWesternZodiacSignAsync(DateTime birthdate)
		{
			return await Task.Run(() =>
			{
				int month = birthdate.Month;
				int day = birthdate.Day;

				if (month == 3 && day >= 21 || month == 4 && day <= 20)
					return "Aries";
				else if (month == 4 && day >= 21 || month == 5 && day <= 21)
					return "Taurus";
				else if (month == 5 && day >= 22 || month == 6 && day <= 21)
					return "Gemini";
				else if (month == 6 && day >= 22 || month == 7 && day <= 22)
					return "Cancer";
				else if (month == 7 && day >= 23 || month == 8 && day <= 23)
					return "Leo";
				else if (month == 8 && day >= 24 || month == 9 && day <= 23)
					return "Virgo";
				else if (month == 9 && day >= 24 || month == 10 && day <= 23)
					return "Libra";
				else if (month == 10 && day >= 23 || month == 11 && day <= 22)
					return "Scorpio";
				else if (month == 11 && day >= 23 || month == 12 && day <= 22)
					return "Sagittarius";
				else if (month == 12 && day >= 23 || month == 1 && day <= 20)
					return "Capricorn";
				else if (month == 1 && day >= 21 || month == 2 && day <= 20)
					return "Aquarius";
				else if (month == 2 && day >= 21 || month == 3 && day <= 20)
					return "Pisces";

				return "Unknown";
			});
		}

		private async Task<string> CalculateChineseZodiacSignAsync(DateTime birthYear)
		{
			return await Task.Run(() =>
			{
				ChineseLunisolarCalendar chineseCalendar = new ChineseLunisolarCalendar();

				int year = chineseCalendar.GetSexagenaryYear(birthYear);

				string[] chineseZodiacSigns = { "Rat", "Ox", "Tiger", "Rabbit", "Dragon", "Snake", "Horse", "Goat", "Monkey", "Rooster", "Dog", "Pig" };
				int index = year % 12;
				return chineseZodiacSigns[index == 0 ? 11 : index - 1];
			});
		}

		private bool IsValidEmail(string email)
		{
			try
			{
				var addr = new System.Net.Mail.MailAddress(email);
				return addr.Address == email;
			}
			catch
			{
				return false;
			}
		}

		public List<Person> MakeSort(List<Person> users, string sortField, string sortDirection)
		{
			if (!string.IsNullOrEmpty(sortField))
			{
				switch (sortField)
				{
					case "firstName":
						users = sortDirection == "asc" ? users.OrderBy(p => p.FirstName).ToList() : users.OrderByDescending(p => p.FirstName).ToList();
						break;
					case "lastName":
						users = sortDirection == "asc" ? users.OrderBy(p => p.LastName).ToList() : users.OrderByDescending(p => p.LastName).ToList();
						break;
					case "emailAddress":
						users = sortDirection == "asc" ? users.OrderBy(p => p.EmailAddress).ToList() : users.OrderByDescending(p => p.EmailAddress).ToList();
						break;
					case "birthdate":
						users = sortDirection == "asc" ? users.OrderBy(p => p.BirthDate).ToList() : users.OrderByDescending(p => p.BirthDate).ToList();
						break;
					case "isAdult":
						users = sortDirection == "asc" ? users.OrderBy(p => p.IsAdult).ToList() : users.OrderByDescending(p => p.IsAdult).ToList();
						break;
					case "sunSign":
						users = sortDirection == "asc" ? users.OrderBy(p => p.SunSign).ToList() : users.OrderByDescending(p => p.SunSign).ToList();
						break;
					case "chineseSign":
						users = sortDirection == "asc" ? users.OrderBy(p => p.ChineseSign).ToList() : users.OrderByDescending(p => p.ChineseSign).ToList();
						break;
					case "isBirthday":
						users = sortDirection == "asc" ? users.OrderBy(p => p.IsBirthday).ToList() : users.OrderByDescending(p => p.IsBirthday).ToList();
						break;
					default:
						throw new InvalidSortFieldException("Wrong sort field.");
				}
			}

			return users;
		}

		public List<Person> SearchUsers(List<Person> cachedUsers, string? searchTerm)
		{
			return cachedUsers.Where(u => u.FirstName.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase) || u.LastName.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
		}

		public List<Person> SearchDateRange(List<Person> filteredUsers, string? startDate, string? endDate)
		{
			return filteredUsers.Where(u => u.BirthDate >= DateTime.Parse(startDate) && u.BirthDate <= DateTime.Parse(endDate)).ToList();
		}

		public List<Person> SearchSunSign(List<Person> filteredUsers, string searchSunSign)
		{
			return filteredUsers.Where(u => u.SunSign.StartsWith(searchSunSign, StringComparison.OrdinalIgnoreCase)).ToList();
		}

		public List<Person> SearchChineseSign(List<Person> filteredUsers, string searchChineseSign)
		{
			return filteredUsers.Where(u => u.ChineseSign.StartsWith(searchChineseSign, StringComparison.OrdinalIgnoreCase)).ToList();
		}

		public List<Person> SearchEmail(List<Person> filteredUsers, string searchEmail)
		{
			return filteredUsers.Where(u => u.EmailAddress.StartsWith(searchEmail, StringComparison.OrdinalIgnoreCase)).ToList();
		}

		public void SerializeUsersToFile(List<Person> users, string filePath)
		{
			try
			{
				string jsonData = System.Text.Json.JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });

				File.WriteAllText(filePath, jsonData);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Something wrong: " + ex.Message);
			}

		}

		public List<Person> FilterAdult(List<Person> filteredUsers, string? filterIsAdult)
		{
			return filteredUsers.Where(u => u.IsAdult == (filterIsAdult == "true")).ToList();
		}

		public List<Person> FilterBirthday(List<Person> filteredUsers, string? filterIsAdult)
		{
			return filteredUsers.Where(u => u.IsBirthday == (filterIsAdult == "true")).ToList();
		}
	}
}