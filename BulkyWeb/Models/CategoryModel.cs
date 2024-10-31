using System.ComponentModel;

namespace BulkyWeb.Models;

public class CategoryModel
{
    public int Id { get; set; }

    [DisplayName("Category Name")]
    public string Name { get; set; } = null!;

    [DisplayName("Display Order")]
    public int DisplayOrder { get; set; }

}
