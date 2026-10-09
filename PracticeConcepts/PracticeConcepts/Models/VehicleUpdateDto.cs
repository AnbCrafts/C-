using System;
using System.Collections.Generic;
using System.Text;


using PracticeConcepts.Models.Enum;

namespace PracticeConcepts.Models;

public class VehicleUpdateDto
{
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public VehicleType Type { get; set; }
    public decimal Price { get; set; }
}
