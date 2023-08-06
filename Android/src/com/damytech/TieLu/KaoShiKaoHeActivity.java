package com.damytech.TieLu;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Intent;
import android.graphics.Rect;
import android.os.Bundle;
import android.view.KeyEvent;
import android.view.View;
import android.view.ViewTreeObserver;
import android.widget.*;
import com.damytech.HttpConn.JsonHttpResponseHandler;
import com.damytech.STData.STKaoShi;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.Timer;

public class KaoShiKaoHeActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    private int nScore = 0;
    private long nProNo = 0;
    private int nExamAllTime = 0;
    private int nExamTime = 0;
    private String g_strProblem = "";
    private ArrayList<STKaoShi> arrData = new ArrayList<STKaoShi>();
    private ArrayList<String> g_arrAns = new ArrayList<String>();

    private ImageView imgBack = null;
    private Button btnChaKan = null;
    private Button btnReturn = null;
    private TextView lblKaoShiShiJian = null;
    private TextView lblTiJiaoShiJian = null;
    private TextView lblDianShu = null;
    private int nZongWenTi = 0;
    private TextView lblZongKaoTi = null;
    private int nZongHuiDa = 0;
    private TextView lblZongHuiDa = null;
    private int nDanXuanWenTi = 0;
    private TextView lblDanXuanWenTi = null;
    private int nDanXuanHuiDa = 0;
    private TextView lblDanXuanHuiDa = null;
    private int nDuoXuanWenTi = 0;
    private TextView lblDuoXuanWenTi = null;
    private int nDuoXuanHuiDa = 0;
    private TextView lblDuoXuanHuiDa = null;
    private int nPanDuanWenTi = 0;
    private TextView lblPanDuanWenTi = null;
    private int nPanDuanHuiDa = 0;
    private TextView lblPanDuanHuiDa = null;

    private JsonHttpResponseHandler handler = new JsonHttpResponseHandler()
    {
        int result = 0;

        @Override
        public void onSuccess(JSONObject jsonData)
        {
            result = 1;

            CommMgr.commService.parseSetKaoShiJieGuo(jsonData);
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
                GlobalData.showToast(KaoShiKaoHeActivity.this, getString(R.string.caozuochenggong));
            if (result == -1)
                GlobalData.showToast(KaoShiKaoHeActivity.this, getString(R.string.network_error));
        }
    };
    private ProgressDialog dialog = null;

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgKaoShiKaoHe_Back:
                    finish();
                    break;
                case R.id.btnKaoShiKaoHe_ChaKan:
                    Intent intentChaKan = new Intent(KaoShiKaoHeActivity.this, KaoShiFenXiActivity.class);
                    intentChaKan.putExtra("Problem", g_strProblem);
                    intentChaKan.putStringArrayListExtra("Answer", g_arrAns);
                    startActivity(intentChaKan);
                    break;
                case R.id.btnKaoShiKaoHe_Return:
                    Intent intent = new Intent(KaoShiKaoHeActivity.this, MainMenuActivity.class);
                    intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_NEW_TASK);
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
        setContentView(R.layout.kaoshikaohe_activity);

        mainLayout = (RelativeLayout)findViewById(R.id.rlKaoShiKaoHeBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlKaoShiKaoHeBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        nProNo = getIntent().getLongExtra("ProNo", 0);
        nExamAllTime = getIntent().getIntExtra("AllTime", 0);
        nExamTime = getIntent().getIntExtra("Time", -1);
        g_strProblem = getIntent().getStringExtra("Problem");
        g_arrAns = getIntent().getStringArrayListExtra("Answer");
        if (nExamTime == -1 || g_strProblem == null || g_strProblem.length() == 0 || g_arrAns == null)
        {
            GlobalData.showToast(KaoShiKaoHeActivity.this, getString(R.string.dataerror));
            Intent intent = new Intent(KaoShiKaoHeActivity.this, MainMenuActivity.class);
            intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_NEW_TASK);
            startActivity(intent);
        }

        try {
            JSONObject jsonObject = new JSONObject(g_strProblem);
            CommMgr.commService.parseGetKaoShiList(jsonObject, arrData);
        } catch (JSONException e) {}

        initControl();

        ShowData();
    }

    private void initControl()
    {
        imgBack = (ImageView) findViewById(R.id.imgKaoShiKaoHe_Back);
        imgBack.setOnClickListener(onClickListener);

        btnChaKan = (Button) findViewById(R.id.btnKaoShiKaoHe_ChaKan);
        btnChaKan.setOnClickListener(onClickListener);

        btnReturn = (Button) findViewById(R.id.btnKaoShiKaoHe_Return);
        btnReturn.setOnClickListener(onClickListener);

        lblKaoShiShiJian = (TextView) findViewById(R.id.lblKaoShiKaoHe_KaoShiShiJian);
        lblTiJiaoShiJian = (TextView) findViewById(R.id.lblKaoShiKaoHe_TiJiaoShiJian);
        lblZongKaoTi = (TextView) findViewById(R.id.lblKaoShiKaoHe_ZongKaoTi);
        lblZongHuiDa = (TextView) findViewById(R.id.lblKaoShiKaoHe_ZongDaDuiTi);
        lblDanXuanWenTi = (TextView) findViewById(R.id.lblKaoShiKaoHe_DanXuanTi);
        lblDanXuanHuiDa = (TextView) findViewById(R.id.lblKaoShiKaoHe_DanXuanTiVal);
        lblDuoXuanWenTi = (TextView) findViewById(R.id.lblKaoShiKaoHe_DuoXuanTi);
        lblDuoXuanHuiDa = (TextView) findViewById(R.id.lblKaoShiKaoHe_DuoXuanTiVal);
        lblPanDuanWenTi = (TextView) findViewById(R.id.lblKaoShiKaoHe_PanDuanTi);
        lblPanDuanHuiDa = (TextView) findViewById(R.id.lblKaoShiKaoHe_PanDuanTiVal);
        lblDianShu = (TextView) findViewById(R.id.lblKaoShiKaoHe_DianShu);
    }

    private void ShowData()
    {
        analyzeData();

        lblKaoShiShiJian.setText(getString(R.string.kaoshishijian) + secondsToTimeStr(nExamAllTime));
        lblTiJiaoShiJian.setText(getString(R.string.tijiaoshijian) + secondsToTimeStr(nExamTime));

        lblZongKaoTi.setText(getString(R.string.zongkaoti) + nZongWenTi);
        lblZongHuiDa.setText(getString(R.string.zongdaduiti) + nZongHuiDa);
        lblDanXuanWenTi.setText(getString(R.string.danxuanti) + nDanXuanWenTi);
        lblDanXuanHuiDa.setText(getString(R.string.daduiti) + nDanXuanHuiDa);
        lblDuoXuanWenTi.setText(getString(R.string.duoxuanti) + nDuoXuanWenTi);
        lblDuoXuanHuiDa.setText(getString(R.string.daduiti) + nDuoXuanHuiDa);
        lblPanDuanWenTi.setText(getString(R.string.panduanti) + nPanDuanWenTi);
        lblPanDuanHuiDa.setText(getString(R.string.daduiti) + nPanDuanHuiDa);

        String strDianShu = "0";
        if (nZongWenTi != 0)
        {
            try
            {
                strDianShu = String.format("%d", ((nDanXuanHuiDa + nDuoXuanHuiDa + nPanDuanHuiDa) * 100) / nZongWenTi);
                lblDianShu.setText(strDianShu);
            }catch (Exception ex)
            {
                lblDianShu.setText("0");
            }
        }
        else
            lblDianShu.setText("0");

        dialog = ProgressDialog.show(KaoShiKaoHeActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null);
        CommMgr.commService.SetKaoShiJieGuo(handler,
                                            Long.toString(GlobalData.GetUid(KaoShiKaoHeActivity.this)),
                                            Long.toString(nProNo),
                                            strDianShu,
                                            "",
                                            Integer.toString(nExamTime));

        return;
    }

    private String secondsToTimeStr(int i_nSeconds)
    {
        int hour=0, min=0, sec=i_nSeconds;
        String szRet;

        if ( sec >= 60 ) {
            min = sec/60;
            sec = sec - min*60;
        }

        if ( min >= 60 ) {
            hour = min/60;
            min = min - hour*60;
        }

        if ( hour == 0 ) {
            szRet = String.format("%02d:%02d", min, sec);
        } else {
            szRet = String.format("%02d:%02d:%02d", hour, min, sec);
        }

        return szRet;
    }

    private void analyzeData()
    {
        for (int i = 0; i < arrData.size(); i++)
        {
            nZongWenTi++;
            try
            {
                if (g_arrAns.get(i) != null)
                {
                    nZongHuiDa++;
                    if (arrData.get(i).getType() == STKaoShi.KAOSHI_TYPE_DANXUAN)
                    {
                        nDanXuanWenTi++;
                        if (arrData.get(i).answers.get(0).equals(g_arrAns.get(i)))
                        {
                            nDanXuanHuiDa++;
                        }
                    }
                    if (arrData.get(i).getType() == STKaoShi.KAOSHI_TYPE_DUOXUAN)
                    {
                        nDuoXuanWenTi++;
                        String strAns = "";
                        for (int j = 0; j < arrData.get(i).answers.size(); j++)
                        {
                            if (j == 0)
                                strAns = arrData.get(i).answers.get(j).toString();
                            else
                                strAns = strAns + "," + arrData.get(i).answers.get(j).toString();
                        }
                        if (strAns.equals(g_arrAns.get(i)))
                            nDuoXuanHuiDa++;
                    }
                    if (arrData.get(i).getType() == STKaoShi.KAOSHI_TYPE_PANDUAN)
                    {
                        nPanDuanWenTi++;
                        if (arrData.get(i).answers.get(0).equals(g_arrAns.get(i)))
                        {
                            nPanDuanHuiDa++;
                        }
                    }
                }
            }
            catch (IndexOutOfBoundsException ex)
            {
                if (arrData.get(i).getType() == STKaoShi.KAOSHI_TYPE_DANXUAN)
                    nDanXuanWenTi++;
                if (arrData.get(i).getType() == STKaoShi.KAOSHI_TYPE_DUOXUAN)
                    nDuoXuanWenTi++;
                if (arrData.get(i).getType() == STKaoShi.KAOSHI_TYPE_PANDUAN)
                    nPanDuanWenTi++;
            }
            catch (Exception ex) {}
        }

        return;
    }

    @Override
    public boolean onKeyDown(int keyCode, KeyEvent event)
    {
        if (keyCode == KeyEvent.KEYCODE_BACK)
        {
            Intent intent = new Intent(KaoShiKaoHeActivity.this, MainMenuActivity.class);
            intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_NEW_TASK);
            startActivity(intent);
            KaoShiKaoHeActivity.this.finish();
        }

        return super.onKeyDown(keyCode, event);
    }
}
