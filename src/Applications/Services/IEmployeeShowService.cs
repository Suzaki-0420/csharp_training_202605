using csharp_training_202605.Applications.Domains;
namespace csharp_training_202605.Applications.Services;

public interface IEmployeeShowService
{
    /// <summary>
    /// すべての社員を取得する
    /// </summary>
    /// <returns></returns>
    List<Employee> GetEmployees();
    //取得したデータをViewに渡す


}