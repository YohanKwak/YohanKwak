package comprehensive;

import java.io.File;
import java.io.FileNotFoundException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Random;
import java.util.Scanner;

/**
 * Creates a specific number of random phrases from a given grammar file according to the arguments given
 * Two arguments are expected for this class to run correctly. 
 * The first argument expected is the filepath to the grammar file
 * The second argument is the number of phrases generated
 * 
 * @author JV Virgin and Yohan Kwak
 *
 */
public class RandomPhraseGenerator {

	public static void main(String[] args) throws FileNotFoundException {

		int repetitions = Integer.parseInt(args[1]);

		ArrayList<String> start = new ArrayList<String>();
		HashMap<String, ArrayList<String>> productions = new HashMap<String, ArrayList<String>>();

		readFile(args[0], start, productions);

		for (int i = 0; i < repetitions; i++) {
			generatePhrase(productions, start);
		}

	}

	/**
	 * Generates and prints a single sentence using the given grammar
	 * 
	 * @param productions - the reference to the production rules created from the readFile() method
	 * @param start - the reference to the list of possible backbones for the randomly generated phrases
	 */
	private static void generatePhrase(HashMap<String, ArrayList<String>> productions, ArrayList<String> start) {
		//Set up a random and a stringBuilder
		Random r = new Random();
		StringBuilder returnString = new StringBuilder(start.get(r.nextInt(start.size())));
		int end;
		ArrayList<String> currentProduction;

		//Iterate through the start String, checking for non-terminals and replacing them with one of their productions
		for (int i = 0; i < returnString.length(); i++) {
			if (returnString.charAt(i) == '<') {
				end = i + 1;
				while (returnString.charAt(end) != '>') {
					end++;
				}
				currentProduction = productions.get(returnString.substring(i, end + 1));
				returnString.replace(i, end + 1, currentProduction.get(r.nextInt(currentProduction.size())));
				i--;
			}
		}
		
		//Print the resulting string
		System.out.println(returnString);
	}

	/**
	 * Takes a grammar file and stores it into the given 'productions' variable and 'start' variable
	 * 
	 * @param pathName - the file path to the desired grammar file
	 * @param start - the reference to the list of possible backbones for the randomly generated phrases
	 * @param productions - the reference to the production rules to be saved 
	 * @throws FileNotFoundException if the pathName does not lead to a grammar file
	 */
	private static void readFile(String pathName, ArrayList<String> start,
			HashMap<String, ArrayList<String>> productions) throws FileNotFoundException {

		//Create a new File from the path name, a Scanner for that file
		File file = new File(pathName);
		Scanner scnr = new Scanner(file);
		String curLine = "";
		String key;

		//Create an ArrayList to store the productions for one non-terminal
		ArrayList<String> curProductions;

		//Iterate until the end of the file has been reached
		while (scnr.hasNextLine()) {

			//Scan until an opening curly brace is found
			curLine = scnr.nextLine();
			if (curLine.equals("{")) {

				//Saves the next line as the key(non terminal) and then begins iterating through its productions
				key = scnr.nextLine();
				curLine = scnr.nextLine();

				//If the key is the start non-terminal, save that independently for later use
				if (key.equals("<start>")) {
					while (!curLine.equals("}")) {
						start.add(curLine);
						curLine = scnr.nextLine();
					}
				} else {
					
					//Creates a new list of productions and adds each subsequent line until a closing curly brace has been found
					curProductions = new ArrayList<String>();

					while (!curLine.equals("}")) {
						curProductions.add(curLine);
						curLine = scnr.nextLine();
					}

					productions.put(key, curProductions);
				}
			}

		}
		//Close the scanner
		scnr.close();

	}

}
