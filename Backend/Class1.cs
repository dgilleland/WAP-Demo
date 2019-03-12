using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend
{
    public class SecurityContext : DbContext
    {
        public SecurityContext() : base("name=DefaultConnection")
        {
        }

        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<SecurityRole> SecurityRoles { get; set; }
    }

    [Table("UserAccounts", Schema = "Security")]
    public class UserAccount
    {
        [Key]
        public int UserAccountID { get; set; }
        public string WindowsAccount { get; set; }
        public string DisplayName { get; set; }

        // Navigation Properties
        public virtual ICollection<Permission> Roles { get; set; }
        = new HashSet<Permission>();
    }
    [Table("Permissions", Schema = "Security")]
    public class Permission
    {
        // Composite key
        [Key, Column(Order = 1)]
        public int UserAccountID { get; set; }
        [Key, Column(Order = 2)]
        public int SecurityRoleID { get; set; }
        public DateTime? Expires { get; set; }

        // Navigation Properties
        public virtual UserAccount UserAccount { get; set; }
        public virtual SecurityRole SecurityRole { get; set; }
    }
    [Table("SecurityRoles", Schema = "Security")]
    public class SecurityRole
    {
        #region Fixed security roles
        public const string SUser = nameof(SUser);
        #endregion

        [Key]
        public int SecurityRoleID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        // Navigation Properties
        public virtual ICollection<Permission> Users { get; set; }
        = new HashSet<Permission>();
    }
}
