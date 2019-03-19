using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Backend
{
    public static class SecurityController
    {
        #region Fixed security roles
        public const string SUser = nameof(SUser);
        #endregion
        public static LoggedInUser FindOrCreateAccount()
        {
            return FindOrCreateAccount(Thread.CurrentPrincipal?.Identity?.Name);
        }
        public static LoggedInUser FindOrCreateAccount(string windowsAccountName)
        {
            if (string.IsNullOrWhiteSpace(windowsAccountName))
                throw new ArgumentNullException(nameof(windowsAccountName), "You must supply some windows account name");
            using (var context = new SecurityContext())
            {
                var found = context.UserAccounts.SingleOrDefault(x => x.WindowsAccount.Equals(windowsAccountName));
                if(found == null)
                {
                    // Create it as a new account
                    found = context.UserAccounts.Add(new UserAccount { WindowsAccount = windowsAccountName });
                    context.SaveChanges();
                    if(windowsAccountName == ConfigurationManager.AppSettings[SUser])
                    {
                        var adminRole = context.SecurityRoles.SingleOrDefault(x => x.Name == SUser);
                        if(adminRole == null)
                        {
                            // Make the Admin role
                            context.SecurityRoles.Add(new SecurityRole { Name = SUser });
                            context.SaveChanges();
                            adminRole = context.SecurityRoles.SingleOrDefault(x => x.Name == SUser);
                        }
                        context.Permissions.Add(new Permission { SecurityRoleID = adminRole.SecurityRoleID, UserAccountID = found.UserAccountID });
                        context.SaveChanges();
                    }
                }
                var result = new LoggedInUser
                {
                    AccountID = found.UserAccountID,
                    DisplayName = found.DisplayName,
                    SecurityRoles = found.Roles.Select(x => x.SecurityRole.Name).ToList()
                };
                return result;
            }
        }
    }
    public class LoggedInUser
    {
        public int AccountID { get; set; }
        public string DisplayName { get; set; }
        public ICollection<string> SecurityRoles { get; set; }
            = new HashSet<string>();
    }
    internal class SecurityContext : DbContext
    {
        public SecurityContext() : base("name=DefaultConnection")
        {
        }

        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<SecurityRole> SecurityRoles { get; set; }
    }

    [Table("UserAccounts", Schema = "Security")]
    internal class UserAccount
    {
        [Key]
        public int UserAccountID { get; set; }
        [Required, Index("IX_UserAccount_Name", IsUnique = true)]
        [MaxLength(200)]
        public string WindowsAccount { get; set; }
        [MaxLength(200)]
        public string DisplayName { get; set; }

        // Navigation Properties
        public virtual ICollection<Permission> Roles { get; set; }
        = new HashSet<Permission>();
    }
    [Table("Permissions", Schema = "Security")]
    internal class Permission
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
    internal class SecurityRole
    {
        [Key]
        public int SecurityRoleID { get; set; }
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }
        [MaxLength(1000)]
        public string Description { get; set; }
        // Navigation Properties
        public virtual ICollection<Permission> Users { get; set; }
        = new HashSet<Permission>();
    }
}
