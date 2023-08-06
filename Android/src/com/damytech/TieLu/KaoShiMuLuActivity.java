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
import com.damytech.STData.STKaoShiLiShi;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;
import org.json.JSONObject;
import java.util.ArrayList;

public class KaoShiMuLuActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    private ImageView imgBack = null;
    private RelativeLayout rlDuanJi = null;
    private RelativeLayout rlCheDui = null;

    private ListView listData = null;
    private ItemAdapter adapter = null;

    private ArrayList<STKaoShiLiShi> arrData = new ArrayList<STKaoShiLiShi>();
    private JsonHttpResponseHandler handler = null;
    private ProgressDialog dialog = null;

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgKaoShiMuLu_Back:
                    finish();
                    break;
                case R.id.rlKaoShiMuLu_DuanJiKaoShi:
                    Intent intentDuanJi = new Intent(KaoShiMuLuActivity.this, KaoShiDuanJiActivity.class);
                    startActivity(intentDuanJi);
                    finish();
                    break;
                case R.id.rlKaoShiMuLu_CheDuiKaoShi:
                    Intent intentCheDui = new Intent(KaoShiMuLuActivity.this, KaoShiCheDuiActivity.class);
                    startActivity(intentCheDui);
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
        setContentView(R.layout.kaoshimulu_activity);

        mainLayout = (RelativeLayout)findViewById(R.id.rlKaoShiMuLuBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlKaoShiMuLuBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        initControl();
        initHandler();

        dialog = ProgressDialog.show(
                KaoShiMuLuActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetKaoShiLiShiList(handler, Long.toString(GlobalData.GetUid(KaoShiMuLuActivity.this)));
    }

    private void initControl()
    {
        imgBack = (ImageView) findViewById(R.id.imgKaoShiMuLu_Back);
        imgBack.setOnClickListener(onClickListener);

        listData = (ListView) findViewById(R.id.listKaoShiMuLu_Data);

        rlDuanJi = (RelativeLayout) findViewById(R.id.rlKaoShiMuLu_DuanJiKaoShi);
        rlDuanJi.setOnClickListener(onClickListener);
        rlCheDui = (RelativeLayout) findViewById(R.id.rlKaoShiMuLu_CheDuiKaoShi);
        rlCheDui.setOnClickListener(onClickListener);
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

                CommMgr.commService.parseGetKaoShiLiShiList(jsonData, arrData);
                if (arrData == null)
                    result = -1;
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

                if (result == 1)
                {
                    adapter = new ItemAdapter(KaoShiMuLuActivity.this, 0, arrData);
                    listData.setAdapter(adapter);
                }

                if (result == -1)
                    GlobalData.showToast(KaoShiMuLuActivity.this, getString(R.string.network_error));
            }
        };
    }

    public class ItemAdapter extends ArrayAdapter<STKaoShiLiShi>
    {
        Context ctx;
        ArrayList<STKaoShiLiShi> list = new ArrayList<STKaoShiLiShi>();

        public ItemAdapter(Context ctx, int resourceId, ArrayList<STKaoShiLiShi> list) {
            super(ctx, resourceId, list);
            this.ctx = ctx;
            this.list = list;
        }

        @Override
        public int getCount() {
            return list.size();
        }

        @Override
        public long getItemId(int position) {
            return position;
        }

        @Override
        public View getView(int position, View convertView, ViewGroup parent) {
            View v = convertView;
            if (v == null)
            {
                LayoutInflater inflater = (LayoutInflater)ctx.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
                v = inflater.inflate(R.layout.kaoshimulu_view, null);
                ResolutionSet._instance.iterateChild(v.findViewById(R.id.rlKaoShiMuLuViewBack));
            }

            TextView lblTitle = (TextView) v.findViewById(R.id.lblKaoShiMuLuView_WenTi);
            lblTitle.setText(getString(R.string.kaoti) + list.get(position).title);
            TextView lblDate = (TextView) v.findViewById(R.id.lblKaoShiMuLuView_ShiJian);
            lblDate.setText(getString(R.string.kaoshishijian) +  list.get(position).examdate);
            TextView lblAns = (TextView) v.findViewById(R.id.lblKaoShiMuLuView_ZhengQue);
            lblAns.setText(getString(R.string.zhengquetishu) + list.get(position).rightans + "(" + getString(R.string.gong) + list.get(position).total + getString(R.string.geti) + ")");
            TextView lblMark = (TextView) v.findViewById(R.id.lblKaoShiMuLuView_WodeChengJi);
            lblMark.setText(getString(R.string.wodechengji) + list.get(position).mark);

            return v;
        }
    }
}
