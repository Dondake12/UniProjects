// Author: 				Badr Alluhaib
// Student ID: 			u3124221
// Date Created: 		22/08/2016
// Date last changed:	31/08/2016
// 
// Database report generator class.
// When Species information supplied this class used to generated database report
// and save it in a file.


package iitg.uc.assignment.stage4;

import java.io.BufferedWriter;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.io.PrintWriter;
import java.util.List;

import iitg.uc.assignment.stage3.SpeciesData;

public class ReportGenerator {

	private final String REPORT_NAME = "database_report.txt";
	private final String sHEADER_TEXT = "              Species generation population Report";
	private final String sHEADER_SEPARATOR = "                       *   *   *   *   *                                  \n";
	private final String sLine_SEPARATOR = "--------------------------------------------------------------------------------------------------------------------------------------------------------------------------";
	private final String sDATA_HEADER = "| Species         | Habitat            | Growth rates                                                                   | Start Population | Final Population | Death    |";
	private final String sSPECIES_BODY_LINE = "| %1$-16s| %2$-18s | %3$6d| %4$6d| %5$6d| %6$6d| %7$6d| %8$6d| %9$6d| %10$6d| %11$6d| %12$6d |%13$18d|%14$18d|%15$10d|";
	private String sReportLocation;

	private PrintWriter pwReportWriter;

	public ReportGenerator() {
		// Initialize PrintWriter object
		try {
			File fReport = new File(REPORT_NAME);
			pwReportWriter = new PrintWriter(new BufferedWriter(new FileWriter(fReport)));
			sReportLocation = fReport.getAbsolutePath();
			System.out.println("Location:" + sReportLocation);
		} catch (IOException e) {
			e.printStackTrace();
		}
	}

	/**
	 * Method generate Database report and save in a file
	 * @param lSpecies
	 */
	public void generateReport(List<SpeciesData> lSpecies) {

		printHeader(pwReportWriter);

		pwReportWriter.println(sDATA_HEADER);
		pwReportWriter.println(sLine_SEPARATOR);

		for (SpeciesData species : lSpecies) {

			int[] iaRates = species.getIaGrowthRates();

			pwReportWriter.println(String.format(sSPECIES_BODY_LINE, species.getsName(), species.getsHabitat(),
					iaRates[0], iaRates[1], iaRates[2], iaRates[3], iaRates[4], iaRates[5], iaRates[6], iaRates[7],
					iaRates[8], iaRates[9], species.getiStartPopulation(), species.getiTotalPopulation(),
					species.getiOverPopulation()));
		}

		pwReportWriter.close();
	}

	/**
	 * Prints header elements for the report
	 * @param pw
	 */
	private void printHeader(PrintWriter pw) {
		pw.println(sHEADER_TEXT);
		pw.println(sHEADER_SEPARATOR);
		pw.println();
	}

	/**
	 * Returns created file location
	 * @return
	 */
	public String getFileLocation() {

		return sReportLocation;

	}

}
