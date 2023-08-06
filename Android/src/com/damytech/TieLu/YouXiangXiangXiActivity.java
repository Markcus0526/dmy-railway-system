package com.damytech.TieLu;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Intent;
import android.graphics.Color;
import android.graphics.Rect;
import android.os.AsyncTask;
import android.os.Bundle;
import android.os.Environment;
import android.os.SystemClock;
import android.view.View;
import android.view.ViewTreeObserver;
import android.widget.*;
import com.damytech.HttpConn.JsonHttpResponseHandler;
import com.damytech.STData.STEmailInfo;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.HorProgressor;
import com.damytech.utils.ResolutionSet;
import com.damytech.utils.SQLiteDBHelper;
import org.apache.http.HttpResponse;
import org.json.JSONObject;
import java.io.*;
import java.net.HttpURLConnection;
import java.net.URL;
import java.net.URLConnection;
import java.net.URLEncoder;

public class YouXiangXiangXiActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    ImageView imgBack = null;
    TextView lblTitle = null;
    TextView lblDate = null;
    TextView lblFile = null;
    Button btnDownload = null;
    Button btnOpen = null;
    Button btnReply = null;
    Button btnReport = null;
    Button btnAnswer = null;
    HorProgressor viewStatus = null;
    TextView lblContent = null;

    long nEmailID = 0;
    private STEmailInfo arrData = new STEmailInfo();
    private JsonHttpResponseHandler handler = null;
    private JsonHttpResponseHandler handlerRead = null;
    private JsonHttpResponseHandler handlerReport = null;
    private ProgressDialog dialog = null;

    int mSent = 0;
    String szFilePath = "";
    SQLiteDBHelper m_db = null;

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgBack:
                    finish();
                    break;
                case R.id.btnDownload:
                    viewStatus.setVisibility(View.VISIBLE);
                    FileDownloaderTask downloaderTask = new FileDownloaderTask();
                    downloaderTask.execute(arrData);
                    break;
                case R.id.btnOpen:
                    if (arrData != null) {
                        String szPath = m_db.getLocalPath(arrData.filepath);
                        if (szPath.length() == 0) {
                            GlobalData.showToast(YouXiangXiangXiActivity.this, getString(R.string.notexitfile));
                            return;
                        } else {
                            try {
                                GlobalData.openFile(YouXiangXiangXiActivity.this, new File(szPath));
                            } catch (IOException e) {
                                GlobalData.showToast(YouXiangXiangXiActivity.this, getString(R.string.fileoopenerror));
                            }
                        }
                    } else {
                        GlobalData.showToast(YouXiangXiangXiActivity.this, getString(R.string.notexitfile));
                    }
                    break;
                case R.id.btnReply:
                    Intent intent = new Intent(YouXiangXiangXiActivity.this, YouJianZhuanFaActivity.class);
                    intent.putExtra("YID", arrData.uid);
                    intent.putExtra("TITLE", arrData.title);
                    intent.putExtra("CONTENT", arrData.content);
                    intent.putExtra("FILENAME", arrData.filename);
                    intent.putExtra("FILEPATH", arrData.filepath);
                    startActivity(intent);
                    break;
                case R.id.btnReport:
                    dialog = ProgressDialog.show(
                            YouXiangXiangXiActivity.this,
                            "",
                            getString(R.string.waiting),
                            true,
                            false,
                            null
                    );
                    CommMgr.commService.SendReceipt(handlerReport, Long.toString(GlobalData.GetUid(YouXiangXiangXiActivity.this)), Long.toString(nEmailID));
                    break;
                case R.id.btnAnswer:
                    Intent anser_intent = new Intent(YouXiangXiangXiActivity.this, YouJianZhuanFaActivity.class);
                    anser_intent.putExtra("YID", arrData.uid);
                    anser_intent.putExtra("TITLE", arrData.title);
                    anser_intent.putExtra("CONTENT", arrData.content);
                    anser_intent.putExtra("FILENAME", arrData.filename);
                    anser_intent.putExtra("FILEPATH", arrData.filepath);
                    anser_intent.putExtra("RECEIVER_ID", arrData.senderid);
                    anser_intent.putExtra("RECEIVER_NAME", arrData.sender);
                    anser_intent.putExtra("IS_ANSWER", 1);
                    startActivity(anser_intent);
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
        setContentView(R.layout.youxiangxiangxi_activity);

        mainLayout = (RelativeLayout)findViewById(R.id.rlYouXiangXiangXiBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlYouXiangXiangXiBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        nEmailID= getIntent().getLongExtra("emailid", 0);
        mSent = getIntent().getIntExtra("sent", 0);

        m_db = new SQLiteDBHelper(this);

        initControl();
        initHandler();

        dialog = ProgressDialog.show(
                YouXiangXiangXiActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetEmailDetInfo(handler, Long.toString(GlobalData.GetUid(YouXiangXiangXiActivity.this)), Long.toString(nEmailID));
        CommMgr.commService.SetReadedState(handlerRead, Long.toString(GlobalData.GetUid(YouXiangXiangXiActivity.this)), Long.toString(nEmailID));
    }

    private void initControl()
    {
        imgBack = (ImageView) findViewById(R.id.imgBack);
        imgBack.setOnClickListener(onClickListener);

        lblTitle = (TextView) findViewById(R.id.lblTitle);
        lblDate = (TextView) findViewById(R.id.lblDate);
        lblFile = (TextView) findViewById(R.id.lblFileName);
        lblContent = (TextView) findViewById(R.id.lblContent);
        btnDownload = (Button) findViewById(R.id.btnDownload);
        btnDownload.setOnClickListener(onClickListener);
        btnOpen = (Button) findViewById(R.id.btnOpen);
        btnOpen.setOnClickListener(onClickListener);
        btnReply = (Button) findViewById(R.id.btnReply);
        btnReply.setOnClickListener(onClickListener);
        btnReport = (Button)findViewById(R.id.btnReport);
        btnReport.setOnClickListener(onClickListener);
        btnAnswer = (Button)findViewById(R.id.btnAnswer);
        btnAnswer.setOnClickListener(onClickListener);
        if (mSent == 1)
        {
            btnReply.setVisibility(View.INVISIBLE);
            btnReport.setVisibility(View.INVISIBLE);
        }
        viewStatus = (HorProgressor) findViewById(R.id.viewStatus);
        viewStatus.setFillColor(Color.parseColor("#2A98E7"));
        viewStatus.setBorderColor(Color.parseColor("#2A98E7"));
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

                CommMgr.commService.parseGetEmailDetInfo(jsonData, arrData);
                if (arrData == null)
                    result = -1;
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
                    lblTitle.setText(getString(R.string.biaoti1) + arrData.title);
                    lblDate.setText(getString(R.string.fasongriqi) + arrData.postdate);
                    lblFile.setText(getString(R.string.fujian) + arrData.filename);
                    lblContent.setText(GlobalData.unescape(arrData.content));
                    if (arrData.filename == null || arrData.filename.length() == 0)
                    {
                        btnDownload.setVisibility(View.INVISIBLE);
                        btnOpen.setVisibility(View.INVISIBLE);
                        viewStatus.setVisibility(View.INVISIBLE);
                    }

                    if (arrData.isreceipt == 1)
                    {
                        btnReport.setText(getString(R.string.yifasong));
                        btnReport.setBackgroundResource(R.drawable.roundbluegray_layout);
                        btnReport.setEnabled(false);
                    }
                    else {}
                }

                if (result == -1)
                {
                    GlobalData.showToast(YouXiangXiangXiActivity.this, getString(R.string.network_error));
                    finish();
                }
            }
        };

        handlerRead = new JsonHttpResponseHandler()
        {
            @Override
            public void onSuccess(JSONObject jsonData) {}
            @Override
            public void onFailure(Throwable ex, String exception) {}
            @Override
            public void onFinish() {}
        };

        handlerReport = new JsonHttpResponseHandler()
        {
            int result = 0;
            long nVal = 0;

            @Override
            public void onSuccess(JSONObject jsonData)
            {
                dialog.dismiss();
                result = 1;

                nVal = CommMgr.commService.parseSendReceipt(jsonData);
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
                    if (nVal == 0)
                    {
                        GlobalData.showToast(YouXiangXiangXiActivity.this, getString(R.string.caozuochenggong));

                        btnReport.setText(getString(R.string.yifasong));
                        btnReport.setBackgroundResource(R.drawable.roundbluegray_layout);
                        btnReport.setEnabled(false);
                    }
                    else
                    {
                        GlobalData.showToast(YouXiangXiangXiActivity.this, getString(R.string.service_error));
                        return;
                    }
                }

                if (result == -1)
                {
                    GlobalData.showToast(YouXiangXiangXiActivity.this, getString(R.string.network_error));
                    finish();
                }
            }
        };
    }

    class FileDownloaderTask extends AsyncTask<STEmailInfo, Integer, Void> {

        public FileDownloaderTask() {
        }

        @Override
        protected void onPreExecute() {
            super.onPreExecute();
        }

        @Override
        protected Void doInBackground(STEmailInfo... params) {
            int count;

            try {
                publishProgress(0);

                int p=params[0].filepath.lastIndexOf("/");
                String dirPath = params[0].filepath.substring(0, p);
                String filePath = params[0].filepath.substring(p + 1);
                //URL url = new URL(dirPath + "/" + URLEncoder.encode(filePath.replace(",", ""), "UTF-8"));
                URL url = new URL(dirPath + "/" + filePath.replace(",", ""));
                HttpURLConnection connection = (HttpURLConnection)url.openConnection();
                connection.setRequestMethod("GET");
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

                int status = connection.getResponseCode();
                if(status != HttpURLConnection.HTTP_OK)
                {
                    InputStream errStream = connection.getErrorStream();
                    BufferedInputStream errBis = new BufferedInputStream(errStream);
                    long total = 0;
                    byte data[] = new byte[8192];
                    int progress = 0;
                    String errStr = "";
                    while ( (count = errBis.read(data)) != -1 ) {
                        total += count;
                        int newProgress = (int)((total*100)/nFileSize);
                        if ( progress != newProgress ) {
                            progress = newProgress;
                            publishProgress(progress);
                        }
                        errStr += new String(data, "UTF-8");
                    }
                    return null;
                }

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
            m_db.saveDownInfo(szFilePath, arrData.filepath);
        }

        @Override
        protected void onCancelled() {
            try
            {
                GlobalData.showToast(YouXiangXiangXiActivity.this, getString(R.string.xiazaishibai));

                super.onCancelled();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
