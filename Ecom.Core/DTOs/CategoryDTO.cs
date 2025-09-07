using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.DTOs
{
    public record CategoryDTO //Just container not real class
   (  string Name, string? Description);
    public record updateCategoryDTO(string Name, string? Description, int id);
}
