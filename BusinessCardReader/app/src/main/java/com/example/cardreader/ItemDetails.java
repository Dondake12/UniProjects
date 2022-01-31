package com.example.cardreader;

import android.content.Intent;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.TextView;

public class ItemDetails extends AppCompatActivity {
    String name;
    String title;
    String phone;
    String email;
    String id;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_item_details);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);

        FloatingActionButton fab = (FloatingActionButton) findViewById(R.id.fab);
        fab.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Snackbar.make(view, "Replace with your own action", Snackbar.LENGTH_LONG)
                        .setAction("Action", null).show();
            }
        });
        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        Bundle extras = getIntent().getExtras();
        name = extras.getString("name");
        title = extras.getString("title");
        phone = extras.getString("phone");
        email = extras.getString("email");
//        id = extras.getString("id");

        TextView tView = (TextView) findViewById(R.id.textViewSelectedItemName);
        tView.setText(name);

        TextView nView = (TextView) findViewById(R.id.textViewItemName);
        nView.setText(name);

        TextView titleView = (TextView) findViewById(R.id.textViewItemTitle);
        titleView.setText(title);

        TextView pView = (TextView) findViewById(R.id.textViewItemPhone);
        pView.setText(phone);

        TextView eView = (TextView) findViewById(R.id.textViewItemEmail);
        eView.setText(email);

    }

    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_ListView) {
            Intent intent = new Intent(this, ListViewEvent.class);
            startActivity(intent);
            return true;
        }
        if (id == R.id.action_GoHome) {
            Intent intent = new Intent(this, MainActivity.class);
            startActivity(intent);
            return true;
        }
        return super.onOptionsItemSelected(item);
    }

    public void goToEditView (View v){
        Intent intent = new Intent(this, EditActivity.class);

        intent.putExtra("name", name);
        intent.putExtra("title", title);
        intent.putExtra("phone", phone);
        intent.putExtra("email", email);
        intent.putExtra("id", id);
        startActivity(intent);
    }
}
