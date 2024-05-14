using System.ComponentModel.DataAnnotations;
using Blog.Data;

namespace Blog.ViewModels;

public class EditorCategoryViewModel
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(40, MinimumLength = 3, ErrorMessage = "this name must contain 3 - 40 characters")]
    public string Name { get; set; }
    [Required (ErrorMessage = "Slug is required")]
    public string Slug { get; set; }
}