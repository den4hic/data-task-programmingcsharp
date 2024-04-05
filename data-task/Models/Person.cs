namespace data_task.Models
{
	public class Person
	{
        public int Id { get; set; }
        public string FirstName { get; set; }
		public string LastName { get; set; }
		public string EmailAddress { get; set; }
		public DateTime BirthDate { get; set; }
        public string ErrorMessage { get; set; }

		public bool IsAdult { get; set;  }
		public string SunSign { get; set; }
		public string ChineseSign { get; set; }
		public bool IsBirthday { get; set; }

		public Person(int id, string firstName, string lastName, string emailAddress, DateTime birthDate, BirthdayResult birthdayInfo)
			:this(firstName, lastName, emailAddress, birthDate)
		{
			Id = id;
			IsAdult = birthdayInfo.IsAdult;
			SunSign = birthdayInfo.WesternZodiacSign;
			ChineseSign = birthdayInfo.ChineseZodiacSign;
			IsBirthday = birthdayInfo.IsBirthdayToday;
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
