using csharp_training_202605.Applications.Adapters;
using csharp_training_202605.Applications.Domains;
namespace csharp_training_202605.Presentations.ViewModels;
/// <summary>
/// EmployeeRegisterViewModel(従業員登録ViewModel)を
/// ドメインオブジェクト:Employeeに変換するアダプターインターフェイスの実装
/// </summary>
/// <typeparam name="TDomain">Employee</typeparam>
/// <typeparam name="TTarget">EmployeeRegisterForm</typeparam>
public class EmployeeRegisterViewModelAdapter : IRestorer<Employee, EmployeeRegisterViewModel>
{
    /// <summary>
    /// EmployeeRegisterViewModelをドメインオブジェクト:Employeeに変換する
    /// </summary>
    /// <param name="target">EmployeeRegisterViewModel</param>
    /// <returns>ドメインオブジェクト:Employee</returns>
    public Employee Restore(EmployeeRegisterViewModel target)
    {
        // Department(部署)を作成する
        var department = new Department(target.DeptId!.Value, target.DeptName);
        // 登録するEmployee(従業員)を作成する
        var employee = new Employee(target.Name!, target.Email!, target.Phone!, department);
        return employee;
    }
}