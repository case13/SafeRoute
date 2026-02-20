namespace SafeRoute.BlazorServer.Components.Selects
{
    public sealed class SelectBoxOption<TValue>
    {
        public SelectBoxOption()
        {
        }

        public SelectBoxOption(TValue value, string text)
        {
            Value = value;
            Text = text;
        }

        public TValue Value { get; set; } = default!;
        public string Text { get; set; } = string.Empty;
    }
}
