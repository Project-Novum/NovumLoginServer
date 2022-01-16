using System.ComponentModel.DataAnnotations;

namespace NovumLoginServer.DBModels;

public class User
{
    [Key]
    [Required]
    public int ID { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Passhash { get; set; }
    
    [Required]
    public string Salt { get; set; }

    [Required]
    public string Email { get; set; }


    public string? GameSessionId { get; set; }
}