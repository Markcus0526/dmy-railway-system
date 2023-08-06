package com.damytech.TieLu;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Intent;
import android.graphics.Rect;
import android.os.Bundle;
import android.view.View;
import android.view.ViewTreeObserver;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.RelativeLayout;
import android.widget.TextView;
import com.damytech.HttpConn.JsonHttpResponseHandler;
import com.damytech.STData.STRenWuItem;
import com.damytech.STData.STRenWuJieDaoItem;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;
import org.json.JSONObject;

import java.util.ArrayList;

public class RenWuJieDaoActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    private ImageView imgBack = null;
    private RelativeLayout rlFaBu = null;
    private Button btnShangYiYe;
    private Button btnXiaYiYe;

    int nPageNo = 0;
    ProgressDialog dialog = null;
    ArrayList<STRenWuJieDaoItem> arrDatas = new ArrayList<STRenWuJieDaoItem>();
    JsonHttpResponseHandler handler = new JsonHttpResponseHandler()
    {
        int result = 0;

        @Override
        public void onSuccess(JSONObject jsonData)
        {
            result = 1;
            dialog.dismiss();

            arrDatas = new ArrayList<STRenWuJieDaoItem>();
            CommMgr.commService.parseGetMyTaskList(jsonData, arrDatas);
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
                GlobalData.showToast(RenWuJieDaoActivity.this, getString(R.string.network_error));
                RenWuJieDaoActivity.this.finish();
            }
            else
            {
                RefreshPage();
            }
        }
    };

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgRenWuJieDao_Back:
                    finish();
                    break;
                case R.id.rlRenWuJieDao_FaBu:
                    Intent intentFaBu = new Intent(RenWuJieDaoActivity.this, RenWuGuanLiActivity.class);
                    startActivity(intentFaBu);
                    finish();
                    break;
                case R.id.btnRenWuJieDao_ShangYiYe:
                    if (nPageNo == 0)
                        return;
                    else
                    {
                        nPageNo--;
                        dialog = ProgressDialog.show(
                                RenWuJieDaoActivity.this,
                                "",
                                getString(R.string.waiting),
                                true,
                                false,
                                null
                        );
                        CommMgr.commService.GetMyTaskList(handler, Long.toString(GlobalData.GetUid(RenWuJieDaoActivity.this)), Integer.toString(nPageNo));
                    }
                    break;
                case R.id.btnRenWuJieDao_XiaYiYe:
                    if (arrDatas.size() == 3)
                    {
                        nPageNo++;
                    }
                    dialog = ProgressDialog.show(
                            RenWuJieDaoActivity.this,
                            "",
                            getString(R.string.waiting),
                            true,
                            false,
                            null
                    );
                    CommMgr.commService.GetMyTaskList(handler, Long.toString(GlobalData.GetUid(RenWuJieDaoActivity.this)), Integer.toString(nPageNo));
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
        setContentView(R.layout.renwujiedao_activity);

        mainLayout = (RelativeLayout)findViewById(R.id.rlRenWuJieDaoBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlRenWuJieDaoBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        initControl();
    }

    @Override
    public void onResume()
    {
        super.onResume();

        dialog = ProgressDialog.show(
                RenWuJieDaoActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetMyTaskList(handler, Long.toString(GlobalData.GetUid(RenWuJieDaoActivity.this)), Integer.toString(nPageNo));
    }

    private void initControl()
    {
        imgBack = (ImageView) findViewById(R.id.imgRenWuJieDao_Back);
        imgBack.setOnClickListener(onClickListener);

        rlFaBu = (RelativeLayout) findViewById(R.id.rlRenWuJieDao_FaBu);
        rlFaBu.setOnClickListener(onClickListener);

        btnShangYiYe = (Button) findViewById(R.id.btnRenWuJieDao_ShangYiYe);
        btnShangYiYe.setOnClickListener(onClickListener);
        btnXiaYiYe = (Button) findViewById(R.id.btnRenWuJieDao_XiaYiYe);
        btnXiaYiYe.setOnClickListener(onClickListener);
    }

    private void RefreshPage()
    {
        if (arrDatas != null)
        {
            RelativeLayout relativeLayout1 = (RelativeLayout) findViewById(R.id.rlLayout1);
            relativeLayout1.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    long nUID = ((Long)v.getTag()).longValue();
                    Intent intent = new Intent(RenWuJieDaoActivity.this, RenWuXiangQingActivity.class);
                    intent.putExtra("uid", nUID);
                    startActivity(intent);
                }
            });
            try {
                if (arrDatas.get(0) != null)
                {
                    TextView lblStartTime = (TextView) findViewById(R.id.lblKaiShiShiJian1);
                    lblStartTime.setText(getString(R.string.qishiriqi) + " ：" + arrDatas.get(0).starttime);
                    TextView lblEndTime = (TextView) findViewById(R.id.lblJieShuShiJian1);
                    lblEndTime.setText(getString(R.string.jieshuriqi) + " ：" + arrDatas.get(0).endtime);
                    TextView lblTitle = (TextView) findViewById(R.id.lblTitle1);
                    lblTitle.setText(getString(R.string.renwumingcheng) + arrDatas.get(0).title);
                    TextView lblState = (TextView) findViewById(R.id.lblStatus1);
                    if (arrDatas.get(0).status == 0)
                        lblState.setTextColor(getResources().getColor(R.color.gray));
                    else if (arrDatas.get(0).status == 1)
                        lblState.setTextColor(getResources().getColor(R.color.lightblue));
                    else if (arrDatas.get(0).status == 2)
                        lblState.setTextColor(getResources().getColor(R.color.darkgreen));
                    else
                        lblState.setTextColor(getResources().getColor(R.color.gray));
                    lblState.setText(getString(R.string.shifouwancheng) + arrDatas.get(0).state);
                    TextView lblFile = (TextView) findViewById(R.id.lblFile1);
                    lblFile.setText(arrDatas.get(0).existfile);
                    relativeLayout1.setVisibility(View.VISIBLE);
                    relativeLayout1.setTag(arrDatas.get(0).uid);
                }
                else
                {
                    relativeLayout1.setVisibility(View.INVISIBLE);
                }
            } catch (Exception ex)
            {
                relativeLayout1.setVisibility(View.INVISIBLE);
            }

            RelativeLayout relativeLayout2 = (RelativeLayout) findViewById(R.id.rlLayout2);
            relativeLayout2.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    long nUID = ((Long)v.getTag()).longValue();
                    Intent intent = new Intent(RenWuJieDaoActivity.this, RenWuXiangQingActivity.class);
                    intent.putExtra("uid", nUID);
                    startActivity(intent);
                }
            });
            try
            {
                if (arrDatas.get(1) != null)
                {
                    TextView lblStartTime = (TextView) findViewById(R.id.lblKaiShiShiJian2);
                    lblStartTime.setText(getString(R.string.qishiriqi) + " ：" + arrDatas.get(1).starttime);
                    TextView lblEndTime = (TextView) findViewById(R.id.lblJieShuShiJian2);
                    lblEndTime.setText(getString(R.string.jieshuriqi) + " ：" + arrDatas.get(1).endtime);
                    TextView lblTitle = (TextView) findViewById(R.id.lblTitle2);
                    lblTitle.setText(getString(R.string.renwumingcheng) + arrDatas.get(1).title);
                    TextView lblState = (TextView) findViewById(R.id.lblStatus2);
                    if (arrDatas.get(1).status == 0)
                        lblState.setTextColor(getResources().getColor(R.color.gray));
                    else if (arrDatas.get(1).status == 1)
                        lblState.setTextColor(getResources().getColor(R.color.lightblue));
                    else if (arrDatas.get(1).status == 2)
                        lblState.setTextColor(getResources().getColor(R.color.darkgreen));
                    else
                        lblState.setTextColor(getResources().getColor(R.color.gray));
                    lblState.setText(getString(R.string.shifouwancheng) + arrDatas.get(1).state);
                    TextView lblFile = (TextView) findViewById(R.id.lblFile2);
                    lblFile.setText(arrDatas.get(1).existfile);
                    relativeLayout2.setVisibility(View.VISIBLE);
                    relativeLayout2.setTag(arrDatas.get(1).uid);
                }
                else
                {
                    relativeLayout2.setVisibility(View.INVISIBLE);
                }
            } catch (Exception ex)
            {
                relativeLayout2.setVisibility(View.INVISIBLE);
            }

            RelativeLayout relativeLayout3 = (RelativeLayout) findViewById(R.id.rlLayout3);
            relativeLayout3.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    long nUID = ((Long)v.getTag()).longValue();
                    Intent intent = new Intent(RenWuJieDaoActivity.this, RenWuXiangQingActivity.class);
                    intent.putExtra("uid", nUID);
                    startActivity(intent);
                }
            });
            try
            {
                if (arrDatas.get(2) != null)
                {
                    TextView lblStartTime = (TextView) findViewById(R.id.lblKaiShiShiJian3);
                    lblStartTime.setText(getString(R.string.qishiriqi) + " ：" + arrDatas.get(2).starttime);
                    TextView lblEndTime = (TextView) findViewById(R.id.lblJieShuShiJian3);
                    lblEndTime.setText(getString(R.string.jieshuriqi) + " ：" + arrDatas.get(2).endtime);
                    TextView lblTitle = (TextView) findViewById(R.id.lblTitle3);
                    lblTitle.setText(getString(R.string.renwumingcheng) + arrDatas.get(2).title);
                    TextView lblState = (TextView) findViewById(R.id.lblStatus3);
                    if (arrDatas.get(2).status == 0)
                        lblState.setTextColor(getResources().getColor(R.color.gray));
                    else if (arrDatas.get(2).status == 1)
                        lblState.setTextColor(getResources().getColor(R.color.lightblue));
                    else if (arrDatas.get(2).status == 2)
                        lblState.setTextColor(getResources().getColor(R.color.darkgreen));
                    else
                        lblState.setTextColor(getResources().getColor(R.color.gray));
                    lblState.setText(getString(R.string.shifouwancheng) + arrDatas.get(2).state);
                    TextView lblFile = (TextView) findViewById(R.id.lblFile3);
                    lblFile.setText(arrDatas.get(2).existfile);
                    relativeLayout3.setVisibility(View.VISIBLE);
                    relativeLayout3.setTag(arrDatas.get(2).uid);
                }
                else
                {
                    relativeLayout3.setVisibility(View.INVISIBLE);
                }
            } catch (Exception ex)
            {
                relativeLayout3.setVisibility(View.INVISIBLE);
            }
        }
    }
}
