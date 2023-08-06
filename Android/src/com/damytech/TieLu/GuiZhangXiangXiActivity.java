package com.damytech.TieLu;

import android.app.Activity;
import android.app.ProgressDialog;
import android.graphics.Color;
import android.graphics.Rect;
import android.os.Bundle;
import android.util.TypedValue;
import android.view.Gravity;
import android.view.View;
import android.view.ViewTreeObserver;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.*;
import com.damytech.HttpConn.JsonHttpResponseHandler;
import com.damytech.STData.STDetRule;
import com.damytech.STData.STRenWuJieDaoItem;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.HorizontalPager;
import com.damytech.utils.NoZoomControllWebView;
import com.damytech.utils.ResolutionSet;
import org.json.JSONObject;

import java.util.ArrayList;

public class GuiZhangXiangXiActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    private ImageView imgBack = null;
    private NoZoomControllWebView webView = null;
    private TextView lblTitle = null;

    long ruleid = 0;

    STDetRule detRule = new STDetRule();
    JsonHttpResponseHandler handler = new JsonHttpResponseHandler()
    {
        int result = 0;

        @Override
        public void onSuccess(JSONObject jsonData)
        {
            result = 1;
            dialog.dismiss();

            CommMgr.commService.parseGetDetRule(jsonData, detRule);
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
                GlobalData.showToast(GuiZhangXiangXiActivity.this, getString(R.string.network_error));
                GuiZhangXiangXiActivity.this.finish();
            }
            else
            {
                RefreshPage();
            }
        }
    };
    ProgressDialog dialog = null;

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgGuiZhangXiangXi_Back:
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
        setContentView(R.layout.guizhangxiangxi_activity);

        mainLayout = (RelativeLayout)findViewById(R.id.rlGuiZhangXiangXiBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlGuiZhangXiangXiBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        ruleid = getIntent().getLongExtra("uid", 0);

        initControl();

        dialog = ProgressDialog.show(
                GuiZhangXiangXiActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetDetRule(handler, Long.toString(ruleid));
    }

    private void initControl()
    {
        imgBack = (ImageView) findViewById(R.id.imgGuiZhangXiangXi_Back);
        imgBack.setOnClickListener(onClickListener);

        lblTitle = (TextView) findViewById(R.id.lblTitle);
        webView = (NoZoomControllWebView) findViewById(R.id.view);
        webView.setWebViewClient(new WebViewClient() {
            @Override
            public boolean shouldOverrideUrlLoading(WebView view, String url)
            {
                view.loadUrl(url);
                return true;
            };
        });
    }

    private void RefreshPage()
    {
        lblTitle.setText(detRule.title);

        webView.setBackgroundColor(Color.parseColor("#FFFFFFFF"));
        webView.getSettings().setLoadWithOverviewMode(true);
        webView.getSettings().setUseWideViewPort(true);
        webView.getSettings().setBuiltInZoomControls(false);
        webView.getSettings().setDisplayZoomControls(false);
        webView.getSettings().setSupportZoom(false);
        webView.setScrollBarStyle(WebView.SCROLLBARS_OUTSIDE_OVERLAY);
        webView.getSettings().setDefaultTextEncodingName("UTF-8");
        webView.getSettings().setJavaScriptEnabled(true);
        webView.getSettings().setDefaultZoom(WebSettings.ZoomDensity.CLOSE);

        StringBuilder strData = new StringBuilder();
        strData.append("<html><head><meta content=\"width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no\" name=\"viewport\">");
        strData.append("<meta content=\"no-cache,must-revalidate\" http-equiv=\"Cache-Control\">");
        strData.append("<meta content=\"no-cache\" http-equiv=\"pragma\">");
        strData.append("<meta content=\"0\" http-equiv=\"expires\">");
        strData.append("<meta content=\"telephone=no, address=no\" name=\"format-detection\">" + "<style>img { width:100%;} </style>" + "</head>");
        strData.append("<body>");
        strData.append(GlobalData.unescape(detRule.content));
        strData.append("</body></html>");
        webView.loadData(strData.toString(), "text/html; charset=UTF-8", "");

        return;
    }
}
