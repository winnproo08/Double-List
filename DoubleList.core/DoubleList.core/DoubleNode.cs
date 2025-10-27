namespace DoubleList.core
{
    public class DoubleNode<T>
    {
        public DoubleNode(T data)
        {
            Data = data;
            Prev = null;
            Next = null;
        }

        public DoubleNode<T>? Prev { get; set; }
        public T? Data { get; set; }
        public DoubleNode<T>? Next { get; set; }




    }

}
