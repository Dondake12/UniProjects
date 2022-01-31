package com.example.cardreader;

import android.content.Context;
import android.graphics.Color;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.widget.Toolbar;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ImageView;
import android.widget.TextView;

import java.util.ArrayList;

public class ListItemsAdapter extends ArrayAdapter<ListItems> {
    ArrayList<ListItems> events;

    public ListItemsAdapter(Context context, int resource, ArrayList<ListItems> objects) {
        super(context, resource, objects);
        events = objects;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {

        if (convertView == null) {
            convertView = LayoutInflater.from(getContext()).
                    inflate(R.layout.my_listview_item, parent, false);
        }

        ListItems event = events.get(position);

        ImageView icon = (ImageView) convertView.findViewById(R.id.imageViewIcon);
        icon.setImageResource(R.mipmap.ic_introduction);

        TextView name = (TextView) convertView.findViewById(R.id.textViewName);
        name.setText(event.getName());

        TextView title = (TextView) convertView.findViewById(R.id.textViewTitle);
        title.setText(event.getTitle());

        if (position % 2 == 0){
            convertView.setBackgroundColor(Color.parseColor("#ccffff"));
        }else{
            convertView.setBackgroundColor(Color.parseColor("#ffffe6"));
        }
        return convertView;
    }
}