using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace csharp_training_202605.tests.TestDoubles;

internal sealed class QueryableDbSet<TEntity> : DbSet<TEntity>, IQueryable<TEntity>, IEnumerable<TEntity>
    where TEntity : class
{
    private readonly List<TEntity> _entities;

    public QueryableDbSet(IEnumerable<TEntity> entities)
    {
        _entities = entities.ToList();
    }

    public IReadOnlyList<TEntity> Entities => _entities;

    public override IEntityType EntityType => throw new NotSupportedException();

    private IQueryable<TEntity> Queryable => _entities.AsQueryable();

    Type IQueryable.ElementType => Queryable.ElementType;

    Expression IQueryable.Expression => Queryable.Expression;

    IQueryProvider IQueryable.Provider => Queryable.Provider;

    public override EntityEntry<TEntity> Add(TEntity entity)
    {
        AssignGeneratedKey(entity);
        _entities.Add(entity);
        return null!;
    }

    public override void AddRange(params TEntity[] entities)
    {
        _entities.AddRange(entities);
    }

    IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
    {
        return Queryable.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Queryable.GetEnumerator();
    }

    private void AssignGeneratedKey(TEntity entity)
    {
        var keyProperty = typeof(TEntity)
            .GetProperties()
            .FirstOrDefault(property =>
                Attribute.IsDefined(property, typeof(KeyAttribute)) &&
                property.PropertyType == typeof(int));

        if (keyProperty == null)
        {
            return;
        }

        var currentValue = (int)(keyProperty.GetValue(entity) ?? 0);
        if (currentValue != 0)
        {
            return;
        }

        var maxValue = _entities
            .Select(item => (int)(keyProperty.GetValue(item) ?? 0))
            .DefaultIfEmpty()
            .Max();

        keyProperty.SetValue(entity, maxValue + 1);
    }
}