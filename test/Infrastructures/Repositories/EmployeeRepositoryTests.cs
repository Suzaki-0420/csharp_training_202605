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
public class EmployeeRightExistTest
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=csharp_training_202605;Username=postgres;Password=training;";

    private EmployeeRepository _employeerepository = null!;
    private AppDbContext _context = null!;

    [TestInitialize]
    public void Setup()
    {
        var employeeAdapter = new EmployeeEntityAdapter();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        _context = new AppDbContext(options);

        var path = Path.Combine(AppContext.BaseDirectory, "sql", "exist.sql");
        var sql = File.ReadAllText(path);
        _context.Database.ExecuteSqlRaw(sql);

        _employeerepository = new EmployeeRepository(_context, employeeAdapter);
    }

    private static void AssertEmployee(Employee employee, int id, string name, string email, string phone)
    {
        Assert.AreEqual(id, employee.Id);
        Assert.AreEqual(name, employee.Name);
        Assert.AreEqual(email, employee.Email);
        Assert.AreEqual(phone, employee.Phone);
    }

    [TestMethod]
    public void Employee_FindAll_ReturnsAllEmployees()
    {
        var employees = _employeerepository.FindAll();
        Assert.AreEqual(2, employees.Count);
        AssertEmployee(employees[0], 1, "田中太郎", "tanakatarou@csharp.com", "090-0000-0001");
        AssertEmployee(employees[1], 2, "鈴木三郎", "suzukisaburou@csharp.com", "090-0000-0002");

    }

    [TestMethod]
    public void Employee_FindById_ReturnsEmployee()
    {
        var employee = _employeerepository.FindById(1);
        AssertEmployee(employee!, 1, "田中太郎", "tanakatarou@csharp.com", "090-0000-0001");
    }

    [TestMethod]
    public void Employee_FindById_ReturnsNull()
    {
        var employee = _employeerepository.FindById(5);
        Assert.IsNull(employee);
    }

    [TestMethod]
    public void Employee_Create_Added()
    {
        var departmententity = new Department(1, "営業部");
        var addentity = new Employee("高橋達郎", "takahasi@csharp.com", "090-0000-0003", departmententity); //追加するエンティティ
        _employeerepository.Create(addentity);

        var employees = _employeerepository.FindAll();//DBから追加後の従業員リストを取得

        Assert.AreEqual(3, employees.Count);
        AssertEmployee(employees[0], 1, "田中太郎", "tanakatarou@csharp.com", "090-0000-0001");
        AssertEmployee(employees[1], 2, "鈴木三郎", "suzukisaburou@csharp.com", "090-0000-0002");
        AssertEmployee(employees[2], 3, "高橋達郎", "takahasi@csharp.com", "090-0000-0003");
    }

    [TestMethod]
    public void Employee_Delete_Deleted()
    {
        var departmententity = new Department(2, "経理部");
        var deleteentity = new Employee(1, "田中太郎", "tanakatarou@csharp.com", "090-0000-0001", departmententity); //追加するエンティティ
        _employeerepository.Delete(deleteentity);

        var employees = _employeerepository.FindAll();//DBから削除後の従業員リストを取得

        Assert.AreEqual(1, employees.Count);
        AssertEmployee(employees[0], 2, "鈴木三郎", "suzukisaburou@csharp.com", "090-0000-0002");
    }
}

[DoNotParallelize]
[TestClass]
public class EmployeeExceptionsTest
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=csharp_training_202605;Username=postgres;Password=training;";

    private EmployeeRepository _employeerepository = null!;
    private AppDbContext _context = null!;

    [TestInitialize]
    public void Setup()
    {
        var employeeAdapter = new EmployeeEntityAdapter();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .Options;

        _context = new AppDbContext(options);

        _employeerepository = new EmployeeRepository(_context, employeeAdapter);
    }

    private static void AssertEmployee(Employee employee, int id, string name)
    {
        Assert.AreEqual(id, employee.Id);
        Assert.AreEqual(name, employee.Name);
    }

    [TestMethod]
    public void Employee_FindAll_Exception()
    {
        var exception = Assert.ThrowsException<InternalException>(() => _employeerepository.FindAll());

        Assert.IsInstanceOfType<InvalidOperationException>(exception.InnerException);
    }

    [TestMethod]
    public void Employee_FindById_Exception()
    {
        var exception = Assert.ThrowsException<InternalException>(() => _employeerepository.FindById(1));

        Assert.IsInstanceOfType<InvalidOperationException>(exception.InnerException);
    }

    [TestMethod]
    public void Employee_Create_Exception()
    {

        var departmententity = new Department(1, "営業部");
        var addentity = new Employee("高橋達郎", "takahasi@csharp.com", "090-0000-0003", departmententity); //追加するエンティティ
        var exception = Assert.ThrowsException<InternalException>(() => _employeerepository.Create(addentity));

        Assert.IsInstanceOfType<InvalidOperationException>(exception.InnerException);
    }

    [TestMethod]
    public void Employee_Delete_Exception()
    {
        var departmententity = new Department(2, "経理部");
        var deleteentity = new Employee(1, "田中太郎", "tanakatarou@csharp.com", "090-0000-0001", departmententity); //追加するエンティティ
        var exception = Assert.ThrowsException<InternalException>(() => _employeerepository.Delete(deleteentity));

        Assert.IsInstanceOfType<InvalidOperationException>(exception.InnerException);
    }
}

[DoNotParallelize]
[TestClass]
public class EmployeeRightNoneTest
{
    private const string ConnectionString =
        "Host=localhost;Port=5432;Database=csharp_training_202605;Username=postgres;Password=training;";

    private EmployeeRepository _employeerepository = null!;
    private AppDbContext _context = null!;

    [TestInitialize]
    public void Setup()
    {
        var employeeAdapter = new EmployeeEntityAdapter();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        _context = new AppDbContext(options);

        var path = Path.Combine(AppContext.BaseDirectory, "sql", "none.sql");
        var sql = File.ReadAllText(path);
        _context.Database.ExecuteSqlRaw(sql);

        _employeerepository = new EmployeeRepository(_context, employeeAdapter);
    }

    [TestMethod]
    public void Employee_FindAll_ReturnsZeroEmployee()
    {
        var employees = _employeerepository.FindAll();
        Assert.AreEqual(0, employees.Count);
    }
}