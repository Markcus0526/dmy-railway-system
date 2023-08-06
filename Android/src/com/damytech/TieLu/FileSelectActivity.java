package com.damytech.TieLu;

import android.app.ListActivity;
import android.content.Intent;
import android.graphics.Rect;
import android.os.Bundle;
import android.view.View;
import android.view.ViewTreeObserver;
import android.widget.*;
import com.damytech.utils.GlobalData;
import com.damytech.utils.ResolutionSet;

import java.io.File;
import java.io.FileInputStream;
import java.util.ArrayList;
import java.util.List;

public class FileSelectActivity extends ListActivity {
    LinearLayout mainLayout;
    boolean bInitialized = false;

    private List<String> item = null;
    private List<String> path = null;
    private String root="/sdcard/";

    /**
     * Called when the activity is first created.
     */
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.fileselect_activity);

        getDir(root);
    }

    private void getDir(String dirPath)
    {
        item = new ArrayList<String>();
        path = new ArrayList<String>();

        File f = new File(dirPath);
        File[] files = f.listFiles();

        if(!dirPath.equals("/sdcard"))
        {
            item.add(root);
            path.add(root);

            item.add("../");
            path.add(f.getParent());
        }

        for(int i=0; i < files.length; i++){
            File file = files[i];
            path.add(file.getPath());

            if(file.isDirectory())
                item.add(file.getName() + "/");
            else
                item.add(file.getName());
        }

        ArrayAdapter<String> fileList = new ArrayAdapter<String>(this, R.layout.row, item);
        setListAdapter(fileList);
    }

    @Override
    protected void onListItemClick(ListView l, View v, int position, long id) {
        File file = new File(path.get(position));

        if (file.isDirectory())
        {
            if(file.canRead())
            {
                try
                {
                    getDir(path.get(position));
                }
                catch (Exception ex) {}
            }
        }
        else
        {
            Bundle bundle = new Bundle();
            Intent retIntent = new Intent();
            bundle.putString("PATH", file.getAbsoluteFile().toString());
            bundle.putString("NAME", file.getName().toString());
            retIntent.putExtras(bundle);
            FileSelectActivity.this.setResult(RESULT_OK, retIntent);
            FileSelectActivity.this.finish();
        }
    }
}
