using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Common;
using Xunit;

namespace AdventOfCode2020
{
    public class Day21
    {
        private static Regex foodRegex = new Regex(@"^(?<ingredients>[^(]+) \(contains (?<allergens>[^)]+)\)$");

        [Fact]
        public void Part1()
        {
            List<Item> items = File.ReadAllLines("Day21Input.txt").Select(line => new Item(line)).ToList();

            Dictionary<string, List<Item>> itemsByAllergen = new Dictionary<string, List<Item>>();

            HashSet<string> allIngredients = new HashSet<string>();

            foreach (Item item in items)
            {
                foreach (string ingredient in item.Ingredients)
                {
                    allIngredients.Add(ingredient);
                }

                foreach (string allergen in item.Allergens)
                {
                    if (!itemsByAllergen.TryGetValue(allergen, out List<Item> list))
                    {
                        list = new List<Item>();
                        itemsByAllergen.Add(allergen, list);
                    }

                    list.Add(item);
                }
            }

            Dictionary<string, string[]> possibleIngredientsByAllergen = new Dictionary<string, string[]>();

            foreach (string allergen in itemsByAllergen.Keys)
            {
                string[] intersection = itemsByAllergen[allergen].Select(i => i.Ingredients).Aggregate((x, y) => x.Intersect(y).ToArray()).ToArray();
                possibleIngredientsByAllergen.Add(allergen, intersection);
            }

            foreach (string ingredient in possibleIngredientsByAllergen.Values.SelectMany(arr => arr.Select(i => i)))
            {
                allIngredients.Remove(ingredient);
            }

            int result = 0;

            foreach(string ingredient in items.SelectMany(i => i.Ingredients))
            {
                if (allIngredients.Contains(ingredient))
                {
                    result++;
                }
            }

            Assert.Equal(2380, result);
        }

        [Fact]
        public void Part2()
        {
            List<Item> items = File.ReadAllLines("Day21Input.txt").Select(line => new Item(line)).ToList();

            Dictionary<string, List<Item>> itemsByAllergen = new Dictionary<string, List<Item>>();

            HashSet<string> allIngredients = new HashSet<string>();

            foreach (Item item in items)
            {
                foreach (string ingredient in item.Ingredients)
                {
                    allIngredients.Add(ingredient);
                }

                foreach (string allergen in item.Allergens)
                {
                    if (!itemsByAllergen.TryGetValue(allergen, out List<Item> list))
                    {
                        list = new List<Item>();
                        itemsByAllergen.Add(allergen, list);
                    }

                    list.Add(item);
                }
            }

            Dictionary<string, HashSet<string>> possibleIngredientsByAllergen = new Dictionary<string, HashSet<string>>();

            foreach (string allergen in itemsByAllergen.Keys)
            {
                string[] intersection = itemsByAllergen[allergen].Select(i => i.Ingredients).Aggregate((x, y) => x.Intersect(y).ToArray()).ToArray();
                possibleIngredientsByAllergen.Add(allergen, new HashSet<string>(intersection));
            }

            foreach (string ingredient in possibleIngredientsByAllergen.Values.SelectMany(arr => arr.Select(i => i)))
            {
                allIngredients.Remove(ingredient);
            }

            foreach (string ingredient in allIngredients /* now non-allergen ingredients */)
            {
                foreach (HashSet<string> set in possibleIngredientsByAllergen.Values)
                {
                    set.Remove(ingredient);
                }
            }

            List<Allergen> unsolvedAllergens = possibleIngredientsByAllergen.Select(kvp => new Allergen() { Name = kvp.Key, PossibleIngredients = kvp.Value }).ToList();
            List<Allergen> solvedAllergens = new List<Allergen>();

            while (unsolvedAllergens.Count > 0)
            {
                Allergen current = unsolvedAllergens.First(a => a.PossibleIngredients.Count == 1);
                unsolvedAllergens.Remove(current);
                solvedAllergens.Add(current);

                current.Ingredient = current.PossibleIngredients.Single();

                foreach (Allergen a in unsolvedAllergens)
                {
                    a.PossibleIngredients.Remove(current.Ingredient);
                }
            }

            string[] resultArray = solvedAllergens.OrderBy(a => a.Name).Select(a => a.Ingredient).ToArray();
            string result = string.Join(",", resultArray);

            Assert.Equal("ktpbgdn,pnpfjb,ndfb,rdhljms,xzfj,bfgcms,fkcmf,hdqkqhh", result);
        }

        class Item
        {
            public string[] Ingredients;
            public string[] Allergens;
            
            public Item(string rawInput)
            {
                Match match = foodRegex.Match(rawInput);

                Ingredients = match.Groups["ingredients"].Value.Split(" ");
                Allergens = match.Groups["allergens"].Value.Split(", ");
            }
        }

        class Allergen
        {
            public string Name;
            public HashSet<string> PossibleIngredients;
            public string Ingredient;
        }
    }
}
