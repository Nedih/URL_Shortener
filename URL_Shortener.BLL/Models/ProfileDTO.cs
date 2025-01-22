
namespace URL_Shortener.BLL.Models
{
    public class ProfileDTO
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Image { get; set; }
        public string DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }

        public static explicit operator ProfileDTO(UserDTO user)
        {
            return new ProfileDTO
            {
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };
        }
    }
}
