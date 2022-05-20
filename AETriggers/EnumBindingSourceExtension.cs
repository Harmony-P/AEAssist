using System;
using System.Windows.Markup;

namespace AETriggers
{
    public class EnumBindingSourceExtension : MarkupExtension
    {
        public Type EnumType { get; set; }
        
        public EnumBindingSourceExtension(Type enumType)
        {
            if (enumType is null || !enumType.IsEnum)
            {
                throw new Exception("EnumType must not be null and of type Enum");
            }
            EnumType = enumType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(EnumType);
        }
    }
}