using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Dto
{
    public record Response(
    int Error,
    String Message,
    object? Data
);
}
