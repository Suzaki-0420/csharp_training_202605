using csharp_training_202605.Applications.Adapters;
using csharp_training_202605.Applications.Domains;
using csharp_training_202605.Infrastructures.Entities;
namespace csharp_training_202605.Infrastructures.Adapters;
/// <summary>
/// ドメインオブジェクト:EmployeeとEmployeeEntityの相互変換インターフェイスの実装
/// </summary>
/// <typeparam name="TDomain">Employee</typeparam>
/// <typeparam name="TTarget">EmployeeEntity</typeparam>
public class EmployeeEntityAdapter :
IConverter<Employee, EmployeeEntity>, IRestorer<Employee, EmployeeEntity>
{
    /// <summary>
    /// ドメインオブジェクト:EmployeeをEmployeeEntityに変換する
    /// </summary>
    /// <param name="domain">ドメインモデル:従業員</param>
    /// <returns>EmployeeEntity</returns>
    public EmployeeEntity Convert(Employee domain)
    {
        var entity = new EmployeeEntity
        {
            EmpName = domain.Name,
            Email = domain.Email,
            Phone = domain.Phone
        };
        if (domain.Id != null)
        {
            entity.EmpId = domain.Id.Value;
        }
        if (domain.Department != null)
        {
            entity.DeptId = domain.Department.Id;
        }
        return entity;
    }

    /// <summary>
    /// EmployeeEntityからドメインオブジェクト:Employeeを復元する
    /// </summary>
    /// <param name="target">EmployeeEntity</param>
    /// <returns>ドメインオブジェクト:Employee</returns>
    public Employee Restore(EmployeeEntity target)
    {
        Department? department = new Department(target.Department?.DeptId, target.Department?.DeptName);//Domeinの形のDepartmentを作る
        var employee = new Employee(
            target.EmpId,
            target.EmpName,
            target.Email,
            target.Phone,
            department
        );
        return employee;
    }
}