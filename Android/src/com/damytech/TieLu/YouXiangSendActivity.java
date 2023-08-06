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
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;
import org.json.JSONObject;

import java.util.ArrayList;

public class YouXiangSendActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    private ImageView imgBack = null;
    private ImageView imgNote = null;
    private ImageView imgReceive = null;
    private TextView lblTitle = null;
    private ListView listDatas = null;
    private ItemAdapter adapter = null;
    private ArrayList<STEmailInfo> arrData = new ArrayList<STEmailInfo>();
    private JsonHttpResponseHandler handler = null;
    private ProgressDialog dialog = null;

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgYouXiang_Back:
                    finish();
                    break;
                case R.id.imgNote:
                    Intent intent = new Intent(YouXiangSendActivity.this, YouJianXieXinActivity.class);
                    startActivity(intent);
                    break;
                case R.id.imgReceive:
                    Intent intentReceive = new Intent(YouXiangSendActivity.this, YouXiangActivity.class);
                    startActivity(intentReceive);
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
        setContentView(R.layout.youxiangsend_activity);

        mainLayout = (RelativeLayout)findViewById(R.id.rlYouXiangBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlYouXiangBack));
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
        imgBack = (ImageView) findViewById(R.id.imgYouXiang_Back);
        imgBack.setOnClickListener(onClickListener);

        lblTitle = (TextView) findViewById(R.id.lblYouXiang_Title);

        imgNote = (ImageView)findViewById(R.id.imgNote);
        imgNote.setOnClickListener(onClickListener);

        imgReceive = (ImageView) findViewById(R.id.imgReceive);
        imgReceive.setOnClickListener(onClickListener);

        listDatas = (ListView) findViewById(R.id.listDatas);
    }

    @Override
    public void onResume()
    {
        super.onResume();

        arrData.clear();
        dialog = ProgressDialog.show(
                YouXiangSendActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetSentEmailList(handler, Long.toString(GlobalData.GetUid(YouXiangSendActivity.this)));
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

                CommMgr.commService.parseGetSentEmailList(jsonData, arrData);
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
                    lblTitle.setText(getString(R.string.fasongxiang) + "(" + arrData.size() + ")");
                    adapter = new ItemAdapter(YouXiangSendActivity.this, 0, arrData);
                    listDatas.setAdapter(adapter);
                }

                if (result == -1)
                {
                    GlobalData.showToast(YouXiangSendActivity.this, getString(R.string.network_error));
                    finish();
                }
            }
        };
    }

    public class ItemAdapter extends ArrayAdapter<STEmailInfo>
    {
        Context ctx;
        ArrayList<STEmailInfo> list = new ArrayList<STEmailInfo>();

        public ItemAdapter(Context ctx, int resourceId, ArrayList<STEmailInfo> list) {
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
                v = inflater.inflate(R.layout.youjianmulu_view, null);
                ResolutionSet._instance.iterateChild(v.findViewById(R.id.rlViewBack));
            }

            TextView lblTitle = (TextView) v.findViewById(R.id.lblTitle);
            lblTitle.setText(list.get(position).title);
            TextView lblDate = (TextView) v.findViewById(R.id.lblDate);
            lblDate.setText(list.get(position).postdate);
            TextView lblData = (TextView) v.findViewById(R.id.lblData);
            lblData.setText(GlobalData.unescape(list.get(position).content));
            TextView lblSender = (TextView) v.findViewById(R.id.lblSender);
            lblSender.setText(list.get(position).sender);
            ImageView imgDot = (ImageView) v.findViewById(R.id.imgBlueDot);
            if (list.get(position).isread == 0)
                imgDot.setVisibility(View.VISIBLE);
            else
                imgDot.setVisibility(View.INVISIBLE);
            imgDot.setVisibility(View.INVISIBLE);
            ImageView imgCheck = (ImageView) v.findViewById(R.id.imgCheck);
            if (list.get(position).isselected == 0)
                imgCheck.setImageResource(R.drawable.uncheck);
            else
                imgCheck.setImageResource(R.drawable.check);
            imgCheck.setVisibility(View.INVISIBLE);
            imgCheck.setTag(position);
            imgCheck.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    Integer nVal = (Integer)v.getTag();
                    if (list.get(nVal.intValue()).isselected == 0)
                    {
                        list.get(nVal.intValue()).isselected = 1;
                        ((ImageView)v).setImageResource(R.drawable.check);
                    }
                    else
                    {
                        list.get(nVal.intValue()).isselected = 0;
                        ((ImageView)v).setImageResource(R.drawable.uncheck);
                    }
                }
            });
            RelativeLayout rlBack = (RelativeLayout)v.findViewById(R.id.rlViewBack);
            rlBack.setTag(list.get(position).uid);
            rlBack.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    Long val = (Long)v.getTag();
                    Intent intent = new Intent(YouXiangSendActivity.this, YouXiangXiangXiActivity.class);
                    intent.putExtra("emailid", val.longValue());
                    intent.putExtra("sent", 1);
                    startActivity(intent);
                }
            });

            return v;
        }
    }
}
