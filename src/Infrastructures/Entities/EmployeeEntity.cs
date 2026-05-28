using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace csharp_training_202605.Infrastructures.Entities;
/// <summary>
/// 社員テーブル(employee)を扱うEntity Framework Coreのエンティティクラス
/// </summary>
[Table("employee")]
public class EmployeeEntity
{
    /// <summary>
    /// 社員Id(主キー)
    /// </summary>
    [Key]
    [Column("id")]
    public int EmpId { get; set; }
    [Column("name")]
    /// <summary>
    /// 社員名
    /// </summary>
    public string EmpName { get; set; } = string.Empty;

    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Column("phone")]
    public string Phone { get; set; } = string.Empty;

    [Column("dept_id")]
    public int? DeptId { get; set; }
    /// <summary>
    /// 所属部署Id(外部キー)
    /// </summary>
    [ForeignKey("DeptId")]
    public DepartmentEntity? Department { get; set; }

    public override string ToString()
    {
        return "Name=" + EmpName + ", DeptName=" + Department?.DeptName;
    }
}