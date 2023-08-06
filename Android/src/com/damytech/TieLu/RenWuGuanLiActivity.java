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
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;
import com.damytech.utils.SmartImageView.Global;
import org.json.JSONObject;

import java.util.ArrayList;

public class RenWuGuanLiActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    private ImageView imgBack;
    private Button btnAdd;
    private Button btnShangYiYe;
    private Button btnXiaYiYe;
    private RelativeLayout rlJieDao = null;

    int nPageNo = 0;
    private ProgressDialog dialog = null;
    ArrayList<STRenWuItem> arrDatas = new ArrayList<STRenWuItem>();
    JsonHttpResponseHandler handler = new JsonHttpResponseHandler()
    {
        int result = 0;

        @Override
        public void onSuccess(JSONObject jsonData)
        {
            result = 1;
            dialog.dismiss();

            arrDatas = new ArrayList<STRenWuItem>();
            CommMgr.commService.parseGetMySendTaskList(jsonData, arrDatas);
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
                GlobalData.showToast(RenWuGuanLiActivity.this, getString(R.string.network_error));
                RenWuGuanLiActivity.this.finish();
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
                case R.id.imgRenWuGuanLi_Back:
                    finish();
                    break;
                case R.id.rlRenWuGuanLi_JieDao:
                    Intent intentJieDao = new Intent(RenWuGuanLiActivity.this, RenWuJieDaoActivity.class);
                    startActivity(intentJieDao);
                    finish();
                    break;
                case R.id.btnRenWuGuanLi_Add:
                    if (GlobalData.GetGrade( RenWuGuanLiActivity.this ) == -1 || GlobalData.GetGrade(RenWuGuanLiActivity.this) == 2)
                    {
                        GlobalData.showToast(RenWuGuanLiActivity.this, getString(R.string.noauthoriry));
                        return;
                    }
                    Intent intentAdd = new Intent(RenWuGuanLiActivity.this, RenWuGuanLiAddActivity.class);
                    startActivity(intentAdd);
                    break;
                case R.id.btnRenWuGuanLi_ShangYiYe:
                    if (nPageNo == 0)
                        return;
                    else
                    {
                        nPageNo--;
                        dialog = ProgressDialog.show(
                                RenWuGuanLiActivity.this,
                                "",
                                getString(R.string.waiting),
                                true,
                                false,
                                null
                        );
                        CommMgr.commService.GetMySendTaskList(handler, Long.toString(GlobalData.GetUid(RenWuGuanLiActivity.this)), Integer.toString(nPageNo));
                    }
                    break;
                case R.id.btnRenWuGuanLi_XiaYiYe:
                    if (arrDatas.size() == 5)
                    {
                        nPageNo++;
                    }
                    dialog = ProgressDialog.show(
                            RenWuGuanLiActivity.this,
                            "",
                            getString(R.string.waiting),
                            true,
                            false,
                            null
                    );
                    CommMgr.commService.GetMySendTaskList(handler, Long.toString(GlobalData.GetUid(RenWuGuanLiActivity.this)), Integer.toString(nPageNo));
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
        setContentView(R.layout.renwuguanli_activity);

        mainLayout = (RelativeLayout)findViewById(R.id.rlRenWuGuanLiBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlRenWuGuanLiBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        initControl();

        dialog = ProgressDialog.show(
                RenWuGuanLiActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetMySendTaskList(handler, Long.toString(GlobalData.GetUid(RenWuGuanLiActivity.this)), Integer.toString(nPageNo));
    }

    private void initControl()
    {
        imgBack = (ImageView) findViewById(R.id.imgRenWuGuanLi_Back);
        imgBack.setOnClickListener(onClickListener);

        btnAdd = (Button) findViewById(R.id.btnRenWuGuanLi_Add);
        btnAdd.setOnClickListener(onClickListener);

        rlJieDao = (RelativeLayout) findViewById(R.id.rlRenWuGuanLi_JieDao);
        rlJieDao.setOnClickListener(onClickListener);

        btnShangYiYe = (Button) findViewById(R.id.btnRenWuGuanLi_ShangYiYe);
        btnShangYiYe.setOnClickListener(onClickListener);
        btnXiaYiYe = (Button) findViewById(R.id.btnRenWuGuanLi_XiaYiYe);
        btnXiaYiYe.setOnClickListener(onClickListener);
    }

    private void RefreshPage()
    {
        if (arrDatas != null)
        {
            RelativeLayout relativeLayout1 = (RelativeLayout) findViewById(R.id.rlLayout1);
            try {
                if (arrDatas.get(0) != null)
                {
                    TextView lblTitle = (TextView) findViewById(R.id.lblRenWuGuanLiItem_MingChengVal1);
                    lblTitle.setText(arrDatas.get(0).title);
                    TextView lblExecutors = (TextView) findViewById(R.id.lblRenWuGuanLiItem_ZeRenRen1);
                    lblExecutors.setText(getString(R.string.zerenren) + arrDatas.get(0).executors);
                    TextView lblState = (TextView) findViewById(R.id.lblRenWuGuanLiItem_WanCheng1);
                    lblState.setText(getString(R.string.shifouwancheng) + arrDatas.get(0).state);
                    relativeLayout1.setVisibility(View.VISIBLE);
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
            try
            {
                if (arrDatas.get(1) != null)
                {
                    TextView lblTitle = (TextView) findViewById(R.id.lblRenWuGuanLiItem_MingChengVal2);
                    lblTitle.setText(arrDatas.get(1).title);
                    TextView lblExecutors = (TextView) findViewById(R.id.lblRenWuGuanLiItem_ZeRenRen2);
                    lblExecutors.setText(getString(R.string.zerenren) + arrDatas.get(1).executors);
                    TextView lblState = (TextView) findViewById(R.id.lblRenWuGuanLiItem_WanCheng2);
                    lblState.setText(getString(R.string.shifouwancheng) + arrDatas.get(1).state);
                    relativeLayout2.setVisibility(View.VISIBLE);
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
            try
            {
                if (arrDatas.get(2) != null)
                {
                    TextView lblTitle = (TextView) findViewById(R.id.lblRenWuGuanLiItem_MingChengVal3);
                    lblTitle.setText(arrDatas.get(2).title);
                    TextView lblExecutors = (TextView) findViewById(R.id.lblRenWuGuanLiItem_ZeRenRen3);
                    lblExecutors.setText(getString(R.string.zerenren) + arrDatas.get(2).executors);
                    TextView lblState = (TextView) findViewById(R.id.lblRenWuGuanLiItem_WanCheng3);
                    lblState.setText(getString(R.string.shifouwancheng) + arrDatas.get(2).state);
                    relativeLayout3.setVisibility(View.VISIBLE);
                }
                else
                {
                    relativeLayout3.setVisibility(View.INVISIBLE);
                }
            } catch (Exception ex)
            {
                relativeLayout3.setVisibility(View.INVISIBLE);
            }

            RelativeLayout relativeLayout4 = (RelativeLayout) findViewById(R.id.rlLayout4);
            try
            {
                if (arrDatas.get(3) != null)
                {
                    TextView lblTitle = (TextView) findViewById(R.id.lblRenWuGuanLiItem_MingChengVal4);
                    lblTitle.setText(arrDatas.get(3).title);
                    TextView lblExecutors = (TextView) findViewById(R.id.lblRenWuGuanLiItem_ZeRenRen4);
                    lblExecutors.setText(getString(R.string.zerenren) + arrDatas.get(3).executors);
                    TextView lblState = (TextView) findViewById(R.id.lblRenWuGuanLiItem_WanCheng4);
                    lblState.setText(getString(R.string.shifouwancheng) + arrDatas.get(3).state);
                    relativeLayout4.setVisibility(View.VISIBLE);
                }
                else
                {
                    relativeLayout4.setVisibility(View.INVISIBLE);
                }
            } catch (Exception ex)
            {
                relativeLayout4.setVisibility(View.INVISIBLE);
            }

            RelativeLayout relativeLayout5 = (RelativeLayout) findViewById(R.id.rlLayout5);
            try
            {
                if (arrDatas.get(4) != null)
                {
                    TextView lblTitle = (TextView) findViewById(R.id.lblRenWuGuanLiItem_MingChengVal5);
                    lblTitle.setText(arrDatas.get(4).title);
                    TextView lblExecutors = (TextView) findViewById(R.id.lblRenWuGuanLiItem_ZeRenRen5);
                    lblExecutors.setText(getString(R.string.zerenren) + arrDatas.get(4).executors);
                    TextView lblState = (TextView) findViewById(R.id.lblRenWuGuanLiItem_WanCheng5);
                    lblState.setText(getString(R.string.shifouwancheng) + arrDatas.get(4).state);
                    relativeLayout5.setVisibility(View.VISIBLE);
                }
                else
                {
                    relativeLayout5.setVisibility(View.INVISIBLE);
                }
            } catch (Exception ex)
            {
                relativeLayout5.setVisibility(View.INVISIBLE);
            }
        }
    }
}
