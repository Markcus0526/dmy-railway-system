package com.damytech.TieLu;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Intent;
import android.graphics.Color;
import android.graphics.Rect;
import android.os.Bundle;
import android.os.Handler;
import android.os.Message;
import android.text.TextUtils;
import android.util.TypedValue;
import android.view.*;
import android.widget.*;
import com.damytech.HttpConn.JsonHttpResponseHandler;
import com.damytech.STData.STKaoShi;
import com.damytech.STData.STKaoShiItem;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.Timer;
import java.util.TimerTask;

public class KaoShiWenTiActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    private ImageView imgBack = null;
    private TextView lblShiJianVal = null;
    private TextView m_lblTitle = null;
    private TextView m_lblProblem = null;
    private ScrollView m_scvWentiData = null;
    private Button btnShangYiTi = null;
    private Button btnXiaYiTi = null;

    private ArrayList<STKaoShi> arrData = new ArrayList<STKaoShi>();
    private ArrayList<ArrayList<String>> arrAnswer = null;
    private ArrayList<String> g_arrAns = new ArrayList<String>();
    private JsonHttpResponseHandler handler = null;
    private ProgressDialog dialog = null;

    // the problem index in focus.
    // if this index is
    //     -1: test not started yet
    //     total number of problems: test finished
    //     or else: testing now...
    private int m_nCurProblem = -1;

    private View m_viewCurProblem = null;

    private final static int ITEM_X_PAD = 40;
    private final static int ITEM_Y_PAD = 10;
    private final static int ITEM_H = 40;

    private final static int RADIO_GROUP_ID = 100000;
    private final static int ITEM_ID_START = 100001;

    private STKaoShiItem m_stKaoShiItem = null;

    private String g_strProblem = "";

    Timer m_timer = null;
    private int m_nTimeRemained = -1;
    TimerTask m_timerTask = new TimerTask() {
        @Override
        public void run() {
            if ( m_nTimeRemained > 0 ) {
                m_nTimeRemained --;
                messageHandler.sendEmptyMessage(0);
            } else {
                messageHandler.sendEmptyMessage(1);
            }
        }
    };

    private Handler messageHandler = new Handler() {
        public void handleMessage(Message msg) {
            super.handleMessage(msg);

            if ( msg.what == 0 ) {
                refreshTimer();
            } else if ( msg.what == 1 ) {
                // time is out
                m_timer.cancel();
                m_timerTask.cancel();
                GlobalData.showToast(KaoShiWenTiActivity.this, getString(R.string.examtimeout));
                Intent intent = new Intent(KaoShiWenTiActivity.this, KaoShiKaoHeActivity.class);
                intent.putExtra("AllTime", m_stKaoShiItem.examtime * 60);
                intent.putExtra("Time", m_nTimeRemained);
                intent.putExtra("Problem", g_strProblem);
                intent.putStringArrayListExtra("Answer", g_arrAns);
                intent.putExtra("ProNo", m_stKaoShiItem.uid);
                startActivity(intent);
            }
        }
    };

    private String secondsToTimeStr(int i_nSeconds) {
        int hour=0, min=0, sec=i_nSeconds;
        String szRet;

        if ( sec >= 60 ) {
            min = sec/60;
            sec = sec - min*60;
        }

        if ( min >= 60 ) {
            hour = min/60;
            min = min - hour*60;
        }

        if ( hour == 0 ) {
            szRet = String.format("%02d:%02d", min, sec);
        } else {
            szRet = String.format("%02d:%02d:%02d", hour, min, sec);
        }

        return szRet;
    }

    private void refreshTimer() {
        String szTimeRemained = secondsToTimeStr(m_nTimeRemained);
        lblShiJianVal.setText(szTimeRemained);
    }

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgKaoShiWenTi_Back:
                    finish();
                    break;

                case R.id.btnKaoShiWenTi_ShangYiTi:
                    if ( m_nCurProblem > 0 )
                    {
                        // save the current answer
                        setCurrentAnswer(calcCurrentAnswer());

                        // show the previous problem
                        m_nCurProblem --;
                        showCurrentProblem();
                    }
                    break;

                case R.id.btnKaoShiWenTi_XiaYiTi:
                    if ( m_nCurProblem < arrData.size() ) {
                        // save the current answer
                        if (setCurrentAnswer(calcCurrentAnswer()) == false)
                        {
                            GlobalData.showToast(KaoShiWenTiActivity.this, getString(R.string.selectanswer));
                            return;
                        }

                        // show the next problem
                        m_nCurProblem ++;
                        if ( m_nCurProblem == arrData.size() )
                        {
                            m_timer.cancel();
                            m_timerTask.cancel();

                            if (arrAnswer != null && arrAnswer.size() > 0)
                            {
                                for (int index = 0; index < arrAnswer.size(); index++)
                                {
                                    String strVal = "";
                                    for (int i = 0; i < arrAnswer.get(index).size(); i++)
                                    {
                                        if ( i == 0 )
                                        {
                                            strVal = arrAnswer.get(index).get(i).toString();
                                        }
                                        else
                                        {
                                            strVal += "," + arrAnswer.get(index).get(i).toString();
                                        }
                                    }

                                    g_arrAns.add(strVal);
                                }
                            }

                            Intent intent = new Intent(KaoShiWenTiActivity.this, KaoShiKaoHeActivity.class);
                            intent.putExtra("AllTime", m_stKaoShiItem.examtime * 60);
                            intent.putExtra("Time", m_nTimeRemained);
                            intent.putExtra("Problem", g_strProblem);
                            intent.putStringArrayListExtra("Answer", g_arrAns);
                            intent.putExtra("ProNo", m_stKaoShiItem.uid);
                            startActivity(intent);
                        }
                        else
                        {
                            showCurrentProblem();
                        }
                    }
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
        setContentView(R.layout.kaoshiwenti_activity);

        m_stKaoShiItem = getIntent().getParcelableExtra("Data");
        m_nTimeRemained = m_stKaoShiItem.examtime * 60; // seconds

        mainLayout = (RelativeLayout)findViewById(R.id.rlKaoShiWenTiBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlKaoShiWenTiBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        initControl();
        initHandler();

        dialog = ProgressDialog.show(
                KaoShiWenTiActivity.this,
                "",
                getString(R.string.waiting),
                true,
                false,
                null
        );
        CommMgr.commService.GetKaoShiList(handler, Long.toString(m_stKaoShiItem.uid));
    }

    private void initControl()
    {
        imgBack = (ImageView) findViewById(R.id.imgKaoShiWenTi_Back);
        imgBack.setOnClickListener(onClickListener);

        lblShiJianVal = (TextView) findViewById(R.id.lblKaoShiWenTi_ShengXiaShiJianVal);
        m_lblTitle = (TextView) findViewById(R.id.lblKaoShiWenTi_Title);
        m_lblProblem = (TextView) findViewById(R.id.lblKaoShiWenTi_Problem);
        m_scvWentiData = (ScrollView) findViewById(R.id.scvKaoShiWenTi_WenTiData);
        btnShangYiTi = (Button) findViewById(R.id.btnKaoShiWenTi_ShangYiTi);
        btnShangYiTi.setOnClickListener(onClickListener);
        btnXiaYiTi = (Button) findViewById(R.id.btnKaoShiWenTi_XiaYiTi);
        btnXiaYiTi.setOnClickListener(onClickListener);
    }

    private void initHandler()
    {
        handler = new JsonHttpResponseHandler()
        {
            int result = 0;

            @Override
            public void onSuccess(JSONObject jsonData)
            {
                result = 1;

                g_strProblem = jsonData.toString();

                CommMgr.commService.parseGetKaoShiList(jsonData, arrData);
                if (arrData == null)
                    result = -1;
            }

            @Override
            public void onFailure(Throwable ex, String exception)
            {
                result = 0;
            }

            @Override
            public void onFinish()
            {
                dialog.dismiss();

                if (result == 1)
                {
                    // first show
                    arrAnswer = new ArrayList<ArrayList<String>>();
                    for ( int i=0; i<arrData.size(); i++ ) {
                        ArrayList<String> newArray = new ArrayList<String>();
                        arrAnswer.add(newArray);
                    }

                    m_nCurProblem = 0;
                    showCurrentProblem();

                    m_timer = new Timer();
                    m_timer.schedule(m_timerTask, 0, 1000); // no delay, 1 sec interval
                }

                if (result == -1)
                    GlobalData.showToast(KaoShiWenTiActivity.this, getString(R.string.network_error));
            }
        };
    }

    // Display the problem text and buttons based on the current problem number.
    public void showCurrentProblem () {
        int nProblemCount = arrData.size();

        btnShangYiTi.setVisibility(View.INVISIBLE);
        btnXiaYiTi.setText(getString(R.string.xiayiti));

        // if this is the first problem, do not show 'prev' problem.
        if ( m_nCurProblem > 0 ) {
            btnShangYiTi.setVisibility(View.VISIBLE);
        }

        // if this is the last problem, change the button label to 'TiJiao'.
        if ( m_nCurProblem == nProblemCount-1 ) {
            btnXiaYiTi.setText(getString(R.string.tijiao));
        }
        
        // remove the current problem layout
        if ( m_viewCurProblem != null ) {
            m_scvWentiData.removeView(m_viewCurProblem);
            m_viewCurProblem = null;
        }

        STKaoShi stKaoShi = arrData.get(m_nCurProblem);

        // change title
        m_lblTitle.setText(String.format("%s %d/%d", m_stKaoShiItem.title, m_nCurProblem+1, nProblemCount));

        // show problem
        m_lblProblem.setText(stKaoShi.title);

        // show user options
        switch ( stKaoShi.getType() ) {
            case STKaoShi.KAOSHI_TYPE_DANXUAN:
                m_viewCurProblem = buildDanXianProblemUI(stKaoShi.questions);
                break;

            case STKaoShi.KAOSHI_TYPE_DUOXUAN:
                m_viewCurProblem = buildDuoXianProblemUI(stKaoShi.questions);
                break;

            case STKaoShi.KAOSHI_TYPE_PANDUAN:
                m_viewCurProblem = buildPanDuanProblemUI();
                break;
        }

        if ( m_viewCurProblem != null ) {
            m_scvWentiData.removeAllViews();
            m_scvWentiData.addView(m_viewCurProblem);
            FrameLayout.LayoutParams layoutParams = new FrameLayout.LayoutParams(FrameLayout.LayoutParams.MATCH_PARENT, calculateCurProblemHeight());
            m_viewCurProblem.setLayoutParams(layoutParams);
        }

        // show the previous answer
        ArrayList<String> answer = getCurrentAnswer();
        boolean bHaveAnswer = ((answer!=null) && (answer.size()>0));

        if ( bHaveAnswer == false ) {
            return;
        }

        switch ( stKaoShi.getType() ) {
            case STKaoShi.KAOSHI_TYPE_DANXUAN:
            {
                int resid = ITEM_ID_START;
                for (int i = 0; i < arrData.get(m_nCurProblem).questions.size(); i++)
                {
                    if (arrData.get(m_nCurProblem).questions.get(i).ind.equals(answer.get(0)))
                        resid = ITEM_ID_START + i;
                }
                RadioGroup radioGroup = (RadioGroup) m_scvWentiData.findViewById(RADIO_GROUP_ID);
                radioGroup.check(resid);
                break;
            }

            case STKaoShi.KAOSHI_TYPE_DUOXUAN:
                for ( int i=0; i<answer.size(); i++ ) {
                    int resid = ITEM_ID_START;
                    for (int j = 0; j < arrData.get(m_nCurProblem).questions.size(); j++)
                    {
                        if (arrData.get(m_nCurProblem).questions.get(j).ind.equals(answer.get(i)))
                            resid = ITEM_ID_START + j;
                    }
                    CheckBox checkBox = (CheckBox) m_scvWentiData.findViewById(resid);
                    checkBox.setMaxLines(1);
                    checkBox.setEllipsize(TextUtils.TruncateAt.END);
                    checkBox.setChecked(true);
                }
                break;

            case STKaoShi.KAOSHI_TYPE_PANDUAN:
            {
                int resid = (answer.get(0).equals("0")) ? ITEM_ID_START+1 : ITEM_ID_START;
                RadioGroup radioGroup = (RadioGroup) m_scvWentiData.findViewById(RADIO_GROUP_ID);
                radioGroup.check(resid);
                break;
            }
        }
    }

    private View buildDanXianProblemUI (ArrayList<STKaoShi.STQuestion> questions) {
        int nQuestionCount = questions.size();

        RelativeLayout newView = new RelativeLayout(m_scvWentiData.getContext());
        newView.setBackgroundColor(Color.WHITE);

        RadioGroup radioGroup = new RadioGroup(m_scvWentiData.getContext());
        radioGroup.setOrientation(RadioGroup.VERTICAL);
        radioGroup.setId(RADIO_GROUP_ID);
        newView.addView(radioGroup);

        RelativeLayout.LayoutParams params = (RelativeLayout.LayoutParams)radioGroup.getLayoutParams();
        params.addRule(RelativeLayout.ALIGN_PARENT_LEFT);
        params.addRule(RelativeLayout.ALIGN_PARENT_TOP);
        radioGroup.setLayoutParams(params);
        radioGroup.setX(ITEM_X_PAD);
        radioGroup.setY(ITEM_Y_PAD);

        for ( int i=0; i<nQuestionCount; i++ ) {
            RadioButton rbtnQuestion = new RadioButton(newView.getContext());
            rbtnQuestion.setGravity(Gravity.CENTER_VERTICAL);
            rbtnQuestion.setMaxLines(1);
            rbtnQuestion.setEllipsize(TextUtils.TruncateAt.END);
            rbtnQuestion.setText(questions.get(i).ind + "。" + questions.get(i).question);
            rbtnQuestion.setTextColor(Color.BLACK);
            rbtnQuestion.setTextSize(TypedValue.COMPLEX_UNIT_PX, 26);
            rbtnQuestion.setId(ITEM_ID_START+i);
            radioGroup.addView(rbtnQuestion);

            // set the space between icon and text
            float scale = this.getResources().getDisplayMetrics().density;
            rbtnQuestion.setPadding(rbtnQuestion.getPaddingLeft() + (int)(15.0f * scale + 0.5f),
                    rbtnQuestion.getPaddingTop(),
                    rbtnQuestion.getPaddingRight(),
                    rbtnQuestion.getPaddingBottom());
        }

        //radioGroup.check(ITEM_ID_START);

        ResolutionSet._instance.iterateChild(newView);

        return newView;
    }

    private View buildDuoXianProblemUI (ArrayList<STKaoShi.STQuestion> questions) {
        int nQuestionCount = questions.size();

        RelativeLayout newView = new RelativeLayout(m_scvWentiData.getContext());
        newView.setBackgroundColor(Color.WHITE);

        for ( int i=0; i<nQuestionCount; i++ ) {
            CheckBox chkQuestion = new CheckBox(newView.getContext());
            chkQuestion.setGravity(Gravity.CENTER_VERTICAL);
            chkQuestion.setText(questions.get(i).ind + "。" + questions.get(i).question);
            chkQuestion.setTextColor(Color.BLACK);
            chkQuestion.setTextSize(TypedValue.COMPLEX_UNIT_PX, 26);
            chkQuestion.setId(ITEM_ID_START+i);
            chkQuestion.setMaxLines(1);
            chkQuestion.setEllipsize(TextUtils.TruncateAt.END);
            newView.addView(chkQuestion);

            RelativeLayout.LayoutParams params = (RelativeLayout.LayoutParams)chkQuestion.getLayoutParams();
            params.addRule(RelativeLayout.ALIGN_PARENT_LEFT);
            params.addRule(RelativeLayout.ALIGN_PARENT_TOP);
            chkQuestion.setLayoutParams(params);
            chkQuestion.setX(ITEM_X_PAD);
            chkQuestion.setY((i+1)*ITEM_Y_PAD + i*ITEM_H);

            // set the space between icon and text
            float scale = this.getResources().getDisplayMetrics().density;
            chkQuestion.setPadding(chkQuestion.getPaddingLeft() + (int)(15.0f * scale + 0.5f),
                    chkQuestion.getPaddingTop(),
                    chkQuestion.getPaddingRight(),
                    chkQuestion.getPaddingBottom());
        }

        ResolutionSet._instance.iterateChild(newView);

        return newView;
    }

    private View buildPanDuanProblemUI () {
        RelativeLayout newView = new RelativeLayout(m_scvWentiData.getContext());
        newView.setBackgroundColor(Color.WHITE);

        RadioGroup radioGroup = new RadioGroup(m_scvWentiData.getContext());
        radioGroup.setOrientation(RadioGroup.VERTICAL);
        radioGroup.setId(RADIO_GROUP_ID);
        newView.addView(radioGroup);

        RelativeLayout.LayoutParams params = (RelativeLayout.LayoutParams)radioGroup.getLayoutParams();
        params.addRule(RelativeLayout.ALIGN_PARENT_LEFT);
        params.addRule(RelativeLayout.ALIGN_PARENT_TOP);
        radioGroup.setLayoutParams(params);
        radioGroup.setX(ITEM_X_PAD);
        radioGroup.setY(ITEM_Y_PAD);

        RadioButton rbtnTrue = new RadioButton(newView.getContext());
        rbtnTrue.setGravity(Gravity.CENTER_VERTICAL);
        rbtnTrue.setMaxLines(1);
        rbtnTrue.setEllipsize(TextUtils.TruncateAt.END);
        rbtnTrue.setText("是");
        rbtnTrue.setTextColor(Color.BLACK);
        rbtnTrue.setId(ITEM_ID_START);
        rbtnTrue.setTextSize(TypedValue.COMPLEX_UNIT_PX, 26);
        radioGroup.addView(rbtnTrue);

        // set the margin between icon and text
        float scale = this.getResources().getDisplayMetrics().density;
        rbtnTrue.setPadding(rbtnTrue.getPaddingLeft() + (int)(15.0f * scale + 0.5f),
                rbtnTrue.getPaddingTop(),
                rbtnTrue.getPaddingRight(),
                rbtnTrue.getPaddingBottom());

        RadioButton rbtnFalse = new RadioButton(newView.getContext());
        rbtnFalse.setMaxLines(1);
        rbtnFalse.setEllipsize(TextUtils.TruncateAt.END);
        rbtnFalse.setText("否");
        rbtnFalse.setTextColor(Color.BLACK);
        rbtnFalse.setGravity(Gravity.CENTER_VERTICAL);
        rbtnFalse.setId(ITEM_ID_START+1);
        rbtnFalse.setTextSize(TypedValue.COMPLEX_UNIT_PX, 26);
        radioGroup.addView(rbtnFalse);

        // set the margin between icon and text
        scale = this.getResources().getDisplayMetrics().density;
        rbtnFalse.setPadding(rbtnFalse.getPaddingLeft() + (int)(15.0f * scale + 0.5f),
                rbtnFalse.getPaddingTop(),
                rbtnFalse.getPaddingRight(),
                rbtnFalse.getPaddingBottom());

        //radioGroup.check(ITEM_ID_START);

        ResolutionSet._instance.iterateChild(newView);

        return newView;
    }

    private int calculateCurProblemHeight () {
        STKaoShi stKaoShi = arrData.get(m_nCurProblem);

        if ( stKaoShi == null ) {
            return 0;
        }

        int nProblemCount = stKaoShi.questions.size();

        switch ( stKaoShi.getType() ) {
            case STKaoShi.KAOSHI_TYPE_PANDUAN:
                return (int)(2 * (ITEM_H+ITEM_Y_PAD) * ResolutionSet.fYpro);
            case STKaoShi.KAOSHI_TYPE_DANXUAN:
                return (int)(nProblemCount * (ITEM_H+ITEM_Y_PAD) * ResolutionSet.fYpro);
            case STKaoShi.KAOSHI_TYPE_DUOXUAN:
                return (int)((nProblemCount*(ITEM_H+ITEM_Y_PAD) + ITEM_Y_PAD) * ResolutionSet.fYpro + 300);
        }

        return 0;
    }

    private ArrayList<String> getCurrentAnswer() {
        if ( arrAnswer == null ) {
            return null;
        }

        return arrAnswer.get(m_nCurProblem);
    }

    private boolean setCurrentAnswer(ArrayList<String> answer) {
        if (answer == null)
            return false;

        if ( arrAnswer == null ) {
            return false;
        }

        arrAnswer.set(m_nCurProblem, answer);

        return true;
    }

    private ArrayList<String> calcCurrentAnswer() {
        ArrayList<String> newAnswer = new ArrayList<String>();

        STKaoShi stKaoShi = arrData.get(m_nCurProblem);
        int nProblemCount = stKaoShi.questions.size();
        if ( stKaoShi.getType() == STKaoShi.KAOSHI_TYPE_PANDUAN ) {
            nProblemCount = 2;
        }

        switch ( stKaoShi.getType() ) {
            case STKaoShi.KAOSHI_TYPE_DUOXUAN:
                for ( int i=0; i<nProblemCount; i++ ) {
                    CheckBox checkBox = (CheckBox) m_scvWentiData.findViewById(ITEM_ID_START+i);
                    checkBox.setMaxLines(1);
                    checkBox.setEllipsize(TextUtils.TruncateAt.END);
                    if ( checkBox.isChecked() ) {
                        newAnswer.add(arrData.get(m_nCurProblem).questions.get(i).ind);
                    }
                }
                break;

            case STKaoShi.KAOSHI_TYPE_PANDUAN:
            {
                RadioGroup radioGroup = (RadioGroup) m_scvWentiData.findViewById(RADIO_GROUP_ID);
                int nCheckedID = radioGroup.getCheckedRadioButtonId();
                newAnswer.add((nCheckedID==ITEM_ID_START)?"1":"0");
                break;
            }

            case STKaoShi.KAOSHI_TYPE_DANXUAN:
            {
                try {
                    RadioGroup radioGroup = (RadioGroup) m_scvWentiData.findViewById(RADIO_GROUP_ID);
                    int nCheckedID = radioGroup.getCheckedRadioButtonId();
                    newAnswer.add(arrData.get(m_nCurProblem).questions.get(nCheckedID - ITEM_ID_START).ind);
                }catch (Exception ex) {
                    return null;
                }
                break;
            }
        }

        return newAnswer;
    }

    @Override
    public boolean onKeyDown(int keyCode, KeyEvent event)
    {
        try
        {
            m_timer.cancel();
            m_timerTask.cancel();
        } catch (Exception ex) {}

        return super.onKeyDown(keyCode, event);
    }
}
