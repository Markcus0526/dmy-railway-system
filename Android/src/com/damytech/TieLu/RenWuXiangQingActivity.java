package com.damytech.TieLu;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.graphics.Color;
import android.graphics.Rect;
import android.os.AsyncTask;
import android.os.Bundle;
import android.os.Environment;
import android.os.SystemClock;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewTreeObserver;
import android.widget.*;
import com.damytech.HttpConn.JsonHttpResponseHandler;
import com.damytech.STData.STTaskDetInfo;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.HorProgressor;
import com.damytech.utils.ResolutionSet;
import com.damytech.utils.SQLiteDBHelper;
import org.json.JSONObject;

import java.io.*;
import java.net.URL;
import java.net.URLConnection;
import java.net.URLEncoder;

public class RenWuXiangQingActivity extends Activity implements DialogInterface.OnDismissListener{
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    long nTaskID = 0;

    ImageView imgBack = null;
    TextView lblTitle = null;
    TextView lblKaiShiShiJian = null;
    TextView lblJieShuShiJian = null;
    ImageView imgStatus = null;
    TextView lblNeiRong = null;
    Button btnXiaZai = null;
    Button btnOpen = null;
    Button btnBaoGao = null;
    Button btnWanCheng = null;
    HorProgressor viewStatus = null;
    LinearLayout rlDatas = null;

    SQLiteDBHelper m_db;

    RenWuBaoGaoDialog dlg = null;

    ProgressDialog dialog = null;
    STTaskDetInfo stDetInfo = new STTaskDetInfo();
    JsonHttpResponseHandler handler = new JsonHttpResponseHandler()
    {
        int result = 0;

        @Override
        public void onSuccess(JSONObject jsonData)
        {
            result = 1;
            dialog.dismiss();

            CommMgr.commService.parseGetTaskDetInfo(jsonData, stDetInfo);
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
                GlobalData.showToast(RenWuXiangQingActivity.this, getString(R.string.network_error));
                RenWuXiangQingActivity.this.finish();
            }
            else
            {
                if (stDetInfo == null)
                {
                    GlobalData.showToast(RenWuXiangQingActivity.this, getString(R.string.dataerror));
                    RenWuXiangQingActivity.this.finish();
                }
                else
                    RefreshPage();
            }
        }
    };

    JsonHttpResponseHandler handlerRun = new JsonHttpResponseHandler()
    {
        int result = 0;

        @Override
        public void onSuccess(JSONObject jsonData)
        {
            result = 1;
            dialog.dismiss();

            long nRet = CommMgr.commService.parseSetTaskRunned(jsonData);
            if (nRet == 0)
            {
                GlobalData.showToast(RenWuXiangQingActivity.this, getString(R.string.caozuochenggong));
                RenWuXiangQingActivity.this.finish();
            }
            else
            {
                GlobalData.showToast(RenWuXiangQingActivity.this, getString(R.string.service_error));
            }
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
                GlobalData.showToast(RenWuXiangQingActivity.this, getString(R.string.network_error));
                RenWuXiangQingActivity.this.finish();
            }
        }
    };

    JsonHttpResponseHandler handlerComplete = new JsonHttpResponseHandler()
    {
        int result = 0;

        @Override
        public void onSuccess(JSONObject jsonData)
        {
            result = 1;
            dialog.dismiss();

            long nRet = CommMgr.commService.parseSetTaskCompleted(jsonData);
            if (nRet == 0)
            {
                GlobalData.showToast(RenWuXiangQingActivity.this, getString(R.string.caozuochenggong));
                RenWuXiangQingActivity.this.finish();
            }
            else if ( nRet == -3 )
            {
                GlobalData.showToast(RenWuXiangQingActivity.this, getString(R.string.yiwanchenggongzuo));
                RenWuXiangQingActivity.this.finish();
            }
            else
            {
                GlobalData.showToast(RenWuXiangQingActivity.this, getString(R.string.service_error));
            }
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
                GlobalData.showToast(RenWuXiangQingActivity.this, getString(R.string.network_error));
                RenWuXiangQingActivity.this.finish();
            }
        }
    };

    JsonHttpResponseHandler handlerLog = new JsonHttpResponseHandler()
    {
        int result = 0;

        @Override
        public void onSuccess(JSONObject jsonData)
        {
            result = 1;
            dialog.dismiss();

            long nRet = CommMgr.commService.parseSetTaskLog(jsonData);
            if (nRet == 0)
            {
                GlobalData.showToast(RenWuXiangQingActivity.this, getString(R.string.caozuochenggong));
                RenWuXiangQingActivity.this.finish();
            }
            else
            {
                GlobalData.showToast(RenWuXiangQingActivity.this, getString(R.string.service_error));
            }
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
                GlobalData.showToast(RenWuXiangQingActivity.this, getString(R.string.network_error));
                RenWuXiangQingActivity.this.finish();
            }
        }
    };

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgRenWuXiangQing_Back:
                    finish();
                    break;
                case R.id.btnXiaZai:
                    viewStatus.setVisibility(View.VISIBLE);
                    FileDownloaderTask downloaderTask = new FileDownloaderTask();
                    downloaderTask.execute(stDetInfo);
                    break;
                case R.id.btnOpen:
                    if (stDetInfo != null) {
                        String szPath = m_db.getLocalPath(stDetInfo.filepath);
                        if (szPath.length() == 0) {
                            GlobalData.showToast(RenWuXiangQingActivity.this, getString(R.string.notexitfile));
                            return;
                        } else {
                            try {
                                GlobalData.openFile(RenWuXiangQingActivity.this, new File(szPath));
                            } catch (IOException e) {
                                GlobalData.showToast(RenWuXiangQingActivity.this, getString(R.string.fileoopenerror));
                            }
                        }
                    } else {
                        GlobalData.showToast(RenWuXiangQingActivity.this, getString(R.string.notexitfile));
                    }
                    break;
                case R.id.btnReport:
                    dlg = new RenWuBaoGaoDialog(RenWuXiangQingActivity.this);
                    dlg.setSize(nWidth, nHeight);
                    dlg.setOnDismissListener(RenWuXiangQingActivity.this);
                    dlg.show();
                    break;
                case R.id.btnRun:
                    dialog = ProgressDialog.show(
                            RenWuXiangQingActivity.this,
                            "",
                            getString(R.string.waiting),
                            true,
                            false,
                            null
                    );
                    if (stDetInfo.state == 0)
                    {
                        CommMgr.commService.SetTaskRunned(handlerRun, Long.toString(GlobalData.GetUid(RenWuXiangQingActivity.this)), Long.toString(nTaskID));
                    }
                    else
                    {
                        CommMgr.commService.SetTaskCompleted(handlerComplete, Long.toString(GlobalData.GetUid(RenWuXiangQingActivity.this)), Long.toString(nTaskID));
                    }
                    break;
            }
        }
    };
    /**
     * Called when the activity is first created.
     */
    int nWidth = 0;
    int nHeight = 0;
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.renwuxiangqing_activity);

        m_db = new SQLiteDBHelper(this);

        mainLayout = (RelativeLayout)findViewById(R.id.rlRenWuXiangQing_Back);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            nWidth = r.width();
                            nHeight = r.height();
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlRenWuXiangQing_Back));
                            bInitialized = true;
                        }
                    }
                }
        );

        initControl();

        nTaskID = getIntent().getLongExtra("uid", 0);

        dialog = ProgressDialog.show(
                RenWuXiangQingActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetTaskDetInfo(handler, Long.toString(GlobalData.GetUid(RenWuXiangQingActivity.this)), Long.toString(nTaskID));
    }

    private void initControl()
    {
        imgBack = (ImageView) findViewById(R.id.imgRenWuXiangQing_Back);
        imgBack.setOnClickListener(onClickListener);

        lblTitle = (TextView) findViewById(R.id.lblTitle);
        lblKaiShiShiJian = (TextView) findViewById(R.id.lblKaiShiShiJian);
        lblJieShuShiJian = (TextView) findViewById(R.id.lblJieShuShiJian);
        lblNeiRong = (TextView) findViewById(R.id.lblNeiRong);
        btnXiaZai = (Button) findViewById(R.id.btnXiaZai);
        btnXiaZai.setOnClickListener(onClickListener);
        btnOpen = (Button) findViewById(R.id.btnOpen);
        btnOpen.setOnClickListener(onClickListener);
        btnBaoGao = (Button) findViewById(R.id.btnReport);
        btnBaoGao.setOnClickListener(onClickListener);
        btnWanCheng = (Button) findViewById(R.id.btnRun);
        btnWanCheng.setOnClickListener(onClickListener);
        viewStatus = (HorProgressor) findViewById(R.id.viewProgress);
        viewStatus.setFillColor(Color.parseColor("#2A98E7"));
        viewStatus.setBorderColor(Color.parseColor("#2A98E7"));
        rlDatas = (LinearLayout) findViewById(R.id.rlDatas);
        imgStatus = (ImageView)findViewById(R.id.imgStatus);
    }

    private void RefreshPage()
    {
        if (stDetInfo != null)
        {
            lblTitle.setText(stDetInfo.title);
            lblKaiShiShiJian.setText(getString(R.string.kaishishijian) + stDetInfo.startdate);
            lblJieShuShiJian.setText(getString(R.string.jieshushijian) + stDetInfo.enddate);
            switch (stDetInfo.state)
            {
                case 0:
                    imgStatus.setImageResource(R.drawable.starting);
                    btnWanCheng.setText(getString(R.string.zhixing));
                    btnWanCheng.setVisibility(View.VISIBLE);
                    btnBaoGao.setVisibility(View.VISIBLE);
                    break;
                case 1:
                    imgStatus.setImageResource(R.drawable.running);
                    btnWanCheng.setText(getString(R.string.wancheng));
                    btnWanCheng.setVisibility(View.VISIBLE);
                    btnBaoGao.setVisibility(View.VISIBLE);
                    break;
                case 2:
                    imgStatus.setImageResource(R.drawable.ending);
                    btnWanCheng.setVisibility(View.INVISIBLE);
                    btnBaoGao.setVisibility(View.INVISIBLE);
                    break;
            }
            lblNeiRong.setText(stDetInfo.content);
            if (stDetInfo.filename.length() > 0)
            {
                btnXiaZai.setVisibility(View.VISIBLE);
                btnOpen.setVisibility(View.VISIBLE);
            }

            if (stDetInfo.tasklog != null && stDetInfo.tasklog.size() > 0)
            {
                for (int i = 0; i < stDetInfo.tasklog.size(); i++)
                {
                    View v = null;
                    LayoutInflater inflater = (LayoutInflater)RenWuXiangQingActivity.this.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
                    v = inflater.inflate(R.layout.renwuxiangqingitem_view, null);
                    ResolutionSet._instance.iterateChild(v.findViewById(R.id.rlRenWuXiangQingItemBack));

                    TextView lblDate = (TextView) v.findViewById(R.id.lblDate);
                    lblDate.setText(stDetInfo.tasklog.get(i).postdate);
                    TextView lblName = (TextView) v.findViewById(R.id.lblName);
                    lblName.setText(stDetInfo.tasklog.get(i).name);
                    TextView lblContent = (TextView) v.findViewById(R.id.lblContent);
                    lblContent.setText(stDetInfo.tasklog.get(i).content);

                    try {
                        rlDatas.addView(v);
                    } catch (Exception ex) {
                    }
                }
            }
        }

        return;
    }

    @Override
    public void onStop()
    {
        super.onStop();
        if (dlg != null && dlg.isShowing())
            dlg.dismiss();
    }

    @Override
    public void onDismiss(DialogInterface dialog1)
    {
        dlg = (RenWuBaoGaoDialog) dialog1;

        String strData = dlg.getContent();
        if (strData.length() > 0 )
        {
            dialog = ProgressDialog.show(
                    RenWuXiangQingActivity.this,
                    "",
                    getString(R.string.waiting),
                    true,
                    false,
                    null
            );
            CommMgr.commService.SetTaskLog(handlerLog, Long.toString(GlobalData.GetUid(RenWuXiangQingActivity.this)),
                    Long.toString(nTaskID), strData);
        }
    }

    String szFilePath = "";
    class FileDownloaderTask extends AsyncTask<STTaskDetInfo, Integer, Void> {

        public FileDownloaderTask() {
        }

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
        }

        @Override
        protected Void doInBackground(STTaskDetInfo... params) {
            int count;

            try {
                publishProgress(0);

                int p=params[0].filepath.lastIndexOf("/");
                String dirPath = params[0].filepath.substring(0, p);
                String filePath = params[0].filepath.substring(p + 1);
                URL url = new URL(dirPath + "/" + URLEncoder.encode(filePath.replace(",", ""), "UTF-8"));
                URLConnection connection = url.openConnection();
                connection.connect();

                int nFileSize = connection.getContentLength();

                File file = new File(Environment.getExternalStorageDirectory(), "/Download");
                if (!file.exists()) {
                    if (!file.mkdirs())
                    {
                        return null;
                    }
                }

                String strFileName = "/" + params[0].filename;
                String strLocalPath = file.toString() + strFileName;
                szFilePath = strLocalPath;

                InputStream input = connection.getInputStream();//
                BufferedInputStream bis = new BufferedInputStream(input);
                OutputStream output = new FileOutputStream(strLocalPath);

                long total = 0;
                byte data[] = new byte[8192];
                int progress = 0;
                while ( (count = bis.read(data)) != -1 ) {
                    total += count;
                    int newProgress = (int)((total*100)/nFileSize);
                    if ( progress != newProgress ) {
                        progress = newProgress;
                        publishProgress(progress);
                    }
                    output.write(data, 0, count);

                    //Thread.sleep(10);
                    SystemClock.sleep(1);
                }
                input.close();
                output.flush();
                output.close();

            } catch (Exception e) {
                onCancelled();
            }

            return null;
        }

        @Override
        protected void onProgressUpdate(Integer... progress) {
            try {
                viewStatus.setProgress(progress[0].intValue());
            }
            catch (Exception ex)
            {
            }
        }

        @Override
        // Once the image is downloaded, associates it to the imageView
        protected void onPostExecute(Void retVal) {
            try
            {
                runOnUiThread(new Runnable() {
                    @Override
                    public void run() {
                        m_db.saveDownInfo(szFilePath, stDetInfo.filepath);
                    }
                });
            }
            catch (Exception ex)
            {
            }
        }

        @Override
        protected void onCancelled() {
            try
            {
                GlobalData.showToast(RenWuXiangQingActivity.this, getString(R.string.xiazaishibai));

                super.onCancelled();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
