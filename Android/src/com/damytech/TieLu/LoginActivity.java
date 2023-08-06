package com.damytech.TieLu;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.Intent;
import android.content.res.Configuration;
import android.graphics.Rect;
import android.net.Uri;
import android.os.Bundle;
import android.util.Log;
import android.view.KeyEvent;
import android.view.View;
import android.view.ViewTreeObserver;
import android.view.inputmethod.InputMethodManager;
import android.widget.*;
import com.crashlytics.android.Crashlytics;
import com.damytech.HttpConn.JsonHttpResponseHandler;
import com.damytech.STData.STUserInfo;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;
import org.json.JSONObject;

public class LoginActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;
    private static long back_pressed;

    boolean bRemPass = false;

    private Button btnLogin = null;
    private ImageView imgRemPass = null;
    private TextView lblRemPass = null;
    private EditText txtUserName = null;
    private EditText txtPass = null;

    STUserInfo userInfo = new STUserInfo();
    private JsonHttpResponseHandler handlerLogin;
    private ProgressDialog dialog;

    private View.OnClickListener RemPassListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {
            bRemPass = !bRemPass;
            if (bRemPass)
                imgRemPass.setImageResource(R.drawable.check);
            else
                imgRemPass.setImageResource(R.drawable.uncheck);
        }
    };

    private void forceCrash() {
        throw new RuntimeException("This is a crash");
    }

    private View.OnClickListener LoginListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {
            String strUserName, strPassword;

            strUserName = txtUserName.getText().toString();
            if (strUserName == null || strUserName.length() == 0) {
                GlobalData.showToast(LoginActivity.this, getString(R.string.please_insert_userid));
                return;
            }

            strPassword = txtPass.getText().toString();
            if (strPassword == null || strPassword.length() == 0) {
                GlobalData.showToast(LoginActivity.this, getString(R.string.please_insert_password));
                return;
            }

            if (IsOfflineUser(strUserName, strPassword) == true) {
                GlobalData.SetOfflineUser(LoginActivity.this, true);
                Intent intent = new Intent(LoginActivity.this, MainMenuActivity.class);
                startActivity(intent);
            } else {
                dialog = ProgressDialog.show(
                        LoginActivity.this,
                        "",
                        getString(R.string.waiting),
                        true,
                        false,
                        null);
                CommMgr.commService.LoginUser(handlerLogin, strUserName, strPassword);
            }
        }
    };

    /**
     * Called when the activity is first created.
     */
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        Crashlytics.start(this);
        setContentView(R.layout.login_activity);

        NewsService.m_ctxMain = getApplicationContext();
        LoginActivity.this.startService(new Intent(LoginActivity.this, NewsService.class));

        mainLayout = (RelativeLayout) findViewById(R.id.rlLoginBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false) {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlLoginBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        initHandler();
    }

    private void initControl() {
        bRemPass = GlobalData.GetPassFlag(LoginActivity.this);
        imgRemPass = (ImageView) findViewById(R.id.imgLogin_RememberPassword);
        if (bRemPass)
            imgRemPass.setImageResource(R.drawable.check);
        else
            imgRemPass.setImageResource(R.drawable.uncheck);
        imgRemPass.setOnClickListener(RemPassListener);
        lblRemPass = (TextView) findViewById(R.id.lblLogin_RememberPassword);
        lblRemPass.setOnClickListener(RemPassListener);

        btnLogin = (Button) findViewById(R.id.btnLogin_Login);
        btnLogin.setOnClickListener(LoginListener);

        txtUserName = (EditText) findViewById(R.id.txtLogin_UserID);
        txtPass = (EditText) findViewById(R.id.txtLogin_Password);
        if (GlobalData.GetPassFlag(LoginActivity.this)) {
            txtUserName.setText(GlobalData.GetUserName(LoginActivity.this));
            txtPass.setText(GlobalData.GetPass(LoginActivity.this));
        } else {
            txtUserName.setText("");
            txtPass.setText("");
        }
    }

    private void initHandler() {
        handlerLogin = new JsonHttpResponseHandler() {
            int result = 0;

            @Override
            public void onSuccess(JSONObject jsonData) {
                result = 1;

                dialog.dismiss();

                CommMgr.commService.parseLoginUser(jsonData, userInfo);
                if (userInfo.grade >= 0) {
                    GlobalData.SetUid(LoginActivity.this, userInfo.uid);
                    GlobalData.SetGrade(LoginActivity.this, userInfo.grade);
                    GlobalData.SetOfflineUser(LoginActivity.this, false);

                    if (bRemPass == true) {
                        GlobalData.SetPassFlag(LoginActivity.this, bRemPass);
                        GlobalData.SetUserName(LoginActivity.this, txtUserName.getText().toString());
                        GlobalData.SetPass(LoginActivity.this, txtPass.getText().toString());
                    } else
                        GlobalData.SetPassFlag(LoginActivity.this, false);

                    Intent intent = new Intent(LoginActivity.this, MainMenuActivity.class);
                    startActivity(intent);
                } else if (userInfo.grade == -1)
                    result = -1;
            }

            @Override
            public void onFailure(Throwable ex, String exception) {
                result = 0;
                dialog.dismiss();
            }

            @Override
            public void onFinish() {
                if (result == -1)
                    GlobalData.showToast(LoginActivity.this, userInfo.gradename);
                else if (result == 0)
                    GlobalData.showToast(LoginActivity.this, getString(R.string.network_error));
            }
        };

        return;
    }

    @Override
    public void onResume() {
        super.onResume();

        initControl();
    }

    @Override
    public void onBackPressed() {
        if (back_pressed + 2000 > System.currentTimeMillis()) {
            super.onBackPressed();
        } else {
            GlobalData.showToast(LoginActivity.this, getString(R.string.exitapp));
            back_pressed = System.currentTimeMillis();
        }
    }

    private boolean IsOfflineUser(String username, String password) {
        if (username.equals("lixian") && password.equals("123456"))
            return true;

        return false;
    }
}
