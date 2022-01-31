package com.zetcode;

import java.awt.Color;

public class SnakeLinkedList {
    SnakeNode head;
    
    public SnakeLinkedList() {
        head = null; 
    }
    
    public void addHead(int x, int y, Color c) {
        head = new SnakeNode(x, y, c);
    }
    
    public void addJoint(int x, int y, Color c) {
        SnakeNode newJoint = new SnakeNode(x, y, c); 
        newJoint.setNext(head.getNext());
        head.setNext(newJoint);

    }
  
    public SnakeNode getHead() {
       
      return head;
    }

    public SnakeNode getJoint(int i) {
       
      SnakeNode curr = head;
      for(int k =0; k < i;k++) {
        curr = curr.getNext();
      }
      return curr;
    }
    

    public void snakeMove(int dots, boolean left, boolean right, boolean up, boolean down, final int SIZE) {

         for (int z = dots-1; z > 0; z--) {
             
             SnakeNode curr = getJoint(z);
             SnakeNode nextnode = getJoint(z-1);
             curr.setX(nextnode.getX());
             curr.setY(nextnode.getY());
  
         }

         if (left) {
             
             head.setX(head.getX() - SIZE);
         }

         if (right) {
             head.setX(head.getX() + SIZE);
         }

         if (up) {
             head.setY(head.getY() - SIZE);
         }

         if (down) {
             head.setY(head.getY() + SIZE);
         }
    }

    }

