using csharp_training_202605.Applications.Adapters;
using csharp_training_202605.Applications.Domains;
namespace csharp_training_202605.Presentations.ViewModels;
/// <summary>
/// DepartmentRegisterViewModel(社員登録ViewModel)を
/// ドメインオブジェクト:Departmentに変換するアダプターインターフェイスの実装
/// </summary>
/// <typeparam name="TDomain">Department</typeparam>
/// <typeparam name="TTarget">DepartmentRegisterForm</typeparam>
public class DepartmentRegisterViewModelAdapter : IRestorer<Department, DepartmentRegisterViewModel>
{
    /// <summary>
    /// DepartmentRegisterViewModelをドメインオブジェクト:Departmentに変換する
    /// </summary>
    /// <param name="target">DepartmentRegisterViewModel</param>
    /// <returns>ドメインオブジェクト:Department</returns>
    public Department Restore(DepartmentRegisterViewModel target)
    {
        // Department(部署)を作成する
        var department = new Department(target.Name);

        return department;
    }
}