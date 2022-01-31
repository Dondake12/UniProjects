// Author: 				Badr Alluhaib
// Student ID: 			u3124221
// Date Created: 		19/08/2016
// Date last changed:	25/08/2016

package iitg.uc.assignment.stage3;

import java.awt.Color;
import java.awt.Graphics2D;
import java.awt.geom.Rectangle2D;
import java.awt.image.BufferedImage;

import javax.swing.JPanel;

public class DrawingArea extends JPanel {
	private static final long serialVersionUID = 1L;
	private final double dTOP_MARGIN = 20;
	private final Color COLOR_BAR = new Color(60, 179, 113);
	private final int iFIRST_POSITION = 48;
	private final int iBAR_WIDTH = 54;
	private final int iBAR_SPACING = 10;

	private BufferedImage biDrawingArea;
	private Graphics2D g2d_Painter;

	/**
	 * Method to draw graphics using current SpeciesData item
	 * 
	 * @param currentSpecies
	 */
	public void drawPopulationGraphic(SpeciesData currentSpecies) {
		// Initialize BufferedImage and Graphics2D
		biDrawingArea = new BufferedImage(getWidth(), getHeight(), BufferedImage.TYPE_INT_BGR);
		g2d_Painter = (Graphics2D) biDrawingArea.getGraphics();

		// Set background color white
		g2d_Painter.setPaint(Color.WHITE);
		g2d_Painter.fill(new Rectangle2D.Double(0, 0, biDrawingArea.getWidth(), biDrawingArea.getHeight()));

		// Local variables to use with bar being drawn
		double dPopulation;
		double dX = iFIRST_POSITION;
		double dY, dHeight, dScale;

		// Start drawing populations
		for (int i = 0; i < currentSpecies.getIaPopulations().length; i++) {

			// Get scale
			dScale = getScale(currentSpecies.getIaPopulations());

			dPopulation = currentSpecies.getIaPopulations()[i];
			dHeight = dPopulation * dScale;
			dY = getHeight() - dHeight;

			// set bar color
			g2d_Painter.setPaint(COLOR_BAR);

			// Draw actual bar
			g2d_Painter.fill(new Rectangle2D.Double(dX, dY, iBAR_WIDTH - iBAR_SPACING, dHeight));

			// increment dX
			dX += iBAR_WIDTH;

		}

		getGraphics().drawImage(biDrawingArea, 0, 0, null);

	}

	/**
	 * Method to find scale ratio of the bar to Drawing area
	 * 
	 * @param iaPopulationData
	 * @return
	 */
	private double getScale(int[] iaPopulationData) {
		double dScale;

		// First we need to find maximum value
		double dMax = getMaxPopulaion(iaPopulationData);

		// Make sure we have extra white space from the top
		double dMaxHeight = getHeight() - dTOP_MARGIN;

		// Now calculate scale
		// because population can be very high number
		// So we have to Scale high when population is lower and Scale low when
		// population is High
		dScale = dMaxHeight / dMax;

		return dScale;
	}

	/**
	 * Method to find Maximum population
	 * 
	 * @return
	 */
	public double getMaxPopulaion(int[] iaPopulation) {
		// Set initial value
		int iMax = iaPopulation[0];

		// Loop through all population data to get Highest number
		for (int i = 1; i < iaPopulation.length; i++) {
			if (iaPopulation[i] > iMax)
				iMax = iaPopulation[i];
		}

		return iMax;
	}

}
