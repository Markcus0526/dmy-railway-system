package com.damytech.utils;

import android.content.Context;
import android.graphics.Canvas;
import android.graphics.Color;
import android.graphics.Paint;
import android.graphics.Rect;
import android.util.AttributeSet;
import android.view.View;

public class HorProgressor extends View
{
	private int nProgress = 0;
	private int fillColor = Color.GREEN;
	private int borderColor = Color.TRANSPARENT;

	public HorProgressor(Context context)
	{
		super(context);
	}

    public HorProgressor(Context context, AttributeSet attrs) {
        super(context, attrs);
    }

    public HorProgressor(Context context, AttributeSet attrs, int defStyle) {
        super(context, attrs, defStyle);
    }

    public HorProgressor(Context context, int fillColor, int borderColor)
	{
		super(context);

		this.fillColor = fillColor;
		this.borderColor = borderColor;
	}

    public void setFillColor(int nColor)
    {
        this.fillColor = nColor;
    }

    public void setBorderColor(int nColor)
    {
        this.borderColor = nColor;
    }

	@Override
	protected void onDraw(Canvas canvas) {
		super.onDraw(canvas);    //To change body of overridden methods use File | Settings | File Templates.

		int nWidth = getWidth();
		int nHeight = getHeight();

		Rect rcBounds = new Rect(0, 0, nWidth - 1, nHeight - 1);

		Rect rcFill = new Rect(0, 0, 0, 0);
		if (nProgress == 100)
			rcFill = rcBounds;
		else
			rcFill = new Rect(0, 0, nWidth * nProgress / 100, nHeight - 1);

		Paint fillPaint = new Paint();
		fillPaint.setColor(fillColor);
		fillPaint.setStyle(Paint.Style.FILL);
		canvas.drawRect(rcFill, fillPaint);

		Paint borderPaint = new Paint();
		borderPaint.setColor(borderColor);
		borderPaint.setStyle(Paint.Style.STROKE);
		borderPaint.setStrokeWidth(1);
		canvas.drawRect(rcBounds, borderPaint);
	}

	public int setProgress(int nProgress)
	{
		if (nProgress < 0 || nProgress > 100)
			return -1;

		this.nProgress = nProgress;

		invalidate();

		return 0;
	}
}
