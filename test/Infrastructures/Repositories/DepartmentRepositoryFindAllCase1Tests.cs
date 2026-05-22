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

namespace csharp_training_202605.Tests.Infrastructures.Repositories;

[TestClass]
public sealed class DepartmentRepositoryTests
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
    public void FindAll_WhenDbSetThrows_WrapsExceptionInInternalException()
    {
        using var context = CreateContext(new ThrowingDbSet<DepartmentEntity>());
        var repository = CreateRepository(context);

        var exception = Assert.ThrowsException<InternalException>(() => repository.FindAll());

        Assert.IsInstanceOfType<InvalidOperationException>(exception.InnerException);
    }

    private static DepartmentRepository CreateRepository(AppDbContext context)
    {
        return new DepartmentRepository(context, new DepartmentEntityAdapter());
    }

    private static AppDbContext CreateContext(IEnumerable<DepartmentEntity> entities)
    {
        return CreateContext(new QueryableDbSet<DepartmentEntity>(entities));
    }

    private static AppDbContext CreateContext(DbSet<DepartmentEntity> departments)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().Options;
        return new AppDbContext(options)
        {
            Departments = departments,
        };
    }

    private static void AssertDepartment(Department department, int id, string name)
    {
        Assert.AreEqual(id, department.Id);
        Assert.AreEqual(name, department.Name);
    }

    private sealed class QueryableDbSet<TEntity> : DbSet<TEntity>, IQueryable<TEntity>, IEnumerable<TEntity>
        where TEntity : class
    {
        private readonly IQueryable<TEntity> _queryable;

        public QueryableDbSet(IEnumerable<TEntity> entities)
        {
            _queryable = entities.AsQueryable();
        }

        public override IEntityType EntityType => throw new NotSupportedException();

        Type IQueryable.ElementType => _queryable.ElementType;

        Expression IQueryable.Expression => _queryable.Expression;

        IQueryProvider IQueryable.Provider => _queryable.Provider;

        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        {
            return _queryable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _queryable.GetEnumerator();
        }
    }

    private sealed class ThrowingDbSet<TEntity> : DbSet<TEntity>, IQueryable<TEntity>, IEnumerable<TEntity>
        where TEntity : class
    {
        private readonly IQueryable<TEntity> _queryable = Array.Empty<TEntity>().AsQueryable();

        public override IEntityType EntityType => throw new NotSupportedException();

        Type IQueryable.ElementType => _queryable.ElementType;

        Expression IQueryable.Expression => _queryable.Expression;

        IQueryProvider IQueryable.Provider => _queryable.Provider;

        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        {
            throw new InvalidOperationException("Database access failed.");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new InvalidOperationException("Database access failed.");
        }
    }
}