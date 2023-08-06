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
import com.damytech.STData.STKaoHeJiLu;
import com.damytech.STData.STKaoHeTongJi;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;
import org.achartengine.ChartFactory;
import org.achartengine.chart.BarChart;
import org.achartengine.model.XYMultipleSeriesDataset;
import org.achartengine.model.XYSeries;
import org.achartengine.renderer.*;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.Calendar;

public class JieGuoFenXiActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    private int nDateMode = 0; // 0 : QiShiRiQi       1 : JieShuRiQi
    private int nQiShiYear = 0, nQiShiMonth = 0, nQiShiDay = 0;
    private int nJieShuYear = 0, nJieShuMonth = 0, nJieShuDay = 0;
    private Calendar curdate = Calendar.getInstance();

    private TextView lblQiShiRiQi = null;
    private TextView lblJieShuRiQi = null;
    private ImageView imgBack = null;
    private RelativeLayout rlKaoHeLuRu = null;
    private RelativeLayout rlKaoHeChaXun = null;

    private ImageView imgCheDui = null;
    private ImageView imgBanZu = null;
    private TextView lblCheDui = null;
    private TextView lblBanZu = null;
    private Button btnTongJi = null;

    private final int CHEDUI_ADAPTER = 0;
    private final int BANZU_ADAPTER = 1;
    private int curAdapterId = CHEDUI_ADAPTER;
    long curCheDuiID = 0;
    long curBanZuID = 0;
    ArrayList<STCheDui> arrCheDui = new ArrayList<STCheDui>();
    ArrayList<STBanZu> arrBanZu = new ArrayList<STBanZu>();
    ArrayList<STKaoHeTongJi> arrTongJi = new ArrayList<STKaoHeTongJi>();
    private JsonHttpResponseHandler handlerCheDui = null;
    private JsonHttpResponseHandler handlerBanZu = null;
    private JsonHttpResponseHandler handlerTongJi = null;
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
                case R.id.imgJieGuoFenXi_Back:
                    finish();
                    break;
                case R.id.rlJieGuoFenXi_KaoHeLuRu:
                    Intent intentLuRu = new Intent(JieGuoFenXiActivity.this, KaoHeLuRuActivity.class);
                    startActivity(intentLuRu);
                    finish();
                    break;
                case R.id.rlJieGuoFenXi_KaoHeChaXun:
                    Intent intentChaXun = new Intent(JieGuoFenXiActivity.this, KaoHeChaXunActivity.class);
                    startActivity(intentChaXun);
                    finish();
                    break;
                case R.id.imgJieGuoFenXi_CheDui:
                    curAdapterId = CHEDUI_ADAPTER;
                    adapter.notifyDataSetChanged();
                    listLayout.setVisibility(View.VISIBLE);
                    adapterListView.setSelectionAfterHeaderView();
                    break;
                case R.id.imgJieGuoFenXi_BanZu:
                    curAdapterId = BANZU_ADAPTER;
                    adapter.notifyDataSetChanged();
                    listLayout.setVisibility(View.VISIBLE);
                    adapterListView.setSelectionAfterHeaderView();
                    break;
                case R.id.btnJieGuoFenXi_ChaXun:
                    if (curCheDuiID == 0)
                    {
                        GlobalData.showToast(JieGuoFenXiActivity.this, getString(R.string.xuanze_chedui));
                        return;
                    }
                    if (curBanZuID == 0)
                    {
                        GlobalData.showToast(JieGuoFenXiActivity.this, getString(R.string.xuanze_banzu));
                        return;
                    }

                    curCheDuiID = 1;
                    curBanZuID = 1;
                    dialog = ProgressDialog.show(
                            JieGuoFenXiActivity.this,
                            "",
                            getString(R.string.waiting),
                            true,
                            false,
                            null
                    );
                    CommMgr.commService.GetKaoHeChartData(handlerTongJi,
                            Long.toString(curCheDuiID),
                            Long.toString(curBanZuID),
                            GlobalData.getDateFormat(nQiShiYear, nQiShiMonth, nQiShiDay),
                            GlobalData.getDateFormat(nJieShuYear, nJieShuMonth, nJieShuDay));
                    break;
                case R.id.lblJieGuoFenXi_QiShiRiQiVal:
                    nDateMode = 0;
                    new DatePickerDialog(JieGuoFenXiActivity.this, dateListener, nQiShiYear, nQiShiMonth, nQiShiDay).show();
                    break;
                case R.id.lblJieGuoFenXi_JieShuRiQiVal:
                    nDateMode = 1;
                    new DatePickerDialog(JieGuoFenXiActivity.this, dateListener, nJieShuYear, nJieShuMonth, nJieShuDay).show();
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
        setContentView(R.layout.jieguofenxi_activity);

        mainLayout = (RelativeLayout)findViewById(R.id.rlJieGuoFenXiBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlJieGuoFenXiBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        initControl();
        initHandler();

        dialog = ProgressDialog.show(
                JieGuoFenXiActivity.this,
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

        imgBack = (ImageView) findViewById(R.id.imgJieGuoFenXi_Back);
        imgBack.setOnClickListener(onClickListener);

        rlKaoHeLuRu = (RelativeLayout) findViewById(R.id.rlJieGuoFenXi_KaoHeLuRu);
        rlKaoHeLuRu.setOnClickListener(onClickListener);

        rlKaoHeChaXun = (RelativeLayout) findViewById(R.id.rlJieGuoFenXi_KaoHeChaXun);
        rlKaoHeChaXun.setOnClickListener(onClickListener);

        lblQiShiRiQi = (TextView) findViewById(R.id.lblJieGuoFenXi_QiShiRiQiVal);
        lblQiShiRiQi.setText(GlobalData.getDateFormat(nQiShiYear, nQiShiMonth, nQiShiDay));
        lblQiShiRiQi.setOnClickListener(onClickListener);

        lblJieShuRiQi = (TextView) findViewById(R.id.lblJieGuoFenXi_JieShuRiQiVal);
        lblJieShuRiQi.setText(GlobalData.getDateFormat(nJieShuYear, nJieShuMonth, nJieShuDay));
        lblJieShuRiQi.setOnClickListener(onClickListener);

        imgCheDui = (ImageView) findViewById(R.id.imgJieGuoFenXi_CheDui);
        imgCheDui.setOnClickListener(onClickListener);
        imgBanZu = (ImageView) findViewById(R.id.imgJieGuoFenXi_BanZu);
        imgBanZu.setOnClickListener(onClickListener);
        lblCheDui = (TextView) findViewById(R.id.lblJieGuoFenXi_CheDuiVal);
        lblBanZu = (TextView) findViewById(R.id.lblJieGuoFenXi_BanZuVal);

        btnTongJi = (Button) findViewById(R.id.btnJieGuoFenXi_ChaXun);
        btnTongJi.setOnClickListener(onClickListener);

        listLayout = (RelativeLayout)findViewById(R.id.rlJieGuoFenXi_List);
        listLayout.setClickable(true);
        listLayout.setOnClickListener(new View.OnClickListener()
        {
            @Override
            public void onClick(View v)
            {
                listLayout.setVisibility(View.GONE);
            }
        });

        adapterListView = (ListView)findViewById(R.id.listJieGuoFenXi_List);
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
                    GlobalData.showToast(JieGuoFenXiActivity.this, getString(R.string.network_error));
                    JieGuoFenXiActivity.this.finish();
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
                    GlobalData.showToast(JieGuoFenXiActivity.this, getString(R.string.network_error));
                    JieGuoFenXiActivity.this.finish();
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

        handlerTongJi = new JsonHttpResponseHandler()
        {
            int result = 0;

            @Override
            public void onSuccess(JSONObject jsonData)
            {
                result = 1;

                CommMgr.commService.parseGetKaoHeChartData(jsonData, arrTongJi);
                if (arrTongJi == null)
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
                    GlobalData.showToast(JieGuoFenXiActivity.this, getString(R.string.network_error));
                    JieGuoFenXiActivity.this.finish();
                }
                else
                {
                    if (arrTongJi != null && arrTongJi.size() > 0)
                    {
                        ShowTongJi();
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
        listLayout.setVisibility(View.GONE);

        dialog = ProgressDialog.show(
                JieGuoFenXiActivity.this,
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

            if (curAdapterId == CHEDUI_ADAPTER)
            {
                cheduiInfo = arrCheDui.get(position);
            }
            else if (curAdapterId == BANZU_ADAPTER)
            {
                banzuInfo = arrBanZu.get(position);
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
            }

            return convertView;
        }
    }

    public class STViewHolder
    {
        public TextView txtItem = null;
    }

    private void ShowTongJi(){
        XYSeries incomeSeries = new XYSeries("项点");
        for(int i=0;i<arrTongJi.size();i++){
            incomeSeries.add(i,arrTongJi.get(i).count);
        }

        XYMultipleSeriesDataset dataset = new XYMultipleSeriesDataset();
        dataset.addSeries(incomeSeries);

        XYSeriesRenderer incomeRenderer = new XYSeriesRenderer();
        incomeRenderer.setColor(Color.rgb(243, 152, 0));
        incomeRenderer.setFillPoints(true);
        incomeRenderer.setLineWidth(2);
        incomeRenderer.setDisplayChartValues(true);

        XYMultipleSeriesRenderer multiRenderer = new XYMultipleSeriesRenderer();
        multiRenderer.setXLabels(0);
        multiRenderer.setChartTitle("结果分析");
        multiRenderer.setXTitle("项点名称");
        multiRenderer.setYTitle("记录数量");
        multiRenderer.setBarSpacing(0.5f);
        multiRenderer.setZoomButtonsVisible(true);
        for(int i=0; i< arrTongJi.size();i++){
            multiRenderer.addXTextLabel(i, arrTongJi.get(i).category);
        }

        multiRenderer.addSeriesRenderer(incomeRenderer);

        Intent intent = ChartFactory.getBarChartIntent(getBaseContext(), dataset, multiRenderer, BarChart.Type.DEFAULT);
        startActivity(intent);

    }
}
