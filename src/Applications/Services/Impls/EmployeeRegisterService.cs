using csharp_training_202605.Applications.Repositories;
using csharp_training_202605.Applications.Domains;
using csharp_training_202605.Exceptions;
using csharp_training_202605.Infrastructures.Context;
namespace csharp_training_202605.Applications.Services.Impls;
/// <summary>
/// 社員登録サービスインターフェイスの実装
/// </summary>
public class EmployeeRegisterService : IEmployeeRegisterService
{

    /// <summary>
    /// アプリケーション用DbContext
    /// </summary>
    private readonly AppDbContext _context;
    /// <summary>
    /// ドメインオブジェクト:社員のCRUD操作インターフェイス
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
    /// <param name="employeeRepository">社員のCRUD操作インターフェイス</param>
    /// <param name="departmentRepository">部署のCRUD操作インターフェイス</param>
    public EmployeeRegisterService(
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
            throw new NotFoundException($"部署Id{id}に該当する部署は存在しません");
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
    /// 新しい社員を登録する
    /// </summary>
    /// <param name="employee"></param>
    public void Register(Employee employee)
    {
        try
        {
            // トランザクションの開始
            _context.Database.BeginTransaction();
            // 社員の登録
            _employeeRepository.Create(employee);
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

    public bool EmailAffiliationCheck(string? email)
    {
        var sameemail = _context.Employees
            .Where(e => e.Email == email)
            .ToList();
        if (sameemail.Count == 0)
        {
            return true; //社員追加OK
        }
        else
        {
            return false;
        }
    }

    public bool PhoneAffiliationCheck(string? phone)
    {
        var samephone = _context.Employees
            .Where(e => e.Phone == phone)
            .ToList();
        if (samephone.Count == 0)
        {
            return true; //社員追加OK
        }
        else
        {
            return false;
        }
    }
}