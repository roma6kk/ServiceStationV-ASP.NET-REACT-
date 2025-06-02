using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceStationV.Core.Models
{
    public class Service
    {
        public const int MAX_NAME_LENGTH = 200;
        public const int MAX_DESCRIPTION_LENGTH = 500;
        public const int MAX_IMAGEPATH_LENGTH = 200;
        private Service(Guid id, string name, string description, decimal price, string imagePath)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            ImagePath = imagePath;
        }

        public Guid Id { get; }
        public string Name { get; } = string.Empty;
        public string Description { get; } = string.Empty;
        public decimal Price { get; }
        public string ImagePath { get; } = string.Empty;
        public static (Service Service, string Error) Create(Guid id,string name, string description, decimal price, string imagePath)
        {
            var error = string.Empty;
            if(string.IsNullOrEmpty(name) || name.Length > MAX_NAME_LENGTH)
            {
                error = "Service name can not be empty or larger then 250 symbols";
            }
            var service = new Service(id, name, description, price, imagePath);
            return (service,error);
        }
    }
}
