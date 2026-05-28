using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using csharp_training_202605.Applications.Domains;
namespace csharp_training_202605.Presentations.ViewModels;
/// <summary>
/// 部署登録ViewModelクラス
/// </summary>
public class DepartmentUpdateViewModel
{

    [Display(Name = "部署")]
    public int? Id { get; set; } = 0;
    /// <summary>
    /// 氏名
    /// </summary>
    [Display(Name = "部署名")]
    [Required(ErrorMessage = "{0}は入力必須です。")]
    [StringLength(20, ErrorMessage = "20文字以内で入力してください。")]
    //[RegularExpression(@"^.{1,20}$", ErrorMessage = "20文字以内で入力してください。")]
    public string? Name { get; set; } = string.Empty;


    public override string ToString()
    {
        return $"部署番号={Id} , 部署名={Name}  ";
    }
}