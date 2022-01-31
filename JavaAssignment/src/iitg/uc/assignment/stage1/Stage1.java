// Author: 				Badr Alluhaib
// Student ID: 			u3124221
// Date Created: 		19/08/2016
// Date last changed:	21/08/2016
//
// Simple Command line program to calculate Total population of species 
// after number of generations. Request user to input several inputs like Start population,
// number of generations, growth rates for each generations.
// 
// Input: Multiple values from user 
// Output: No output. Prints result to command line.

package iitg.uc.assignment.stage1;

import java.util.Scanner;

public class Stage1 {

	final static int CAPACITY = 5000;
	static int iStart, iTotalPopulation, iGrowthRate, iaGrowthRate[], iGenerations, iOverPopulation = 0;
	static char cOption;
	static PopulationCalculator calculator;

	public static void main(String[] args) {

		System.out.println("**************************************************************");
		System.out.println("*                                                            *");
		System.out.println("            Stage 1 - Growth Calculator Program              *");
		System.out.println("*                                                            *");
		System.out.println("**************************************************************\n\n\n");

		// Initialize console
		Scanner inConsole = new Scanner(System.in);

		// Initialize calculator
		calculator = new PopulationCalculator();

		// Read Starting population from console
		System.out.print("Please enter starting population: ");
		iStart = inConsole.nextInt();

		// Get number of generations
		System.out.print("Please enter number of generations: ");
		iGenerations = inConsole.nextInt();

		// Read option from user whether Fixed rate or Variable rate
		System.out.print("Please  enter F for Fixed rate or V for variable rate: ");
		cOption = inConsole.next().charAt(0);

		// Depending on option program continues
		if (cOption == 'F' || cOption == 'f') {
			// Fixed rate calculation

			System.out.print("Please enter growth rate (0 - 100):");
			iGrowthRate = inConsole.nextInt();

			// Calculate population using fixed rates
			iTotalPopulation = calculator.calculateTotalPopulation(iStart, iGrowthRate, iGenerations);

		}

		if (cOption == 'V' || cOption == 'v') {
			// Variable rate calculation
			// Initialize growth rate array
			iaGrowthRate = new int[iGenerations];

			for (int i = 1; i <= iGenerations; i++) {
				System.out.print("Enter growth rate for generation " + i + ": ");

				iaGrowthRate[i - 1] = inConsole.nextInt();
			}

			// Calculate population using variable rates
			iTotalPopulation = calculator.calculateTotalPopulation(iStart, iaGrowthRate);

		}

		// Print result
		System.out.println("\nTotal Population: " + iTotalPopulation);

		// Check if over populated
		if (iTotalPopulation > CAPACITY) {
			iOverPopulation = iTotalPopulation - CAPACITY;
		}

		// Pint over population info
		System.out.println(iOverPopulation + " died due to over population.");
		
		// Finally close scanner
		inConsole.close();

	}

}
