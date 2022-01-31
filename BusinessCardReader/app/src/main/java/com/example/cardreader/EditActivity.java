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
import android.widget.EditText;

import java.util.ArrayList;

public class EditActivity extends AppCompatActivity {
    String name;
    String title;
    String phone;
    String email;
    String id;

    EditText nView;
    EditText tView;
    EditText pView;
    EditText eView;

    ArrayList<ListItems> events = new ArrayList<ListItems>();
    ListItemDbHelper mydb = new ListItemDbHelper(this, "ListItemDb", null, 8);

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_edit);
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
        id = extras.getString("id");

        nView = (EditText) findViewById(R.id.editTextName);
        nView.setText(name);
        tView = (EditText) findViewById(R.id.editTextTitle);
        tView.setText(title);
        pView = (EditText) findViewById(R.id.editTextPhone);
        pView.setText(phone);
        eView = (EditText) findViewById(R.id.editTextEmail);
        eView.setText(email);

    }
    public void goBack (View v){
        super.onBackPressed();
    }

    public void goToSaveView (View v){
        Intent intent = new Intent(this, ItemDetails.class);

        intent.putExtra("name", nView.getText().toString());
        intent.putExtra("title", tView.getText().toString());
        intent.putExtra("phone", pView.getText().toString());
        intent.putExtra("email", eView.getText().toString());

        ListItems update = new ListItems(nView.getText().toString(), tView.getText().toString(),
                pView.getText().toString(), eView.getText().toString());

        events = mydb.getAllEvents();
        mydb.updateEvent(id, update);
        events = mydb.getAllEvents();

        startActivity(intent);
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
        if (id == R.id.action_GoHome) {
            Intent intent = new Intent(this, MainActivity.class);
            startActivity(intent);
            return true;
        }

        if (id == R.id.action_add) {
            Intent intent = new Intent(this, AddActivity.class);
            startActivity(intent);
            return true;
        }
        return super.onOptionsItemSelected(item);
    }
}
