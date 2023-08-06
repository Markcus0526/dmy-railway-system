package com.damytech.TieLu;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Intent;
import android.graphics.Rect;
import android.os.Bundle;
import android.os.Environment;
import android.view.View;
import android.view.ViewTreeObserver;
import android.widget.*;
import com.damytech.HttpConn.JsonHttpResponseHandler;
import com.damytech.STData.STDaiQianItem;
import com.damytech.STData.STServiceData;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;
import com.damytech.utils.SQLiteDBHelper;
import org.json.JSONObject;

import java.io.*;
import java.net.URL;
import java.net.URLConnection;
import java.net.URLEncoder;

public class GongWenXiangQingActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    private int nQianShouVisible = 0;

    private ImageView imgBack = null;
    private TextView lblBiaoTi = null;
    private TextView lblTongZhiHao = null;
    private TextView lblFaBuRen = null;
    private TextView lblFaBuBuMen = null;
    private TextView lblFaBuShiJian = null;
    private TextView lblLiuZhuanRen = null;
    private TextView lblLiuZhuanShiJian = null;
    private TextView lblGongWenNeiRong = null;
    private TextView lblLiuZhuanNeiRong = null;
    private TextView lblFuJian = null;
    private Button btnXiaZai = null;
    private Button btnOpen = null;
    private Button btnLiuZhuan = null;
    private Button btnQianShou = null;

    private SQLiteDBHelper m_db = null;

    private long nDocID = 0;

    private String upgrade_url = "";
    private String local_file_path = "";
    private STDaiQianItem stInfo = new STDaiQianItem();

    private JsonHttpResponseHandler handler = null;

    ProgressDialog dialog = null;
    private JsonHttpResponseHandler handlerUpload = new JsonHttpResponseHandler()
    {
        int result = 0;
        long nRet = 0;

        @Override
        public void onSuccess(JSONObject jsonData)
        {
            result = 1;

            nRet = CommMgr.commService.parseSetQianShouData(jsonData);
        }

        @Override
        public void onFailure(Throwable ex, String exception)
        {
            result = 0;
        }

        @Override
        public void onFinish()
        {
            dialog.dismiss();
            if (result == 0)
            {
                GlobalData.showToast(GongWenXiangQingActivity.this, getString(R.string.network_error));
            }
            if (nRet == STServiceData.ERR_SUCCESS)
            {
                GlobalData.showToast(GongWenXiangQingActivity.this, getString(R.string.caozuochenggong));
                GongWenXiangQingActivity.this.finish();
            }
            if (nRet == STServiceData.ERR_SERVER_ERROR)
            {
                GlobalData.showToast(GongWenXiangQingActivity.this, getString(R.string.service_error));
            }
        }
    };

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgGongWenDaiQian_XiangQingBack:
                    finish();
                    break;
                case R.id.btnGongWenDaiQian_XiangQingXiaZai:
                    DownloadData();
                    break;
                case R.id.btnOpen:
                    if (stInfo != null) {
                        String szPath = m_db.getLocalPath(stInfo.filepath);
                        if (szPath.length() == 0) {
                            GlobalData.showToast(GongWenXiangQingActivity.this, getString(R.string.notexitfile));
                            return;
                        } else {
                            try {
                                GlobalData.openFile(GongWenXiangQingActivity.this, new File(szPath));
                            } catch (IOException e) {
                                GlobalData.showToast(GongWenXiangQingActivity.this, getString(R.string.fileoopenerror));
                            }
                        }
                    } else {
                        GlobalData.showToast(GongWenXiangQingActivity.this, getString(R.string.notexitfile));
                    }
                    break;
                case R.id.btnGongWenDaiQian_XiangQing_LiuZhuan:
                    if (stInfo != null && stInfo.uid > 0)
                    {
                        Intent intent = new Intent(GongWenXiangQingActivity.this, GongWenLiuZhuanActivity.class);
                        intent.putExtra("Data", stInfo);
                        startActivity(intent);
                    }
                    break;
                case R.id.btnGongWenDaiQian_XiangQing_QianShou:
                    if (stInfo != null && stInfo.uid > 0)
                    {
                        dialog = ProgressDialog.show(
                                GongWenXiangQingActivity.this,
                                "",
                                getString(R.string.waiting),
                                true,
                                false,
                                null
                        );
                        CommMgr.commService.SetLiuZhuanData(handlerUpload, Long.toString(GlobalData.GetUid(GongWenXiangQingActivity.this)), Long.toString(stInfo.uid), "", stInfo.receiver);
                    }
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
        setContentView(R.layout.gongwendaiqian_xiangxi_activity);

        nDocID = getIntent().getLongExtra("uid", 0);
        nQianShouVisible = getIntent().getIntExtra("QianShouFlag", 0);

        m_db = new SQLiteDBHelper(this);

        mainLayout = (RelativeLayout)findViewById(R.id.rlGongWenDaiQian_XiangQingBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlGongWenDaiQian_XiangQingBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        initControl();
        initHandler();

        dialog = ProgressDialog.show(
                GongWenXiangQingActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetDaiQianDetail(handler, GlobalData.GetUid(GongWenXiangQingActivity.this), nDocID);
    }

    private void initControl()
    {
        imgBack = (ImageView) findViewById(R.id.imgGongWenDaiQian_XiangQingBack);
        imgBack.setOnClickListener(onClickListener);

        lblBiaoTi = (TextView) findViewById(R.id.lblGongWenDaiQian_XiangQingBiaoTi);
        lblTongZhiHao = (TextView) findViewById(R.id.lblGongWenDaiQian_XiangQingTongZhiHaoVal);
        lblFaBuRen = (TextView) findViewById(R.id.lblGongWenDaiQian_XiangQingShouXinRenVal);
        lblFaBuBuMen = (TextView) findViewById(R.id.lblGongWenDaiQian_XiangQingFaBuBuMenVal);
        lblFaBuShiJian = (TextView) findViewById(R.id.lblGongWenDaiQian_XiangQingFaBuShiJianVal);
        lblLiuZhuanRen = (TextView) findViewById(R.id.lblGongWenDaiQian_XiangQingLiuZhuanRenVal);
        lblLiuZhuanShiJian = (TextView) findViewById(R.id.lblGongWenDaiQian_XiangQingLiuZhuanShiJianVal);
        lblGongWenNeiRong = (TextView) findViewById(R.id.lblGongWenDaiQian_XiangQingNeiRongVal);
        lblLiuZhuanNeiRong = (TextView) findViewById(R.id.lblGongWenDaiQian_XiangQingPiShiVal);
        lblFuJian = (TextView)findViewById(R.id.lblGongWenDaiQian_XiangQingFuJian);

        btnXiaZai = (Button) findViewById(R.id.btnGongWenDaiQian_XiangQingXiaZai);
        btnXiaZai.setOnClickListener(onClickListener);
        btnOpen = (Button) findViewById(R.id.btnOpen);
        btnOpen.setOnClickListener(onClickListener);

        btnLiuZhuan = (Button) findViewById(R.id.btnGongWenDaiQian_XiangQing_LiuZhuan);
        btnLiuZhuan.setOnClickListener(onClickListener);

        btnQianShou = (Button) findViewById(R.id.btnGongWenDaiQian_XiangQing_QianShou);
        btnQianShou.setOnClickListener(onClickListener);
        if (nQianShouVisible == 1)
            btnQianShou.setVisibility(View.VISIBLE);
        else
            btnQianShou.setVisibility(View.INVISIBLE);
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

                CommMgr.commService.parseGetDaiQianDetail(jsonData, stInfo);
                if (stInfo == null)
                    result = -1;
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
                if (result == 1)
                {
                    ShowData();
                }

                if (result == 0)
                {
                    GlobalData.showToast(GongWenXiangQingActivity.this, getString(R.string.network_error));
                    finish();
                }
                else if (result == -1)
                {
                    GlobalData.showToast(GongWenXiangQingActivity.this, getString(R.string.service_error));
                    finish();
                }
            }
        };

        return;
    }

    private void ShowData()
    {
        if (stInfo != null)
        {
            lblBiaoTi.setText(stInfo.title);
            lblFaBuBuMen.setText(stInfo.bumen);
            lblFaBuRen.setText(stInfo.faburen);
            lblFaBuShiJian.setText(stInfo.fbdate);
            lblTongZhiHao.setText(stInfo.tongzhihao);
            lblFuJian.setText(getString(R.string.fujian) + stInfo.filename);
            lblGongWenNeiRong.setText(stInfo.content);

            if (stInfo.filepath.length() > 0) {
                btnXiaZai.setVisibility(View.VISIBLE);
                btnOpen.setVisibility(View.VISIBLE);
            }

            String strData = "";
            if (stInfo.liuzhuanrenpishi != null && stInfo.liuzhuanrenpishi.length > 0) {
                int nLen = stInfo.liuzhuanrenpishi.length;

                for (int i = 0; i < nLen; i++) {
                    strData = strData + stInfo.liuzhuanrenpishi[i] + "\n";
                }
            }

            lblLiuZhuanNeiRong.setText(strData);
        }
    }

    private void DownloadData()
    {
        Thread thr = new Thread(new Runnable()
        {
            @Override
            public void run()
            {
                try
                {
                    int nBytesRead = 0, nByteWritten = 0;
                    byte[] buf = new byte[1024];

                    URLConnection urlConn = null;
                    URL fileUrl = null;
                    InputStream inStream = null;
                    OutputStream outStream = null;

                    File dir_item = null, file_item = null;

                    // Show progress dialog
                    runOnUiThread(runnable_showProgress);

                    // Downloading file from address
                    int p=stInfo.filepath.lastIndexOf("/");
                    String dirPath = stInfo.filepath.substring(0, p);
                    String filePath = stInfo.filepath.substring(p + 1);
                    upgrade_url = dirPath + "/" + URLEncoder.encode(filePath.replace(",", ""), "UTF-8");

                    fileUrl = new URL(upgrade_url);
                    urlConn = fileUrl.openConnection();
                    inStream = urlConn.getInputStream();
                    local_file_path = upgrade_url.substring(upgrade_url.lastIndexOf("/") + 1);
                    dir_item = new File(Environment.getExternalStorageDirectory(), "Download");
                    dir_item.mkdirs();
                    file_item = new File(dir_item, local_file_path);

                    local_file_path = file_item.getAbsolutePath();

                    outStream = new BufferedOutputStream(new FileOutputStream(file_item));

                    while ((nBytesRead = inStream.read(buf)) != -1)
                    {
                        outStream.write(buf, 0, nBytesRead);
                        nByteWritten += nBytesRead;
                        UpdateProgress(nByteWritten);
                    }

                    UpdateProgress(getResources().getString(R.string.download_success));

                    inStream.close();
                    outStream.flush();
                    outStream.close();
                    /////////////////////////////////////////////////////////////////////////

                    // Hide progress dialog
                    runOnUiThread(runnable_hideProgress);

                    // Finish downloading and install
                    runOnUiThread(runnable_finish_download);
                }
                catch (Exception ex)
                {
                    ex.printStackTrace();
                    runOnUiThread(runnable_download_error);
                }
            }
        });

        thr.start();
    }


    private void UpdateProgress(int nValue)
    {
        double fVal = nValue / 1024.0f / 1024.0f;
        String strVal = String.format("%.1fM", fVal);
        UpdateProgress(strVal);
    }

    private void UpdateProgress(final String szMsg)
    {
        Runnable runnable_update = new Runnable() {
            @Override
            public void run() {
                dialog.setMessage(szMsg);
            }
        };

        runOnUiThread(runnable_update);
    }

    private Runnable runnable_showProgress = new Runnable() {
        @Override
        public void run() {
            showProgress();
        }
    };

    private Runnable runnable_hideProgress = new Runnable() {
        @Override
        public void run() {
            showProgress();
        }
    };

    public void showProgress()
    {
        if (dialog == null)
        {
            dialog = new ProgressDialog(GongWenXiangQingActivity.this);
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

    Runnable runnable_finish_download = new Runnable()
    {
        public void run()
        {
            runOnUiThread(new Runnable() {
                @Override
                public void run() {
                    m_db.saveDownInfo(local_file_path, stInfo.filepath);
                }
            });
            hideProgress();
        }
    };

    Runnable runnable_download_error = new Runnable() {
        @Override
        public void run() {
            hideProgress();
            GlobalData.showToast(GongWenXiangQingActivity.this, getResources().getString(R.string.download_fail));
        }
    };
}
