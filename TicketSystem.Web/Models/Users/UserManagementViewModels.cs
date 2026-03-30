using System.ComponentModel.DataAnnotations;

namespace TicketSystem.Web.Models.Users
{
    public class UserListViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public bool IsActive { get; set; }
    }

    public class CreateUserViewModel
    {
        [Required(ErrorMessage = "O E-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "O perfil (Role) é obrigatório")]
        [Display(Name = "Perfil")]
        public string RoleId { get; set; }
    }

}
