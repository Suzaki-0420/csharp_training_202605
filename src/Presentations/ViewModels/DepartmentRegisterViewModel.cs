using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using csharp_training_202605.Applications.Domains;
namespace csharp_training_202605.Presentations.ViewModels;
/// <summary>
/// 部署登録ViewModelクラス
/// </summary>
public class DepartmentRegisterViewModel
{
    /// <summary>
    /// 氏名
    /// </summary>
    [Display(Name = "部署名")]
    [Required(ErrorMessage = "{0}は入力必須です。")]
    [RegularExpression(@"^.{1,20}$", ErrorMessage = "20文字以内で入力してください。")]
    public string? Name { get; set; } = string.Empty;

}