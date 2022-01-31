// Author: 				Badr Alluhaib
// Student ID: 			u3124221
// Date Created: 		19/08/2016
// Date last changed:	22/08/2016
//
// Simple Java class to use for reading given file to obtain Species data.
// 
//

package iitg.uc.assignment.stage3;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import iitg.uc.assignment.stage1.PopulationCalculator;

public class Stage3FileReader {

	private final String FILE_NAME = "growth.txt";
	private final String FIELD_DELIM = ";";
	private final String RATE_DELIM = ",";
	private BufferedReader bfInput;
	private List<SpeciesData> lData;
	private PopulationCalculator calculator;

	public Stage3FileReader() throws IOException {
		// initialize file reader
		bfInput = new BufferedReader(new FileReader(new File(FILE_NAME)));

		// initialize List object
		lData = new ArrayList<>();

		// initialize PopulationCalculator
		calculator = new PopulationCalculator();
	}

	/**
	 * Method reads a file which contains Species data in a CSV format. Go
	 * through each line and convert data to SpeciesData object. In the end
	 * returns List<SpeciesData> object which contains all species info.
	 * 
	 * @return
	 * @throws IOException
	 */
	public List<SpeciesData> readSpeciesData() throws IOException {

		String sLine;

		// Read line by line till end of file
		while ((sLine = bfInput.readLine()) != null) {

			// Separate fields using semicolon
			String sTemp[] = sLine.split(FIELD_DELIM);

			// Initialize new Species Data Object
			SpeciesData sdSpecies = new SpeciesData();

			sdSpecies.setsName(sTemp[0]);
			sdSpecies.setsHabitat(sTemp[1]);
			sdSpecies.setiStartPopulation(Integer.parseInt(sTemp[2]));

			// get rates for species
			String[] sRates = sTemp[3].split(RATE_DELIM);
			int[] iaTempRates = new int[sRates.length];

			// For each growth rates read from file which is in String form
			// Convert to int and put in a int array element
			for (int i = 0; i < sRates.length; i++) {
				iaTempRates[i] = Integer.parseInt(sRates[i]);
			}

			sdSpecies.setIaGrowthRates(iaTempRates);

			// Calculate total population for species
			calculator.calculateTotalPopulation(sdSpecies);

			lData.add(sdSpecies);
		}
		return lData;
	}

}
