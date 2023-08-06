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
import com.damytech.STData.STKaoShiItem;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;
import org.json.JSONObject;

import java.util.ArrayList;

public class KaoShiCheDuiActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    private ImageView imgBack = null;
    private RelativeLayout rlDuanJi = null;
    private RelativeLayout rlLiShi = null;

    private ListView listData = null;
    private ItemAdapter adapter = null;

    private ArrayList<STKaoShiItem> arrData = new ArrayList<STKaoShiItem>();
    private JsonHttpResponseHandler handler = null;
    private ProgressDialog dialog = null;

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgCheDuiKaoShi_Back:
                    finish();
                    break;
                case R.id.rlCheDuiKaoShi_DuanJiKaoShi:
                    Intent intentDuanJi = new Intent(KaoShiCheDuiActivity.this, KaoShiDuanJiActivity.class);
                    startActivity(intentDuanJi);
                    finish();
                    break;
                case R.id.rlCheDuiKaoShi_KaoShiFenXi:
                    Intent intentMuLu = new Intent(KaoShiCheDuiActivity.this, KaoShiMuLuActivity.class);
                    startActivity(intentMuLu);
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
        setContentView(R.layout.cheduikaoshi_activity);

        mainLayout = (RelativeLayout)findViewById(R.id.rlCheDuiKaoShiBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlCheDuiKaoShiBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        initControl();
        initHandler();

        dialog = ProgressDialog.show(
                KaoShiCheDuiActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetKaoShiWenTiList(handler, Long.toString(GlobalData.GetUid(KaoShiCheDuiActivity.this)), "1");
    }

    private void initControl()
    {
        imgBack = (ImageView) findViewById(R.id.imgCheDuiKaoShi_Back);
        imgBack.setOnClickListener(onClickListener);

        listData = (ListView) findViewById(R.id.listCheDuiKaoShi_Data);

        rlDuanJi = (RelativeLayout) findViewById(R.id.rlCheDuiKaoShi_DuanJiKaoShi);
        rlDuanJi.setOnClickListener(onClickListener);
        rlLiShi = (RelativeLayout) findViewById(R.id.rlCheDuiKaoShi_KaoShiFenXi);
        rlLiShi.setOnClickListener(onClickListener);
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

                CommMgr.commService.parseGetKaoShiWenTiList(jsonData, arrData);
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
                    adapter = new ItemAdapter(KaoShiCheDuiActivity.this, 0, arrData);
                    listData.setAdapter(adapter);
                }

                if (result == -1)
                    GlobalData.showToast(KaoShiCheDuiActivity.this, getString(R.string.network_error));
            }
        };
    }

    public class ItemAdapter extends ArrayAdapter<STKaoShiItem>
    {
        Context ctx;
        ArrayList<STKaoShiItem> list = new ArrayList<STKaoShiItem>();

        public ItemAdapter(Context ctx, int resourceId, ArrayList<STKaoShiItem> list) {
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
                v = inflater.inflate(R.layout.kaoshiitem_view, null);
                ResolutionSet._instance.iterateChild(v.findViewById(R.id.rlKaoShiItemViewBack));
            }

            TextView lblTitle = (TextView) v.findViewById(R.id.lblKaoShiItemView_WenTi);
            lblTitle.setText(list.get(position).title);
            TextView lblProblems = (TextView) v.findViewById(R.id.lblKaoShiItemView_WenTiShu);
            lblProblems.setText(list.get(position).problems + getString(R.string.geti));
            RelativeLayout rlBack = (RelativeLayout) v.findViewById(R.id.rlKaoShiItemViewBack);
            rlBack.setTag(list.get(position));
            rlBack.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    STKaoShiItem obj = (STKaoShiItem) v.getTag();
                    Intent intent = new Intent(KaoShiCheDuiActivity.this, KaoShiXiangXiActivity.class);
                    intent.putExtra("Data", obj);
                    startActivity(intent);
                }
            });

            return v;
        }
    }
}
