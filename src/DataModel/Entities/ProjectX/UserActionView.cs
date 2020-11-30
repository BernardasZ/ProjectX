using DataModel.Enums;

namespace DataModel.Entities.ProjectX
{
    public class UserActionView
    {
        public int Id { get; set; }
        public UserRoleEnum RoleValue { get; set; }
        public string ActionName { get; set; }
        public ActionPermissionEnum PermissionValue { get; set; }
    }
}
