package com.damytech.TieLu;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.Intent;
import android.graphics.Rect;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.ViewTreeObserver;
import android.widget.*;
import com.damytech.HttpConn.JsonHttpResponseHandler;
import com.damytech.STData.STEmailInfo;
import com.damytech.STData.STOpinionData;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;
import org.json.JSONObject;
import org.w3c.dom.Text;

import java.util.ArrayList;

public class WoDeSuQiuXiangXiActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    TextView textTitle;
    TextView textDate;
    TextView textContent;
    TextView textFeedback;
    ImageView imgBack;

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
            }
        }
    };
    /**
     * Called when the activity is first created.
     */
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.wodesuqiuxiangxi_activity);

        mainLayout = (RelativeLayout)findViewById(R.id.rlBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        initControl();
    }

    private void initControl()
    {
        textTitle = (TextView) findViewById(R.id.textTitle);
        textTitle.setText(getIntent().getStringExtra("title"));
        textContent = (TextView) findViewById(R.id.textContent);
        textContent.setText(getIntent().getStringExtra("content"));
        textFeedback = (TextView) findViewById(R.id.textFeedback);
        textFeedback.setText(getIntent().getStringExtra("feedback"));
        textDate = (TextView) findViewById(R.id.textPostDateVal);
        textDate.setText(getIntent().getStringExtra("postdate"));

        imgBack = (ImageView) findViewById(R.id.imageBack);
        imgBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                WoDeSuQiuXiangXiActivity.this.finish();
            }
        });
    }

    @Override
    public void onResume()
    {
        super.onResume();
    }
}
