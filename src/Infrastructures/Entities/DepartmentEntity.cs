using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApp_Sample.Infrastructures.Entities;
/// <summary>
/// 部署テーブル(department)を扱うEntity Framework Coreのエンティティクラス
/// </summary>
[Table("department")]
public class DepartmentEntity
{
    /// <summary>
    /// 部署Id(主キー)
    /// </summary> 
    [Key]
    [Column("id")]
    public int DeptId { get; set; }
    /// <summary>
    /// 部署名
    /// </summary> 
    [Column("name")]
    public string DeptName { get; set; } = string.Empty;
}