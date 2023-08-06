package com.damytech.utils;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;
import com.damytech.STData.STPdfFileData;

import java.util.ArrayList;

public class SQLiteDBHelper extends SQLiteOpenHelper {
    /*
    * Database Name Definition
    */
    private final static String DATABASE_NAME = "TieLu";
    private final static int DATABASE_VERSION = 1;

    /*
    * Table Name Definition
    */
    private final static String TABLE_PDFINFO = "tbl_pdfinfo";
        public final static String FIELD_ID = "uid";
        public final static String FIELD_PDFID = "pdfid";
        public final static String FIELD_ICONPATH = "iconpath";
        public final static String FIELD_LOCALICONPATH = "localiconpath";
        public final static String FIELD_LOCALPATH = "localpath";
        public final static String FIELD_REMOTEPATH = "remotepath";

    public final static String TABLE_DOWNLOADINFO = "tbl_downloadinfo";
        public final static String FIELD_DID = "uid";
        public final static String FIELD_LOCPATH = "locpath";
        public final static String FIELD_REMPATH = "rempath";

    public SQLiteDBHelper(Context context)
    {
        super(context, DATABASE_NAME, null, DATABASE_VERSION);
    }

    @Override
    public void onCreate(SQLiteDatabase db)
    {
        String sql="Create table " + TABLE_PDFINFO + "( " + FIELD_ID + " integer primary key autoincrement,"
                + FIELD_PDFID + " integer, "
                + FIELD_ICONPATH + " text, "
                + FIELD_LOCALICONPATH + " text, "
                + FIELD_LOCALPATH + " text, "
                + FIELD_REMOTEPATH + " text"
                + ");";
        db.execSQL(sql);

        sql = "Create table " + TABLE_DOWNLOADINFO + "( " + FIELD_DID + " integer primary key autoincrement,"
                + FIELD_LOCPATH + " text, "
                + FIELD_REMPATH + " text"
                + ");";
        db.execSQL(sql);
    }

    @Override
    public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
    {
        String sqlQuery = "DROP TABLE IF EXISTS " + DATABASE_NAME;
        db.execSQL(sqlQuery);

        onCreate(db);
    }

    public ArrayList<STPdfFileData> getDataList(  )
    {
        Cursor cursor = null;
        ArrayList<STPdfFileData> arrData = new ArrayList<STPdfFileData>();

        try {
            SQLiteDatabase db = this.getReadableDatabase();
            cursor = db.query(TABLE_PDFINFO,
                    new String[] { FIELD_PDFID, FIELD_ICONPATH, FIELD_LOCALICONPATH, FIELD_LOCALPATH, FIELD_REMOTEPATH },
                    "1=1",
                    null,
                    null,
                    null,
                    null,
                    null);

            if (cursor != null)
            {
                cursor.moveToFirst();

                do
                {
                    try {
                        STPdfFileData stPdfFileData = new STPdfFileData();
                        stPdfFileData.pdfid = cursor.getInt(cursor.getColumnIndex(SQLiteDBHelper.FIELD_PDFID));
                        stPdfFileData.iconpath = cursor.getString(cursor.getColumnIndex(SQLiteDBHelper.FIELD_ICONPATH));
                        stPdfFileData.localiconpath = cursor.getString(cursor.getColumnIndex(SQLiteDBHelper.FIELD_LOCALICONPATH));
                        stPdfFileData.localpath = cursor.getString(cursor.getColumnIndex(SQLiteDBHelper.FIELD_LOCALPATH));
                        stPdfFileData.remotepath = cursor.getString(cursor.getColumnIndex(SQLiteDBHelper.FIELD_REMOTEPATH));

                        arrData.add(stPdfFileData);

                    } catch (Exception ex) { }
                } while (cursor.moveToNext());
            }
        } catch (Exception ex) {
            arrData = new ArrayList<STPdfFileData>();
        }

        return arrData;
    }

    public long savePdfInfo(int pdfid, String iconpath, String localiconpath, String localpath, String remotepath)
    {
        long row = 0;

        try {
            SQLiteDatabase db = this.getWritableDatabase();
            ContentValues cv = new ContentValues();
            cv.put(FIELD_PDFID, pdfid);
            cv.put(FIELD_ICONPATH, iconpath);
            cv.put(FIELD_LOCALICONPATH, localiconpath);
            cv.put(FIELD_LOCALPATH, localpath);
            cv.put(FIELD_REMOTEPATH, remotepath);
            row = db.insert(TABLE_PDFINFO, null, cv);
        } catch (Exception ex) {
            row = 0;
        }

        return row;
    }

    public long updatePdfData(int pdfid, String iconpath, String localiconpath, String localpath, String remotepath)
    {
        long row = 0;

        SQLiteDatabase db=this.getWritableDatabase();

        try {
            String where = FIELD_PDFID + "=?";
            String[] whereValue={Integer.toString(pdfid)};
            ContentValues cv=new ContentValues();
            cv.put(FIELD_LOCALPATH, localpath);
            cv.put(FIELD_ICONPATH, iconpath);
            cv.put(FIELD_LOCALICONPATH, localiconpath);
            cv.put(FIELD_REMOTEPATH, remotepath);
            db.update(TABLE_PDFINFO, cv, where, whereValue);
        } catch(Exception ex) {}

        return row;
    }

    public boolean deletePdfInfo(int pdfid)
    {
        boolean bRet = true;
        try {
            SQLiteDatabase db=this.getWritableDatabase();
            String where = FIELD_PDFID + "=?";
            String[] where1 = {Integer.toString(pdfid)};
            db.delete(TABLE_PDFINFO, where, where1);
        } catch (Exception ex) {
            bRet = false;
        }

        return bRet;
    }

    public STPdfFileData getPdfInfo(int pdfid)
    {
        STPdfFileData stPdfFileData = new STPdfFileData();

        Cursor cursor = null;

        try {
            SQLiteDatabase db = this.getReadableDatabase();
            cursor = db.query(TABLE_PDFINFO,
                    new String[] { FIELD_PDFID, FIELD_ICONPATH, FIELD_LOCALICONPATH, FIELD_LOCALPATH, FIELD_REMOTEPATH},
                    FIELD_PDFID + "=?",
                    new String[]{String.valueOf(pdfid)},
                    null,
                    null,
                    null,
                    null);

            if (cursor != null) {
                cursor.moveToFirst();

                stPdfFileData.pdfid = pdfid;
                stPdfFileData.iconpath = cursor.getString(cursor.getColumnIndex(FIELD_ICONPATH));
                stPdfFileData.localiconpath = cursor.getString(cursor.getColumnIndex(FIELD_LOCALICONPATH));
                stPdfFileData.localpath = cursor.getString(cursor.getColumnIndex(FIELD_LOCALPATH));
                stPdfFileData.remotepath = cursor.getString(cursor.getColumnIndex(FIELD_REMOTEPATH));
            }

        } catch (Exception ex) {
            stPdfFileData = new STPdfFileData();
        }


        return stPdfFileData;
    }

    public long saveDownInfo(String localpath, String remotepath)
    {
        long row = 0;

        String szPath = getLocalPath(remotepath);
        if (szPath.length() == 0) {
            try {
                SQLiteDatabase db = this.getWritableDatabase();
                ContentValues cv = new ContentValues();
                cv.put(FIELD_LOCPATH, localpath);
                cv.put(FIELD_REMPATH, remotepath);
                row = db.insert(TABLE_DOWNLOADINFO, null, cv);
            } catch (Exception ex) {
                row = 0;
            }
        } else {
            return updateDownloadInfo(localpath, remotepath);
        }

        return row;
    }

    public long updateDownloadInfo(String localpath, String remotepath)
    {
        long row = 0;

        SQLiteDatabase db=this.getWritableDatabase();

        try {
            String where = FIELD_REMPATH + "=?";
            String[] whereValue={remotepath};
            ContentValues cv=new ContentValues();
            cv.put(FIELD_LOCPATH, localpath);
            cv.put(FIELD_REMPATH, remotepath);
            db.update(TABLE_DOWNLOADINFO, cv, where, whereValue);
        } catch(Exception ex) {}

        return row;
    }

    public String getLocalPath(String remotepath)
    {
        String locPath = "";

        Cursor cursor = null;

        try {
            SQLiteDatabase db = this.getReadableDatabase();
            cursor = db.query(TABLE_DOWNLOADINFO,
                    new String[] { FIELD_LOCPATH},
                    FIELD_REMPATH + "=?",
                    new String[]{remotepath},
                    null,
                    null,
                    null,
                    null);

            if (cursor != null) {
                cursor.moveToFirst();
                locPath = cursor.getString(cursor.getColumnIndex(FIELD_LOCPATH));
            }

        } catch (Exception ex) {
            locPath = "";
        }


        return locPath;
    }
}
