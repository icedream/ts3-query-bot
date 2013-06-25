using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TS3Query
{
    public class Permissions
    {
        static Permissions()
        {
            permissions.Load("permissions.xml");
        }

        static XmlDocument permissions = new XmlDocument();

        public static bool HasPermission(string uniqueID, string groupID, string permissionNode) // permissionNode => inspired from Bukkit permissions, lol.
        {
            bool allowed = false;
            bool revoke = false;

            // TODO: Wildcards
            // TODO: Check up if invoker IDs are okay
            // TODO: Group inheritances? We shouldn't overdo that inb4 new permissions system.

            allowed = allowed || permissions.SelectNodes("/permissions/group[@id='" + groupID + "']/permission[@id='" + permissionNode + "']").Count > 0;
            allowed = allowed || permissions.SelectNodes("/permissions/user[@uid='" + uniqueID + "']/permission[@id='" + permissionNode + "']").Count > 0;

            revoke = revoke || permissions.SelectNodes("/permissions/group[@id='" + groupID + "']/permission[@revoke='1' and @id='" + permissionNode + "']").Count > 0;
            revoke = revoke || permissions.SelectNodes("/permissions/user[@uid='" + uniqueID + "']/permission[@revoke='1' and @id='" + permissionNode + "']").Count > 0;

            return allowed && !revoke;
        }

    }
}
