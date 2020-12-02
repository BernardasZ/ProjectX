using DataModel.Enums;

namespace DataModel.Entities.ProjectX
{
    public class UserActionView
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string ActionName { get; set; }
        public string PermissionName { get; set; }
    }
}
