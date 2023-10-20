using System;

class Program
{
    /// <summary>
    /// Returns the sum of all numbers between n and m, inclusively.
    /// </summary>
    static int SumRecursive(int n, int m)
    {
        if (n > m) return SumRecursive(m, n);
        if (n == m) return n;
        return n + SumRecursive(n + 1, m);
    }

    /// <summary>
    /// Counts how many times the number can be divided by 2 before it becomes odd.
    /// </summary>
    static int DivideByTwoRecursive(int num, int count = 0)
    {
        if (num <= 0) throw new ArgumentException("Number must be positive");
        if (num % 2 != 0) return count;
        return DivideByTwoRecursive(num / 2, count + 1);
    }

    static void Main(string[] args)
    {
        // Sum of numbers between n and m
        Console.WriteLine("Recursive Addition");
        Console.WriteLine("-------------------------");
        Console.Write("Enter number n: ");
        int n = int.Parse(Console.ReadLine());
        Console.Write("Enter number m: ");
        int m = int.Parse(Console.ReadLine());
        int result = SumRecursive(n, m);
        Console.WriteLine($"Sum of numbers between {n} and {m} is: {result}");
        Console.WriteLine();

        // Count how many times the number can be divided by 2
        Console.WriteLine("Recursive Division by Two");
        Console.WriteLine("-------------------------");
        Console.Write("Enter your number: ");
        int num = int.Parse(Console.ReadLine());
        int count = DivideByTwoRecursive(num);
        Console.WriteLine($"{num} can be divided by 2 for {count} times before it becomes odd.");
    }
}