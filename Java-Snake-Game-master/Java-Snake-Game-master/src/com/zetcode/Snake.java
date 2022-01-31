package com.zetcode;

import java.awt.EventQueue;
import javax.swing.JFrame;
import javax.swing.JLabel;
import java.awt.Color;
import javax.swing.SwingConstants;

public class Snake extends JFrame {

	public static JLabel lblScore;
	
    public Snake() {
        
        initUI();
    }
    
    private void initUI() {
        
        Board board = new Board();
        getContentPane().add(board);
        
        lblScore = new JLabel("Score: 0");
//        lblScore.setText("Score: " + Integer.toString(Board.dots));
        lblScore.setForeground(Color.YELLOW);
        board.add(lblScore);
               
        setResizable(false);
        pack();
        
        setTitle("Snake");
        setLocationRelativeTo(null);
        setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
    }
    

    public static void main(String[] args) {
        
        EventQueue.invokeLater(() -> {
            JFrame ex = new Snake();
            ex.setVisible(true);
        });
    }
}
