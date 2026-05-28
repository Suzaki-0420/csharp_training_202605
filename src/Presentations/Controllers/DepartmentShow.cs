using Microsoft.AspNetCore.Mvc;
using csharp_training_202605.Applications.Services;
using csharp_training_202605.Presentations.ViewModels;
namespace csharp_training_202605.Presentations.Controllers;
/// <summary>
/// 社員登録コントローラ
/// </summary>
[Route("DepartmentShow")]
public class DepartmentShowController : Controller
{
    /// <summary>
    /// ロガー
    /// </summary>
    private readonly ILogger<DepartmentShowController> _logger;
    /// <summary>
    /// 社員登録サービスインターフェイス
    /// </summary>
    private readonly IDepartmentShowService _departmentShowService;
    /// <summary>
    /// 社員登録ViewModelをDepartmentに変換するアダプター
    /// </summary>
    private readonly DepartmentShowViewModelAdapter _adapter;
    /// <summary>
    /// TempDataを通じて一時的にViewModelを保存・復元するためのクラス
    /// </summary>
    private readonly TempDataStore<DepartmentShowViewModel> _empDataStore;


    public DepartmentShowController(
        ILogger<DepartmentShowController> logger,
        IDepartmentShowService departmentShowService,
        DepartmentShowViewModelAdapter departmentShowViewModelAdapter,
        TempDataStore<DepartmentShowViewModel> empDataStore)
    {
        _logger = logger;
        _departmentShowService = departmentShowService;
        _adapter = departmentShowViewModelAdapter;
        _empDataStore = empDataStore;
    }

    /// <summary>
    /// 従業登録(入力)画面表示 アクションメソッド
    /// </summary>
    /// <returns></returns>
    [HttpGet("Show")]
    public IActionResult Show() //表示するviewModelをViewに渡す
    {
        var results = _departmentShowService.GetDepartments();
        var viewModels = results
            .Select(result => _adapter.Convert(result)) // 1件ずつ変換
            .ToList(); // List<DepartmentShowViewModel>
        return View(viewModels);
    }

}