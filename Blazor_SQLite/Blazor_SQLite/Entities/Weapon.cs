using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Blazor_SQLite.Entities
{
    /// <summary>
    /// 武器テーブルのエンティティクラス
    /// </summary>
    [Table("Weapons")]
    public class Weapon
    {
        /// <summary>
        /// 識別子
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 名前
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 武器説明
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// 攻撃力
        /// </summary>
        [Required]
        public int AttackPower { get; set; }
    }

}
