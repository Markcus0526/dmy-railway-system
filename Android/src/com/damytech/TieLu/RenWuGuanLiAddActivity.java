package com.damytech.TieLu;

import android.app.Activity;
import android.app.DatePickerDialog;
import android.app.ProgressDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.graphics.Color;
import android.graphics.Rect;
import android.net.Uri;
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

import java.io.File;
import java.util.ArrayList;
import java.util.Calendar;

public class RenWuGuanLiAddActivity extends FileBrowserActivity{
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    private int nDateMode = 0; // 0 : QiShiRiQi       1 : JieShuRiQi
    private int nQiShiYear = 0, nQiShiMonth = 0, nQiShiDay = 0;
    private int nJieShuYear = 0, nJieShuMonth = 0, nJieShuDay = 0;
    private Calendar curdate = Calendar.getInstance();

    private EditText txtTitle = null;
    private EditText txtContent = null;
    private ImageView imgBack = null;
    private TextView lblQiShiRiQi = null;
    private TextView lblJieShuRiQi = null;
    private ImageView imgJiBie = null;
    private TextView lblJiBie = null;
    private ImageView imgJiDuan = null;
    private TextView lblJiDuan = null;
    private ImageView imgCheDui = null;
    private TextView lblCheDui = null;
    private ImageView imgRenYuan = null;
    private TextView lblRenYuan = null;
    private TextView lblShouXinRen = null;
    private Button btnAddRenYuan = null;
    private Button btnAddFile = null;
    private Button btnBrowse = null;

    long curJiBieID = 0;
    long curJiDuanID = 0;
    long curCheDuiID = 0;
    long curRenYuanID = 0;

    private final int JIBIE_ADAPTER = 0;
    private final int BUMEN_ADAPTER = 1;
    private final int CHEDUI_ADAPTER = 2;
    private final int RENYUAN_ADAPTER = 3;
    private int curAdapterId = BUMEN_ADAPTER;

    RelativeLayout listLayout = null;
    ListView adapterListView = null;
    ItemAdapter adapter = null;

    private String strSXRList = "";
    private String strSXRIDList = "";
    ArrayList<STRenYuan> arrUploadRenYuan = new ArrayList<STRenYuan>();

    private int RenYuanMode = 0; //0   :   BuMen      1     :   CheDui
    ArrayList<STJiBie> arrJiBie = new ArrayList<STJiBie>();
    ArrayList<STJiDuan> arrJiDuan = new ArrayList<STJiDuan>();
    ArrayList<STCheDui> arrCheDui = new ArrayList<STCheDui>();
    ArrayList<STRenYuan> arrRenYuan = new ArrayList<STRenYuan>();
    private JsonHttpResponseHandler handlerJiDuan = null;
    private JsonHttpResponseHandler handlerCheDui = null;
    private JsonHttpResponseHandler handlerRenYuan = null;
    private ProgressDialog dialog = null;

    private static final int FILE_RESULT = 1;

    public class STJiBie
    {
        long uid = 0;
        String name = "";
    }

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

    DialogInterface.OnDismissListener onDismissListener = new DialogInterface.OnDismissListener() {
        @Override
        public void onDismiss(DialogInterface dialog) {
            runOnUiThread(new Runnable() {
                @Override
                public void run() {
                    if ( (szSelFilePath.length() != 0) && (szSelFileName.length() != 0) ) {
                        //lblAddFile.setText(getString(R.string.fujian) + szSelFileName);
                    }
                }
            });
        }
    };

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgRenWuGuanLiAdd_Back:
                    finish();
                    break;
                case R.id.lblRenWuGuanLiAdd_QiShiRiQiVal:
                    nDateMode = 0;
                    new DatePickerDialog(RenWuGuanLiAddActivity.this, dateListener, nQiShiYear, nQiShiMonth, nQiShiDay).show();
                    break;
                case R.id.lblRenWuGuanLiAdd_JieShuRiQiVal:
                    nDateMode = 1;
                    new DatePickerDialog(RenWuGuanLiAddActivity.this, dateListener, nJieShuYear, nJieShuMonth, nJieShuDay).show();
                    break;
                case R.id.imgRenWuGuanLiAdd_JiBie:
                    curAdapterId = JIBIE_ADAPTER;
                    adapter.notifyDataSetChanged();
                    listLayout.setVisibility(View.VISIBLE);
                    adapterListView.setSelectionAfterHeaderView();
                    break;
                case R.id.imgRenWuGuanLiAdd_BuMen:
                    strSXRIDList = "";
                    strSXRList = "";
                    arrUploadRenYuan.clear();
                    lblShouXinRen.setText(strSXRList);
                    curAdapterId = BUMEN_ADAPTER;
                    adapter.notifyDataSetChanged();
                    listLayout.setVisibility(View.VISIBLE);
                    adapterListView.setSelectionAfterHeaderView();
                    break;
                case R.id.imgRenWuGuanLiAdd_CheDui:
                    strSXRIDList = "";
                    strSXRList = "";
                    arrUploadRenYuan.clear();
                    lblShouXinRen.setText(strSXRList);
                    curAdapterId = CHEDUI_ADAPTER;
                    adapter.notifyDataSetChanged();
                    listLayout.setVisibility(View.VISIBLE);
                    adapterListView.setSelectionAfterHeaderView();
                    break;
                case R.id.imgRenWuGuanLiAdd_RenYuan:
                    curAdapterId = RENYUAN_ADAPTER;
                    adapter.notifyDataSetChanged();
                    listLayout.setVisibility(View.VISIBLE);
                    adapterListView.setSelectionAfterHeaderView();
                    break;
                case R.id.btnRenWuGuanLiAdd_AddShouXinRen:
                    String strName = lblRenYuan.getText().toString();
                    if (strSXRList.length() == 0)
                    {
                        strSXRList = strName;
                        strSXRIDList = Long.toString(curRenYuanID);

                        STRenYuan renYuan = new STRenYuan();
                        renYuan.uid = curRenYuanID;
                        renYuan.name = strName;

                        arrUploadRenYuan.add(renYuan);
                    }
                    else
                    {
                        boolean bFlag = true;
                        for (int i = 0; i < arrUploadRenYuan.size(); i++)
                        {
                            if (arrUploadRenYuan.get(i).uid == curRenYuanID)
                            {
                                bFlag = false;
                                break;
                            }
                        }

                        if (bFlag == true)
                        {
                            strSXRList = strSXRList + ", " + strName;
                            strSXRIDList = strSXRIDList + "," + Long.toString(curRenYuanID);

                            STRenYuan renYuan = new STRenYuan();
                            renYuan.uid = curRenYuanID;
                            renYuan.name = strName;

                            arrUploadRenYuan.add(renYuan);
                        }
                    }
                    lblShouXinRen.setText(strSXRList);

                    break;
                case R.id.btnRenWuGuanLiAdd_App:
                    Intent intent_addfile = new Intent("android.intent.action.GET_CONTENT");
                    intent_addfile.setType("application/pdf;application/vnd.openxmlformats—officedocument.spreadsheetml.sheet;application/vnd.openxmlformats—officedocument.wordprocessingml.document");
                    intent_addfile.addCategory("android.intent.category.OPENABLE");
                    try
                    {
                        RenWuGuanLiAddActivity.this.startActivityForResult(intent_addfile, FILE_RESULT);
                    }
                    catch (Exception localException2)
                    {
                    }
                    break;
                case R.id.btnRenWuGuanLiAdd_YuLan:
                    String strTitle = txtTitle.getText().toString();
                    if (strTitle.length() == 0)
                    {
                        GlobalData.showToast(RenWuGuanLiAddActivity.this, getString(R.string.insert_renwumingcheng));
                        return;
                    }
                    String strContent = txtContent.getText().toString();
                    if (strContent.length() == 0)
                    {
                        GlobalData.showToast(RenWuGuanLiAddActivity.this, getString(R.string.shuru_fabuneirong));
                        return;
                    }
                    if (arrUploadRenYuan.size() == 0)
                    {
                        GlobalData.showToast(RenWuGuanLiAddActivity.this, getString(R.string.xuanze_shouxinren));
                        return;
                    }

                    Intent intentBrowse = new Intent(RenWuGuanLiAddActivity.this, RenWuYuLanActivity.class);
                    intentBrowse.putExtra("title", strTitle);
                    intentBrowse.putExtra("starttime", GlobalData.getDateFormat(nQiShiYear, nQiShiMonth, nQiShiDay));
                    intentBrowse.putExtra("endtime", GlobalData.getDateFormat(nJieShuYear, nJieShuMonth, nJieShuDay));
                    intentBrowse.putExtra("ShouXinRen", lblShouXinRen.getText().toString());
                    intentBrowse.putExtra("ShouXinRenIDList", strSXRIDList);
                    intentBrowse.putExtra("content", strContent);
                    intentBrowse.putExtra("FileName", szSelFileName);
                    intentBrowse.putExtra("FilePath", szSelFilePath);
                    startActivity(intentBrowse);
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
        setContentView(R.layout.renwuguanliadd_activity);

        mainLayout = (RelativeLayout)findViewById(R.id.rlRenWuGuanLiAddBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlRenWuGuanLiAddBack));
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
        newItem.name = getString(R.string.STR_BUMEN);
        arrJiBie.add(newItem);
        newItem = new STJiBie();
        newItem.uid = 1;
        newItem.name = getString(R.string.STR_CHEDUI);
        arrJiBie.add(newItem);

        nQiShiYear = curdate.get(Calendar.YEAR);
        nQiShiMonth = curdate.get(Calendar.MONTH);
        nQiShiDay = curdate.get(Calendar.DAY_OF_MONTH);
        nJieShuYear = curdate.get(Calendar.YEAR);
        nJieShuMonth = curdate.get(Calendar.MONTH);
        nJieShuDay = curdate.get(Calendar.DAY_OF_MONTH);

        imgBack = (ImageView) findViewById(R.id.imgRenWuGuanLiAdd_Back);
        imgBack.setOnClickListener(onClickListener);

        txtTitle = (EditText) findViewById(R.id.txtRenWuGuanLiAdd_MingChengVal);
        txtContent = (EditText) findViewById(R.id.txtRenWuGuanLiAdd_NeiRong);

        lblQiShiRiQi = (TextView) findViewById(R.id.lblRenWuGuanLiAdd_QiShiRiQiVal);
        lblQiShiRiQi.setText(GlobalData.getDateFormat(nQiShiYear, nQiShiMonth, nQiShiDay));
        lblQiShiRiQi.setOnClickListener(onClickListener);

        lblJieShuRiQi = (TextView) findViewById(R.id.lblRenWuGuanLiAdd_JieShuRiQiVal);
        lblJieShuRiQi.setText(GlobalData.getDateFormat(nJieShuYear, nJieShuMonth, nJieShuDay));
        lblJieShuRiQi.setOnClickListener(onClickListener);

        lblJiBie = (TextView) findViewById(R.id.lblRenWuGuanLiAdd_JiBieVal);
        lblJiDuan = (TextView) findViewById(R.id.lblRenWuGuanLiAdd_BuMenVal);
        lblCheDui = (TextView) findViewById(R.id.lblRenWuGuanLiAdd_CheDuiVal);
        lblRenYuan = (TextView) findViewById(R.id.lblRenWuGuanLiAdd_RenYuanVal);
        lblShouXinRen = (TextView) findViewById(R.id.lblRenWuGuanLiAdd_LiuZhuanRenVal);

        imgJiBie = (ImageView) findViewById(R.id.imgRenWuGuanLiAdd_JiBie);
        imgJiBie.setOnClickListener(onClickListener);

        imgJiDuan = (ImageView) findViewById(R.id.imgRenWuGuanLiAdd_BuMen);
        imgJiDuan.setOnClickListener(onClickListener);

        imgCheDui = (ImageView) findViewById(R.id.imgRenWuGuanLiAdd_CheDui);
        imgCheDui.setOnClickListener(onClickListener);

        imgRenYuan = (ImageView) findViewById(R.id.imgRenWuGuanLiAdd_RenYuan);
        imgRenYuan.setOnClickListener(onClickListener);

        btnAddRenYuan = (Button) findViewById(R.id.btnRenWuGuanLiAdd_AddShouXinRen);
        btnAddRenYuan.setOnClickListener(onClickListener);

        btnAddFile = (Button) findViewById(R.id.btnRenWuGuanLiAdd_App);
        btnAddFile.setOnClickListener(onClickListener);

        btnBrowse = (Button) findViewById(R.id.btnRenWuGuanLiAdd_YuLan);
        btnBrowse.setOnClickListener(onClickListener);

        listLayout = (RelativeLayout)findViewById(R.id.rlRenWuGuanLiAdd_List);
        listLayout.setClickable(true);
        listLayout.setOnClickListener(new View.OnClickListener()
        {
            @Override
            public void onClick(View v)
            {
                listLayout.setVisibility(View.GONE);
            }
        });

        adapterListView = (ListView)findViewById(R.id.listRenWuGuanLiAdd_List);
        adapter = new ItemAdapter();
        adapterListView.setAdapter(adapter);

        lblCheDui.setEnabled(false);
        imgCheDui.setEnabled(false);
    }

    private void initHandler()
    {
        handlerJiDuan = new JsonHttpResponseHandler()
        {
            int result = 0;

            @Override
            public void onSuccess(JSONObject jsonData)
            {
                result = 1;

                CommMgr.commService.parseGetJiDuanList(jsonData, arrJiDuan);
                if (arrJiDuan == null)
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
                    GlobalData.showToast(RenWuGuanLiAddActivity.this, getString(R.string.network_error));
                    RenWuGuanLiAddActivity.this.finish();
                }
                else
                {
                    if (arrJiDuan != null && arrJiDuan.size() > 0)
                    {
                        ShowJiDuan(0);
                    }
                }
            }
        };

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
                    GlobalData.showToast(RenWuGuanLiAddActivity.this, getString(R.string.network_error));
                    RenWuGuanLiAddActivity.this.finish();
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

        handlerRenYuan = new JsonHttpResponseHandler()
        {
            int result = 0;

            @Override
            public void onSuccess(JSONObject jsonData)
            {
                result = 1;

                if (RenYuanMode == 0)
                    CommMgr.commService.parseGetAllBMRenYuanList(jsonData, arrRenYuan);
                else
                    CommMgr.commService.parseGetAllCDRenYuanList(jsonData, arrRenYuan);

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
                    GlobalData.showToast(RenWuGuanLiAddActivity.this, getString(R.string.network_error));
                    RenWuGuanLiAddActivity.this.finish();
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

    private void ShowJiBie(int nNo)
    {
        curJiBieID = nNo;
        lblJiBie.setText(arrJiBie.get(nNo).name);
        listLayout.setVisibility(View.GONE);

        dialog = ProgressDialog.show(
                RenWuGuanLiAddActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );

        if (curJiBieID == 0)
            CommMgr.commService.GetJiDuanList(handlerJiDuan);
        else
            CommMgr.commService.GetAllCheDuiList(handlerCheDui);

        return;
    }

    private void ShowJiDuan(int nNo)
    {
        curJiDuanID = arrJiDuan.get(nNo).uid;
        lblJiDuan.setText(arrJiDuan.get(nNo).name);
        lblRenYuan.setText("");
        listLayout.setVisibility(View.GONE);

        RenYuanMode = 0;

        dialog = ProgressDialog.show(
                RenWuGuanLiAddActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetAllBMRenYuanList(handlerRenYuan, Long.toString(curJiDuanID));

        return;
    }

    private void ShowCheDui(int nNo)
    {
        curCheDuiID = arrCheDui.get(nNo).uid;
        lblCheDui.setText(arrCheDui.get(nNo).name);
        lblRenYuan.setText("");
        listLayout.setVisibility(View.GONE);

        RenYuanMode = 1;

        dialog = ProgressDialog.show(
                RenWuGuanLiAddActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetAllCDRenYuanList(handlerRenYuan, Long.toString(curCheDuiID));

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
            if (curAdapterId == BUMEN_ADAPTER)
            {
                if (arrJiDuan == null)
                    return 0;
                return arrJiDuan.size();
            }
            else if (curAdapterId == CHEDUI_ADAPTER)
            {
                if (arrCheDui == null)
                    return 0;
                return arrCheDui.size();
            }
            else if (curAdapterId == JIBIE_ADAPTER)
            {
                if (arrJiBie == null)
                    return 0;
                return arrJiBie.size();
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
            if (curAdapterId == BUMEN_ADAPTER)
            {
                return arrJiDuan.get(position);
            }
            else if (curAdapterId == CHEDUI_ADAPTER)
            {
                return arrCheDui.get(position);
            }
            else if (curAdapterId == JIBIE_ADAPTER)
            {
                return arrJiBie.get(position);
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
            STJiBie jibieInfo = null;
            STJiDuan bumenInfo = null;
            STCheDui cheduiInfo = null;
            STRenYuan renyuanInfo = null;

            if (curAdapterId == BUMEN_ADAPTER)
            {
                bumenInfo = arrJiDuan.get(position);
            }
            else if (curAdapterId == CHEDUI_ADAPTER)
            {
                cheduiInfo = arrCheDui.get(position);
            }
            else if (curAdapterId == JIBIE_ADAPTER)
            {
                jibieInfo = arrJiBie.get(position);
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
                RelativeLayout.LayoutParams layoutParams = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.FILL_PARENT, 90);
                convertView.setLayoutParams(layoutParams);
                convertView.setBackgroundColor(Color.GRAY);

                TextView txtItem = new TextView(convertView.getContext());
                txtItem.setTextSize(TypedValue.COMPLEX_UNIT_PX, 30);
                txtItem.setTextColor(Color.WHITE);
                txtItem.setBackgroundResource(R.drawable.rectgray_layout);
                txtItem.setPadding(40, 0, 0, 0);
                txtItem.setGravity(Gravity.CENTER_VERTICAL);
                RelativeLayout.LayoutParams txtLayoutParams = new RelativeLayout.LayoutParams(RelativeLayout.LayoutParams.FILL_PARENT, RelativeLayout.LayoutParams.FILL_PARENT);
                txtItem.setLayoutParams(txtLayoutParams);
                if (bumenInfo != null)
                    txtItem.setText(bumenInfo.name);
                else if (cheduiInfo != null)
                    txtItem.setText(cheduiInfo.name);
                else if (jibieInfo != null)
                    txtItem.setText(jibieInfo.name);
                else if (renyuanInfo != null)
                    txtItem.setText(renyuanInfo.name);

                if (bumenInfo != null)
                {
                    txtItem.setTag(position);
                    txtItem.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            Integer nID = (Integer)v.getTag();
                            if (nID != null)
                            {
                                ShowJiDuan(nID.intValue());
                            }
                        }
                    });
                }
                else if (cheduiInfo != null)
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
                else if (jibieInfo != null)
                {
                    txtItem.setTag(position);
                    txtItem.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            Integer nID = (Integer)v.getTag();
                            if (nID != null)
                            {
                                if (nID.intValue() == 0)
                                {
                                    lblCheDui.setText("");
                                    lblCheDui.setEnabled(false);
                                    imgCheDui.setEnabled(false);
                                    lblJiDuan.setEnabled(true);
                                    imgJiDuan.setEnabled(true);
                                }
                                else if (nID.intValue() == 1)
                                {
                                    lblJiDuan.setText("");
                                    lblJiDuan.setEnabled(false);
                                    imgJiDuan.setEnabled(false);
                                    lblCheDui.setEnabled(true);
                                    imgCheDui.setEnabled(true);
                                }

                                ShowJiBie(nID.intValue());
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
                if (bumenInfo != null)
                {
                    viewHolder.txtItem.setText(bumenInfo.name);
                    viewHolder.txtItem.setTag(position);
                    viewHolder.txtItem.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            Integer nUid = (Integer)v.getTag();
                            if (nUid != null)
                                ShowJiDuan(nUid.intValue());
                        }
                    });
                }
                else if (cheduiInfo != null)
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
                else if (jibieInfo != null)
                {
                    viewHolder.txtItem.setText(jibieInfo.name);
                    viewHolder.txtItem.setTag(position);
                    viewHolder.txtItem.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            Integer nID = (Integer)v.getTag();
                            if (nID != null)
                            {
                                if (nID.intValue() == 0)
                                {
                                    lblCheDui.setText("");
                                    lblCheDui.setEnabled(false);
                                    imgCheDui.setEnabled(false);
                                    lblJiDuan.setEnabled(true);
                                    imgJiDuan.setEnabled(true);
                                }
                                else if (nID.intValue() == 1)
                                {
                                    lblJiDuan.setText("");
                                    lblJiDuan.setEnabled(false);
                                    imgJiDuan.setEnabled(false);
                                    lblCheDui.setEnabled(true);
                                    imgCheDui.setEnabled(true);
                                }

                                ShowJiBie(nID.intValue());
                            }
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

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent intent)
    {
        super.onActivityResult(requestCode, resultCode, intent);

        if (resultCode == RESULT_OK) {
            if (requestCode == FILE_RESULT) {
                Uri u = intent.getData();
                if(u == null) return;
                szSelFilePath = u.getEncodedPath();
                File fileSelected = new File(szSelFilePath);
                szSelFileName = fileSelected.getName();
            }
        }
    }
}
