namespace data_task.Models
{
	public class Person
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string EmailAddress { get; set; }
		public DateTime BirthDate { get; set; }
        public string ErrorMessage { get; set; }
        private readonly bool isAdult;
		private readonly string sunSign;
		private readonly string chineseSign;
		private readonly bool isBirthday;

		public bool IsAdult { get { return isAdult; } }
		public string SunSign { get { return sunSign; } }
		public string ChineseSign { get { return chineseSign; } }
		public bool IsBirthday { get { return isBirthday; } }

		public Person(string firstName, string lastName, string emailAddress, DateTime birthDate, BirthdayResult birthdayInfo)
			:this(firstName, lastName, emailAddress, birthDate)
		{
			isAdult = birthdayInfo.IsAdult;
			sunSign = birthdayInfo.WesternZodiacSign;
			chineseSign = birthdayInfo.ChineseZodiacSign;
			isBirthday = birthdayInfo.IsBirthdayToday;
		}

		public Person(string firstName, string lastName, string emailAddress, DateTime birthDate)
		{
			FirstName = firstName;
			LastName = lastName;
			EmailAddress = emailAddress;
			BirthDate = birthDate;
		}

		public Person(string firstName, string lastName, string emailAddress)
			: this(firstName, lastName, emailAddress, DateTime.MinValue)
		{
		}

		public Person(string firstName, string lastName, DateTime birthDate)
			: this(firstName, lastName, string.Empty, birthDate)
		{
		}

		public Person()
			: this(string.Empty, string.Empty, string.Empty, DateTime.MinValue)
		{
		}

		public async Task<bool> IsAdultAsync()
		{
			return await Task.Run(() => DateTime.Now.Year - BirthDate.Year >= 18);
		}

		public async Task<string> SunSignAsync()
		{
			return await Task.Run(() =>
			{
				int month = BirthDate.Month;
				int day = BirthDate.Day;

				if ((month == 3 && day >= 21) || (month == 4 && day <= 19))
					return "Aries";
				else if ((month == 4 && day >= 20) || (month == 5 && day <= 20))
					return "Taurus";
				// Add more cases for other sun signs...
				return "Unknown";
			});
		}

		public async Task<string> ChineseSignAsync()
		{
			return await Task.Run(() =>
			{
				int year = BirthDate.Year % 12;
				switch (year)
				{
					case 0: return "Monkey";
					case 1: return "Rooster";
					case 2: return "Dog";
					// Add more cases for other Chinese signs...
					default: return "Unknown";
				}
			});
		}

		public async Task<bool> IsBirthdayAsync()
		{
			return await Task.Run(() => DateTime.Now.Month == BirthDate.Month && DateTime.Now.Day == BirthDate.Day);
		}

	}
}
