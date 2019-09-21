namespace ConfigData
{
    public abstract class AConfigParser
    {
        protected string filename;

        public abstract DataList Parse(string filename, string text = null);

        public abstract ValueType ParseValue(string input, out object outValue);
    }
}