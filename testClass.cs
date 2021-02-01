using System;
using System.Collections.Generic;
using Xunit;

public class TestClass{
    [Fact]
    static void testPrintMatrix(){
        string[,] testMatrix = {{"1C", "1D","1E", "1D"},{"1C", "1E","1C", "1D"}, {"1C", "1F","1C", "1D"}};
        MatrixSolver matrixSolver = new MatrixSolver(testMatrix);
        matrixSolver.printMatrix();
    }

    [Fact]
    static void testCombinationFinder(){
        string[,] testMatrix = {};
        MatrixSolver matrixSolver = new MatrixSolver(testMatrix);
        int[] hackIds = {1,2,3,4};
        IList<int[]> returnList = matrixSolver.findAllCombinationsPossible(hackIds);   
    }

    [Fact]
    static void testConvertStringListToArrayList(){
        string[,] testMatrix = {};
        MatrixSolver matrixSolver = new MatrixSolver(testMatrix);
        List<string> listOfHacks = new List<string>{"1M 2E 55", "2E 4D 34", "RE 54 78"};
        List<List<string>> hackArrays = matrixSolver.convertStringsToStringLists(listOfHacks);
    }

    [Fact]
    static void testFindWordOverlap(){
        string[] string1 = {"OM", "55", "ED", "UU"};
        string[] string2 = {"55", "ED"};
        MatrixSolver matrixSolver = new MatrixSolver(new string[,]{});
        int overlap = matrixSolver.findWordOverlap(string1, string2);
    }

    [Fact]
    static void testFindShortestSuperString(){
        List<List<string>> hackArrays = 
            new List<List<string>> {
                new List<string>{"1C", "1C", "55"}, 
                new List<string>{"55", "FF", "1C"},
                new List<string>{"BD", "E9", "BD", "55"},
                new List<string>{"55", "1C", "FF", "BD"}
            };
        MatrixSolver matrixSolver = new MatrixSolver(new string[,]{}); 
        matrixSolver.findShortestSuperStrings(hackArrays);
    }

    [Fact]
    static void testFindHackPath1(){
        string[,] testMatrix = {{"1C", "BD","55", "E9", "55"},
                                {"1C", "BD", "1C", "55", "E9"}, 
                                {"55", "E9", "E9", "BD", "BD"},
                                {"55", "FF", "FF", "1C", "1C"},
                                {"FF", "E9", "1C", "BD", "FF"}};
        MatrixSolver matrixSolver = new MatrixSolver(testMatrix);

        matrixSolver.findHackPath(new List<string>{"E9", "BD", "FF", "1C"});
    }

    [Fact]
    static void testFindHackPath_sameSquareBypass(){
        string[,] testMatrix = {{"1C", "BD","55", "E9", "55"},
                                {"1C", "BD", "1C", "55", "E9"}, 
                                {"55", "E9", "E9", "BD", "BD"},
                                {"55", "FF", "FF", "1C", "1C"},
                                {"FF", "E9", "1C", "BD", "FF"}};
        MatrixSolver matrixSolver = new MatrixSolver(testMatrix);

        List<Tuple<int, int>> expectedPath = null;
        Assert.Equal(expectedPath, matrixSolver.findHackPath(new List<string>{"1C", "BD", "BD", "1C", "1C"}));
    }

    [Fact]
    static void test_findAllPermutationsOfStrings(){
        List<List<string>> hackArrays = new List<List<string>>();
        hackArrays.Add(new List<string>{"ED", "OM", "FF", "UF", "PP"});
        hackArrays.Add(new List<string>{"ED", "OM", "FF", "UF"});
        hackArrays.Add(new List<string>{"ED", "OM", "FF"});
        hackArrays.Add(new List<string>{"ED", "OM"});
        hackArrays.Add(new List<string>{"ED"});
        MatrixSolver matrixSolver = new MatrixSolver(new string[,]{}); 
        matrixSolver.findAllPermutationsOfStrings(hackArrays);
    }

    [Fact]
    static void test_getShortestPermutation(){
        string[,] testMatrix = {{"1C", "1C","BD", "55", "1C"},
                                {"1C", "55","BD", "1D", "1C"}, 
                                {"1C", "1F","FF", "1D", "1C"},
                                {"1C", "E9","FF", "1C", "BD"},
                                {"1C", "1F","1C", "1D", "1C"}};
        List<string> hacks = new List<string>();
        hacks.Add("55 1C");
        hacks.Add("55 BD 1C");
        hacks.Add("55 1C E9");
        int bufferSize = 7;
        MatrixSolver matrixSolver = new MatrixSolver(testMatrix, hacks, bufferSize);
         List<List<string>> hackArrays = 
            new List<List<string>> {
                new List<string>{"1D", "1C"}, 
                new List<string>{"1D", "BD", "1C"},
                new List<string>{"1D", "1C", "E9"}
            };
        List<List<string>> shortestSuperStrings = matrixSolver.findShortestSuperStrings(hackArrays);
        matrixSolver.getShortestPermutation(shortestSuperStrings);
    }

    [Fact]
    static void test_findAllPathsForHacks(){
        string[,] testMatrix = {{"1C", "1C","BD", "55", "1C"},
                                {"1C", "E9","55", "BD", "1C"}, 
                                {"55", "55","55", "E9", "1C"},
                                {"1C", "1C","55", "BD", "BD"},
                                {"55", "1C","BD", "1C", "1C"}};
        List<string> hacks = new List<string>();
        hacks.Add("55 1C");
        hacks.Add("55 BD 1C");
        hacks.Add("55 1C E9");
        int bufferSize = 7;
        MatrixSolver matrixSolver = new MatrixSolver(testMatrix, hacks, bufferSize);
        matrixSolver.findAllHackPaths();
    }
    [Fact]
    static void test_findSpecificPathForHacks(){
        string[,] testMatrix = {{"1C", "1C","BD", "55", "1C"},
                                {"55", "E9","55", "BD", "1C"}, 
                                {"1C", "55","55", "E9", "1C"},
                                {"1C", "1C","55", "BD", "BD"},
                                {"1C", "1C","BD", "1C", "1C"}};
        List<string> hacks = new List<string>();
        hacks.Add("55 1C");
        hacks.Add("55 BD 1C");
        hacks.Add("55 1C E9");
        int bufferSize = 20;
        MatrixSolver matrixSolver = new MatrixSolver(testMatrix, hacks, bufferSize);
        int[] hackIds = new int[]{0};
        matrixSolver.findSpecificHackPath(hackIds);
    }
}