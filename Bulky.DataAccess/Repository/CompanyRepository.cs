using Bulky.DataAccess.Context;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository;

public class CompanyRepository : Repository<CompanyModel>, ICompanyRepository
{
    private ApplicationDbContext _db;

    public CompanyRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }
    public void Update(CompanyModel obj)
    {
        _db.Companies.Update(obj);
    }
}
