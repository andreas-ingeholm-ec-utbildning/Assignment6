using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class TagAddView
{
    [MinLength(3)]
    public string Name { get; set; } = null!;
}