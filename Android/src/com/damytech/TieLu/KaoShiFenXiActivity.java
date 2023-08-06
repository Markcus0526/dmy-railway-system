package com.damytech.TieLu;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.graphics.Rect;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.view.ViewTreeObserver;
import android.widget.*;
import com.damytech.STData.STKaoHeJiLu;
import com.damytech.STData.STKaoShi;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;

public class KaoShiFenXiActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    ArrayList<STKSCuoWu> arrProblemAnswer = new ArrayList<STKSCuoWu>();
    private ItemAdapter adapter = null;

    private ImageView imgBack = null;
    private ListView listData = null;

    private String g_strProblem = "";
    private ArrayList<STKaoShi> arrData = new ArrayList<STKaoShi>();
    private ArrayList<String> g_arrAns = new ArrayList<String>();

    View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {

            switch (v.getId())
            {
                case R.id.imgKaoShiFenXi_Back:
                    finish();
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
        setContentView(R.layout.kaoshifenxi_activity);

        mainLayout = (RelativeLayout)findViewById(R.id.rlKaoShiFenXiBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlKaoShiFenXiBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        g_strProblem = getIntent().getStringExtra("Problem");
        g_arrAns = getIntent().getStringArrayListExtra("Answer");
        if (g_strProblem == null || g_strProblem.length() == 0 || g_arrAns == null)
        {
            GlobalData.showToast(KaoShiFenXiActivity.this, getString(R.string.dataerror));
            Intent intent = new Intent(KaoShiFenXiActivity.this, MainMenuActivity.class);
            intent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP | Intent.FLAG_ACTIVITY_NEW_TASK);
            startActivity(intent);
        }

        try {
            JSONObject jsonObject = new JSONObject(g_strProblem);
            CommMgr.commService.parseGetKaoShiList(jsonObject, arrData);
        } catch (JSONException e) {}

        initControl();

        parseData();

        adapter = new ItemAdapter(KaoShiFenXiActivity.this, 0, arrProblemAnswer);
        listData.setAdapter(adapter);
    }

    private void initControl()
    {
        imgBack = (ImageView) findViewById(R.id.imgKaoShiFenXi_Back);
        imgBack.setOnClickListener(onClickListener);

        listData = (ListView) findViewById(R.id.listKaoShiFenXi_Data);
    }

    private void parseData()
    {
        for (int i = 0; i < arrData.size(); i++)
        {
            if (g_arrAns.get(i) != null)
            {
                if (arrData.get(i).getType() == STKaoShi.KAOSHI_TYPE_DANXUAN)
                {
                    if (arrData.get(i).answers.get(0).equals(g_arrAns.get(i)) == false)
                    {
                        STKSCuoWu newItem = new STKSCuoWu();
                        newItem.wenti = getString(R.string.kaoti1) + Integer.toString((i+1)) + " : " + arrData.get(i).title;
                        for (int k = 0; k < arrData.get(i).questions.size(); k++)
                        {
                            if (arrData.get(i).questions.get(k).ind.equals(arrData.get(i).answers.get(0)))
                            {
                                newItem.trueHuiDa = arrData.get(i).questions.get(k).question;
                                break;
                            }
                        }

                        for (int k = 0; k < arrData.get(i).questions.size(); k++)
                        {
                            if (arrData.get(i).questions.get(k).ind.equals(g_arrAns.get(i)))
                            {
                                newItem.falseHuiDa = arrData.get(i).questions.get(k).question;
                                break;
                            }
                        }

                        arrProblemAnswer.add(newItem);
                    }
                }
                if (arrData.get(i).getType() == STKaoShi.KAOSHI_TYPE_DUOXUAN)
                {
                    String strAns = "";
                    for (int j = 0; j < arrData.get(i).answers.size(); j++)
                    {
                        if (j == 0)
                            strAns = arrData.get(i).answers.get(j).toString();
                        else
                            strAns = strAns + "," + arrData.get(i).answers.get(j).toString();
                    }
                    if (strAns.equals(g_arrAns.get(i)) == false)
                    {
                        STKSCuoWu newItem = new STKSCuoWu();
                        newItem.wenti = getString(R.string.kaoti1) + Integer.toString((i+1)) + " : " + arrData.get(i).title;

                        String strTrueData = "";
                        for (int j = 0; j < arrData.get(i).answers.size(); j++)
                        {
                            for (int k = 0; k < arrData.get(i).questions.size(); k++)
                            {
                                if (arrData.get(i).answers.get(j).equals(arrData.get(i).questions.get(k).ind))
                                {
                                    if (j == 0)
                                        strTrueData = arrData.get(i).questions.get(k).question;
                                    else
                                        strTrueData = strTrueData + "," + arrData.get(i).questions.get(k).question;
                                }
                            }
                        }
                        newItem.trueHuiDa = strTrueData;

                        String strFalseData = "";
                        String strAnswerData = g_arrAns.get(i) + ",";
                        while ( strAnswerData.indexOf(',') != -1)
                        {
                            int nPos = strAnswerData.indexOf(',');
                            String strIndex = strAnswerData.substring(0, nPos);
                            strAnswerData = strAnswerData.substring(nPos+1);

                            for (int k = 0; k < arrData.get(i).questions.size(); k++)
                            {
                                if (strIndex.equals(arrData.get(i).questions.get(k).ind))
                                {
                                    strFalseData = strFalseData + "," + arrData.get(i).questions.get(k).question;
                                    break;
                                }
                            }
                        }
                        newItem.falseHuiDa = (strFalseData.length() == 0?"":strFalseData.substring(1));

                        arrProblemAnswer.add(newItem);
                    }
                }
                if (arrData.get(i).getType() == STKaoShi.KAOSHI_TYPE_PANDUAN)
                {
                    if (arrData.get(i).answers.get(0).equals(g_arrAns.get(i)) == false)
                    {
                        STKSCuoWu newItem = new STKSCuoWu();
                        newItem.wenti = getString(R.string.kaoti1) + Integer.toString((i+1)) + " : " + arrData.get(i).title;
                        if (g_arrAns.get(0).equals("0"))
                        {
                            newItem.trueHuiDa = "是";
                            newItem.falseHuiDa = "否";
                        }
                        else
                        {
                            newItem.trueHuiDa = "否";
                            newItem.falseHuiDa = "是";
                        }

                        arrProblemAnswer.add(newItem);
                    }
                }
            }
            else
            {
                if (arrData.get(i).getType() == STKaoShi.KAOSHI_TYPE_DANXUAN)
                {
                    if (arrData.get(i).answers.get(0).equals(g_arrAns.get(i)) == false)
                    {
                        STKSCuoWu newItem = new STKSCuoWu();
                        newItem.wenti = getString(R.string.kaoti1) + Integer.toString((i+1)) + " : " + arrData.get(i).title;
                        newItem.trueHuiDa = arrData.get(i).answers.get(0);
                        newItem.falseHuiDa = "";

                        arrProblemAnswer.add(newItem);
                    }
                }
                if (arrData.get(i).getType() == STKaoShi.KAOSHI_TYPE_DUOXUAN)
                {
                    String strAns = "";
                    for (int j = 0; j < arrData.get(i).answers.size(); j++)
                    {
                        if (j == 0)
                            strAns = arrData.get(i).answers.get(j).toString();
                        else
                            strAns = strAns + "," + arrData.get(i).answers.get(j).toString();
                    }
                    if (strAns.equals(g_arrAns.get(i)) == false)
                    {
                        STKSCuoWu newItem = new STKSCuoWu();
                        newItem.wenti = getString(R.string.kaoti1) + Integer.toString((i+1)) + " : " + arrData.get(i).title;

                        String strTrueData = "";
                        for (int j = 0; j < arrData.get(i).answers.size(); j++)
                        {
                            for (int k = 0; k < arrData.get(i).questions.size(); k++)
                            {
                                if (arrData.get(i).answers.get(j).equals(arrData.get(i).questions.get(k).ind))
                                {
                                    if (j == 0)
                                        strTrueData = arrData.get(i).questions.get(k).question;
                                    else
                                        strTrueData = strTrueData + "," + arrData.get(i).questions.get(k).question;
                                }
                            }
                        }
                        newItem.trueHuiDa = strTrueData;
                        newItem.falseHuiDa = "";

                        arrProblemAnswer.add(newItem);
                    }
                }
                if (arrData.get(i).getType() == STKaoShi.KAOSHI_TYPE_PANDUAN)
                {
                    if (arrData.get(i).answers.get(0).equals(g_arrAns.get(i)) == false)
                    {
                        STKSCuoWu newItem = new STKSCuoWu();
                        newItem.wenti = getString(R.string.kaoti1) + Integer.toString((i+1)) + " : " + arrData.get(i).title;
                        newItem.trueHuiDa = arrData.get(i).answers.get(0);
                        newItem.falseHuiDa = "";

                        arrProblemAnswer.add(newItem);

                    }
                }
            }
        }

        return;
    }

    public class STKSCuoWu
    {
        public String wenti = "";
        public String trueHuiDa = "";
        public String falseHuiDa = "";
    }

    public class ItemAdapter extends ArrayAdapter<STKSCuoWu>
    {
        Context ctx;
        ArrayList<STKSCuoWu> list = new ArrayList<STKSCuoWu>();

        public ItemAdapter(Context ctx, int resourceId, ArrayList<STKSCuoWu> list) {
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
                v = inflater.inflate(R.layout.kaoshifenxi_view, null);
                ResolutionSet._instance.iterateChild(v.findViewById(R.id.rlKaoShiFenXiViewBack));
            }

            TextView lblTitle = (TextView) v.findViewById(R.id.lblKaoShiFenXiView_WenTi);
            lblTitle.setText(list.get(position).wenti);
            TextView lblTrueAns = (TextView) v.findViewById(R.id.lblKaoShiFenXiView_DaAn);
            lblTrueAns.setText( getString(R.string.zhengquedaan) + list.get(position).trueHuiDa);
            TextView lblFalseAns = (TextView) v.findViewById(R.id.lblKaoShiFenXiView_NinDeDaAn);
            lblFalseAns.setText( getString(R.string.nindedaan) +  list.get(position).falseHuiDa);

            return v;
        }
    }
}
