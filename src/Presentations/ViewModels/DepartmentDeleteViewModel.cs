using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using csharp_training_202605.Applications.Domains;
namespace csharp_training_202605.Presentations.ViewModels;
/// <summary>
/// 部署登録ViewModelクラス
/// </summary>
public class DepartmentDeleteViewModel
{

    [Display(Name = "部門")]
    public int? Id { get; set; } = 0;
    /// <summary>
    /// 氏名
    /// </summary>
    [Display(Name = "削除する部門名")]
    public string? Name { get; set; } = string.Empty;


    /// <summary>
    /// 従業員のリストをSelectListItemのリストに変換してプロパティに設定する
    /// </summary>
    /// <param name="departments"></param>
    public void SetDepartments(List<Department> departments)
    {
        // SelectListItemのリストを作成
        var selectItems = new List<SelectListItem>();
        foreach (var dep in departments)
        {
            if (dep.Id.HasValue)
            {
                var item = new SelectListItem();
                item.Value = dep.Id.Value.ToString();
                item.Text = dep.ToString();
                selectItems.Add(item);
            }
        }
        Departments = selectItems;
    }
    // 部署のリスト
    public List<SelectListItem>? Departments { get; set; } = null;

    public override string ToString()
    {
        return $"部門番号={Id} , 部門名={Name}  ";
    }
}