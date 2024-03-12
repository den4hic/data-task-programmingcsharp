using data_task.Models;
using System.Globalization;

namespace data_task.Services
{
	public class PersonService : IPersonService
	{
		public async Task<Person> CalculatePersonInfoAsync(PersonModel model)
		{
			DateTime today = DateTime.Today;
			int age = today.Year - model.Birthdate.Year;

			if(model.Birthdate > today.AddYears(-age))
				age--;

			if (age > 135)
			{
				return new Person
				{
					ErrorMessage = "The age is too high"
				};
			}

			if (age < 0)
			{
				return new Person
				{
					ErrorMessage = "The age is negative"
				};
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

			return new Person(model.FirstName, model.LastName, model.Email, model.Birthdate, birthdayInfo);

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
				return chineseZodiacSigns[index - 1];
			});
		}
	}
}