package com.damytech.TieLu;

import android.app.Activity;
import android.app.AlertDialog;
import android.app.Dialog;
import android.content.Context;
import android.content.DialogInterface;
import android.os.Bundle;
import android.os.Environment;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ListAdapter;
import android.widget.TextView;
import java.io.File;
import java.io.FilenameFilter;
import java.util.ArrayList;

/**
 * Created by ruifeng on 2015/2/22.
 */
public class FileBrowserActivity extends Activity{
    protected static final int DIALOG_LOAD_FILE = 1000;
    protected static final int MODE_IMGFILES = 0;
    protected static final int MODE_DOCFILES = 1;
    protected static int MODE_BROWSE = MODE_IMGFILES;

    protected boolean isRootLevel = true;
    ArrayList<String> arrDirectoryList = new ArrayList<String>();
    private Item[] fileList;
    ListAdapter adapterFile;
    private File currentPath = null;

    protected String szSelFileName = "";
    protected String szSelFilePath = "";

    private DialogInterface.OnDismissListener childDismiss;

    @Override
    public void onCreate(Bundle saveInstance) {
        super.onCreate(saveInstance);
    }

    protected void loadFileList(final int browseMode, DialogInterface.OnDismissListener chDismiss) {
        childDismiss = chDismiss;
        MODE_BROWSE = browseMode;

        if (browseMode == MODE_IMGFILES) {
            //currentPath = new File(Environment.getExternalStorageDirectory() + "/DCIM");
            currentPath = new File(getFilePath() + "DCIM");
        }
        else if (browseMode == MODE_DOCFILES) {
            currentPath = new File(getFilePath() + "download");
        }

        currentPath = new File(Environment.getExternalStorageDirectory() + "");

        try {
            currentPath.mkdirs();
        } catch (SecurityException e) {
        }

        if (currentPath.exists()) {
            FilenameFilter filter = new FilenameFilter() {
                @Override
                public boolean accept(File dir, String filename) {
                    File sel = new File(dir, filename);
                    // Filters based on whether the file is hidden or not
                    return (sel.isFile() || sel.isDirectory())
                            && !sel.isHidden();
                }
            };

            String[] fList = currentPath.list(filter);
            fileList = new Item[fList.length];
            for (int i = 0; i < fList.length; i++) {
                fileList[i] = new Item(fList[i], R.drawable.icon_file);

                File sel = new File(currentPath, fList[i]);
                if (sel.isDirectory()) {
                    fileList[i].icon = R.drawable.icon_directory;
                }
            }

            if (!isRootLevel) {
                Item temp[] = new Item[fileList.length + 1];
                for (int i = 0; i < fileList.length; i++) {
                    temp[i + 1] = fileList[i];
                }
                temp[0] = new Item("Up", R.drawable.icon_directoryup);
                fileList = temp;
            }
        }

        adapterFile = new ArrayAdapter<Item>(this, android.R.layout.select_dialog_item, android.R.id.text1, fileList) {
            @Override
            public View getView(int position, View convertView, ViewGroup parent) {
                View view = super.getView(position, convertView, parent);
                TextView textView = (TextView) view.findViewById(android.R.id.text1);
                textView.setCompoundDrawablesWithIntrinsicBounds(fileList[position].icon, 0, 0, 0);

                int dp5 = (int) (5 * getResources().getDisplayMetrics().density + 0.5f);
                textView.setCompoundDrawablePadding(dp5);

                return view;
            }
        };

    }

    private class Item {
        public String file;
        public int icon;

        public Item(String file, Integer icon) {
            this.file = file;
            this.icon = icon;
        }

        @Override
        public String toString() {
            return file;
        }
    }

    @Override
    protected Dialog onCreateDialog(int id) {
        Dialog dlgFileBrowser = null;
        AlertDialog.Builder builder = new AlertDialog.Builder(this);

        if (fileList == null) {
            dlgFileBrowser = builder.create();
            return dlgFileBrowser;
        }

        switch (id) {
            case DIALOG_LOAD_FILE:
                builder.setTitle("Choose your file");
                builder.setAdapter(adapterFile, new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        String chosenFile = fileList[which].file;
                        final File fileSelected = new File(currentPath + "/" + chosenFile);
                        if (fileSelected.isDirectory()) {
                            isRootLevel = false;
                            fileList = null;
                            arrDirectoryList.add(chosenFile);
                            currentPath = new File(fileSelected + "");

                            loadFileList(MODE_BROWSE, childDismiss);

                            removeDialog(DIALOG_LOAD_FILE);
                            showDialog(DIALOG_LOAD_FILE);
                        } else if (chosenFile.equalsIgnoreCase("up") && !fileSelected.exists()) {
                            fileList = null;
                            String szRemoveFile = arrDirectoryList.remove(arrDirectoryList.size() - 1);
                            currentPath = new File(currentPath.toString().substring(0, currentPath.toString().lastIndexOf(szRemoveFile)));

                            if (arrDirectoryList.isEmpty()) {
                                isRootLevel = true;
                            }

                            loadFileList(MODE_BROWSE, childDismiss);

                            removeDialog(DIALOG_LOAD_FILE);
                            showDialog(DIALOG_LOAD_FILE);

                        } else {
                            szSelFilePath = fileSelected.getAbsolutePath().toString();
                            szSelFileName = fileSelected.getName();
                        }

                    }
                });
                break;
        }

        dlgFileBrowser = builder.show();
        dlgFileBrowser.setCanceledOnTouchOutside(false);
        dlgFileBrowser.setOnDismissListener(childDismiss);
        return dlgFileBrowser;
    }

    private String getFilePath () {
        String szFilePath = "";
        if (new File ("/mnt/sdcard2/").exists())
            szFilePath = "/mnt/sdcard2/";
        else if (new File ("/mnt/sd-ext/").exists())
            szFilePath = "/mnt/sd-ext/";
        else if (new File ("/mnt/sdcard/").exists())
            szFilePath = "/mnt/sdcard/";
        else if (new File ("/mnt/sdcard0/").exists())
            szFilePath = "/mnt/sdcard0/";

        return szFilePath;
    }
}
