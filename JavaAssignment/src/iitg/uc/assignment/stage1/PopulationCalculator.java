// Author: 				Badr Alluhaib
// Student ID: 			u3124221
// Date Created: 		19/08/2016
// Date last changed:	21/08/2016
//
// Simple class used to calculate Total Population of the species 
// based on the conditions. 
// There are 3 different methods
// 1 - Calculate population using Start population, Growth rate (Fixed), and Number of generations
// 2 - Calculate population using Start population, Growth rates (Variable)
// 3 - Using SpeciesData object to calculate it's final population

package iitg.uc.assignment.stage1;

import iitg.uc.assignment.stage3.SpeciesData;

public class PopulationCalculator {

	final int HUNDRED_PERCENT = 100;

	public PopulationCalculator() {
	}

	/**
	 * Method calculates Total population when it's having fixed growth rate
	 * 
	 * @param iStart
	 *            Starting population
	 * @param iGrowthRate
	 *            Growth rate percent 0 to 100
	 * @param iGenerations
	 *            Number of generations
	 * @return
	 */
	public int calculateTotalPopulation(int iStart, int iGrowthRate, int iGenerations) {

		int iTotalPopulation = iStart;

		for (int i = 0; i < iGenerations; i++) {
			iTotalPopulation = iTotalPopulation + iTotalPopulation * iGrowthRate / HUNDRED_PERCENT;
		}

		return iTotalPopulation;
	}

	/**
	 * Method calculates total population when it's having variable growth rates
	 * 
	 * @param iStart
	 *            Starting population
	 * @param iaGrowthRates
	 *            Integer array containing growth rates for each generations
	 * @return
	 */
	public int calculateTotalPopulation(int iStart, int[] iaGrowthRates) {

		int iTotalPopulation = iStart;

		for (int iGrowthRate : iaGrowthRates) {
			iTotalPopulation = iTotalPopulation + iTotalPopulation * iGrowthRate / HUNDRED_PERCENT;
		}

		return iTotalPopulation;
	}

	/**
	 * Method calculates total population using values from given SpeciesData object
	 * 
	 * @param currentSpecies
	 */
	public void calculateTotalPopulation(SpeciesData currentSpecies) {

		int iTotalPopulation = currentSpecies.getiStartPopulation();
		int iaPopulations[] = new int[currentSpecies.getIaGrowthRates().length];

		for (int i = 0; i < currentSpecies.getIaGrowthRates().length; i++) {
			iTotalPopulation = iTotalPopulation
					+ iTotalPopulation * currentSpecies.getIaGrowthRates()[i] / HUNDRED_PERCENT;
			iaPopulations[i] = iTotalPopulation;
		}

		currentSpecies.setiTotalPopulation(iTotalPopulation);

		if (iTotalPopulation > currentSpecies.getCAPACITY()) {

			currentSpecies.setiOverPopulation((iTotalPopulation - currentSpecies.getCAPACITY()));
		}

		currentSpecies.setIaPopulations(iaPopulations);

	}

}
