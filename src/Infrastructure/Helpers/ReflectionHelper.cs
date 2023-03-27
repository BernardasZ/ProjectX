[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Infrastructure.Tests")]

namespace Infrastructure.Helpers;

internal class ReflectionHelper
{
	public static T GetPropertyValue<T>(object instance, string propertyName) =>
		(T)instance.GetType()?.GetProperty(propertyName)?.GetValue(instance);
}