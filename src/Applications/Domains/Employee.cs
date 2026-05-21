using System.Text.RegularExpressions;
using csharp_training_202605.Exceptions;
namespace csharp_training_202605.Applications.Domains;
/// <summary>
/// 従業員を表すドメインオブジェクト
/// </summary>
public class Employee
{
    public int? Id { get; private set; } // 社員Id
    public string Name { get; private set; } = string.Empty; // 氏名
    public string Email { get; private set; } = string.Empty; // メールアドレス
    public string Phone { get; private set; } = string.Empty; // 電話番号
    public Department? Department { get; private set; } // 所属部署（null可）

    private const int MaxLength = 20;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="id">社員Id</param>
    /// <param name="name">氏名</param>
    /// <param name="department">所属部署</param>
    public Employee(int? id, string name, string email, string phone, Department? department)
    {
        ValidateName(name);
        IsValidMailAddress(email);
        IsValidPhoneNumber(phone);
        Id = id;
        Name = name;
        Email = email;
        Phone = phone;
        Department = department;
    }

    /// <summary>
    /// ID未定の社員を作成する場合のコンストラクタ
    /// </summary>
    /// <param name="name">氏名</param>
    /// <param name="department">所属部署</param>
    public Employee(string name, string email, string phone, Department? department)
        : this(null, name, email, phone, department) { }

    /// <summary>
    /// 氏名の検証
    /// </summary>
    private void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("氏名は必須です");
        if (name.Length > MaxLength)
            throw new DomainException($"氏名は{MaxLength}文字以内で入力してください");
    }

    private void IsValidMailAddress(string address)
    {
        if (string.IsNullOrEmpty(address))
        {
            throw new DomainException("メールアドレスは必須です");
        }

        try
        {
            System.Net.Mail.MailAddress a =
                new System.Net.Mail.MailAddress(address);
        }
        catch (FormatException)
        {
            throw new DomainException("メールアドレスの形式と合っていません");
        }
    }

    private void IsValidPhoneNumber(string phonenumber)
    {
        if (string.IsNullOrEmpty(phonenumber))
        {
            throw new DomainException("電話番号は必須です");
        }
        bool isMatch = Regex.IsMatch(phonenumber, @"^\d{3}-\d{4}-\d{4}");

        if (isMatch == false)
        {
            throw new DomainException("電話番号の形式と合っていません");
        }

    }

    /// <summary>
    /// 氏名を変更する
    /// </summary>
    public void ChangeName(string name)
    {
        ValidateName(name);
        Name = name;
    }

    /// <summary>
    /// 所属部署を変更する
    /// </summary>
    public void ChangeDepartment(Department? department)
    {
        Department = department;
    }

    /// <summary>
    /// 等価性（IDによる比較）
    /// </summary>
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj)) return true;
        if (obj is not Employee other) return false;
        return Id == other.Id;
    }

    public override int GetHashCode() => Id?.GetHashCode() ?? 0;

    public override string ToString()
        => $"{Id?.ToString() ?? "未登録"}: {Name} / {Department?.Name ?? "未配属"}";
}