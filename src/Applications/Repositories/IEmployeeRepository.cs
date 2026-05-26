using csharp_training_202605.Applications.Domains;
namespace csharp_training_202605.Applications.Repositories;
/// <summary>
/// ドメインオブジェクト:従業員のCRUD操作インターフェイス
/// </summary>
public interface IEmployeeRepository
{
    /// <summary>
    /// 従業員を永続化する
    /// </summary>
    /// <param name="employee">永続化対象の従業員</param>
    void Create(Employee employee);

    List<Employee> FindAll();

    Employee? FindById(int id);

    void Delete(Employee employee);
    void Renewal(Employee employee);
}

