// Copyright 2018 The Flutter team. All rights reserved.
// Use of this source code is governed by a BSD-style license that can be
// found in the LICENSE file.

import 'package:flutter/material.dart';
import 'myMemories/myMemories.dart';

void main() {
  runApp(MaterialApp(
    title: 'Dashboard',
    home: Dashboard(),
  ));
}

class Dashboard extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: Text("Dashboard"),
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
              child: const Text("My Memories"),
              color: Colors.lightGreen[400],
              onPressed: () {
                Navigator.push(
                  context,
                  MaterialPageRoute(builder: (context) => MyMemories()),
                );
              },
            ),
            RaisedButton(
              padding: const EdgeInsets.all(8),
              child: const Text('Phone Call'),
              color: Colors.pink[400],
              onPressed: () {
                Navigator.push(
                  context,
                  MaterialPageRoute(builder: (context) => MyMemories()),
                );
              },
            ),
          ],
        ));
  }
}
