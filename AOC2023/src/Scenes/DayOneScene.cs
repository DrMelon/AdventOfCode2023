using System;
using System.Collections.Generic;
using System.Diagnostics;
using AOC2023.Common;
using Lutra.Utility;
using System.Linq;

namespace AOC2023.Scenes;

public class DayOneScene : DayScene
{
   public DayOneScene()
   {
      Day = 1;
   }
   

   
   public override void StarOne()
   {
      int digitSum = 0;

      foreach (var line in Input)
      {
         Util.Log($"Searching in '{line}' for first and last digits.");
         // Scan from left, find first digit.
         char firstDigit = '0';
         for (int i = 0; i < line.Length; i++)
         {
            if (CharHelpers.IsDigit(line[i]))
            {
               firstDigit = line[i];
               break;
            }
         }
         Util.Log($"  *  First digit: {firstDigit}");

         // Scan from right, find last digit.
         char lastDigit = firstDigit;
         for (int i = line.Length - 1; i >= 0; i--)
         {
            if (CharHelpers.IsDigit(line[i]))
            {
               lastDigit = line[i];
               break;
            }
         }
         Util.Log($"  *  Last digit: {lastDigit}");


         // Create double digit number by concatenating the digits.
         int doubleDigitNumber = int.Parse(firstDigit.ToString() + lastDigit);
         Util.Log($"  *  Double digit number: {doubleDigitNumber}.");

         // Add to sum.
         digitSum += doubleDigitNumber;
      }
        
      Util.Log($"Catapult calibration complete. Result: {digitSum}!");
      SimpleOutput.String = $"Catapult calibration complete. Result: {digitSum}!";
      SimpleOutput.Refresh();
   }

   public override void StarTwo()
   {
      int digitSum = 0;

      foreach (var line in Input)
      {
         Util.Log($"Searching in '{line}' for first and last digits.");
         // Scan from left, find first digit.
         char firstDigit = '0';
         int firstDigitLocation = line.Length - 1;
         for (int i = 0; i < line.Length; i++)
         {
            if (CharHelpers.IsDigit(line[i]))
            {
               firstDigit = line[i];
               firstDigitLocation = i;
               break;
            }
         }
         Util.Log($"  *  First numeric digit: {firstDigit}");

         // Scan from right, find last digit.
         char lastDigit = firstDigit;
         int lastDigitLocation = 0;
         for (int i = line.Length - 1; i >= 0; i--)
         {
            if (CharHelpers.IsDigit(line[i]))
            {
               lastDigit = line[i];
               lastDigitLocation = i;
               break;
            }
         }
         Util.Log($"  *  Last numeric digit: {lastDigit}");
         
         // Now, find the first and last "textual digits".
         var zero = GetFirstAndLastIndicesOfSubstring(line, "zero");
         var one = GetFirstAndLastIndicesOfSubstring(line, "one");
         var two = GetFirstAndLastIndicesOfSubstring(line, "two");
         var three = GetFirstAndLastIndicesOfSubstring(line, "three");
         var four = GetFirstAndLastIndicesOfSubstring(line, "four");
         var five = GetFirstAndLastIndicesOfSubstring(line, "five");
         var six = GetFirstAndLastIndicesOfSubstring(line, "six");
         var seven = GetFirstAndLastIndicesOfSubstring(line, "seven");
         var eight = GetFirstAndLastIndicesOfSubstring(line, "eight");
         var nine = GetFirstAndLastIndicesOfSubstring(line, "nine");

         SetFirstLastDigitIfIndicesBetter(zero, ref firstDigit, ref lastDigit, ref firstDigitLocation, ref lastDigitLocation, '0');
         SetFirstLastDigitIfIndicesBetter(one, ref firstDigit, ref lastDigit, ref firstDigitLocation, ref lastDigitLocation, '1');
         SetFirstLastDigitIfIndicesBetter(two, ref firstDigit, ref lastDigit, ref firstDigitLocation, ref lastDigitLocation, '2');
         SetFirstLastDigitIfIndicesBetter(three, ref firstDigit, ref lastDigit, ref firstDigitLocation, ref lastDigitLocation, '3');
         SetFirstLastDigitIfIndicesBetter(four, ref firstDigit, ref lastDigit, ref firstDigitLocation, ref lastDigitLocation, '4');
         SetFirstLastDigitIfIndicesBetter(five, ref firstDigit, ref lastDigit, ref firstDigitLocation, ref lastDigitLocation, '5');
         SetFirstLastDigitIfIndicesBetter(six, ref firstDigit, ref lastDigit, ref firstDigitLocation, ref lastDigitLocation, '6');
         SetFirstLastDigitIfIndicesBetter(seven, ref firstDigit, ref lastDigit, ref firstDigitLocation, ref lastDigitLocation, '7');
         SetFirstLastDigitIfIndicesBetter(eight, ref firstDigit, ref lastDigit, ref firstDigitLocation, ref lastDigitLocation, '8');
         SetFirstLastDigitIfIndicesBetter(nine, ref firstDigit, ref lastDigit, ref firstDigitLocation, ref lastDigitLocation, '9');


         // Create double digit number by concatenating the digits.
         int doubleDigitNumber = int.Parse(firstDigit.ToString() + lastDigit);
         Util.Log($"  *  Double digit number: {doubleDigitNumber}.");

         // Add to sum.
         digitSum += doubleDigitNumber;
      }
        
      Util.Log($"Catapult calibration (Star 2) complete. Result: {digitSum}!");
      SimpleOutput.String = $"Catapult calibration (Star 2) complete. Result: {digitSum}!";
      SimpleOutput.Refresh();
   }

   public (int, int) GetFirstAndLastIndicesOfSubstring(string line, string match)
   {
      int firstOne = line.IndexOf(match);
      int lastOne = line.LastIndexOf(match);
      
      Util.Log($"  *  Found {match} at {firstOne} and {lastOne}.");

      return (firstOne, lastOne);
   }

   public void SetFirstLastDigitIfIndicesBetter((int, int) pairToCheck, ref char firstDigit, ref char lastDigit, ref int firstDigitLocation, ref int lastDigitLocation, char pairSet)
   {
      if (pairToCheck.Item1 != -1)
      {
         if (pairToCheck.Item1 < firstDigitLocation)
         {
            Util.Log($"  *  First digit was {firstDigit}, is now {pairSet}.");
            firstDigitLocation = pairToCheck.Item1;
            firstDigit = pairSet;
         }

         if (pairToCheck.Item2 > lastDigitLocation)
         {
            Util.Log($"  *  Last digit was {lastDigit}, is now {pairSet}.");
            lastDigitLocation = pairToCheck.Item2;
            lastDigit = pairSet;
         }
      }
   }
}