using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace URL_Shortener.BLL.Models
{
    public class UserDTO 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public UserDTO() { }
        public UserDTO(string id, string email, ProfileDTO profile)
        {
            Id = id;
            Email = email;
            Name = profile.Name;
            PhoneNumber = profile.PhoneNumber;
        }
    }
}
