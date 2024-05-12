using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01RestaurantSystem
{
    public class Customer : User
    {
        private string _address;
        private string _phoneNumber;

        // Constructor
        public Customer(string name, string password, string email, string dateOfBirth, string address, string phoneNumber)
            : base(name, password, email, dateOfBirth)
        {
            _address = address;
            _phoneNumber = phoneNumber;
        }

        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
            }
        }

        public string PhoneNumber
        {
            get
            {
                return _phoneNumber;
            }
            set
            {
                _phoneNumber = value;
            }
        }

        // Implementing the abstract method from the base class
        public override void DisplayInfo()
        {
            Console.WriteLine($"Customer Information:\n" +
                              $"Name: {Name}\n" +
                              $"Email: {Email}\n" +
                              $"Date of Birth: {DateOfBirth}\n" +
                              $"Address: {Address}\n" +
                              $"Phone Number: {PhoneNumber}\n");
        }
    }
}

