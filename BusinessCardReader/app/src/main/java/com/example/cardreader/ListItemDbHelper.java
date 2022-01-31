package com.example.cardreader;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;

import java.util.ArrayList;

public class ListItemDbHelper extends SQLiteOpenHelper{
    public static final String TABLE_NAME = "events";
    public static final String COLUMN_ID = "id";
    public static final String COLUMN_NAME = "name";
    public static final String COLUMN_TITLE = "title";
    public static final String COLUMN_PHONE = "phone";
    public static final String COLUMN_EMAIL = "email";

    public ListItemDbHelper(Context context, String name, SQLiteDatabase.CursorFactory factory, int version) {
        super(context, name, factory, version);
    }

    @Override
    public void onCreate(SQLiteDatabase db) {
        db.execSQL("create table " + TABLE_NAME + "(" +
                COLUMN_ID + " integer primary key, " +
                COLUMN_NAME + " text, " +
                COLUMN_TITLE + " text, " +
                COLUMN_PHONE + " number, " +
                COLUMN_EMAIL + " text)" );
    }

    @Override
    public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
        db.execSQL("drop table if exists " + TABLE_NAME);
        onCreate(db);
    }

    public ArrayList<ListItems> getAllEvents() {
        ArrayList<ListItems> eventList = new ArrayList<ListItems>();
        SQLiteDatabase db = this.getReadableDatabase();
        Cursor res = db.rawQuery("select * from " + TABLE_NAME, null);
        res.moveToFirst();
        while (res.isAfterLast() == false) {
            ListItems event = new ListItems(
                    res.getString(res.getColumnIndex(COLUMN_NAME)),
                    res.getString(res.getColumnIndex(COLUMN_TITLE)),
                    res.getString(res.getColumnIndex(COLUMN_PHONE)),
                    res.getString(res.getColumnIndex(COLUMN_EMAIL)) );
            event.setId(res.getString(res.getColumnIndex(COLUMN_ID)) );
            eventList.add(event);
            res.moveToNext();
        }
        return eventList;
    }

    public String insertEvent(ListItems event) {
        SQLiteDatabase db = this.getWritableDatabase();
        ContentValues contentValues = new ContentValues();
        contentValues.put(COLUMN_NAME, event.getName());
        contentValues.put(COLUMN_TITLE, event.getTitle());
        contentValues.put(COLUMN_PHONE, event.getPhone());
        contentValues.put(COLUMN_EMAIL, event.getEmail());
        long id = db.insert(TABLE_NAME, null, contentValues);
        event.setId(Long.toString(id));
        return (Long.toString(id));
    }

    public Integer deleteEvent(String id) {
        SQLiteDatabase db = this.getWritableDatabase();
        return db.delete(TABLE_NAME, "id = ? ", new String[]{id});
    }

    public boolean updateEvent(String id, ListItems event) {
        SQLiteDatabase db = this.getWritableDatabase();
        ContentValues contentValues = new ContentValues();
        contentValues.put(COLUMN_NAME, event.getName());
        contentValues.put(COLUMN_TITLE, event.getTitle());
        contentValues.put(COLUMN_PHONE, event.getPhone());
        contentValues.put(COLUMN_EMAIL, event.getEmail());
        db.update(TABLE_NAME, contentValues, "id = ? ", new String[]{id});
        return true;
    }

}
