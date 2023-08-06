package com.damytech.utils;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.content.pm.ResolveInfo;
import android.database.Cursor;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.net.Uri;
import android.provider.MediaStore;
import android.view.View;
import android.widget.Toast;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.List;
import java.util.regex.Pattern;

public class GlobalData
{
    public static String g_strPrefName = "TieLu";
    public static String g_strUid = "Uid";
    public static String g_strGrade = "Grade";
    public static String g_strPassFlag = "PassFlag";
    public static String g_strOfflineUser = "OfflineUser";
    public static String g_strUserName = "UserName";
    public static String g_strPass = "Pass";
    public static String g_strGongWen = "GongWen";
    public static String g_strEmail = "Email";
    public static String g_strTask = "Task";

    public static boolean getExistGongWenID(Context context, long id)
    {
        try {
            SharedPreferences pref = context.getSharedPreferences(g_strPrefName, Context.MODE_PRIVATE);
            String strBannerList = pref.getString(g_strGongWen, "");
            String temp = strBannerList;

            while (temp.length() > 0)
            {
                try
                {
                    int nPos = temp.indexOf(',', 0);
                    if (nPos < 0 || nPos == 0)
                    {
                        return true;
                    }
                    String strVal = temp.substring(0, nPos);
                    long nOldID = Long.parseLong(strVal);
                    if (nOldID == id)
                    {
                        return true;
                    }
                    temp = temp.substring(nPos + 1);
                } catch (Exception ex) {
                    temp = "";
                }
            }

            SharedPreferences.Editor editor = pref.edit();
            editor.putString(g_strGongWen, strBannerList + Long.toString(id) + ",");
            editor.commit();

            return false;
        } catch (Exception ex) {
            return false;
        }
    }

    public static boolean getExistEmailID(Context context, long id)
    {
        try {
            SharedPreferences pref = context.getSharedPreferences(g_strPrefName, Context.MODE_PRIVATE);
            String strBannerList = pref.getString(g_strEmail, "");
            String temp = strBannerList;

            while (temp.length() > 0)
            {
                try
                {
                    int nPos = temp.indexOf(',', 0);
                    if (nPos < 0 || nPos == 0)
                    {
                        return true;
                    }
                    String strVal = temp.substring(0, nPos);
                    long nOldID = Long.parseLong(strVal);
                    if (nOldID == id)
                    {
                        return true;
                    }
                    temp = temp.substring(nPos + 1);
                } catch (Exception ex) {
                    temp = "";
                }
            }

            SharedPreferences.Editor editor = pref.edit();
            editor.putString(g_strEmail, strBannerList + Long.toString(id) + ",");
            editor.commit();

            return false;
        } catch (Exception ex) {
            return false;
        }
    }

    public static boolean getExistTaskID(Context context, long id)
    {
        try {
            SharedPreferences pref = context.getSharedPreferences(g_strPrefName, Context.MODE_PRIVATE);
            String strBannerList = pref.getString(g_strTask, "");
            String temp = strBannerList;

            while (temp.length() > 0)
            {
                try
                {
                    int nPos = temp.indexOf(',', 0);
                    if (nPos < 0 || nPos == 0)
                    {
                        return true;
                    }
                    String strVal = temp.substring(0, nPos);
                    long nOldID = Long.parseLong(strVal);
                    if (nOldID == id)
                    {
                        return true;
                    }
                    temp = temp.substring(nPos + 1);
                } catch (Exception ex) {
                    temp = "";
                }
            }

            SharedPreferences.Editor editor = pref.edit();
            editor.putString(g_strTask, strBannerList + Long.toString(id) + ",");
            editor.commit();

            return false;
        } catch (Exception ex) {
            return false;
        }
    }

    public static long GetUid(Context context)
    {
        long nID = 0;

        SharedPreferences pref = context.getSharedPreferences(g_strPrefName, Context.MODE_PRIVATE);
        nID = pref.getLong(g_strUid, 0);

        return nID;
    }

    public static void SetUid(Context context, long nID)
    {
        SharedPreferences pref = context.getSharedPreferences(g_strPrefName, Context.MODE_PRIVATE);
        SharedPreferences.Editor editor = pref.edit();
        editor.putLong(g_strUid, nID);
        editor.commit();

        return;
    }

    public static long GetGrade(Context context)
    {
        long nID = 0;

        SharedPreferences pref = context.getSharedPreferences(g_strPrefName, Context.MODE_PRIVATE);
        nID = pref.getLong(g_strGrade, -1);

        return nID;
    }

    public static void SetGrade(Context context, long nID)
    {
        SharedPreferences pref = context.getSharedPreferences(g_strPrefName, Context.MODE_PRIVATE);
        SharedPreferences.Editor editor = pref.edit();
        editor.putLong(g_strGrade, nID);
        editor.commit();

        return;
    }

    public static boolean GetPassFlag(Context context)
    {
        boolean bRet = true;

        SharedPreferences pref = context.getSharedPreferences(g_strPrefName, Context.MODE_PRIVATE);
        bRet = pref.getBoolean(g_strPassFlag, false);

        return bRet;
    }

    public static void SetPassFlag(Context context, boolean bFlag)
    {
        SharedPreferences pref = context.getSharedPreferences(g_strPrefName, Context.MODE_PRIVATE);
        SharedPreferences.Editor editor = pref.edit();
        editor.putBoolean(g_strPassFlag, bFlag);
        editor.commit();

        return;
    }

    public static boolean GetOfflineUser(Context context)
    {
        boolean bRet = true;

        SharedPreferences pref = context.getSharedPreferences(g_strPrefName, Context.MODE_PRIVATE);
        bRet = pref.getBoolean(g_strOfflineUser, false);

        return bRet;
    }

    public static void SetOfflineUser(Context context, boolean bFlag)
    {
        SharedPreferences pref = context.getSharedPreferences(g_strPrefName, Context.MODE_PRIVATE);
        SharedPreferences.Editor editor = pref.edit();
        editor.putBoolean(g_strOfflineUser, bFlag);
        editor.commit();

        return;
    }

    public static String GetUserName(Context context)
    {
        String strUserName = "";

        SharedPreferences pref = context.getSharedPreferences(g_strPrefName, Context.MODE_PRIVATE);
        strUserName = pref.getString(g_strUserName, "");

        return strUserName;
    }

    public static void SetUserName(Context context, String strUserName)
    {
        SharedPreferences pref = context.getSharedPreferences(g_strPrefName, Context.MODE_PRIVATE);
        SharedPreferences.Editor editor = pref.edit();
        editor.putString(g_strUserName, strUserName);
        editor.commit();

        return;
    }

    public static String GetPass(Context context)
    {
        String strPass = "";

        SharedPreferences pref = context.getSharedPreferences(g_strPrefName, Context.MODE_PRIVATE);
        strPass = pref.getString(g_strPass, "");

        return strPass;
    }

    public static void SetPass(Context context, String strPass)
    {
        SharedPreferences pref = context.getSharedPreferences(g_strPrefName, Context.MODE_PRIVATE);
        SharedPreferences.Editor editor = pref.edit();
        editor.putString(g_strPass, strPass);
        editor.commit();

        return;
    }

    private static final Pattern EMAIL_ADDRESS_PATTERN = Pattern.compile(
	          "[a-zA-Z0-9\\+\\.\\_\\%\\-\\+]{1,256}" +
	          "\\@" +
	          "[a-zA-Z0-9][a-zA-Z0-9\\-]{0,64}" +
	          "(" +
	          "\\." +
	          "[a-zA-Z0-9][a-zA-Z0-9\\-]{0,25}" +
	          ")+"
	      );

    private static final Pattern PHONE_NUMBER_PATTERN = Pattern.compile("^[+]?[0-9]{10,13}$");

	private static Toast g_Toast = null;
	public static void showToast(Context context, String toastStr)
	{
		if ((g_Toast == null) || (g_Toast.getView().getWindowVisibility() != View.VISIBLE))
		{
			g_Toast = Toast.makeText(context, toastStr, Toast.LENGTH_SHORT);
			g_Toast.show();
		}

		return;
	}

	public static boolean isValidEmail(String strEmail)
	{
		return EMAIL_ADDRESS_PATTERN.matcher(strEmail).matches();
	}

    public static boolean isValidPhone(String strPhone)
    {
        return PHONE_NUMBER_PATTERN.matcher(strPhone).matches();
    }

    public static boolean isOnline(Context ctx)
    {
        ConnectivityManager cm = (ConnectivityManager) ctx.getSystemService(Context.CONNECTIVITY_SERVICE);
        NetworkInfo netInfo = cm.getActiveNetworkInfo();
        if (netInfo != null && netInfo.isConnectedOrConnecting()) {
            return true;
        }
        return false;
    }

    public static String toTwiceNum(int c) {
        if (c >= 10)
            return String.valueOf(c);
        else
            return "0" + String.valueOf(c);
    }

    public static String getDateFormat(int nYear, int nMonth, int nDay)
    {
        return (nYear + "-" + toTwiceNum(nMonth+1) + "-" + toTwiceNum(nDay));
    }

    public static String getDateFormatWithYM(int nYear, int nMonth)
    {
        return (nYear + "-" + toTwiceNum(nMonth+1));
    }

    public static int getCountSpecialCharInString(String str, char c)
    {
        int nCount = 0;
        for (int i = 0; i < str.length(); i++)
        {
            if (str.charAt(i) == c)
                nCount++;
        }

        return nCount;
    }

    public static String  unescape(String src)
    {
        StringBuffer tmp = new StringBuffer();
        tmp.ensureCapacity(src.length());
        int  lastPos=0,pos=0;
        char ch;
        while (lastPos<src.length())
        {
            pos = src.indexOf("%",lastPos);
            if (pos == lastPos)
            {
                if (src.charAt(pos+1)=='u')
                {
                    ch = (char)Integer.parseInt(src.substring(pos+2,pos+6),16);
                    tmp.append(ch);
                    lastPos = pos+6;
                }
                else
                {
                    ch = (char)Integer.parseInt(src.substring(pos+1,pos+3),16);
                    tmp.append(ch);
                    lastPos = pos+3;
                }
            }
            else
            {
                if (pos == -1)
                {
                    tmp.append(src.substring(lastPos));
                    lastPos=src.length();
                }
                else
                {
                    tmp.append(src.substring(lastPos,pos));
                    lastPos=pos;
                }
            }
        }
        return tmp.toString();
    }

    public static boolean IsPackageInstalled(Context context, String packageName)
    {
        PackageManager pm = context.getPackageManager();
        try {
            PackageInfo info = pm.getPackageInfo(packageName, PackageManager.GET_META_DATA);
        } catch (Exception e) {
            return false;
        }
        return true;
    }

    public static boolean InstallAPKFromAsset(Context context, String filename)
    {
        try {
            InputStream is = context.getAssets().open(filename);
            FileOutputStream fos = context.openFileOutput(filename, Context.MODE_WORLD_READABLE | Context.MODE_WORLD_WRITEABLE);
            byte[] buffer = new byte[1024];
            int byteRead = 0;

            while ((byteRead = is.read(buffer)) != -1) {
                fos.write(buffer, 0, byteRead);
            }

            fos.close();

            File apk = context.getFileStreamPath(filename);
            Uri uri = Uri.fromFile(apk);

            Intent intent = new Intent(Intent.ACTION_VIEW);
            intent.setDataAndType(uri, "application/vnd.android.package-archive");
            context.startActivity(intent);
        }
        catch (Exception e) {
            return false;
        }

        return true;
    }

    public static boolean isAvailable(Context ctx, Intent intent) {
        final PackageManager mgr = ctx.getPackageManager();
        List<ResolveInfo> list =   mgr.queryIntentActivities(intent,PackageManager.MATCH_DEFAULT_ONLY);
        return list.size() > 0;
    }

    public static void openFile(Context context, File url) throws IOException {
        File file=url;
        Uri uri = Uri.fromFile(file);

        Intent intent = new Intent(Intent.ACTION_VIEW);
        if (url.toString().contains(".doc") || url.toString().contains(".docx")) {
            intent.setDataAndType(uri, "application/msword");
        } else if(url.toString().contains(".pdf")) {
            intent.setDataAndType(uri, "application/pdf");
        } else if(url.toString().contains(".ppt") || url.toString().contains(".pptx")) {
            intent.setDataAndType(uri, "application/vnd.ms-powerpoint");
        } else if(url.toString().contains(".xls") || url.toString().contains(".xlsx")) {
            intent.setDataAndType(uri, "application/vnd.ms-excel");
        } else if(url.toString().contains(".zip") || url.toString().contains(".rar")) {
            intent.setDataAndType(uri, "application/x-wav");
        } else if(url.toString().contains(".rtf")) {
            intent.setDataAndType(uri, "application/rtf");
        } else if(url.toString().contains(".wav") || url.toString().contains(".mp3")) {
            intent.setDataAndType(uri, "audio/x-wav");
        } else if(url.toString().contains(".gif")) {
            intent.setDataAndType(uri, "image/gif");
        } else if(url.toString().contains(".jpg") || url.toString().contains(".jpeg") || url.toString().contains(".png")) {
            intent.setDataAndType(uri, "image/jpeg");
        } else if(url.toString().contains(".txt")) {
            intent.setDataAndType(uri, "text/plain");
        } else if(url.toString().contains(".3gp") || url.toString().contains(".mpg") || url.toString().contains(".mpeg") || url.toString().contains(".mpe") || url.toString().contains(".mp4") || url.toString().contains(".avi")) {
            intent.setDataAndType(uri, "video/*");
        } else {
            intent.setDataAndType(uri, "*/*");
        }

        if(!isAvailable(context, intent)) {
            showToast(context, "你的手机还没安装能打开本文件的应用...");
            return;
        }

        intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        context.startActivity(intent);
    }

    public static String getRealPathFromURI(Activity activity, Uri uri) {
        String szPath = uri.toString();

        try {
            if (szPath.startsWith("content://")) {

                String[] projection = {MediaStore.Images.Media.DATA};
                @SuppressWarnings("deprecation")
                Cursor cursor = activity.managedQuery(uri, projection, null, null, null);
                int column_index = cursor.getColumnIndexOrThrow(MediaStore.Images.Media.DATA);
                cursor.moveToFirst();

                return cursor.getString(column_index);
            } else if (szPath.startsWith("file://")) {
                return szPath.replace("file:///storage/emulated/0", "/sdcard");
            }
        } catch (Exception ex) {}

        return "";
    }
}
