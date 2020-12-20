using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2020
{
    static class Day04
    {
        public static void Part1()
        {
            int result = File.ReadAllText("Day04Input.txt")
                .Split(Environment.NewLine + Environment.NewLine)
                .Select(line => line.Replace(Environment.NewLine, " "))
                .Select(data => new Passport(data))
                .Count(p => p.IsValidPart1());

            Debug.Assert(result == 210);
        }

        public static void Part2()
        {
            int result = File.ReadAllText("Day04Input.txt")
                .Split(Environment.NewLine + Environment.NewLine)
                .Select(line => line.Replace(Environment.NewLine, " "))
                .Select(data => new Passport(data))
                .Count(p => p.IsValidPart2());

            Debug.Assert(result == 131);
        }
    }

    class Passport
    {
        public Passport(string rawData)
        {
            foreach(string entry in rawData.Split(' '))
            {
                string key = entry.Substring(0, 3);
                string value = entry.Substring(4);

                switch (key)
                {
                    case "byr":
                        BirthYear = value;
                        break;
                    case "iyr":
                        IssueYear = value;
                        break;
                    case "eyr":
                        ExpirationYear = value;
                        break;
                    case "hgt":
                        Height = value;
                        break;
                    case "hcl":
                        HairColor = value;
                        break;
                    case "ecl":
                        EyeColor = value;
                        break;
                    case "pid":
                        PassportId = value;
                        break;
                    case "cid":
                        break;
                    default:
                        Debug.Fail("Unknown");
                        break;
                }
            }
        }

        public string BirthYear { get; }
        public string IssueYear { get; }
        public string ExpirationYear { get; }
        public string Height { get; }
        public string HairColor { get; }
        public string EyeColor { get; }
        public string PassportId { get; }

        public bool IsValidPart1()
        {
            return
                BirthYear != null &&
                IssueYear != null &&
                ExpirationYear != null &&
                Height != null &&
                HairColor != null &&
                EyeColor != null &&
                PassportId != null;
        }

        public bool IsValidPart2()
        {
            Regex validHairColorRegex = new Regex(@"^#[0-9a-f]{6}$");
            HashSet<string> validEyeColors = new HashSet<string>() { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };
            Regex validPassportIdRegex = new Regex(@"^[0-9]{9}$");

            return
                (BirthYear != null && BirthYear.Length == 4 && int.TryParse(BirthYear, out int birthYear) && birthYear >= 1920 && birthYear <= 2002) &&
                (IssueYear != null && IssueYear.Length == 4 && int.TryParse(IssueYear, out int issueYear) && issueYear >= 2010 && issueYear <= 2020) &&
                (ExpirationYear != null && ExpirationYear.Length == 4 && int.TryParse(ExpirationYear, out int expYear) && expYear >= 2020 && expYear <= 2030) &&
                (Height != null &&
                    ((Height.EndsWith("cm") && Height.Length == 5 && int.TryParse(Height.Substring(0, 3), out int heightCM) && heightCM >= 150 && heightCM <= 193) ||
                    (Height.EndsWith("in") && Height.Length == 4 && int.TryParse(Height.Substring(0, 2), out int heightIn) && heightIn >= 59 && heightIn <= 76))) &&
                (HairColor != null && validHairColorRegex.IsMatch(HairColor)) &&
                (EyeColor != null && validEyeColors.Contains(EyeColor)) &&
                (PassportId != null && validPassportIdRegex.IsMatch(PassportId));
        }
    }
}
