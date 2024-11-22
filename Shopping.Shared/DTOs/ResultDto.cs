using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Shared;

public record ResultDto
(
    bool? Success = false,
    HttpStatusCode? Code = HttpStatusCode.NoContent,
    string Message = null
);
