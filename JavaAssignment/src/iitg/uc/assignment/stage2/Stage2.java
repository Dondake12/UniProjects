// Author: 				Badr Alluhaib
// Student ID: 			u3124221
// Date Created: 		19/08/2016
// Date last changed:	21/08/2016
// 
// Simple GUI program extending Stage1 program.
// Now all information can be inputted using GUI elements.

package iitg.uc.assignment.stage2;

import java.awt.BorderLayout;
import java.awt.EventQueue;

import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.border.EmptyBorder;

import iitg.uc.assignment.stage1.PopulationCalculator;

import javax.swing.JLabel;
import java.awt.Font;
import javax.swing.SwingConstants;
import java.awt.GridLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import javax.swing.JTextField;
import javax.swing.JRadioButton;
import javax.swing.ButtonGroup;
import javax.swing.JButton;
import java.awt.Color;

public class Stage2 extends JFrame {

	private final int CAPACITY = 5000;
	private final int TOTAL_GENERATIONS = 10;
	private JPanel contentPane;
	private JTextField jTextField_Species;
	private JTextField jTextField_StartPopulation;
	private JTextField jTextField_Generations;
	private JTextField jTextField_Rate1;
	private JTextField jTextField_Rate2;
	private JTextField jTextField_Rate3;
	private JTextField jTextField_Rate4;
	private JTextField jTextField_Rate5;
	private JTextField jTextField_Rate6;
	private JTextField jTextField_Rate7;
	private JTextField jTextField_Rate8;
	private JTextField jTextField_Rate9;
	private JTextField jTextField_Rate10;
	private JTextField jTextField_GrowthRate;
	private JRadioButton jRadioBtn_FixedRate;
	private JRadioButton jRadioBtn_VariableRate;
	private JLabel jLabel_OverPopulation;
	private JLabel jLabel_FinalPopulation;
	private ButtonGroup buttonGroup_Options;
	private int iStart, iGrowthRate, iaGrowthRates[], iGenerations, iTotalPopulation, iOverPopulation;
	private PopulationCalculator calculator;

	/**
	 * Launch the application.
	 */
	public static void main(String[] args) {
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					Stage2 frame = new Stage2();
					frame.setVisible(true);
				} catch (Exception e) {
					e.printStackTrace();
				}
			}
		});
	}

	/**
	 * Create the frame.
	 */
	public Stage2() {
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		setBounds(100, 100, 625, 379);
		contentPane = new JPanel();
		contentPane.setBorder(new EmptyBorder(5, 5, 5, 5));
		setContentPane(contentPane);
		contentPane.setLayout(new BorderLayout(0, 0));

		JLabel lblPopulationGrowthCalculator = new JLabel("Population Growth Calculator");
		lblPopulationGrowthCalculator.setHorizontalAlignment(SwingConstants.CENTER);
		lblPopulationGrowthCalculator.setFont(new Font("Courier New", Font.BOLD, 24));
		contentPane.add(lblPopulationGrowthCalculator, BorderLayout.NORTH);

		JPanel panel = new JPanel();
		contentPane.add(panel, BorderLayout.CENTER);
		panel.setLayout(null);

		JPanel jPanel_Container = new JPanel();
		jPanel_Container.setBounds(10, 11, 200, 80);
		panel.add(jPanel_Container);
		jPanel_Container.setLayout(new GridLayout(3, 2, 5, 5));

		JLabel lblSpecies = new JLabel("Species: ");
		jPanel_Container.add(lblSpecies);

		jTextField_Species = new JTextField();
		jPanel_Container.add(jTextField_Species);
		jTextField_Species.setColumns(10);

		JLabel lblStartPopulation = new JLabel("Start Population: ");
		jPanel_Container.add(lblStartPopulation);

		jTextField_StartPopulation = new JTextField();
		jPanel_Container.add(jTextField_StartPopulation);
		jTextField_StartPopulation.setColumns(10);

		JLabel lblGenerations = new JLabel("Generations: ");
		jPanel_Container.add(lblGenerations);

		jTextField_Generations = new JTextField();
		jPanel_Container.add(jTextField_Generations);
		jTextField_Generations.setColumns(10);

		JPanel panel_1 = new JPanel();
		panel_1.setBounds(10, 150, 579, 141);
		panel.add(panel_1);
		panel_1.setLayout(null);

		jRadioBtn_FixedRate = new JRadioButton("Fixed Rate");
		jRadioBtn_FixedRate.setBounds(6, 7, 109, 23);
		panel_1.add(jRadioBtn_FixedRate);

		jRadioBtn_VariableRate = new JRadioButton("VariableRate");
		jRadioBtn_VariableRate.setBounds(6, 40, 109, 23);
		panel_1.add(jRadioBtn_VariableRate);

		buttonGroup_Options = new ButtonGroup();
		buttonGroup_Options.add(jRadioBtn_FixedRate);
		buttonGroup_Options.add(jRadioBtn_VariableRate);

		JPanel panel_2 = new JPanel();
		panel_2.setBounds(6, 70, 563, 28);
		panel_1.add(panel_2);
		panel_2.setLayout(new GridLayout(0, 10, 5, 5));

		jTextField_Rate1 = new JTextField();
		jTextField_Rate1.setHorizontalAlignment(SwingConstants.RIGHT);
		panel_2.add(jTextField_Rate1);
		jTextField_Rate1.setColumns(10);

		jTextField_Rate2 = new JTextField();
		jTextField_Rate2.setHorizontalAlignment(SwingConstants.RIGHT);
		panel_2.add(jTextField_Rate2);
		jTextField_Rate2.setColumns(10);

		jTextField_Rate3 = new JTextField();
		jTextField_Rate3.setHorizontalAlignment(SwingConstants.RIGHT);
		panel_2.add(jTextField_Rate3);
		jTextField_Rate3.setColumns(10);

		jTextField_Rate4 = new JTextField();
		jTextField_Rate4.setHorizontalAlignment(SwingConstants.RIGHT);
		panel_2.add(jTextField_Rate4);
		jTextField_Rate4.setColumns(10);

		jTextField_Rate5 = new JTextField();
		jTextField_Rate5.setHorizontalAlignment(SwingConstants.RIGHT);
		panel_2.add(jTextField_Rate5);
		jTextField_Rate5.setColumns(10);

		jTextField_Rate6 = new JTextField();
		jTextField_Rate6.setHorizontalAlignment(SwingConstants.RIGHT);
		panel_2.add(jTextField_Rate6);
		jTextField_Rate6.setColumns(10);

		jTextField_Rate7 = new JTextField();
		jTextField_Rate7.setHorizontalAlignment(SwingConstants.RIGHT);
		panel_2.add(jTextField_Rate7);
		jTextField_Rate7.setColumns(10);

		jTextField_Rate8 = new JTextField();
		jTextField_Rate8.setHorizontalAlignment(SwingConstants.RIGHT);
		panel_2.add(jTextField_Rate8);
		jTextField_Rate8.setColumns(10);

		jTextField_Rate9 = new JTextField();
		jTextField_Rate9.setHorizontalAlignment(SwingConstants.RIGHT);
		panel_2.add(jTextField_Rate9);
		jTextField_Rate9.setColumns(10);

		jTextField_Rate10 = new JTextField();
		jTextField_Rate10.setHorizontalAlignment(SwingConstants.RIGHT);
		panel_2.add(jTextField_Rate10);
		jTextField_Rate10.setColumns(10);

		JButton jButton_Exit = new JButton("Exit");
		jButton_Exit.setBounds(480, 109, 89, 23);
		panel_1.add(jButton_Exit);

		JButton jButton_Calculate = new JButton("Calculate");
		jButton_Calculate.setBounds(381, 109, 89, 23);
		panel_1.add(jButton_Calculate);

		jTextField_GrowthRate = new JTextField();
		jTextField_GrowthRate.setBounds(121, 8, 86, 20);
		panel_1.add(jTextField_GrowthRate);
		jTextField_GrowthRate.setColumns(10);

		JLabel lblFinalPopulation = new JLabel("Final Population:");
		lblFinalPopulation.setFont(new Font("Courier New", Font.BOLD, 14));
		lblFinalPopulation.setBounds(220, 11, 140, 24);
		panel.add(lblFinalPopulation);

		jLabel_FinalPopulation = new JLabel("");
		jLabel_FinalPopulation.setFont(new Font("Courier New", Font.BOLD | Font.ITALIC, 18));
		jLabel_FinalPopulation.setBounds(369, 11, 200, 24);
		panel.add(jLabel_FinalPopulation);

		jLabel_OverPopulation = new JLabel("");
		jLabel_OverPopulation.setForeground(Color.RED);
		jLabel_OverPopulation.setFont(new Font("Courier New", Font.BOLD | Font.ITALIC, 14));
		jLabel_OverPopulation.setBounds(220, 46, 369, 24);
		panel.add(jLabel_OverPopulation);

		// Add listeners
		jButton_Exit.addActionListener(new ActionListener() {

			@Override
			public void actionPerformed(ActionEvent e) {
				System.exit(0);

			}
		});

		jRadioBtn_FixedRate.addActionListener(new ActionListener() {

			@Override
			public void actionPerformed(ActionEvent e) {
				enableFixedRate(true);
			}
		});

		jRadioBtn_VariableRate.addActionListener(new ActionListener() {

			@Override
			public void actionPerformed(ActionEvent e) {
				enableFixedRate(false);

			}
		});

		jButton_Calculate.addActionListener(new ActionListener() {

			@Override
			public void actionPerformed(ActionEvent e) {
				// Calculate total population
				// Initialize Population calculator
				calculator = new PopulationCalculator();
				iStart = Integer.parseInt(jTextField_StartPopulation.getText());

				// Check whether we calculate fixed rate growth or variable rate
				// growth

				if (jRadioBtn_FixedRate.isSelected()) {
					// Fixed rate chosen
					iGrowthRate = Integer.parseInt(jTextField_GrowthRate.getText());
					iGenerations = Integer.parseInt(jTextField_Generations.getText());
					iTotalPopulation = calculator.calculateTotalPopulation(iStart, iGrowthRate, iGenerations);
				}

				if (jRadioBtn_VariableRate.isSelected()) {
					// Variable rate chosen
					iaGrowthRates = new int[TOTAL_GENERATIONS];
					iaGrowthRates[0] = Integer.parseInt(jTextField_Rate1.getText());
					iaGrowthRates[1] = Integer.parseInt(jTextField_Rate2.getText());
					iaGrowthRates[2] = Integer.parseInt(jTextField_Rate3.getText());
					iaGrowthRates[3] = Integer.parseInt(jTextField_Rate4.getText());
					iaGrowthRates[4] = Integer.parseInt(jTextField_Rate5.getText());
					iaGrowthRates[5] = Integer.parseInt(jTextField_Rate6.getText());
					iaGrowthRates[6] = Integer.parseInt(jTextField_Rate7.getText());
					iaGrowthRates[7] = Integer.parseInt(jTextField_Rate8.getText());
					iaGrowthRates[8] = Integer.parseInt(jTextField_Rate9.getText());
					iaGrowthRates[9] = Integer.parseInt(jTextField_Rate10.getText());

					iTotalPopulation = calculator.calculateTotalPopulation(iStart, iaGrowthRates);
				}

				// Update jLabels to show total populations
				jLabel_FinalPopulation.setText(String.valueOf(iTotalPopulation));

				// Check if there is any death by over population
				if (iTotalPopulation > CAPACITY) {
					iOverPopulation = iTotalPopulation - CAPACITY;

					jLabel_OverPopulation.setText(String.format("%s %s died due to over population.", iOverPopulation,
							jTextField_Species.getText()));
				}
			}
		});
	}

	public void enableFixedRate(boolean state) {
		jTextField_GrowthRate.setEnabled(state);

		jTextField_Rate1.setEnabled(!state);
		jTextField_Rate2.setEnabled(!state);
		jTextField_Rate3.setEnabled(!state);
		jTextField_Rate4.setEnabled(!state);
		jTextField_Rate5.setEnabled(!state);
		jTextField_Rate6.setEnabled(!state);
		jTextField_Rate7.setEnabled(!state);
		jTextField_Rate8.setEnabled(!state);
		jTextField_Rate9.setEnabled(!state);
		jTextField_Rate10.setEnabled(!state);

	}
}
