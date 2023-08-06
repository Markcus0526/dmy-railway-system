package com.damytech.TieLu;

import android.app.Activity;
import android.content.Intent;
import android.graphics.Rect;
import android.net.Uri;
import android.os.Bundle;
import android.os.Environment;
import android.provider.MediaStore;
import android.view.View;
import android.view.ViewTreeObserver;
import android.widget.Button;
import android.widget.RelativeLayout;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;

import java.io.File;

public class SelectPhotoActivity extends Activity
{
    RelativeLayout mainLayout;
    boolean bInitialized = false;

    public static int IMAGE_WIDTH = 350;
    public static int IMAGE_HEIGHT = 640;
	Button btnTakePhoto = null, btnSelImage = null, btnCancel = null;
	static Uri fileUri = null;
	public static int CAPTURE_IMAGE_ACTIVITY_REQ = 0;
	public static int SELECT_IMAGE_ACTIVITY_REQ = 1;
	public static String szRetCode = "RET";
	public static String szRetPath = "PATH";
	public static String szRetUri = "URI";
	public static int nRetSuccess = 1;
	public static int nRetCancelled = 0;
	public static int nRetFail = -1;
	private String photo_path = "";
	private Uri photo_uri = null;

	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		setContentView(R.layout.selectphoto_activity);

		initVariables();
		initControls();
		initHandlers();

        mainLayout = (RelativeLayout)findViewById(R.id.parent_layout);
        mainLayout.getViewTreeObserver().addOnGlobalLayoutListener(
                new ViewTreeObserver.OnGlobalLayoutListener() {
                    public void onGlobalLayout() {
                        if (bInitialized == false)
                        {
                            Rect r = new Rect();
                            mainLayout.getLocalVisibleRect(r);
                            ResolutionSet._instance.setResolution(r.width(), r.height());
                            ResolutionSet._instance.iterateChild(findViewById(R.id.parent_layout));
                            bInitialized = true;
                        }
                    }
                }
        );
	}

	private void initVariables()
	{
		btnTakePhoto = null;
		btnSelImage = null;
		btnCancel = null;

		CAPTURE_IMAGE_ACTIVITY_REQ = 0;
		SELECT_IMAGE_ACTIVITY_REQ = 1;

		szRetCode = "RET";
		szRetPath = "PATH";
		szRetUri = "URI";

		nRetSuccess = 1;
		nRetCancelled = 0;
		nRetFail = -1;

		photo_path = "";
		photo_uri = null;
	}

	public void initControls()
	{
		btnTakePhoto = (Button)findViewById(R.id.btnTakePhoto);
		btnSelImage = (Button)findViewById(R.id.btnSelImage);
		btnCancel = (Button)findViewById(R.id.btnCancel);
	}

	public void initHandlers()
	{
		btnTakePhoto.setOnClickListener(onClickTakePhoto);
		btnSelImage.setOnClickListener(onClickSelImage);
		btnCancel.setOnClickListener(onClickCancel);
	}

	@Override
	protected void onResume()
	{
		super.onResume();	//To change body of overridden methods use File | Settings | File Templates.

		if (photo_path.equals("") && photo_uri == null)
			return;

		Intent retIntent = new Intent();
		retIntent.putExtra(szRetCode, nRetSuccess);

		if (!photo_path.equals("") && photo_path != null)
			retIntent.putExtra(szRetPath, photo_path);

		if (photo_uri != null)
			retIntent.putExtra(szRetUri, photo_uri);

		setResult(RESULT_OK, retIntent);

		finish();
	}

	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data)
	{
		super.onActivityResult(requestCode, resultCode, data);

		if (requestCode == CAPTURE_IMAGE_ACTIVITY_REQ)
		{
			if (resultCode == RESULT_OK)
			{
				Uri photoUri = null;

				if (data == null)
					photoUri = fileUri;
				else
					photoUri = data.getData();

				try
				{
					if (photoUri != null)
					{
						String szPath = photoUri.getPath();
						if (szPath == null || szPath.equals(""))
						{
							GlobalData.showToast(SelectPhotoActivity.this, getResources().getString(R.string.STR_LOADING_IMAGE_FAILED));
						}
						else
						{
							photo_path = szPath;
							photo_uri = null;
						}
					}
					else
					{
						photo_path = fileUri.getPath();
						photo_uri = null;
					}
				}
				catch (Exception ex)
				{
					ex.printStackTrace();
					GlobalData.showToast(SelectPhotoActivity.this, getResources().getString(R.string.STR_TAKING_PHOTO_FAILED));
				}
			}
		}
		else if (requestCode == SELECT_IMAGE_ACTIVITY_REQ)
		{
			if (resultCode == RESULT_OK)
			{
				Uri selImage = data.getData();
				if (selImage != null)
				{
					photo_path = "";
					photo_uri = selImage;
				}
			}
		}
	}

	public View.OnClickListener onClickTakePhoto = new View.OnClickListener()
	{
		@Override
		public void onClick(View v)
		{
			Intent intent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);
			File file = getOutputPhotoFile();
			fileUri = Uri.fromFile(file);
			intent.putExtra(MediaStore.EXTRA_OUTPUT, fileUri);
			startActivityForResult(intent, CAPTURE_IMAGE_ACTIVITY_REQ);
		}
	};

	public View.OnClickListener onClickSelImage = new View.OnClickListener()
	{
		@Override
		public void onClick(View v)
		{
			Intent intent = new Intent();
			intent.setType("image/*");
			intent.setAction(Intent.ACTION_GET_CONTENT);
			startActivityForResult(Intent.createChooser(intent, "Select Picture"), SELECT_IMAGE_ACTIVITY_REQ);
		}
	};

	public View.OnClickListener onClickCancel = new View.OnClickListener()
	{
		@Override
		public void onClick(View v)
		{
			cancelWithData();
		}
	};

	private File getOutputPhotoFile()
	{
		File directory = new File(Environment.getExternalStoragePublicDirectory(Environment.DIRECTORY_PICTURES), getPackageName());
		if (!directory.exists())
		{
			if (!directory.mkdirs())
				return null;
		}

		String timeStamp = "userPhoto";
		return new File(directory.getPath() + File.separator + "IMG_" + timeStamp + ".jpg");
	}

	private void cancelWithData()
	{
		Intent returnIntent = new Intent();
		setResult(RESULT_CANCELED, returnIntent);
		SelectPhotoActivity.this.finish();
	}
}
