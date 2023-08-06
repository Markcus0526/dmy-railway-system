package com.damytech.TieLu;

import android.app.Activity;
import android.app.DatePickerDialog;
import android.app.ProgressDialog;
import android.content.Intent;
import android.content.res.Resources;
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

public class ZiKongLvChaXunActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    final int MODE_BUMEN = 0;
    final int MODE_LEIXING = 1;

    long nSectorID = 0;

    int nLeiXingMode = 0;

    private int nMode = MODE_BUMEN;
    private int nYear = 0, nMonth = 0;
    private Calendar curdate = Calendar.getInstance();

    private ImageView imgBack = null;
    private TextView lblYueFen = null;
    private TextView lblSuoZaiBuMen = null;
    private ImageView imgSuoZaiBuMen = null;
    private TextView lblLeiXing = null;
    private ImageView imgLeiXing = null;
    private Button btnTongJi = null;

    ArrayList<STCheDui> arrCheDui = new ArrayList<STCheDui>();
    ArrayList<STTongJiLeiXing> arrLeiXing = new ArrayList<STTongJiLeiXing>();
    private JsonHttpResponseHandler handler = null;
    private ProgressDialog dialog = null;

    RelativeLayout listLayout = null;
    ListView adapterListView = null;
    ItemAdapter adapter = null;

    public class STTongJiLeiXing
    {
        long uid = 0;
        String name = "";
    }

    DatePickerDialog.OnDateSetListener dateListener =
            new DatePickerDialog.OnDateSetListener() {
                @Override
                public void onDateSet(DatePicker datePicker, int year, int month, int dayOfMonth) {
                    nYear = year;
                    nMonth = month;
                    lblYueFen.setText(GlobalData.getDateFormatWithYM(nYear, nMonth));
                }
            };

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgZiKongLvChaXun_Back:
                    finish();
                    break;
                case R.id.lblYueFenVal:
                    DatePickerDialog mDialog = new DatePickerDialog(ZiKongLvChaXunActivity.this, dateListener, nYear, nMonth+1, 0);
                    mDialog.show();
                    ((ViewGroup) mDialog.getDatePicker()).findViewById(Resources.getSystem().getIdentifier("day", "id", "android")).setVisibility(View.GONE);
                    break;
                case R.id.imgSuoZaiBuMen:
                    nMode = MODE_BUMEN;
                    adapter.notifyDataSetChanged();
                    listLayout.setVisibility(View.VISIBLE);
                    adapterListView.setSelectionAfterHeaderView();
                    break;
                case R.id.imgTongJiLeiXing:
                    nMode = MODE_LEIXING;
                    adapter.notifyDataSetChanged();
                    listLayout.setVisibility(View.VISIBLE);
                    adapterListView.setSelectionAfterHeaderView();
                    break;
                case R.id.btnTongJi:
                    String strTime = lblYueFen.getText().toString();

                    if (nLeiXingMode == 0)
                    {
                        Intent intent = new Intent(ZiKongLvChaXunActivity.this, SelfCheckActivity.class);
                        intent.putExtra("FINDTIME", strTime);
                        intent.putExtra("SECTORID", nSectorID);
                        startActivity(intent);
                    }
                    else
                    {
                        Intent intent = new Intent(ZiKongLvChaXunActivity.this, CombineCheckActivity.class);
                        intent.putExtra("FINDTIME", strTime);
                        intent.putExtra("SECTORID", nSectorID);
                        startActivity(intent);
                    }
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
        setContentView(R.layout.zikonglvchaxun_activity);

        mainLayout = (RelativeLayout)findViewById(R.id.rlZiKongLvChaXunBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlZiKongLvChaXunBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        initControl();
        initHandler();

        ShowLeiXing(0);

        dialog = ProgressDialog.show(
                ZiKongLvChaXunActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetCheDuiListWithUid(handler, "0");
    }

    private void initControl()
    {
        STTongJiLeiXing newItem = new STTongJiLeiXing();
        newItem.uid = 0;
        newItem.name = getString(R.string.liangweikaohe);
        arrLeiXing.add(newItem);
        newItem = new STTongJiLeiXing();
        newItem.uid = 1;
        newItem.name = getString(R.string.jiehebu);
        arrLeiXing.add(newItem);

        nYear = curdate.get(Calendar.YEAR);
        nMonth = curdate.get(Calendar.MONTH);

        imgBack = (ImageView) findViewById(R.id.imgZiKongLvChaXun_Back);
        imgBack.setOnClickListener(onClickListener);

        lblYueFen = (TextView) findViewById(R.id.lblYueFenVal);
        lblYueFen.setText(GlobalData.getDateFormatWithYM(nYear, nMonth));
        lblYueFen.setOnClickListener(onClickListener);

        lblSuoZaiBuMen = (TextView) findViewById(R.id.lblSuoZaiBuMenVal);
        imgSuoZaiBuMen = (ImageView) findViewById(R.id.imgSuoZaiBuMen);
        imgSuoZaiBuMen.setOnClickListener(onClickListener);
        lblLeiXing = (TextView) findViewById(R.id.lblTongJiLeiXingVal);
        imgLeiXing = (ImageView) findViewById(R.id.imgTongJiLeiXing);
        imgLeiXing.setOnClickListener(onClickListener);

        btnTongJi = (Button) findViewById(R.id.btnTongJi);
        btnTongJi.setOnClickListener(onClickListener);

        listLayout = (RelativeLayout)findViewById(R.id.rlList);
        listLayout.setClickable(true);
        listLayout.setOnClickListener(new View.OnClickListener()
        {
            @Override
            public void onClick(View v)
            {
                listLayout.setVisibility(View.GONE);
            }
        });

        adapterListView = (ListView)findViewById(R.id.listData);
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
                dialog.dismiss();
                result = 1;

                CommMgr.commService.parseGetCheDuiListWithUid(jsonData, arrCheDui);
                if (arrCheDui == null)
                    result = 0;
            }

            @Override
            public void onFailure(Throwable ex, String exception)
            {
                dialog.dismiss();
                result = 0;
            }

            @Override
            public void onFinish()
            {
                if (result == 0)
                {
                    GlobalData.showToast(ZiKongLvChaXunActivity.this, getString(R.string.network_error));
                    ZiKongLvChaXunActivity.this.finish();
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
    }

    private void ShowCheDui(int nNo)
    {
        lblSuoZaiBuMen.setText(arrCheDui.get(nNo).name);
        listLayout.setVisibility(View.GONE);
        nSectorID = arrCheDui.get(nNo).uid;

        return;
    }

    private void ShowLeiXing(int nNo)
    {
        lblLeiXing.setText(arrLeiXing.get(nNo).name);
        listLayout.setVisibility(View.GONE);

        return;
    }

    public class ItemAdapter extends BaseAdapter
    {
        @Override
        public int getCount() {
            if (nMode == MODE_BUMEN)
            {
                if (arrCheDui == null)
                    return 0;
                return arrCheDui.size();
            }
            else if (nMode == MODE_LEIXING)
            {
                if (arrLeiXing == null)
                    return 0;
                return arrLeiXing.size();
            }

            return 0;
        }

        @Override
        public Object getItem(int position) {
            if (nMode == MODE_BUMEN)
            {
                return arrCheDui.get(position);
            }
            else if (nMode == MODE_LEIXING)
            {
                return arrLeiXing.get(position);
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
            STCheDui stCheDui = null;
            STTongJiLeiXing stLeiXing = null;

            if (nMode == MODE_BUMEN)
            {
                stCheDui = arrCheDui.get(position);
            }
            else if (nMode == MODE_LEIXING)
            {
                nLeiXingMode = position;
                stLeiXing = arrLeiXing.get(position);
            }
            else
                return null;

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
                if (stCheDui != null)
                    txtItem.setText(stCheDui.name);
                else if (stLeiXing != null)
                    txtItem.setText(stLeiXing.name);

                if (stCheDui != null)
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
                else if (stLeiXing != null)
                {
                    txtItem.setTag(position);
                    txtItem.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            Integer nID = (Integer)v.getTag();
                            if (nID != null)
                            {
                                ShowLeiXing(nID.intValue());
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
                if (stCheDui != null)
                {
                    viewHolder.txtItem.setText(stCheDui.name);
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
                else if (stLeiXing != null)
                {
                    viewHolder.txtItem.setText(stLeiXing.name);
                    viewHolder.txtItem.setTag(position);
                    viewHolder.txtItem.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            Integer nUid = (Integer)v.getTag();
                            if (nUid != null)
                                ShowLeiXing(nUid.intValue());
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

    private DatePicker findDatePicker(ViewGroup group) {
        if (group != null) {
            for (int i = 0, j = group.getChildCount(); i < j; i++) {
                View child = group.getChildAt(i);
                if (child instanceof DatePicker) {
                    return (DatePicker) child;
                } else if (child instanceof ViewGroup) {
                    DatePicker result = findDatePicker((ViewGroup) child);
                    if (result != null)
                        return result;
                }
            }
        }
        return null;
    }
}
