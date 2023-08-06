package com.damytech.STData;

import java.util.ArrayList;

public class STKaoShi {
    public static final int KAOSHI_TYPE_UNKNOWN = 0;

    public static final int KAOSHI_TYPE_DANXUAN = 1;
    public static final int KAOSHI_TYPE_DUOXUAN = 2;
    public static final int KAOSHI_TYPE_PANDUAN = 3;

    public class STQuestion {
        public String ind = "";
        public String question = "";
    };

    public long id = 0;
    public String examtype = "";
    public String title = "";
    public ArrayList<STQuestion> questions = null;
    public ArrayList<String> answers = null;

    public int getType() {
        // check parameters
        if (examtype == null || examtype.length() == 0)
            return KAOSHI_TYPE_UNKNOWN;

        if (examtype.equals("单选题"))
            return KAOSHI_TYPE_DANXUAN;
        if (examtype.equals("多选题"))
            return KAOSHI_TYPE_DUOXUAN;
        if (examtype.equals("判断题"))
            return KAOSHI_TYPE_PANDUAN;

        return KAOSHI_TYPE_UNKNOWN;
    }
};