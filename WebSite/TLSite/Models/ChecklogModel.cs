using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TLSite.Models
{
    public class ChecklogModel 
    {
        //
        // GET: /ChecklogModel/


      public long uid { get; set; }
        //内容详情
      public  string content { get;set; }
       //乘务员id
      public long crewid { get; set; }
        //被联挂人id
      public long crewrelid { get; set; }
        //项点编号
      public long infoid { get; set; }

      public int checkpoint { get; set; }
      public double relatedpoint { get; set; }
      public DateTime checktime{ get; set; }
      public string checklevel { get; set; }
      public string relatedpointstring { get; set; }
        
    }

    

}
