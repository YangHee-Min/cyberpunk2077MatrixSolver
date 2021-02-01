using System;
using System.Collections.Generic;
using System.Linq;

/*
This class is meant to take the hackMatrix
and the hack combos and process them in the following manner:

Find all hack combinations possible: findAllCombinationsPossible
Find shortest superstring of each combination: findShortestSuperString
Find path of one superstring: findHackPath

*/

public class MatrixSolver
{
    private string[,] hackMatrix_;
    private List<List<string>> hacks_;
    private int bufferSize_;
    public MatrixSolver(string[,] hackMatrix){
        hackMatrix_ = (string[,]) hackMatrix.Clone();
    }
    
    public MatrixSolver(string[,] hackMatrix, List<string> hacks, int bufferSize){
        hackMatrix_ = (string[,]) hackMatrix.Clone();
        setHacks(hacks);
        bufferSize_ = bufferSize;
    }

    public void setHacks(List<string> hacks){
        hacks_ = convertStringsToStringLists(hacks);
    }

    private void print(string msg){
        Console.Write(msg);
    }
    public void printMatrix(){
        int matrixLength = hackMatrix_.GetLength(1);
        int matrixWidth = hackMatrix_.GetLength(0);
        for(int i = 0; i < matrixWidth; i++){
            print("|");
            for(int j = 0; j < matrixLength; j++){
                print(hackMatrix_.GetValue(i,j) + "|");
            }
            print("\n");
        }
    }

    public List<int[]> findAllCombinationsPossible(int[] hackIds){
        int numberOfCombinations = maxAmountOfCombinations(hackIds.Length);
        List<int[]> returnList = new List<int[]>();
        for(int i = 1; i <= numberOfCombinations; i++){
            List<int> combination = new List<int>();
            for(int j = 0; j < hackIds.Length; j++){
                if((i & (1 << j)) >= 1){
                    combination.Add(hackIds[j]);
                }
            }
            int[] combinationArray = new int[combination.Count];
            combination.CopyTo(combinationArray, 0);
            returnList.Add(combinationArray);
        }
        return returnList.OrderBy(c => c.Length).ToList();
    }

    private int maxAmountOfCombinations(int matrixLength){
        int numberOfCombinations = 0;

        for(int i =0; i < matrixLength; i++){
            numberOfCombinations = (numberOfCombinations << 1) + 1;
        }
        return numberOfCombinations;
    }

    public List<List<string>> convertStringsToStringLists(List<string> listOfHacks){
        List<List<string>> returnList = new List<List<string>>();
        foreach(string hack in listOfHacks){
            returnList.Add(hack.Trim().Split(" ").ToList());
        }

        return returnList;
    }

    public int findWordOverlap(string[] firstCombo, string[] secondCombo){
        int overlap = 0;
        foreach(string element in firstCombo){
            if(element == secondCombo[overlap] && overlap < secondCombo.Length)
                overlap++;
            else
                overlap = 0;
            if(overlap == secondCombo.Length)
                break;
        }

        return overlap;
    }

    public List<List<string>> findShortestSuperStrings(List<List<string>> hackArraysOriginal){
        List<List<string>> hackArrays = new List<List<string>>(hackArraysOriginal);
        while(hackArrays.Count > 1){
            int firstOverlapIndex = -1;
            int secondOverlapIndex = -1;
            int longestOverlap = -1;
            for(int firstStringIndex = 0; firstStringIndex < hackArrays.Count; firstStringIndex++){
                for(int secondStringIndex = 0; secondStringIndex < hackArrays.Count; secondStringIndex++){
                    if(secondStringIndex == firstStringIndex)
                        continue;
                    int overlap = this.findWordOverlap(hackArrays[firstStringIndex].ToArray(), hackArrays[secondStringIndex].ToArray());
                    if( overlap > longestOverlap){
                        longestOverlap = overlap;
                        firstOverlapIndex = firstStringIndex;
                        secondOverlapIndex = secondStringIndex;
                    }
                }
            }
            if(longestOverlap == 0)
                break;

            // merge two longest words together
            if(longestOverlap != hackArrays[secondOverlapIndex].Count)
                hackArrays[firstOverlapIndex].AddRange(hackArrays[secondOverlapIndex].GetRange(longestOverlap, hackArrays[secondOverlapIndex].Count - longestOverlap));

            hackArrays.RemoveAt(secondOverlapIndex);
        }
        // get the longest string
        return hackArrays;
    }

    public List<List<List<string>>> findAllPermutationsOfStrings(List<List<string>> hackArrays){
        List<List<string>> hackArrayCopy = new List<List<string>>(hackArrays);

        Queue<List<List<string>>> permutations = new Queue<List<List<string>>>();
        List<List<string>> firstElementToBeAdded = new List<List<string>>();
        firstElementToBeAdded.Add(new List<string>(hackArrayCopy[0]));
        permutations.Enqueue(firstElementToBeAdded);
        hackArrayCopy.RemoveAt(0);

        while(hackArrayCopy.Count != 0){
            List<string> hackToBeAdded = new List<string>(hackArrayCopy[0]);
            hackArrayCopy.RemoveAt(0);
            int numberOfPermutations = permutations.Count;
            for(int j = 0; j < numberOfPermutations; j++){  
                List<List<string>> currentPermutation = new List<List<string>>(permutations.Dequeue());
                currentPermutation.Insert(0, hackToBeAdded); 
                int permutationLength = currentPermutation.Count;
                for(int i = 0; i < permutationLength; i++){
                    permutations.Enqueue(new List<List<string>>(currentPermutation));
                    if(i == permutationLength - 1)
                        break;

                    List<string> tmp = currentPermutation[i];
                    currentPermutation[i] = currentPermutation[i+1];
                    currentPermutation[i+1] = tmp;
                }
            }
        }
        return permutations.ToList();
    }

    public List<Tuple<int, int>> findHackPath(List<string> hackCombo){
        // Initialize hackMatrixIterators with 0s
        int[] hackMatrixIterators = new int[hackCombo.Count];
        for(int i =0; i < hackMatrixIterators.Length; i++){
            hackMatrixIterators[i] = 0;
        }

        // Initialize stack for actual path
        List<Tuple<int, int>> comboElementList = new List<Tuple<int, int>>();

        // if true, performing row search. Else performing column search
        bool isRowSearch = true;

        int matchingStringCount = 0;
        int x = 0;
        int y = 0;
        while(comboElementList.Count != hackCombo.Count){
            
            if(isRowSearch){
                x = hackMatrixIterators[matchingStringCount];
            }
            else{
                y = hackMatrixIterators[matchingStringCount];
            }

            hackMatrixIterators[matchingStringCount]++; 

            // skip item if it is the last one we selected
            if(comboElementList.Count != 0)
                if(x == comboElementList[matchingStringCount - 1].Item1 && y == comboElementList[matchingStringCount - 1].Item2){
                    continue;
                }

            if(comboElementList.Contains(new Tuple<int, int>(x,y)))
                continue;

            // this will be the mechanic if the current row cannot give a valid combinations.
            // In this case we need to go to the next possible combo
            if(x > hackMatrix_.GetLength(0) - 1 || y > hackMatrix_.GetLength(0) - 1){
                if(y == 0 && matchingStringCount == 0)
                    return null;
                hackMatrixIterators[matchingStringCount] = 0;
                matchingStringCount--;
                x = matchingStringCount == 0 ? 0 : comboElementList[matchingStringCount].Item1;
                y = matchingStringCount == 0 ? 0 : comboElementList[matchingStringCount].Item2;
                comboElementList.RemoveAt(comboElementList.Count - 1);
                isRowSearch = !isRowSearch;
                continue;
            }

            if(hackMatrix_[y,x] == hackCombo[matchingStringCount] || hackCombo[matchingStringCount] == "XX"){
                comboElementList.Add(new Tuple<int, int>(x, y));
                x = comboElementList[matchingStringCount].Item1;
                y = comboElementList[matchingStringCount].Item2;
                matchingStringCount++;
                // toggle isRowSearch
                isRowSearch = !isRowSearch;
            }
        }
        return comboElementList;
    }

    public List<Tuple<int, int>> getShortestPermutation(List<List<string>> shortestSuperStrings){
        List<List<List<string>>> shortestPermutationsCopy = new List<List<List<string>>>(findAllPermutationsOfStrings(shortestSuperStrings));

        while(getCurrentBufferCount(shortestPermutationsCopy) <= bufferSize_){

            foreach(List<List<string>> permutation in shortestPermutationsCopy){
                List<string> hackCombo = new List<string>();
                foreach(List<string> hack in permutation){
                    hackCombo.AddRange(hack);
                }
                List<Tuple<int, int>> hackPath = findHackPath(hackCombo);
                if(hackPath != null)
                    return hackPath;
            }
            if(shortestPermutationsCopy[0].Count == bufferSize_)
                break;

            shortestPermutationsCopy[0].Insert(0, new List<string>{"XX"});
            List<List<List<string>>> permutationXX = findAllPermutationsOfStrings(shortestPermutationsCopy[0]);
            shortestPermutationsCopy.Clear();
            shortestPermutationsCopy.AddRange(permutationXX);
        }
        return null;
    }

    private int getCurrentBufferCount(List<List<List<string>>> shortestPermutations){
        int currentBufferCount = 0;
        foreach(List<string> hackSequence in shortestPermutations[0]){
            currentBufferCount += hackSequence.Count;
        }
        return currentBufferCount;
    }

    public List<Tuple<int, int>> findSpecificHackPath(int[] hackIdCombination){
        List<List<string>> currentHackCombination = new List<List<string>>();
        foreach(int hackId in hackIdCombination){
            currentHackCombination.Add(hacks_[hackId]);
        }
        List<List<string>> shortestSuperStrings = findShortestSuperStrings(currentHackCombination);
        return getShortestPermutation(shortestSuperStrings);
    }

    private bool dictionaryContainsHackCombination(Dictionary<int[], List<Tuple<int, int>>> hackIdToPathDictionary,int[] hackIdCombination){
        foreach(KeyValuePair<int[], List<Tuple<int, int>>> hackIdToPath in hackIdToPathDictionary){
            if(hackIdToPath.Key.Length < hackIdCombination.Length)
                continue;
            if(hackIdToPath.Key.Length > hackIdCombination.Length)
                break;
            int count = 0;
            foreach(int hackId in hackIdCombination){
                if(!hackIdToPath.Key.Contains(hackId))
                    break;
                count++;
            }
            if(count == hackIdCombination.Length)
                return true;
        }
        return false;
    }

    public Dictionary<int[], List<Tuple<int, int>>> findAllHackPaths(){
        int[] hackIds = new int[hacks_.Count];
        for(int i = 0; i < hacks_.Count; i++){
            hackIds[i] = i;
        }

        List<int[]> allHackIdCombinations = findAllCombinationsPossible(hackIds);
        Dictionary<int[], List<Tuple<int, int>>> hackIdToPathDictionary = new Dictionary<int[], List<Tuple<int, int>>>();
        foreach(int[] hackIdCombination in allHackIdCombinations){
            hackIdToPathDictionary.Add(hackIdCombination,findSpecificHackPath(hackIdCombination));
        }
        return hackIdToPathDictionary;
    }
}
