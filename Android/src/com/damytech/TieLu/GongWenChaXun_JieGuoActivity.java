package com.damytech.TieLu;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.Intent;
import android.graphics.Rect;
import android.os.Bundle;
import android.view.*;
import android.widget.*;
import com.damytech.HttpConn.JsonHttpResponseHandler;
import com.damytech.STData.STDaiQianItem;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;
import org.json.JSONObject;

import java.util.ArrayList;

public class GongWenChaXun_JieGuoActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    private ImageView imgBack = null;
    private ListView listData = null;
    private ItemAdapter adapter = null;

    private ArrayList<STDaiQianItem> arrData = new ArrayList<STDaiQianItem>();
    private JsonHttpResponseHandler handler = null;
    private ProgressDialog dialog = null;

    private String strStartDate = "", strEndDate = "";
    private String strQuery = "", strBuMen = "";

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgGongWenChaXun_JieGuo_Back:
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
        setContentView(R.layout.gongwenchaxun_jieguo_activity);

        strStartDate = getIntent().getStringExtra("StartDate");
        strEndDate = getIntent().getStringExtra("EndDate");
        strQuery = getIntent().getStringExtra("Query");
        strBuMen = getIntent().getStringExtra("BuMen");

        mainLayout = (RelativeLayout)findViewById(R.id.rlGongWenChaXun_JieGuoBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlGongWenChaXun_JieGuoBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        initControl();
        initHandler();

        dialog = ProgressDialog.show(
                GongWenChaXun_JieGuoActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetGongWenList(handler, strBuMen, strStartDate, strEndDate, strQuery, -1);
    }

    private void initControl()
    {
        imgBack = (ImageView) findViewById(R.id.imgGongWenChaXun_JieGuo_Back);
        imgBack.setOnClickListener(onClickListener);

        listData = (ListView) findViewById(R.id.listGongWenChaXun_JieGuo_Data);
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

                CommMgr.commService.parseGetGongWenList(jsonData, arrData);
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
                    adapter = new ItemAdapter(GongWenChaXun_JieGuoActivity.this, 0, arrData);
                    listData.setAdapter(adapter);
                }

                if (result == -1)
                    GlobalData.showToast(GongWenChaXun_JieGuoActivity.this, getString(R.string.network_error));
            }
        };
    }

    public class ItemAdapter extends ArrayAdapter<STDaiQianItem>
    {
        Context ctx;
        ArrayList<STDaiQianItem> list = new ArrayList<STDaiQianItem>();

        public ItemAdapter(Context ctx, int resourceId, ArrayList<STDaiQianItem> list) {
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
                v = inflater.inflate(R.layout.gongwenchaxunitem_view, null);
                ResolutionSet._instance.iterateChild(v.findViewById(R.id.rlGongWenChaXunItemBack));
            }

            TextView lblTitle = (TextView) v.findViewById(R.id.lblGongWenChaXunItem_Title);
            lblTitle.setText(list.get(position).title);
            TextView lblFaBuShiJian = (TextView) v.findViewById(R.id.lblGongWenChaXunItem_FaBuShiJianVal);
            lblFaBuShiJian.setText(list.get(position).fbdate);
            TextView lblTongZhiHao = (TextView) v.findViewById(R.id.lblGongWenChaXunItem_TongZhiHaoVal);
            lblTongZhiHao.setText(list.get(position).tongzhihao);
            TextView lblFaBuRen = (TextView) v.findViewById(R.id.lblGongWenChaXunItem_FaBuRenVal);
            lblFaBuRen.setText(list.get(position).faburen);
            Button btnDetail = (Button) v.findViewById(R.id.btnGongWenChaXunItem_ChaKan);
            btnDetail.setTag(list.get(position));
            btnDetail.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    STDaiQianItem stInfo = (STDaiQianItem) v.getTag();
                    Intent intent = new Intent(GongWenChaXun_JieGuoActivity.this, GongWenXiangQingActivity.class);
                    intent.putExtra("uid", stInfo.uid);
                    startActivity(intent);
                }
            });

            return v;
        }
    }
}
