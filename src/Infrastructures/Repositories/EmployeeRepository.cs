using Microsoft.EntityFrameworkCore;
using WebApp_Sample.Infrastructures.Context;
using WebApp_Sample.Applications.Domains;
using WebApp_Sample.Applications.Repositories;
using WebApp_Sample.Infrastructures.Adapters;
using WebApp_Sample.Exceptions;
namespace WebApp_Sample.Infrastructures.Repositories;
/// <summary>
/// ドメインオブジェクト:従業員のCRUD操作インターフェイスの実装
/// </summary>
public class EmployeeRepository : IEmployeeRepository
{
    /// <summary>
    /// アプリケーション用DbContext
    /// </summary>
    private readonly AppDbContext _context;
    /// <summary>
    /// ドメインモデル:従業員と従業員エンティティの相互変換インターフェイスの実装
    /// </summary>
    private readonly EmployeeEntityAdapter _adapter;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="context"></param>
    /// <param name="adapter"></param>
    public EmployeeRepository(AppDbContext context, EmployeeEntityAdapter adapter)
    {
        _context = context;
        _adapter = adapter;
    }

    /// <summary>
    /// 従業員を永続化する
    /// </summary>
    /// <param name="employee">永続化対象の従業員</param>
    public void Create(Employee employee)
    {
        try
        {
            var entity = _adapter.Convert(employee);
            _context.Employees.Add(entity);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            throw new InternalException(
                "従業員の永続化ができませんでした。", e);
        }
    }

    public List<Employee> FindAll()
    {
        try
        {
            var entities = _context.Employees
                //.Include(e => e.Department) いったん部署名を入れずにIDで出す
                .ToList();

            var employees = new List<Employee>();

            foreach (var entity in entities)
            {
                var employee = _adapter.Restore(entity);
                employees.Add(employee);
            }

            return employees;
        }
        catch (Exception e)
        {
            throw new InternalException(
                "従業員一覧を取得できませんでした。", e);
        }
    }
}