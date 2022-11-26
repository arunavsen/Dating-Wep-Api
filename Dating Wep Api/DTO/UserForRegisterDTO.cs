using System.ComponentModel.DataAnnotations;

namespace Dating_Wep_Api.DTO
{
    public class UserForRegisterDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage ="Username can not be blank")]
        public string  username { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage ="Password length should be 4 to 8")]
        public string  password { get; set; }
    }
}
