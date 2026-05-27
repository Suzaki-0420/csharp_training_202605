using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using csharp_training_202605.Applications.Domains;
namespace csharp_training_202605.Presentations.ViewModels;
/// <summary>
/// 部署登録ViewModelクラス
/// </summary>
public class DepartmentShowViewModel
{

    [Display(Name = "部署ID")]
    public int? Id { get; set; } = 0;

    /// <summary>
    /// 従業員名
    /// </summary>
    [Display(Name = "部署名")]
    public string? Name { get; set; } = string.Empty;


    public DepartmentShowViewModel(int? id, string? name)
    {
        Id = id;
        Name = name;
    }
}