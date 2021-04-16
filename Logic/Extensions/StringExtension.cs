using System.Text;

namespace Logic.Extensions
{
	public static class StringExtension
	{
		/// <summary>
		/// Return string without white spaces and in upper case, for easier comparing string to string
		/// </summary>
		/// <param name="input">String to transform</param>
		/// <returns>String to compare</returns>
		public static string ToComparable(this string input) =>
			input.Replace(" ", string.Empty).ToString().ToUpper();
	}
}
