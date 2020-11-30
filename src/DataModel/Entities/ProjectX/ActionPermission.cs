﻿using System;
using System.Collections.Generic;

namespace DataModel.Entities.ProjectX
{
    public class ActionPermission
    {
        public ActionPermission()
        {
            UserAction = new HashSet<UserAction>();
        }

        public int Id { get; set; }
        public string PermissionName { get; set; }
        public byte PermissionValue { get; set; }

        public virtual ICollection<UserAction> UserAction { get; set; }
    }
}
