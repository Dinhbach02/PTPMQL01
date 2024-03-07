using System.ComponentModel.DataAnnotations;
namespace MvcBach.Models ;
public class Person
{
    public string PersonId { get ; set;}
    public string FullName { get ; set ;}
    [ DataType(DataType.Date) ]
    
    public string Address { get ; set ;}    
   
}