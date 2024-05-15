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

        // Constructor for derived classes
        protected User(string name, string password)
        {
            _fName = name;
            _fPassword = password;
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
    }
}

