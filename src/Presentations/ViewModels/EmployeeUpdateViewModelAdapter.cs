using csharp_training_202605.Applications.Adapters;
using csharp_training_202605.Applications.Domains;
namespace csharp_training_202605.Presentations.ViewModels;
/// <summary>
/// EmployeeUpdateViewModel(従業員登録ViewModel)を
/// ドメインオブジェクト:Employeeに変換するアダプターインターフェイスの実装
/// </summary>
/// <typeparam name="TDomain">Employee</typeparam>
/// <typeparam name="TTarget">EmployeeUpdateForm</typeparam>
public class EmployeeUpdateViewModelAdapter : IRestorer<Employee, EmployeeUpdateViewModel>
{
    /// <summary>
    /// EmployeeUpdateViewModelをドメインオブジェクト:Employeeに変換する
    /// </summary>
    /// <param name="target">EmployeeUpdateViewModel</param>
    /// <returns>ドメインオブジェクト:Employee</returns>
    public Employee Restore(EmployeeUpdateViewModel target)
    {
        // Department(部署)を作成する
        var department = new Department(target.DeptId!.Value, target.DeptName);
        // 登録するEmployee(従業員)を作成する
        var employee = new Employee(target.Id!, target.Name!, target.Email!, target.Phone!, department);
        return employee;
    }
}