package com.damytech.TieLu;

import android.app.Activity;
import android.app.DatePickerDialog;
import android.app.ProgressDialog;
import android.content.Context;
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
import com.damytech.utils.SmartImageView.Global;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.Calendar;

public class JiFenChaXunActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    final int MODE_BUMEN = 0;
    final int MODE_XIANLU = 1;

    private int nMode = MODE_BUMEN;
    private int nYear = 0, nMonth = 0;
    private Calendar curdate = Calendar.getInstance();

    private ImageView imgBack = null;
    private TextView lblYueFen = null;
    private TextView lblSuoZaiBuMen = null;
    private ImageView imgSuoZaiBuMen = null;
    private TextView lblXianLu = null;
    private ImageView imgXianLu = null;
    private Button btnTongJi = null;
    private EditText txtName = null;

    long nSectorID = 0;
    long nRouteID = 0;

    ArrayList<STCheDui> arrCheDui = new ArrayList<STCheDui>();
    ArrayList<STBanZu> arrBanZu = new ArrayList<STBanZu>();
    private JsonHttpResponseHandler handler = null;
    private JsonHttpResponseHandler handlerBanZu = null;
    private ProgressDialog dialog = null;

    RelativeLayout listLayout = null;
    ListView adapterListView = null;
    ItemAdapter adapter = null;

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
                case R.id.imgJiFenChaXun_Back:
                    finish();
                    break;
                case R.id.lblYueFenVal:
                    DatePickerDialog mDialog = new DatePickerDialog(JiFenChaXunActivity.this, dateListener, nYear, nMonth+1, 0);
                    mDialog.show();
                    ((ViewGroup) mDialog.getDatePicker()).findViewById(Resources.getSystem().getIdentifier("day", "id", "android")).setVisibility(View.GONE);
                    break;
                case R.id.imgSuoZaiBuMen:
                    nMode = MODE_BUMEN;
                    adapter.notifyDataSetChanged();
                    listLayout.setVisibility(View.VISIBLE);
                    adapterListView.setSelectionAfterHeaderView();
                    break;
                case R.id.imgXianLu:
                    nMode = MODE_XIANLU;
                    adapter.notifyDataSetChanged();
                    listLayout.setVisibility(View.VISIBLE);
                    adapterListView.setSelectionAfterHeaderView();
                    break;
                case R.id.btnTongJi:
                    String strTime = lblYueFen.getText().toString();
                    String strName = txtName.getText().toString();
//                    if (strName.length() == 0)
//                    {
//                        GlobalData.showToast(JiFenChaXunActivity.this, getString(R.string.qingshurumingcheng));
//                        return;
//                    }

                    Global.hideKeyboardFromText(txtName, JiFenChaXunActivity.this);

                    Intent intent = new Intent(JiFenChaXunActivity.this, JiFenChaXunJieGuoActivity.class);
                    intent.putExtra("FINDTIME", strTime);
                    intent.putExtra("NAME", strName);
                    intent.putExtra("SECTORID", nSectorID);
                    intent.putExtra("ROUTEID", nRouteID);
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
        setContentView(R.layout.jifenchaxun_activity);

        mainLayout = (RelativeLayout)findViewById(R.id.rlJiFenChaXunBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlJiFenChaXunBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        initControl();
        initHandler();

        dialog = ProgressDialog.show(
                JiFenChaXunActivity.this,
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
        nYear = curdate.get(Calendar.YEAR);
        nMonth = curdate.get(Calendar.MONTH);

        imgBack = (ImageView) findViewById(R.id.imgJiFenChaXun_Back);
        imgBack.setOnClickListener(onClickListener);

        lblYueFen = (TextView) findViewById(R.id.lblYueFenVal);
        lblYueFen.setText(GlobalData.getDateFormatWithYM(nYear, nMonth));
        lblYueFen.setOnClickListener(onClickListener);

        txtName = (EditText) findViewById(R.id.txtName);

        lblSuoZaiBuMen = (TextView) findViewById(R.id.lblSuoZaiBuMenVal);
        imgSuoZaiBuMen = (ImageView) findViewById(R.id.imgSuoZaiBuMen);
        imgSuoZaiBuMen.setOnClickListener(onClickListener);
        lblXianLu = (TextView) findViewById(R.id.lblXianLuVal);
        imgXianLu = (ImageView) findViewById(R.id.imgXianLu);
        imgXianLu.setOnClickListener(onClickListener);

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
                result = 1;
                dialog.dismiss();

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
                    GlobalData.showToast(JiFenChaXunActivity.this, getString(R.string.network_error));
                    JiFenChaXunActivity.this.finish();
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
                dialog.dismiss();

                CommMgr.commService.parseGetAllBanZuListWithJiFen(jsonData, arrBanZu);
                if (arrBanZu == null)
                    result = 0;
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

                if (result == 0)
                {
                    GlobalData.showToast(JiFenChaXunActivity.this, getString(R.string.network_error));
                    JiFenChaXunActivity.this.finish();
                }
                else
                {
                    if (arrBanZu != null)
                    {
                        ShowBanZu(0);
                    }
                }
            }
        };
    }

    private void ShowCheDui(int nNo)
    {
        lblSuoZaiBuMen.setText(arrCheDui.get(nNo).name);
        nSectorID = arrCheDui.get(nNo).uid;
        listLayout.setVisibility(View.GONE);

        dialog = ProgressDialog.show(
                JiFenChaXunActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetAllBanZuListWithJiFen(handlerBanZu, Long.toString(nSectorID));

        return;
    }

    private void ShowBanZu(int nNo)
    {
        if (arrBanZu.size() == 0) {
            nRouteID = 0;
            lblXianLu.setText("");

            return;
        }

        lblXianLu.setText(arrBanZu.get(nNo).name);
        nRouteID = arrBanZu.get(nNo).uid;
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
            else if (nMode == MODE_XIANLU)
            {
                if (arrBanZu == null)
                    return 0;
                return arrBanZu.size();
            }

            return 0;
        }

        @Override
        public Object getItem(int position) {
            if (nMode == MODE_BUMEN)
            {
                return arrCheDui.get(position);
            }
            else if (nMode == MODE_XIANLU)
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
            STCheDui stCheDui = null;
            STBanZu stRoute = null;

            if (nMode == MODE_BUMEN)
            {
                stCheDui = arrCheDui.get(position);
            }
            else if (nMode == MODE_XIANLU)
            {
                stRoute = arrBanZu.get(position);
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
                else if (stRoute != null)
                    txtItem.setText(stRoute.name);

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
                else if (stRoute != null)
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
                else if (stRoute != null)
                {
                    viewHolder.txtItem.setText(stRoute.name);
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
}
