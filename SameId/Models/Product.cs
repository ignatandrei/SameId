using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SameId.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public static Product GetFromId(int id)
        {
            return new Product() { Id = id, Name = " product number :" + id };
        }
    }
}