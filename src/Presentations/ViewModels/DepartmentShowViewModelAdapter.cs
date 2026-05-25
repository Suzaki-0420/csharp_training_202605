using csharp_training_202605.Applications.Adapters;
using csharp_training_202605.Applications.Domains;
namespace csharp_training_202605.Presentations.ViewModels;
/// <summary>
/// DepartmentRegisterViewModel(従業員登録ViewModel)を
/// ドメインオブジェクト:Departmentに変換するアダプターインターフェイスの実装
/// </summary>
/// <typeparam name="TDomain">Department</typeparam>
/// <typeparam name="TTarget">DepartmentRegisterForm</typeparam>
public class DepartmentShowViewModelAdapter : IConverter<Department, DepartmentShowViewModel>
{
    /// <summary>
    /// ドメインオブジェクト:DepartmentをDepartmentRegisterViewModelに変換する
    /// </summary>
    /// <param name="target">DepartmentRegisterViewModel</param>
    /// <returns>ドメインオブジェクト:Department</returns>

    public DepartmentShowViewModel Convert(Department target)
    {
        var departmentshowviewmodel = new DepartmentShowViewModel(target.Id, target.Name);
        return departmentshowviewmodel;
    }
}