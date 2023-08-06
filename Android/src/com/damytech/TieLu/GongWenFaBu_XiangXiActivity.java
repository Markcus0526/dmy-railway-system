package com.damytech.TieLu;

import android.app.Activity;
import android.app.ProgressDialog;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Rect;
import android.os.Bundle;
import android.util.Base64;
import android.view.View;
import android.view.ViewTreeObserver;
import android.widget.*;
import com.damytech.HttpConn.JsonHttpResponseHandler;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;
import org.json.JSONObject;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;

public class GongWenFaBu_XiangXiActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    private ImageView imgBack = null;
    private TextView lblBiaoTi = null;
    private TextView lblTongZhiHao = null;
    private TextView lblShouXinRen = null;
    private TextView lblBuMen = null;
    private TextView lblFaBuShiJian = null;
    private TextView lblLiuZhuanRen = null;
    private TextView lblLiuZhuanShiJian = null;
    private EditText lblGongWenNeiRong = null;
    private EditText lblLiuZhuanNeiRong = null;
    private TextView lblFileName = null;
    private Button btnOK = null;

    private String strBiaoTi = "";
    private String strTongZhiHao = "";
    private String strShouXinRen = "";
    private String strShouXinRenIDList = "";
    private String strBuMen = "";
    private String strFaBuShiJian = "";
    private String strLiuZhuanRen = "";
    private String strLiuZhuanShiJian = "";
    private String strGongWenNeiRong = "";
    private String strLiuZhuanNeiRong = "";
    private String strFileName = "";
    private String strFilePath = "";
    private String strFileData = "";

    private JsonHttpResponseHandler handler = null;
    ProgressDialog dialog = null;

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgGongWenFaBu_XiangXi_Back:
                    finish();
                    break;
                case R.id.btnGongWenFaBu_XiangXi_FaBu:
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

                    dialog = ProgressDialog.show(
                            GongWenFaBu_XiangXiActivity.this,
                            "",
                            getString(R.string.waiting),
                            true,
                            false,
                            null
                    );

                    CommMgr.commService.SetGongWenItem(handler,
                            Long.toString(GlobalData.GetUid(GongWenFaBu_XiangXiActivity.this)),
                            strBiaoTi,
                            strTongZhiHao,
                            strShouXinRenIDList,
                            strBuMen,
                            strFaBuShiJian,
                            strLiuZhuanRen,
                            strLiuZhuanShiJian,
                            strGongWenNeiRong,
                            strLiuZhuanNeiRong,
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
        setContentView(R.layout.gongwenfabu_xiangxi_activity);

        strBiaoTi = getIntent().getStringExtra("BiaoTi");
        strTongZhiHao = getIntent().getStringExtra("TongZhiHao");
        strShouXinRen = getIntent().getStringExtra("ShouXinRen");
        strShouXinRenIDList = getIntent().getStringExtra("ShouXinRenIDList");
        strBuMen = getIntent().getStringExtra("BuMen");
        strFaBuShiJian = getIntent().getStringExtra("FaBuShiJian");
        strLiuZhuanRen = getIntent().getStringExtra("LiuZhuanRen");
        strLiuZhuanShiJian =  getIntent().getStringExtra("LiuZhuanShiJian");
        strGongWenNeiRong = getIntent().getStringExtra("GongWenNeiRong");
        strLiuZhuanNeiRong = getIntent().getStringExtra("LiuZhuanNeiRong");
        strFileName = getIntent().getStringExtra("FileName");
        strFilePath = getIntent().getStringExtra("FilePath");

        mainLayout = (RelativeLayout)findViewById(R.id.rlGongWenFaBu_XiangXiBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlGongWenFaBu_XiangXiBack));
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
        imgBack = (ImageView) findViewById(R.id.imgGongWenFaBu_XiangXi_Back);
        imgBack.setOnClickListener(onClickListener);

        lblBiaoTi = (TextView) findViewById(R.id.lblGongWenFaBu_XiangXi_BiaoTi);
        lblBiaoTi.setText(strBiaoTi);
        lblTongZhiHao = (TextView) findViewById(R.id.lblGongWenFaBu_XiangXi_TongZhiHaoVal);
        lblTongZhiHao.setText(strTongZhiHao);
        lblShouXinRen = (TextView) findViewById(R.id.lblGongWenFaBu_XiangXi_ShouXinRenVal);
        lblShouXinRen.setText(strShouXinRen);
        lblBuMen = (TextView) findViewById(R.id.lblGongWenFaBu_XiangXi_BuMenVal);
        lblBuMen.setText(strBuMen);
        lblFaBuShiJian = (TextView) findViewById(R.id.lblGongWenFaBu_XiangXi_FaBuShiJianVal);
        lblFaBuShiJian.setText(strFaBuShiJian);
        lblLiuZhuanRen = (TextView) findViewById(R.id.lblGongWenFaBu_XiangXi_LiuZhuanRenVal);
        lblLiuZhuanRen.setText(strLiuZhuanRen);
        lblLiuZhuanShiJian = (TextView) findViewById(R.id.lblGongWenFaBu_XiangXi_LiuZhuanShiJianVal);
        lblLiuZhuanShiJian.setText(strLiuZhuanShiJian);
        lblGongWenNeiRong = (EditText) findViewById(R.id.txtGongWenFaBu_XiangXi_NeiRong);
        lblGongWenNeiRong.setText(strGongWenNeiRong);
        lblLiuZhuanNeiRong = (EditText) findViewById(R.id.txtGongWenFaBu_XiangXi_PiShi);
        lblLiuZhuanNeiRong.setText(strLiuZhuanNeiRong);
        lblFileName = (TextView) findViewById(R.id.lblGongWenFaBu_XiangXi_App);
        if (strFileName != null)
            lblFileName.setText(getString(R.string.fujian) + strFileName);

        btnOK = (Button) findViewById(R.id.btnGongWenFaBu_XiangXi_FaBu);
        btnOK.setOnClickListener(onClickListener);
    }

    private void initHandler()
    {
        handler = new JsonHttpResponseHandler()
        {
            int result = 0;
            long nRetVal = 0;

            @Override
            public void onSuccess(JSONObject jsonData)
            {
                result = 1;

                nRetVal = CommMgr.commService.parseSetGongWenItem(jsonData);
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
                    GlobalData.showToast(GongWenFaBu_XiangXiActivity.this, getString(R.string.network_error));
                }
                else
                {
                    if (nRetVal == -1)
                        GlobalData.showToast(GongWenFaBu_XiangXiActivity.this, getString(R.string.param_error));
                    if (nRetVal == 500)
                        GlobalData.showToast(GongWenFaBu_XiangXiActivity.this, getString(R.string.service_error));
                    if (nRetVal == 0)
                    {
                        GlobalData.showToast(GongWenFaBu_XiangXiActivity.this, getString(R.string.service_success));
                        finish();
                    }
                }
            }
        };

        return;
    }
}
