using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoubleList.core
{
    public class DoubleLinkedList<T>
    {
        private DoubleNode<T>? _head;
        private DoubleNode<T>? _tail;
        public DoubleLinkedList()
        {
            _tail = null;
            _head = null;
        }
        public void Insert(T data)
        {
            var newNode = new DoubleNode<T>(data);
            var newData = data?.ToString();
            if (_head == null)
            {
                _head = newNode;
                _tail = newNode;
                return;
            }
            var current = _head;
            while (current != null && current.Data?.ToString()?.CompareTo(newData) <= 0)
            {
                current = current.Next;
            }
            if (current == _head)
            {
                newNode.Next = _head;
                _head.Prev = newNode;
                _head = newNode;
            }
            else if (current == null)
            {
                newNode.Prev = _tail;
                _tail!.Next = newNode;
                _tail = newNode;
            }
            else
            {
                newNode.Next = current;
                newNode.Prev = current.Prev;
                current.Prev!.Next = newNode;
                current.Prev = newNode;
            }
        }
        
        //print  the list from head to tail
        public string GetForward()
        {
            //var exit
            var output = string.Empty;
            var current = _head;
            while (current != null)
            {
                output += $"{current.Data}, ";
                current = current.Next;
            }
            return output.Substring(0, output.Length - 2);
        }
        //print the list from tail to head
        public string GetBackward()
        {
            //var exit
            var output = string.Empty;
            var current = _tail;
            while (current != null)
            {
                output += $"{current.Data}, ";
                current = current.Prev;
            }
            return output.Substring(0, output.Length - 2);
        }
        public void OrderDecently()
        {
            var current = _head;
            while (current != null)
            {
                var change = current.Next;
                while (change != null)
                {
                    if (current.Data?.ToString()?.CompareTo(change.Data?.ToString()) < 0)
                    {
                        var aux = current.Data;
                        current.Data = change.Data;
                        change.Data = aux;
                    }
                    change = change.Next;
                }
                current = current.Next;
            }

        }
       
        
        public string GetMode()
        {
            if (_head == null)
                return "The list is empty.";
            var current = _head;
            int max = 0;
            // Primero determinamos la frecuencia máxima
            while (current != null)
            {
                int count = 0;
                var temp = _head;
                while (temp != null)
                {
                    if (temp.Data!.Equals(current.Data))
                        count++;
                    temp = temp.Next;
                }
                if (count > max)
                    max = count;
                current = current.Next;
            }
            // Ahora buscamos todos los elementos con esa frecuencia máxima
            current = _head;
            string modes = "";
            while (current != null)
            {
                int count = 0;
                var temp = _head;
                while (temp != null)
                {
                    if (temp.Data!.Equals(current.Data))
                        count++;
                    temp = temp.Next;
                }

                if (count == max && !modes.Contains(current.Data!.ToString()!))
                {
                    modes += $"{current.Data}, ";
                }
                current = current.Next;
            }
            return modes = modes.Substring(0, modes.Length - 2);
          


        }
        public string GetGraph()
        {
            if (_head == null)
                return "The list is empty.";
            var current = _head;
            var counted = new List<T>();
            var result = string.Empty;
            while (current != null)
            {
                if (!counted.Contains(current.Data))
                {
                    int count = 0;
                    var temp = _head;
                    while (temp != null)
                    {
                        if (temp.Data!.Equals(current.Data))
                            count++;
                        temp = temp.Next;
                    }
                    result += $"{current.Data} ";
                    for (int i = 0; i < count; i++)
                        result += "*";
                    result += "\n";
                    counted.Add(current.Data);
                }
                current = current.Next;
            }
            return result;
        }
        public bool Exists(T data)
        {
            var current = _head;
            while (current != null)
            {
                if (current.Data!.Equals(data))
                {
                    return true;
                }
                current = current.Next;
            }
            return false;
           
        }
        public void Remove(T data)
        {
            var current = _head;
            while (current != null)
            {
                if (current.Data!.Equals(data))
                {
                    if (current.Prev != null)
                    {
                        current.Prev.Next = current.Next;
                    }
                    else
                    {
                        _head = current.Next; //Remove head
                    }
                    if (current.Next != null)
                    {
                        current.Next.Prev = current.Prev;
                    }
                    else
                    {

                        _tail = current.Prev; //Remove tail
                    }
                    break;


                }
                current = current.Next;
            }
        }
        public void RemoveAll(T data)
        {
            var current = _head;
            while (current != null)
            {
                var next = current.Next;
                if (current.Data!.Equals(data))
                {
                    if (current.Prev != null)
                        current.Prev.Next = current.Next;
                  
                      
                    else
                        _head = current.Next;
                    if (current.Next != null)
                        current.Next.Prev = current.Prev;
                    else
                        _tail = current.Prev;
                }
                current = next;
            }
        }
    }
}

