using Microsoft.AspNetCore.Mvc;
using csharp_training_202605.Applications.Services;
using csharp_training_202605.Presentations.ViewModels;
namespace csharp_training_202605.Presentations.Controllers;
/// <summary>
/// 従業員登録コントローラ
/// </summary>
[Route("DepartmentUpdate")]
public class DepartmentUpdateController : Controller
{
    /// <summary>
    /// ロガー
    /// </summary>
    private readonly ILogger<DepartmentUpdateController> _logger;
    /// <summary>
    /// 従業員登録サービスインターフェイス
    /// </summary>
    private readonly IDepartmentUpdateService _DepartmentUpdateService;
    /// <summary>
    /// 従業員登録ViewModelをDepartmentに変換するアダプター
    /// </summary>
    private readonly DepartmentUpdateViewModelAdapter _adapter;
    /// <summary>
    /// TempDataを通じて一時的にViewModelを保存・復元するためのクラス
    /// </summary>
    private readonly TempDataStore<DepartmentUpdateViewModel> _empDataStore;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="logger">ロガー</param>
    /// <param name="DepartmentUpdateService">従業員登録サービスインターフェイス</param>
    /// <param name="DepartmentUpdateViewModelAdapter">従業員登録ViewModelをDepartmentに変換するアダプター</param>
    /// <param name="empDataStore">TempDataを通じて一時的にViewModelを保存・復元するためのクラス</param>
    public DepartmentUpdateController(
        ILogger<DepartmentUpdateController> logger,
        IDepartmentUpdateService DepartmentUpdateService,
        DepartmentUpdateViewModelAdapter DepartmentUpdateViewModelAdapter,
        TempDataStore<DepartmentUpdateViewModel> empDataStore)
    {
        _logger = logger;
        _DepartmentUpdateService = DepartmentUpdateService;
        _adapter = DepartmentUpdateViewModelAdapter;
        _empDataStore = empDataStore;
    }

    /// <summary>
    /// 従業登録(入力)画面表示 アクションメソッド
    /// </summary>
    /// <returns></returns>
    [HttpPost("Enter")]
    public IActionResult Enter(DepartmentUpdateViewModel viewModel)
    {
        // 部署一覧を取得してViewModelに設定する(SelectListItem形式)
        // viewModelをviewに渡して画面表示する
        return View(viewModel);
    }

    /// <summary>
    /// 入力画面の[完了]ボタンクリックアクションメソッド
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    [HttpPost("Confirm")]
    public IActionResult Confirm(DepartmentUpdateViewModel viewModel)
    {
        Console.WriteLine($"該当従業員最初チェックId：{viewModel.Id}");
        // バリデーションチェック
        if (!ModelState.IsValid) // バリデーションエラーあり
        {
            // 部署一覧を取得してViewModelに設定する(SelectListItem形式)
            // 入力画面の表示
            return View("Enter", viewModel);
        }
        // 選択された部署のIdで部署データを取得する
        //var Department = _DepartmentUpdateService.GetById(viewModel.Id ?? 0);
        _logger.LogInformation($"部署Id:{viewModel.Id ?? 0}の部署を取得する");

        bool deptnamejudge = _DepartmentUpdateService.DeptNameAffiliationCheck(viewModel.Name, viewModel.Id);

        if (deptnamejudge == true)
        {
            TempData["msg"] = "同じ名前の部署がすでに存在しています。";
        }

        // 確認画面を表示する
        return View(viewModel);
    }

    /// <summary>
    /// 確認画面の[登録]ボタンクリックアクションメソッド
    /// </summary>
    /// <param name="form"></param>
    /// <returns></returns>
    [HttpPost("Update")]
    public IActionResult Update(DepartmentUpdateViewModel viewModel)
    {
        // DepartmentUpdateViewModelをシリアライズして、TempDataに保存する
        _empDataStore.Save(this, viewModel);
        // 登録処理GETアクションメソッドにリダイレクトする
        return RedirectToAction("Complete");
    }

    /// <summary>
    /// アクションメソッド:Regiter()のリダイレクト先
    /// PRGパターン
    /// </summary>
    /// <returns></returns>
    [HttpGet("Complete")]
    public IActionResult Complete()
    {
        DepartmentUpdateViewModel? viewModel = null;
        // TempDataからDepartmentUpdateViewModelを取得する
        viewModel = _empDataStore.Load(this);
        if (viewModel == null)
        {
            // データが存在しない場合、入力画面にリダイレクト
            return RedirectToAction("Enter");
        }
        // DepartmentUpdateFormをドメインモデル:Departmentに変換する
        var Department = _adapter.Restore(viewModel!);
        // 新しい従業員を登録する
        _DepartmentUpdateService.Update(Department);
        return View(viewModel);
    }

    /// <summary>
    /// 確認画面の[戻る]ボタンクリックアクションメソッド
    /// </summary>
    /// <returns></returns> 
    [HttpPost("Back")]
    public IActionResult Back(DepartmentUpdateViewModel viewModel)
    {
        _logger.LogInformation("[戻る]ボタンクリック:{0}", viewModel!.ToString());
        // DepartmentUpdateViewModelをシリアライズして、TempDataに保存する
        _empDataStore.Save(this, viewModel);
        // 入力画面を出力するアクションメソッドにリダイレクトする
        return RedirectToAction("Enter");
    }

}