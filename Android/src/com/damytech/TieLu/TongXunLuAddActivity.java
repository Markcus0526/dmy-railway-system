package com.damytech.TieLu;

import android.app.Activity;
import android.content.Intent;
import android.graphics.Rect;
import android.os.Bundle;
import android.view.View;
import android.view.ViewTreeObserver;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.RelativeLayout;
import com.damytech.utils.ResolutionSet;

public class TongXunLuAddActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    private ImageView imgBack = null;
    private Button btnOK = null;
    private Button btnCancel = null;

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgTongXunLuAdd_Back:
                    finish();
                    break;
                case R.id.btnTongXunLuAdd_OK:
                    finish();
                    break;
                case R.id.btnTongXunLuAdd_Cancel:
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
        setContentView(R.layout.tongxunluadd_activity);

        mainLayout = (RelativeLayout)findViewById(R.id.rlTongXunLuAddBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlTongXunLuAddBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        initControl();
    }

    private void initControl()
    {
        imgBack = (ImageView) findViewById(R.id.imgTongXunLuAdd_Back);
        imgBack.setOnClickListener(onClickListener);

        btnOK = (Button) findViewById(R.id.btnTongXunLuAdd_OK);
        btnOK.setOnClickListener(onClickListener);

        btnCancel = (Button) findViewById(R.id.btnTongXunLuAdd_Cancel);
        btnCancel.setOnClickListener(onClickListener);
    }
}
