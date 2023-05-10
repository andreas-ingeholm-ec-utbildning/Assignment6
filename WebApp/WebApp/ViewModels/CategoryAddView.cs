using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class CategoryAddView
{
    [MinLength(3)]
    public string Name { get; set; } = null!;
}