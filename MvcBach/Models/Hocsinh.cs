using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace MvcBach.Models
{
     [Table("HS")]
    public class Hocsinh
   
    { [Key]
    public string HocsinhID { get; set; }
    }
}