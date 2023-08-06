package com.damytech.TieLu;

import android.app.IntentService;
import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.graphics.Color;
import android.media.RingtoneManager;
import com.damytech.HttpConn.JsonHttpResponseHandler;
import com.damytech.STData.STNewsItem;
import com.damytech.STData.STNewsList;
import com.damytech.commservice.CommMgr;
import com.damytech.utils.GlobalData;
import org.json.JSONObject;
import java.util.ArrayList;
import java.util.Random;
import java.util.Timer;
import java.util.TimerTask;

public class NewsService extends IntentService {
	public static Context m_ctxMain = null;
	private STNewsList m_stNewsList = new STNewsList();
	
    private static final int THIRTY_SECOND = 1000 * 30;

	public NewsService() {
		super("");
	}

	public NewsService(String name) {
        super(name);
	}

	@Override
	protected void onHandleIntent(Intent intent) {}

	@Override
	public int onStartCommand(Intent intent, int flags, int startId)
	{
		Timer timer = new Timer();
		timer.schedule(new TimerTask() {
			@Override
			public void run() {
				while (true)
				{
					try
					{
						CommMgr.commService.GetNewsList(handler, GlobalData.GetUid(m_ctxMain));
                        Thread.sleep(THIRTY_SECOND);
                    }
					catch (Exception ex)
					{
						ex.printStackTrace();
					}
				}
			}
		}, 100);
		return START_STICKY;
	}

	private JsonHttpResponseHandler handler = new JsonHttpResponseHandler()
	{
		int result = 0;

		@Override
		public void onSuccess(JSONObject jsonData)
		{
			result = 1;

			CommMgr.commService.parseGetNewsList(jsonData, m_stNewsList);
		}

		@Override
		public void onFailure(Throwable ex, String exception)
		{
		}

		@Override
		public void onFinish()
		{
			if (result == 1 && m_stNewsList != null)
			{
                if (m_stNewsList.arrGongWen != null) {
                    for (int i = 0; i < m_stNewsList.arrGongWen.size(); i++) {
                        if (GlobalData.getExistGongWenID(getApplicationContext(), m_stNewsList.arrGongWen.get(i).uid) == false) {
                            if (m_ctxMain != null) {
                                int icon = R.drawable.ic_launcher;
                                long when = System.currentTimeMillis();

                                Intent notificationIntent = new Intent(m_ctxMain, GongWenXiangQingActivity.class);
                                notificationIntent.putExtra("uid", m_stNewsList.arrGongWen.get(i).uid);
                                notificationIntent.putExtra("QianShouFlag", 1);
                                notificationIntent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                                Random random = new Random(System.currentTimeMillis());
                                PendingIntent intent = PendingIntent.getActivity(m_ctxMain, random.nextInt(10000), notificationIntent, PendingIntent.FLAG_CANCEL_CURRENT);

                                Notification.Builder notif_builder = new Notification.Builder(m_ctxMain);
                                notif_builder.setSmallIcon(icon);
                                notif_builder.setContentTitle(m_stNewsList.arrGongWen.get(i).title);
                                notif_builder.setContentText(getString(R.string.xindexiaoxi));
                                notif_builder.setWhen(when);
                                notif_builder.setContentIntent(intent);
                                notif_builder.setSound(RingtoneManager.getDefaultUri(RingtoneManager.TYPE_NOTIFICATION));
                                notif_builder.setAutoCancel(true);

                                NotificationManager notificationManager = (NotificationManager) m_ctxMain.getSystemService(Context.NOTIFICATION_SERVICE);
                                notificationManager.notify(random.nextInt(10000), notif_builder.build());
                            }
                        }
                    }
                }

                if (m_stNewsList.arrEmail != null) {
                    for (int i = 0; i < m_stNewsList.arrEmail.size(); i++) {
                        if (GlobalData.getExistEmailID(getApplicationContext(), m_stNewsList.arrEmail.get(i).uid) == false) {
                            if (m_ctxMain != null) {
                                int icon = R.drawable.ic_launcher;
                                long when = System.currentTimeMillis();

                                Intent notificationIntent = new Intent(m_ctxMain, YouXiangXiangXiActivity.class);
                                notificationIntent.putExtra("emailid", m_stNewsList.arrEmail.get(i).uid);
                                notificationIntent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                                Random random = new Random(System.currentTimeMillis());
                                PendingIntent intent = PendingIntent.getActivity(m_ctxMain, random.nextInt(10000), notificationIntent, PendingIntent.FLAG_CANCEL_CURRENT);

                                Notification.Builder notif_builder = new Notification.Builder(m_ctxMain);
                                notif_builder.setSmallIcon(icon);
                                notif_builder.setContentTitle(m_stNewsList.arrEmail.get(i).title);
                                notif_builder.setContentText(getString(R.string.xindexiaoxi));
                                notif_builder.setWhen(when);
                                notif_builder.setContentIntent(intent);
                                notif_builder.setSound(RingtoneManager.getDefaultUri(RingtoneManager.TYPE_NOTIFICATION));
                                notif_builder.setVibrate(new long[]{1000, 1000, 1000, 1000, 1000});
                                notif_builder.setLights(Color.RED, 3000, 3000);
                                //notif_builder.setDefaults(Notification.DEFAULT_VIBRATE | Notification.DEFAULT_SOUND);
                                notif_builder.setAutoCancel(true);

                                NotificationManager notificationManager = (NotificationManager) m_ctxMain.getSystemService(Context.NOTIFICATION_SERVICE);
                                notificationManager.notify(random.nextInt(10000), notif_builder.build());
                            }
                        }
                    }
                }

                if (m_stNewsList.arrTask != null) {
                    for (int i = 0; i < m_stNewsList.arrTask.size(); i++) {
                        if (GlobalData.getExistTaskID(getApplicationContext(), m_stNewsList.arrTask.get(i).uid) == false) {
                            if (m_ctxMain != null) {
                                int icon = R.drawable.ic_launcher;
                                long when = System.currentTimeMillis();

                                Intent notificationIntent = new Intent(m_ctxMain, RenWuXiangQingActivity.class);
                                notificationIntent.putExtra("uid", m_stNewsList.arrTask.get(i).uid);
                                notificationIntent.setFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                                Random random = new Random(System.currentTimeMillis());
                                PendingIntent intent = PendingIntent.getActivity(m_ctxMain, random.nextInt(10000), notificationIntent, PendingIntent.FLAG_CANCEL_CURRENT);

                                Notification.Builder notif_builder = new Notification.Builder(m_ctxMain);
                                notif_builder.setSmallIcon(icon);
                                notif_builder.setContentTitle(m_stNewsList.arrTask.get(i).title);
                                notif_builder.setContentText(getString(R.string.xindexiaoxi));
                                notif_builder.setWhen(when);
                                notif_builder.setContentIntent(intent);
                                notif_builder.setSound(RingtoneManager.getDefaultUri(RingtoneManager.TYPE_NOTIFICATION));
                                notif_builder.setAutoCancel(true);

                                NotificationManager notificationManager = (NotificationManager) m_ctxMain.getSystemService(Context.NOTIFICATION_SERVICE);
                                notificationManager.notify(random.nextInt(10000), notif_builder.build());
                            }
                        }
                    }
                }
			}
		}
	};		
}
