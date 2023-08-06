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

public class CombineCheckActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    LinearLayout rlData = null;

    ArrayList<STCombineCheckInfo> arrInfos = new ArrayList<STCombineCheckInfo>();
    private JsonHttpResponseHandler handler = null;
    private ProgressDialog dialog = null;

    /**
     * Called when the activity is first created.
     */
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.combinecheck_jieguo_activity);

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
                CombineCheckActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetCombineCheckList(handler, strFindTime, Long.toString(nSectorID));
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

                CommMgr.commService.parseGetCombineCheckList(jsonData, arrInfos);
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
                    GlobalData.showToast(CombineCheckActivity.this, getString(R.string.network_error));
                    CombineCheckActivity.this.finish();
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
            LayoutInflater inflater = (LayoutInflater)CombineCheckActivity.this.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            v = inflater.inflate(R.layout.combinecheck_jieguo_view, null);
            Rect r = new Rect();
            mainLayout.getLocalVisibleRect(r);
            ResolutionSet._instance.setResolution(r.height(), r.width());
            ResolutionSet._instance.iterateChild(v.findViewById(R.id.parent_layout));

            STCombineCheckInfo stInfo = arrInfos.get(i);

            TextView lblCheDui = (TextView) v.findViewById(R.id.lblCheDui);
            lblCheDui.setText("" + stInfo.CheDui);
            TextView lblBanZu = (TextView) v.findViewById(R.id.lblBanZu);
            lblBanZu.setText("" + stInfo.BanZu);
            TextView lblCheDuiZiCha = (TextView) v.findViewById(R.id.lblOne);
            lblCheDuiZiCha.setText("" + stInfo.oneval);
            TextView lblBanZuZiCha = (TextView) v.findViewById(R.id.lblTwo);
            lblBanZuZiCha.setText("" + stInfo.twoval);
            TextView lblDuanJiYiShang = (TextView) v.findViewById(R.id.lblThree);
            lblDuanJiYiShang.setText("" + stInfo.threeval);

            try {
                rlData.addView(v);
            } catch (Exception ex) {
            }
        }

        return;
    }
}
