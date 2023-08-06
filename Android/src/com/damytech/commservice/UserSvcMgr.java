package com.damytech.commservice;

import com.damytech.HttpConn.AsyncHttpClient;
import com.damytech.HttpConn.AsyncHttpResponseHandler;
import com.damytech.HttpConn.RequestParams;
import com.damytech.STData.*;
import org.apache.http.entity.StringEntity;
import org.json.JSONArray;
import org.json.JSONObject;

import java.net.URLEncoder;
import java.util.ArrayList;

public class UserSvcMgr {

    public void LoginUser(AsyncHttpResponseHandler handler, String username, String pwd)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdLoginUser;

            param.put("username", username);
            param.put("pwd", pwd);

            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseLoginUser(JSONObject jsonObject, STUserInfo userInfo)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            JSONObject object = jsonObject.getJSONObject("SVCC_DATA");
            userInfo.uid = object.getLong("uid");
            userInfo.grade = object.getLong("grade");
            userInfo.gradename = object.getString("gradename");

            return retResult;

        } catch (Exception ex) {
            userInfo = null;
        }

        return retResult;
    }

    public void GetJiDuanList(AsyncHttpResponseHandler handler)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetJiDuanList;
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, null, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetJiDuanList(JSONObject jsonObject, ArrayList<STJiDuan> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;
        arrData.clear();

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == STServiceData.ERR_SUCCESS)
            {
                JSONArray jsonArray = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    JSONObject item = jsonArray.getJSONObject(i);
                    STJiDuan stInfo = new STJiDuan();
                    stInfo.uid= item.getLong("Uid");
                    stInfo.name = item.getString("Name");

                    arrData.add(stInfo);
                }
            }
            else
            {
                arrData = null;
            }
        } catch (Exception ex) {
            arrData = null;
        }

        return retResult;
    }

    public void GetBuMenList(AsyncHttpResponseHandler handler)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetBuMenList;
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, null, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetBuMenList(JSONObject jsonObject, ArrayList<STBuMen> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;
        arrData.clear();

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == STServiceData.ERR_SUCCESS)
            {
                JSONArray jsonArray = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    JSONObject item = jsonArray.getJSONObject(i);
                    STBuMen stInfo = new STBuMen();
                    stInfo.uid= item.getLong("Uid");
                    stInfo.name = item.getString("Name");

                    arrData.add(stInfo);
                }
            }
            else
            {
                arrData = null;
            }
        } catch (Exception ex) {
            arrData = null;
        }

        return retResult;
    }

    public void GetCheDuiListWithUid(AsyncHttpResponseHandler handler, String bmID)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetCheDuiListWithUid;
            param.put("bmID", bmID);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetCheDuiListWithUid(JSONObject jsonObject, ArrayList<STCheDui> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;
        arrData.clear();

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == STServiceData.ERR_SUCCESS)
            {
                JSONArray jsonArray = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    JSONObject item = jsonArray.getJSONObject(i);
                    STCheDui stInfo = new STCheDui();
                    stInfo.uid= item.getLong("Uid");
                    stInfo.name = item.getString("Name");

                    arrData.add(stInfo);
                }
            }
            else
            {
                arrData = null;
            }
        } catch (Exception ex) {
            arrData = null;
        }

        return retResult;
    }

    public void GetCheDuiList(AsyncHttpResponseHandler handler, String starttime)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetCheDuiList;
            param.put("starttime", starttime);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetCheDuiList(JSONObject jsonObject, ArrayList<STCheDui> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;
        arrData.clear();

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == STServiceData.ERR_SUCCESS)
            {
                JSONArray jsonArray = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    JSONObject item = jsonArray.getJSONObject(i);
                    STCheDui stInfo = new STCheDui();
                    stInfo.uid= item.getLong("Uid");
                    stInfo.name = item.getString("Name");

                    arrData.add(stInfo);
                }
            }
            else
            {
                arrData = null;
            }
        } catch (Exception ex) {
            arrData = null;
        }

        return retResult;
    }

    public void GetAllCheDuiList(AsyncHttpResponseHandler handler)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetAllCheDuiList;
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, null, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetAllCheDuiList(JSONObject jsonObject, ArrayList<STCheDui> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;
        arrData.clear();

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == STServiceData.ERR_SUCCESS)
            {
                JSONArray jsonArray = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    JSONObject item = jsonArray.getJSONObject(i);
                    STCheDui stInfo = new STCheDui();
                    stInfo.uid= item.getLong("Uid");
                    stInfo.name = item.getString("Name");

                    arrData.add(stInfo);
                }
            }
            else
            {
                arrData = null;
            }
        } catch (Exception ex) {
            arrData = null;
        }

        return retResult;
    }

    public void GetBanZuList(AsyncHttpResponseHandler handler, String cdID, String starttime)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetBanZuList;
            param.put("cdID", cdID);
            param.put("starttime", starttime);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetBanZuList(JSONObject jsonObject, ArrayList<STBanZu> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;
        arrData.clear();

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == STServiceData.ERR_SUCCESS)
            {
                JSONArray jsonArray = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    JSONObject item = jsonArray.getJSONObject(i);
                    STBanZu stInfo = new STBanZu();
                    stInfo.uid= item.getLong("Uid");
                    stInfo.name = item.getString("Name");

                    arrData.add(stInfo);
                }
            }
            else
            {
                arrData = null;
            }
        } catch (Exception ex) {
            arrData = null;
        }

        return retResult;
    }

    public void GetAllBanZuList(AsyncHttpResponseHandler handler, String cdID)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetAllBanZuList;
            param.put("cdID", cdID);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetAllBanZuList(JSONObject jsonObject, ArrayList<STBanZu> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;
        arrData.clear();

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == STServiceData.ERR_SUCCESS)
            {
                JSONArray jsonArray = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    JSONObject item = jsonArray.getJSONObject(i);
                    STBanZu stInfo = new STBanZu();
                    stInfo.uid= item.getLong("Uid");
                    stInfo.name = item.getString("Name");

                    arrData.add(stInfo);
                }
            }
            else
            {
                arrData = null;
            }
        } catch (Exception ex) {
            arrData = null;
        }

        return retResult;
    }

    public void GetAllBanZuListWithJiFen(AsyncHttpResponseHandler handler, String cdID)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetAllBanZuListWithJiFen;
            param.put("cdID", cdID);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetAllBanZuListWithJiFen(JSONObject jsonObject, ArrayList<STBanZu> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;
        arrData.clear();

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == STServiceData.ERR_SUCCESS)
            {
                JSONArray jsonArray = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    JSONObject item = jsonArray.getJSONObject(i);
                    STBanZu stInfo = new STBanZu();
                    stInfo.uid= item.getLong("Uid");
                    stInfo.name = item.getString("Name");

                    arrData.add(stInfo);
                }
            }
            else
            {
                arrData = null;
            }
        } catch (Exception ex) {
            arrData = null;
        }

        return retResult;
    }

    public void GetZeRenRenList(AsyncHttpResponseHandler handler, String bzID, String starttime)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetZeRenRenList;
            param.put("bzID", bzID);
            param.put("starttime", starttime);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetZeRenRenList(JSONObject jsonObject, ArrayList<STZeRenRen> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;
        arrData.clear();

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == STServiceData.ERR_SUCCESS)
            {
                JSONArray jsonArray = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    JSONObject item = jsonArray.getJSONObject(i);
                    STZeRenRen stInfo = new STZeRenRen();
                    stInfo.uid= item.getLong("Uid");
                    stInfo.name = item.getString("Name");

                    arrData.add(stInfo);
                }
            }
            else
            {
                arrData = null;
            }
        } catch (Exception ex) {
            arrData = null;
        }

        return retResult;
    }

    public void GetAllZeRenRenList(AsyncHttpResponseHandler handler, String bzID)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetAllZeRenRenList;
            param.put("bzID", bzID);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetAllZeRenRenList(JSONObject jsonObject, ArrayList<STZeRenRen> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;
        arrData.clear();

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == STServiceData.ERR_SUCCESS)
            {
                JSONArray jsonArray = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    JSONObject item = jsonArray.getJSONObject(i);
                    STZeRenRen stInfo = new STZeRenRen();
                    stInfo.uid= item.getLong("Uid");
                    stInfo.name = item.getString("Name");

                    arrData.add(stInfo);
                }
            }
            else
            {
                arrData = null;
            }
        } catch (Exception ex) {
            arrData = null;
        }

        return retResult;
    }

    public void GetLieCheZhangList(AsyncHttpResponseHandler handler, String bzID, String starttime)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetLieCheZhangList;
            param.put("bzID", bzID);
            param.put("starttime", starttime);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetLieCheZhangList(JSONObject jsonObject, ArrayList<STLieCheZhang> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;
        arrData.clear();

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == STServiceData.ERR_SUCCESS)
            {
                JSONArray jsonArray = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    JSONObject item = jsonArray.getJSONObject(i);
                    STLieCheZhang stInfo = new STLieCheZhang();
                    stInfo.uid= item.getLong("Uid");
                    stInfo.name = item.getString("Name");

                    arrData.add(stInfo);
                }
            }
            else
            {
                arrData = null;
            }
        } catch (Exception ex) {
            arrData = null;
        }

        return retResult;
    }

    public void GetXiangDianList(AsyncHttpResponseHandler handler, String query)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetXiangDianList;
            param.put("query", query);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetXiangDianList(JSONObject jsonObject, ArrayList<STXiangDian> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;
        arrData.clear();

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == STServiceData.ERR_SUCCESS)
            {
                JSONArray jsonArray = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    JSONObject item = jsonArray.getJSONObject(i);
                    STXiangDian stInfo = new STXiangDian();
                    stInfo.uid= item.getLong("Uid");
                    stInfo.checkno = item.getString("CheckNo");
                    stInfo.chkpoint = item.getInt("ChkPoint");
                    stInfo.relpoint = item.getString("RelPoint");
                    stInfo.category = item.getString("Category");
                    stInfo.info = item.getString("Info");

                    arrData.add(stInfo);
                }
            }
            else
            {
                arrData = null;
            }
        } catch (Exception ex) {
            arrData = null;
        }

        return retResult;
    }

    public void GetKaoHeJiLuList(AsyncHttpResponseHandler handler, String cdID, String bzID, String ryID, String dtstart, String dtend)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetKaoHeJiLuList;
            param.put("cdID", cdID);
            param.put("bzID", bzID);
            param.put("ryID", ryID);
            param.put("dtstart", dtstart);
            param.put("dtend", dtend);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetKaoHeJiLuList(JSONObject jsonObject, ArrayList<STKaoHeJiLu> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;
        arrData.clear();

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == STServiceData.ERR_SUCCESS)
            {
                JSONArray jsonArray = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    JSONObject item = jsonArray.getJSONObject(i);
                    STKaoHeJiLu stInfo = new STKaoHeJiLu();
                    stInfo.uid= item.getLong("Uid");
                    stInfo.title = item.getString("ChkTitle");
                    stInfo.name = item.getString("Name");
                    stInfo.chkdate = item.getString("ChkDate");

                    arrData.add(stInfo);
                }
            }
            else
            {
                arrData = null;
            }
        } catch (Exception ex) {
            arrData = null;
        }

        return retResult;
    }

    public void SetKaoHeItem(AsyncHttpResponseHandler handler, String uid, String starttime, String checktime,
                              String cdID, String bzID, String zrrID, String lczID, String xdID, String content, String imgdata)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdSetKaoHeItem;
            JSONObject jsonParams = new JSONObject();
            jsonParams.put("uid", uid);
            jsonParams.put("starttime", starttime);
            jsonParams.put("checktime", checktime);
            jsonParams.put("cdID", cdID);
            jsonParams.put("bzID", bzID);
            jsonParams.put("zrrID", zrrID);
            jsonParams.put("lczID", lczID);
            jsonParams.put("xdID", xdID);
            jsonParams.put("content", content);
            jsonParams.put("imgdata", imgdata);

            StringEntity entity = new StringEntity(jsonParams.toString(), "utf-8");
            client.setTimeout(STServiceData.connectTimeout);
            client.post(null, url, entity, "application/json", handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseSetKaoHeItem(JSONObject jsonObject)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
        } catch (Exception ex) {
            retResult = STServiceData.ERR_SERVER_ERROR;
        }

        return retResult;
    }

    public void GetKaoHeJiLuDet(AsyncHttpResponseHandler handler, String uid)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetKaoHeJiLuDet;
            param.put("uid", uid);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetKaoHeJiLuDet(JSONObject jsonObject, STKaoHeJiLuDet objData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;
        String strUri = "";

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            strUri = jsonObject.getString("SVCC_BASEURL");
            if (retResult == STServiceData.ERR_SUCCESS)
            {
                JSONObject obj = jsonObject.getJSONObject("SVCC_DATA");
                objData.uid = obj.getLong("Uid");
                objData.title = obj.getString("ChkTitle");
                objData.chedui = obj.getString("CheDui");
                objData.banzu = obj.getString("BanZu");
                objData.zerenren = obj.getString("ZeRenRen");
                objData.liechezhang = obj.getString("LieCheZhang");
                objData.imgpath = strUri + obj.getString("ImgPath");
                objData.chkdata = obj.getString("ChkData");
                objData.chkpoint = obj.getLong("ChkPoint");
                objData.relpoint = obj.getString("RelPoint");
            }
            else
                objData = null;
        } catch (Exception ex) {
            objData = null;
        }

        return retResult;
    }

    public void GetDaiQianItem(AsyncHttpResponseHandler handler, String uid, String pageno)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetDaiQianItem;
            param.put("uid", uid);
            param.put("pageno", pageno);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetDaiQianItem(JSONObject jsonObject, STDaiQianItem objData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;
        String strUri = "";

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            strUri = jsonObject.getString("SVCC_BASEURL");
            if (retResult == STServiceData.ERR_SUCCESS)
            {
                JSONObject obj = jsonObject.getJSONObject("SVCC_DATA");
                objData.uid = obj.getLong("Uid");
                objData.title = obj.getString("Title");
                objData.bumen = obj.getString("BuMen");
                objData.fbdate = obj.getString("FBDate");
                objData.faburen = obj.getString("FaBuRen");
                objData.tongzhihao = obj.getString("TongZhiHao");
                String strPath = obj.getString("FilePath");
                if (strPath != null && strPath.length() > 0)
                    objData.filepath = strUri + strPath;
                else
                    objData.filepath = "";
                objData.filename = obj.getString("FileName");
                objData.total = obj.getLong("Total");
                objData.content = obj.getString("Content");
                objData.receiver = obj.getString("ShouXinRen");
                objData.receiverrange = obj.getString("ShouXinBuMen");
            }
            else
                objData = null;
        } catch (Exception ex) {
            objData = null;
        }

        return retResult;
    }

    public void GetYiShouItem(AsyncHttpResponseHandler handler, String uid, String pageno)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetYiShouItem;
            param.put("uid", uid);
            param.put("pageno", pageno);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetYiShouItem(JSONObject jsonObject, STDaiQianItem objData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;
        String strUri = "";

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            strUri = jsonObject.getString("SVCC_BASEURL");
            if (retResult == STServiceData.ERR_SUCCESS)
            {
                JSONObject obj = jsonObject.getJSONObject("SVCC_DATA");
                objData.uid = obj.getLong("Uid");
                objData.title = obj.getString("Title");
                objData.bumen = obj.getString("BuMen");
                objData.fbdate = obj.getString("FBDate");
                objData.faburen = obj.getString("FaBuRen");
                objData.tongzhihao = obj.getString("TongZhiHao");
                String strPath = obj.getString("FilePath");
                if (strPath != null && strPath.length() > 0)
                    objData.filepath = strUri + obj.getString("FilePath");
                else
                    objData.filepath = "";
                objData.filename = obj.getString("FileName");
                objData.total = obj.getLong("Total");
                objData.content = obj.getString("Content");
                objData.receiver = obj.getString("ShouXinRen");
                objData.receiverrange = obj.getString("ShouXinBuMen");

                JSONArray arrayData = obj.getJSONArray("LiuZhuanRenPiShi");
                objData.liuzhuanrenpishi = new String[arrayData.length()];
                for (int i = 0; i < arrayData.length(); i++)
                {
                    JSONObject object = arrayData.getJSONObject(i);
                    objData.liuzhuanrenpishi[i] = object.getString("execnote") + "[" + object.getString("createtime") + "]" + "   " + object.getString("username");
                }
            }
            else
                objData = null;
        } catch (Exception ex) {
            objData = null;
        }

        return retResult;
    }

    public void GetAllCDRenYuanList(AsyncHttpResponseHandler handler, String cdID)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetAllCDRenYuanList;
            param.put("cdID", cdID);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetAllCDRenYuanList(JSONObject jsonObject, ArrayList<STRenYuan> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;
        arrData.clear();

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == STServiceData.ERR_SUCCESS)
            {
                JSONArray jsonArray = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    JSONObject item = jsonArray.getJSONObject(i);
                    STRenYuan stInfo = new STRenYuan();
                    stInfo.uid= item.getLong("Uid");
                    stInfo.name = item.getString("Name");

                    arrData.add(stInfo);
                }
            }
            else
            {
                arrData = null;
            }
        } catch (Exception ex) {
            arrData = null;
        }

        return retResult;
    }

    public void GetAllBMRenYuanList(AsyncHttpResponseHandler handler, String bmID)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetAllBMRenYuanList;
            param.put("bmID", bmID);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetAllBMRenYuanList(JSONObject jsonObject, ArrayList<STRenYuan> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;
        arrData.clear();

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == STServiceData.ERR_SUCCESS)
            {
                JSONArray jsonArray = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    JSONObject item = jsonArray.getJSONObject(i);
                    STRenYuan stInfo = new STRenYuan();
                    stInfo.uid= item.getLong("Uid");
                    stInfo.name = item.getString("Name");

                    arrData.add(stInfo);
                }
            }
            else
            {
                arrData = null;
            }
        } catch (Exception ex) {
            arrData = null;
        }

        return retResult;
    }

    public void SetGongWenItem(AsyncHttpResponseHandler handler, String uid, String title, String docno,
                                 String receiver, String sector, String outdate, String attach, String attachdate,
                                 String pubcontent, String attachcontent, String filename, String filedata)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdSetGongWenItem;
            JSONObject jsonParams = new JSONObject();
            jsonParams.put("uid", uid);
            jsonParams.put("title", title);
            jsonParams.put("docno", docno);
            jsonParams.put("receiver", receiver);
            jsonParams.put("sector", sector);
            jsonParams.put("outdate", outdate);
            jsonParams.put("attach", attach);
            jsonParams.put("attachdate", attachdate);
            jsonParams.put("pubcontent", pubcontent);
            jsonParams.put("attachcontent", attachcontent);
            jsonParams.put("filename", filename);
            jsonParams.put("filedata", filedata);

            StringEntity entity = new StringEntity(jsonParams.toString(), "utf-8");
            client.setTimeout(STServiceData.connectTimeout);
            client.post(null, url, entity, "application/json", handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseSetGongWenItem(JSONObject jsonObject)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            String str = jsonObject.getString("SVCC_DATA");
        } catch (Exception ex) {
            retResult = STServiceData.ERR_SERVER_ERROR;
        }

        return retResult;
    }

    public void GetGWFaBuList(AsyncHttpResponseHandler handler)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetGWFaBuList;
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, null, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetGWFaBuList(JSONObject jsonObject, ArrayList<STGWFaBu> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;
        arrData.clear();

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == STServiceData.ERR_SUCCESS)
            {
                JSONArray jsonArray = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    JSONObject item = jsonArray.getJSONObject(i);
                    STGWFaBu stInfo = new STGWFaBu();
                    stInfo.name = item.getString("Name");

                    arrData.add(stInfo);
                }
            }
            else
            {
                arrData = null;
            }
        } catch (Exception ex) {
            arrData = null;
        }

        return retResult;
    }

    public void GetGongWenList(AsyncHttpResponseHandler handler, String bumen, String startdate, String enddate, String query, long userid)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetGongWenList;
            param.put("bumen", bumen);
            param.put("startdate", startdate);
            param.put("enddate", enddate);
            param.put("query", query);
            param.put("userid", userid + "");
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetGongWenList(JSONObject jsonObject, ArrayList<STDaiQianItem> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;
        String strUri = "";

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            strUri = jsonObject.getString("SVCC_BASEURL");
            if (retResult == STServiceData.ERR_SUCCESS)
            {
                JSONArray jsonArray = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    JSONObject obj = jsonArray.getJSONObject(i);
                    STDaiQianItem objData = new STDaiQianItem();
                    objData.uid = obj.getLong("Uid");
                    objData.title = obj.getString("Title");
                    objData.bumen = obj.getString("BuMen");
                    objData.fbdate = obj.getString("FBDate");
                    objData.faburen = obj.getString("FaBuRen");
                    objData.tongzhihao = obj.getString("TongZhiHao");
                    objData.filepath = strUri + obj.getString("FilePath");
                    objData.filename = obj.getString("FileName");
                    objData.total = obj.getLong("Total");
                    objData.content = obj.getString("Content");

                    arrData.add(objData);
                }
            }
            else
                arrData = null;
        } catch (Exception ex) {
            arrData = null;
        }

        return retResult;
    }

    public void GetKaoShiList(AsyncHttpResponseHandler handler, String pid)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetKaoShiList;
            param.put("pid", pid);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetKaoShiList(JSONObject jsonObject, ArrayList<STKaoShi> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;
        arrData.clear();

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == STServiceData.ERR_SUCCESS)
            {
                JSONArray jsonArray = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    JSONObject item = jsonArray.getJSONObject(i);
                    STKaoShi stInfo = new STKaoShi();
                    stInfo.id = item.getLong("id");
                    stInfo.examtype = item.getString("examtype");
                    stInfo.title = item.getString("title");
                    stInfo.questions = new ArrayList<STKaoShi.STQuestion>();
                    {
                        JSONArray jsonArray1 = item.getJSONArray("question");
                        for ( int j=0; j<jsonArray1.length(); j++ ) {
                            JSONObject item1 = jsonArray1.getJSONObject(j);
                            STKaoShi.STQuestion stQuestion = stInfo.new STQuestion();
                            stQuestion.ind = item1.getString("ind");
                            stQuestion.question = item1.getString("question");
                            stInfo.questions.add(stQuestion);
                        }
                    }
                    stInfo.answers = new ArrayList<String>();
                    {
                        JSONArray jsonArray2 = item.getJSONArray("answer");
                        for ( int k=0; k<jsonArray2.length(); k++ ) {
                           stInfo.answers.add(jsonArray2.getString(k));
                        }
                    }

                    arrData.add(stInfo);
                }
            }
            else
            {
                arrData = null;
            }
        } catch (Exception ex) {
            arrData = null;
        }

        return retResult;
    }

    public void GetKaoShiLiShiList(AsyncHttpResponseHandler handler, String uid)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetKaoShiLiShiList;
            param.put("uid", uid);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetKaoShiLiShiList(JSONObject jsonObject, ArrayList<STKaoShiLiShi> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;
        arrData.clear();

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == STServiceData.ERR_SUCCESS)
            {
                JSONArray jsonArray = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    JSONObject item = jsonArray.getJSONObject(i);
                    STKaoShiLiShi stInfo = new STKaoShiLiShi();
                    stInfo.uid= item.getLong("Uid");
                    stInfo.title = item.getString("Title");
                    stInfo.examdate = item.getString("ExamDate");
                    stInfo.total = item.getInt("Total");
                    stInfo.rightans = item.getInt("RightAns");
                    stInfo.mark = item.getDouble("Mark");

                    arrData.add(stInfo);
                }
            }
            else
            {
                arrData = null;
            }
        } catch (Exception ex) {
            arrData = null;
        }

        return retResult;
    }

    public void GetKaoShiWenTiList(AsyncHttpResponseHandler handler, String uid, String kind)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetKaoShiWenTiList;
            param.put("uid", uid);
            param.put("kind", kind);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetKaoShiWenTiList(JSONObject jsonObject, ArrayList<STKaoShiItem> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;
        arrData.clear();

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == STServiceData.ERR_SUCCESS)
            {
                JSONArray jsonArray = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    JSONObject item = jsonArray.getJSONObject(i);
                    STKaoShiItem stInfo = new STKaoShiItem();
                    stInfo.uid= item.getLong("Uid");
                    stInfo.title = item.getString("Title");
                    stInfo.examtime = item.getInt("ExamTime");
                    stInfo.problems = item.getInt("Problems");
                    stInfo.content = item.getString("Content");
                    stInfo.isexam = item.getInt("IsExam");

                    if (stInfo.isexam == 0)
                        arrData.add(stInfo);
                }
            }
            else
            {
                arrData = null;
            }
        } catch (Exception ex) {
            arrData = null;
        }

        return retResult;
    }

    public void GetKaoHeChartData(AsyncHttpResponseHandler handler, String teamid, String groupid, String starttime, String endtime)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetKaoHeChartData;
            param.put("teamid", teamid);
            param.put("groupid", groupid);
            param.put("starttime", starttime);
            param.put("endtime", endtime);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetKaoHeChartData(JSONObject jsonObject, ArrayList<STKaoHeTongJi> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == STServiceData.ERR_SUCCESS)
            {
                JSONArray jsonArray = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    JSONObject obj = jsonArray.getJSONObject(i);
                    STKaoHeTongJi objData = new STKaoHeTongJi();
                    objData.category = obj.getString("category");
                    objData.count = obj.getLong("count");

                    arrData.add(objData);
                }
            }
            else
                arrData = null;
        } catch (Exception ex) {
            arrData = null;
        }

        return retResult;
    }

    public void SetKaoShiJieGuo(AsyncHttpResponseHandler handler, String uid, String eid, String score,
                             String examres, String examsecond)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdSetKaoShiJieGuo;
            JSONObject jsonParams = new JSONObject();
            jsonParams.put("uid", uid);
            jsonParams.put("eid", eid);
            jsonParams.put("score", score);
            jsonParams.put("examres", examres);
            jsonParams.put("examsecond", examsecond);

            StringEntity entity = new StringEntity(jsonParams.toString(), "utf-8");
            client.setTimeout(STServiceData.connectTimeout);
            client.post(null, url, entity, "application/json", handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseSetKaoShiJieGuo(JSONObject jsonObject)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
        } catch (Exception ex) {
            retResult = STServiceData.ERR_SERVER_ERROR;
        }

        return retResult;
    }

    public void SetLiuZhuanData(AsyncHttpResponseHandler handler, String uid, String docid, String content, String receiver)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdSetLiuZhuanData;
            param.put("uid", uid);
            param.put("docid", docid);
            param.put("content", content);
            param.put("receiver", receiver);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseSetLiuZhuanData(JSONObject jsonObject)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
        } catch (Exception ex) {
            retResult = STServiceData.ERR_SERVER_ERROR;
        }

        return retResult;
    }

    public void SetQianShouData(AsyncHttpResponseHandler handler, String uid, String docid, String content, String receiver)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdSetQianShouData;
            param.put("uid", uid);
            param.put("docid", docid);
            param.put("content", content);
            param.put("receiver", receiver);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseSetQianShouData(JSONObject jsonObject)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
        } catch (Exception ex) {
            retResult = STServiceData.ERR_SERVER_ERROR;
        }

        return retResult;
    }

    public void SetTaskItem(AsyncHttpResponseHandler handler, String uid, String title, String receiver, String starttime,
                            String endtime, String contents, String filename, String filedata)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdSetTaskItem;
            JSONObject jsonParams = new JSONObject();
            jsonParams.put("uid", uid);
            jsonParams.put("title", title);
            jsonParams.put("receiver", receiver);
            jsonParams.put("starttime", starttime);
            jsonParams.put("endtime", endtime);
            jsonParams.put("contents", contents);
            jsonParams.put("filename", filename);
            jsonParams.put("filedata", filedata);

            StringEntity entity = new StringEntity(jsonParams.toString(), "utf-8");
            client.setTimeout(STServiceData.connectTimeout);
            client.post(null, url, entity, "application/json", handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseSetTaskItem(JSONObject jsonObject)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
        } catch (Exception ex) {
            retResult = STServiceData.ERR_SERVER_ERROR;
        }

        return retResult;
    }

    public void GetMySendTaskList(AsyncHttpResponseHandler handler, String uid, String pageno)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetMySendTaskList;
            param.put("uid", uid);
            param.put("pageno", pageno);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetMySendTaskList(JSONObject jsonObject, ArrayList<STRenWuItem> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == 0)
            {
                JSONArray arr = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < arr.length(); i++)
                {
                    JSONObject object = arr.getJSONObject(i);
                    STRenWuItem newItem = new STRenWuItem();
                    newItem.uid = object.getLong("uid");
                    newItem.title = object.getString("title");
                    newItem.executors = object.getString("receivernames");
                    newItem.state = object.getString("statusstr");

                    arrData.add(newItem);
                }
            }
        } catch (Exception ex) {
            retResult = STServiceData.ERR_SERVER_ERROR;
            arrData = new ArrayList<STRenWuItem>();
        }

        return retResult;
    }

    public void GetMyTaskList(AsyncHttpResponseHandler handler, String uid, String pageno)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetMyTaskList;
            param.put("uid", uid);
            param.put("pageno", pageno);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetMyTaskList(JSONObject jsonObject, ArrayList<STRenWuJieDaoItem> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == 0)
            {
                JSONArray arr = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < arr.length(); i++)
                {
                    JSONObject object = arr.getJSONObject(i);
                    STRenWuJieDaoItem newItem = new STRenWuJieDaoItem();
                    newItem.uid = object.getLong("uid");
                    newItem.title = object.getString("title");
                    newItem.starttime = object.getString("starttime_desc");
                    newItem.endtime = object.getString("endtime_desc");
                    newItem.state = object.getString("statusstr");
                    newItem.status = object.getInt("status");
                    String strFile = object.getString("attachname");
                    if (strFile.length() == 0)
                        newItem.existfile = "";
                    else
                        newItem.existfile = "有附件";

                    arrData.add(newItem);
                }
            }
        } catch (Exception ex) {
            retResult = STServiceData.ERR_SERVER_ERROR;
            arrData = new ArrayList<STRenWuJieDaoItem>();
        }

        return retResult;
    }

    public void GetTaskDetInfo(AsyncHttpResponseHandler handler, String uid, String taskid)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetTaskDetInfo;
            param.put("uid", uid);
            param.put("taskid", taskid);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetTaskDetInfo(JSONObject jsonObject, STTaskDetInfo objData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;
        String strURL = "";

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            strURL = jsonObject.getString("SVCC_BASEURL");
            if (retResult == 0)
            {
                JSONObject object = jsonObject.getJSONObject("SVCC_DATA");
                objData.uid = object.getLong("uid");
                objData.taskstatusid = object.getLong("taskstatusid");
                objData.title = object.getString("title");
                objData.startdate = object.getString("startdate");
                objData.enddate = object.getString("enddate");
                objData.state = object.getInt("state");
                objData.content = object.getString("content");
                objData.filename = object.getString("filename");
                objData.filepath = strURL+ object.getString("filepath");

                JSONArray array = object.getJSONArray("tasklog");
                if (array != null && array.length() > 0)
                {
                    for (int i = 0; i < array.length(); i++)
                    {
                        STTaskLog newLog = new STTaskLog();
                        JSONObject obj = array.getJSONObject(i);
                        newLog.postdate = obj.getString("postdate");
                        newLog.name = obj.getString("name");
                        newLog.content = obj.getString("content");

                        objData.tasklog.add(newLog);
                    }
                }
            }
        } catch (Exception ex) {
            retResult = STServiceData.ERR_SERVER_ERROR;
            objData = null;
        }

        return retResult;
    }

    public void SetTaskRunned(AsyncHttpResponseHandler handler, String uid, String taskid)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdSetTaskRunned;
            param.put("uid", uid);
            param.put("taskid", taskid);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseSetTaskRunned(JSONObject jsonObject)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
        } catch (Exception ex) {
        }

        return retResult;
    }

    public void SetTaskCompleted(AsyncHttpResponseHandler handler, String uid, String taskid)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdSetTaskCompleted;
            param.put("uid", uid);
            param.put("taskid", taskid);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseSetTaskCompleted(JSONObject jsonObject)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
        } catch (Exception ex) {
        }

        return retResult;
    }

    public void SetTaskLog(AsyncHttpResponseHandler handler, String uid, String taskid, String contents)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdSetTaskLog;
            JSONObject jsonParams = new JSONObject();
            jsonParams.put("uid", uid);
            jsonParams.put("taskid", taskid);
            jsonParams.put("contents", contents);

            StringEntity entity = new StringEntity(jsonParams.toString(), "utf-8");
            client.setTimeout(STServiceData.connectTimeout);
            client.post(null, url, entity, "application/json", handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseSetTaskLog(JSONObject jsonObject)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
        } catch (Exception ex) {
        }

        return retResult;
    }

    public void GetContactList(AsyncHttpResponseHandler handler, String contactkind, String rolename)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetContactList;
            param.put("contactkind", contactkind);
            param.put("rolename", rolename);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetContactList(JSONObject jsonObject, ArrayList<STContactInfo> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == 0)
            {
                JSONArray arr = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < arr.length(); i++)
                {
                    JSONObject object = arr.getJSONObject(i);
                    STContactInfo newItem = new STContactInfo();
                    newItem.name = object.getString("name");
                    newItem.partname = object.getString("partname");
                    newItem.groupname = object.getString("groupname");
                    newItem.rolename = object.getString("rolename");
                    newItem.rolekind = object.getString("rolekind");
                    newItem.trainno = object.getString("trainno");
                    newItem.phonenum1 = object.getString("phonenum1");
                    newItem.phonenum2 = object.getString("phonenum2");
                    newItem.sortnum1 = object.getString("sortnum1");
                    newItem.sortnum2 = object.getString("sortnum2");
                    newItem.linenum = object.getString("linenum");

                    arrData.add(newItem);
                }
            }
        } catch (Exception ex) {
            retResult = STServiceData.ERR_SERVER_ERROR;
            arrData = new ArrayList<STContactInfo>();
        }

        return retResult;
    }

    public void GetEmailList(AsyncHttpResponseHandler handler, String uid)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetEmailList;
            param.put("uid", uid);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetEmailList(JSONObject jsonObject, ArrayList<STEmailInfo> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == 0)
            {
                JSONArray arr = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < arr.length(); i++)
                {
                    JSONObject object = arr.getJSONObject(i);
                    STEmailInfo newItem = new STEmailInfo();
                    newItem.uid = object.getLong("uid");
                    newItem.title = object.getString("title");
                    newItem.isread = object.getInt("isread");
                    newItem.postdate = object.getString("postdate");
                    newItem.content = object.getString("content");
                    newItem.filename = object.getString("filename");
                    newItem.filepath = object.getString("filepath");
                    newItem.sender = object.getString("sender");

                    arrData.add(newItem);
                }
            }
        } catch (Exception ex) {
            retResult = STServiceData.ERR_SERVER_ERROR;
            arrData = new ArrayList<STEmailInfo>();
        }

        return retResult;
    }

    public void GetSentEmailList(AsyncHttpResponseHandler handler, String uid)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetSentEmailList;
            param.put("uid", uid);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetSentEmailList(JSONObject jsonObject, ArrayList<STEmailInfo> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == 0)
            {
                JSONArray arr = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < arr.length(); i++)
                {
                    JSONObject object = arr.getJSONObject(i);
                    STEmailInfo newItem = new STEmailInfo();
                    newItem.uid = object.getLong("uid");
                    newItem.title = object.getString("title");
                    newItem.isread = object.getInt("isread");
                    newItem.postdate = object.getString("postdate");
                    newItem.content = object.getString("content");
                    newItem.filename = object.getString("filename");
                    newItem.filepath = object.getString("filepath");
                    newItem.sender = object.getString("sender");

                    arrData.add(newItem);
                }
            }
        } catch (Exception ex) {
            retResult = STServiceData.ERR_SERVER_ERROR;
            arrData = new ArrayList<STEmailInfo>();
        }

        return retResult;
    }

    public void SendReceipt(AsyncHttpResponseHandler handler, String uid, String mailid)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdSendReceipt;
            RequestParams param = new RequestParams();
            param.put("uid", uid);
            param.put("mailid", mailid);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseSendReceipt(JSONObject jsonObject)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
        } catch (Exception ex) {
        }

        return retResult;
    }

    public void GetEmailDetInfo(AsyncHttpResponseHandler handler, String uid, String emailid)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetEmailDetInfo;
            param.put("uid", uid);
            param.put("emailid", emailid);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetEmailDetInfo(JSONObject jsonObject, STEmailInfo arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            String strURL = jsonObject.getString("SVCC_BASEURL");
            if (retResult == 0)
            {
                JSONObject object = jsonObject.getJSONObject("SVCC_DATA");
                arrData.uid = object.getLong("uid");
                arrData.sender = object.getString("sender");
                arrData.senderid = object.getLong("senderid");
                arrData.title = object.getString("title");
                arrData.isread = object.getInt("isread");
                arrData.isreceipt = object.getInt("isreceipt");
                arrData.postdate = object.getString("postdate");
                arrData.content = object.getString("content");
                arrData.filename = object.getString("filename");
                arrData.filepath = strURL + URLEncoder.encode(object.getString("filepath"), "UTF-8");
            }
        } catch (Exception ex) {
            retResult = STServiceData.ERR_SERVER_ERROR;
            arrData = new STEmailInfo();
        }

        return retResult;
    }

    public void GetAllUserList(AsyncHttpResponseHandler handler, String keyword, String pageno)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetAllUserList;
            param.put("keyword", keyword);
            param.put("pageno", pageno);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetAllUserList(JSONObject jsonObject, ArrayList<STEmailUser> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == 0)
            {
                JSONArray arr = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < arr.length(); i++)
                {
                    JSONObject object = arr.getJSONObject(i);
                    STEmailUser newItem = new STEmailUser();
                    newItem.uid = object.getLong("uid");
                    newItem.name = object.getString("realname");

                    arrData.add(newItem);
                }
            }
        } catch (Exception ex) {
            retResult = STServiceData.ERR_SERVER_ERROR;
            arrData = new ArrayList<STEmailUser>();
        }

        return retResult;
    }

    public void SetEmail(AsyncHttpResponseHandler handler, String uid, String receiver, String title,
                             String content, String filename, String filedata)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdSetEmail;
            JSONObject jsonParams = new JSONObject();
            jsonParams.put("uid", uid);
            jsonParams.put("receiver", receiver);
            jsonParams.put("title", title);
            jsonParams.put("content", content);
            jsonParams.put("filename", filename);
            jsonParams.put("filedata", filedata);

            StringEntity entity = new StringEntity(jsonParams.toString(), "utf-8");
            client.setTimeout(STServiceData.connectTimeout);
            client.post(null, url, entity, "application/json", handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseSetEmail(JSONObject jsonObject)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
        } catch (Exception ex) {
            retResult = STServiceData.ERR_SERVER_ERROR;
        }

        return retResult;
    }

    public void SetReply(AsyncHttpResponseHandler handler, String uid, String receiver, String title,
                         String content, String filename, String filedata, String yid)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdSetReply;
            JSONObject jsonParams = new JSONObject();
            jsonParams.put("uid", uid);
            jsonParams.put("receiver", receiver);
            jsonParams.put("title", title);
            jsonParams.put("content", content);
            jsonParams.put("filename", filename);
            jsonParams.put("filedata", filedata);
            jsonParams.put("yid", yid);

            StringEntity entity = new StringEntity(jsonParams.toString(), "utf-8");
            client.setTimeout(STServiceData.connectTimeout);
            client.post(null, url, entity, "application/json", handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseSetReply(JSONObject jsonObject)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
        } catch (Exception ex) {
            retResult = STServiceData.ERR_SERVER_ERROR;
        }

        return retResult;
    }

    public void SetReadedState(AsyncHttpResponseHandler handler, String uid, String emailid)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdSetReadedState;
            param.put("uid", uid);
            param.put("emailid", emailid);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseSetReadedState(JSONObject jsonObject)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
        } catch (Exception ex) {
        }

        return retResult;
    }

    public void SetOpinion(AsyncHttpResponseHandler handler, String uid, String kind, String title, String content)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdSetOpinion;
            JSONObject jsonParams = new JSONObject();
            jsonParams.put("uid", uid);
            jsonParams.put("kind", kind);
            jsonParams.put("title", title);
            jsonParams.put("content", content);

            StringEntity entity = new StringEntity(jsonParams.toString(), "utf-8");
            client.setTimeout(STServiceData.connectTimeout);
            client.post(null, url, entity, "application/json", handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseSetOpinion(JSONObject jsonObject)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
        } catch (Exception ex) {
            retResult = STServiceData.ERR_SERVER_ERROR;
        }

        return retResult;
    }

    public void GetOpinionList(AsyncHttpResponseHandler handler, long uid)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetOpinionList;
            param.put("uid", "" + uid);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetOpinionList(JSONObject jsonObject, ArrayList<STOpinionData> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            String strURL = jsonObject.getString("SVCC_BASEURL");
            if (retResult == 0)
            {
                JSONArray arr = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < arr.length(); i++)
                {
                    JSONObject object = arr.getJSONObject(i);
                    STOpinionData newItem = new STOpinionData();
                    newItem.uid = object.getInt("uid");
                    newItem.postdate = object.getString("postdate");
                    newItem.feedback = object.getString("feedback");
                    newItem.title = object.getString("title");
                    newItem.content = object.getString("content");

                    arrData.add(newItem);
                }
            }
        } catch (Exception ex) {
            retResult = STServiceData.ERR_SERVER_ERROR;
            arrData = new ArrayList<STOpinionData>();
        }

        return retResult;
    }

    public void GetRuleList(AsyncHttpResponseHandler handler)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetRuleList;
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetRuleList(JSONObject jsonObject, ArrayList<STPdfFileData> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            String strURL = jsonObject.getString("SVCC_BASEURL");
            if (retResult == 0)
            {
                JSONArray arr = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < arr.length(); i++)
                {
                    JSONObject object = arr.getJSONObject(i);
                    STPdfFileData newItem = new STPdfFileData();
                    newItem.pdfid = object.getInt("uid");
                    newItem.iconpath = strURL + object.getString("path");
                    newItem.remotepath = strURL + object.getString("pdfpath");

                    arrData.add(newItem);
                }
            }
        } catch (Exception ex) {
            retResult = STServiceData.ERR_SERVER_ERROR;
            arrData = new ArrayList<STPdfFileData>();
        }

        return retResult;
    }

    public void GetDetRule(AsyncHttpResponseHandler handler, String ruleid)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetDetRule;
            param.put("ruleid", ruleid);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetDetRule(JSONObject jsonObject, STDetRule arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            String strURL = jsonObject.getString("SVCC_BASEURL");
            if (retResult == 0)
            {
                JSONObject object = jsonObject.getJSONObject("SVCC_DATA");
                arrData.uid = object.getLong("uid");
                arrData.title = object.getString("title");
                arrData.content = object.getString("content");
            }
        } catch (Exception ex) {
            retResult = STServiceData.ERR_SERVER_ERROR;
            arrData = new STDetRule();
        }

        return retResult;
    }

    public void GetRoutes(AsyncHttpResponseHandler handler)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetRoutes;
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, null, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetRoutes(JSONObject jsonObject, ArrayList<STRoute> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;
        arrData.clear();

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == STServiceData.ERR_SUCCESS)
            {
                JSONArray jsonArray = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    JSONObject item = jsonArray.getJSONObject(i);
                    STRoute stInfo = new STRoute();
                    stInfo.uid= item.getLong("Uid");
                    stInfo.name = item.getString("Name");

                    arrData.add(stInfo);
                }
            }
            else
            {
                arrData = null;
            }
        } catch (Exception ex) {
            arrData = null;
        }

        return retResult;
    }

    public void GetCreditList(AsyncHttpResponseHandler handler, String findtime, String name, String sectorid, String routeid)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams params = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetCreditList;
            params.put("findtime", findtime);
            params.put("name", name);
            params.put("sectorid", sectorid);
            params.put("routeid", routeid);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, params, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetCreditList(JSONObject jsonObject, ArrayList<STCreditInfo> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;
        arrData.clear();

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == STServiceData.ERR_SUCCESS)
            {
                JSONArray jsonArray = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    JSONObject item = jsonArray.getJSONObject(i);
                    STCreditInfo stInfo = new STCreditInfo();
                    stInfo.uid= item.getLong("Uid");
                    stInfo.chedui = item.getString("CheDui");
                    stInfo.gongzihao = item.getString("GongZiHao");
                    stInfo.name = item.getString("Name");
                    stInfo.banzu = item.getString("BanZu");
                    stInfo.duanjiyishang = item.getDouble("DuanJiYiShang");
                    stInfo.banzuchedui = item.getDouble("BanZuCheDui");
                    stInfo.liangua = item.getDouble("LianGua");
                    stInfo.ligang = item.getDouble("LiGang");
                    stInfo.diaozheng = item.getDouble("DiaoZheng");
                    stInfo.benyue = item.getDouble("BenYue");
                    stInfo.jili = item.getDouble("JiLi");
                    stInfo.leiji = item.getDouble("LeiJi");

                    arrData.add(stInfo);
                }
            }
            else
            {
                arrData = null;
            }
        } catch (Exception ex) {
            arrData = null;
        }

        return retResult;
    }

    public void GetSelfCheckList(AsyncHttpResponseHandler handler, String findtime,String sectorid)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams params = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetSelfCheckList;
            params.put("findtime", findtime);
            params.put("sectorid", sectorid);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, params, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetSelfCheckList(JSONObject jsonObject, ArrayList<STSelfCheck> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;
        arrData.clear();

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == STServiceData.ERR_SUCCESS)
            {
                JSONArray jsonArray = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    JSONObject item = jsonArray.getJSONObject(i);
                    STSelfCheck stInfo = new STSelfCheck();
                    stInfo.groupname= item.getString("groupname");
                    stInfo.teamname = item.getString("teamname");
                    stInfo.one = item.getInt("one");
                    stInfo.two = item.getInt("two");
                    stInfo.three = item.getInt("three");
                    stInfo.four = item.getInt("four");
                    stInfo.five = item.getInt("five");
                    stInfo.six = item.getInt("six");
                    stInfo.seven = item.getInt("seven");
                    stInfo.eight = item.getInt("eight");

                    arrData.add(stInfo);
                }
            }
            else
            {
                arrData = null;
            }
        } catch (Exception ex) {
            arrData = null;
        }

        return retResult;
    }

    public void GetCombineCheckList(AsyncHttpResponseHandler handler, String findtime, String sectorid)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams params = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetCombineCheckList;
            params.put("findtime", findtime);
            params.put("sectorid", sectorid);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, params, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetCombineCheckList(JSONObject jsonObject, ArrayList<STCombineCheckInfo> arrData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;
        arrData.clear();

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            if (retResult == STServiceData.ERR_SUCCESS)
            {
                JSONArray jsonArray = jsonObject.getJSONArray("SVCC_DATA");
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    JSONObject item = jsonArray.getJSONObject(i);

                    STCombineCheckInfo stInfo = new STCombineCheckInfo();
                    stInfo.CheDui = item.getString("CheDui");
                    stInfo.BanZu = item.getString("BanZu");
                    stInfo.oneval = item.getInt("oneval");
                    stInfo.twoval = item.getInt("twoval");
                    stInfo.threeval = item.getInt("threeval");

                    arrData.add(stInfo);
                }
            }
            else
            {
                arrData = null;
            }
        } catch (Exception ex) {
            arrData = null;
        }

        return retResult;
    }

    public void GetNewsList(AsyncHttpResponseHandler handler, long uid)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetNewsList;
            param.put("uid", "" + uid);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetNewsList(JSONObject jsonObject, STNewsList listData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            String strURL = jsonObject.getString("SVCC_BASEURL");
            if (retResult == 0)
            {
                JSONObject object = jsonObject.getJSONObject("SVCC_DATA");

                JSONArray objGongWen = object.getJSONArray("gongwen");
                for (int i = 0; i < objGongWen.length(); i++)
                {
                    JSONObject item = objGongWen.getJSONObject(i);

                    STNewsItem stInfo = new STNewsItem();
                    stInfo.uid = item.getLong("uid");
                    stInfo.mode = item.getInt("mode");
                    stInfo.title = item.getString("title");

                    listData.arrGongWen.add(stInfo);
                }

                JSONArray objEmail = object.getJSONArray("email");
                for (int i = 0; i < objEmail.length(); i++)
                {
                    JSONObject item = objEmail.getJSONObject(i);

                    STNewsItem stInfo = new STNewsItem();
                    stInfo.uid = item.getLong("uid");
                    stInfo.mode = item.getInt("mode");
                    stInfo.title = item.getString("title");

                    listData.arrEmail.add(stInfo);
                }

                JSONArray objTask = object.getJSONArray("task");
                for (int i = 0; i < objTask.length(); i++)
                {
                    JSONObject item = objTask.getJSONObject(i);

                    STNewsItem stInfo = new STNewsItem();
                    stInfo.uid = item.getLong("uid");
                    stInfo.mode = item.getInt("mode");
                    stInfo.title = item.getString("title");

                    listData.arrTask.add(stInfo);
                }
            }
        } catch (Exception ex) {
            retResult = STServiceData.ERR_SERVER_ERROR;
            listData = new STNewsList();
        }

        return retResult;
    }

    public void GetDaiQianDetail(AsyncHttpResponseHandler handler, long uid, long docid)
    {
        String url = "";
        AsyncHttpClient client = new AsyncHttpClient();
        RequestParams param = new RequestParams();

        try {
            url = STServiceData.serviceAddr + STServiceData.cmdGetDaiQianDetail;
            param.put("uid", "" + uid);
            param.put("docid", "" + docid);
            client.setTimeout(STServiceData.connectTimeout);
            client.get(null, url, param, handler);
        }
        catch (Exception e) {
            e.printStackTrace();
        }
    }

    public long parseGetDaiQianDetail(JSONObject jsonObject, STDaiQianItem objData)
    {
        int retResult = STServiceData.ERR_SERVER_ERROR;
        String strUri = "";

        try {
            retResult = jsonObject.getInt("SVCC_RETVAL");
            strUri = jsonObject.getString("SVCC_BASEURL");
            if (retResult == STServiceData.ERR_SUCCESS)
            {
                JSONObject obj = jsonObject.getJSONObject("SVCC_DATA");
                objData.uid = obj.getLong("Uid");
                objData.title = obj.getString("Title");
                objData.bumen = obj.getString("BuMen");
                objData.fbdate = obj.getString("FBDate");
                objData.faburen = obj.getString("FaBuRen");
                objData.tongzhihao = obj.getString("TongZhiHao");
                String  strPath = obj.getString("FilePath");
                if (strPath != null && strPath.length() > 0)
                    objData.filepath = strUri + obj.getString("FilePath");
                else
                    objData.filepath = "";
                objData.filename = obj.getString("FileName");
                objData.total = obj.getLong("Total");
                objData.content = obj.getString("Content");
                objData.receiver = obj.getString("ShouXinRen");
                objData.receiverrange = obj.getString("ShouXinBuMen");

                JSONArray arrayData = obj.getJSONArray("LiuZhuanRenPiShi");
                objData.liuzhuanrenpishi = new String[arrayData.length()];
                for (int i = 0; i < arrayData.length(); i++)
                {
                    JSONObject object = arrayData.getJSONObject(i);
                    objData.liuzhuanrenpishi[i] = object.getString("execnote") + "[" + object.getString("createtime") + "]" + "   " + object.getString("username");
                }
            }
            else
                objData = null;
        } catch (Exception ex) {
            objData = null;
        }

        return retResult;
    }
}
