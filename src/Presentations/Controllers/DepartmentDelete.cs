using Microsoft.AspNetCore.Mvc;
using csharp_training_202605.Applications.Services;
using csharp_training_202605.Presentations.ViewModels;
namespace csharp_training_202605.Presentations.Controllers;
/// <summary>
/// 従業員登録コントローラ
/// </summary>
[Route("DepartmentDelete")]
public class DepartmentDeleteController : Controller
{
    /// <summary>
    /// ロガー
    /// </summary>
    private readonly ILogger<DepartmentDeleteController> _logger;
    /// <summary>
    /// 従業員登録サービスインターフェイス
    /// </summary>
    private readonly IDepartmentDeleteService _departmentDeleteService;
    /// <summary>
    /// 従業員登録ViewModelをDepartmentに変換するアダプター
    /// </summary>
    private readonly DepartmentDeleteViewModelAdapter _adapter;
    /// <summary>
    /// TempDataを通じて一時的にViewModelを保存・復元するためのクラス
    /// </summary>
    private readonly TempDataStore<DepartmentDeleteViewModel> _empDataStore;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="logger">ロガー</param>
    /// <param name="departmentDeleteService">従業員登録サービスインターフェイス</param>
    /// <param name="departmentDeleteViewModelAdapter">従業員登録ViewModelをDepartmentに変換するアダプター</param>
    /// <param name="empDataStore">TempDataを通じて一時的にViewModelを保存・復元するためのクラス</param>
    public DepartmentDeleteController(
        ILogger<DepartmentDeleteController> logger,
        IDepartmentDeleteService departmentDeleteService,
        DepartmentDeleteViewModelAdapter departmentDeleteViewModelAdapter,
        TempDataStore<DepartmentDeleteViewModel> empDataStore)
    {
        _logger = logger;
        _departmentDeleteService = departmentDeleteService;
        _adapter = departmentDeleteViewModelAdapter;
        _empDataStore = empDataStore;
    }

    /// <summary>
    /// 従業登録(入力)画面表示 アクションメソッド
    /// </summary>
    /// <returns></returns>
    [HttpGet("Enter")]
    public IActionResult Enter()
    {
        DepartmentDeleteViewModel? viewModel = null;
        // [戻る]ボタンへの対応
        // TempDataからDepartmentDeleteViewModelを取得する
        viewModel = _empDataStore.Load(this);
        if (viewModel == null)
        {
            // 従業員登録ViewModelを生成する
            viewModel = new DepartmentDeleteViewModel();
        }
        // 部署一覧を取得してViewModelに設定する(SelectListItem形式)
        PopulateDepartments(viewModel);
        // viewModelをviewに渡して画面表示する
        Console.WriteLine("中身チェック");
        Console.WriteLine(viewModel);
        return View(viewModel);
    }

    /// <summary>
    /// 入力画面の[完了]ボタンクリックアクションメソッド
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    [HttpPost("Confirm")]
    public IActionResult Confirm(DepartmentDeleteViewModel viewModel)
    {
        // バリデーションチェック
        if (!ModelState.IsValid) // バリデーションエラーあり
        {
            // 従業員一覧を取得してViewModelに設定する(SelectListItem形式)
            PopulateDepartments(viewModel);
            // 入力画面の表示
            return View("Enter", viewModel);
        }
        // 選択された従業員のIdで従業員データを取得する
        var department = _departmentDeleteService.GetById(viewModel.Id ?? 0);
        _logger.LogInformation($"部門Id:{viewModel.Id ?? 0}の部門を取得する");
        // ViewModelに部署名を設定する
        viewModel.Id = department.Id;
        viewModel.Name = department.Name;
        PopulateDepartments(viewModel);


        return View(viewModel);
    }

    /// <summary>
    /// 確認画面の[登録]ボタンクリックアクションメソッド
    /// </summary>
    /// <param name="form"></param>
    /// <returns></returns>
    [HttpPost("Delete")]
    public IActionResult Delete(DepartmentDeleteViewModel viewModel)
    {
        // EmployeeDeleteViewModelをシリアライズして、TempDataに保存する
        _empDataStore.Save(this, viewModel);

        bool judge = _departmentDeleteService.AffiliationCheck(viewModel.Id);
        if (judge == false)
        {
            TempData["msg"] = "所属従業員がいるため削除できません。";
            return View("Confirm", viewModel);
        }
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
        DepartmentDeleteViewModel? viewModel = null;
        // TempDataからDepartmentDeleteViewModelを取得する
        viewModel = _empDataStore.Load(this);
        if (viewModel == null)
        {
            // データが存在しない場合、入力画面にリダイレクト
            return RedirectToAction("Enter");
        }
        var department = _adapter.Restore(viewModel!);
        Console.WriteLine($"Completeでのdepartment：{department}");
        // 従業員を削除する
        _departmentDeleteService.Delete(department);
        return View(viewModel);
    }

    /// <summary>
    /// 確認画面の[戻る]ボタンクリックアクションメソッド
    /// </summary>
    /// <returns></returns> 
    [HttpPost("Back")]
    public IActionResult Back(DepartmentDeleteViewModel viewModel)
    {
        _logger.LogInformation("[戻る]ボタンクリック:{0}", viewModel!.ToString());
        // DepartmentDeleteViewModelをシリアライズして、TempDataに保存する
        _empDataStore.Save(this, viewModel);
        // 入力画面を出力するアクションメソッドにリダイレクトする
        return RedirectToAction("Enter");
    }

    /// <summary>
    /// 部署一覧を取得してViewModelに設定する(SelectListItem形式)
    /// </summary>
    private void PopulateDepartments(DepartmentDeleteViewModel viewModel)
    {
        // 従業員登録サービスから部署一覧を取得する
        var departments = _departmentDeleteService.GetDepartments();
        // 従業員情報をDepartmentDeleteViewModelに登録する
        viewModel.SetDepartments(departments);
        _logger.LogInformation("従業員リストを設定");
    }
}