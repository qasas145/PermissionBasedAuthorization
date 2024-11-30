using System.ComponentModel.DataAnnotations;

public class RoleFormViewModel {
    [MaxLength(25), Required]
    public string Role{get;set;}
}