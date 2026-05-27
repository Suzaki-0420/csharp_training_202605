using csharp_training_202605.Applications.Adapters;
using csharp_training_202605.Applications.Domains;
namespace csharp_training_202605.Presentations.ViewModels;
/// <summary>
/// DepartmentUpdateViewModel(従業員登録ViewModel)を
/// ドメインオブジェクト:Departmentに変換するアダプターインターフェイスの実装
/// </summary>
/// <typeparam name="TDomain">Department</typeparam>
/// <typeparam name="TTarget">DepartmentUpdateForm</typeparam>
public class DepartmentUpdateViewModelAdapter : IRestorer<Department, DepartmentUpdateViewModel>
{
    /// <summary>
    /// DepartmentUpdateViewModelをドメインオブジェクト:Departmentに変換する
    /// </summary>
    /// <param name="target">DepartmentUpdateViewModel</param>
    /// <returns>ドメインオブジェクト:Department</returns>
    public Department Restore(DepartmentUpdateViewModel target)
    {
        // Department(部署)を作成する
        var department = new Department(target.Id, target.Name);

        return department;
    }
}