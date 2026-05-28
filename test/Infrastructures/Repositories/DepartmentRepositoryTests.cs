using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using csharp_training_202605.Applications.Domains;
using csharp_training_202605.Infrastructures.Adapters;
using csharp_training_202605.Infrastructures.Context;
using csharp_training_202605.Infrastructures.Repositories;
using csharp_training_202605.Exceptions;

namespace csharp_training_202605.Test.Infrastructures.Repositories;

[DoNotParallelize]
[TestClass]
public class DepartmentRightExistTest
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=csharp_training_202605;Username=postgres;Password=training;";

    private DepartmentRepository _departmentrepository = null!;
    private AppDbContext _context = null!;

    [TestInitialize]
    public void Setup()
    {
        var departmentAdapter = new DepartmentEntityAdapter();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        _context = new AppDbContext(options);

        var path = Path.Combine(AppContext.BaseDirectory, "sql", "exist.sql");
        var sql = File.ReadAllText(path);
        _context.Database.ExecuteSqlRaw(sql);

        _departmentrepository = new DepartmentRepository(_context, departmentAdapter);
    }

    private static void AssertDepartment(Department department, int id, string name)
    {
        Assert.AreEqual(id, department.Id);
        Assert.AreEqual(name, department.Name);
    }

    [TestMethod]
    public void Department_FindAll_ReturnsAllDepartments()
    {
        var departments = _departmentrepository.FindAll();
        Assert.AreEqual(3, departments.Count);
        AssertDepartment(departments[0], 1, "総務部");
        AssertDepartment(departments[1], 2, "経理部");
        AssertDepartment(departments[2], 3, "開発部");
    }

    [TestMethod]
    public void Department_FindById_ReturnsDepartment()
    {
        var department = _departmentrepository.FindById(1);
        AssertDepartment(department!, 1, "総務部");
    }

    [TestMethod]
    public void Department_FindById_ReturnsNull()
    {
        var department = _departmentrepository.FindById(5);
        Assert.IsNull(department);
    }

    [TestMethod]
    public void Department_Create_Added()
    {
        var addentity = new Department("営業部"); //追加するエンティティ
        _departmentrepository.Create(addentity);

        var departments = _departmentrepository.FindAll();//DBから追加後の社員リストを取得

        Assert.AreEqual(4, departments.Count);
        AssertDepartment(departments[0], 1, "総務部");
        AssertDepartment(departments[1], 2, "経理部");
        AssertDepartment(departments[2], 3, "開発部");
        AssertDepartment(departments[3], 4, "営業部");
    }

    [TestMethod]
    public void Department_Delete_Deleted()
    {
        var deleteentity = new Department(3, "開発部"); //削除するエンティティ
        _departmentrepository.Delete(deleteentity);

        var departments = _departmentrepository.FindAll();//DBから追加後の社員リストを取得

        Assert.AreEqual(2, departments.Count);
        AssertDepartment(departments[0], 1, "総務部");
        AssertDepartment(departments[1], 2, "経理部");
    }

    [TestMethod]
    public void Department_Renewal_Success()
    {
        var deleteentity = new Department(3, "営業部"); //更新するエンティティ
        _departmentrepository.Renewal(deleteentity);

        var departments = _departmentrepository.FindAll();//DBから追加後の社員リストを取得

        Assert.AreEqual(3, departments.Count);
        AssertDepartment(departments[0], 1, "総務部");
        AssertDepartment(departments[1], 2, "経理部");
        AssertDepartment(departments[2], 3, "営業部");
    }
}

[DoNotParallelize]
[TestClass]
public class DepartmentExceptionsTest
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=csharp_training_202605;Username=postgres;Password=training;";

    private DepartmentRepository _departmentrepository = null!;
    private AppDbContext _context = null!;

    [TestInitialize]
    public void Setup()
    {
        var departmentAdapter = new DepartmentEntityAdapter();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .Options;

        _context = new AppDbContext(options);

        _departmentrepository = new DepartmentRepository(_context, departmentAdapter);
    }

    private static void AssertDepartment(Department department, int id, string name)
    {
        Assert.AreEqual(id, department.Id);
        Assert.AreEqual(name, department.Name);
    }

    [TestMethod]
    public void Department_FindAll_Exception()
    {
        var exception = Assert.ThrowsException<InternalException>(() => _departmentrepository.FindAll());

        Assert.IsInstanceOfType<InvalidOperationException>(exception.InnerException);
    }

    [TestMethod]
    public void Department_FindById_Exception()
    {
        var exception = Assert.ThrowsException<InternalException>(() => _departmentrepository.FindById(1));

        Assert.IsInstanceOfType<InvalidOperationException>(exception.InnerException);
    }

    [TestMethod]
    public void Department_Create_Exception()
    {
        var addentity = new Department("開発部");
        var exception = Assert.ThrowsException<InternalException>(() => _departmentrepository.Create(addentity));

        Assert.IsInstanceOfType<InvalidOperationException>(exception.InnerException);
    }

    [TestMethod]
    public void Department_Delete_Exception()
    {
        var deleteentity = new Department(1, "総務部");
        var exception = Assert.ThrowsException<InternalException>(() => _departmentrepository.Delete(deleteentity));

        Assert.IsInstanceOfType<InvalidOperationException>(exception.InnerException);
    }

    [TestMethod]
    public void Department_Renewal_Exception()
    {
        var deleteentity = new Department(3, "営業部");
        var exception = Assert.ThrowsException<InternalException>(() => _departmentrepository.Renewal(deleteentity));

        Assert.IsInstanceOfType<InvalidOperationException>(exception.InnerException);
    }
}

[DoNotParallelize]
[TestClass]
public class DepartmentRightNoneTest
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=csharp_training_202605;Username=postgres;Password=training;";

    private DepartmentRepository _departmentrepository = null!;
    private AppDbContext _context = null!;

    [TestInitialize]
    public void Setup()
    {
        var departmentAdapter = new DepartmentEntityAdapter();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        _context = new AppDbContext(options);

        var path = Path.Combine(AppContext.BaseDirectory, "sql", "none.sql");
        var sql = File.ReadAllText(path);
        _context.Database.ExecuteSqlRaw(sql);

        _departmentrepository = new DepartmentRepository(_context, departmentAdapter);
    }

    [TestMethod]
    public void Department_FindAll_ReturnsZeroDepartment()
    {
        var departments = _departmentrepository.FindAll();
        Assert.AreEqual(0, departments.Count);
    }
}