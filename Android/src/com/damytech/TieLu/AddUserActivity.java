package com.damytech.TieLu;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.Intent;
import android.graphics.Color;
import android.graphics.Rect;
import android.graphics.drawable.ColorDrawable;
import android.os.Bundle;
import android.text.format.DateUtils;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.ViewTreeObserver;
import android.widget.*;
import com.damytech.HttpConn.JsonHttpResponseHandler;
import com.damytech.STData.STEmailUser;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.PullRefreshListView.PullToRefreshBase;
import com.damytech.utils.PullRefreshListView.PullToRefreshListView;
import com.damytech.utils.ResolutionSet;
import com.damytech.utils.SmartImageView.Global;
import org.json.JSONObject;
import java.util.ArrayList;

public class AddUserActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    final static int HISTORY_LIST_ONESHOW_COUNT = 10;

    private ImageView imgBack = null;
    private TextView lblOk = null;
    private EditText txtKeyword;
    private ImageView imgFind;

    boolean bExistNext = true;
    private int nRequestPageNo = 0;
    private PullToRefreshListView mListData;
    private ListView mRealListView;
    private ItemAdapter adapter;

    private JsonHttpResponseHandler handler;
    private ArrayList<STEmailUser> arrDatas = new ArrayList<STEmailUser>();
    private ProgressDialog progDialog = null;

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgBack:
                    finish();
                    break;
                case R.id.lblOk:
                    if (arrDatas != null)
                    {
                        String strIDs = "", strNames = "";
                        for (int i = 0; i < arrDatas.size(); i++)
                        {
                            if (arrDatas.get(i).isselected == 1)
                            {
                                strIDs = strIDs + arrDatas.get(i).uid + ",";
                                strNames = strNames + arrDatas.get(i).name + ", ";
                            }
                        }

                        if (strIDs.length() == 0)
                        {
                            GlobalData.showToast(AddUserActivity.this, getString(R.string.selectuser));
                            return;
                        }
                        else
                        {
                            Global.hideKeyboardFromText(txtKeyword, AddUserActivity.this);

                            Bundle bundle = new Bundle();
                            Intent retIntent = new Intent();
                            bundle.putString("ids", strIDs.substring(0, strIDs.length() - 1));
                            bundle.putString("names", strNames.substring(0, strNames.length() - 2));
                            retIntent.putExtras(bundle);
                            AddUserActivity.this.setResult(RESULT_OK, retIntent);
                            AddUserActivity.this.finish();
                        }
                    }
                    break;
                case R.id.imgFind:
                    String strKeyword = txtKeyword.getText().toString();
                    if (strKeyword.length() == 0)
                    {
                        GlobalData.showToast(AddUserActivity.this, getString(R.string.insertkeyword));
                        return;
                    }
                    RunBackgroundHandler(strKeyword);
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
        setContentView(R.layout.adduser_activity);

        mainLayout = (RelativeLayout)findViewById(R.id.rlAddUserBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlAddUserBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        initControl();
        initHandler();

        Thread.UncaughtExceptionHandler mUEHandler = new Thread.UncaughtExceptionHandler() {
            @Override
            public void uncaughtException(Thread t, Throwable e) {
                e.printStackTrace();
            }
        };
        Thread.setDefaultUncaughtExceptionHandler(mUEHandler);
    }

    private void initControl()
    {
        imgBack = (ImageView) findViewById(R.id.imgBack);
        imgBack.setOnClickListener(onClickListener);
        txtKeyword = (EditText) findViewById(R.id.txtKeyword);
        lblOk = (TextView) findViewById(R.id.lblOk);
        lblOk.setOnClickListener(onClickListener);
        imgFind = (ImageView) findViewById(R.id.imgFind);
        imgFind.setOnClickListener(onClickListener);

        mListData = (PullToRefreshListView) findViewById(R.id.viewDatas);
        mListData.setMode(PullToRefreshBase.Mode.PULL_FROM_END);

        arrDatas.clear();
        mListData.setOnRefreshListener(new PullToRefreshBase.OnRefreshListener<ListView>() {
            @Override
            public void onRefresh(PullToRefreshBase<ListView> refreshView) {
                String label = DateUtils.formatDateTime(getApplicationContext(), System.currentTimeMillis(), DateUtils.FORMAT_SHOW_TIME | DateUtils.FORMAT_SHOW_DATE | DateUtils.FORMAT_ABBREV_ALL);
                refreshView.getLoadingLayoutProxy().setLastUpdatedLabel(label);
                nRequestPageNo = nRequestPageNo + 1;

                String str = txtKeyword.getText().toString();
                CommMgr.commService.GetAllUserList(handler, str, Integer.toString(nRequestPageNo));
            }
        });

        mListData.setOnLastItemVisibleListener( new PullToRefreshBase.OnLastItemVisibleListener() {
            @Override
            public void onLastItemVisible() {}
        });

        mRealListView = mListData.getRefreshableView();
    }

    private void initHandler()
    {
        handler = new JsonHttpResponseHandler()
        {
            @Override
            public void onSuccess(JSONObject jsonData)
            {
                progDialog.dismiss();
                mListData.onRefreshComplete();

                ArrayList<STEmailUser> extraInfoList = new ArrayList<STEmailUser>();
                CommMgr.commService.parseGetAllUserList(jsonData, extraInfoList);

                for ( int i = 0; i < extraInfoList.size(); i++ )
                {
                    arrDatas.add(extraInfoList.get(i));
                }

                initContents();
            }

            @Override
            public void onFailure(Throwable ex, String exception) {
                progDialog.dismiss();
            }

            @Override
            public void onFinish() {}
        };
    }

    @Override
    public void onResume()
    {
        super.onResume();
    }

    private void RunBackgroundHandler(String keyword)
    {
        nRequestPageNo = 0;
        progDialog = ProgressDialog.show(
                AddUserActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null);
        CommMgr.commService.GetAllUserList(handler, keyword, Integer.toString(nRequestPageNo));

        return;
    }

    private void initContents()
    {
        if (mRealListView != null)
        {
            if (arrDatas == null)
                return;

            if (arrDatas.size() % HISTORY_LIST_ONESHOW_COUNT == 0)
            {
                bExistNext = true;
                mListData.setMode(PullToRefreshBase.Mode.PULL_FROM_END);
            }
            else
            {
                bExistNext = false;
                mListData.setMode(PullToRefreshBase.Mode.DISABLED);
            }

            if (mRealListView != null)
            {
                mRealListView.setCacheColorHint(Color.parseColor("#FFF1F1F1"));
                mRealListView.setDivider(new ColorDrawable(Color.parseColor("#FFCCCCCC")));
                mRealListView.setDividerHeight(2);

                adapter = new ItemAdapter(AddUserActivity.this, 0, arrDatas);
                mRealListView.setAdapter(adapter);
            }
        }
    }

    class ItemAdapter extends ArrayAdapter<STEmailUser>
    {
        ArrayList<STEmailUser> list;
        Context ctx;

        public ItemAdapter(Context ctx, int resourceId, ArrayList<STEmailUser> list) {
            super(ctx, resourceId, list);
            this.ctx = ctx;
            this.list = list;
        }

        @Override
        public int getCount() {
            return list.size();
        }

        @Override
        public long getItemId(int position) {
            return position;
        }

        @Override
        public View getView(int position, View convertView, ViewGroup parent) {
            View v = convertView;
            STEmailUser data = list.get(position);

            if (v == null)
            {
                LayoutInflater inflater = (LayoutInflater)ctx.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
                v = inflater.inflate(R.layout.adduseritem_view, null);
                ResolutionSet._instance.iterateChild(v.findViewById(R.id.rlViewBack));
            }

            ImageView imgCheck = (ImageView) v.findViewById(R.id.imgCheck);
            if (data.isselected == 0)
                imgCheck.setImageResource(R.drawable.uncheck);
            else
                imgCheck.setImageResource(R.drawable.check);
            imgCheck.setTag(position);
            imgCheck.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    Integer val = (Integer)v.getTag();
                    if (arrDatas.get(val).isselected == 0)
                    {
                        arrDatas.get(val).isselected = 1;
                        ((ImageView)v).setImageResource(R.drawable.check);
                    }
                    else
                    {
                        arrDatas.get(val).isselected = 0;
                        ((ImageView)v).setImageResource(R.drawable.uncheck);
                    }
                }
            });
            TextView lblName = (TextView) v.findViewById(R.id.lblName);
            lblName.setText(data.name);

            return v;
        }
    }
}
