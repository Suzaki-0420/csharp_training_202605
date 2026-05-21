using csharp_training_202605.Applications.Adapters;
using csharp_training_202605.Applications.Domains;
using csharp_training_202605.Infrastructures.Entities;
namespace csharp_training_202605.Infrastructures.Adapters;
/// <summary>
/// ドメインオブジェクト:DepartmentとDepartmentEntityの相互変換インターフェイスの実装
/// </summary>
/// <typeparam name="TDomain">Department</typeparam>
/// <typeparam name="TTarget">DepartmentEntity</typeparam>
public class DepartmentEntityAdapter :
IConverter<Department, DepartmentEntity>, IRestorer<Department, DepartmentEntity>
{
    // <summary>
    /// ドメインオブジェクト:DepartmentをDepartmentEntityに変換する
    /// </summary>
    /// <param name="domain">ドメインオブジェクト:Department</param>
    /// <returns>DepartmentEntity</returns>
    public DepartmentEntity Convert(Department domain)
    {
        var entity = new DepartmentEntity
        {
            DeptName = domain.Name!,
        };
        if (domain.Id != null)
        {
            // int?（nullable int）から int への暗黙的な変換ができない
            // 明示的にValueを使う
            entity.DeptId = domain.Id.Value;
        }
        return entity;
    }

    /// <summary>
    /// DepartmentEntityからドメインオブジェクト:Departmentを復元する
    /// </summary>
    /// <param name="entity">DepartmentEntity</param>
    /// <returns>ドメインオブジェクト:Department</returns>
    public Department Restore(DepartmentEntity target)
    {
        var department = new Department(target.DeptId, target.DeptName!);
        return department;
    }
}