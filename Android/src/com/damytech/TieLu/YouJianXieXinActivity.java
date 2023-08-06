package com.damytech.TieLu;

import android.app.Dialog;
import android.app.ProgressDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Rect;
import android.net.Uri;
import android.os.Bundle;
import android.os.Environment;
import android.util.Base64;
import android.view.View;
import android.view.ViewTreeObserver;
import android.widget.*;
import com.damytech.HttpConn.JsonHttpResponseHandler;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;
import org.json.JSONObject;
import java.io.*;

public class YouJianXieXinActivity extends FileBrowserActivity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    final static int ADD_RECEIVER = 1;
    final static int FILE_RESULT = 2;

    private String strReceiverNames = "";
    private String strReceiverIDs= "";

    ImageView imgBack = null;
    ImageView imgAdd = null;
    TextView lblSend = null;
    TextView lblAddFile = null;
    EditText txtReceiver = null;
    EditText txtTitle = null;
    Button btnAddFile = null;
    Button btnAddPhoto = null;
    EditText txtContent = null;
    ImageView imgAddPhoto = null;

    private Bitmap bmpPhoto = null;
    Dialog dlgFileBrowse = null;

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
                        GlobalData.showToast(YouJianXieXinActivity.this, getString(R.string.selectuser));
                        return;
                    }
                    String strTitle = txtTitle.getText().toString();
                    if (strTitle.length() == 0)
                    {
                        GlobalData.showToast(YouJianXieXinActivity.this, getString(R.string.shuru_biaoti));
                        return;
                    }
                    String strContent = txtContent.getText().toString();
                    if (strContent.length() == 0)
                    {
                        GlobalData.showToast(YouJianXieXinActivity.this, getString(R.string.insertemailcontent));
                        return;
                    }
                    String strFileData = "";
                    if (bmpPhoto == null) {
                        try {
                            File file = new File(szSelFilePath);
                            int fileLen = (int) file.length();
                            byte[] data = new byte[fileLen];
                            FileInputStream fis = new FileInputStream(file);
                            fis.read(data);
                            strFileData = Base64.encodeToString(data, Base64.DEFAULT);
                        } catch (Exception e) {
                            szSelFileName = "";
                            strFileData = "";
                        }
                    }  else {
                        try {
                            String file_path = Environment.getExternalStorageDirectory().getAbsolutePath();
                            File dir = new File(file_path);
                            if (!dir.exists())
                                dir.mkdirs();

                            byte[] bmpBytes = null;
                            if (bmpPhoto != null)
                            {
                                ByteArrayOutputStream baos = new ByteArrayOutputStream();
                                bmpPhoto.compress(Bitmap.CompressFormat.JPEG, 100, baos);
                                bmpBytes = baos.toByteArray();
                                strFileData = Base64.encodeToString(bmpBytes, Base64.DEFAULT);
                            }
                        } catch (Exception ex) {
                            szSelFileName = "";
                            strFileData = "";
                        }
                    }
                    dialog = ProgressDialog.show(
                            YouJianXieXinActivity.this,
                            "",
                            getString(R.string.waiting),
                            true,
                            false,
                            null
                    );
                    CommMgr.commService.SetEmail(handler,
                            Long.toString(GlobalData.GetUid(YouJianXieXinActivity.this)),
                            strReceiverIDs,
                            strTitle,
                            strContent,
                            szSelFileName,
                            strFileData
                            );
                    break;
                case R.id.btnAddFile:
                    Intent intent_addfile = new Intent("android.intent.action.GET_CONTENT");
                    intent_addfile.setType("application/pdf;application/vnd.openxmlformats—officedocument.spreadsheetml.sheet;application/vnd.openxmlformats—officedocument.wordprocessingml.document");
                    intent_addfile.addCategory("android.intent.category.OPENABLE");
                    try
                    {
                        YouJianXieXinActivity.this.startActivityForResult(intent_addfile, FILE_RESULT);
                    }
                    catch (Exception localException2)
                    {
                    }
                    break;
                case R.id.btnAddPhoto:
                    Intent intent = new Intent(YouJianXieXinActivity.this, SelectPhotoActivity.class);
                    startActivityForResult(intent, 1);
                    break;
                case R.id.imgAddUser:
                    Intent intentAdd = new Intent(YouJianXieXinActivity.this, AddUserActivity.class);
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
        setContentView(R.layout.youjianxiexin_activity);

        mainLayout = (RelativeLayout)findViewById(R.id.rlYouJianXieXinBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false) {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlYouJianXieXinBack));
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
        txtContent = (EditText) findViewById(R.id.txtContent);
        txtReceiver = (EditText) findViewById(R.id.txtReceiver);
        lblAddFile = (TextView) findViewById(R.id.lblFileName);
        lblSend = (TextView) findViewById(R.id.lblSend);
        lblSend.setOnClickListener(onClickListener);
        btnAddFile = (Button) findViewById(R.id.btnAddFile);
        btnAddFile.setOnClickListener(onClickListener);
        btnAddPhoto = (Button)findViewById(R.id.btnAddPhoto);
        btnAddPhoto.setOnClickListener(onClickListener);
        imgAddPhoto = (ImageView)findViewById(R.id.imgAddedPhoto);
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
                    GlobalData.showToast(YouJianXieXinActivity.this, getString(R.string.caozuochenggong));
                    finish();
                }

                if (result == -1)
                {
                    GlobalData.showToast(YouJianXieXinActivity.this, getString(R.string.network_error));
                    finish();
                }
            }
        };
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent intent) {
        if (resultCode == RESULT_OK) {
            if (requestCode == ADD_RECEIVER) {
                strReceiverIDs = intent.getStringExtra("ids");
                strReceiverNames = intent.getStringExtra("names");

                txtReceiver.setText(strReceiverNames);
            }
            else if (requestCode == FILE_RESULT) {
                Uri u = intent.getData();
                if(u == null) return;
                szSelFilePath = u.getEncodedPath();
                File fileSelected = new File(szSelFilePath);
                szSelFileName = fileSelected.getName();
                lblAddFile.setText(szSelFileName);
            }
        }

        if (requestCode == 1 && resultCode == RESULT_OK)
        {
            updateUserImage(intent);
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
                szSelFilePath = ""; szSelFileName = "tempImage" + System.currentTimeMillis() + ".jpg";
                lblAddFile.setText(szSelFileName);
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
            szSelFilePath = ""; szSelFileName = "tempImage" + System.currentTimeMillis() + ".jpg";
            lblAddFile.setText(szSelFileName);
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

    DialogInterface.OnDismissListener onDismissListener = new DialogInterface.OnDismissListener() {
        @Override
        public void onDismiss(DialogInterface dialog) {
            runOnUiThread(new Runnable() {
                @Override
                public void run() {
                    if ( (szSelFilePath.length() != 0) && (szSelFileName.length() != 0) ) {
                        lblAddFile.setText(getString(R.string.fujian) + szSelFileName);
                        bmpPhoto = null;
                    }
                    try {
                        removeDialog(DIALOG_LOAD_FILE);
                    } catch (Exception ex) {}
                }
            });
        }
    };
}
