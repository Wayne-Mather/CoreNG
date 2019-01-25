namespace CoreNg.RequestResponse.Interfaces
{
    public class SearchResult<T>
    {
        public bool IsSelected { get; set; }
        public T Result { get; set; }

        public void Fill(T result)
        {
            this.Result = result;
        }
    }
}