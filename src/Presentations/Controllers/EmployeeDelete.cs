using Microsoft.AspNetCore.Mvc;
using csharp_training_202605.Applications.Services;
using csharp_training_202605.Presentations.ViewModels;
namespace csharp_training_202605.Presentations.Controllers;
/// <summary>
/// 従業員登録コントローラ
/// </summary>
[Route("EmployeeDelete")]
public class EmployeeDeleteController : Controller
{
    /// <summary>
    /// ロガー
    /// </summary>
    private readonly ILogger<EmployeeDeleteController> _logger;
    /// <summary>
    /// 従業員登録サービスインターフェイス
    /// </summary>
    private readonly IEmployeeDeleteService _employeeDeleteService;
    /// <summary>
    /// 従業員登録ViewModelをEmployeeに変換するアダプター
    /// </summary>
    private readonly EmployeeDeleteViewModelAdapter _adapter;
    /// <summary>
    /// TempDataを通じて一時的にViewModelを保存・復元するためのクラス
    /// </summary>
    private readonly TempDataStore<EmployeeDeleteViewModel> _empDataStore;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="logger">ロガー</param>
    /// <param name="employeeDeleteService">従業員登録サービスインターフェイス</param>
    /// <param name="employeeDeleteViewModelAdapter">従業員登録ViewModelをEmployeeに変換するアダプター</param>
    /// <param name="empDataStore">TempDataを通じて一時的にViewModelを保存・復元するためのクラス</param>
    public EmployeeDeleteController(
        ILogger<EmployeeDeleteController> logger,
        IEmployeeDeleteService employeeDeleteService,
        EmployeeDeleteViewModelAdapter employeeDeleteViewModelAdapter,
        TempDataStore<EmployeeDeleteViewModel> empDataStore)
    {
        _logger = logger;
        _employeeDeleteService = employeeDeleteService;
        _adapter = employeeDeleteViewModelAdapter;
        _empDataStore = empDataStore;
    }

    /// <summary>
    /// 従業登録(入力)画面表示 アクションメソッド
    /// </summary>
    /// <returns></returns>
    [HttpGet("Enter")]
    public IActionResult Enter()
    {
        EmployeeDeleteViewModel? viewModel = null;
        // [戻る]ボタンへの対応
        // TempDataからEmployeeDeleteViewModelを取得する
        viewModel = _empDataStore.Load(this);
        if (viewModel == null)
        {
            // 従業員登録ViewModelを生成する
            viewModel = new EmployeeDeleteViewModel();
        }
        // 部署一覧を取得してViewModelに設定する(SelectListItem形式)
        PopulateEmployees(viewModel);
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
    public IActionResult Confirm(EmployeeDeleteViewModel viewModel)
    {
        // バリデーションチェック
        if (!ModelState.IsValid) // バリデーションエラーあり
        {
            // 社員一覧を取得してViewModelに設定する(SelectListItem形式)
            PopulateEmployees(viewModel);
            // 入力画面の表示
            return View("Enter", viewModel);
        }
        // 選択された社員のIdで社員データを取得する
        Console.WriteLine("データ取得");
        Console.WriteLine(viewModel.Id);
        var employee = _employeeDeleteService.GetById(viewModel.Id ?? 0);
        Console.WriteLine($"コントローラーのConfirm：{employee}");
        _logger.LogInformation($"社員Id:{viewModel.Id ?? 0}の社員を取得する");
        // ViewModelに部署名を設定する
        Console.WriteLine(employee);
        viewModel.Id = employee.Id;
        viewModel.Name = employee.Name;
        viewModel.DeptId = employee.Department!.Id;
        viewModel.DeptName = employee.Department.Name;
        Console.WriteLine($"コントローラーのConfirm2：社員Id={viewModel.Id},社員名={viewModel.Name},部署Id={viewModel.DeptId},部署名={viewModel.DeptName}");
        // 確認画面を表示する
        return View(viewModel);
    }

    /// <summary>
    /// 確認画面の[登録]ボタンクリックアクションメソッド
    /// </summary>
    /// <param name="form"></param>
    /// <returns></returns>
    [HttpPost("Delete")]
    public IActionResult Delete(EmployeeDeleteViewModel viewModel)
    {
        Console.WriteLine($"コントローラーのDelete：社員Id={viewModel.Id},社員名={viewModel.Name},部署Id={viewModel.DeptId},部署名={viewModel.DeptName}");
        // EmployeeDeleteViewModelをシリアライズして、TempDataに保存する
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
        EmployeeDeleteViewModel? viewModel = null;
        // TempDataからEmployeeDeleteViewModelを取得する
        viewModel = _empDataStore.Load(this);
        Console.WriteLine($"コントローラーのComplete：社員Id={viewModel.Id},社員名={viewModel.Name},部署Id={viewModel.DeptId},部署名={viewModel.DeptName}");
        if (viewModel == null)
        {
            // データが存在しない場合、入力画面にリダイレクト
            return RedirectToAction("Enter");
        }
        // EmployeeDeleteFormをドメインモデル:Employeeに変換する
        var employee = _adapter.Restore(viewModel!);
        Console.WriteLine($"Completeでのemployee：{employee}");
        // 従業員を削除する
        _employeeDeleteService.Delete(employee);
        return View(viewModel);
    }

    /// <summary>
    /// 確認画面の[戻る]ボタンクリックアクションメソッド
    /// </summary>
    /// <returns></returns> 
    [HttpPost("Back")]
    public IActionResult Back(EmployeeDeleteViewModel viewModel)
    {
        _logger.LogInformation("[戻る]ボタンクリック:{0}", viewModel!.ToString());
        // EmployeeDeleteViewModelをシリアライズして、TempDataに保存する
        _empDataStore.Save(this, viewModel);
        // 入力画面を出力するアクションメソッドにリダイレクトする
        return RedirectToAction("Enter");
    }

    /// <summary>
    /// 部署一覧を取得してViewModelに設定する(SelectListItem形式)
    /// </summary>
    private void PopulateEmployees(EmployeeDeleteViewModel viewModel)
    {
        // 従業員登録サービスから部署一覧を取得する
        var employees = _employeeDeleteService.GetEmployees();
        // 社員情報をEmployeeDeleteViewModelに登録する
        viewModel.SetEmployees(employees);
        _logger.LogInformation("社員リストを設定");
    }
}