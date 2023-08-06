package com.damytech.TieLu;

import android.app.Activity;
import android.app.ProgressDialog;
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
import com.damytech.utils.SmartImageView.Global;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.Calendar;

public class GongWenLiuZhuanActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    private int nLiuZhuanFlag = 0;
    Calendar calendar = Calendar.getInstance();

    private ImageView imgBack = null;
    private ImageView imgJiBie = null;
    private TextView lblJiBie = null;
    private ImageView imgBuMen = null;
    private TextView lblBuMen = null;
    private ImageView imgCheDui = null;
    private TextView lblCheDui = null;
    private ImageView imgRenYuan = null;
    private TextView lblRenYuan = null;
    private TextView lblShouXinRen = null;
    private Button btnAddRenYuan = null;
    private Button btnYuLan = null;
    private EditText txtNeiRong = null;

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
    private JsonHttpResponseHandler handlerUpload = null;
    private ProgressDialog dialog = null;

    private STDaiQianItem stInfo = new STDaiQianItem();

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgGongWenLiuZhuan_Back:
                    finish();
                    break;
                case R.id.imgGongWenLiuZhuan_JiBie:
                    curAdapterId = JIBIE_ADAPTER;
                    adapter.notifyDataSetChanged();
                    listLayout.setVisibility(View.VISIBLE);
                    adapterListView.setSelectionAfterHeaderView();
                    break;
                case R.id.imgGongWenLiuZhuan_BuMen:
                    strSXRIDList = "";
                    strSXRList = "";
                    arrUploadRenYuan.clear();
                    lblShouXinRen.setText(strSXRList);
                    curAdapterId = BUMEN_ADAPTER;
                    adapter.notifyDataSetChanged();
                    listLayout.setVisibility(View.VISIBLE);
                    adapterListView.setSelectionAfterHeaderView();
                    break;
                case R.id.imgGongWenLiuZhuan_CheDui:
                    strSXRIDList = "";
                    strSXRList = "";
                    arrUploadRenYuan.clear();
                    lblShouXinRen.setText(strSXRList);
                    curAdapterId = CHEDUI_ADAPTER;
                    adapter.notifyDataSetChanged();
                    listLayout.setVisibility(View.VISIBLE);
                    adapterListView.setSelectionAfterHeaderView();
                    break;
                case R.id.imgGongWenLiuZhuan_RenYuan:
                    curAdapterId = RENYUAN_ADAPTER;
                    adapter.notifyDataSetChanged();
                    listLayout.setVisibility(View.VISIBLE);
                    adapterListView.setSelectionAfterHeaderView();
                    break;
                case R.id.btnGongWenLiuZhuan_Add:
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
                            strSXRIDList = strSXRIDList + ", " + Long.toString(curRenYuanID);

                            STRenYuan renYuan = new STRenYuan();
                            renYuan.uid = curRenYuanID;
                            renYuan.name = strName;

                            arrUploadRenYuan.add(renYuan);
                        }
                    }
                    lblShouXinRen.setText(strSXRList);

                    break;
                case R.id.btnGongWenLiuZhuan_Browse:
                    if (arrUploadRenYuan.size() == 0)
                    {
                        GlobalData.showToast(GongWenLiuZhuanActivity.this, getString(R.string.xuanze_shouxinren));
                        return;
                    }
                    String strNeiRong = txtNeiRong.getText().toString();
                    if (strNeiRong.length() == 0)
                    {
                        GlobalData.showToast(GongWenLiuZhuanActivity.this, getString(R.string.shuru_fabuneirong));
                        return;
                    }

                    dialog = ProgressDialog.show(
                            GongWenLiuZhuanActivity.this,
                            "",
                            getString(R.string.waiting),
                            true,
                            false,
                            null
                    );
                    CommMgr.commService.SetLiuZhuanData(handlerUpload, Long.toString(GlobalData.GetUid(GongWenLiuZhuanActivity.this)), Long.toString(stInfo.uid), strNeiRong, strSXRIDList);

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
        setContentView(R.layout.gongwenliuzhuan_activity);

        mainLayout = (RelativeLayout)findViewById(R.id.rlGongWenLiuZhuanBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlGongWenLiuZhuanBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        stInfo = (STDaiQianItem) getIntent().getParcelableExtra("Data");
        nLiuZhuanFlag = getIntent().getIntExtra("LiuZhuanFlag", 0);

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

        TextView oldPiShi = (TextView) findViewById(R.id.txtGongWenLiuZhuan_OldPiShiVal);
        String strData = "";
        if (stInfo.liuzhuanrenpishi != null && stInfo.liuzhuanrenpishi.length > 0) {
            int nLen = stInfo.liuzhuanrenpishi.length;

            for (int i = 0; i < nLen; i++) {
                strData = strData + stInfo.liuzhuanrenpishi[i] + "\n";
            }
        }
        oldPiShi.setText(strData);

        imgBack = (ImageView) findViewById(R.id.imgGongWenLiuZhuan_Back);
        imgBack.setOnClickListener(onClickListener);

        lblJiBie = (TextView) findViewById(R.id.lblGongWenLiuZhuan_JiBieVal);
        lblBuMen = (TextView) findViewById(R.id.lblGongWenLiuZhuan_BuMenVal);
        lblCheDui = (TextView) findViewById(R.id.lblGongWenLiuZhuan_CheDuiVal);
        lblRenYuan = (TextView) findViewById(R.id.lblGongWenLiuZhuan_RenYuanVal);
        lblShouXinRen = (TextView) findViewById(R.id.lblGongWenLiuZhuan_LiuZhuanRenVal);

        imgJiBie = (ImageView) findViewById(R.id.imgGongWenLiuZhuan_JiBie);
        imgJiBie.setOnClickListener(onClickListener);

        imgBuMen = (ImageView) findViewById(R.id.imgGongWenLiuZhuan_BuMen);
        imgBuMen.setOnClickListener(onClickListener);

        imgCheDui = (ImageView) findViewById(R.id.imgGongWenLiuZhuan_CheDui);
        imgCheDui.setOnClickListener(onClickListener);

        imgRenYuan = (ImageView) findViewById(R.id.imgGongWenLiuZhuan_RenYuan);
        imgRenYuan.setOnClickListener(onClickListener);

        txtNeiRong = (EditText) findViewById(R.id.txtGongWenLiuZhuan_PiShiVal);

        btnAddRenYuan = (Button) findViewById(R.id.btnGongWenLiuZhuan_Add);
        btnAddRenYuan.setOnClickListener(onClickListener);

        btnYuLan = (Button) findViewById(R.id.btnGongWenLiuZhuan_Browse);
        btnYuLan.setOnClickListener(onClickListener);

        listLayout = (RelativeLayout)findViewById(R.id.rlGongWenLiuZhuan_List);
        listLayout.setClickable(true);
        listLayout.setOnClickListener(new View.OnClickListener()
        {
            @Override
            public void onClick(View v)
            {
                listLayout.setVisibility(View.GONE);
            }
        });

        adapterListView = (ListView)findViewById(R.id.listGongWenLiuZhuan_List);
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
                    GlobalData.showToast(GongWenLiuZhuanActivity.this, getString(R.string.network_error));
                    GongWenLiuZhuanActivity.this.finish();
                }
                else
                {
                    if (arrJiDuan != null && arrJiDuan.size() > 0)
                    {
                        ShowBuMen(0);
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
                    GlobalData.showToast(GongWenLiuZhuanActivity.this, getString(R.string.network_error));
                    GongWenLiuZhuanActivity.this.finish();
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
                    GlobalData.showToast(GongWenLiuZhuanActivity.this, getString(R.string.network_error));
                    GongWenLiuZhuanActivity.this.finish();
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

        handlerUpload = new JsonHttpResponseHandler()
        {
            int result = 0;
            long nRet = 0;

            @Override
            public void onSuccess(JSONObject jsonData)
            {
                result = 1;

                dialog.dismiss();
                nRet = CommMgr.commService.parseSetLiuZhuanData(jsonData);
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
                    GlobalData.showToast(GongWenLiuZhuanActivity.this, getString(R.string.network_error));
                }
                if (nRet == STServiceData.ERR_SUCCESS)
                {
                    GlobalData.showToast(GongWenLiuZhuanActivity.this, getString(R.string.caozuochenggong));
                    if (nLiuZhuanFlag == 0)
                    {
                        Intent intent = new Intent(GongWenLiuZhuanActivity.this, GongWenDaiQianActivity.class);
                        intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_NEW_TASK);
                        startActivity(intent);
                    }
                    else
                    {
                        Intent intent = new Intent(GongWenLiuZhuanActivity.this, GongWenYiShouActivity.class);
                        intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_NEW_TASK);
                        startActivity(intent);
                    }
                    GongWenLiuZhuanActivity.this.finish();
                }
                else if(nRet == STServiceData.ERR_EXIST_DATA) {
                    GlobalData.showToast(GongWenLiuZhuanActivity.this, "此公文已流转过，不能再流转！");
                }
                else
                {
                    GlobalData.showToast(GongWenLiuZhuanActivity.this, getString(R.string.service_error));
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
                GongWenLiuZhuanActivity.this,
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

    private void ShowBuMen(int nNo)
    {
        curJiDuanID = arrJiDuan.get(nNo).uid;
        lblBuMen.setText(arrJiDuan.get(nNo).name);
        lblRenYuan.setText("");
        listLayout.setVisibility(View.GONE);

        RenYuanMode = 0;

        dialog = ProgressDialog.show(
                GongWenLiuZhuanActivity.this,
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
                GongWenLiuZhuanActivity.this,
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
                                ShowBuMen(nID.intValue());
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
                                    lblBuMen.setEnabled(true);
                                    imgBuMen.setEnabled(true);
                                }
                                else if (nID.intValue() == 1)
                                {
                                    lblBuMen.setText("");
                                    lblBuMen.setEnabled(false);
                                    imgBuMen.setEnabled(false);
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
                                ShowBuMen(nUid.intValue());
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
                                    lblBuMen.setEnabled(true);
                                    imgBuMen.setEnabled(true);
                                }
                                else if (nID.intValue() == 1)
                                {
                                    lblBuMen.setText("");
                                    lblBuMen.setEnabled(false);
                                    imgBuMen.setEnabled(false);
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
}
