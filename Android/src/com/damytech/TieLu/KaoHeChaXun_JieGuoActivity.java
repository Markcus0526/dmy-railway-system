package com.damytech.TieLu;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.Intent;
import android.graphics.Color;
import android.graphics.Rect;
import android.os.Bundle;
import android.text.TextUtils;
import android.util.TypedValue;
import android.view.*;
import android.widget.*;
import com.damytech.HttpConn.JsonHttpResponseHandler;
import com.damytech.STData.STKaoHeJiLu;
import com.damytech.STData.STXiangDian;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;
import org.json.JSONObject;

import java.util.ArrayList;

public class KaoHeChaXun_JieGuoActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    private ImageView imgBack = null;
    private ListView listData = null;
    private ItemAdapter adapter = null;

    private ArrayList<STKaoHeJiLu> arrData = new ArrayList<STKaoHeJiLu>();
    private JsonHttpResponseHandler handler = null;
    private ProgressDialog dialog = null;

    private long nRenYuanID = 0, nCheDuiID = 0, nBanZuID = 0;
    private String strStartDate = "", strEndDate = "";

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgKaoHeChaXun_JieGuo_Back:
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
        setContentView(R.layout.kaohechaxun_jieguo_activity);

        nCheDuiID = getIntent().getLongExtra("CheDuiVal", 0);
        nBanZuID = getIntent().getLongExtra("BanZuVal", 0);
        nRenYuanID = getIntent().getLongExtra("RenYuanVal", 0);
        strStartDate = getIntent().getStringExtra("StartDate");
        strEndDate = getIntent().getStringExtra("EndDate");

        mainLayout = (RelativeLayout)findViewById(R.id.rlKaoHeChaXun_JieGuoBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlKaoHeChaXun_JieGuoBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        initControl();
        initHandler();

        dialog = ProgressDialog.show(
                KaoHeChaXun_JieGuoActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetKaoHeJiLuList(handler, Long.toString(nCheDuiID), Long.toString(nBanZuID),Long.toString(nRenYuanID), strStartDate, strEndDate);
    }

    private void initControl()
    {
        imgBack = (ImageView) findViewById(R.id.imgKaoHeChaXun_JieGuo_Back);
        imgBack.setOnClickListener(onClickListener);

        listData = (ListView) findViewById(R.id.listKaoHeChaXun_JieGuo_Data);
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

                CommMgr.commService.parseGetKaoHeJiLuList(jsonData, arrData);
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
                    adapter = new ItemAdapter(KaoHeChaXun_JieGuoActivity.this, 0, arrData);
                    listData.setAdapter(adapter);
                }

                if (result == -1)
                    GlobalData.showToast(KaoHeChaXun_JieGuoActivity.this, getString(R.string.network_error));
            }
        };
    }

    public class ItemAdapter extends ArrayAdapter<STKaoHeJiLu>
    {
        Context ctx;
        ArrayList<STKaoHeJiLu> list = new ArrayList<STKaoHeJiLu>();

        public ItemAdapter(Context ctx, int resourceId, ArrayList<STKaoHeJiLu> list) {
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
                v = inflater.inflate(R.layout.kaohechaxun_jieguo_view, null);
                ResolutionSet._instance.iterateChild(v.findViewById(R.id.rlKaoHeChaXun_JieGuoViewBack));
            }

            TextView lblTitle = (TextView) v.findViewById(R.id.lblKaoHeChaXun_JieGuoView_Title);
            lblTitle.setText(list.get(position).title);
            TextView lblName = (TextView) v.findViewById(R.id.lblKaoHeChaXun_JieGuoView_Name);
            lblName.setText(list.get(position).name);
            TextView lblDate = (TextView) v.findViewById(R.id.lblKaoHeChaXun_JieGuoView_Date);
            lblDate.setText(list.get(position).chkdate);
            ImageView imgDetail = (ImageView) v.findViewById(R.id.imgKaoHeChaXun_JieGuoView_Detail);
            imgDetail.setTag(list.get(position).uid);
            imgDetail.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    Long nVal = (Long) v.getTag();
                    Intent intent = new Intent(KaoHeChaXun_JieGuoActivity.this, KaoHeChaXun_XiangXiActivity.class);
                    intent.putExtra("ID", nVal.longValue());
                    startActivity(intent);
                }
            });

            return v;
        }
    }
}
