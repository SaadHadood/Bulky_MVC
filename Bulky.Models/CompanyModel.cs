﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Models;

public class CompanyModel
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = null!;
    public string? StreetAddress { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }

    [DataType(DataType.PhoneNumber)]
    public int? PhoneNumber { get; set; }
}