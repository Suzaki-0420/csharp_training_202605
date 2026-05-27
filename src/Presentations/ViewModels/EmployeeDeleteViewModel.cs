using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using csharp_training_202605.Applications.Domains;
namespace csharp_training_202605.Presentations.ViewModels;
/// <summary>
/// 部署登録ViewModelクラス
/// </summary>
public class EmployeeDeleteViewModel
{

    [Display(Name = "従業員番号")]
    public int? Id { get; set; } = 0;
    /// <summary>
    /// 氏名
    /// </summary>
    [Display(Name = "削除する従業員名")]
    public string? Name { get; set; } = string.Empty;

    [Display(Name = "メールアドレス")]
    public string? Email { get; set; } = string.Empty;

    [Display(Name = "電話番号")]
    public string? Phone { get; set; } = string.Empty;


    /// <summary>
    /// 所属部署
    /// </summary>
    [Display(Name = "所属部署")]
    public int? DeptId { get; set; } = 0;

    /// <summary>
    /// 選択された部署名
    /// </summary>
    [Display(Name = "部署名")]
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 従業員のリストをSelectListItemのリストに変換してプロパティに設定する
    /// </summary>
    /// <param name="departments"></param>
    public void SetEmployees(List<Employee> employees)
    {
        // SelectListItemのリストを作成
        var selectItems = new List<SelectListItem>();
        foreach (var emp in employees)
        {
            if (emp.Id.HasValue)
            {
                var item = new SelectListItem();
                item.Value = emp.Id.Value.ToString();
                item.Text = emp.ToString();
                selectItems.Add(item);
            }
        }
        Employees = selectItems;
    }
    // 部署のリスト
    public List<SelectListItem>? Employees { get; set; } = null;

    public override string ToString()
    {
        return $"従業員番号={Id} , 名前={Name} , 部署ID={DeptId} , 部署名={DeptName} ";
    }
}