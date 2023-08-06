package com.damytech.TieLu;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.Intent;
import android.graphics.Rect;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.ViewTreeObserver;
import android.widget.*;
import com.damytech.HttpConn.JsonHttpResponseHandler;
import com.damytech.STData.STContactInfo;
import com.damytech.STData.STEmailInfo;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;
import org.json.JSONObject;
import java.util.ArrayList;

public class TongXunLuActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    private ImageView imgBack = null;
    private Button btnShangYiYe;
    private Button btnXiaYiYe;

    int nPageNo = 0;
    ProgressDialog dialog = null;
    ArrayList<STContactInfo> arrDatas = new ArrayList<STContactInfo>();
    private ListView listData = null;
    private ContactItemAdapter adapter = null;
    private Button btnSortKeshi = null;
    private Button btnSortChedui = null;
    private Button btnSortLiechezhang = null;

    private String mContactKind = "";
    private String mRoleName = "";

    JsonHttpResponseHandler handler = new JsonHttpResponseHandler()
    {
        int result = 0;

        @Override
        public void onSuccess(JSONObject jsonData)
        {
            result = 1;
            dialog.dismiss();

            arrDatas = new ArrayList<STContactInfo>();
            CommMgr.commService.parseGetContactList(jsonData, arrDatas);
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
                GlobalData.showToast(TongXunLuActivity.this, getString(R.string.network_error));
                TongXunLuActivity.this.finish();
            }
            else
            {
                adapter = new ContactItemAdapter(TongXunLuActivity.this, 0, arrDatas);
                listData.setAdapter(adapter);
            }
        }
    };

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgTongXunLu_Back:
                    finish();
                    break;
                case R.id.btnSortKeshi:
                    mContactKind = getString(R.string.keshi);
                    mRoleName = "";
                    CommMgr.commService.GetContactList(handler, mContactKind, mRoleName);
                    break;
                case R.id.btnSortChedui:
                    mContactKind = getString(R.string.chedui);
                    mRoleName = "";
                    CommMgr.commService.GetContactList(handler, mContactKind, mRoleName);
                    break;
                case R.id.btnSortLiechezhang:
                    mContactKind = getString(R.string.chedui);
                    mRoleName = getString(R.string.liechezhang);
                    CommMgr.commService.GetContactList(handler, mContactKind, mRoleName);
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
        setContentView(R.layout.tongxunlu_activity);

        mainLayout = (RelativeLayout)findViewById(R.id.rlTongXunLuBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlTongXunLuBack));
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
                TongXunLuActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetContactList(handler, mContactKind, mRoleName);
    }

    private void initControl()
    {
        imgBack = (ImageView) findViewById(R.id.imgTongXunLu_Back);
        imgBack.setOnClickListener(onClickListener);
        listData = (ListView) findViewById(R.id.listData);
        btnSortKeshi = (Button)findViewById(R.id.btnSortKeshi);
        btnSortKeshi.setOnClickListener(onClickListener);
        btnSortChedui = (Button)findViewById(R.id.btnSortChedui);
        btnSortChedui.setOnClickListener(onClickListener);
        btnSortLiechezhang = (Button)findViewById(R.id.btnSortLiechezhang);
        btnSortLiechezhang.setOnClickListener(onClickListener);

        mContactKind = getString(R.string.keshi);
        mRoleName = "";
    }

    public class ContactItemAdapter extends ArrayAdapter<STContactInfo>
    {
        Context ctx;
        ArrayList<STContactInfo> list = new ArrayList<STContactInfo>();

        public ContactItemAdapter(Context ctx, int resourceId, ArrayList<STContactInfo> list) {
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
            if (v == null)
            {
                LayoutInflater inflater = (LayoutInflater)ctx.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
                v = inflater.inflate(R.layout.tongxunlu_item_view, null);
                ResolutionSet._instance.iterateChild(v.findViewById(R.id.rlViewBack));
            }

            STContactInfo item = list.get(position);
            TextView lblName = (TextView) v.findViewById(R.id.lblNameVal);
            lblName.setText(item.name);
            TextView lblPart = (TextView) v.findViewById(R.id.lblPartVal);
            lblPart.setText(item.partname);
            TextView lblGroup = (TextView) v.findViewById(R.id.lblGroupVal);
            lblGroup.setText(item.groupname);
            TextView lblRole = (TextView) v.findViewById(R.id.lblRoleVal);
            lblRole.setText(item.rolename);
            TextView lblRoleKind = (TextView) v.findViewById(R.id.lblRoleKindVal);
            lblRoleKind.setText(item.rolekind);
            TextView lblTrainNo = (TextView) v.findViewById(R.id.lblTrainNoVal);
            lblTrainNo.setText(item.trainno);
            TextView lblPhone = (TextView) v.findViewById(R.id.lblPhoneNumVal);
            lblPhone.setText(item.phonenum1);
            TextView lblSort = (TextView) v.findViewById(R.id.lblSortNumVal);
            lblSort.setText(item.sortnum1);
            TextView lblLine = (TextView) v.findViewById(R.id.lblLineNumVal);
            lblLine.setText(item.linenum);

            return v;
        }
    }
}
