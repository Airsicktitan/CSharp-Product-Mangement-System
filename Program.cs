namespace ConsoleApp6
{
    internal class Program
    {
        private static Menu menu = new();
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to your Product Management App!\n");

            menu.MenuSelection();
        }
    }
}
