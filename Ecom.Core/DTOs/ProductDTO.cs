using Ecom.Core.Entities.Product;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.DTOs
{
    public record ProductDTO
    {
        public string categoryName { get; set; }


        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public virtual List<PhotoDTO> Photos { get; set; }
    }
    public record PhotoDTO
    {
        public int ImageId { get; set; }
        public string ImageName { get; set; }

    }
    public record AddPrroductDTO {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }

        public int CategoryId { get; set; }
        public IFormFileCollection Photo { get; set; }

    }

}
