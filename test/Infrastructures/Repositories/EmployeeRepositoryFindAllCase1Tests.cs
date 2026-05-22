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

using System;
using System.Collections.Generic;
using System.Linq; // ToList() を使うために必要

namespace csharp_training_202605.Tests.Infrastructures.Repositories;

[TestClass]
public sealed class EmployeeRepositoryTests
{
    [TestMethod]
    public void FindAll_ReturnsAllEmployees()
    {
        using var context = CreateContext(
        [
            new EmployeeEntity { EmpId = 1, EmpName = "鈴木一郎" ,Email="ichiro@test.com",Phone="090-0000-1111"},
            new EmployeeEntity { EmpId = 2, EmpName = "鈴木次郎" ,Email="ziro@test.com",Phone="090-0000-2222"},
        ]);
        var repository = CreateRepository(context);

        var employees = repository.FindAll();

        Assert.AreEqual(2, employees.Count);
        AssertEmployee(employees[0], 1, "鈴木一郎", "ichiro@test.com", "090-0000-1111");
        AssertEmployee(employees[1], 2, "鈴木次郎", "ziro@test.com", "090-0000-2222");
    }

    [TestMethod]
    public void FindAll_WhenDbSetThrows_WrapsExceptionInInternalException()
    {
        using var context = CreateContext(new ThrowingDbSet<EmployeeEntity>());
        var repository = CreateRepository(context);

        var exception = Assert.ThrowsException<InternalException>(() => repository.FindAll());

        Assert.IsInstanceOfType<InvalidOperationException>(exception.InnerException);
    }

    //例外処理を一度飛ばす（ここはテスト失敗でもOK）
    [TestMethod]
    public void Create_InternalException()
    {
        //追加する従業員定義に必要なDpartmentのDomain Object
        var department = new Department(1, "営業部");

        using var context = CreateContext(new ThrowingDbSet<EmployeeEntity>());
        var repository = CreateRepository(context);

        var addentity = new Employee("鈴木次郎", "ziro@test.com", "090-0000-2222", department);

        var exception = Assert.ThrowsException<InternalException>(() => repository.Create(addentity));

        Assert.IsInstanceOfType<InvalidOperationException>(exception.InnerException);
    }

    [TestMethod]
    public void Create_Success()
    {
        //追加する従業員定義に必要なDpartmentのDomain Object
        var department = new Department(1, "営業部");

        using var context = CreateContext(
        new[]
        {
            new EmployeeEntity
            {
                EmpId = 1,
                EmpName = "鈴木一郎",
                Email = "ichiro@test.com",
                Phone="090-0000-1111",
                DeptId = 1,
            },

            new EmployeeEntity
            {
                EmpId = 2,
                EmpName = "鈴木次郎",
                Email = "ziro@test.com",
                Phone="090-0000-2222",
                DeptId = 1,
            },
        });
        var repository = CreateRepository(context);

        //追加する従業員（Createの中でDomainObject→Entityに変換するので、ここではDomainObjectを作る）
        var addentity = new Employee("鈴木三郎", "saburou@test.com", "090-1234-5678", department);

        repository.Create(addentity);

        var employees = repository.FindAll();//DBから追加後の従業員リストを取得

        Assert.AreEqual(3, employees.Count);
        AssertEmployee(employees[0], 1, "鈴木一郎", "ichiro@test.com", "090-0000-1111");
        AssertEmployee(employees[1], 2, "鈴木次郎", "ziro@test.com", "090-0000-2222");
        AssertEmployee(employees[2], 3, "鈴木三郎", "saburou@test.com", "090-1234-5678");
    }


    private static EmployeeRepository CreateRepository(AppDbContext context)
    {
        return new EmployeeRepository(context, new EmployeeEntityAdapter());
    }

    private static AppDbContext CreateContext(IEnumerable<EmployeeEntity> entities)
    {
        return CreateContext(new QueryableDbSet<EmployeeEntity>(entities));
    }

    private static AppDbContext CreateContext(DbSet<EmployeeEntity> employees)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().Options;
        return new TestAppDbContext(options)
        {
            Employees = employees,
        };
    }

    private static void AssertEmployee(Employee employee, int id, string name, string email, string phone)
    {
        Assert.AreEqual(id, employee.Id);
        Assert.AreEqual(name, employee.Name);
        Assert.AreEqual(email, employee.Email);
        Assert.AreEqual(phone, employee.Phone);
    }

}