package com.damytech.TieLu;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Intent;
import android.graphics.Rect;
import android.os.Bundle;
import android.view.View;
import android.view.ViewTreeObserver;
import android.widget.ImageView;
import android.widget.RelativeLayout;
import android.widget.TextView;
import com.damytech.HttpConn.JsonHttpResponseHandler;
import com.damytech.STData.STKaoHeJiLuDet;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;
import com.damytech.utils.SmartImageView.SmartImageView;
import org.json.JSONObject;

public class KaoHeChaXun_XiangXiActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    long nID = 0;
    private ImageView imgBack = null;
    private TextView lblTitle = null;
    private TextView lblCheDui = null;
    private TextView lblBanZu = null;
    private TextView lblZeRenRen = null;
    private TextView lblLieCheZhang = null;
    private SmartImageView imgPhoto = null;
    private TextView lblData = null;

    STKaoHeJiLuDet stDet = new STKaoHeJiLuDet();
    private ProgressDialog dialog = null;
    private JsonHttpResponseHandler handler = null;

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgKaoHeChaXun_XiangXi_Back:
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
        setContentView(R.layout.kaohechaxun_xiangxi_activity);

        nID = getIntent().getLongExtra("ID", 0);

        mainLayout = (RelativeLayout)findViewById(R.id.rlKaoHeChaXun_XiangXiBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlKaoHeChaXun_XiangXiBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        initControl();
        initHandler();

        dialog = ProgressDialog.show(
                KaoHeChaXun_XiangXiActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetKaoHeJiLuDet(handler, Long.toString(nID));
    }

    private void initControl()
    {
        imgBack = (ImageView) findViewById(R.id.imgKaoHeChaXun_XiangXi_Back);
        imgBack.setOnClickListener(onClickListener);

        lblTitle = (TextView) findViewById(R.id.lblKaoHeChaXun_XiangXi_WenTi);
        lblCheDui = (TextView) findViewById(R.id.lblKaoHeChaXun_XiangXi_CheDui);
        lblBanZu = (TextView) findViewById(R.id.lblKaoHeChaXun_XiangXi_BanZu);
        lblZeRenRen = (TextView) findViewById(R.id.lblKaoHeChaXun_XiangXi_FuZeRen);
        lblLieCheZhang = (TextView) findViewById(R.id.lblKaoHeChaXun_XiangXi_LieCheZhang);
        lblData = (TextView) findViewById(R.id.lblKaoHeChaXun_XiangXi_Data);
        imgPhoto = (SmartImageView) findViewById(R.id.imgKaoHeChaXun_XiangXi_Photo);
    }

    private void initHandler()
    {
        handler = new JsonHttpResponseHandler()
        {
            int result = 0;

            @Override
            public void onSuccess(JSONObject jsonData)
            {
                result = 1;

                CommMgr.commService.parseGetKaoHeJiLuDet(jsonData, stDet);
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
                    GlobalData.showToast(KaoHeChaXun_XiangXiActivity.this, getString(R.string.network_error));
                    KaoHeChaXun_XiangXiActivity.this.finish();
                }
                else
                {
                    if (stDet != null)
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
        lblTitle.setText(getString(R.string.wenti) + stDet.title);
        lblCheDui.setText(getString(R.string.chedui) + stDet.chedui);
        lblBanZu.setText(getString(R.string.banzu) + stDet.banzu);
        lblZeRenRen.setText(getString(R.string.zerenren) + stDet.zerenren + ", " + getString(R.string.koufen) + Long.toString(stDet.chkpoint));
        if (stDet.liechezhang != null && stDet.liechezhang.length() > 0)
            lblLieCheZhang.setText(getString(R.string.fuzeliechezhang) + stDet.liechezhang + ", " + getString(R.string.koufen) + stDet.relpoint);
        lblData.setText(stDet.chkdata);
        imgPhoto.setImageUrl(stDet.imgpath, R.drawable.defphoto);

        return;
    }
}
