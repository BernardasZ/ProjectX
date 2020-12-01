namespace DataModel.Entities.ProjectX
{
    public class UserAction
    {
        public int Id { get; set; }
        public int UserRoleId { get; set; }
        public int ActionId { get; set; }
        public int ActionPermissionId { get; set; }

        public virtual ControllerAction Action { get; set; }
        public virtual ActionPermission ActionPermission { get; set; }
        public virtual UserRole UserRole { get; set; }
    }
}
