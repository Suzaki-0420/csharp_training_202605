using csharp_training_202605.Applications.Domains;
namespace csharp_training_202605.Applications.Services;
/// <summary>
/// 社員登録サービスインターフェイス
/// </summary>
public interface IEmployeeUpdateService
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
    /// 新しい社員を登録する
    /// </summary>
    /// <param name="employee"></param>
    void Update(Employee employee);
    bool EmailAffiliationCheck(string? email, int? id);
    bool PhoneAffiliationCheck(string? phone, int? id);

}