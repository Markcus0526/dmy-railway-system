using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TLSite.Models
{
    public class CrewRoleModel
    {

        TieluDBDataContext db = new TieluDBDataContext();
        public List<tbl_crewrole> GetCrewRoleList()
        {
            return db.tbl_crewroles.ToList();
        }

    }
}