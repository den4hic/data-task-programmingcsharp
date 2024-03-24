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
	}
}
