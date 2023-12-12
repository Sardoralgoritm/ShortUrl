using System.ComponentModel.DataAnnotations;

namespace ApiForUrl.DataAccess.Entities;

public class BaseModel
{
    [Key, Required]
    public int Id { get; set; }
    public DateTime CreateAt { get; set; }
}
