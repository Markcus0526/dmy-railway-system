package com.damytech.TieLu;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Intent;
import android.graphics.Rect;
import android.net.Uri;
import android.os.Bundle;
import android.util.Base64;
import android.view.View;
import android.view.ViewTreeObserver;
import android.widget.*;
import com.damytech.HttpConn.JsonHttpResponseHandler;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;
import org.json.JSONObject;
import java.io.File;
import java.io.FileInputStream;

public class YouJianZhuanFaActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    final static int FILE_RESULT = 1;
    final static int ADD_RECEIVER = 2;
    private Uri uriFileResult = null;
    private String strFilePath = "";
    private String strFileName = "";
    private String strReceiverNames = "";
    private String strReceiverIDs= "";

    ImageView imgBack = null;
    ImageView imgAdd = null;
    TextView lblSend = null;
    TextView lblFileName = null;
    EditText txtReceiver = null;
    EditText txtTitle = null;
    Button btnAddFile = null;
    EditText txtContent = null;

    long uid = 0;
    String title = "";
    String content = "";
    String filename = "";
    String filepath = "";
    String receiver = "";
    long receiver_id = 0;
    int is_answer = 0;

    private JsonHttpResponseHandler handler = null;
    private ProgressDialog dialog = null;

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgBack:
                    finish();
                    break;
                case R.id.lblSend:
                    if (strReceiverNames.length() == 0)
                    {
                        GlobalData.showToast(YouJianZhuanFaActivity.this, getString(R.string.selectuser));
                        return;
                    }
                    String strTitle = txtTitle.getText().toString();
                    if (strTitle.length() == 0)
                    {
                        GlobalData.showToast(YouJianZhuanFaActivity.this, getString(R.string.shuru_biaoti));
                        return;
                    }
                    String strContent = txtContent.getText().toString();
                    if (strContent.length() == 0)
                    {
                        GlobalData.showToast(YouJianZhuanFaActivity.this, getString(R.string.insertemailcontent));
                        return;
                    }
                    String strFileData = "";
                    try {
                        File file = new File(strFilePath);
                        int fileLen = (int) file.length();
                        byte []data = new byte[fileLen];
                        FileInputStream fis = new FileInputStream(file);
                        fis.read(data);
                        strFileData = Base64.encodeToString(data, Base64.DEFAULT);
                    } catch (Exception e) {
                        strFileName = "";
                        strFileData = "";
                    }
                    dialog = ProgressDialog.show(
                            YouJianZhuanFaActivity.this,
                            "",
                            getString(R.string.waiting),
                            true,
                            false,
                            null
                    );
                    CommMgr.commService.SetReply(handler,
                            Long.toString(GlobalData.GetUid(YouJianZhuanFaActivity.this)),
                            strReceiverIDs,
                            strTitle,
                            strContent,
                            strFileName,
                            strFileData,
                            Long.toString(uid)
                            );
                    break;
                case R.id.btnAddFile:
                    Intent intentFile = new Intent(YouJianZhuanFaActivity.this, FileSelectActivity.class);
                    startActivityForResult(intentFile, FILE_RESULT);
                    break;
                case R.id.imgAddUser:
                    Intent intentAdd = new Intent(YouJianZhuanFaActivity.this, AddUserActivity.class);
                    startActivityForResult(intentAdd, ADD_RECEIVER);
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
        setContentView(R.layout.youjianzhuanfa_activity);

        try {
            uid = getIntent().getLongExtra("YID", 0);
            title = getIntent().getStringExtra("TITLE");
            content = getIntent().getStringExtra("CONTENT");
            filename = getIntent().getStringExtra("FILENAME");
            filepath = getIntent().getStringExtra("FILEPATH");
            receiver_id = getIntent().getLongExtra("RECEIVER_ID", 0);
            receiver = getIntent().getStringExtra("RECEIVER_NAME");
            is_answer = getIntent().getIntExtra("IS_ANSWER", 0);

            strReceiverIDs = receiver_id + "";
            strReceiverNames = receiver;
        }
        catch (Exception ex) {
            ex.printStackTrace();
        }


        mainLayout = (RelativeLayout)findViewById(R.id.rlYouJianZhuanFaBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlYouJianZhuanFaBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        initControl();
        initHandler();
    }

    private void initControl()
    {

        imgBack = (ImageView) findViewById(R.id.imgBack);
        imgBack.setOnClickListener(onClickListener);
        imgAdd = (ImageView) findViewById(R.id.imgAddUser);
        imgAdd.setOnClickListener(onClickListener);

        txtTitle = (EditText) findViewById(R.id.txtTitle);
        txtTitle.setText(title);
        txtContent = (EditText) findViewById(R.id.txtContent);
        txtContent.setText(content);
        txtReceiver = (EditText) findViewById(R.id.txtReceiver);

        lblSend = (TextView) findViewById(R.id.lblSend);
        lblSend.setOnClickListener(onClickListener);
        lblFileName = (TextView) findViewById(R.id.lblFileName);
        lblFileName.setText(getString(R.string.fujian) + filename);
        btnAddFile = (Button) findViewById(R.id.btnAddFile);
        btnAddFile.setOnClickListener(onClickListener);

        if(is_answer == 1) {
            TextView lblYouJianZhuanFaTitle = (TextView)findViewById(R.id.lblYouJianZhuanFaTitle);
            lblYouJianZhuanFaTitle.setText("回复");
            txtReceiver.setText(receiver);
        }
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
                if (result == 1)
                {
                    GlobalData.showToast(YouJianZhuanFaActivity.this, getString(R.string.caozuochenggong));
                    finish();
                }
                else
                {
                    GlobalData.showToast(YouJianZhuanFaActivity.this, getString(R.string.network_error));
                }
            }
        };
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent intent)
    {
        if (resultCode == RESULT_OK)
        {
            if (requestCode == FILE_RESULT)
            {
                if (intent != null)
                {
                    uriFileResult = intent.getData();
                    strFilePath = intent.getStringExtra("PATH");
                    strFileName = intent.getStringExtra("NAME");

                    uid = -1;
                    lblFileName.setText(getString(R.string.fujian) + strFileName);

                    //GlobalData.showToast(YouJianZhuanFaActivity.this, strFilePath);
                }
                else
                {
                    GlobalData.showToast(YouJianZhuanFaActivity.this, getString(R.string.fileselecterror));
                }
            }

            if (requestCode == ADD_RECEIVER)
            {
                strReceiverIDs = intent.getStringExtra("ids");
                strReceiverNames = intent.getStringExtra("names");

                txtReceiver.setText(strReceiverNames);
            }
        }
    }
}
