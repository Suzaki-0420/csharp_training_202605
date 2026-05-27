using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using csharp_training_202605.Applications.Domains;
namespace csharp_training_202605.Presentations.ViewModels;
/// <summary>
/// 部署登録ViewModelクラス
/// </summary>
public class EmployeeUpdateViewModel
{

    public int? Id { get; set; } = 0;
    /// <summary>
    /// 氏名
    /// </summary>
    [Display(Name = "氏名")]
    [Required(ErrorMessage = "{0}は入力必須です。")]
    [StringLength(20, ErrorMessage = "20文字以内で入力してください。")]
    //[RegularExpression(@"^.{1,20}$", ErrorMessage = "20文字以内で入力してください。")]
    public string? Name { get; set; } = string.Empty;

    [Display(Name = "メールアドレス")]
    [Required(ErrorMessage = "{0}は入力必須です。")]
    [EmailAddress(ErrorMessage = "メールアドレスの形式で入力してください。")]

    public string? Email { get; set; } = string.Empty;

    [Display(Name = "電話番号")]
    [Required(ErrorMessage = "{0}は入力必須です。")]
    [RegularExpression(@"^0\d{1,4}-\d{1,4}-\d{4}$", ErrorMessage = "電話番号の形式（例: 03-1234-5678）で入力してください。")]
    public string? Phone { get; set; } = string.Empty;
    /// <summary>
    /// 所属部署
    /// </summary>
    [Display(Name = "所属部署")]
    [Required(ErrorMessage = "{0}は選択必須です。")]
    public int? DeptId { get; set; } = 0;

    /// <summary>
    /// 選択された部署名
    /// </summary>
    [Display(Name = "部署名")]
    public string? DeptName { get; set; } = string.Empty;

    /// <summary>
    /// 部署のリストをSelectListItemのリストに変換してプロパティに設定する
    /// </summary>
    /// <param name="departments"></param>
    public void SetDepartments(List<Department> departments)
    {
        // SelectListItemのリストを作成
        var selectItems = new List<SelectListItem>();
        foreach (var dept in departments)
        {
            if (dept.Id.HasValue)
            {
                var item = new SelectListItem();
                item.Value = dept.Id.Value.ToString();
                item.Text = string.IsNullOrEmpty(dept.Name) ? "(名称未設定)" : dept.Name;
                selectItems.Add(item);
            }
        }
        Departments = selectItems;
    }
    // 部署のリスト
    public List<SelectListItem>? Departments { get; set; } = null;

    public override string ToString()
    {
        return $"Name={Name} , DeptId={DeptId} , DeptName={DeptName} , Departments={Departments}";
    }
}