using Domain.Resources;
using System.Globalization;
using System.Resources;

namespace Api.Resources;

public class ApiErrorResourceManager : IResourceManager
{
	private static readonly ResourceManager _resourceManager;

	static ApiErrorResourceManager()
	{
		_resourceManager = new ResourceManager(typeof(ApiErrorResource));
	}

	public string GetString(string name, CultureInfo culture) =>
		_resourceManager.GetString(name, culture);
}