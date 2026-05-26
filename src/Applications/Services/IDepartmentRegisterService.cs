using csharp_training_202605.Applications.Domains;
namespace csharp_training_202605.Applications.Services;
/// <summary>
/// 従業員登録サービスインターフェイス
/// </summary>
public interface IDepartmentRegisterService
{
    /// <summary>
    /// すべての部署を取得する
    /// </summary>
    /// <returns></returns>
    List<Department> GetDepartments();

    /// <summary>
    /// 指定された部署Idの部署を取得する
    /// </summary>
    /// <param name="id">部署Id</param>
    /// <returns></returns>
    Department GetById(int id);

    /// <summary>
    /// 新しい部署を登録する
    /// </summary>
    /// <param name="department"></param>
    void Register(Department department);

    bool AffiliationCheck(string? deptname);

}