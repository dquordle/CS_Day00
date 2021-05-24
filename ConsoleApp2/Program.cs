using System;
using System.IO;
using System.Text.RegularExpressions;

int LevenshteinDistance(string s1, string s2)
{
    s1 = s1.ToLower();
    s2 = s2.ToLower();
    int[,] Matrix = new int[s1.Length + 1, s2.Length + 1];

    for (int i = 0; i <= s1.Length; i++)
    {
        for (int j = 0; j <= s2.Length; j++)
            Matrix[i, j] = 0;
    }

    for (int i = 0; i <= s1.Length; i++)
        Matrix[i, 0] = i;
    for (int j = 0; j <= s2.Length; j++)
        Matrix[0, j] = j;

    int substitution_Cost;
    for (int i = 1; i <= s1.Length; i++)
    {
        for (int j = 1; j <= s2.Length; j++)
        {
            substitution_Cost = s1[i - 1] == s2[j - 1] ? 0 : 1;

            Matrix[i, j] = Math.Min(Matrix[i - 1, j] + 1, Matrix[i, j - 1] + 1);
            Matrix[i, j] = Math.Min(Matrix[i - 1, j - 1] + substitution_Cost, Matrix[i, j]);
        }
    }

    return Matrix[s1.Length, s2.Length];
}

if (!File.Exists("./us.txt"))
{
    Console.WriteLine("Файл со списком имен не найден");
    return 1;
}

string[] names = File.ReadAllLines("./us.txt");

Console.WriteLine("Enter Name: ");
string user = Console.ReadLine();

if (string.IsNullOrEmpty(user))
{
    Console.WriteLine("Your name was not found");
    return 0;
}

if (!Regex.IsMatch(user, @"^[a-zA-Z _]+$"))
{
    Console.WriteLine("Invalid name");
    return 0;
}

string[] dist_1 = new string[names.Length];
string[] dist_2 = new string[names.Length];
string[] dist_3 = new string[names.Length];
int d1, d2, d3;
d1 = d2 = d3 = 0;
foreach (var name in names)
{
    if (name == user)
    {
        Console.WriteLine($"Hello, {name}!");
        return 0;
    }

    int distance = LevenshteinDistance(name, user);
    
    if (distance == 1)
    {
        dist_1[d1] = name;
        d1++;
    }
    else if (distance == 2)
    {
        dist_2[d2] = name;
        d2++;
    }
    if (distance == 3)
    {
        dist_3[d3] = name;
        d3++;
    }
}

int i = 0;
while (d1 > 0 || d2 > 0 || d3 > 0)
{
    string option;
    if (d1 > 0)
    {
        option = dist_1[i++];
        if (i == d1)
            i = d1 = 0;
    }
    else if (d2 > 0)
    {
        option = dist_2[i++];
        if (i == d2)
            i = d2 = 0;
    }
    else
    {
        option = dist_3[i++];
        if (i == d3)
            i = d3 = 0;
    }
    Console.WriteLine($"Did you mean \"{option}\"? Y/N");
    string answer = Console.ReadLine();
    while (answer != "Y" && answer != "N")
    {
        Console.WriteLine("Wrong input, try again!");
        answer = Console.ReadLine();
    }

    if (answer == "Y")
    {
        Console.WriteLine($"Hello, {option}!");
        return 0;
    }
}
Console.WriteLine("Your name was not found.");
return 0;
