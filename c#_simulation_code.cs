using System;
using Accord.Statistics.Testing;


static (double[], double[]) getSamples(int seed, int numOfSamples) {
    //Temp array for rolls
    double[] temp2LineArray = new double[4];
    double[] temp3LineArray = new double[16];

    //Set seed to check for a 1000 sample distribution where p value < 0.01
    Random rnd = new Random(seed);
    int roll = rnd.Next(1, 5);
    String tempString = "4";

    for (int i = 0; i < numOfSamples; i++) {
        //Roll until we get a 4 indicating start of sequence
        while (roll != 4)
            roll = rnd.Next(1, 5);

        //Get a 4 line sequence
        for (int j = 0; j < 3; j++) {
            tempString += rnd.Next(1, 5).ToString();
        }
        // Do 2 line sorting
        temp2LineArray[tempString[1] - '1'] += 1; 

        // Do 3 line sorting
        temp3LineArray[4*(tempString[1] - '1') + tempString[2] - '1'] += 1;

        // Reset temp string
        tempString = "4";
    }
    return (temp2LineArray, temp3LineArray);
}

int[] arrayOfSampleSizes = [1000, 10000, 100000, 1000000, 10000000];

// Initializing values
double pVal = 1;
int seed = 0;

// Temp array for rolls
double[] temp2LineArray = new double[4];
double[] temp3LineArray = new double[16];

// Expected array for rolls (1000 samples)
double[] expected2LineArray = {250, 250, 250, 250};

double[] expected3LineArray = new double[16];
for(int i = 0; i < expected3LineArray.Length; i++)
        expected3LineArray[i] = 62.5;

while (pVal > 0.01) {
    (temp2LineArray, temp3LineArray) = getSamples(seed, 1000);

    ChiSquareTest chiSquareTest = new ChiSquareTest(temp2LineArray, expected2LineArray, 3);
    pVal = chiSquareTest.PValue; // gets p-value

    if (pVal < 0.01) {
        Console.WriteLine("Seed " + seed.ToString() + " has a p-value smaller than 0.01 for the 2 line patterns.");
        Console.WriteLine("___________________\n");
    }
    else
        seed++;
}

// Print out seed 116 2 and 3 line pattern counts
Console.Write("simulatedCounts = { \n");
foreach (int size in arrayOfSampleSizes) {
    for(int i = 0; i < expected2LineArray.Length; i++)
        expected2LineArray[i] = size/4;
    (temp2LineArray, temp3LineArray) = getSamples(seed, size);
    

    // Write 2 line distribution
    Console.Write("'twoLineArray" + size.ToString() + "Samples': [");
    for(int j = 0; j < expected2LineArray.Length - 1; j++)
        Console.Write(temp2LineArray[j].ToString() + ",");
    Console.Write(temp2LineArray[expected2LineArray.Length - 1].ToString() + "], \n");

    // Write 3 line distribution
    Console.Write("'threeLineArray" + size.ToString() + "Samples': [");
    for(int j = 0; j < expected3LineArray.Length - 1; j++)
        Console.Write(temp3LineArray[j].ToString() + ",");
    Console.Write(temp3LineArray[expected3LineArray.Length - 1].ToString() + "], \n");
}
Console.Write("}");
