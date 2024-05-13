using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group01RestaurantSystem.Roles
{
    public abstract class User
    {
        private string _fName;
        private string _fPassword;
        private string _fEmail;
        private string _fDateOfBirth;

        // Constructor for derived classes
        protected User(string name, string password, string email, string dateOfBirth)
        {
            _fName = name;
            _fPassword = password;
            _fEmail = email;
            _fDateOfBirth = dateOfBirth;
        }

        // Abstract method to be implemented by derived classes
        public abstract void DisplayInfo();

        public string Name
        {
            get 
            { 
                return _fName; 
            }
            set
            { 
                _fName = value; 
            }
        }

        public string Password
        {
            get 
            { 
                return _fPassword; 
            }
            set 
            { 
                _fPassword = value; 
            }
        }

        public string Email
        {
            get 
            { 
                return _fEmail; 
            }
            set 
            { 
                _fEmail = value; 
            }
        }

        public string DateOfBirth
        {
            get 
            { 
                return _fDateOfBirth; 
            }
            set 
            { 
                _fDateOfBirth = value; 
            }
        }
    }
}

