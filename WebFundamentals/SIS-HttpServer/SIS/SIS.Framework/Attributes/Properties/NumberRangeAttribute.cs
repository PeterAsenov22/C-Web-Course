namespace SIS.Framework.Attributes.Properties
{
    using System.ComponentModel.DataAnnotations;

    public class NumberRangeAttribute : ValidationAttribute
    {
        private readonly double minimumValue;
        private readonly double maximumValue;

        public NumberRangeAttribute(int minimumValue, int maximumValue)
        {
            this.minimumValue = minimumValue;
            this.maximumValue = maximumValue;
        }

        public override bool IsValid(object value)
        {
            return this.minimumValue <= (double) value && this.maximumValue >= (double) value;
        }
    }
}
