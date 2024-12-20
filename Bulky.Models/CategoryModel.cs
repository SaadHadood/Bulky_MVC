﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bulky.Models;

public class CategoryModel
{
    [Key]
    public int Id { get; set; }

    [DisplayName("Category Name")]
    [MaxLength(30)]
    public string Name { get; set; } = null!;

    [DisplayName("Display Order")]
    [Range(1,100, ErrorMessage = "Display order must be between 1-100")]
    public int DisplayOrder { get; set; }

}
