using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;
using ProductApi.Entities;

namespace ProductApi.Data
{
    public class ProductApiSeeder
    {
        private readonly ProductDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public ProductApiSeeder(ProductDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public void Seed()
        {
            _context.Database.EnsureCreated();
            
            if (!_context.Roles.Any())
            {
                var roles = GetRoles();
                _context.Roles.AddRange(roles);
                _context.SaveChanges();
            }

            if (!_context.Users.Any())
            {
                var users = GetUsers();
                _context.Users.AddRange(users);
                _context.SaveChanges();
            }

            if (!_context.Products.Any())
            {
                var products = ParseXmlFiles();
                foreach (var product in products)
                {
                    _context.Products.Add(product);
                }
                _context.SaveChanges();
            }
        }

        private List<User> GetUsers()
        {
            var users = new List<User>();

            for (int i = 1; i <= 15; i++)
            {
                User user = new User()
                {
                    Username = "testUser" + i.ToString(),
                    Password = "testPassword" + i.ToString(),
                    RoleId = 1
                };
                user.Password = _passwordHasher.HashPassword(user, user.Password);
                users.Add(user);
            }

            for (int i = 1; i <= 15; i++)
            {
                User user = new User()
                {
                    Username = "newTestUser" + i.ToString(),
                    Password = "newTestPassword" + i.ToString(),
                    RoleId = 1
                };
                user.Password = _passwordHasher.HashPassword(user, user.Password);
                users.Add(user);
            }
            return users;
        }

        private List<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                },
                new Role()
                {
                    Name = "Admin"
                }
            };
            return roles;
        }

        private static List<Product> ParseXmlFiles()
        {
            var filesFromSupplier1 = new List<string>
            {
                //there's no need to use first file as it is a shortened version of the second file
                //"wwwroot/xml/dostawca1plik1.xml",
                "data/xml/dostawca1plik2.xml"
            };

            var filesFromSupplier2 = new List<string>
            {
                //same as above
                //"wwwroot/xml/dostawca2plik1.xml",
                "data/xml/dostawca2plik2.xml"
            };

            var filesFromSupplier3 = new List<string>
            {
                 "data/xml/dostawca3plik1.xml"
            };

            var allProducts = new List<Product>();

            foreach (var path in filesFromSupplier1)
                allProducts.AddRange(ParseDataFromSupplier1(path));

            foreach (var path in filesFromSupplier2)
                allProducts.AddRange(ParseDataFromSupplier2(path));

            foreach (var path in filesFromSupplier3)
                allProducts.AddRange(ParseDataFromSupplier3(path));

            return allProducts;
        }

        private static List<Product> ParseDataFromSupplier1(string path)
        {
            XDocument xDocument = XDocument.Load(path);
            var root = xDocument.Root;

            return root.Descendants("product").Select(p => new Product
            {
                Name = p.Element("description").Elements("name").FirstOrDefault(n => n.Attribute("{http://www.w3.org/XML/1998/namespace}lang")?.Value == "pol")?.Value,
                Description = p.Element("description").Elements("long_desc").FirstOrDefault(n => n.Attribute("{http://www.w3.org/XML/1998/namespace}lang")?.Value == "pol")?.Value,
                Images = p.Element("images")?.Element("large")?.Elements("image").Select(x => new ProductImage { Url = x.Attribute("url").Value }).ToList(),
                Variants = new List<Variant>
                {
                    new Variant()
                    {
                        Color = p.Element("parameters").Elements("parameter")?.FirstOrDefault(param => param.Attribute("name")?.Value == "Kolor")?.Element("value")?.Attribute("name")?.Value,
                        Size = "N/A", //not provided by supplier
                        Weight = p.Element("parameters").Elements("parameter")?.FirstOrDefault(param => param.Attribute("name")?.Value == "Waga [g]")?.Element("value")?.Attribute("name")?.Value,
                        Dimensions = p.Element("parameters").Elements("parameter")?.FirstOrDefault(param => param.Attribute("name")?.Value == "Wymiary [cm]")?.Element("value")?.Attribute("name")?.Value
                    }
                }
            }).ToList();
        }

        private static List<Product> ParseDataFromSupplier2(string filepath)
        {
            XDocument xDocument = XDocument.Load(filepath);
            var root = xDocument.Root;

            return root.Descendants("product").Select(p => new Product
            {
                Name = p.Element("name").Value,
                Description = p.Element("desc").Value,
                Images = p.Element("photos")?.Elements("photo").Select(x => new ProductImage { Url = x.Value }).ToList(),
                Variants = new List<Variant>()
            {
                new Variant()
                {
                    Color = "N/A", //not provided by supplier
                    Size = "N/A", //not provided by supplier
                    Weight = p.Element("weight").Value,
                    Dimensions = "N/A" //not provided by supplier
                }
            }
            }).ToList();
        }

        private static List<Product> ParseDataFromSupplier3(string filepath)
        {
            XDocument xDocument = XDocument.Load(filepath);
            var root = xDocument.Root;

            return root.Descendants("produkt").Select(p => new Product
            {
                Name = p.Element("nazwa_pl").Value,
                Description = p.Element("dlugi_opis_pl").Value,
                Images = p.Element("zdjecia")?.Elements("zdjecie").Select(x => new ProductImage { Url = x.Attribute("url").Value }).ToList(),
                Variants = new List<Variant>()
            {
                new Variant()
                {
                    Color = p.Element("kolor").Value,
                    Size = p.Element("rozmiar").Value,
                    Weight = "N/A", //not provided by supplier
                    Dimensions = "N/A" //not provided by supplier
                }
            }
            }).ToList();
        }
    }
}
