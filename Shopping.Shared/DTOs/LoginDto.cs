using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Shared;

public class LoginDto
{
    [Required]
    //[EmailAddress]
    public string UserName { get; set; }

    [Required]
    //[StringLength(100, MinimumLength = 6)]
    public string Password { get; set; }
}
