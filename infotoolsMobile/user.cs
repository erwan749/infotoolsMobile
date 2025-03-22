using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace infotoolsMobile
{
    internal class user
    {
        int _id;
        string _name;
        string _token;
        public int Id 
        { 
            get { return _id; } 
            set { _id = value; } 
        }
        public string name
        {
            get { return _name; }
            set { _name = value; }

        }
        public string token
        {
            get { return _token;  }
            set { _token = value; }
        }
    }
    
}
