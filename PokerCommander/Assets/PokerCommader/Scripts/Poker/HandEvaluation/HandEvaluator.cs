using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// Evaluates the strength of a hand so that hands can be easily compared to each other.
/// </summary>
public static class HandEvaluator
{
	public static int EvaluateHand(CardHand hand, string cardTableFilePath, double loadFactor = 1.25, bool debug = true)
	{
		int cardNumber = hand.Cards.Length;

		Debug.Assert(cardNumber >= 5 && cardNumber <= 7);

		bool fiveCard = cardNumber >= 5;
		bool sixCard = cardNumber >= 6;
		bool sevenCard = cardNumber == 7;
		
		DateTime start = DateTime.UtcNow;
		HashMap handRankMap;

		// Load hand rank table or create one if no filename was given
		if (cardTableFilePath != null && File.Exists(cardTableFilePath))
		{
			if (debug) Console.WriteLine("Loading table from {0}", cardTableFilePath);
			handRankMap = LoadFromFile(cardTableFilePath);
		}
		else
		{
			int minHashMapSize = (fiveCard ? 2598960 : 0) + (sixCard ? 20358520 : 0) + (sevenCard ? 133784560 : 0);
			handRankMap = new HashMap((uint) (minHashMapSize * loadFactor));
			if (fiveCard)
			{
				if (debug) Console.WriteLine("Generating new five card lookup table");
				GenerateFiveCardTable(handRankMap, debug);
			}

			if (sixCard)
			{
				if (debug) Debug.Log("Generating new six card lookup table");
				GenerateSixCardTable(handRankMap, debug);
			}

			if (sevenCard)
			{
				if (debug) Debug.Log("Generating new seven card lookup table");
				GenerateSevenCardTable(handRankMap, debug);
			}
			
			SaveToFile(cardTableFilePath, handRankMap, debug);
		}

		//Evaluate Hand
		ulong bitMap = hand.GetBitmap();
		int result = (int)handRankMap[bitMap];
		
		TimeSpan elapsed = DateTime.UtcNow - start;
		if (debug) Debug.Log($"Hand evaluator setup completed in {elapsed.TotalSeconds.ToString(CultureInfo.InvariantCulture)}s");
		
		return result;
	}

	public static HandStrength GetStrength(Card[] hand)
	{
		if (hand.Length == 5)
		{
			var strength = new HandStrength();
			strength.Kickers = new List<int>();

			hand = hand.OrderBy(card => card.PrimeRank * 100 + card.PrimeSuit).ToArray();

			int rankProduct = hand.Select(card => card.PrimeRank).Aggregate((acc, r) => acc * r);
			int suitProduct = hand.Select(card => card.PrimeSuit).Aggregate((acc, r) => acc * r);

			bool straight =
				rankProduct == 8610 // 5-high straight
				|| rankProduct == 2310 // 6-high straight
				|| rankProduct == 15015 // 7-high straight
				|| rankProduct == 85085 // 8-high straight
				|| rankProduct == 323323 // 9-high straight
				|| rankProduct == 1062347 // T-high straight
				|| rankProduct == 2800733 // J-high straight
				|| rankProduct == 6678671 // Q-high straight
				|| rankProduct == 14535931 // K-high straight
				|| rankProduct == 31367009; // A-high straight

			bool flush =
				suitProduct == 147008443 // Spades
				|| suitProduct == 229345007 // Hearts
				|| suitProduct == 418195493 // Diamonds
				|| suitProduct == 714924299; // Clubs

			var cardCounts = hand.GroupBy(card => (int) card.Rank).Select(group => group).ToList();

			var fourOfAKind = -1;
			var threeOfAKind = -1;
			var onePair = -1;
			var twoPair = -1;

			foreach (var group in cardCounts)
			{
				var rank = group.Key;
				var count = group.Count();
				if (count == 4) fourOfAKind = rank;
				else if (count == 3) threeOfAKind = rank;
				else if (count == 2)
				{
					twoPair = onePair;
					onePair = rank;
				}
			}

			if (straight && flush)
			{
				strength.HandRanking = HandRanking.StraightFlush;
				strength.Kickers = hand.Select(card => (int) card.Rank).Reverse().ToList();
			}
			else if (fourOfAKind >= 0)
			{
				strength.HandRanking = HandRanking.FourOfAKind;
				strength.Kickers.Add(fourOfAKind);
				strength.Kickers.AddRange(hand
					.Where(card => (int) card.Rank != fourOfAKind)
					.Select(card => (int) card.Rank));
			}
			else if (threeOfAKind >= 0 && onePair >= 0)
			{
				strength.HandRanking = HandRanking.FullHouse;
				strength.Kickers.Add(threeOfAKind);
				strength.Kickers.Add(onePair);
			}
			else if (flush)
			{
				strength.HandRanking = HandRanking.Flush;
				strength.Kickers.AddRange(hand
					.Select(card => (int) card.Rank)
					.Reverse());
			}
			else if (straight)
			{
				strength.HandRanking = HandRanking.Straight;
				strength.Kickers.AddRange(hand
					.Select(card => (int) card.Rank)
					.Reverse());
			}
			else if (threeOfAKind >= 0)
			{
				strength.HandRanking = HandRanking.ThreeOfAKind;
				strength.Kickers.Add(threeOfAKind);
				strength.Kickers.AddRange(hand
					.Where(card => (int) card.Rank != threeOfAKind)
					.Select(card => (int) card.Rank));
			}
			else if (twoPair >= 0)
			{
				strength.HandRanking = HandRanking.TwoPair;
				strength.Kickers.Add(Math.Max(twoPair, onePair));
				strength.Kickers.Add(Math.Min(twoPair, onePair));
				strength.Kickers.AddRange(hand
					.Where(card => (int) card.Rank != twoPair && (int) card.Rank != onePair)
					.Select(card => (int) card.Rank));
			}
			else if (onePair >= 0)
			{
				strength.HandRanking = HandRanking.Pair;
				strength.Kickers.Add(onePair);
				strength.Kickers.AddRange(hand
					.Where(card => (int) card.Rank != onePair)
					.Select(card => (int) card.Rank));
			}
			else
			{
				strength.HandRanking = HandRanking.HighCard;
				strength.Kickers.AddRange(hand
					.Select(card => (int) card.Rank)
					.Reverse());
			}

			return strength;
		}

		return null;
	}

	public static void SaveToFile(string fileName, HashMap handRankMap, bool debug)
	{
		if (debug) Debug.Log($"Saving table to {fileName}");
		string table = JsonConvert.SerializeObject(handRankMap);
		File.WriteAllText(fileName, table);
	}

	private static HashMap LoadFromFile(string path)
	{
		string json = File.ReadAllText(path);
		return JsonConvert.DeserializeObject<HashMap>(json);
	}

	private static void GenerateFiveCardTable(HashMap handRankMap, bool debug)
	{
		var sourceSet = Enumerable.Range(0, 52).ToList();
		var combinations = new Combinatorics.Collections.Combinations<int>(sourceSet, 5);

		// Generate all possible 5 card hand bitmaps
		Debug.Log("Generating bitmaps");
		var handBitmaps = new List<ulong>();
		int count = 0;
		foreach (List<int> values in combinations)
		{
			if (debug && count++ % 1000 == 0) Debug.Log($"{count} / {combinations.Count}\r");
			handBitmaps.Add(values.Aggregate(0ul, (acc, el) => acc | (1ul << el)));
		}

		// Calculate hand strength of each hand
		Debug.Log("Calculating hand strength");
		var handStrengths = new Dictionary<ulong, HandStrength>();
		count = 0;
		foreach (ulong bitmap in handBitmaps)
		{
			if (debug && count++ % 1000 == 0) Debug.Log($"{count} / {handBitmaps.Count}\r");
			var hand = new CardHand(5, bitmap);
			handStrengths.Add(bitmap, GetStrength(hand.Cards));
		}

		// Generate a list of all unique hand strengths
		Debug.Log("Generating equivalence classes");
		var uniqueHandStrengths = new List<HandStrength>();
		count = 0;
		foreach (KeyValuePair<ulong, HandStrength> strength in handStrengths)
		{
			if (debug && count++ % 1000 == 0) Debug.Log("{count} / {handStrengths.Count}\r");
			uniqueHandStrengths.BinaryInsert(strength.Value);
		}

		Debug.Log($"{uniqueHandStrengths.Count} unique hand strengths");

		// Create a map of hand bitmaps to hand strength indices
		Debug.Log("Creating lookup table");
		count = 0;
		foreach (ulong bitmap in handBitmaps)
		{
			if (debug && count++ % 1000 == 0) Debug.Log($"{count} / {handBitmaps.Count}\r");
			var hand = new CardHand(5, bitmap);
			HandStrength strength = GetStrength(hand.Cards);
			var equivalence = uniqueHandStrengths.BinarySearch(strength);
			handRankMap[bitmap] = (ulong) equivalence;
		}
	}

	private static void GenerateSixCardTable(HashMap handRankMap, bool debug)
	{
		var sourceSet = Enumerable.Range(1, 52).ToList();
		var combinations = new Combinatorics.Collections.Combinations<int>(sourceSet, 6);
		int count = 0;
		foreach (List<int> cards in combinations)
		{
			if (debug && count++ % 1000 == 0) Debug.Log($"{count} / { combinations.Count}\r");
			var subsets = new Combinatorics.Collections.Combinations<int>(cards, 5);
			var subsetValues = new List<ulong>();
			foreach (List<int> subset in subsets)
			{
				ulong subsetBitmap = subset.Aggregate(0ul, (acc, el) => acc | (1ul << el));
				subsetValues.Add(handRankMap[subsetBitmap]);
			}

			ulong bitmap = cards.Aggregate(0ul, (acc, el) => acc | (1ul << el));
			handRankMap[bitmap] = subsetValues.Max();
		}
	}

	private static void GenerateSevenCardTable(HashMap handRankMap, bool debug)
	{
		var sourceSet = Enumerable.Range(1, 52).ToList();
		var combinations = new Combinatorics.Collections.Combinations<int>(sourceSet, 7);
		int count = 0;
		foreach (List<int> cards in combinations)
		{
			if (debug && count++ % 1000 == 0) Debug.Log($"{count} / { combinations.Count}\r");
			var subsets = new Combinatorics.Collections.Combinations<int>(cards, 6);
			var subsetValues = new List<ulong>();
			foreach (List<int> subset in subsets)
			{
				ulong subsetBitmap = subset.Aggregate(0ul, (acc, el) => acc | (1ul << el));
				subsetValues.Add(handRankMap[subsetBitmap]);
			}

			ulong bitmap = cards.Aggregate(0ul, (acc, el) => acc | (1ul << el));
			handRankMap[bitmap] = subsetValues.Max();
		}
	}

	// private void GenerateMonteCarloMap(int iterations)
	// {
	// 	Dictionary<ulong, ulong> monteCarloMap = new Dictionary<ulong, ulong>();
	// 	var sourceSet = Enumerable.Range(1, 52).ToList();
	// 	var combinations = new Combinatorics.Collections.Combinations<int>(sourceSet, 2);
	// 	int count = 0;
	// 	foreach (List<int> cards in combinations)
	// 	{
	// 		Debug.Log("{0}\r", count++);
	//
	// 		ulong bitmap = cards.Aggregate(0ul, (acc, el) => acc | (1ul << el));
	// 		var hand = new Hand(bitmap);
	// 		var deck = new Deck(removedCards: bitmap);
	//
	// 		ulong evaluationSum = 0;
	// 		for (int i = 0; i < iterations; i++)
	// 		{
	// 			if (deck.CardsRemaining < 13) deck.Shuffle();
	// 			evaluationSum += handRankMap[bitmap | deck.Draw(5)];
	// 		}
	//
	// 		monteCarloMap[bitmap] = evaluationSum / (ulong) iterations;
	// 	}
	//
	// 	foreach (KeyValuePair<ulong, ulong> kvp in monteCarloMap.OrderBy(kvp => kvp.Value))
	// 	{
	// 		var hand = new Hand(kvp.Key);
	// 		hand.PrintColoredCards("\t");
	// 		Debug.Log(kvp.Value);
	// 		handRankMap[kvp.Key] = kvp.Value;
	// 	}
	//
	// 	Console.ReadLine();
	// }
}
