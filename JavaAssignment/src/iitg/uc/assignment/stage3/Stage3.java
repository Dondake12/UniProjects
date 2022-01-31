// Author: 				Badr Alluhaib
// Student ID: 			u3124221
// Date Created: 		19/08/2016
// Date last changed:	25/08/2016
//
// Extended Stage2 GUI program. Now program reads all Species information from a file and shows in GUI elements.
// Also shows population information on Graphic.
// User no need to input information on GUI elements.
//

package iitg.uc.assignment.stage3;

import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.EventQueue;
import java.awt.Font;
import java.awt.GridLayout;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.IOException;
import java.util.List;

import javax.swing.JButton;
import javax.swing.JComboBox;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.SwingConstants;
import javax.swing.UIManager;
import javax.swing.border.EmptyBorder;
import javax.swing.border.EtchedBorder;
import javax.swing.border.LineBorder;
import javax.swing.border.TitledBorder;

public class Stage3 extends JFrame {

	private JPanel contentPane;
	private JComboBox<SpeciesData> jComboBox_Species;
	private List<SpeciesData> lData;
	private JLabel jLabel_StartPopulation;
	private JLabel jLabel_Rate1;
	private JLabel jLabel_Rate2;
	private JLabel jLabel_Rate3;
	private JLabel jLabel_Rate4;
	private JLabel jLabel_Rate5;
	private JLabel jLabel_Rate6;
	private JLabel jLabel_Rate7;
	private JLabel jLabel_Rate8;
	private JLabel jLabel_Rate9;
	private JLabel jLabel_Rate10;
	private JLabel jLabel_Population1;
	private JLabel jLabel_OverPopulation;
	private JLabel jLabel_FinalPopulation;
	private JLabel jLabel_Population10;
	private JLabel jLabel_Population9;
	private JLabel jLabel_Population8;
	private JLabel jLabel_Population7;
	private JLabel jLabel_Population6;
	private JLabel jLabel_Population5;
	private JLabel jLabel_Population4;
	private JLabel jLabel_Population3;
	private JLabel jLabel_Population2;
	private JLabel jLabel_Habitat;
	private final String STRING_OVERPOPULATION = "%s %s died due to overpopulation.";
	private JPanel panel_3;
	private DrawingArea jPanel_DrawingArea;
	private boolean isDrawingReady = false;

	/**
	 * Launch the application.
	 */
	public static void main(String[] args) {
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					Stage3 frame = new Stage3();
					frame.setVisible(true);
				} catch (Exception e) {
					e.printStackTrace();
				}
			}
		});
	}

	/**
	 * Create the frame.
	 * 
	 * @throws IOException
	 */
	public Stage3() throws IOException {
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		setBounds(100, 100, 650, 610);
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
		jPanel_Container.setBounds(10, 11, 604, 30);
		panel.add(jPanel_Container);
		jPanel_Container.setLayout(new GridLayout(0, 6, 5, 5));

		JLabel lblSpecies = new JLabel("Species: ");
		jPanel_Container.add(lblSpecies);

		jComboBox_Species = new JComboBox<SpeciesData>();
		jComboBox_Species.setFont(new Font("Courier New", Font.PLAIN, 12));
		jPanel_Container.add(jComboBox_Species);

		JLabel lblHabitat = new JLabel("Habitat:");
		jPanel_Container.add(lblHabitat);

		jLabel_Habitat = new JLabel("");
		jLabel_Habitat.setBackground(Color.WHITE);
		jPanel_Container.add(jLabel_Habitat);

		JLabel lblStartPopulation = new JLabel("Population: ");
		jPanel_Container.add(lblStartPopulation);

		jLabel_StartPopulation = new JLabel();
		jLabel_StartPopulation.setBackground(Color.WHITE);
		jPanel_Container.add(jLabel_StartPopulation);

		JPanel panel_1 = new JPanel();
		panel_1.setBounds(10, 52, 604, 437);
		panel.add(panel_1);
		panel_1.setLayout(null);

		JPanel panel_2 = new JPanel();
		panel_2.setBounds(6, 29, 588, 97);
		panel_1.add(panel_2);
		panel_2.setLayout(new GridLayout(3, 10, 5, 5));

		JLabel lblGen = new JLabel("Gen 1");
		lblGen.setFont(new Font("Times New Roman", Font.BOLD, 12));
		lblGen.setHorizontalAlignment(SwingConstants.RIGHT);
		panel_2.add(lblGen);

		JLabel lblGen_1 = new JLabel("Gen 2");
		lblGen_1.setFont(new Font("Times New Roman", Font.BOLD, 12));
		lblGen_1.setHorizontalAlignment(SwingConstants.RIGHT);
		panel_2.add(lblGen_1);

		JLabel lblGen_2 = new JLabel("Gen 3");
		lblGen_2.setFont(new Font("Times New Roman", Font.BOLD, 12));
		lblGen_2.setHorizontalAlignment(SwingConstants.RIGHT);
		panel_2.add(lblGen_2);

		JLabel lblGen_3 = new JLabel("Gen 4");
		lblGen_3.setFont(new Font("Times New Roman", Font.BOLD, 12));
		lblGen_3.setHorizontalAlignment(SwingConstants.RIGHT);
		panel_2.add(lblGen_3);

		JLabel lblGen_4 = new JLabel("Gen 5");
		lblGen_4.setFont(new Font("Times New Roman", Font.BOLD, 12));
		lblGen_4.setHorizontalAlignment(SwingConstants.RIGHT);
		panel_2.add(lblGen_4);

		JLabel lblGen_5 = new JLabel("Gen 6");
		lblGen_5.setFont(new Font("Times New Roman", Font.BOLD, 12));
		lblGen_5.setHorizontalAlignment(SwingConstants.RIGHT);
		panel_2.add(lblGen_5);

		JLabel lblGen_6 = new JLabel("Gen 7");
		lblGen_6.setFont(new Font("Times New Roman", Font.BOLD, 12));
		lblGen_6.setHorizontalAlignment(SwingConstants.RIGHT);
		panel_2.add(lblGen_6);

		JLabel lblGen_7 = new JLabel("Gen 8");
		lblGen_7.setFont(new Font("Times New Roman", Font.BOLD, 12));
		lblGen_7.setHorizontalAlignment(SwingConstants.RIGHT);
		panel_2.add(lblGen_7);

		JLabel lblGen_8 = new JLabel("Gen 9");
		lblGen_8.setFont(new Font("Times New Roman", Font.BOLD, 12));
		lblGen_8.setHorizontalAlignment(SwingConstants.RIGHT);
		panel_2.add(lblGen_8);

		JLabel lblGen_9 = new JLabel("Gen 10");
		lblGen_9.setFont(new Font("Times New Roman", Font.BOLD, 12));
		lblGen_9.setHorizontalAlignment(SwingConstants.RIGHT);
		panel_2.add(lblGen_9);

		jLabel_Rate1 = new JLabel();
		jLabel_Rate1.setBorder(new EtchedBorder(EtchedBorder.LOWERED, null, null));
		jLabel_Rate1.setBackground(new Color(169, 169, 169));
		jLabel_Rate1.setFont(new Font("Courier New", Font.PLAIN, 12));
		jLabel_Rate1.setHorizontalAlignment(SwingConstants.CENTER);
		panel_2.add(jLabel_Rate1);

		jLabel_Rate2 = new JLabel();
		jLabel_Rate2.setBorder(new EtchedBorder(EtchedBorder.LOWERED, null, null));
		jLabel_Rate2.setBackground(new Color(169, 169, 169));
		jLabel_Rate2.setFont(new Font("Courier New", Font.PLAIN, 12));
		jLabel_Rate2.setHorizontalAlignment(SwingConstants.CENTER);
		panel_2.add(jLabel_Rate2);

		jLabel_Rate3 = new JLabel();
		jLabel_Rate3.setBorder(new EtchedBorder(EtchedBorder.LOWERED, null, null));
		jLabel_Rate3.setBackground(new Color(169, 169, 169));
		jLabel_Rate3.setFont(new Font("Courier New", Font.PLAIN, 12));
		jLabel_Rate3.setHorizontalAlignment(SwingConstants.CENTER);
		panel_2.add(jLabel_Rate3);

		jLabel_Rate4 = new JLabel();
		jLabel_Rate4.setBorder(new EtchedBorder(EtchedBorder.LOWERED, null, null));
		jLabel_Rate4.setBackground(new Color(169, 169, 169));
		jLabel_Rate4.setFont(new Font("Courier New", Font.PLAIN, 12));
		jLabel_Rate4.setHorizontalAlignment(SwingConstants.CENTER);
		panel_2.add(jLabel_Rate4);

		jLabel_Rate5 = new JLabel();
		jLabel_Rate5.setBorder(new EtchedBorder(EtchedBorder.LOWERED, null, null));
		jLabel_Rate5.setBackground(new Color(169, 169, 169));
		jLabel_Rate5.setFont(new Font("Courier New", Font.PLAIN, 12));
		jLabel_Rate5.setHorizontalAlignment(SwingConstants.CENTER);
		panel_2.add(jLabel_Rate5);

		jLabel_Rate6 = new JLabel();
		jLabel_Rate6.setBorder(new EtchedBorder(EtchedBorder.LOWERED, null, null));
		jLabel_Rate6.setBackground(new Color(169, 169, 169));
		jLabel_Rate6.setFont(new Font("Courier New", Font.PLAIN, 12));
		jLabel_Rate6.setHorizontalAlignment(SwingConstants.CENTER);
		panel_2.add(jLabel_Rate6);

		jLabel_Rate7 = new JLabel();
		jLabel_Rate7.setBorder(new EtchedBorder(EtchedBorder.LOWERED, null, null));
		jLabel_Rate7.setBackground(new Color(169, 169, 169));
		jLabel_Rate7.setFont(new Font("Courier New", Font.PLAIN, 12));
		jLabel_Rate7.setHorizontalAlignment(SwingConstants.CENTER);
		panel_2.add(jLabel_Rate7);

		jLabel_Rate8 = new JLabel();
		jLabel_Rate8.setBorder(new EtchedBorder(EtchedBorder.LOWERED, null, null));
		jLabel_Rate8.setBackground(new Color(169, 169, 169));
		jLabel_Rate8.setFont(new Font("Courier New", Font.PLAIN, 12));
		jLabel_Rate8.setHorizontalAlignment(SwingConstants.CENTER);
		panel_2.add(jLabel_Rate8);

		jLabel_Rate9 = new JLabel();
		jLabel_Rate9.setBorder(new EtchedBorder(EtchedBorder.LOWERED, null, null));
		jLabel_Rate9.setBackground(new Color(169, 169, 169));
		jLabel_Rate9.setFont(new Font("Courier New", Font.PLAIN, 12));
		jLabel_Rate9.setHorizontalAlignment(SwingConstants.CENTER);
		panel_2.add(jLabel_Rate9);

		jLabel_Rate10 = new JLabel();
		jLabel_Rate10.setBorder(new EtchedBorder(EtchedBorder.LOWERED, null, null));
		jLabel_Rate10.setBackground(new Color(169, 169, 169));
		jLabel_Rate10.setFont(new Font("Courier New", Font.PLAIN, 12));
		jLabel_Rate10.setHorizontalAlignment(SwingConstants.CENTER);
		panel_2.add(jLabel_Rate10);

		jLabel_Population1 = new JLabel("");
		jLabel_Population1.setBorder(new EtchedBorder(EtchedBorder.LOWERED, null, null));
		panel_2.add(jLabel_Population1);
		jLabel_Population1.setBackground(new Color(169, 169, 169));
		jLabel_Population1.setHorizontalAlignment(SwingConstants.CENTER);
		jLabel_Population1.setFont(new Font("Courier New", Font.BOLD, 12));

		jLabel_Population2 = new JLabel("");
		jLabel_Population2.setBorder(new EtchedBorder(EtchedBorder.LOWERED, null, null));
		panel_2.add(jLabel_Population2);
		jLabel_Population2.setBackground(new Color(169, 169, 169));
		jLabel_Population2.setHorizontalAlignment(SwingConstants.CENTER);
		jLabel_Population2.setFont(new Font("Courier New", Font.BOLD, 12));

		jLabel_Population3 = new JLabel("");
		jLabel_Population3.setBorder(new EtchedBorder(EtchedBorder.LOWERED, null, null));
		panel_2.add(jLabel_Population3);
		jLabel_Population3.setBackground(new Color(169, 169, 169));
		jLabel_Population3.setHorizontalAlignment(SwingConstants.CENTER);
		jLabel_Population3.setFont(new Font("Courier New", Font.BOLD, 12));

		jLabel_Population4 = new JLabel("");
		jLabel_Population4.setBorder(new EtchedBorder(EtchedBorder.LOWERED, null, null));
		panel_2.add(jLabel_Population4);
		jLabel_Population4.setBackground(new Color(169, 169, 169));
		jLabel_Population4.setHorizontalAlignment(SwingConstants.CENTER);
		jLabel_Population4.setFont(new Font("Courier New", Font.BOLD, 12));

		jLabel_Population5 = new JLabel("");
		jLabel_Population5.setBorder(new EtchedBorder(EtchedBorder.LOWERED, null, null));
		panel_2.add(jLabel_Population5);
		jLabel_Population5.setBackground(new Color(169, 169, 169));
		jLabel_Population5.setHorizontalAlignment(SwingConstants.CENTER);
		jLabel_Population5.setFont(new Font("Courier New", Font.BOLD, 12));

		jLabel_Population6 = new JLabel("");
		jLabel_Population6.setBorder(new EtchedBorder(EtchedBorder.LOWERED, null, null));
		panel_2.add(jLabel_Population6);
		jLabel_Population6.setBackground(new Color(169, 169, 169));
		jLabel_Population6.setHorizontalAlignment(SwingConstants.CENTER);
		jLabel_Population6.setFont(new Font("Courier New", Font.BOLD, 12));

		jLabel_Population7 = new JLabel("");
		jLabel_Population7.setBorder(new EtchedBorder(EtchedBorder.LOWERED, null, null));
		panel_2.add(jLabel_Population7);
		jLabel_Population7.setBackground(new Color(169, 169, 169));
		jLabel_Population7.setHorizontalAlignment(SwingConstants.CENTER);
		jLabel_Population7.setFont(new Font("Courier New", Font.BOLD, 12));

		jLabel_Population8 = new JLabel("");
		jLabel_Population8.setBorder(new EtchedBorder(EtchedBorder.LOWERED, null, null));
		panel_2.add(jLabel_Population8);
		jLabel_Population8.setBackground(new Color(169, 169, 169));
		jLabel_Population8.setHorizontalAlignment(SwingConstants.CENTER);
		jLabel_Population8.setFont(new Font("Courier New", Font.BOLD, 12));

		jLabel_Population9 = new JLabel("");
		jLabel_Population9.setBorder(new EtchedBorder(EtchedBorder.LOWERED, null, null));
		panel_2.add(jLabel_Population9);
		jLabel_Population9.setBackground(new Color(169, 169, 169));
		jLabel_Population9.setHorizontalAlignment(SwingConstants.CENTER);
		jLabel_Population9.setFont(new Font("Courier New", Font.BOLD, 12));

		jLabel_Population10 = new JLabel("");
		jLabel_Population10.setBorder(new EtchedBorder(EtchedBorder.LOWERED, null, null));
		panel_2.add(jLabel_Population10);
		jLabel_Population10.setBackground(new Color(169, 169, 169));
		jLabel_Population10.setHorizontalAlignment(SwingConstants.CENTER);
		jLabel_Population10.setFont(new Font("Courier New", Font.BOLD, 12));

		JLabel lblNewLabel = new JLabel("Growth rates & Population after each generation");
		lblNewLabel.setFont(new Font("Courier New", Font.BOLD, 12));
		lblNewLabel.setBounds(6, 11, 588, 14);
		panel_1.add(lblNewLabel);

		JLabel lblFinalPopulation = new JLabel("Final Population:");
		lblFinalPopulation.setBounds(6, 137, 140, 24);
		panel_1.add(lblFinalPopulation);
		lblFinalPopulation.setFont(new Font("Courier New", Font.BOLD, 14));

		jLabel_FinalPopulation = new JLabel("");
		jLabel_FinalPopulation.setBorder(new LineBorder(new Color(0, 0, 0)));
		jLabel_FinalPopulation.setBackground(new Color(169, 169, 169));
		jLabel_FinalPopulation.setBounds(155, 137, 140, 24);
		panel_1.add(jLabel_FinalPopulation);
		jLabel_FinalPopulation.setFont(new Font("Courier New", Font.BOLD | Font.ITALIC, 18));

		jLabel_OverPopulation = new JLabel("");
		jLabel_OverPopulation.setBorder(new LineBorder(new Color(0, 0, 0)));
		jLabel_OverPopulation.setBackground(new Color(169, 169, 169));
		jLabel_OverPopulation.setBounds(6, 172, 588, 24);
		panel_1.add(jLabel_OverPopulation);
		jLabel_OverPopulation.setForeground(Color.RED);
		jLabel_OverPopulation.setFont(new Font("Courier New", Font.BOLD | Font.ITALIC, 14));

		panel_3 = new JPanel();
		panel_3.setBorder(new TitledBorder(
				new TitledBorder(UIManager.getBorder("TitledBorder.border"), "", TitledBorder.LEADING, TitledBorder.TOP,
						null, new Color(0, 0, 0)),
				"Population Graphic", TitledBorder.LEADING, TitledBorder.TOP, null, new Color(0, 0, 0)));
		panel_3.setBounds(6, 207, 588, 219);
		panel_1.add(panel_3);
		panel_3.setLayout(new BorderLayout(0, 0));

		jPanel_DrawingArea = new DrawingArea();
		panel_3.add(jPanel_DrawingArea, BorderLayout.CENTER);

		JButton jButton_Exit = new JButton("Exit");
		jButton_Exit.setBounds(500, 500, 89, 23);
		panel.add(jButton_Exit);

		// Add listeners
		jButton_Exit.addActionListener(new ActionListener() {

			@Override
			public void actionPerformed(ActionEvent e) {
				System.exit(0);

			}
		});

		jComboBox_Species.addActionListener(new ActionListener() {

			@Override
			public void actionPerformed(ActionEvent e) {
				if (jComboBox_Species.getSelectedIndex() != -1) {

					// When item selected fill data
					SpeciesData currentItem = (SpeciesData) jComboBox_Species.getSelectedItem();

					showSpeciesData(currentItem);

					// Draw graphic
					if (isDrawingReady) {
						jPanel_DrawingArea.drawPopulationGraphic(currentItem);
					}

				} else {
					cleanFields();
				}

			}

		});

		Stage3FileReader fileReader = new Stage3FileReader();
		lData = fileReader.readSpeciesData();

		for (SpeciesData newSpecies : lData) {
			jComboBox_Species.addItem(newSpecies);
		}

		// Make sure that in the beginning NO species selected
		jComboBox_Species.setSelectedIndex(-1);

		// When we add new species to the JComboBox its action Listener executes
		// But we can not start drawing graphic since program is not shown in
		// the screen
		// till its shown we can't draw so we use boolean to wait program shown
		// on the screen
		isDrawingReady = true;

	}

	/**
	 * Method shows curently selected species info on the Screen by updating
	 * Jlabels and JTextfield
	 * 
	 * @param currentItem
	 */
	private void showSpeciesData(SpeciesData currentItem) {
		jLabel_Habitat.setText(currentItem.getsHabitat());
		jLabel_StartPopulation.setText(String.valueOf(currentItem.getiStartPopulation()));

		jLabel_Rate1.setText(String.valueOf(currentItem.getIaGrowthRates()[0]));
		jLabel_Rate2.setText(String.valueOf(currentItem.getIaGrowthRates()[1]));
		jLabel_Rate3.setText(String.valueOf(currentItem.getIaGrowthRates()[2]));
		jLabel_Rate4.setText(String.valueOf(currentItem.getIaGrowthRates()[3]));
		jLabel_Rate5.setText(String.valueOf(currentItem.getIaGrowthRates()[4]));
		jLabel_Rate6.setText(String.valueOf(currentItem.getIaGrowthRates()[5]));
		jLabel_Rate7.setText(String.valueOf(currentItem.getIaGrowthRates()[6]));
		jLabel_Rate8.setText(String.valueOf(currentItem.getIaGrowthRates()[7]));
		jLabel_Rate9.setText(String.valueOf(currentItem.getIaGrowthRates()[8]));
		jLabel_Rate10.setText(String.valueOf(currentItem.getIaGrowthRates()[9]));

		jLabel_Population1.setText(String.valueOf(currentItem.getIaPopulations()[0]));
		jLabel_Population2.setText(String.valueOf(currentItem.getIaPopulations()[1]));
		jLabel_Population3.setText(String.valueOf(currentItem.getIaPopulations()[2]));
		jLabel_Population4.setText(String.valueOf(currentItem.getIaPopulations()[3]));
		jLabel_Population5.setText(String.valueOf(currentItem.getIaPopulations()[4]));
		jLabel_Population6.setText(String.valueOf(currentItem.getIaPopulations()[5]));
		jLabel_Population7.setText(String.valueOf(currentItem.getIaPopulations()[6]));
		jLabel_Population8.setText(String.valueOf(currentItem.getIaPopulations()[7]));
		jLabel_Population9.setText(String.valueOf(currentItem.getIaPopulations()[8]));
		jLabel_Population10.setText(String.valueOf(currentItem.getIaPopulations()[9]));

		jLabel_FinalPopulation.setText(String.valueOf(currentItem.getiTotalPopulation()));
		jLabel_OverPopulation.setText(
				String.format(STRING_OVERPOPULATION, currentItem.getiOverPopulation(), currentItem.getsName()));

	}

	/**
	 * Clears all Jlabels and JTextfields
	 */
	public void cleanFields() {
		jLabel_StartPopulation.setText("");
		jLabel_Rate1.setText("");
		jLabel_Rate2.setText("");
		jLabel_Rate3.setText("");
		jLabel_Rate4.setText("");
		jLabel_Rate5.setText("");
		jLabel_Rate6.setText("");
		jLabel_Rate7.setText("");
		jLabel_Rate8.setText("");
		jLabel_Rate9.setText("");
		jLabel_Rate10.setText("");

		jLabel_FinalPopulation.setText("");
		jLabel_OverPopulation.setText("");
		jLabel_Habitat.setText("");

		jLabel_Population1.setText("");
		jLabel_Population2.setText("");
		jLabel_Population3.setText("");
		jLabel_Population4.setText("");
		jLabel_Population5.setText("");
		jLabel_Population6.setText("");
		jLabel_Population7.setText("");
		jLabel_Population8.setText("");
		jLabel_Population9.setText("");
		jLabel_Population10.setText("");
	}
}
