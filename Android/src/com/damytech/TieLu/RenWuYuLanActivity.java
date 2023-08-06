package com.damytech.TieLu;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Intent;
import android.graphics.Rect;
import android.os.Bundle;
import android.util.Base64;
import android.view.View;
import android.view.ViewTreeObserver;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.RelativeLayout;
import android.widget.TextView;
import com.damytech.HttpConn.JsonHttpResponseHandler;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.File;
import java.io.FileInputStream;

public class RenWuYuLanActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    ImageView imgBack = null;
    Button btnOk = null;
    TextView lblTitle = null;
    TextView lblKaiShiShiJian = null;
    TextView lblJieShuShiJian = null;
    TextView lblZhiXingRen = null;
    TextView lblNeiRong = null;
    TextView lblFileName = null;

    String strTitle = "";
    String strStartTime = "";
    String strEndTime = "";
    String strShouXinRen = "";
    String strShouXinRenIDS = "";
    String strcontent = "";
    String strFileName = "";
    String strFilePath = "";

    private JsonHttpResponseHandler handlerAddTask = null;
    private ProgressDialog dialog = null;

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgRenWuGuanLi_Back:
                    finish();
                    break;
                case R.id.btnRenWuYuLan_Ok:
                    String strFileData = "";
                    try {
                        File file = new File(strFilePath);
                        int fileLen = (int) file.length();
                        byte []data = new byte[fileLen];
                        FileInputStream fis = new FileInputStream(file);
                        fis.read(data);
                        strFileData = Base64.encodeToString(data, Base64.DEFAULT);
                    } catch (Exception e) {
                        strFileName = "";
                        strFileData = "";
                    }
                    dialog = ProgressDialog.show(RenWuYuLanActivity.this,
                            "",
                            getString(R.string.waiting),
                            true,
                            false,
                            null);
                    CommMgr.commService.SetTaskItem(handlerAddTask,
                            Long.toString(GlobalData.GetUid(RenWuYuLanActivity.this)),
                            strTitle,
                            strShouXinRenIDS,
                            strStartTime,
                            strEndTime,
                            strcontent,
                            strFileName,
                            strFileData);
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
        setContentView(R.layout.renwuguanli_yulan_activity);

        strTitle = getIntent().getStringExtra("title");
        strStartTime = getIntent().getStringExtra("starttime");
        strEndTime = getIntent().getStringExtra("endtime");
        strShouXinRen = getIntent().getStringExtra("ShouXinRen");
        strShouXinRenIDS = getIntent().getStringExtra("ShouXinRenIDList");
        strcontent = getIntent().getStringExtra("content");
        strFileName = getIntent().getStringExtra("FileName");
        strFilePath = getIntent().getStringExtra("FilePath");

        mainLayout = (RelativeLayout)findViewById(R.id.rlRenWuYuLan_Back);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlRenWuYuLan_Back));
                            bInitialized = true;
                        }
                    }
                }
        );

        initControl();
        initHandler();
    }

    private void initControl()
    {
        imgBack = (ImageView) findViewById(R.id.imgRenWuYuLan_Back);
        imgBack.setOnClickListener(onClickListener);

        lblTitle = (TextView) findViewById(R.id.lblRenWuYuLan_WenTi);
        lblTitle.setText(getString(R.string.renwumingcheng) + strTitle);
        lblKaiShiShiJian = (TextView) findViewById(R.id.lblRenWuYuLan_KaiShiShiJian);
        lblKaiShiShiJian.setText(getString(R.string.kaishishijian) + strStartTime);
        lblJieShuShiJian = (TextView) findViewById(R.id.lblRenWuYuLan_JieShuShiJian);
        lblJieShuShiJian.setText(getString(R.string.jieshushijian) + strEndTime);
        lblZhiXingRen = (TextView) findViewById(R.id.lblRenWuYuLan_ZhiXingRenVal);
        lblZhiXingRen.setText(strShouXinRen);
        lblNeiRong  = (TextView) findViewById(R.id.lblRenWuYuLan_NeiRongVal);
        lblNeiRong.setText(strcontent);
        lblFileName = (TextView) findViewById(R.id.lblRenWuYuLan_FileName);
        if (strFileName != null && strFileName.length() > 0)
            lblFileName.setText(getString(R.string.fujian) + strFileName);

        btnOk= (Button) findViewById(R.id.btnRenWuYuLan_Ok);
        btnOk.setOnClickListener(onClickListener);
    }

    private void initHandler()
    {
        handlerAddTask = new JsonHttpResponseHandler()
        {
            int result = 0;

            @Override
            public void onSuccess(JSONObject jsonData)
            {
                dialog.dismiss();
                result = 1;

                long nRet = -1;
                try {
                    nRet = jsonData.getLong("SVCC_RETVAL");
                } catch (JSONException e) {
                    e.printStackTrace();
                }
                if (nRet == 0)
                {
                    GlobalData.showToast(RenWuYuLanActivity.this, getString(R.string.service_success));
                    Intent intent = new Intent(RenWuYuLanActivity.this, RenWuGuanLiActivity.class);
                    intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK | Intent.FLAG_ACTIVITY_CLEAR_TOP);
                    startActivity(intent);
                    finish();
                }
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
                    GlobalData.showToast(RenWuYuLanActivity.this, getString(R.string.network_error));
                }
            }
        };

        return;
    }
}
