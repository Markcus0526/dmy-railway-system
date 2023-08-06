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

public class KaoHeYuLanActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    private ImageView imgBack = null;
    private TextView lblTitle = null;
    private TextView lblCheDui = null;
    private TextView lblBanZu = null;
    private TextView lblZeRenRen = null;
    private TextView lblLieCheZhang = null;
    private ImageView imgPhoto = null;
    private TextView lblData = null;
    private Button btnOK = null;

    private String strTitle = "";
    private String strStartTime = "";
    private String strCheckTime = "";
    private String strCheDui = "";
    private long nCheDui = 0;
    private String strBanZu = "";
    private long nBanZu = 0;
    private String strZeRenRen = "";
    private long nZeRenRen = 0;
    private String strLieCheZhang = "";
    private long nLieCheZhang = 0;
    private long nXiangDian = 0;
    private int nChkPoint = 0;
    private String strRelPoint = "";
    private String strData = "";
    private Bitmap bmpPhoto = null;
    private String strImgData = "";

    private JsonHttpResponseHandler handler = null;
    ProgressDialog dialog = null;

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgKaoHeLuRu_Back:
                    finish();
                    break;
                case R.id.btnKaoHeYuLan_OK:
                    try {
                        ByteArrayOutputStream bytes = new ByteArrayOutputStream();
                        bmpPhoto.compress(Bitmap.CompressFormat.JPEG, 100, bytes);
                        strImgData = Base64.encodeToString(bytes.toByteArray(), Base64.DEFAULT);
                    }catch (Exception e)
                    {
                        strImgData = "";
                    }

                    dialog = ProgressDialog.show(
                            KaoHeYuLanActivity.this,
                            "",
                            getString(R.string.waiting),
                            true,
                            false,
                            null
                    );

                    CommMgr.commService.SetKaoHeItem(handler,
                            Long.toString(GlobalData.GetUid(KaoHeYuLanActivity.this)),
                            strStartTime,
                            strCheckTime,
                            Long.toString(nCheDui),
                            Long.toString(nBanZu),
                            Long.toString(nZeRenRen),
                            Long.toString(nLieCheZhang),
                            Long.toString(nXiangDian),
                            strData,
                            strImgData);
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
        setContentView(R.layout.kaoheluru_yulan_activity);

        strTitle = getIntent().getStringExtra("Title");
        strStartTime = getIntent().getStringExtra("StartTime");
        strCheckTime = getIntent().getStringExtra("CheckTime");
        strCheDui = getIntent().getStringExtra("CheDui");
        nCheDui = getIntent().getLongExtra("CheDuiVal", 0);
        strBanZu = getIntent().getStringExtra("BanZu");
        nBanZu = getIntent().getLongExtra("BanZuVal", 0);
        strZeRenRen = getIntent().getStringExtra("ZeRenRen");
        nZeRenRen = getIntent().getLongExtra("ZeRenRenVal", 0);
        nChkPoint = getIntent().getIntExtra("ChkPoint", 0);
        strLieCheZhang = getIntent().getStringExtra("LieCheZhang");
        nLieCheZhang = getIntent().getLongExtra("LieCheZhangVal", 0);
        nXiangDian = getIntent().getLongExtra("XiangDianVal", 0);
        strRelPoint =  getIntent().getStringExtra("RelPoint");
        strData = getIntent().getStringExtra("Data");
        byte[] bis = getIntent().getByteArrayExtra("Photo");
        if (bis != null)
            bmpPhoto = BitmapFactory.decodeByteArray(bis, 0, bis.length);
        else
            bmpPhoto = null;

        mainLayout = (RelativeLayout)findViewById(R.id.rlKaoHeYuLan_YuLanBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlKaoHeYuLan_YuLanBack));
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
        imgBack = (ImageView) findViewById(R.id.imgKaoHeYuLan_Back);
        imgBack.setOnClickListener(onClickListener);

        lblTitle = (TextView) findViewById(R.id.lblKaoHeYuLan_WenTi);
        lblTitle.setText(getString(R.string.wenti) + strTitle);
        lblCheDui = (TextView) findViewById(R.id.lblKaoHeYuLan_CheDui);
        lblCheDui.setText(getString(R.string.chedui) + strCheDui);
        lblBanZu = (TextView) findViewById(R.id.lblKaoHeYuLan_BanZu);
        lblBanZu.setText(getString(R.string.banzu) + strBanZu);
        lblZeRenRen = (TextView) findViewById(R.id.lblKaoHeYuLan_FuZeRen);
        lblZeRenRen.setText(getString(R.string.zerenren) + strZeRenRen + ", " + getString(R.string.jianfen) + Integer.toString(nChkPoint));
        lblLieCheZhang = (TextView) findViewById(R.id.lblKaoHeYuLan_LieCheZhang);
        if (nLieCheZhang != 0)
        {
            if (strRelPoint != null && strRelPoint.equals("null") == false )
                lblLieCheZhang.setText(getString(R.string.liangualiechezhang1) + strLieCheZhang + ", " + getString(R.string.jianfen) + strRelPoint);
            else
                lblLieCheZhang.setText(getString(R.string.liangualiechezhang1) + strLieCheZhang);
        }
        lblData = (TextView) findViewById(R.id.lblKaoHeYuLan_Data);
        lblData.setText(strData);

        imgPhoto = (ImageView) findViewById(R.id.imgKaoHeYuLan_Photo);
        imgPhoto.setImageBitmap(bmpPhoto);

        btnOK = (Button) findViewById(R.id.btnKaoHeYuLan_OK);
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
                dialog.dismiss();

                nRetVal = CommMgr.commService.parseSetKaoHeItem(jsonData);
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
                    GlobalData.showToast(KaoHeYuLanActivity.this, getString(R.string.network_error));
                }
                else
                {
                    if (nRetVal == -1)
                        GlobalData.showToast(KaoHeYuLanActivity.this, getString(R.string.param_error));
                    if (nRetVal == 500)
                        GlobalData.showToast(KaoHeYuLanActivity.this, getString(R.string.service_error));
                    if (nRetVal == 0)
                    {
                        GlobalData.showToast(KaoHeYuLanActivity.this, getString(R.string.service_success));
                        finish();
                    }
                }
            }
        };

        return;
    }
}
