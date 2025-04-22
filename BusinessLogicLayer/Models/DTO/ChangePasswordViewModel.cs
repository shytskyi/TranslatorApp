using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Models.DTO
{
    public class ChangePasswordViewModel
    {
        [Required]
        public int UserId { get; set; }

        [Required, DataType(DataType.Password)]
        public string OldPassword { get; set; } = string.Empty;

        [Required, DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;

        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
