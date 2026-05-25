using Microsoft.EntityFrameworkCore;
using csharp_training_202605.Infrastructures.Context;
using csharp_training_202605.Applications.Domains;
using csharp_training_202605.Applications.Repositories;
using csharp_training_202605.Infrastructures.Adapters;
using csharp_training_202605.Exceptions;
namespace csharp_training_202605.Infrastructures.Repositories;
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
                .Include(e => e.Department) //いったん部署名を入れずにIDで出す
                .ToList();

            var employees = new List<Employee>();

            foreach (var entity in entities)
            {
                Console.WriteLine(entity);
                var employee = _adapter.Restore(entity);
                //Console.WriteLine(employee);
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

    public Employee? FindById(int id)
    {
        try
        {
            var result = _context.Employees
                .Include(e => e.Department)
                .FirstOrDefault(i => i.EmpId == id);//ここでたくさんのEntityの集まりから1つにする

            if (result == null)
            {
                return null;
            }
            return _adapter.Restore(result);
        }
        catch (Exception e)
        {
            throw new InternalException(
                "指定された社員を取得できませんでした。", e);
        }
    }

    //Deleteを追加
    public void Delete(Employee employee)
    {
        Console.WriteLine($"リポジトリでのemployee：{employee}");
        try
        {
            var entity = _context.Employees
                .FirstOrDefault(e => e.EmpId == employee.Id!.Value);
            _context.Employees.Remove(entity!);
            _context.SaveChanges();
        }
        catch (Exception e)
        {
            throw new InternalException(
                "従業員の削除ができませんでした。", e);
        }
    }
}