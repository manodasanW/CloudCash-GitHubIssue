using CloudCash.Common.Functions;
using CloudCash.Interface.BL;
using System.ComponentModel.DataAnnotations;

namespace CloudCash.Common.ModelBase
{
    public abstract record ListModelBase : IListModelBase
    {
        [Key]
        public long ID { get; set; }

        public abstract void CheckValues();

        public virtual void DoBeforeCheck() { }

        public string GetPropertyStringValue(string propertyName) => AttributeReader.GetPropertyLocalizationStringValue(this, propertyName);
    }
}
