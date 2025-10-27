
using DoubleList.core;
using System.ComponentModel.Design;
var list = new DoubleLinkedList<string>();
var opc = "0";
do
{
    opc = Menu();
    switch (opc)
    {
        case "0":
            Console.WriteLine("leaving the menu...");
            break;
        case "1":
            Console.Write("Enter the data to insert at the beginning: ");
            var insertdata = Console.ReadLine();
            if (insertdata != null)
            {

                list.Insert(insertdata);
            }
            break;
        case "2":
            Console.WriteLine(list.GetForward());
            break;
        case "3":
            Console.WriteLine(list.GetBackward());
            break;
        case "4":
            list.OrderDecently();
            Console.WriteLine("List sorted in descending order!");

            break;
        case "5":
            var mode = list.GetMode();
            Console.WriteLine($"The mode(s) is: {mode}");
            break;
        case "6":
            Console.WriteLine("Occurrences graph:\n");
            Console.WriteLine(list.GetGraph());
            break;
        case "7":
            Console.Write("Enter the data to search: ");
            var search = Console.ReadLine();
            if (search != null)
            {
                bool exists = list.Exists(search);
                if (exists)
                {
                    Console.WriteLine($"The data {search} exists in the list.");
                }
                else
                {
                    Console.WriteLine($"The data {search} does not exist in the list.");

                }
            }
            break;
        case "8":
            Console.Write("Enter the data to remove: ");
            var remove = Console.ReadLine();
            if (remove != null)
            {
                list.Remove(remove);
                Console.WriteLine("Occurrence removed");
            }
            break;
        case "9":
            Console.Write("Enter the data to remove all occurrences: ");
            var removeAll = Console.ReadLine();
            if (removeAll != null)
            {
                list.RemoveAll(removeAll);
                Console.WriteLine("All occurrences removed.");
            }
            break;

        default:
            Console.WriteLine("Invalid option");
            break;
    }
} while (opc != "0");
string Menu()
{
    Console.WriteLine();
    Console.WriteLine("1. Inser at beginning");
    Console.WriteLine("2. Show list forward");
    Console.WriteLine("3. Show list backward");
    Console.WriteLine("4. Sort list descending");
    Console.WriteLine("5. Show the mode");
    Console.WriteLine("6. Show graph");
    Console.WriteLine("7. Data exist");
    Console.WriteLine("8. Delete an occurrence");
    Console.WriteLine("9. Delete all occurrences");
    Console.WriteLine("0. Exit");
    Console.Write("Choose an option: ");
    return Console.ReadLine() ?? "0";
    Console.WriteLine();
}