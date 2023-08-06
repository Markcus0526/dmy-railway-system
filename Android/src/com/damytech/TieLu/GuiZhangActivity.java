package com.damytech.TieLu;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Rect;
import android.net.Uri;
import android.os.Bundle;
import android.os.Environment;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewTreeObserver;
import android.widget.*;
import com.damytech.HttpConn.JsonHttpResponseHandler;
import com.damytech.STData.STPdfFileData;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;
import com.damytech.utils.SQLiteDBHelper;
import com.damytech.utils.SmartImageView.SmartImageView;
import org.json.JSONObject;

import java.io.*;
import java.net.MalformedURLException;
import java.net.URL;
import java.net.URLConnection;
import java.util.ArrayList;

public class GuiZhangActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;
    long fileSize = 0;
    long nWrittenBytes = 0;

    SQLiteDBHelper m_db = null;

    private ImageView imgBack = null;
    private LinearLayout rlData = null;

    private JsonHttpResponseHandler handler = null;
    private ProgressDialog dialog = null;

    ArrayList<STPdfFileData> arrPdfData = new ArrayList<STPdfFileData>();

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgGuiZhang_Back:
                    finish();
                    break;
            }
        }
    };
    /**
     * Called when the activity is first created.
     */
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.guizhang_activity);

        m_db = new SQLiteDBHelper(GuiZhangActivity.this);

        mainLayout = (RelativeLayout)findViewById(R.id.rlGuiZhangBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlGuiZhangBack));
                            bInitialized = true;

                            RefreshPage();
                        }
                    }
                }
        );

        initControl();
        initHandler();

        if(GlobalData.GetOfflineUser(GuiZhangActivity.this)) {
            arrPdfData = m_db.getDataList();
        }
        else {
            dialog = ProgressDialog.show(
                    GuiZhangActivity.this,
                    "",
                    getString(R.string.waiting),
                    true,
                    false,
                    null
            );
            CommMgr.commService.GetRuleList(handler);
        }

        if (bInitialized)
            RefreshPage();
    }

    private void initControl()
    {
        imgBack = (ImageView) findViewById(R.id.imgGuiZhang_Back);
        imgBack.setOnClickListener(onClickListener);

        rlData = (LinearLayout) findViewById(R.id.rlDatas);
    }

    private void initHandler()
    {
        handler = new JsonHttpResponseHandler()
        {
            int result = 0;

            @Override
            public void onSuccess(JSONObject jsonData)
            {
                dialog.dismiss();
                result = 1;

                arrPdfData.clear();
                CommMgr.commService.parseGetRuleList(jsonData, arrPdfData);
                RefreshPage();
            }

            @Override
            public void onFailure(Throwable ex, String exception)
            {
                result = 0;
                dialog.dismiss();
            }

            @Override
            public void onFinish()
            {
                if (result == 0)
                {
                    //GlobalData.showToast(GuiZhangActivity.this, getString(R.string.network_error));
                    //finish();
                }
            }
        };

        return;
    }

    @Override
    public void onResume()
    {
        super.onResume();
    }

    private void RefreshPage()
    {
        if (arrPdfData == null || arrPdfData.size() == 0)
            return;

        int nColCount = (arrPdfData.size() + 3) / 3;
        for (int i = 0; i < nColCount; i++)
        {
            boolean bFlag = true;
            View v = null;
            LayoutInflater inflater = (LayoutInflater)GuiZhangActivity.this.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            v = inflater.inflate(R.layout.guizhangitem_view, null);
            ResolutionSet._instance.iterateChild(v.findViewById(R.id.rlGuiZhangItemBack));

            try
            {
                if (arrPdfData.get(i * 3) != null)
                {
                    ImageView imgNews = (ImageView) v.findViewById(R.id.imgNewFile1);
                    if (isNewPdfFile(arrPdfData.get(i*3).pdfid))
                        imgNews.setVisibility(View.VISIBLE);
                    SmartImageView imgBack = (SmartImageView)v.findViewById(R.id.imgGuiZhangItem1);
                    String localIconPath = arrPdfData.get(i * 3).localiconpath;
                    if(localIconPath.length() > 0)
                    {
                        File imgFile = new  File(localIconPath);
                        if(imgFile.exists()){
                            Bitmap myBitmap = BitmapFactory.decodeFile(imgFile.getAbsolutePath());
                            imgBack.setImageBitmap(myBitmap);
                        }
                    }
                    else
                    {
                        imgBack.setImageUrl(arrPdfData.get(i * 3).iconpath, R.drawable.bookicon);
                    }
                    imgBack.setVisibility(View.VISIBLE);
                    imgBack.setTag(arrPdfData.get(i * 3));
                    imgBack.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            STPdfFileData newPdfFile = (STPdfFileData) v.getTag();
                            STPdfFileData oldPdfFile = m_db.getPdfInfo(newPdfFile.pdfid);
                            //if (GlobalData.GetOfflineUser(GuiZhangActivity.this) == false)
                            {
                                if (newPdfFile.remotepath.equals(oldPdfFile.remotepath) == false)
                                    DownloadPdfFile(newPdfFile.pdfid);
                                else
                                {
                                    try {
                                        if (existPdfFile(oldPdfFile.localpath) == false)
                                        {
                                            DownloadPdfFile(newPdfFile.pdfid);
                                            return;
                                        }

                                        if (GlobalData.IsPackageInstalled(GuiZhangActivity.this, "com.adobe.reader") == false)
                                        {
                                            //GlobalData.showToast(GuiZhangActivity.this, getString(R.string.pdfreadererror));
                                            //return;
                                            GlobalData.InstallAPKFromAsset(GuiZhangActivity.this, "adobereader.apk");
                                        }

                                        Uri path = Uri.fromFile(new File(oldPdfFile.localpath));
                                        Intent intent = new Intent(Intent.ACTION_VIEW);
                                        intent.setPackage("com.adobe.reader");
                                        intent.setDataAndType(path, "application/pdf");
                                        intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                                        startActivity(intent);

                                    } catch (Exception ex) {
                                        //GlobalData.showToast(GuiZhangActivity.this, getString(R.string.pdfreadererror));
                                        GlobalData.InstallAPKFromAsset(GuiZhangActivity.this, "adobereader.apk");
                                    }
                                }
                            }
                            /*
                            else
                            {
                                if (existPdfFile(newPdfFile.localpath) == false)
                                {
                                    GlobalData.showToast(GuiZhangActivity.this, getString(R.string.notexitfile));
                                    return;
                                }
                                else {
                                    if (GlobalData.IsPackageInstalled(GuiZhangActivity.this, "com.adobe.reader") == false) {
                                        //GlobalData.showToast(GuiZhangActivity.this, getString(R.string.pdfreadererror));
                                        //return;
                                        GlobalData.InstallAPKFromAsset(GuiZhangActivity.this, "adobereader.apk");
                                    }

                                    Uri path = Uri.fromFile(new File(newPdfFile.localpath));
                                    Intent intent = new Intent(Intent.ACTION_VIEW);
                                    intent.setPackage("com.adobe.reader");
                                    intent.setDataAndType(path, "application/pdf");
                                    intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                                    startActivity(intent);
                                }
                            }
                            */
                        }
                    });
                }
            } catch (Exception ex) {
                bFlag = false;
            }

            try
            {
                if (arrPdfData.get(i * 3 + 1) != null)
                {
                    ImageView imgNews = (ImageView) v.findViewById(R.id.imgNewFile2);
                    if (isNewPdfFile(arrPdfData.get(i*3+1).pdfid))
                        imgNews.setVisibility(View.VISIBLE);
                    SmartImageView imgBack = (SmartImageView)v.findViewById(R.id.imgGuiZhangItem2);
                    String localIconPath = arrPdfData.get(i * 3 + 1).localiconpath;
                    if(localIconPath.length() > 0)
                    {
                        File imgFile = new  File(localIconPath);
                        if(imgFile.exists()){
                            Bitmap myBitmap = BitmapFactory.decodeFile(imgFile.getAbsolutePath());
                            imgBack.setImageBitmap(myBitmap);
                        }
                    }
                    else
                    {
                        imgBack.setImageUrl(arrPdfData.get(i * 3 + 1).iconpath, R.drawable.bookicon);
                    }
                    imgBack.setVisibility(View.VISIBLE);
                    imgBack.setTag(arrPdfData.get(i * 3 + 1));
                    imgBack.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            STPdfFileData newPdfFile = (STPdfFileData) v.getTag();
                            STPdfFileData oldPdfFile = m_db.getPdfInfo(newPdfFile.pdfid);
                            //if (GlobalData.GetOfflineUser(GuiZhangActivity.this) == false)
                            {
                                if (newPdfFile.remotepath.equals(oldPdfFile.remotepath) == false)
                                    DownloadPdfFile(newPdfFile.pdfid);
                                else
                                {
                                    try {
                                        if (existPdfFile(oldPdfFile.localpath) == false)
                                        {
                                            DownloadPdfFile(newPdfFile.pdfid);
                                            return;
                                        }

                                        if (GlobalData.IsPackageInstalled(GuiZhangActivity.this, "com.adobe.reader") == false)
                                        {
//                                            GlobalData.showToast(GuiZhangActivity.this, getString(R.string.pdfreadererror));
//                                            return;
                                            GlobalData.InstallAPKFromAsset(GuiZhangActivity.this, "adobereader.apk");
                                        }

                                        Uri path = Uri.fromFile(new File(oldPdfFile.localpath));
                                        Intent intent = new Intent(Intent.ACTION_VIEW);
                                        intent.setDataAndType(path, "application/pdf");
                                        intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                                        startActivity(intent);

                                    } catch (Exception ex) {
                                        //GlobalData.showToast(GuiZhangActivity.this, getString(R.string.pdfreadererror));
                                        GlobalData.InstallAPKFromAsset(GuiZhangActivity.this, "adobereader.apk");
                                    }
                                }
                            }
                            /*
                            else
                            {
                                if (existPdfFile(newPdfFile.localpath) == false)
                                {
                                    GlobalData.showToast(GuiZhangActivity.this, getString(R.string.notexitfile));
                                    return;
                                }
                                else {
                                    if (GlobalData.IsPackageInstalled(GuiZhangActivity.this, "com.adobe.reader") == false) {
//                                        GlobalData.showToast(GuiZhangActivity.this, getString(R.string.pdfreadererror));
//                                        return;
                                        GlobalData.InstallAPKFromAsset(GuiZhangActivity.this, "adobereader.apk");
                                    }

                                    Uri path = Uri.fromFile(new File(newPdfFile.localpath));
                                    Intent intent = new Intent(Intent.ACTION_VIEW);
                                    intent.setDataAndType(path, "application/pdf");
                                    intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                                    startActivity(intent);
                                }
                            }
                            */
                        }
                    });
                }
            } catch (Exception ex) {}

            try
            {
                if (arrPdfData.get(i * 3 + 2) != null)
                {
                    ImageView imgNews = (ImageView) v.findViewById(R.id.imgNewFile3);
                    if (isNewPdfFile(arrPdfData.get(i*3+2).pdfid))
                        imgNews.setVisibility(View.VISIBLE);
                    SmartImageView imgBack = (SmartImageView)v.findViewById(R.id.imgGuiZhangItem3);
                    String localIconPath = arrPdfData.get(i * 3 + 2).localiconpath;
                    if(localIconPath.length() > 0)
                    {
                        File imgFile = new  File(localIconPath);
                        if(imgFile.exists()){
                            Bitmap myBitmap = BitmapFactory.decodeFile(imgFile.getAbsolutePath());
                            imgBack.setImageBitmap(myBitmap);
                        }
                    }
                    else
                    {
                        imgBack.setImageUrl(arrPdfData.get(i * 3 + 2).iconpath, R.drawable.bookicon);
                    }
                    imgBack.setVisibility(View.VISIBLE);
                    imgBack.setTag(arrPdfData.get(i * 3 + 2));
                    imgBack.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            STPdfFileData newPdfFile = (STPdfFileData) v.getTag();
                            STPdfFileData oldPdfFile = m_db.getPdfInfo(newPdfFile.pdfid);
                            //if (GlobalData.GetOfflineUser(GuiZhangActivity.this) == false)
                            {
                                if (newPdfFile.remotepath.equals(oldPdfFile.remotepath) == false)
                                    DownloadPdfFile(newPdfFile.pdfid);
                                else
                                {
                                    try {
                                        if (existPdfFile(oldPdfFile.localpath) == false)
                                        {
                                            DownloadPdfFile(newPdfFile.pdfid);
                                            return;
                                        }

                                        if (GlobalData.IsPackageInstalled(GuiZhangActivity.this, "com.adobe.reader") == false)
                                        {
//                                            GlobalData.showToast(GuiZhangActivity.this, getString(R.string.pdfreadererror));
//                                            return;
                                            GlobalData.InstallAPKFromAsset(GuiZhangActivity.this, "adobereader.apk");
                                        }

                                        Uri path = Uri.fromFile(new File(oldPdfFile.localpath));
                                        Intent intent = new Intent(Intent.ACTION_VIEW);
                                        intent.setDataAndType(path, "application/pdf");
                                        intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                                        startActivity(intent);

                                    } catch (Exception ex) {
                                        //GlobalData.showToast(GuiZhangActivity.this, getString(R.string.pdfreadererror));
                                        GlobalData.InstallAPKFromAsset(GuiZhangActivity.this, "adobereader.apk");
                                    }
                                }
                            }
                            /*
                            else
                            {
                                if (existPdfFile(newPdfFile.localpath) == false)
                                {
                                    GlobalData.showToast(GuiZhangActivity.this, getString(R.string.notexitfile));
                                    return;
                                }
                                else {
                                    if (GlobalData.IsPackageInstalled(GuiZhangActivity.this, "com.adobe.reader") == false) {
//                                        GlobalData.showToast(GuiZhangActivity.this, getString(R.string.pdfreadererror));
//                                        return;
                                        GlobalData.InstallAPKFromAsset(GuiZhangActivity.this, "adobereader.apk");
                                    }

                                    Uri path = Uri.fromFile(new File(newPdfFile.localpath));
                                    Intent intent = new Intent(Intent.ACTION_VIEW);
                                    intent.setDataAndType(path, "application/pdf");
                                    intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                                    startActivity(intent);
                                }
                            }
                            */
                        }
                    });
                }
            } catch (Exception ex) {}

            if (v != null && bFlag)
                rlData.addView(v);
        }
    }

    public boolean isNewPdfFile(int pdfid)
    {
        int nIndex = -1;
        STPdfFileData oldFile = m_db.getPdfInfo(pdfid);
        for (int i = 0; i < arrPdfData.size(); i++)
        {
            if (arrPdfData.get(i).pdfid == pdfid)
            {
                nIndex = i;
                break;
            }
        }

        if (nIndex == -1) return false;

        STPdfFileData newFile = arrPdfData.get(nIndex);

        return !(newFile.remotepath.equals(oldFile.remotepath));
    }

    private boolean existPdfFile(String path)
    {
        try {
            File file = new File(path);
            return file.exists();
        } catch (Exception ex){
            return false;
        }
    }

    private void DownloadPdfFile(final int pdfid)
    {
        int nVal = 0;
        for (int i = 0; i < arrPdfData.size(); i++)
        {
            if (arrPdfData.get(i).pdfid == pdfid)
            {
                nVal = i;
                break;
            }
        }

        final int index = nVal;
        final String upgrade_url = arrPdfData.get(index).remotepath;
        final String icon_url = arrPdfData.get(index).iconpath;

        Thread thr = new Thread(new Runnable()
        {
            @Override
            public void run()
            {
                // download pdf icon
                String local_file_path = "";
                String local_icon_path = "";

                try {
                    int nBytesRead = 0;
                    byte[] buf = new byte[1024];

                    URLConnection urlConn = null;
                    URL fileUrl = null;
                    InputStream inStream = null;
                    OutputStream outStream = null;

                    File dir_item = null, file_item = null;

                    fileUrl = new URL(icon_url);
                    urlConn = fileUrl.openConnection();
                    fileSize = urlConn.getContentLength();
                    inStream = urlConn.getInputStream();
                    local_icon_path = icon_url.substring(icon_url.lastIndexOf("/") + 1);
                    dir_item = new File(Environment.getExternalStorageDirectory(), "Download");
                    dir_item.mkdirs();
                    file_item = new File(dir_item, local_icon_path);

                    local_icon_path = file_item.getAbsolutePath();
                    outStream = new BufferedOutputStream(new FileOutputStream(file_item));
                    nWrittenBytes = 0;
                    while ((nBytesRead = inStream.read(buf)) != -1)
                    {
                        outStream.write(buf, 0, nBytesRead);
                        nWrittenBytes += nBytesRead;
                    }
                    inStream.close();
                    outStream.flush();
                    outStream.close();
                } catch (MalformedURLException e) {
                    e.printStackTrace();
                } catch (FileNotFoundException e) {
                    e.printStackTrace();
                } catch (IOException e) {
                    e.printStackTrace();
                } finally {

                }

                // download pdf data
                try
                {
                    int nBytesRead = 0;
                    byte[] buf = new byte[1024];

                    URLConnection urlConn = null;
                    URL fileUrl = null;
                    InputStream inStream = null;
                    OutputStream outStream = null;

                    File dir_item = null, file_item = null;

                    fileUrl = new URL(upgrade_url);
                    urlConn = fileUrl.openConnection();
                    fileSize = urlConn.getContentLength();
                    inStream = urlConn.getInputStream();
                    local_file_path = upgrade_url.substring(upgrade_url.lastIndexOf("/") + 1);
                    dir_item = new File(Environment.getExternalStorageDirectory(), "Download");
                    dir_item.mkdirs();
                    file_item = new File(dir_item, local_file_path);

                    local_file_path = file_item.getAbsolutePath();
                    outStream = new BufferedOutputStream(new FileOutputStream(file_item));

                    nWrittenBytes = 0;
                    while ((nBytesRead = inStream.read(buf)) != -1)
                    {
                        outStream.write(buf, 0, nBytesRead);
                        nWrittenBytes += nBytesRead;
                        runOnUiThread(new Runnable() {
                            @Override
                            public void run() {
                                if (fileSize != 0)
                                {
                                    String strPercent = getString(R.string.xiazaizhong) + " (" + String.format("%.0f", (nWrittenBytes * 100.0f) / fileSize) + "%)";
                                    if (dialog.isShowing())
                                        dialog.setMessage(strPercent);
                                }
                            }
                        });
                    }

                    inStream.close();
                    outStream.flush();
                    outStream.close();
                    /////////////////////////////////////////////////////////////////////////

                    STPdfFileData stPdfFileData = m_db.getPdfInfo(pdfid);
                    if (stPdfFileData.remotepath.length() == 0 && stPdfFileData.localpath.length() == 0 && stPdfFileData.iconpath.length() == 0)
                        m_db.savePdfInfo(pdfid, arrPdfData.get(index).iconpath, local_icon_path, local_file_path, upgrade_url);
                    else
                        m_db.updatePdfData(pdfid, arrPdfData.get(index).iconpath, local_icon_path, local_file_path, upgrade_url);
                    runOnUiThread(new Runnable()
                    {
                        @Override
                        public void run()
                        {
                            hideProgress();

                            try {
                                STPdfFileData fileData = m_db.getPdfInfo(pdfid);

                                if (GlobalData.IsPackageInstalled(GuiZhangActivity.this, "com.adobe.reader") == false) {
                                    //GlobalData.showToast(GuiZhangActivity.this, getString(R.string.pdfreadererror));
                                    //return;
                                    GlobalData.InstallAPKFromAsset(GuiZhangActivity.this, "adobereader.apk");
                                }

                                Uri path = Uri.fromFile(new File(fileData.localpath));
                                Intent intent = new Intent(Intent.ACTION_VIEW);
                                intent.setDataAndType(path, "application/pdf");
                                intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                                startActivity(intent);

                            } catch (Exception ex) {
                                //GlobalData.showToast(GuiZhangActivity.this, getString(R.string.pdfreadererror));
                                GlobalData.InstallAPKFromAsset(GuiZhangActivity.this, "adobereader.apk");
                            }
                        }
                    });
                }
                catch (Exception ex)
                {
                    ex.printStackTrace();

                    hideProgress();

                    runOnUiThread(new Runnable()
                    {
                        @Override
                        public void run()
                        {
                            GlobalData.showToast(GuiZhangActivity.this, getString(R.string.download_fail));
                        }
                    });

                    return;
                }
            }
        });

        showProgress();
        thr.start();
    }

    public void showProgress()
    {
        if (dialog == null)
        {
            dialog = new ProgressDialog(GuiZhangActivity.this);
            dialog.setProgressStyle(ProgressDialog.STYLE_SPINNER);
            dialog.setMessage(getResources().getString(R.string.waiting));
            dialog.setCancelable(false);
        }

        if (dialog.isShowing())
            return;

        dialog.show();
    }

    public void hideProgress()
    {
        if (dialog != null)
            dialog.dismiss();
    }
}
