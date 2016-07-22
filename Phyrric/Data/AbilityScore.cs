namespace Phyrric.Data
{
	public class AbilityScore
	{
		public int Value { get; set; }
		public int Bonus { get { return (Value - 10) / 2; } }

		public AbilityScore(int val)
		{
			Value = val;
		}
	}
}
