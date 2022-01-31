// TODO Implement this library.

import 'package:flutter/material.dart';
import 'readMyMemories.dart';
import 'writeMyMemories.dart';

class MyMemories extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('My Memories'),
      ),
      body: GridView.count(
          primary: false,
          padding: const EdgeInsets.all(20),
          crossAxisSpacing: 10,
          mainAxisSpacing: 10,
          crossAxisCount: 3,
          children: <Widget>[
            RaisedButton(
              padding: const EdgeInsets.all(8),
              child: const Text("Read Memories"),
              color: Colors.lightGreen[400],
              onPressed: () {
                Navigator.push(
                  context,
                  MaterialPageRoute(builder: (context) => ReadMyMemories()),
                );
              },
            ),
            RaisedButton(
              padding: const EdgeInsets.all(8),
              child: const Text('Write Memories'),
              color: Colors.lightBlue[400],
              onPressed: () {
                Navigator.push(
                  context,
                  MaterialPageRoute(builder: (context) => WriteMyMemories()),
                );
              },
            ),
            RaisedButton(
              padding: const EdgeInsets.all(8),
              child: const Text('Record Memories'),
              color: Colors.orange[400],
              onPressed: () {
                Navigator.pop(context);
              },
            ),
          ]),
    );
  }
}
