using csharp_training_202605.Applications.Repositories;
using csharp_training_202605.Applications.Domains;
using csharp_training_202605.Exceptions;
using csharp_training_202605.Infrastructures.Context;
namespace csharp_training_202605.Applications.Services.Impls;
/// <summary>
/// 従業員登録サービスインターフェイスの実装
/// </summary>
public class DepartmentDeleteService : IDepartmentDeleteService
{

    /// <summary>
    /// アプリケーション用DbContext
    /// </summary>
    private readonly AppDbContext _context;
    /// <summary>
    /// ドメインオブジェクト:従業員のCRUD操作インターフェイス
    /// </summary>
    private readonly IEmployeeRepository _employeeRepository;
    /// <summary>
    /// ドメインオブジェクト:部署のCRUD操作インターフェイス
    /// </summary>
    private readonly IDepartmentRepository _departmentRepository;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="context">アプリケーション用DbContext</param>
    /// <param name="employeeRepository">従業員のCRUD操作インターフェイス</param>
    /// <param name="departmentRepository">部署のCRUD操作インターフェイス</param>
    public DepartmentDeleteService(
        AppDbContext context,
        IEmployeeRepository employeeRepository,
        IDepartmentRepository departmentRepository)
    {
        _context = context;
        _employeeRepository = employeeRepository;
        _departmentRepository = departmentRepository;
    }

    /// <summary>
    /// 指定された部署Idの部署を取得する
    /// </summary>
    /// <param name="id">部署Id</param>
    /// <returns></returns>
    public Department GetById(int id)
    {
        var result = _departmentRepository.FindById(id)!;
        if (result == null)
        {
            throw new NotFoundException($"部門d{id}に該当する部門は存在しません");
        }
        return result;
    }

    /// <summary>
    /// すべての部署を取得する
    /// </summary>
    /// <returns></returns>
    public List<Department> GetDepartments()
    {
        return _departmentRepository.FindAll();
    }

    /// <summary>
    /// 既存の従業員を削除する
    /// </summary>
    /// <param name="employee"></param>
    public void Delete(Department department)
    {
        try
        {
            // トランザクションの開始
            _context.Database.BeginTransaction();
            // 従業員の登録
            _departmentRepository.Delete(department);
            // トランザクションのコミット
            _context.Database.CommitTransaction();
        }
        catch
        {
            // トランザクションのロールバック
            _context.Database.RollbackTransaction();
            throw;
        }
    }

    public bool AffiliationCheck(int deptid)
    {
        var employeesindepartment = _context.Employees
            .Where(e => e.DeptId == deptid)
            .ToList();
        if (employeesindepartment.Count == 0)
        {
            return true; //部署削除OK
        }
        else
        {
            return false;
        }
    }
}