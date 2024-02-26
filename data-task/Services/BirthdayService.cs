using data_task.Models;
using System.Globalization;

namespace data_task.Services
{
	public class BirthdayService : IBirthdayService
	{
		public BirthdayResult CalculateBirthdayInfo(BirthdayModel model)
		{
			DateTime today = DateTime.Today;
			int age = today.Year - model.Birthdate.Year;

			if(model.Birthdate > today.AddYears(-age))
				age--;

			if (age > 135)
			{
				return new BirthdayResult
				{
					ErrorMessage = "The age is too high"
				};
			}

			if (age < 0)
			{
				return new BirthdayResult
				{
					ErrorMessage = "The age is negative"
				};
			}

			string westernZodiacSign = CalculateWesternZodiacSign(model.Birthdate);
			string chineseZodiacSign = CalculateChineseZodiacSign(model.Birthdate);

			return new BirthdayResult
			{
				Age = age,
				IsBirthdayToday = today.Day == model.Birthdate.Day && today.Month == model.Birthdate.Month,
				WesternZodiacSign = westernZodiacSign,
				ChineseZodiacSign = chineseZodiacSign
			};
		}

		private string CalculateWesternZodiacSign(DateTime birthdate)
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
		}

		private string CalculateChineseZodiacSign(DateTime birthYear)
		{
			ChineseLunisolarCalendar chineseCalendar = new ChineseLunisolarCalendar();

			int year = chineseCalendar.GetSexagenaryYear(birthYear);

			string[] chineseZodiacSigns = { "Rat", "Ox", "Tiger", "Rabbit", "Dragon", "Snake", "Horse", "Goat", "Monkey", "Rooster", "Dog", "Pig" };
			int index = year % 12;
			return chineseZodiacSigns[index - 1];
		}
	}
}