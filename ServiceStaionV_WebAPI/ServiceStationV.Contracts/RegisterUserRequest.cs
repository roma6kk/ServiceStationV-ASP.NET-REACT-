using System.ComponentModel.DataAnnotations;

namespace ServiceStationV.Contracts
{
    public record RegisterUserRequest(
        [Required(ErrorMessage = "UserName is required")]
        [StringLength(30, ErrorMessage = "UserName cannot exceed 30 characters")]
        string UserName,

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$",
            ErrorMessage = "Password does not meet complexity requirements.")]
        string Password,

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        string Email,

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^[+]?(375|7|8)(\d{9,12})$",
            ErrorMessage = "Invalid phone number format.")]
        string PhoneNumber);
}
