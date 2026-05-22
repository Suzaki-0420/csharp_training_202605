using Microsoft.AspNetCore.Mvc;
using csharp_training_202605.Applications.Services;
using csharp_training_202605.Presentations.ViewModels;
namespace csharp_training_202605.Presentations.Controllers;
/// <summary>
/// 従業員登録コントローラ
/// </summary>
[Route("EmployeeShow")]
public class EmployeeShowController : Controller
{
    /// <summary>
    /// ロガー
    /// </summary>
    private readonly ILogger<EmployeeShowController> _logger;
    /// <summary>
    /// 従業員登録サービスインターフェイス
    /// </summary>
    private readonly IEmployeeShowService _employeeShowService;
    /// <summary>
    /// 従業員登録ViewModelをEmployeeに変換するアダプター
    /// </summary>
    private readonly EmployeeShowViewModelAdapter _adapter;
    /// <summary>
    /// TempDataを通じて一時的にViewModelを保存・復元するためのクラス
    /// </summary>
    private readonly TempDataStore<EmployeeShowViewModel> _empDataStore;


    public EmployeeShowController(
        ILogger<EmployeeShowController> logger,
        IEmployeeShowService employeeShowService,
        EmployeeShowViewModelAdapter employeeShowViewModelAdapter,
        TempDataStore<EmployeeShowViewModel> empDataStore)
    {
        _logger = logger;
        _employeeShowService = employeeShowService;
        _adapter = employeeShowViewModelAdapter;
        _empDataStore = empDataStore;
    }

    /// <summary>
    /// 従業登録(入力)画面表示 アクションメソッド
    /// </summary>
    /// <returns></returns>
    [HttpGet("Show")]
    public IActionResult Show() //表示するviewModelをViewに渡す
    {
        var results = _employeeShowService.GetEmployees();
        var viewModels = results
            .Select(result => _adapter.Convert(result)) // 1件ずつ変換
            .ToList(); // List<EmployeeShowViewModel>
        return View(viewModels);
    }

}