namespace Phyrric.Data
{
	public class AbilityScoreSet
	{
		public AbilityScore Strength { get; set; }
		public AbilityScore Dexterity { get; set; }
		public AbilityScore Constitution { get; set; }
		public AbilityScore Intelligence { get; set; }

		public int AverageScore
		{
			get
			{
				return (Strength.Value + Dexterity.Value
					+ Constitution.Value + Intelligence.Value) / 4;
			}
		}

		public AbilityScoreSet(int defaultValue = 10)
		{
			Strength = new AbilityScore(defaultValue);
			Dexterity = new AbilityScore(defaultValue);
			Constitution = new AbilityScore(defaultValue);
			Intelligence = new AbilityScore(defaultValue);
		}

		public static AbilityScoreSet GetRandomSet(int min = 5, int max = 14)
		{
			var set = new AbilityScoreSet();

			set.Strength.Value = PhyrricGame.Rng.Next(min, max);
			set.Dexterity.Value = PhyrricGame.Rng.Next(min, max);
			set.Constitution.Value = PhyrricGame.Rng.Next(min, max);
			set.Intelligence.Value = PhyrricGame.Rng.Next(min, max);

			System.Console.WriteLine($"Generated entity with avg score of: {set.AverageScore}");

			return set;
		}
	}
}
