using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NovumLoginServer.DBModels;

[Table("users")]
public class Users
{
    [Key]
    [Required]
    [Column("id")]
    public int ID { get; set; }
    
    [Required]
    [Column("name")]
    public string Name { get; set; }
    
    [Required]
    [Column("passhash")]
    public string Passhash { get; set; }
    
    [Required]
    [Column("salt")]
    [MaxLength]
    public string Salt { get; set; }
    
    
    [Required]
    [Column("email")]
    public string Email { get; set; }
}