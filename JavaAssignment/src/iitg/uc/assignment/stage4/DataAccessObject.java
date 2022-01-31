// Author: 				Badr Alluhaib
// Student ID: 			u3124221
// Date Created: 		22/08/2016
// Date last changed:	31/08/2016
//
// Simple Java class which can retrieve Species data from database
//

package iitg.uc.assignment.stage4;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.List;

import iitg.uc.assignment.stage1.PopulationCalculator;
import iitg.uc.assignment.stage3.SpeciesData;

public class DataAccessObject {

	// Selection query
	private final String sSELECTION_QUERY = "SELECT speciesName, habitat, startingPopulation, Gen1_Growth, Gen2_Growth, "
			+ "Gen3_Growth, Gen4_Growth, Gen5_Growth, Gen6_Growth, Gen7_Growth, Gen8_Growth, Gen9_Growth, Gen10_Growth FROM tblSpecies ORDER BY speciesName";
	private final String sCONNECTION_NAME = "sun.jdbc.odbc.JdbcOdbcDriver";
	private final String sCONNECTION_STRING = "jdbc:odbc:species";
	private Connection cConnection;
	private Statement sStatement;
	private ResultSet rsResults;
	private List<SpeciesData> lData = new ArrayList<>();
	private final PopulationCalculator scCalculator = new PopulationCalculator();

	/**
	 * Reads data from Database and save in List<SpeciesData> object
	 * @return
	 */
	public List<SpeciesData> getSpeciesData() {

		try {
			Class.forName(sCONNECTION_NAME);

			// Create Connection object
			cConnection = DriverManager.getConnection(sCONNECTION_STRING);

			// Create statement
			sStatement = cConnection.createStatement();

			// Execute query to get data
			rsResults = sStatement.executeQuery(sSELECTION_QUERY);

			// fetch all data
			while (rsResults.next()) {
				SpeciesData speciesData = new SpeciesData();

				speciesData.setsName(rsResults.getString("speciesName"));
				speciesData.setsHabitat(rsResults.getString("habitat"));
				speciesData.setiStartPopulation(rsResults.getInt("startingPopulation"));

				speciesData.getIaGrowthRates()[0] = rsResults.getInt("Gen1_Growth");
				speciesData.getIaGrowthRates()[1] = rsResults.getInt("Gen2_Growth");
				speciesData.getIaGrowthRates()[2] = rsResults.getInt("Gen3_Growth");
				speciesData.getIaGrowthRates()[3] = rsResults.getInt("Gen4_Growth");
				speciesData.getIaGrowthRates()[4] = rsResults.getInt("Gen5_Growth");
				speciesData.getIaGrowthRates()[5] = rsResults.getInt("Gen6_Growth");
				speciesData.getIaGrowthRates()[6] = rsResults.getInt("Gen7_Growth");
				speciesData.getIaGrowthRates()[7] = rsResults.getInt("Gen8_Growth");
				speciesData.getIaGrowthRates()[8] = rsResults.getInt("Gen9_Growth");
				speciesData.getIaGrowthRates()[9] = rsResults.getInt("Gen10_Growth");

				scCalculator.calculateTotalPopulation(speciesData);

				lData.add(speciesData);

			}

			// Finally close connection
			cConnection.close();

		} catch (ClassNotFoundException e) {
			e.printStackTrace();
		} catch (SQLException e) {
			e.printStackTrace();
		}

		return lData;
	}

}
