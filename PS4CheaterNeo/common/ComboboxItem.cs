namespace PS4CheaterNeo
{
    public class ComboboxItem
    {
        public object Text { get; set; }
        public object Value { get; set; }

        public ComboboxItem(object text, object value=null)
        {
            Text = text;
            Value = value;
        }

        public override string ToString() => Text.ToString();
    }
}
