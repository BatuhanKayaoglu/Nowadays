using Nowadays.Entity.Models;
using Microsoft.EntityFrameworkCore;
using Nowadays.Infrastructure.Context;
using Nowadays.Infrastructure.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nowadays.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NowadaysContext _context;

        public UnitOfWork(NowadaysContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(DbContext));
            Companies = new CompanyRepository(_context);
            Projects = new ProjectRepository(_context);
            Employees = new EmployeeRepository(_context);
            Issue = new IssueRepository(_context);
            Report = new ReportRepository(_context);    
        }

        public ICompanyRepository Companies { get; private set; }
        public IProjectRepository Projects { get; private set; }
        public IEmployeeRepository Employees { get; private set; }
        public IIssueRepository Issue { get; private set; }
        public IReportRepository Report { get; private set; }


        public int SaveChanges() => _context.SaveChanges();

        public void Dispose() => _context.Dispose();


    }




    //public class UnitOfWork : IUnitOfWork
    //{
    //    private readonly ECommerceContext _context;

    //    private CategoryRepository _categoryRepository;
    //    private ProductRepository _productRepository;


    //    public UnitOfWork(ECommerceContext context, ProductRepository productRepository)
    //    {
    //        _context = context ?? throw new ArgumentNullException(nameof(context));
    //        _categoryRepository = new CategoryRepository(_context);
    //        _productRepository = new ProductRepository(_context);
    //    }

    //    //public CategoryRepository CategoryRepository => _categoryRepository ?? (this._categoryRepository = new CategoryRepository(_context));

    //    public ICategoryRepository Categories => _categoryRepository;
    //    public IProductRepository Products => _productRepository;



    //    public void SaveChanges()
    //    {
    //        try
    //        {
    //            using (var transaction = _context.Database.BeginTransaction())
    //            {
    //                try
    //                {
    //                    _context.SaveChanges();
    //                    transaction.Commit();
    //                }
    //                catch
    //                {
    //                    transaction.Rollback();
    //                    throw;
    //                }
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            //TODO:Logging
    //        }
    //    }
    //    private bool disposed = false;
    //    protected virtual void Dispose(bool disposing)
    //    {
    //        if (!this.disposed)
    //        {
    //            if (disposing)
    //            {
    //                _context.Dispose();
    //            }
    //        }
    //        this.disposed = true;
    //    }
    //    public void Dispose()
    //    {
    //        Dispose(true);
    //        GC.SuppressFinalize(this);
    //    }

    //}
}
