package com.damytech.TieLu;

import android.app.Activity;
import android.app.DatePickerDialog;
import android.app.ProgressDialog;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
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
import java.io.*;
import java.util.ArrayList;
import java.util.Calendar;

public class KaoHeLuRuActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    private int nDateMode = 0; // 0 : JianCeRiQi       1 : ChuShengRiQi
    private int nJianCeYear = 0, nJianCeMonth = 0, nJianCeDay = 0;
    private int nChuChengYear = 0, nChuChengMonth = 0, nChuChengDay = 0;
    private Calendar curdate = Calendar.getInstance();
    private final int CHEDUI_ADAPTER = 0;
    private final int BANZU_ADAPTER = 1;
    private final int ZERENREN_ADAPTER = 2;
    private final int LIECHEZHANG_ADAPTER = 3;
    private int curAdapterId = CHEDUI_ADAPTER;

    private ImageView imgBack = null;
    private RelativeLayout rlKaoHeChaXun = null;
    private RelativeLayout rlKaoHeJieGuo = null;
    private TextView lblJianCeRiQi = null;
    private TextView lblChuChengRiQi = null;
    private ImageView imgCheDui = null;
    private ImageView imgBanZu = null;
    private ImageView imgZeRenRen = null;
    private ImageView imgLieCheZhang = null;
    private TextView lblCheDui = null;
    private TextView lblBanZu = null;
    private TextView lblZeRenRen = null;
    private TextView lblLieCheZhang = null;
    private ImageView imgXiangDianDetail = null;
    private TextView lblXiangDianItem = null;
    private ImageView imgPhoto = null;
    private EditText txtData = null;
    private Button btnYuLan = null;
    private TextView lblPhoto = null;

    private Bitmap bmpPhoto = null;
    private String strXiangDianInfo = "";
    private int nXiangDianChkInfo = 0;
    private String strXiangDianRelInfo = "";

    long curCheDuiID = 0;
    long curBanZuID = 0;
    long curZeRenRenID = 0;
    long curLieCheZhangID = 0;
    long curXiangDianID = 0;

    RelativeLayout listLayout = null;
    ListView adapterListView = null;
    ItemAdapter adapter = null;

    ArrayList<STCheDui> arrCheDui = new ArrayList<STCheDui>();
    ArrayList<STBanZu> arrBanZu = new ArrayList<STBanZu>();
    ArrayList<STZeRenRen> arrZeRenRen = new ArrayList<STZeRenRen>();
    ArrayList<STLieCheZhang> arrLieCheZhang = new ArrayList<STLieCheZhang>();
    private JsonHttpResponseHandler handlerCheDui = null;
    private JsonHttpResponseHandler handlerBanZu = null;
    private JsonHttpResponseHandler handlerZeRenRen = null;
    private JsonHttpResponseHandler handlerLieCheZhang = null;
    private ProgressDialog dialog = null;

    DatePickerDialog.OnDateSetListener dateListener =
            new DatePickerDialog.OnDateSetListener() {
                @Override
                public void onDateSet(DatePicker datePicker, int year, int month, int dayOfMonth) {
                    if (nDateMode == 0)
                    {
                        nJianCeYear = year;
                        nJianCeMonth = month;
                        nJianCeDay = dayOfMonth;
                        lblJianCeRiQi.setText(GlobalData.getDateFormat(nJianCeYear, nJianCeMonth, nJianCeDay));
                    }
                    else
                    {
                        nChuChengYear = year;
                        nChuChengMonth = month;
                        nChuChengDay = dayOfMonth;
                        lblChuChengRiQi.setText(GlobalData.getDateFormat(nChuChengYear, nChuChengMonth, nChuChengDay));

                        if (dialog.isShowing() == false) {
                            dialog = ProgressDialog.show(
                                    KaoHeLuRuActivity.this,
                                    "",
                                    getString(R.string.waiting),
                                    true,
                                    false,
                                    null
                            );
                            CommMgr.commService.GetCheDuiList(handlerCheDui, GlobalData.getDateFormat(nChuChengYear, nChuChengMonth, nChuChengDay));
                        }
                    }
                }
            };

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgKaoHeLuRu_Back:
                    finish();
                    break;
                case R.id.rlKaoHeLuRu_KaoHeChaXun:
                    Intent intentKaoHeChaXun = new Intent(KaoHeLuRuActivity.this, KaoHeChaXunActivity.class);
                    startActivity(intentKaoHeChaXun);
                    finish();
                    break;
                case R.id.rlKaoHeLuRu_JieGuoFenXi:
                    Intent intentJieGuoFenXi = new Intent(KaoHeLuRuActivity.this, JieGuoFenXiActivity.class);
                    startActivity(intentJieGuoFenXi);
                    finish();
                    break;
                case R.id.lblKaoHeLuRu_JianCeRiQiVal:
                    nDateMode = 0;
                    new DatePickerDialog(KaoHeLuRuActivity.this, dateListener, nJianCeYear, nJianCeMonth, nJianCeDay).show();
                    break;
                case R.id.lblKaoHeLuRu_ChuChengRiQiVal:
                    nDateMode = 1;
                    new DatePickerDialog(KaoHeLuRuActivity.this, dateListener, nChuChengYear, nChuChengMonth, nChuChengDay).show();
                    break;
                case R.id.imgKaoHeLuRu_CheDuiVal:
                    curAdapterId = CHEDUI_ADAPTER;
                    adapter.notifyDataSetChanged();
                    listLayout.setVisibility(View.VISIBLE);
                    adapterListView.setSelectionAfterHeaderView();
                    break;
                case R.id.imgKaoHeLuRu_BanZuVal:
                    curAdapterId = BANZU_ADAPTER;
                    adapter.notifyDataSetChanged();
                    listLayout.setVisibility(View.VISIBLE);
                    adapterListView.setSelectionAfterHeaderView();
                    break;
                case R.id.imgKaoHeLuRu_ZeRenRenVal:
                    curAdapterId = ZERENREN_ADAPTER;
                    adapter.notifyDataSetChanged();
                    listLayout.setVisibility(View.VISIBLE);
                    adapterListView.setSelectionAfterHeaderView();
                    break;
                case R.id.imgKaoHeLuRu_LieCheZhang:
                    curAdapterId = LIECHEZHANG_ADAPTER;
                    adapter.notifyDataSetChanged();
                    listLayout.setVisibility(View.VISIBLE);
                    adapterListView.setSelectionAfterHeaderView();
                    break;
                case R.id.imgKaoHeLuRu_XiangDianDetail:
                    Intent intentKaoHeRuLuDetail = new Intent(KaoHeLuRuActivity.this, KaoHeLuRu_XiangDianActivity.class);
                    startActivityForResult(intentKaoHeRuLuDetail, 0);
                    break;
                case R.id.imgKaoHeLuRu_Photo:
                    Intent intent = new Intent(KaoHeLuRuActivity.this, SelectPhotoActivity.class);
                    startActivityForResult(intent, 1);
                    break;
                case R.id.btnKaoHeLuRu_YuLan:
                    if (curCheDuiID < 1)
                    {
                        GlobalData.showToast(KaoHeLuRuActivity.this, getString(R.string.xuanze_chedui));
                        return;
                    }
                    if (curBanZuID < 1)
                    {
                        GlobalData.showToast(KaoHeLuRuActivity.this, getString(R.string.xuanze_banzu));
                        return;
                    }
                    if (curZeRenRenID < 1)
                    {
                        GlobalData.showToast(KaoHeLuRuActivity.this, getString(R.string.xuanze_zerenren));
                        return;
                    }

                    String strXiangDian = lblXiangDianItem.getText().toString();
                    if (strXiangDian == null || strXiangDian.length() == 0)
                    {
                        GlobalData.showToast(KaoHeLuRuActivity.this, getString(R.string.xuanze_xiangdian));
                        return;
                    }

                    String strData = txtData.getText().toString();
                    if (strData == null || strData.length() == 0)
                    {
                        GlobalData.showToast(KaoHeLuRuActivity.this, getString(R.string.shuru_kaoheshuju));
                        return;
                    }

                    byte[] bmpBytes = null;
                    if (bmpPhoto != null)
                    {
                        ByteArrayOutputStream baos = new ByteArrayOutputStream();
                        bmpPhoto.compress(Bitmap.CompressFormat.JPEG, 100, baos);
                        bmpBytes = baos.toByteArray();
                    }

                    Intent intentYuLan = new Intent(KaoHeLuRuActivity.this, KaoHeYuLanActivity.class);
                    intentYuLan.putExtra("Title", strXiangDianInfo);
                    intentYuLan.putExtra("StartTime", GlobalData.getDateFormat(nChuChengYear, nChuChengMonth, nChuChengDay));
                    intentYuLan.putExtra("CheckTime", GlobalData.getDateFormat(nJianCeYear, nJianCeMonth, nJianCeDay));
                    intentYuLan.putExtra("CheDui", lblCheDui.getText().toString());
                    intentYuLan.putExtra("CheDuiVal", curCheDuiID);
                    intentYuLan.putExtra("BanZu", lblBanZu.getText().toString());
                    intentYuLan.putExtra("BanZuVal", curBanZuID);
                    intentYuLan.putExtra("ZeRenRen", lblZeRenRen.getText().toString());
                    intentYuLan.putExtra("ZeRenRenVal", curZeRenRenID);
                    intentYuLan.putExtra("XiangDianVal", curXiangDianID);
                    intentYuLan.putExtra("ChkPoint", nXiangDianChkInfo);
                    intentYuLan.putExtra("LieCheZhang", lblLieCheZhang.getText().toString());
                    intentYuLan.putExtra("LieCheZhangVal", curLieCheZhangID);
                    intentYuLan.putExtra("RelPoint", strXiangDianRelInfo);
                    intentYuLan.putExtra("Data", strData);
                    intentYuLan.putExtra("Photo", bmpBytes);
                    startActivity(intentYuLan);

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
        setContentView(R.layout.kaoheluru_activity);

        mainLayout = (RelativeLayout)findViewById(R.id.rlKaoHeLuRuBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlKaoHeLuRuBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        initControl();
        initHandler();

        dialog = ProgressDialog.show(
                KaoHeLuRuActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetCheDuiList(handlerCheDui, GlobalData.getDateFormat(nChuChengYear, nChuChengMonth, nChuChengDay));
    }

    private void initControl()
    {
        nJianCeYear = curdate.get(Calendar.YEAR);
        nJianCeMonth = curdate.get(Calendar.MONTH);
        nJianCeDay = curdate.get(Calendar.DAY_OF_MONTH);
        nChuChengYear = curdate.get(Calendar.YEAR);
        nChuChengMonth = curdate.get(Calendar.MONTH);
        nChuChengDay = curdate.get(Calendar.DAY_OF_MONTH);

        imgBack = (ImageView) findViewById(R.id.imgKaoHeLuRu_Back);
        imgBack.setOnClickListener(onClickListener);

        rlKaoHeChaXun = (RelativeLayout) findViewById(R.id.rlKaoHeLuRu_KaoHeChaXun);
        rlKaoHeChaXun.setOnClickListener(onClickListener);

        rlKaoHeJieGuo = (RelativeLayout) findViewById(R.id.rlKaoHeLuRu_JieGuoFenXi);
        rlKaoHeJieGuo.setOnClickListener(onClickListener);

        lblJianCeRiQi = (TextView) findViewById(R.id.lblKaoHeLuRu_JianCeRiQiVal);
        lblJianCeRiQi.setText(GlobalData.getDateFormat(nJianCeYear, nJianCeMonth, nJianCeDay));
        lblJianCeRiQi.setOnClickListener(onClickListener);

        lblChuChengRiQi = (TextView) findViewById(R.id.lblKaoHeLuRu_ChuChengRiQiVal);
        lblChuChengRiQi.setText(GlobalData.getDateFormat(nJianCeYear, nJianCeMonth, nJianCeDay));
        lblChuChengRiQi.setOnClickListener(onClickListener);

        imgCheDui = (ImageView) findViewById(R.id.imgKaoHeLuRu_CheDuiVal);
        imgCheDui.setOnClickListener(onClickListener);

        imgBanZu = (ImageView) findViewById(R.id.imgKaoHeLuRu_BanZuVal);
        imgBanZu.setOnClickListener(onClickListener);

        imgZeRenRen = (ImageView) findViewById(R.id.imgKaoHeLuRu_ZeRenRenVal);
        imgZeRenRen.setOnClickListener(onClickListener);

        imgLieCheZhang = (ImageView) findViewById(R.id.imgKaoHeLuRu_LieCheZhang);
        imgLieCheZhang.setOnClickListener(onClickListener);

        lblCheDui = (TextView) findViewById(R.id.lblKaoHeLuRu_CheDuiVal);
        lblBanZu = (TextView) findViewById(R.id.lblKaoHeLuRu_BanZuVal);
        lblZeRenRen = (TextView) findViewById(R.id.lblKaoHeLuRu_ZeRenRenVal);
        lblLieCheZhang = (TextView) findViewById(R.id.lblKaoHeLuRu_LieCheZhangVal);

        imgXiangDianDetail = (ImageView) findViewById(R.id.imgKaoHeLuRu_XiangDianDetail);
        imgXiangDianDetail.setOnClickListener(onClickListener);

        lblXiangDianItem = (TextView) findViewById(R.id.lblKaoHeLuRu_XiangDian);

        imgPhoto = (ImageView) findViewById(R.id.imgKaoHeLuRu_Photo);
        imgPhoto.setOnClickListener(onClickListener);

        txtData = (EditText) findViewById(R.id.txtKaoHeLuRu_Data);

        btnYuLan = (Button) findViewById(R.id.btnKaoHeLuRu_YuLan);
        btnYuLan.setOnClickListener(onClickListener);

        lblPhoto = (TextView) findViewById(R.id.lblKaoHeLuRu_Photo);

        listLayout = (RelativeLayout)findViewById(R.id.list_layout);
        listLayout.setClickable(true);
        listLayout.setOnClickListener(new View.OnClickListener()
        {
            @Override
            public void onClick(View v)
            {
                listLayout.setVisibility(View.GONE);
            }
        });

        adapterListView = (ListView)findViewById(R.id.adapter_list);
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
                dialog.dismiss();

                CommMgr.commService.parseGetCheDuiList(jsonData, arrCheDui);
                if (arrCheDui == null)
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
                    GlobalData.showToast(KaoHeLuRuActivity.this, getString(R.string.network_error));
                    KaoHeLuRuActivity.this.finish();
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

                CommMgr.commService.parseGetBanZuList(jsonData, arrBanZu);
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
                    GlobalData.showToast(KaoHeLuRuActivity.this, getString(R.string.network_error));
                    KaoHeLuRuActivity.this.finish();
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

        handlerZeRenRen = new JsonHttpResponseHandler()
        {
            int result = 0;

            @Override
            public void onSuccess(JSONObject jsonData)
            {
                result = 1;
                dialog.dismiss();

                CommMgr.commService.parseGetZeRenRenList(jsonData, arrZeRenRen);
                if (arrZeRenRen == null)
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
                    GlobalData.showToast(KaoHeLuRuActivity.this, getString(R.string.network_error));
                    KaoHeLuRuActivity.this.finish();
                }
                else
                {
                    if (arrZeRenRen != null && arrZeRenRen.size() > 0)
                    {
                        ShowZeRenRen(0);
                    }
                }
            }
        };

        handlerLieCheZhang = new JsonHttpResponseHandler()
        {
            int result = 0;

            @Override
            public void onSuccess(JSONObject jsonData)
            {
                result = 1;
                dialog.dismiss();

                CommMgr.commService.parseGetLieCheZhangList(jsonData, arrLieCheZhang);
                if (arrLieCheZhang == null)
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
                    GlobalData.showToast(KaoHeLuRuActivity.this, getString(R.string.network_error));
                    KaoHeLuRuActivity.this.finish();
                }
                else
                {
                    if (arrLieCheZhang != null && arrLieCheZhang.size() > 0)
                    {
                        ShowLieCheZhang(0);
                    }
                }
            }
        };

        return;
    }

    @Override
    protected void onActivityResult(int nRequestCode, int nResultcode, Intent data)
    {
        super.onActivityResult(nRequestCode, nResultcode, data);

        if (nRequestCode == 0)
        {
            if (nResultcode == RESULT_OK)
            {
                lblXiangDianItem.setText(data.getStringExtra("XIANGDIANKEY"));
                strXiangDianInfo = data.getStringExtra("XIANGDIANINFO");
                nXiangDianChkInfo = data.getIntExtra("XIANGDIANCHKPOINT", 0);
                strXiangDianRelInfo = data.getStringExtra("XIANGDIANRELPOINT");
                curXiangDianID = data.getLongExtra("XIANGDIANID", 0);

                txtData.setText(strXiangDianInfo);
            }
        }

        if (nRequestCode == 1 && nResultcode == RESULT_OK)
        {
            updateUserImage(data);
        }
    }

    private void updateUserImage(Intent data)
    {
        if (data.getIntExtra(SelectPhotoActivity.szRetCode, -999) == SelectPhotoActivity.nRetSuccess)
        {
            Object objPath = data.getExtras().get(SelectPhotoActivity.szRetPath);
            Object objUri = data.getExtras().get(SelectPhotoActivity.szRetUri);

            String szPath = "";
            Uri fileUri = null;

            if (objPath != null)
                szPath = (String)objPath;

            if (objUri != null)
                fileUri = (Uri)objUri;

            lblPhoto.setVisibility(View.INVISIBLE);
            if (szPath != null && !szPath.equals(""))
                updateUserImageWithPath(szPath);
            else if (fileUri != null)
                updateUserImageWithUri(fileUri);
        }
    }

    private void updateUserImageWithPath(String szPath)
    {
        try {
            BitmapFactory.Options options = new BitmapFactory.Options();
            options.inPreferredConfig = Bitmap.Config.ARGB_8888;
            Bitmap bitmap = BitmapFactory.decodeFile(szPath, options);

            if (bitmap != null)
            {
                int nWidth = bitmap.getWidth(), nHeight = bitmap.getHeight();
                int nScaledWidth = 0, nScaledHeight = 0;
                if (nWidth > nHeight)
                {
                    nScaledWidth = SelectPhotoActivity.IMAGE_WIDTH;
                    nScaledHeight = nScaledWidth * nHeight / nWidth;
                }
                else
                {
                    nScaledHeight = SelectPhotoActivity.IMAGE_HEIGHT;
                    nScaledWidth = nScaledHeight * nWidth / nHeight;
                }

                bmpPhoto = Bitmap.createScaledBitmap(bitmap, nScaledWidth, nScaledHeight, false);
                imgPhoto.setImageBitmap(bmpPhoto);
            }
        } catch (Exception ex) {
            ex.printStackTrace();
        }
    }

    private void updateUserImageWithUri(Uri uri)
    {
        BufferedInputStream bis = null;
        InputStream is = null;
        Bitmap bmp = null;

        try {
            BitmapFactory.Options options = new BitmapFactory.Options();
            options.inPreferredConfig = Bitmap.Config.ARGB_8888;

            is = getContentResolver().openInputStream(uri);
            bmp = BitmapFactory.decodeStream(is, null, options);

            int nWidth = bmp.getWidth(), nHeight = bmp.getHeight();
            int nScaledWidth = 0, nScaledHeight = 0;
            if (nWidth > nHeight)
            {
                nScaledWidth = SelectPhotoActivity.IMAGE_WIDTH;
                nScaledHeight = nScaledWidth * nHeight / nWidth;
            }
            else
            {
                nScaledHeight = SelectPhotoActivity.IMAGE_HEIGHT;
                nScaledWidth = nScaledHeight * nWidth / nHeight;
            }

            bmpPhoto = Bitmap.createScaledBitmap(bmp, nScaledWidth, nScaledHeight, false);
            imgPhoto.setImageBitmap(bmpPhoto);
        }
        catch (Exception ex) {
            ex.printStackTrace();
        }
        finally {
            if (bis != null) {
                try {
                    bis.close();
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }

            if (is != null) {
                try {
                    is.close();
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
        }
    }

    private void ShowCheDui(int nNo)
    {
        curCheDuiID = arrCheDui.get(nNo).uid;
        lblCheDui.setText(arrCheDui.get(nNo).name);
        lblBanZu.setText("");
        lblZeRenRen.setText("");
        listLayout.setVisibility(View.GONE);

        dialog = ProgressDialog.show(
                KaoHeLuRuActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetBanZuList(handlerBanZu, Long.toString(curCheDuiID), GlobalData.getDateFormat(nChuChengYear, nChuChengMonth, nChuChengDay));

        return;
    }

    private void ShowBanZu(int nNo)
    {
        curBanZuID = arrBanZu.get(nNo).uid;
        lblBanZu.setText(arrBanZu.get(nNo).name);
        lblZeRenRen.setText("");
        listLayout.setVisibility(View.GONE);

        dialog = ProgressDialog.show(
                KaoHeLuRuActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetZeRenRenList(handlerZeRenRen,
                Long.toString(curBanZuID),
                GlobalData.getDateFormat(nChuChengYear, nChuChengMonth, nChuChengDay));
        CommMgr.commService.GetLieCheZhangList(handlerLieCheZhang,
                Long.toString(curBanZuID),
                GlobalData.getDateFormat(nChuChengYear, nChuChengMonth, nChuChengDay));

        return;
    }

    private void ShowZeRenRen(int nNo)
    {
        curZeRenRenID = arrZeRenRen.get(nNo).uid;
        lblZeRenRen.setText(arrZeRenRen.get(nNo).name);
        listLayout.setVisibility(View.GONE);

        return;
    }

    private void ShowLieCheZhang(int nNo)
    {
        curLieCheZhangID = arrLieCheZhang.get(nNo).uid;
        lblLieCheZhang.setText(arrLieCheZhang.get(nNo).name);
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
            else if (curAdapterId == ZERENREN_ADAPTER)
            {
                if (arrZeRenRen == null)
                    return 0;
                return arrZeRenRen.size();
            }
            else if (curAdapterId == LIECHEZHANG_ADAPTER)
            {
                if (arrLieCheZhang == null)
                    return 0;
                return arrLieCheZhang.size();
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
            else if (curAdapterId == ZERENREN_ADAPTER)
            {
                return arrZeRenRen.get(position);
            }
            else if (curAdapterId == LIECHEZHANG_ADAPTER)
            {
                return arrLieCheZhang.get(position);
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
            STZeRenRen zerenrenInfo = null;
            STLieCheZhang liechezhangInfo = null;

            if (curAdapterId == CHEDUI_ADAPTER)
            {
                cheduiInfo = arrCheDui.get(position);
            }
            else if (curAdapterId == BANZU_ADAPTER)
            {
                banzuInfo = arrBanZu.get(position);
            }
            else if (curAdapterId == ZERENREN_ADAPTER)
            {
                zerenrenInfo = arrZeRenRen.get(position);
            }
            else if (curAdapterId == LIECHEZHANG_ADAPTER)
            {
                liechezhangInfo = arrLieCheZhang.get(position);
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
                else if (zerenrenInfo != null)
                    txtItem.setText(zerenrenInfo.name);
                else if (liechezhangInfo != null)
                    txtItem.setText(liechezhangInfo.name);
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
                else if (zerenrenInfo != null)
                {
                    txtItem.setTag(position);
                    txtItem.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            Integer nID = (Integer)v.getTag();
                            if (nID != null)
                            {
                                ShowZeRenRen(nID.intValue());
                            }
                        }
                    });
                }
                else if (liechezhangInfo != null)
                {
                    txtItem.setTag(position);
                    txtItem.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            Integer nID = (Integer)v.getTag();
                            if (nID != null)
                            {
                                ShowLieCheZhang(nID.intValue());
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
                else if (zerenrenInfo != null)
                {
                    viewHolder.txtItem.setText(zerenrenInfo.name);
                    viewHolder.txtItem.setTag(position);
                    viewHolder.txtItem.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            Integer nUid = (Integer)v.getTag();
                            if (nUid != null)
                                ShowZeRenRen(nUid.intValue());
                        }
                    });
                }
                else if (liechezhangInfo != null)
                {
                    viewHolder.txtItem.setText(liechezhangInfo.name);
                    viewHolder.txtItem.setTag(position);
                    viewHolder.txtItem.setOnClickListener(new View.OnClickListener() {
                        @Override
                        public void onClick(View v) {
                            Integer nUid = (Integer)v.getTag();
                            if (nUid != null)
                                ShowLieCheZhang(nUid.intValue());
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
