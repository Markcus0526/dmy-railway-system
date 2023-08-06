package com.damytech.TieLu;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Intent;
import android.graphics.Rect;
import android.os.Bundle;
import android.view.View;
import android.view.ViewTreeObserver;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.RelativeLayout;
import android.widget.TextView;
import com.damytech.HttpConn.JsonHttpResponseHandler;
import com.damytech.STData.STDaiQianItem;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;
import com.damytech.utils.SmartImageView.Global;
import org.json.JSONObject;

public class GongWenDaiQianActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;
    int nPageNo = 0;

    private ImageView imgBack = null;
    private RelativeLayout rlFaBu = null;
    private RelativeLayout rlYiShou = null;
    private RelativeLayout rlChaXun = null;
    private TextView lblBiaoTi = null;
    private TextView lblBuMen = null;
    private TextView lblDate = null;
    private TextView lblFaBuRen = null;
    private TextView lblTongZhiHao = null;
    private TextView lblFileName = null;
    private Button btnPrev = null;
    private Button btnNext = null;
    private Button btnXiangXi = null;
    private Button btnLiuZhuan = null;
    private Button btnPublished = null;

    private STDaiQianItem stInfo = new STDaiQianItem();
    private JsonHttpResponseHandler handler = null;
    private ProgressDialog dialog = null;

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgGongWenDaiQian_Back:
                    finish();
                    break;
                case R.id.rlGongWenDaiQian_FaBu:
                    Intent intentFaBu = new Intent(GongWenDaiQianActivity.this, GongWenFaBuActivity.class);
                    startActivity(intentFaBu);
                    finish();
                    break;
                case R.id.rlGongWenDaiQian_YiShou:
                    Intent intentYiShou = new Intent(GongWenDaiQianActivity.this, GongWenYiShouActivity.class);
                    startActivity(intentYiShou);
                    finish();
                    break;
                case R.id.rlGongWenDaiQian_ChaXun:
                    Intent intentChaXun = new Intent(GongWenDaiQianActivity.this, GongWenChaXunActivity.class);
                    startActivity(intentChaXun);
                    finish();
                    break;
                case R.id.btnGongWenDaiQian_ShangYiYe:
                    {
                        if (nPageNo <= 0)
                        {
                            return;
                        }
                        else
                        {
                            nPageNo--;
                            dialog = ProgressDialog.show(
                                    GongWenDaiQianActivity.this,
                                    "",
                                    getString(R.string.waiting),
                                    true,
                                    false,
                                    null
                            );
                            CommMgr.commService.GetDaiQianItem(handler, Long.toString(GlobalData.GetUid(GongWenDaiQianActivity.this)), Integer.toString(nPageNo));
                        }
                    }
                    break;
                case R.id.btnGongWenDaiQian_XiaYiYe:
                    {
                        if (stInfo == null)
                            return;
                        else
                        {
                            if ( (nPageNo+1) >= stInfo.total )
                            {
                                return;
                            }
                            else
                            {
                                nPageNo++;
                                dialog = ProgressDialog.show(
                                        GongWenDaiQianActivity.this,
                                        "",
                                        getString(R.string.waiting),
                                        true,
                                        false,
                                        null
                                );
                                CommMgr.commService.GetDaiQianItem(handler, Long.toString(GlobalData.GetUid(GongWenDaiQianActivity.this)), Integer.toString(nPageNo));
                            }
                        }
                    }
                    break;
                case R.id.btnGongWenDaiQian_XiangQing:
                    if (stInfo != null && stInfo.uid > 0)
                    {
                        Intent intent = new Intent(GongWenDaiQianActivity.this, GongWenXiangQingActivity.class);
                        intent.putExtra("uid", stInfo.uid);
                        intent.putExtra("QianShouFlag", 1);
                        startActivity(intent);
                    }
                    break;
                case R.id.btnGongWenDaiQian_LiuZhuan:
                    if (stInfo != null && stInfo.uid > 0)
                    {
                        Intent intent = new Intent(GongWenDaiQianActivity.this, GongWenLiuZhuanActivity.class);
                        intent.putExtra("Data", stInfo);
                        intent.putExtra("LiuZhuanFlag", 0);
                        startActivity(intent);
                    }
                    break;
                case R.id.btnPublished:
                    Intent intent_published = new Intent(GongWenDaiQianActivity.this, YiFaBuGongWenActivity.class);
                    startActivity(intent_published);
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
        setContentView(R.layout.gongwendaiqian_activity);

        mainLayout = (RelativeLayout)findViewById(R.id.rlGongWenDaiQianBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlGongWenDaiQianBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        initControl();
        initHandler();

        dialog = ProgressDialog.show(
                GongWenDaiQianActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetDaiQianItem(handler, Long.toString(GlobalData.GetUid(GongWenDaiQianActivity.this)), Integer.toString(nPageNo));
    }

    private void initControl()
    {
        imgBack = (ImageView) findViewById(R.id.imgGongWenDaiQian_Back);
        imgBack.setOnClickListener(onClickListener);

        rlFaBu = (RelativeLayout) findViewById(R.id.rlGongWenDaiQian_FaBu);
        rlFaBu.setOnClickListener(onClickListener);

        rlYiShou = (RelativeLayout) findViewById(R.id.rlGongWenDaiQian_YiShou);
        rlYiShou.setOnClickListener(onClickListener);

        rlChaXun = (RelativeLayout) findViewById(R.id.rlGongWenDaiQian_ChaXun);
        rlChaXun.setOnClickListener(onClickListener);

        lblBiaoTi = (TextView) findViewById(R.id.lblGongWenDaiQian_GongWenBiaoTiVal);
        lblBuMen = (TextView) findViewById(R.id.lblGongWenDaiQian_BuMenVal);
        lblDate = (TextView) findViewById(R.id.lblGongWenDaiQian_ShiJianVal);
        lblFaBuRen = (TextView) findViewById(R.id.lblGongWenDaiQian_ShouXinRenVal);
        lblTongZhiHao = (TextView) findViewById(R.id.lblGongWenDaiQian_TongZhiHaoVal);
        lblFileName = (TextView) findViewById(R.id.lblGongWenDaiQian_FuJian);

        btnPrev = (Button) findViewById(R.id.btnGongWenDaiQian_ShangYiYe);
        btnPrev.setOnClickListener(onClickListener);

        btnNext = (Button) findViewById(R.id.btnGongWenDaiQian_XiaYiYe);
        btnNext.setOnClickListener(onClickListener);

        btnXiangXi = (Button) findViewById(R.id.btnGongWenDaiQian_XiangQing);
        btnXiangXi.setOnClickListener(onClickListener);

        btnLiuZhuan = (Button) findViewById(R.id.btnGongWenDaiQian_LiuZhuan);
        btnLiuZhuan.setOnClickListener(onClickListener);

        btnPublished = (Button) findViewById(R.id.btnPublished);
        btnPublished.setOnClickListener(onClickListener);
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

                nRetVal = CommMgr.commService.parseGetDaiQianItem(jsonData, stInfo);
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
                    GlobalData.showToast(GongWenDaiQianActivity.this, getString(R.string.network_error));
                }
                else
                {
                    if (nRetVal == 0 && stInfo == null)
                        GlobalData.showToast(GongWenDaiQianActivity.this, getString(R.string.noexist_data));
                    if (nRetVal == 500)
                        GlobalData.showToast(GongWenDaiQianActivity.this, getString(R.string.service_error));
                    if (stInfo != null)
                    {
                        ShowData();
                    }
                }
            }
        };

        return;
    }

    private void ShowData()
    {
        lblBiaoTi.setText(stInfo.title);
        lblBuMen.setText(stInfo.bumen);
        lblDate.setText(stInfo.fbdate);
        lblFaBuRen.setText(stInfo.faburen);
        lblTongZhiHao.setText(stInfo.tongzhihao);
        lblFileName.setText(getString(R.string.fujian) + " " + stInfo.filename);

        return;
    }
}
