using System.Collections.Generic;

namespace DataModel.Entities.ProjectX
{
    public class ControllerAction
    {
        public ControllerAction()
        {
            UserAction = new HashSet<UserAction>();
        }

        public int Id { get; set; }
        public string ActionName { get; set; }

        public virtual ICollection<UserAction> UserAction { get; set; }
    }
}
