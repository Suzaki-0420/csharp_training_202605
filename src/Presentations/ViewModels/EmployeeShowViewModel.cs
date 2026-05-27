using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using csharp_training_202605.Applications.Domains;
namespace csharp_training_202605.Presentations.ViewModels;
/// <summary>
/// 部署登録ViewModelクラス
/// </summary>
public class EmployeeShowViewModel
{

    [Display(Name = "社員ID")]
    public int? Id { get; set; } = 0;

    /// <summary>
    /// 社員名
    /// </summary>
    [Display(Name = "氏名")]
    public string? Name { get; set; } = string.Empty;

    [Display(Name = "メールアドレス")]
    public string? Email { get; set; } = string.Empty;

    [Display(Name = "電話番号")]
    public string? Phone { get; set; } = string.Empty;

    public int? DeptId { get; set; } = 0;
    [Display(Name = "部署")]
    public string? DeptName { get; set; } = string.Empty;


    public EmployeeShowViewModel(int? id, string? name, string? email, string? phone, int? deptid, string? deptname)
    {
        Id = id;
        Name = name;
        Email = email;
        Phone = phone;
        DeptId = deptid;
        DeptName = deptname;
    }
}