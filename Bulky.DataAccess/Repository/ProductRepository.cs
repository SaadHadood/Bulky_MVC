using Bulky.DataAccess.Context;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository;

public class ProductRepository : Repository<ProductModel>, IProductRepository
{
    private ApplicationDbContext _db; 
    public ProductRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
    public void Update(ProductModel obj)
    {
        _db.Products.Update(obj);
    }
}
