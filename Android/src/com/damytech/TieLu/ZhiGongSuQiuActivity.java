package com.damytech.TieLu;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Intent;
import android.graphics.Color;
import android.graphics.Rect;
import android.os.Bundle;
import android.util.TypedValue;
import android.view.Gravity;
import android.view.View;
import android.view.ViewGroup;
import android.view.ViewTreeObserver;
import android.widget.*;
import com.damytech.HttpConn.JsonHttpResponseHandler;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;
import org.json.JSONObject;

import java.util.ArrayList;

public class ZhiGongSuQiuActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    private ImageView imgBack = null;
    private ImageView imgJiBie = null;
    private TextView lblJiBie = null;
    private Button btnFaSong = null;

    private EditText txtTitle = null;
    private EditText txtContent = null;

    private TextView textWoDeSuQiu = null;

    private JsonHttpResponseHandler handler = null;
    private ProgressDialog dialog = null;

    long curJiBieID = 0;

    RelativeLayout listLayout = null;
    ListView adapterListView = null;
    ItemAdapter adapter = null;

    ArrayList<STJiBie> arrJiBie = new ArrayList<STJiBie>();

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgZhiGongSuQiu_Back:
                    finish();
                    break;
                case R.id.textWoDeSuQiu:
                    Intent intent = new Intent(ZhiGongSuQiuActivity.this, WoDeSuQiuActivity.class);
                    startActivity(intent);
                    break;
                case R.id.imgZhiGongSuQiu_BuMenYouXiang:
                    adapter.notifyDataSetChanged();
                    listLayout.setVisibility(View.VISIBLE);
                    adapterListView.setSelectionAfterHeaderView();
                    break;
                case R.id.btnZhiGongSuQiu_FaSong:
                    String strTitle = txtTitle.getText().toString();
                    if (strTitle.length() == 0)
                    {
                        GlobalData.showToast(ZhiGongSuQiuActivity.this, getString(R.string.shuru_biaoti));
                        return;
                    }
                    String strContent = txtContent.getText().toString();
                    if (strContent.length() == 0)
                    {
                        GlobalData.showToast(ZhiGongSuQiuActivity.this, getString(R.string.shuru_fabuneirong));
                        return;
                    }
                    dialog = ProgressDialog.show(
                            ZhiGongSuQiuActivity.this,
                            "",
                            getString(R.string.waiting),
                            true,
                            false,
                            null
                    );
                    CommMgr.commService.SetOpinion(handler,
                            Long.toString(GlobalData.GetUid(ZhiGongSuQiuActivity.this)),
                            Long.toString(curJiBieID),
                            strTitle,
                            strContent
                            );
                    break;
            }
        }
    };

    public class STJiBie
    {
        long uid = 0;
        String name = "";
    }

    /**
     * Called when the activity is first created.
     */
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.zhigongsuqiu_activity);

        mainLayout = (RelativeLayout)findViewById(R.id.rlZhiGongSuQiuBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlZhiGongSuQiuBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        initControl();
        initHandler();

        ShowJiBie(0);
    }

    private void initControl()
    {
        STJiBie newItem = new STJiBie();
        newItem.uid = 0;
        newItem.name = getString(R.string.STR_DUANJI);
        arrJiBie.add(newItem);
        newItem = new STJiBie();
        newItem.uid = 1;
        newItem.name = getString(R.string.STR_CHEDUI);
        arrJiBie.add(newItem);

        imgBack = (ImageView) findViewById(R.id.imgZhiGongSuQiu_Back);
        imgBack.setOnClickListener(onClickListener);

        txtTitle = (EditText) findViewById(R.id.txtTitle);
        txtContent = (EditText) findViewById(R.id.txtZhiGongSuQiu_Data);

        lblJiBie = (TextView) findViewById(R.id.lblZhiGongSuQiu_BuMenYouXiangVal);

        textWoDeSuQiu = (TextView) findViewById(R.id.textWoDeSuQiu);
        textWoDeSuQiu.setOnClickListener(onClickListener);

        imgJiBie = (ImageView) findViewById(R.id.imgZhiGongSuQiu_BuMenYouXiang);
        imgJiBie.setOnClickListener(onClickListener);

        btnFaSong = (Button) findViewById(R.id.btnZhiGongSuQiu_FaSong);
        btnFaSong.setOnClickListener(onClickListener);

        listLayout = (RelativeLayout)findViewById(R.id.rlZhiGongSuQiu_List);
        listLayout.setClickable(true);
        listLayout.setOnClickListener(new View.OnClickListener()
        {
            @Override
            public void onClick(View v)
            {
                listLayout.setVisibility(View.GONE);
            }
        });

        adapterListView = (ListView)findViewById(R.id.listZhiGongSuQiu_List);
        adapter = new ItemAdapter();
        adapterListView.setAdapter(adapter);
    }

    private void initHandler()
    {
        handler = new JsonHttpResponseHandler()
        {
            int result = 0;
            long nRet = 0;

            @Override
            public void onSuccess(JSONObject jsonData)
            {
                dialog.dismiss();

                nRet = CommMgr.commService.parseSetOpinion(jsonData);
                result = 1;
            }

            @Override
            public void onFailure(Throwable ex, String exception)
            {
                result = 0;
                dialog.dismiss();
            }

            @Override
            public void onFinish()
            {
                if (result == 1)
                {
                    if (nRet == 0)
                    {
                        GlobalData.showToast(ZhiGongSuQiuActivity.this, getString(R.string.caozuochenggong));
                        finish();
                    }
                    else if (nRet == 100)
                    {
                        GlobalData.showToast(ZhiGongSuQiuActivity.this, "科室干部无法提交车队诉求");
                    }
                }

                if (result == -1)
                {
                    GlobalData.showToast(ZhiGongSuQiuActivity.this, getString(R.string.network_error));
                    finish();
                }
            }
        };

        return;
    }

    private void ShowJiBie(int nNo)
    {
        curJiBieID = nNo;
        lblJiBie.setText(arrJiBie.get(nNo).name);
        listLayout.setVisibility(View.GONE);

        return;
    }

    public class ItemAdapter extends BaseAdapter
    {
        @Override
        public int getCount() {
            if (arrJiBie == null)
                return 0;

            return arrJiBie.size();
        }

        @Override
        public Object getItem(int position) {
            return arrJiBie.get(position);
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
            STJiBie jibieInfo = arrJiBie.get(position);

            if (convertView == null)
            {
                convertView = new RelativeLayout(parent.getContext());
                AbsListView.LayoutParams layoutParams = new AbsListView.LayoutParams(AbsListView.LayoutParams.FILL_PARENT, 90);
                convertView.setLayoutParams(layoutParams);
                convertView.setBackgroundColor(Color.GRAY);

                TextView txtItem = new TextView(convertView.getContext());
                txtItem.setTextSize(TypedValue.COMPLEX_UNIT_PX, 30);
                txtItem.setTextColor(Color.WHITE);
                txtItem.setBackgroundResource(R.drawable.rectgray_layout);
                txtItem.setPadding(40, 0, 0, 0);
                txtItem.setGravity(Gravity.CENTER_VERTICAL);
                AbsListView.LayoutParams txtLayoutParams = new AbsListView.LayoutParams(AbsListView.LayoutParams.FILL_PARENT, AbsListView.LayoutParams.FILL_PARENT);
                txtItem.setLayoutParams(txtLayoutParams);
                if (jibieInfo != null)
                    txtItem.setText(jibieInfo.name);

                if (jibieInfo != null)
                {
                    txtItem.setTag(position);
                    txtItem.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            Integer nID = (Integer)v.getTag();
                            if (nID != null)
                            {
                                ShowJiBie(nID.intValue());
                            }
                        }
                    });
                }

                ((RelativeLayout)convertView).addView(txtItem);
                ResolutionSet._instance.iterateChild(convertView);

                STViewHolder viewHolder = new STViewHolder();
                viewHolder.txtItem = txtItem;

                convertView.setTag(viewHolder);
            }
            else
            {
                STViewHolder viewHolder = (STViewHolder)convertView.getTag();
                if (jibieInfo != null)
                {
                    viewHolder.txtItem.setText(jibieInfo.name);
                    viewHolder.txtItem.setTag(position);
                    viewHolder.txtItem.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            Integer nID = (Integer)v.getTag();
                            if (nID != null)
                            {
                                ShowJiBie(nID.intValue());
                            }
                        }
                    });
                }
            }

            return convertView;
        }
    }

    public class STViewHolder
    {
        public TextView txtItem = null;
    }
}
