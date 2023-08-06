using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Configuration;
using System.Web.Security;
using System.Diagnostics;
using System.IO;
using System.Web.Hosting;
using System.Text;
using System.Xml;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Web.Mvc;
using System.Web.UI;

namespace TLSite.Models
{
    public class Foreign_Allow
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public enum AuthType
    {
        TYPE_ADMIN = 1,
        TYPE_AGENT,
        TYPE_MERCHANT
    }



    public class TopNavInfo
    {
        public TopNavInfo()
        {
            isend = true;
        }

        public bool isend { get; set; }
        public string title { get; set; }
        public string linkurl { get; set; }
    }

    public class CommonModel
    {
        public static string _logFilename = "Log.txt";

        public static string GetDisplayDateFormat()
        {
            return "yyyy年M月d日";
        }

        public static string GetDisplayDateTimeFormat()
        {
            return "yyyy年M月d日 HH:mm:ss";
        }

        public static string GetDisplayTimeFormat()
        {
            return "HH - mm";
        }

        public static string GetParamDateFormat()
        {
            return "yyyy-MM-dd";
        }

        public static string GetParamDateTimeFormat()
        {
            return "yyyy-MM-dd HH:mm:ss";
        }

        public static string ConnectionString
        {
            get 
            {
                return ConfigurationManager.ConnectionStrings["TieluConnectionString"].ConnectionString;
            }
        }

        private static string ranData = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public static string RandomString(int size)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = ranData[Convert.ToInt32(Math.Floor(36 * random.NextDouble()))];
                builder.Append(ch);
            }

            return builder.ToString();
        }

        private static string ranNumber = "0123456789";
        public static string RandomNumber(int size)
        {
            Random random = new Random((int)DateTime.Now.Ticks);
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = ranNumber[Convert.ToInt32(Math.Floor(10 * random.NextDouble()))];
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public static string GetUserRoleInfo()
        {
            FormsIdentity fId = (FormsIdentity)HttpContext.Current.User.Identity;
            FormsAuthenticationTicket authTicket = fId.Ticket;
            string[] udata = authTicket.UserData.Split(new Char[] { '|' });
            string role = udata[0];

            return role;
        }

        public static string[] GetUserRoles()
        {
            string roleinfo = GetUserRoleInfo();

            return roleinfo.Split(',');
        }

        public bool CheckUserRoles(string p_role)
        {
            string[] uroles = GetUserRoles();
            if (uroles.Contains(p_role) == true) {
                return true;
            }

            return false;
        }

        public string GetCurrentUserName()
        {
            FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;

            if (id != null)
            {
                return id.Name;
            }

            return null;
        }

        public static void WriteLogFile(string fileName, string methodName, string message)
        {

            try
            {
                string filepath = HostingEnvironment.MapPath("~/") + "\\" + _logFilename;
                if (!string.IsNullOrEmpty(message))
                {
                    using (FileStream file = new FileStream(filepath, File.Exists(filepath)? FileMode.Append:FileMode.OpenOrCreate, FileAccess.Write))
                    {
                        StreamWriter streamWriter = new StreamWriter(file);
                        streamWriter.WriteLine((((System.DateTime.Now + " - ") + fileName + " - ") + methodName + " - ") + message);
                        streamWriter.Close();
                    }
                }
            }
            catch {

            }
        }

        public static string GetBaseUrl()
        {
            var request = HttpContext.Current.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;
            var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);

            return baseUrl;
        }

        public static bool DeleteDirectory(DirectoryInfo dirInfo)
        {
            foreach (System.IO.FileInfo file in dirInfo.GetFiles())
            {
                try
                {
                    if (file.IsReadOnly)
                        file.Attributes = FileAttributes.Normal;
                    file.Delete();
                }
                catch (System.Exception ex)
                {
                    CommonModel.WriteLogFile("CommonModel", "DeleteDirectory() -> Delete file", ex.ToString());
                }
            }

            foreach (System.IO.DirectoryInfo subDirectory in dirInfo.GetDirectories())
            {
                DeleteDirectory(subDirectory);

                try
                {
                    if (dirInfo.Attributes == FileAttributes.ReadOnly)
                        dirInfo.Attributes = FileAttributes.Normal;
                    dirInfo.Delete(true);
                }
                catch (System.Exception ex)
                {
                    CommonModel.WriteLogFile("CommonModel", "DeleteDirectory() -> Delete directory", ex.ToString());
                }
            }

            return true;
        }

        public static string GetSHA1Hash(string value)
        {
            SHA1 sha1Hasher = SHA1.Create();
            byte[] data = sha1Hasher.ComputeHash(Encoding.Default.GetBytes(value));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public static long GetSessionUserID()
        {
            FormsIdentity fId = (FormsIdentity)HttpContext.Current.User.Identity;
            FormsAuthenticationTicket authTicket = fId.Ticket;
            string[] udata = authTicket.UserData.Split(new Char[] { '|' });
            string role = udata[0];
            string uid = udata[1];

            return long.Parse(uid);

        }

        public static string GetSessionUserImgUrl()
        {
            FormsIdentity fId = (FormsIdentity)HttpContext.Current.User.Identity;
            FormsAuthenticationTicket authTicket = fId.Ticket;
            string[] udata = authTicket.UserData.Split(new Char[] { '|' });
            string imgurl = udata[3];

            return imgurl;

        }

        public static AuthType GetSessionAuthType()
        {
            FormsIdentity fId = (FormsIdentity)HttpContext.Current.User.Identity;
            FormsAuthenticationTicket authTicket = fId.Ticket;
            string[] udata = authTicket.UserData.Split(new Char[] { '|' });
            string role = udata[0];
            string uid = udata[1];
            long authtype = int.Parse(udata[2]);

            AuthType type = (AuthType)authtype;
            if (type == AuthType.TYPE_ADMIN)
            {
                // check top admin type
//                 AccountModel aModel = new AccountModel();
//                 long sId = GetSessionUserID();
//                 var aInfo = aModel.GetAgentInfo(sId);
//                 if (aInfo.parentid > 0)
//                 {
//                     type = AuthType.TYPE_AGENT;
//                 }
            }

            return type;
        }

        public static string HexToStr(string hexStr)
        {
            string ret = "";

            string convStr = hexStr;
            int i = 0;
            int strlen = hexStr.Length;
            do
            {
                //Regex reg = new Regex(@"(?<hex>[0-9A-F]{2})", RegexOptions.IgnoreCase | RegexOptions.Compiled);
                Regex reg = new Regex(@"^([0-9A-Fa-f]{4})");
                if (reg.IsMatch(hexStr))
                {
                    char c = (char)int.Parse(hexStr.Substring(2, 4), NumberStyles.HexNumber);
                    ret += c;
                    i += 6;
                }
                else
                {
                    ret += hexStr[0];
                    i++;
                }
                hexStr = convStr.Substring(i);

            } while (i < strlen);
            return ret;
        }

        public static string RenderPartialToString(string controlName, object viewData)
        {
            ViewPage viewPage = new ViewPage() { ViewContext = new ViewContext() };

            viewPage.ViewData = new ViewDataDictionary(viewData);
            viewPage.Controls.Add(viewPage.LoadControl(controlName));

            StringBuilder sb = new StringBuilder();
            using (StringWriter sw = new StringWriter(sb))
            {
                using (HtmlTextWriter tw = new HtmlTextWriter(sw))
                {
                    viewPage.RenderControl(tw);
                }
            }

            return sb.ToString();
        }

        public static string getRealUrl(string inStr)
        {
            string strRet = inStr;

            if (inStr == null)
                return strRet;

            if (inStr.Length > 1 && inStr.IndexOf('/') == 0)
            {
                strRet = inStr.Substring(1);
            }

            return strRet;
        }

        public static byte[] ToByteArray(string StringToConvert)
        {

            char[] CharArray = StringToConvert.ToCharArray();

            byte[] ByteArray = new byte[CharArray.Length];

            for (int i = 0; i < CharArray.Length; i++)
            {

                ByteArray[i] = Convert.ToByte(CharArray[i]);

            }

            return ByteArray;

        }

        public static string DecodeFromUtf8(string utf8String)
        {
            // read the string as UTF-8 bytes.
            byte[] encodedBytes = ToByteArray(utf8String);

            // builds the converted string.
            return Encoding.UTF8.GetString(encodedBytes);
        }

        public static string han_cut(string val, int cut_len)
        {
            string retStr = "";
            int byteLen = val.Length;
            /*
		    int i = 0;
		    int strLen = 0;

		    while (i < byteLen && strLen < cut_len)
            {
                if (val[i] > 0xE0)
                    i += 3;
                else
                    i++;

			    strLen ++;
		    }
            */

            if (byteLen <= cut_len)
            {
                return val;
            }
            else
            {
                retStr = val.Substring(0, cut_len);
                retStr = String.Concat(retStr, "...");
            }

            /*
            if (i < byteLen && strLen >= cut_len)
                //retStr .= "...";
                String.Concat(retStr, "...");
            */
            return retStr;
        }

        public static string GetNavTitle(string navname)
        {
            string rst = "NULL";
            switch (navname)
            {
                case "Home":
                    rst = "首页";
                    break;
                case "User":
                    rst = "管理员";
                    break;
                case "UserList":
                    rst = "管理员列表";
                    break;
                case "AddUser":
                    rst = "添加管理员";
                    break;
                case "EditUser":
                    rst = "编辑管理员";
                    break;
                case "RoleList":
                    rst = "角色列表";
                    break;
                case "AddRole":
                    rst = "添加角色";
                    break;
                case "EditRole":
                    rst = "编辑角色";
                    break;
                case "Goods":
                    rst = "商品管理";
                    break;
                case "GoodsList":
                    rst = "商品列表";
                    break;
                case "AddGoods":
                    rst = "添加商品";
                    break;
                case "EditGoods":
                    rst = "编辑商品";
                    break;
                case "GoodsCategory":
                    rst = "分类列表";
                    break;
                case "AddCategory":
                    rst = "添加分类";
                    break;
                case "EditCategory":
                    rst = "编辑分类";
                    break;
                case "GoodsSpec":
                    rst = "规格列表";
                    break;
                case "AddSpec":
                    rst = "添加规格";
                    break;
                case "EditSpec":
                    rst = "编辑规格";
                    break;
                case "GoodsBrand":
                    rst = "品牌列表";
                    break;
                case "AddBrand":
                    rst = "添加品牌";
                    break;
                case "EditBrand":
                    rst = "编辑品牌";
                    break;
                case "GoodsTag":
                    rst = "标签列表";
                    break;
                case "AddTag":
                    rst = "添加标签";
                    break;
                case "EditTag":
                    rst = "编辑标签";
                    break;
            }

            return rst;
        }

        public static string GetNavLink(string navname, string rootUri)
        {
            string rst = "NULL";
            switch (navname)
            {
                case "Home":
                    rst = rootUri;
                    break;
                case "User":
                    rst = rootUri + "User/UserList";
                    break;
                case "UserList":
                    rst = rootUri + "User/UserList";
                    break;
                case "AddUser":
                    rst = rootUri + "User/AddUser";
                    break;
                case "EditUser":
                    rst = rootUri + "User/EditUser";
                    break;
                case "RoleList":
                    rst = rootUri + "User/RoleList";
                    break;
                case "AddRole":
                    rst = rootUri + "User/AddRole";
                    break;
                case "EditRole":
                    rst = rootUri + "User/EditRole";
                    break;
                case "Goods":
                    rst = rootUri + "Goods/GoodsList";
                    break;
                case "GoodsList":
                    rst = rootUri + "Goods/GoodsList";
                    break;
                case "AddGoods":
                    rst = rootUri + "Goods/AddGoods";
                    break;
                case "EditGoods":
                    rst = rootUri + "Goods/EditList";
                    break;
                case "GoodsCategory":
                    rst = rootUri + "Goods/CategoryList";
                    break;
                case "AddCategory":
                    rst = rootUri + "Goods/AddCategory";
                    break;
                case "EditCategory":
                    rst = rootUri + "Goods/EditCategory";
                    break;
                case "GoodsSpec":
                    rst = rootUri + "Goods/SpecList";
                    break;
                case "AddSpec":
                    rst = rootUri + "Goods/AddSpec";
                    break;
                case "EditSpec":
                    rst = rootUri + "Goods/EditSpec";
                    break;
                case "GoodsBrand":
                    rst = rootUri + "Goods/BrandList";
                    break;
                case "AddBrand":
                    rst = rootUri + "Goods/AddBrand";
                    break;
                case "EditBrand":
                    rst = rootUri + "Goods/EditBrand";
                    break;
                case "GoodsTag":
                    rst = rootUri + "Goods/TagList";
                    break;
                case "AddTag":
                    rst = rootUri + "Goods/AddTag";
                    break;
                case "EditTag":
                    rst = rootUri + "Goods/EditTag";
                    break;
            }

            return rst;
        }

        public static List<TopNavInfo> GetTopNavInfo(string level1str, string level2str, string level3str, string level4str, string rootUri)
        {
            List<TopNavInfo> retlist = new List<TopNavInfo>();

            if (!String.IsNullOrEmpty(level1str))
            {
                TopNavInfo newitem = new TopNavInfo();

                if (!String.IsNullOrEmpty(level2str))
                {
                    newitem.isend = false;
                }

                newitem.title = GetNavTitle(level1str);
                newitem.linkurl = GetNavLink(level1str, rootUri);

                retlist.Add(newitem);
            }

            if (!String.IsNullOrEmpty(level2str))
            {
                TopNavInfo newitem = new TopNavInfo();

                if (!String.IsNullOrEmpty(level3str))
                {
                    newitem.isend = false;
                    newitem.linkurl = GetNavLink(level2str, rootUri);
                }
                else
                {
                    newitem.isend = true;
                }

                newitem.title = GetNavTitle(level2str);
                retlist.Add(newitem);
            }

            if (!String.IsNullOrEmpty(level3str))
            {
                TopNavInfo newitem = new TopNavInfo();

                if (!String.IsNullOrEmpty(level4str))
                {
                    newitem.isend = false;
                    newitem.linkurl = GetNavLink(level3str, rootUri);
                }
                else
                {
                    newitem.isend = true;
                }

                newitem.title = GetNavTitle(level3str);
                retlist.Add(newitem);
            }

            if (!String.IsNullOrEmpty(level4str))
            {
                TopNavInfo newitem = new TopNavInfo();

                newitem.isend = true;

                newitem.title = GetNavTitle(level4str);
                retlist.Add(newitem);
            }

            return retlist;
        }

        public static long GetCurrAccountId()
        {
            long rst = 0;
#if !DEBUG
            string domainurl = HttpContext.Current.Request.Url.Authority;
            string[] urilist = domainurl.Split('.');

            try
            {
                rst = long.Parse(urilist[0]);
            }
            catch (System.Exception ex)
            {
            	
            }
            
#else
            try
            {
                rst = long.Parse(ConfigurationManager.AppSettings["DebugAccountID"].ToString());
            }
            catch (System.Exception ex)
            {
            	
            }
#endif
            return rst;
        }

        public static string GetTimeDiffFromNow(DateTime difftime)
        {
            var ts = new TimeSpan(DateTime.Now.Ticks - difftime.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            if (delta < 0)
            {
                return "刚才";
            }
            if (delta < 1 * MINUTE)
            {
                return "刚才";
            }
            if (delta < 2 * MINUTE)
            {
                return "一分钟前";
            }
            if (delta < 45 * MINUTE)
            {
                return ts.Minutes + " 分钟前";
            }
            if (delta < 90 * MINUTE)
            {
                return "一个小时前";
            }
            if (delta < 24 * HOUR)
            {
                return ts.Hours + "个小时前";
            }
            if (delta < 48 * HOUR)
            {
                return "昨天";
            }
            if (delta < 30 * DAY)
            {
                return ts.Days + "天前";
            }
            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return months <= 1 ? "一个月前" : months + "个月前";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return years <= 1 ? "一年前" : years + "年前";
            }
        }

        public static long GetCurrZoneId()
        {
            long rst = 0;



            return rst;
        }

        public String ToUtf8String(String s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (c >= 0 && c <= 255)
                {
                    sb.Append(c);
                }
                else
                {
                    byte[] b;
                    try
                    {
                        b = Encoding.UTF8.GetBytes(c.ToString());
                    }
                    catch (Exception ex)
                    {
                        b = new byte[0];
                    }
                    for (int j = 0; j < b.Length; j++)
                    {
                        int k = b[j];
                        if (k < 0) k += 256;

                        sb.Append("%" + Convert.ToString(k, 16).ToUpper());
                    }
                }
            }
            return sb.ToString();
        }
    }
}