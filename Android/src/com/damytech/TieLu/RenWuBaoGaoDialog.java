package com.damytech.TieLu;

import android.app.Dialog;
import android.content.Context;
import android.graphics.Rect;
import android.os.Bundle;
import android.view.View;
import android.view.ViewTreeObserver;
import android.view.Window;
import android.widget.Button;
import android.widget.EditText;
import android.widget.RelativeLayout;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;

public class RenWuBaoGaoDialog extends Dialog
{
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    Context mContext;

    String strContent = "";
    long mPassId = 0;
    OnDismissListener dismissListener = null;

    EditText txtContent;
    Button btnClear, btnOk;

    public RenWuBaoGaoDialog(Context context)
    {
        super(context);
        mContext = context;
    }

    @Override
    public void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        requestWindowFeature(Window.FEATURE_NO_TITLE);
        setContentView(R.layout.renwubaogao_dialog);

        mainLayout = (RelativeLayout)findViewById(R.id.parent_layout);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            if (nWidth != 0 && nHeight != 0)
                                ResolutionSet._instance.setResolution(nWidth, nHeight);
                            else
                                ResolutionSet._instance.setResolution(r.width(), r.height());

                            ResolutionSet._instance.iterateChild(findViewById(R.id.parent_layout));
                            bInitialized = true;
                        }
                    }
                }
        );

        initControl();
    }

    int nWidth = 0;
    int nHeight = 0;
    public void setSize(int width, int height)
    {
        nWidth = width;
        nHeight = height;
    }

    private void initControl()
    {
        txtContent = (EditText) findViewById(R.id.txtContent);

        btnOk = (Button) findViewById(R.id.btnOk);
        btnOk.setOnClickListener(onClickListener);
        btnClear = (Button) findViewById(R.id.btnClear);
        btnClear.setOnClickListener(onClickListener);
    }

    public void setOnDismissListener(OnDismissListener listener)
    {
        dismissListener = listener;
    }

    public String getContent()
    {
        strContent = txtContent.getText().toString();
        return strContent;
    }

    View.OnClickListener onClickListener = new View.OnClickListener()
    {
        @Override
        public void onClick(View v)
        {
            if (v.getId() == R.id.btnClear)
            {
                dismiss();
            }
            if (v.getId() == R.id.btnOk)
            {
                strContent = txtContent.getText().toString();
                if (strContent.length() == 0)
                {
                    String message = mContext.getResources().getString(R.string.shurubaogao);
                    GlobalData.showToast(mContext, message);
                    return;
                }

                dismissListener.onDismiss(RenWuBaoGaoDialog.this);
                dismiss();

                return;
            }
        }
    };
}