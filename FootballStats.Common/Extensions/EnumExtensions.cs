using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballStats.Common
{
    public static class EnumExtensions
    {
        public static int Aggregate<T>()
            where T : struct
        {
            return GetItems<T>().Cast<Enum>().Aggregate();
        }

        public static int Aggregate(this IEnumerable<Enum> self)
        {
            return self.Select(s => s.AsInt()).Aggregate((r, p) => r | p);
        }

        public static IEnumerable<T> GetItems<T>(bool includeNone = false)
            where T : struct
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum)
                throw new ArgumentException("enumType must be an enumeration type.");

            var result = Enum.GetValues(enumType);
            return includeNone ? result.Cast<T>() : result.Cast<int>().Where(i => i > 0 || Enum.GetName(enumType, i) != "None").Cast<T>();
        }

        public static Dictionary<string, string> ToDictionary(Type enumType, bool includeNone = false)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException("enumType must be an enumeration type.");

            return Enum.GetValues(enumType).Cast<int>().Where(i => includeNone || i > 0 || Enum.GetName(enumType, i) != "None").ToDictionary(v => Enum.GetName(enumType, v), v => v.ToString());
        }

        public static Dictionary<string, string> ToDictionary<EnumType>(bool includeNone = false)
        {
            return ToDictionary(typeof(EnumType), includeNone);
        }

        public static bool HasFlag(this int self, int flag)
        {
            return (self & flag) == flag;
        }

        public static int AddFlag(this int self, int flag)
        {
            return self | flag;
        }

        public static int RemoveFlag(this int self, int flag)
        {
            return self & ~flag;
        }

        public static TEnum ToggleFlag<TEnum>(this Enum self, TEnum flag, bool toggle)
            where TEnum : struct, IConvertible
        {
            if (!self.GetType().IsEquivalentTo(typeof(TEnum)))
                throw new ArgumentException("Enum value and flag types don't match.");

            return (TEnum)(ValueType)(toggle ?
                Convert.ToInt32(self) | Convert.ToInt32(flag) :
                Convert.ToInt32(self) & ~Convert.ToInt32(flag));
        }
    }
}
