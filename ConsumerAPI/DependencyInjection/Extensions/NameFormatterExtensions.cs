using MassTransit;
using System.Reflection;

namespace ConsumerAPI.DependencyInjection.Extensions
{
    internal static class NameFormatterExtensions
    {
        public static string ToKebabCaseString(this MemberInfo member)
            => KebabCaseEndpointNameFormatter.Instance.SanitizeName(member.Name);
    }

    internal class KebabCaseEntityNameFormatter : IEntityNameFormatter
    {
        public string FormatEntityName<T>()
            => typeof(T).ToKebabCaseString();
    }
}
