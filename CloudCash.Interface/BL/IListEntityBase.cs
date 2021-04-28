namespace CloudCash.Interface.BL
{
    public interface IListModelBase
    {
        long ID { get; set; }
        string GetPropertyStringValue(string propertyName);

        void CheckValues();
    }
}
