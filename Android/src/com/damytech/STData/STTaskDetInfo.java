package com.damytech.STData;

import java.util.ArrayList;

public class STTaskDetInfo {
    public long uid = 0;
	public long taskstatusid = 0;
	public String title = "";
	public String startdate = "";
	public String enddate = "";
	public int state = 0;
	public String content = "";
	public String filename = "";
	public String filepath = "";
	public ArrayList<STTaskLog> tasklog = new ArrayList<STTaskLog>();
}