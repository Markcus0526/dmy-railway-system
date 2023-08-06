package com.damytech.STData;

import android.os.Parcel;
import android.os.Parcelable;

public class STKaoShiItem implements Parcelable {
    public long uid = 0;
    public String title = "";
	public int examtime = 0;
	public int problems = 0;
	public String content = "";
	public int isexam = 0;

    public STKaoShiItem() {}

    public STKaoShiItem(Parcel in)
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
        dest.writeInt(examtime);
        dest.writeInt(problems);
        dest.writeString(content);
        dest.writeInt(isexam);

        return;
    }

    private void readFromParcel(Parcel in)
    {
        uid = in.readLong();
        title = in.readString();
        examtime = in.readInt();
        problems = in.readInt();
        content = in.readString();
        isexam = in.readInt();

        return;
    }

    @SuppressWarnings({ "rawtypes", "unchecked" })
    public static final Parcelable.Creator<STKaoShiItem> CREATOR = new Parcelable.Creator() {
        @Override
        public Object createFromParcel(Parcel source) {
            return new STKaoShiItem(source);
        }
        @Override
        public Object[] newArray(int size) {
            return new STKaoShiItem[size];
        }
    };
}