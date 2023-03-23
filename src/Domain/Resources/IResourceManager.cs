using System.Globalization;

namespace Domain.Resources;

public interface IResourceManager
{
	string GetString(string name, CultureInfo culture);
}