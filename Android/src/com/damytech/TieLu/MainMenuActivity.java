package com.damytech.TieLu;

import android.app.Activity;
import android.content.Intent;
import android.graphics.Rect;
import android.os.Bundle;
import android.view.View;
import android.view.ViewTreeObserver;
import android.widget.RelativeLayout;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;

public class MainMenuActivity extends Activity {
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    private RelativeLayout rlKaoShi = null;
    private RelativeLayout rlKaoHeChaXun = null;
    private RelativeLayout rlGanBuKaoHe = null;
    private RelativeLayout rlGongWenLiuZhuan = null;
    private RelativeLayout rlRenWu = null;
    private RelativeLayout rlGuiZhang = null;
    private RelativeLayout rlTongXun = null;
    private RelativeLayout rlYouXiang = null;
    private RelativeLayout rlSuQiu = null;
    private RelativeLayout rlJiFen = null;
    private RelativeLayout rlZiKongLv = null;

    private View.OnClickListener onClickListener = new View.OnClickListener() {
        @Override
        public void onClick(View v) {
            switch (v.getId())
            {
                case R.id.rlMainMenu_GanBuKaoHe:
                    if (GlobalData.GetOfflineUser(MainMenuActivity.this))
                    {
                        GlobalData.showToast(MainMenuActivity.this, getString(R.string.offlineuser));
                        return;
                    }
                    else
                    {
                        if (GlobalData.GetGrade(MainMenuActivity.this) == -1 || GlobalData.GetGrade(MainMenuActivity.this) == 2) {
                            GlobalData.showToast(MainMenuActivity.this, getString(R.string.noauthoriry));
                            return;
                        }
                        Intent intent = new Intent(MainMenuActivity.this, KaoHeLuRuActivity.class);
                        startActivity(intent);
                    }
                    break;
                case R.id.rlMainMenu_GongWenLiuZhuan:
                    if (GlobalData.GetOfflineUser(MainMenuActivity.this))
                    {
                        GlobalData.showToast(MainMenuActivity.this, getString(R.string.offlineuser));
                        return;
                    }
                    else {
                        if (GlobalData.GetGrade(MainMenuActivity.this) == -1 || GlobalData.GetGrade(MainMenuActivity.this) == 2) {
                            GlobalData.showToast(MainMenuActivity.this, getString(R.string.noauthoriry));
                            return;
                        }
                        Intent intentDaiQian = new Intent(MainMenuActivity.this, GongWenDaiQianActivity.class);
                        startActivity(intentDaiQian);
                    }
                    break;
                case R.id.rlMainMenu_RenWuGuanLi:
                    if (GlobalData.GetOfflineUser(MainMenuActivity.this))
                    {
                        GlobalData.showToast(MainMenuActivity.this, getString(R.string.offlineuser));
                        return;
                    }
                    else {
                        Intent intentRenWu = new Intent(MainMenuActivity.this, RenWuGuanLiActivity.class);
                        startActivity(intentRenWu);
                    }
                    break;
                case R.id.rlMainMenu_GuiZhangChaXun:
                    Intent intentGuiZhang = new Intent(MainMenuActivity.this, GuiZhangActivity.class);
                    startActivity(intentGuiZhang);
                    break;
                case R.id.rlMainMenu_TongXunLu:
                    if (GlobalData.GetOfflineUser(MainMenuActivity.this))
                    {
                        GlobalData.showToast(MainMenuActivity.this, getString(R.string.offlineuser));
                        return;
                    }
                    else {
                        Intent intentTongXunLu = new Intent(MainMenuActivity.this, TongXunLuActivity.class);
                        startActivity(intentTongXunLu);
                    }
                    break;
                case R.id.rlMainMenu_YouXiangChaXun:
                    if (GlobalData.GetOfflineUser(MainMenuActivity.this))
                    {
                        GlobalData.showToast(MainMenuActivity.this, getString(R.string.offlineuser));
                        return;
                    }
                    else {
                        Intent intentYouXiang = new Intent(MainMenuActivity.this, YouXiangActivity.class);
                        startActivity(intentYouXiang);
                    }
                    break;
				case R.id.rlMainMenu_ZaiXianKaoShi: // 在线考试
                    if (GlobalData.GetOfflineUser(MainMenuActivity.this))
                    {
                        GlobalData.showToast(MainMenuActivity.this, getString(R.string.offlineuser));
                        return;
                    }
                    else {
                        Intent intentKaoShi = new Intent(MainMenuActivity.this, KaoShiDuanJiActivity.class);
                        startActivity(intentKaoShi);
                    }
                    break;
                case R.id.rlMainMenu_KaoHeChaXun:
                    if (GlobalData.GetOfflineUser(MainMenuActivity.this))
                    {
                        GlobalData.showToast(MainMenuActivity.this, getString(R.string.offlineuser));
                        return;
                    }
                    else {
                        Intent intentKaoHeChaXun = new Intent(MainMenuActivity.this, KaoHeChaXunActivity.class);
                        startActivity(intentKaoHeChaXun);
                    }
                    break;
                case R.id.rlMainMenu_ZhiGongSuQiu:
                    if (GlobalData.GetOfflineUser(MainMenuActivity.this))
                    {
                        GlobalData.showToast(MainMenuActivity.this, getString(R.string.offlineuser));
                        return;
                    }
                    else {
                        Intent intentSuQiu = new Intent(MainMenuActivity.this, ZhiGongSuQiuActivity.class);
                        startActivity(intentSuQiu);
                    }
                    break;
                case R.id.rlMainMenu_JiFenChaXun:
                    if (GlobalData.GetOfflineUser(MainMenuActivity.this))
                    {
                        GlobalData.showToast(MainMenuActivity.this, getString(R.string.offlineuser));
                        return;
                    }
                    else {
                        Intent intentJiFen = new Intent(MainMenuActivity.this, JiFenChaXunActivity.class);
                        startActivity(intentJiFen);
                    }
                    break;
                case R.id.rlMainMenu_ZiKongLvChaXun:
                    if (GlobalData.GetOfflineUser(MainMenuActivity.this))
                    {
                        GlobalData.showToast(MainMenuActivity.this, getString(R.string.offlineuser));
                        return;
                    }
                    else {
                        Intent intentZiKongLv = new Intent(MainMenuActivity.this, ZiKongLvChaXunActivity.class);
                        startActivity(intentZiKongLv);
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
        setContentView(R.layout.mainmenu_activity);

        mainLayout = (RelativeLayout)findViewById(R.id.rlMainMenuBack);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.rlMainMenuBack));
                            bInitialized = true;
                        }
                    }
                }
        );

        initControl();
    }

    private void initControl()
    {
        rlGanBuKaoHe = (RelativeLayout) findViewById(R.id.rlMainMenu_GanBuKaoHe);
        rlGanBuKaoHe.setOnClickListener(onClickListener);

        rlGongWenLiuZhuan = (RelativeLayout)findViewById(R.id.rlMainMenu_GongWenLiuZhuan);
        rlGongWenLiuZhuan.setOnClickListener(onClickListener);

        rlRenWu = (RelativeLayout)findViewById(R.id.rlMainMenu_RenWuGuanLi);
        rlRenWu.setOnClickListener(onClickListener);

        rlGuiZhang = (RelativeLayout)findViewById(R.id.rlMainMenu_GuiZhangChaXun);
        rlGuiZhang.setOnClickListener(onClickListener);

        rlTongXun = (RelativeLayout)findViewById(R.id.rlMainMenu_TongXunLu);
        rlTongXun.setOnClickListener(onClickListener);

        rlYouXiang = (RelativeLayout)findViewById(R.id.rlMainMenu_YouXiangChaXun);
        rlYouXiang.setOnClickListener(onClickListener);

		// 在线考试
        rlKaoShi = (RelativeLayout) findViewById(R.id.rlMainMenu_ZaiXianKaoShi);
        rlKaoShi.setOnClickListener(onClickListener);

		rlKaoHeChaXun = (RelativeLayout) findViewById(R.id.rlMainMenu_KaoHeChaXun);
        rlKaoHeChaXun.setOnClickListener(onClickListener);

        rlSuQiu = (RelativeLayout) findViewById(R.id.rlMainMenu_ZhiGongSuQiu);
        rlSuQiu.setOnClickListener(onClickListener);

        rlJiFen = (RelativeLayout) findViewById(R.id.rlMainMenu_JiFenChaXun);
        rlJiFen.setOnClickListener(onClickListener);

        rlZiKongLv = (RelativeLayout) findViewById(R.id.rlMainMenu_ZiKongLvChaXun);
        rlZiKongLv.setOnClickListener(onClickListener);
    }
}
