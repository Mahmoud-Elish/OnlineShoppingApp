using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Shared;

public class UpdateItemDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string ItemName { get; set; }
    [StringLength(100, MinimumLength = 3)]
    public string? Description { get; set; }
    [Required]
    public int UomId { get; set; }
    public int? Quantity { get; set; } = 0;
    public decimal? Price { get; set; } = decimal.Zero;
}
