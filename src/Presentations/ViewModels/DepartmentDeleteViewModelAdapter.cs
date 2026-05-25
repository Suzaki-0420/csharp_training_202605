using csharp_training_202605.Applications.Adapters;
using csharp_training_202605.Applications.Domains;
namespace csharp_training_202605.Presentations.ViewModels;
/// <summary>
/// DepartmentDeleteViewModel(従業員登録ViewModel)を
/// ドメインオブジェクト:Departmentに変換するアダプターインターフェイスの実装
/// </summary>
/// <typeparam name="TDomain">Department</typeparam>
/// <typeparam name="TTarget">DepartmentDeleteForm</typeparam>
public class DepartmentDeleteViewModelAdapter : IRestorer<Department, DepartmentDeleteViewModel>
{
    /// <summary>
    /// DepartmentDeleteViewModelをドメインオブジェクト:Departmentに変換するa
    /// </summary>
    /// <param name="target">DepartmentDeleteViewModel</param>
    /// <returns>ドメインオブジェクト:Department</returns>
    public Department Restore(DepartmentDeleteViewModel target)
    {
        // Department(部署)を作成する
        var department = new Department(target.Id!.Value, target.Name);

        return department;
    }
}