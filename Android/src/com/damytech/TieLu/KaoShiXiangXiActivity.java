package com.damytech.TieLu;

import android.app.Activity;
import android.content.Intent;
import android.graphics.Rect;
import android.os.Bundle;
import android.view.View;
import android.view.ViewTreeObserver;
import android.widget.*;
import com.damytech.STData.STKaoShiItem;
import com.damytech.utils.ResolutionSet;

public class KaoShiXiangXiActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    private ImageView imgBack = null;
    private TextView lblBiaoTi = null;
    private TextView lblShiJian = null;
    private TextView lblKaoTiShu = null;
    private EditText txtNeiRong = null;
    private Button btnKaiShi = null;

    STKaoShiItem itemData = new STKaoShiItem();

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgKaoShiXiangXi_Back:
                    finish();
                    break;

                case R.id.btnKaoShiXiangXi_KaiShi:
                    Intent intentWenTi = new Intent(KaoShiXiangXiActivity.this, KaoShiWenTiActivity.class);
                    intentWenTi.putExtra("Data", itemData);
                    startActivity(intentWenTi);
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
        setContentView(R.layout.kaoshixiangxi_activity);

        itemData = (STKaoShiItem)getIntent().getParcelableExtra("Data");

        mainLayout = (RelativeLayout)findViewById(R.id.rlKaoShiXiangXiBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlKaoShiXiangXiBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        initControl();
    }

    private void initControl()
    {
        imgBack = (ImageView) findViewById(R.id.imgKaoShiXiangXi_Back);
        imgBack.setOnClickListener(onClickListener);

        lblBiaoTi = (TextView) findViewById(R.id.lblKaoShiXiangXi_BiaoTiVal);
        lblShiJian = (TextView) findViewById(R.id.lblKaoShiXiangXi_ShiJianVal);
        lblKaoTiShu = (TextView) findViewById(R.id.lblKaoShiXiangXi_KaoTiShuVal);
        txtNeiRong = (EditText) findViewById(R.id.txtKaoShiXiangXi_NeiRongVal);

        if (itemData != null)
        {
            lblBiaoTi.setText(itemData.title);
            lblShiJian.setText(itemData.examtime + getString(R.string.fenzhong));
            lblKaoTiShu.setText(itemData.problems + getString(R.string.geti));
            txtNeiRong.setText(itemData.content);
        }

        Button btnKaiShi = (Button) findViewById(R.id.btnKaoShiXiangXi_KaiShi);
        btnKaiShi.setOnClickListener(onClickListener);
    }
}
