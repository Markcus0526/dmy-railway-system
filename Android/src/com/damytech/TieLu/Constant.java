package com.damytech.TieLu;

/**
 * Created by ruifeng on 2015/2/22.
 */
public class Constant {
    public static String PNG_FILEFILTER = ".png";
    public static String JPG_FILEFILTER = ".jpg";
    public static String JPEG_FILEFILTER = ".jpeg";
    public static String WORD2003_FILEFILTER = ".doc";
    public static String WORD2010_FILEFILTER = ".docx";
    public static String EXCEL2003_FILEFILTER = ".xls";
    public static String EXCEL2010_FILEFILTER = ".xlsx";
    public static String PDF_FILEFILTER = ".pdf";

    public static boolean isDocFile(String filePath) {
        return (filePath.endsWith(WORD2003_FILEFILTER) || filePath.endsWith(WORD2010_FILEFILTER) ||
                filePath.endsWith(EXCEL2003_FILEFILTER) || filePath.endsWith(EXCEL2010_FILEFILTER) ||
                filePath.endsWith(PDF_FILEFILTER));
    }

    public static boolean isImgFile(String filePath) {
        return (filePath.endsWith(PNG_FILEFILTER) || filePath.endsWith(JPEG_FILEFILTER) || filePath.endsWith(JPG_FILEFILTER));
    }
}
