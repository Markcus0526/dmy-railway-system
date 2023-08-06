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
import com.damytech.STData.STBanZu;
import com.damytech.STData.STCheDui;
import com.damytech.STData.STZeRenRen;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.Calendar;

public class KaoHeChaXunActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    private int nDateMode = 0; // 0 : QiShiRiQi       1 : JieShuRiQi
    private int nQiShiYear = 0, nQiShiMonth = 0, nQiShiDay = 0;
    private int nJieShuYear = 0, nJieShuMonth = 0, nJieShuDay = 0;
    private Calendar curdate = Calendar.getInstance();
    private final int CHEDUI_ADAPTER = 0;
    private final int BANZU_ADAPTER = 1;
    private final int RENYUAN_ADAPTER = 2;
    private int curAdapterId = CHEDUI_ADAPTER;

    private TextView lblQiShiRiQi = null;
    private TextView lblJieShuRiQi = null;
    private ImageView imgBack = null;
    private ImageView imgCheDui = null;
    private ImageView imgBanZu = null;
    private ImageView imgRenYuan = null;
    private TextView lblCheDui = null;
    private TextView lblBanZu = null;
    private TextView lblRenYuan = null;
    private Button btnChaXun = null;
    private RelativeLayout rlKaoHeLuRu = null;
    private RelativeLayout rlJieGuoFenXi = null;

    long curCheDuiID = 0;
    long curBanZuID = 0;
    long curRenYuanID = 0;

    RelativeLayout listLayout = null;
    ListView adapterListView = null;
    ItemAdapter adapter = null;

    ArrayList<STCheDui> arrCheDui = new ArrayList<STCheDui>();
    ArrayList<STBanZu> arrBanZu = new ArrayList<STBanZu>();
    ArrayList<STZeRenRen> arrRenYuan = new ArrayList<STZeRenRen>();
    private JsonHttpResponseHandler handlerCheDui = null;
    private JsonHttpResponseHandler handlerBanZu = null;
    private JsonHttpResponseHandler handlerRenYuan = null;
    private ProgressDialog dialog = null;

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
                case R.id.imgKaoHeChaXun_Back:
                    finish();
                    break;
                case R.id.btnKaoHeChaXun_ChaXun:
                    Intent intentJieGuo = new Intent(KaoHeChaXunActivity.this, KaoHeChaXun_JieGuoActivity.class);
                    intentJieGuo.putExtra("CheDuiVal", curCheDuiID);
                    intentJieGuo.putExtra("BanZuVal", curBanZuID);
                    intentJieGuo.putExtra("RenYuanVal", curRenYuanID);
                    intentJieGuo.putExtra("StartDate", Integer.toString(nQiShiYear) + "-" + Integer.toString(nQiShiMonth+1) + "-" + Integer.toString(nQiShiDay));
                    intentJieGuo.putExtra("EndDate", Integer.toString(nJieShuYear) + "-" + Integer.toString(nJieShuMonth+1) + "-" + Integer.toString(nJieShuDay));
                    startActivity(intentJieGuo);
                    break;
                case R.id.rlKaoHeChaXun_KaoHeLuRu:
                    Intent intentLuRu = new Intent(KaoHeChaXunActivity.this, KaoHeLuRuActivity.class);
                    startActivity(intentLuRu);
                    finish();
                    break;
                case R.id.rlKaoHeChaXun_JieGuoFenXi:
                    Intent intentJieGuoFenXi = new Intent(KaoHeChaXunActivity.this, JieGuoFenXiActivity.class);
                    startActivity(intentJieGuoFenXi);
                    finish();
                    break;
                case R.id.lblKaoHeChaXun_QiShiRiQiVal:
                    nDateMode = 0;
                    new DatePickerDialog(KaoHeChaXunActivity.this, dateListener, nQiShiYear, nQiShiMonth, nQiShiDay).show();
                    break;
                case R.id.lblKaoHeChaXun_JieShuRiQiVal:
                    nDateMode = 1;
                    new DatePickerDialog(KaoHeChaXunActivity.this, dateListener, nJieShuYear, nJieShuMonth, nJieShuDay).show();
                    break;
                case R.id.imgKaoHeChaXun_CheDui:
                    curAdapterId = CHEDUI_ADAPTER;
                    adapter.notifyDataSetChanged();
                    listLayout.setVisibility(View.VISIBLE);
                    adapterListView.setSelectionAfterHeaderView();
                    break;
                case R.id.imgKaoHeChaXun_BanZu:
                    curAdapterId = BANZU_ADAPTER;
                    adapter.notifyDataSetChanged();
                    listLayout.setVisibility(View.VISIBLE);
                    adapterListView.setSelectionAfterHeaderView();
                    break;
                case R.id.imgKaoHeChaxun_RenYuan:
                    curAdapterId = RENYUAN_ADAPTER;
                    adapter.notifyDataSetChanged();
                    listLayout.setVisibility(View.VISIBLE);
                    adapterListView.setSelectionAfterHeaderView();
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
        setContentView(R.layout.kaohechaxun_activity);

        mainLayout = (RelativeLayout)findViewById(R.id.rlKaoHeChaXunBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlKaoHeChaXunBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        initControl();
        initHandler();

        dialog = ProgressDialog.show(
                KaoHeChaXunActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetAllCheDuiList(handlerCheDui);
    }

    private void initControl()
    {
        nQiShiYear = curdate.get(Calendar.YEAR);
        nQiShiMonth = curdate.get(Calendar.MONTH);
        nQiShiDay = curdate.get(Calendar.DAY_OF_MONTH);
        nJieShuYear = curdate.get(Calendar.YEAR);
        nJieShuMonth = curdate.get(Calendar.MONTH);
        nJieShuDay = curdate.get(Calendar.DAY_OF_MONTH);

        imgBack = (ImageView) findViewById(R.id.imgKaoHeChaXun_Back);
        imgBack.setOnClickListener(onClickListener);

        btnChaXun = (Button) findViewById(R.id.btnKaoHeChaXun_ChaXun);
        btnChaXun.setOnClickListener(onClickListener);

        rlKaoHeLuRu = (RelativeLayout) findViewById(R.id.rlKaoHeChaXun_KaoHeLuRu);
        rlKaoHeLuRu.setOnClickListener(onClickListener);

        rlJieGuoFenXi = (RelativeLayout) findViewById(R.id.rlKaoHeChaXun_JieGuoFenXi);
        rlJieGuoFenXi.setOnClickListener(onClickListener);

        lblQiShiRiQi = (TextView) findViewById(R.id.lblKaoHeChaXun_QiShiRiQiVal);
        lblQiShiRiQi.setText(GlobalData.getDateFormat(nQiShiYear, nQiShiMonth, nQiShiDay));
        lblQiShiRiQi.setOnClickListener(onClickListener);

        lblJieShuRiQi = (TextView) findViewById(R.id.lblKaoHeChaXun_JieShuRiQiVal);
        lblJieShuRiQi.setText(GlobalData.getDateFormat(nJieShuYear, nJieShuMonth, nJieShuDay));
        lblJieShuRiQi.setOnClickListener(onClickListener);

        imgCheDui = (ImageView) findViewById(R.id.imgKaoHeChaXun_CheDui);
        imgCheDui.setOnClickListener(onClickListener);

        imgBanZu = (ImageView) findViewById(R.id.imgKaoHeChaXun_BanZu);
        imgBanZu.setOnClickListener(onClickListener);

        imgRenYuan = (ImageView) findViewById(R.id.imgKaoHeChaxun_RenYuan);
        imgRenYuan.setOnClickListener(onClickListener);

        lblCheDui = (TextView) findViewById(R.id.lblKaoHeChaXun_CheDuiVal);
        lblBanZu = (TextView) findViewById(R.id.lblKaoHeChaXun_BanZuVal);
        lblRenYuan = (TextView) findViewById(R.id.lblKaoHeChaXun_RenYuanVal);

        listLayout = (RelativeLayout)findViewById(R.id.rlKaoHeChaXunList);
        listLayout.setClickable(true);
        listLayout.setOnClickListener(new View.OnClickListener()
        {
            @Override
            public void onClick(View v)
            {
                listLayout.setVisibility(View.GONE);
            }
        });

        adapterListView = (ListView)findViewById(R.id.listKaoHeChaXun);
        adapter = new ItemAdapter();
        adapterListView.setAdapter(adapter);
    }

    private void initHandler()
    {
        handlerCheDui = new JsonHttpResponseHandler()
        {
            int result = 0;

            @Override
            public void onSuccess(JSONObject jsonData)
            {
                result = 1;

                CommMgr.commService.parseGetAllCheDuiList(jsonData, arrCheDui);
                if (arrCheDui == null)
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
                    GlobalData.showToast(KaoHeChaXunActivity.this, getString(R.string.network_error));
                    KaoHeChaXunActivity.this.finish();
                }
                else
                {
                    if (arrCheDui != null && arrCheDui.size() > 0)
                    {
                        ShowCheDui(0);
                    }
                }
            }
        };

        handlerBanZu = new JsonHttpResponseHandler()
        {
            int result = 0;

            @Override
            public void onSuccess(JSONObject jsonData)
            {
                result = 1;

                CommMgr.commService.parseGetAllBanZuList(jsonData, arrBanZu);
                if (arrBanZu == null)
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
                    GlobalData.showToast(KaoHeChaXunActivity.this, getString(R.string.network_error));
                    KaoHeChaXunActivity.this.finish();
                }
                else
                {
                    if (arrBanZu != null && arrBanZu.size() > 0)
                    {
                        ShowBanZu(0);
                    }
                }
            }
        };

        handlerRenYuan = new JsonHttpResponseHandler()
        {
            int result = 0;

            @Override
            public void onSuccess(JSONObject jsonData)
            {
                result = 1;

                CommMgr.commService.parseGetAllZeRenRenList(jsonData, arrRenYuan);
                if (arrRenYuan == null)
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
                    GlobalData.showToast(KaoHeChaXunActivity.this, getString(R.string.network_error));
                    KaoHeChaXunActivity.this.finish();
                }
                else
                {
                    if (arrRenYuan != null && arrRenYuan.size() > 0)
                    {
                        ShowRenYuan(0);
                    }
                }
            }
        };

        return;
    }

    private void ShowCheDui(int nNo)
    {
        curCheDuiID = arrCheDui.get(nNo).uid;
        lblCheDui.setText(arrCheDui.get(nNo).name);
        lblBanZu.setText("");
        lblRenYuan.setText("");
        listLayout.setVisibility(View.GONE);

        dialog = ProgressDialog.show(
                KaoHeChaXunActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetAllBanZuList(handlerBanZu, Long.toString(curCheDuiID));

        return;
    }

    private void ShowBanZu(int nNo)
    {
        curBanZuID = arrBanZu.get(nNo).uid;
        lblBanZu.setText(arrBanZu.get(nNo).name);
        lblRenYuan.setText("");
        listLayout.setVisibility(View.GONE);

        dialog = ProgressDialog.show(
                KaoHeChaXunActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetAllZeRenRenList(handlerRenYuan,
                Long.toString(curBanZuID));

        return;
    }

    private void ShowRenYuan(int nNo)
    {
        curRenYuanID = arrRenYuan.get(nNo).uid;
        lblRenYuan.setText(arrRenYuan.get(nNo).name);
        listLayout.setVisibility(View.GONE);

        return;
    }

    public class ItemAdapter extends BaseAdapter
    {
        @Override
        public int getCount() {
            if (curAdapterId == CHEDUI_ADAPTER)
            {
                if (arrCheDui == null)
                    return 0;
                return arrCheDui.size();
            }
            else if (curAdapterId == BANZU_ADAPTER)
            {
                if (arrBanZu == null)
                    return 0;
                return arrBanZu.size();
            }
            else if (curAdapterId == RENYUAN_ADAPTER)
            {
                if (arrRenYuan == null)
                    return 0;
                return arrRenYuan.size();
            }

            return 0;
        }

        @Override
        public Object getItem(int position) {
            if (curAdapterId == CHEDUI_ADAPTER)
            {
                return arrCheDui.get(position);
            }
            else if (curAdapterId == BANZU_ADAPTER)
            {
                return arrBanZu.get(position);
            }
            else if (curAdapterId == RENYUAN_ADAPTER)
            {
                return arrRenYuan.get(position);
            }

            return null;
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
            STCheDui cheduiInfo = null;
            STBanZu banzuInfo = null;
            STZeRenRen renyuanInfo = null;

            if (curAdapterId == CHEDUI_ADAPTER)
            {
                cheduiInfo = arrCheDui.get(position);
            }
            else if (curAdapterId == BANZU_ADAPTER)
            {
                banzuInfo = arrBanZu.get(position);
            }
            else if (curAdapterId == RENYUAN_ADAPTER)
            {
                renyuanInfo = arrRenYuan.get(position);
            }
            else
            {
                return null;
            }

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
                if (cheduiInfo != null)
                    txtItem.setText(cheduiInfo.name);
                else if (banzuInfo != null)
                    txtItem.setText(banzuInfo.name);
                else if (renyuanInfo != null)
                    txtItem.setText(renyuanInfo.name);
                if (cheduiInfo != null)
                {
                    txtItem.setTag(position);
                    txtItem.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            Integer nID = (Integer)v.getTag();
                            if (nID != null)
                            {
                                ShowCheDui(nID.intValue());
                            }
                        }
                    });
                }
                else if (banzuInfo != null)
                {
                    txtItem.setTag(position);
                    txtItem.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            Integer nID = (Integer)v.getTag();
                            if (nID != null)
                            {
                                ShowBanZu(nID.intValue());
                            }
                        }
                    });
                }
                else if (renyuanInfo != null)
                {
                    txtItem.setTag(position);
                    txtItem.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            Integer nID = (Integer)v.getTag();
                            if (nID != null)
                            {
                                ShowRenYuan(nID.intValue());
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
                if (cheduiInfo != null)
                {
                    viewHolder.txtItem.setText(cheduiInfo.name);
                    viewHolder.txtItem.setTag(position);
                    viewHolder.txtItem.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            Integer nUid = (Integer)v.getTag();
                            if (nUid != null)
                                ShowCheDui(nUid.intValue());
                        }
                    });
                }
                else if (banzuInfo != null)
                {
                    viewHolder.txtItem.setText(banzuInfo.name);
                    viewHolder.txtItem.setTag(position);
                    viewHolder.txtItem.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            Integer nUid = (Integer)v.getTag();
                            if (nUid != null)
                                ShowBanZu(nUid.intValue());
                        }
                    });
                }
                else if (renyuanInfo != null)
                {
                    viewHolder.txtItem.setText(renyuanInfo.name);
                    viewHolder.txtItem.setTag(position);
                    viewHolder.txtItem.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            Integer nUid = (Integer)v.getTag();
                            if (nUid != null)
                                ShowRenYuan(nUid.intValue());
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
