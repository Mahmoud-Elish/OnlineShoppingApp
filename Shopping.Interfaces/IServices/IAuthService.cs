using Shopping.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Interfaces;

public interface IAuthService
{
    Task<ResultDto> RegisterAsync(RegisterDto registerDto);
    Task<ResultDto> LoginAsync(LoginDto loginDto);
}
