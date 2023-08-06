package com.damytech.TieLu;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Intent;
import android.graphics.Color;
import android.graphics.Rect;
import android.os.Bundle;
import android.text.TextUtils;
import android.util.TypedValue;
import android.view.Gravity;
import android.view.View;
import android.view.ViewGroup;
import android.view.ViewTreeObserver;
import android.widget.*;
import com.damytech.HttpConn.JsonHttpResponseHandler;
import com.damytech.STData.STXiangDian;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;
import org.json.JSONObject;

import java.util.ArrayList;

public class KaoHeLuRu_XiangDianActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    Bundle bundle;
    Intent intent;

    private ImageView imgBack = null;
    private ImageView imgFind = null;
    private EditText txtSearchText = null;
    private ListView listData = null;
    private ItemAdapter adapter = null;

    private String strQuery = "";
    private ArrayList<STXiangDian> arrData = new ArrayList<STXiangDian>();
    private JsonHttpResponseHandler handler = null;
    private ProgressDialog dialog = null;

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {
            switch (v.getId())
            {
                case R.id.imgKaoHeLuRu_XiangDian_Back:
                    finish();
                    break;
                case R.id.imgKaoHeLuRu_XiangDian_Search:
                    strQuery = txtSearchText.getText().toString();
                    dialog = ProgressDialog.show(
                            KaoHeLuRu_XiangDianActivity.this,
                            "",
                            getString(R.string.waiting),
                            true,
                            false,
                            null
                    );
                    CommMgr.commService.GetXiangDianList(handler, strQuery);
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
        setContentView(R.layout.kaoheluru_xiangdian_activity);

        mainLayout = (RelativeLayout)findViewById(R.id.rlKaoHeLuRu_XiangDianBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlKaoHeLuRu_XiangDianBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        bundle = new Bundle();
        intent = new Intent();

        initControl();
        initHandler();
    }

    private void initControl()
    {
        txtSearchText = (EditText) findViewById(R.id.txtKaoHeLuRu_XiangDian_SearchText);
        listData = (ListView) findViewById(R.id.listKaoHeLuRu_XiangDian_Data);

        imgBack = (ImageView) findViewById(R.id.imgKaoHeLuRu_XiangDian_Back);
        imgBack.setOnClickListener(onClickListener);

        imgFind = (ImageView) findViewById(R.id.imgKaoHeLuRu_XiangDian_Search);
        imgFind.setOnClickListener(onClickListener);
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

                CommMgr.commService.parseGetXiangDianList(jsonData, arrData);
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
                    adapter = new ItemAdapter();
                    listData.setAdapter(adapter);
                }

                if (result == -1)
                    GlobalData.showToast(KaoHeLuRu_XiangDianActivity.this, getString(R.string.network_error));
            }
        };
    }

    public class ItemAdapter extends BaseAdapter
    {
        @Override
        public int getCount() {
            if (arrData != null)
                return arrData.size();
            return 0;
        }

        @Override
        public Object getItem(int position) {
           return  arrData.get(position);
        }

        @Override
        public long getItemId(int position) {
            return position;
        }

        @Override
        public boolean hasStableIds() {
            return true;
        }

        @Override
        public boolean isEmpty() {
            return getCount() == 0;
        }

        @Override
        public View getView(int position, View convertView, ViewGroup parent) {
            if (convertView == null)
            {
                convertView = new RelativeLayout(parent.getContext());
                AbsListView.LayoutParams layoutParams = new AbsListView.LayoutParams(AbsListView.LayoutParams.FILL_PARENT, 90);
                convertView.setLayoutParams(layoutParams);
                convertView.setBackgroundColor(Color.GRAY);

                TextView txtItem = new TextView(convertView.getContext());
                txtItem.setTextSize(TypedValue.COMPLEX_UNIT_PX, 30);
                txtItem.setTextColor(getResources().getColor(R.color.gray));
                txtItem.setSingleLine(true);
                txtItem.setEllipsize(TextUtils.TruncateAt.END);
                txtItem.setBackgroundResource(R.drawable.rectgraywhite_layout);
                txtItem.setPadding(40, 0, 40, 0);
                txtItem.setGravity(Gravity.CENTER_VERTICAL);
                AbsListView.LayoutParams txtLayoutParams = new AbsListView.LayoutParams(AbsListView.LayoutParams.FILL_PARENT, AbsListView.LayoutParams.FILL_PARENT);
                txtItem.setLayoutParams(txtLayoutParams);
                txtItem.setText("项点号 ：" + arrData.get(position).checkno + " 减分 ：" + arrData.get(position).chkpoint + " 问题 ：" + arrData.get(position).info);
                txtItem.setTag(position);
                txtItem.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        Integer nID = (Integer)v.getTag();
                        if (nID != null)
                        {
                            bundle.putString("XIANGDIANKEY", arrData.get(nID.intValue()).checkno);
                            bundle.putInt("XIANGDIANCHKPOINT", arrData.get(nID.intValue()).chkpoint);
                            bundle.putString("XIANGDIANRELPOINT", arrData.get(nID.intValue()).relpoint);
                            bundle.putString("XIANGDIANINFO", arrData.get(nID.intValue()).info);
                            bundle.putLong("XIANGDIANID", arrData.get(nID.intValue()).uid);
                            intent.putExtras(bundle);
                            KaoHeLuRu_XiangDianActivity.this.setResult(RESULT_OK, intent);
                            KaoHeLuRu_XiangDianActivity.this.finish();
                        }
                    }
                });

                ((RelativeLayout)convertView).addView(txtItem);
                ResolutionSet._instance.iterateChild(convertView);

                STViewHolder viewHolder = new STViewHolder();
                viewHolder.txtItem = txtItem;

                convertView.setTag(viewHolder);
            }
            else
            {
                STViewHolder viewHolder = (STViewHolder)convertView.getTag();
                viewHolder.txtItem.setText(arrData.get(position).info);
                viewHolder.txtItem.setTag(position);
                viewHolder.txtItem.setOnClickListener(new View.OnClickListener() {
                    @Override
                    public void onClick(View v) {
                        Integer nUid = (Integer)v.getTag();
                        if (nUid != null)
                        {
                            bundle.putString("XIANGDIANKEY", arrData.get(nUid.intValue()).checkno);
                            bundle.putInt("XIANGDIANCHKPOINT", arrData.get(nUid.intValue()).chkpoint);
                            bundle.putString("XIANGDIANRELPOINT", arrData.get(nUid.intValue()).relpoint);
                            bundle.putString("XIANGDIANINFO", arrData.get(nUid.intValue()).info);
                            bundle.putLong("XIANGDIANID", arrData.get(nUid.intValue()).uid);
                            intent.putExtras(bundle);
                            KaoHeLuRu_XiangDianActivity.this.setResult(RESULT_OK, intent);
                            KaoHeLuRu_XiangDianActivity.this.finish();
                        }
                    }
                });
            }

            return convertView;
        }
    }

    public class STViewHolder
    {
        public TextView txtItem = null;
    }
}
