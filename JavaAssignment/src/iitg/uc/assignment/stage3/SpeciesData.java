// Author: 				Badr Alluhaib
// Student ID: 			u3124221
// Date Created: 		19/08/2016
// Date last changed:	23/08/2016
//
// Simple Class for keeping each Species data in same place.
// Each species will have its own name, habitat, starting population and growth rates for each generations
//

package iitg.uc.assignment.stage3;

public class SpeciesData {

	private final int CAPACITY = 5000;
	private final int iNUMBER_OF_GENERATIONS = 10;
	private String sName;
	private String sHabitat;
	private int iStartPopulation = 0;
	private int[] iaGrowthRates = new int[iNUMBER_OF_GENERATIONS];
	private int[] iaPopulations = new int[iNUMBER_OF_GENERATIONS];
	private int iTotalPopulation = 0;
	private int iOverPopulation = 0;

	public String getsName() {
		return sName;
	}

	public void setsName(String sName) {
		this.sName = sName;
	}

	public String getsHabitat() {
		return sHabitat;
	}

	public void setsHabitat(String sHabitat) {
		this.sHabitat = sHabitat;
	}

	public int getiStartPopulation() {
		return iStartPopulation;
	}

	public void setiStartPopulation(int iStartPopulation) {
		this.iStartPopulation = iStartPopulation;
	}

	public int[] getIaGrowthRates() {
		return iaGrowthRates;
	}

	public void setIaGrowthRates(int[] iaGrowthRates) {
		this.iaGrowthRates = iaGrowthRates;
	}

	public int[] getIaPopulations() {
		return iaPopulations;
	}

	public void setIaPopulations(int[] iaPopulations) {
		this.iaPopulations = iaPopulations;
	}

	public int getiTotalPopulation() {
		return iTotalPopulation;
	}

	public void setiTotalPopulation(int iTotalPopulation) {
		this.iTotalPopulation = iTotalPopulation;
	}

	public int getiOverPopulation() {
		return iOverPopulation;
	}

	public void setiOverPopulation(int iOverPopulation) {
		this.iOverPopulation = iOverPopulation;
	}

	public int getCAPACITY() {
		return CAPACITY;
	}

	public String toString() {
		return getsName();
	}

}
