using ByeBye.Entities;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string SurName { get; set; }
    public string? FatherName { get; set; }
    
    [DataType(DataType.Date)]
    [Display(Name = "Дата последнего входа в систему")]
    [DisplayFormatAttribute(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
    [Column(TypeName = "date")]
    public DateTime? LastLoginDate { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Дата окончания действия пароля")]
    [DisplayFormatAttribute(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
    [Column(TypeName = "date")]
    public DateTime? DateOfEnd { get; set; }

    public Position? Position { get; set; }

    public Department? Department { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Дата окончания контракта")]
    [DisplayFormatAttribute(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
    [Column(TypeName = "date")]
    public DateTime? ContractExpiryDate { get; set; }

    public string GetInitials
    {
        get { return getInitials(); }
    }

    private string getInitials() => $"{SurName} {FirstName}.{FatherName}.";
}
