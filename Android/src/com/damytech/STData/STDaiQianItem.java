package com.damytech.STData;

import android.os.Parcel;
import android.os.Parcelable;

import java.util.ArrayList;

public class STDaiQianItem implements Parcelable {
    public long uid = 0;
	public String title = "";
	public String bumen = "";
    public String fbdate = "";
    public String faburen = "";
    public String tongzhihao = "";
    public String filepath = "";
    public String filename = "";
    public long total = 0;
    public String content = "";
    public String receiver = "";
    public String receiverrange = "";
    public String[] liuzhuanrenpishi = new String[0];

    public STDaiQianItem() {}

    public STDaiQianItem(Parcel in)
    {
        readFromParcel(in);
    }

    @Override
    public int describeContents()
    {
        return 0;
    }

    @Override
    public void writeToParcel(Parcel dest, int flags)
    {
        dest.writeLong(uid);
        dest.writeString(title);
        dest.writeString(bumen);
        dest.writeString(fbdate);
        dest.writeString(faburen);
        dest.writeString(tongzhihao);
        dest.writeString(filepath);
        dest.writeString(filename);
        dest.writeLong(total);
        dest.writeString(content);
        dest.writeString(receiver);
        dest.writeString(receiverrange);
        dest.writeStringArray(liuzhuanrenpishi);

        return;
    }

    private void readFromParcel(Parcel in)
    {
        uid = in.readLong();
        title = in.readString();
        bumen = in.readString();
        fbdate = in.readString();
        faburen = in.readString();
        tongzhihao = in.readString();
        filepath = in.readString();
        filename = in.readString();
        total = in.readLong();
        content = in.readString();
        receiver = in.readString();
        receiverrange = in.readString();
        try {
            liuzhuanrenpishi = in.createStringArray();
        } catch (Exception ex)
        {
            liuzhuanrenpishi = null;
        }
    }

    @SuppressWarnings({ "rawtypes", "unchecked" })
    public static final Parcelable.Creator<STDaiQianItem> CREATOR = new Parcelable.Creator() {
        @Override
        public Object createFromParcel(Parcel source) {
            return new STDaiQianItem(source);
        }
        @Override
        public Object[] newArray(int size) {
            return new STDaiQianItem[size];
        }
    };
}