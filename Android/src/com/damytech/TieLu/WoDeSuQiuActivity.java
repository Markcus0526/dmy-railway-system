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

import java.util.ArrayList;

public class WoDeSuQiuActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    private ImageView imgBack;
    private ListView listDatas = null;
    private ItemAdapter adapter = null;
    private ArrayList<STOpinionData> arrData = new ArrayList<STOpinionData>();
    private JsonHttpResponseHandler handler = null;
    private ProgressDialog dialog = null;

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
        setContentView(R.layout.wodesuqiu_activity);

        mainLayout = (RelativeLayout)findViewById(R.id.rlWoDeSuQiuBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlWoDeSuQiuBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        initControl();
        initHandler();
    }

    private void initControl()
    {
        listDatas = (ListView) findViewById(R.id.listDatas);
        imgBack = (ImageView) findViewById(R.id.imgBack);
        imgBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                WoDeSuQiuActivity.this.finish();
            }
        });
    }

    @Override
    public void onResume()
    {
        super.onResume();

        arrData.clear();
        dialog = ProgressDialog.show(
                WoDeSuQiuActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetOpinionList(handler, GlobalData.GetUid(WoDeSuQiuActivity.this));
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

                CommMgr.commService.parseGetOpinionList(jsonData, arrData);
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
                    adapter = new ItemAdapter(WoDeSuQiuActivity.this, 0, arrData);
                    listDatas.setAdapter(adapter);
                }

                if (result == -1)
                {
                    GlobalData.showToast(WoDeSuQiuActivity.this, getString(R.string.network_error));
                    finish();
                }
            }
        };
    }

    public class ItemAdapter extends ArrayAdapter<STOpinionData>
    {
        Context ctx;
        ArrayList<STOpinionData> list = new ArrayList<STOpinionData>();

        public ItemAdapter(Context ctx, int resourceId, ArrayList<STOpinionData> list) {
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
                v = inflater.inflate(R.layout.wodesuqiuitem_view, null);
                ResolutionSet._instance.iterateChild(v.findViewById(R.id.rlBack));
            }

            TextView lblTitle = (TextView) v.findViewById(R.id.lblTitle);
            lblTitle.setText(list.get(position).title);
            TextView lblDate = (TextView) v.findViewById(R.id.lblDate);
            lblDate.setText(list.get(position).postdate);

            RelativeLayout rlBack = (RelativeLayout)v.findViewById(R.id.rlBack);
            rlBack.setTag(position);
            rlBack.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    Integer val = (Integer)v.getTag();
                    Intent intent = new Intent(WoDeSuQiuActivity.this, WoDeSuQiuXiangXiActivity.class);
                    intent.putExtra("title", list.get(val.intValue()).title);
                    intent.putExtra("postdate", list.get(val.intValue()).postdate);
                    intent.putExtra("content", list.get(val.intValue()).content);
                    intent.putExtra("feedback", list.get(val.intValue()).feedback);
                    startActivity(intent);
                }
            });

            return v;
        }
    }
}
