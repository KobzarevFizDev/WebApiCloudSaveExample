using System.ComponentModel.DataAnnotations;

public class AuthenticationRequest
{
    [Required]
    public string Login { get; set; }
    [Required]
    public string Password { get; set; }
}