using System.Collections;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using csharp_training_202605.Applications.Domains;
using csharp_training_202605.Exceptions;
using csharp_training_202605.Infrastructures.Adapters;
using csharp_training_202605.Infrastructures.Context;
using csharp_training_202605.Infrastructures.Entities;
using csharp_training_202605.Infrastructures.Repositories;
using csharp_training_202605.tests.TestDoubles;

namespace csharp_training_202605.Tests.Infrastructures.Repositories;

[DoNotParallelize]
[TestClass]
public class DepartmentRepositoryTests
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=csharp_training_202605;Username=postgres;Password=training;";

    private DepartmentRepository _departmentrepository = null!;
    private AppDbContext _context = null!;

    [TestMethod]
    public void FindAll_NoDapartments_exist()
    {
        var adapter = new DepartmentEntityAdapter();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        _context = new AppDbContext(options);

        var path = Path.Combine(AppContext.BaseDirectory, "sql", "none.sql");
        var sql = File.ReadAllText(path);
        _context.Database.ExecuteSqlRaw(sql);

        _departmentrepository = new DepartmentRepository(_context, adapter);

        var departments = _departmentrepository.FindAll();
        Assert.AreEqual(0, departments.Count);
    }

    [TestMethod]
    public void FindAll_ReturnsAllDepartmentsNew()
    {
        var adapter = new DepartmentEntityAdapter();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        _context = new AppDbContext(options);

        var path = Path.Combine(AppContext.BaseDirectory, "sql", "exist.sql");
        var sql = File.ReadAllText(path);
        _context.Database.ExecuteSqlRaw(sql);

        _departmentrepository = new DepartmentRepository(_context, adapter);

        var departments = _departmentrepository.FindAll();

        Assert.AreEqual(2, departments.Count);
        AssertDepartment(departments[0], 1, "総務部");
        AssertDepartment(departments[1], 2, "経理部");
    }

    [TestMethod]
    public void FindAll_WhenDbSetThrows_WrapsExceptionInInternalException()
    {
        var adapter = new DepartmentEntityAdapter();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .Options;

        _context = new AppDbContext(options);

        var path = Path.Combine(AppContext.BaseDirectory, "sql", "exist.sql");
        var sql = File.ReadAllText(path);
        //_context.Database.ExecuteSqlRaw(sql);

        _departmentrepository = new DepartmentRepository(_context, adapter);

        var exception = Assert.ThrowsException<InternalException>(() => _departmentrepository.FindAll());

        Assert.IsInstanceOfType<InvalidOperationException>(exception.InnerException);
    }

    private static void AssertDepartment(Department department, int id, string name)
    {
        Assert.AreEqual(id, department.Id);
        Assert.AreEqual(name, department.Name);
    }

    private static AppDbContext CreateContext(DbSet<DepartmentEntity> departments)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().Options;
        return new TestAppDbContext(options)
        {
            Departments = departments,
        };
    }

}