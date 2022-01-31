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
import android.widget.AdapterView;
import android.widget.ListView;

import java.util.ArrayList;

public class ListViewEvent extends AppCompatActivity {
    ArrayList<ListItems> events = new ArrayList<ListItems>();
    ListItemDbHelper mydb = new ListItemDbHelper(this, "ListItemDb", null, 8);

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_list_view_event);
        Toolbar toolbar = findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);

        FloatingActionButton fab = findViewById(R.id.fab);
        fab.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Snackbar.make(view, "Replace with your own action", Snackbar.LENGTH_LONG)
                        .setAction("Action", null).show();
            }
        });

        getSupportActionBar().setDisplayHomeAsUpEnabled(true);

        events = mydb.getAllEvents();
        if (events.isEmpty()) {
            mydb.insertEvent(new ListItems("Random Business Man", "Manager", "919", "business@random.com"));
            mydb.insertEvent(new ListItems("Me, The Best In the World", "Supreme Ruler of the World", "6969", "supremeRuler@world.com"));
            mydb.insertEvent(new ListItems("Naruto Uzumaki", "Hokage", "00", "hokage@konoha.com"));
            events = mydb.getAllEvents();
        }

        ListItemsAdapter adapter = new ListItemsAdapter(this, R.layout.my_listview_item, events);

        ListView listView = (ListView) findViewById(R.id.ListViewItem);
        listView.setAdapter(adapter);

        listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                ListItems itemList = events.get(position);
                Intent intent = new Intent(view.getContext(), ItemDetails.class);
                intent.putExtra("name", itemList.getName());
                intent.putExtra("title", itemList.getTitle());
                intent.putExtra("phone", itemList.getPhone());
                intent.putExtra("email", itemList.getEmail());
                intent.putExtra("id", itemList.getId());
                startActivity(intent);
            }
        });
    }

    @Override
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
        return super.onOptionsItemSelected(item);
    }

    public void goToAddView (View v){
        Intent intent = new Intent(this, AddActivity.class);
        startActivity(intent);
    }

}
