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

public class JiFenChaXunJieGuoActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    LinearLayout rlData = null;

    ArrayList<STCreditInfo> arrInfos = new ArrayList<STCreditInfo>();
    private JsonHttpResponseHandler handler = null;
    private ProgressDialog dialog = null;

    /**
     * Called when the activity is first created.
     */
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.jifenchaxun_jieguo_activity);

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
        String strName = getIntent().getStringExtra("NAME");
        long nSectorID = getIntent().getLongExtra("SECTORID", 0);
        long nRouteID = getIntent().getLongExtra("ROUTEID", 0);

        initControl();
        initHandler();

        dialog = ProgressDialog.show(
                JiFenChaXunJieGuoActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetCreditList(handler, strFindTime, strName, Long.toString(nSectorID), Long.toString(nRouteID));
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

                CommMgr.commService.parseGetCreditList(jsonData, arrInfos);
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
                    GlobalData.showToast(JiFenChaXunJieGuoActivity.this, getString(R.string.network_error));
                    JiFenChaXunJieGuoActivity.this.finish();
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
            LayoutInflater inflater = (LayoutInflater)JiFenChaXunJieGuoActivity.this.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            v = inflater.inflate(R.layout.jifenchaxun_jieguo_view, null);
            Rect r = new Rect();
            mainLayout.getLocalVisibleRect(r);
            ResolutionSet._instance.setResolution(r.height(), r.width());
            ResolutionSet._instance.iterateChild(v.findViewById(R.id.parent_layout));

            STCreditInfo stInfo = arrInfos.get(i);

            TextView lblCheDui = (TextView) v.findViewById(R.id.lblCheDui);
            lblCheDui.setText("" + stInfo.chedui);
            TextView lblBanZu = (TextView) v.findViewById(R.id.lblBanZu);
            lblBanZu.setText("" + stInfo.banzu);
            TextView lblXingMing = (TextView) v.findViewById(R.id.lblOne);
            lblXingMing.setText("" + stInfo.name);
            TextView lblJiDuan = (TextView) v.findViewById(R.id.lblTwo);
            lblJiDuan.setText("" + stInfo.duanjiyishang);
            TextView lblBanZuCheDui = (TextView) v.findViewById(R.id.lblThree);
            lblBanZuCheDui.setText("" + stInfo.banzuchedui);
            TextView lblLianGua = (TextView) v.findViewById(R.id.lblFour);
            lblLianGua.setText("" + stInfo.liangua);
            TextView lblLiGang = (TextView) v.findViewById(R.id.lblFive);
            lblLiGang.setText("" + stInfo.ligang);
            TextView lblDiaoZheng = (TextView) v.findViewById(R.id.lblSix);
            lblDiaoZheng.setText("" + stInfo.diaozheng);
            TextView lblBenYue = (TextView) v.findViewById(R.id.lblSeven);
            lblBenYue.setText("" + stInfo.benyue);
            TextView lblJiLi = (TextView) v.findViewById(R.id.lblEight);
            lblJiLi.setText("" + stInfo.jili);
            TextView lblLeiJi = (TextView) v.findViewById(R.id.lblLeiJi);
            lblLeiJi.setText("" + stInfo.leiji);

            try {
                rlData.addView(v);
            } catch (Exception ex) {
            }
        }

        return;
    }
}
