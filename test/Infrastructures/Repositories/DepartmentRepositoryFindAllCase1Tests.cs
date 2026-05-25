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

[TestClass]
public sealed class DepartmentRepositoryTestsOld
{
    [TestMethod]
    public void FindAll_ReturnsAllDepartments()
    {
        using var context = CreateContext(
        [
            new DepartmentEntity { DeptId = 1, DeptName = "営業部" },
            new DepartmentEntity { DeptId = 2, DeptName = "開発部" },
        ]);
        var repository = CreateRepository(context);

        var departments = repository.FindAll();

        Assert.AreEqual(2, departments.Count);
        AssertDepartment(departments[0], 1, "営業部");
        AssertDepartment(departments[1], 2, "開発部");
    }

    [TestMethod]
    public void FindById_WhenDepartmentExists_ReturnsDepartment()
    {
        using var context = CreateContext(
        [
            new DepartmentEntity { DeptId = 1, DeptName = "営業部" },
            new DepartmentEntity { DeptId = 2, DeptName = "開発部" },
        ]);
        var repository = CreateRepository(context);

        var department = repository.FindById(2);

        Assert.IsNotNull(department);
        AssertDepartment(department, 2, "開発部");
    }

    [TestMethod]
    public void FindById_WhenDepartmentDoesNotExist_ReturnsNull()
    {
        using var context = CreateContext(
        [
            new DepartmentEntity { DeptId = 1, DeptName = "営業部" },
        ]);
        var repository = CreateRepository(context);

        var department = repository.FindById(2);

        Assert.IsNull(department);
    }

    [TestMethod]
    public void FindById_WhenAdapterIsBroken_ReturnException()
    {
        using var context = CreateContext(
        [
            new DepartmentEntity { DeptId = 1, DeptName = "営業部" },
        ]);
        var repository = CreateRepositoryForInternalException(context);


        var exception = Assert.ThrowsException<InternalException>(() => repository.FindById(1));

        Assert.IsInstanceOfType<InvalidOperationException>(exception.InnerException);
    }


    [TestMethod]
    public void FindAll_WhenDbSetThrows_WrapsExceptionInInternalException()
    {
        using var context = CreateContext(new ThrowingDbSet<DepartmentEntity>());
        var repository = CreateRepository(context);

        var exception = Assert.ThrowsException<InternalException>(() => repository.FindAll());

        Assert.IsInstanceOfType<InvalidOperationException>(exception.InnerException);
    }

    [TestMethod]
    public void Create_InternalException()
    {
        //追加する従業員定義に必要なDpartmentのDomain Object
        var department = new Department(1, "営業部");

        using var context = CreateContext(new ThrowingDbSet<DepartmentEntity>());
        var repository = CreateRepository(context);

        var addentity = new Department("営業部");

        var exception = Assert.ThrowsException<InternalException>(() => repository.Create(addentity));

        Assert.IsInstanceOfType<InvalidOperationException>(exception.InnerException);
    }

    [TestMethod]
    public void Create_Success()
    {

        using var context = CreateContext(
        new[]
        {
            new DepartmentEntity
            {
                DeptId=1,
                DeptName="営業部"
            },

            new DepartmentEntity
            {
                DeptId=2,
                DeptName="人事部"
            },
        });
        var repository = CreateRepository(context);

        //追加する従業員（Createの中でDomainObject→Entityに変換するので、ここではDomainObjectを作る）
        var addentity = new Department("開発部");

        repository.Create(addentity);

        var departments = repository.FindAll();//DBから追加後の従業員リストを取得

        Assert.AreEqual(3, departments.Count);
        AssertDepartment(departments[0], 1, "営業部");
        AssertDepartment(departments[1], 2, "人事部");
        AssertDepartment(departments[2], 3, "開発部");
    }

    private static DepartmentRepository CreateRepository(AppDbContext context)
    {
        return new DepartmentRepository(context, new DepartmentEntityAdapter());
    }

    private static DepartmentRepository CreateRepositoryForInternalException(AppDbContext context)
    {
        return new DepartmentRepository(context, null!);
    }

    private static AppDbContext CreateContext(IEnumerable<DepartmentEntity> entities)
    {
        return CreateContext(new QueryableDbSet<DepartmentEntity>(entities));
    }

    private static AppDbContext CreateContext(DbSet<DepartmentEntity> departments)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().Options;
        return new TestAppDbContext(options)
        {
            Departments = departments,
        };
    }

    private static void AssertDepartment(Department department, int id, string name)
    {
        Assert.AreEqual(id, department.Id);
        Assert.AreEqual(name, department.Name);
    }

}