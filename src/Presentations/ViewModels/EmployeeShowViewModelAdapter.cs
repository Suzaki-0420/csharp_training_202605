using csharp_training_202605.Applications.Adapters;
using csharp_training_202605.Applications.Domains;
namespace csharp_training_202605.Presentations.ViewModels;
/// <summary>
/// EmployeeRegisterViewModel(社員登録ViewModel)を
/// ドメインオブジェクト:Employeeに変換するアダプターインターフェイスの実装
/// </summary>
/// <typeparam name="TDomain">Employee</typeparam>
/// <typeparam name="TTarget">EmployeeRegisterForm</typeparam>
public class EmployeeShowViewModelAdapter : IConverter<Employee, EmployeeShowViewModel>
{
    /// <summary>
    /// ドメインオブジェクト:EmployeeをEmployeeRegisterViewModelに変換する
    /// </summary>
    /// <param name="target">EmployeeRegisterViewModel</param>
    /// <returns>ドメインオブジェクト:Employee</returns>

    public EmployeeShowViewModel Convert(Employee target)
    {
        var employeeshowviewmodel = new EmployeeShowViewModel(target.Id, target.Name, target.Email, target.Phone, target.Department?.Id, target.Department?.Name);
        return employeeshowviewmodel;
    }
}