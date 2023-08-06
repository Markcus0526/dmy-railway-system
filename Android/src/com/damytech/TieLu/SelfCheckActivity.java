package com.damytech.TieLu;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Context;
import android.graphics.Rect;
import android.os.Bundle;
import android.view.*;
import android.widget.*;
import com.damytech.HttpConn.JsonHttpResponseHandler;
import com.damytech.STData.*;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;
import org.json.JSONObject;
import java.util.ArrayList;

public class SelfCheckActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    LinearLayout rlData = null;

    ArrayList<STSelfCheck> arrInfos = new ArrayList<STSelfCheck>();
    private JsonHttpResponseHandler handler = null;
    private ProgressDialog dialog = null;

    /**
     * Called when the activity is first created.
     */
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.selfcheck_activity);

        mainLayout = (RelativeLayout)findViewById(R.id.parent_layout);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.height(), r.width());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.parent_layout));
                            bInitialized = true;
                        }
                    }
                }
        );

        String strFindTime = getIntent().getStringExtra("FINDTIME");
        long nSectorID = getIntent().getLongExtra("SECTORID", 0);

        initControl();
        initHandler();

        dialog = ProgressDialog.show(
                SelfCheckActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetSelfCheckList(handler, strFindTime, Long.toString(nSectorID));
    }

    private void initControl()
    {
        rlData = (LinearLayout)findViewById(R.id.rlDataList);
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

                CommMgr.commService.parseGetSelfCheckList(jsonData, arrInfos);
                if (arrInfos == null)
                    result = 0;
            }

            @Override
            public void onFailure(Throwable ex, String exception)
            {
                dialog.dismiss();
                result = 0;
            }

            @Override
            public void onFinish()
            {
                if (result == 0)
                {
                    GlobalData.showToast(SelfCheckActivity.this, getString(R.string.network_error));
                    SelfCheckActivity.this.finish();
                }
                else
                {
                    if (arrInfos != null && arrInfos.size() > 0)
                    {
                        ShowInfos();
                    }
                }
            }
        };
    }

    private void ShowInfos()
    {
        for (int i = 0; i < arrInfos.size(); i++)
        {
            View v = null;
            LayoutInflater inflater = (LayoutInflater)SelfCheckActivity.this.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            v = inflater.inflate(R.layout.selfcheck_view, null);
            Rect r = new Rect();
            mainLayout.getLocalVisibleRect(r);
            ResolutionSet._instance.setResolution(r.height(), r.width());
            ResolutionSet._instance.iterateChild(v.findViewById(R.id.parent_layout));

            STSelfCheck stInfo = arrInfos.get(i);

            TextView lblCheDui = (TextView) v.findViewById(R.id.lblCheDui);
            lblCheDui.setText("" + stInfo.teamname);
            TextView lblBanZu = (TextView) v.findViewById(R.id.lblBanZu);
            lblBanZu.setText("" + stInfo.groupname);
            TextView lblOne = (TextView) v.findViewById(R.id.lblOne);
            lblOne.setText("" + stInfo.one);
            TextView lblTwo = (TextView) v.findViewById(R.id.lblTwo);
            lblTwo.setText("" + stInfo.two);
            TextView lblThree = (TextView) v.findViewById(R.id.lblThree);
            lblThree.setText("" + stInfo.three);
            TextView lblFour = (TextView) v.findViewById(R.id.lblFour);
            lblFour.setText("" + stInfo.four);
            TextView lblFive = (TextView) v.findViewById(R.id.lblFive);
            lblFive.setText("" + stInfo.seven);
            TextView lblSix = (TextView) v.findViewById(R.id.lblSix);
            lblSix.setText("" + stInfo.eight);

            try {
                rlData.addView(v);
            } catch (Exception ex) {
            }
        }

        return;
    }
}
