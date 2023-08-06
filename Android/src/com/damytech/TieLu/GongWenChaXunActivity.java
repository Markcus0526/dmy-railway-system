package com.damytech.TieLu;

import android.app.Activity;
import android.app.DatePickerDialog;
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
import com.damytech.STData.*;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.Calendar;

public class GongWenChaXunActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    private int nDateMode = 0; // 0 : QiShiRiQi       1 : JieShuRiQi
    private int nQiShiYear = 0, nQiShiMonth = 0, nQiShiDay = 0;
    private int nJieShuYear = 0, nJieShuMonth = 0, nJieShuDay = 0;
    private Calendar curdate = Calendar.getInstance();

    private ImageView imgBack = null;
    private RelativeLayout rlDaiQian = null;
    private RelativeLayout rlFaBu = null;
    private RelativeLayout rlYiShou = null;
    private TextView lblQiShiRiQi = null;
    private TextView lblJieShuRiQi = null;
    private TextView lblBuMen = null;
    private ImageView imgFaBuBuMen = null;
    private Button btnChaKan = null;
    private EditText txtQuery = null;

    ArrayList<STGWFaBu> arrFaBu = new ArrayList<STGWFaBu>();
    private JsonHttpResponseHandler handler = null;
    private ProgressDialog dialog = null;

    RelativeLayout listLayout = null;
    ListView adapterListView = null;
    ItemAdapter adapter = null;

    DatePickerDialog.OnDateSetListener dateListener =
            new DatePickerDialog.OnDateSetListener() {
                @Override
                public void onDateSet(DatePicker datePicker, int year, int month, int dayOfMonth) {
                    if (nDateMode == 0)
                    {
                        nQiShiYear = year;
                        nQiShiMonth = month;
                        nQiShiDay = dayOfMonth;
                        lblQiShiRiQi.setText(GlobalData.getDateFormat(nQiShiYear, nQiShiMonth, nQiShiDay));
                    }
                    else
                    {
                        nJieShuYear = year;
                        nJieShuMonth = month;
                        nJieShuDay = dayOfMonth;
                        lblJieShuRiQi.setText(GlobalData.getDateFormat(nJieShuYear, nJieShuMonth, nJieShuDay));
                    }
                }
            };

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgGongWenChaXun_Back:
                    finish();
                    break;
                case R.id.rlGongWenChaXun_DaiQian:
                    Intent intentDaiQian = new Intent(GongWenChaXunActivity.this, GongWenDaiQianActivity.class);
                    startActivity(intentDaiQian);
                    finish();
                    break;
                case R.id.rlGongWenChaXun_FaBu:
                    Intent intentFaBu = new Intent(GongWenChaXunActivity.this, GongWenFaBuActivity.class);
                    startActivity(intentFaBu);
                    finish();
                    break;
                case R.id.rlGongWenChaXun_YiShou:
                    Intent intentYiShou = new Intent(GongWenChaXunActivity.this, GongWenYiShouActivity.class);
                    startActivity(intentYiShou);
                    finish();
                    break;
                case R.id.lblGongWenChaXun_QiShiRiQiVal:
                    nDateMode = 0;
                    new DatePickerDialog(GongWenChaXunActivity.this, dateListener, nQiShiYear, nQiShiMonth, nQiShiDay).show();
                    break;
                case R.id.lblGongWenChaXun_JieShuRiQiVal:
                    nDateMode = 1;
                    new DatePickerDialog(GongWenChaXunActivity.this, dateListener, nJieShuYear, nJieShuMonth, nJieShuDay).show();
                    break;
                case R.id.imgGongWenChaXun_FaBuBuMen:
                    adapter.notifyDataSetChanged();
                    listLayout.setVisibility(View.VISIBLE);
                    adapterListView.setSelectionAfterHeaderView();
                    break;
                case R.id.btnGongWenChaXun_ChaXun:
                    Intent intent = new Intent(GongWenChaXunActivity.this, GongWenChaXun_JieGuoActivity.class);
                    intent.putExtra("Query", txtQuery.getText().toString());
                    intent.putExtra("StartDate", GlobalData.getDateFormat(nQiShiYear, nQiShiMonth, nQiShiDay));
                    intent.putExtra("EndDate", GlobalData.getDateFormat(nJieShuYear, nJieShuMonth, nJieShuDay));
                    intent.putExtra("BuMen", lblBuMen.getText().toString());
                    startActivity(intent);
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
        setContentView(R.layout.gongwenchaxun_activity);

        mainLayout = (RelativeLayout)findViewById(R.id.rlGongWenChaXunBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlGongWenChaXunBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        initControl();
        initHandler();

        dialog = ProgressDialog.show(
                GongWenChaXunActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetGWFaBuList(handler);
    }

    private void initControl()
    {
        nQiShiYear = curdate.get(Calendar.YEAR);
        nQiShiMonth = curdate.get(Calendar.MONTH);
        nQiShiDay = curdate.get(Calendar.DAY_OF_MONTH);
        nJieShuYear = curdate.get(Calendar.YEAR);
        nJieShuMonth = curdate.get(Calendar.MONTH);
        nJieShuDay = curdate.get(Calendar.DAY_OF_MONTH);

        imgBack = (ImageView) findViewById(R.id.imgGongWenChaXun_Back);
        imgBack.setOnClickListener(onClickListener);

        rlDaiQian = (RelativeLayout) findViewById(R.id.rlGongWenChaXun_DaiQian);
        rlDaiQian.setOnClickListener(onClickListener);

        rlFaBu = (RelativeLayout) findViewById(R.id.rlGongWenChaXun_FaBu);
        rlFaBu.setOnClickListener(onClickListener);

        rlYiShou = (RelativeLayout) findViewById(R.id.rlGongWenChaXun_YiShou);
        rlYiShou.setOnClickListener(onClickListener);

        lblQiShiRiQi = (TextView) findViewById(R.id.lblGongWenChaXun_QiShiRiQiVal);
        lblQiShiRiQi.setText(GlobalData.getDateFormat(nQiShiYear, nQiShiMonth, nQiShiDay));
        lblQiShiRiQi.setOnClickListener(onClickListener);

        lblJieShuRiQi = (TextView) findViewById(R.id.lblGongWenChaXun_JieShuRiQiVal);
        lblJieShuRiQi.setText(GlobalData.getDateFormat(nJieShuYear, nJieShuMonth, nJieShuDay));
        lblJieShuRiQi.setOnClickListener(onClickListener);

        lblBuMen = (TextView) findViewById(R.id.lblGongWenChaXun_FaBuBuMenVal);
        imgFaBuBuMen = (ImageView) findViewById(R.id.imgGongWenChaXun_FaBuBuMen);
        imgFaBuBuMen.setOnClickListener(onClickListener);

        txtQuery = (EditText) findViewById(R.id.txtGongWenChaXun_ChaXunWenZi);
        btnChaKan = (Button) findViewById(R.id.btnGongWenChaXun_ChaXun);
        btnChaKan.setOnClickListener(onClickListener);

        listLayout = (RelativeLayout)findViewById(R.id.rlGongWenChaXun_List);
        listLayout.setClickable(true);
        listLayout.setOnClickListener(new View.OnClickListener()
        {
            @Override
            public void onClick(View v)
            {
                listLayout.setVisibility(View.GONE);
            }
        });

        adapterListView = (ListView)findViewById(R.id.listGongWenChaXun_List);
        adapter = new ItemAdapter();
        adapterListView.setAdapter(adapter);
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

                CommMgr.commService.parseGetGWFaBuList(jsonData, arrFaBu);
                if (arrFaBu == null)
                    result = 0;
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
                if (result == 0)
                {
                    GlobalData.showToast(GongWenChaXunActivity.this, getString(R.string.network_error));
                    GongWenChaXunActivity.this.finish();
                }
                else
                {
                    if (arrFaBu != null && arrFaBu.size() > 0)
                    {
                        ShowBuMen(0);
                    }
                }
            }
        };
    }

    private void ShowBuMen(int nNo)
    {
        lblBuMen.setText(arrFaBu.get(nNo).name);
        listLayout.setVisibility(View.GONE);

        return;
    }

    public class ItemAdapter extends BaseAdapter
    {
        @Override
        public int getCount() {
            if (arrFaBu == null)
                return 0;
            return arrFaBu.size();
        }

        @Override
        public Object getItem(int position) {
            return arrFaBu.get(position);
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
            STGWFaBu fabuInfo = null;
            fabuInfo = arrFaBu.get(position);

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
                if (fabuInfo != null)
                    txtItem.setText(fabuInfo.name);
                if (fabuInfo != null)
                {
                    txtItem.setTag(position);
                    txtItem.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            Integer nID = (Integer)v.getTag();
                            if (nID != null)
                            {
                                ShowBuMen(nID.intValue());
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
                if (fabuInfo != null)
                {
                    viewHolder.txtItem.setText(fabuInfo.name);
                    viewHolder.txtItem.setTag(position);
                    viewHolder.txtItem.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            Integer nUid = (Integer)v.getTag();
                            if (nUid != null)
                                ShowBuMen(nUid.intValue());
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
