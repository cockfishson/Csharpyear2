using System;
using System.Text;

class StringProcessor
{
    public static string RemovePunc(string input)
    {
        StringBuilder result = new();
        foreach (char c in input)
        {
            if (!char.IsPunctuation(c))
            {
                result.Append(c);
            }
        }
        return result.ToString();
    }

    public static string AddSymbols(string input, string symbols)
    {
        return input + symbols;
    }

    public static string ToUpperCase(string input)
    {
        return input.ToUpper();
    }

    public static string RemoveSpaces(string input)
    {
        string[] words = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        return string.Join(" ", words);
    }

    public static string CustomProcessing(string input, Predicate<char> condition, Func<char, char> transformation)
    {
        StringBuilder result = new();
        foreach (char c in input)
        {
            if (condition(c))
            {
                result.Append(transformation(c));
            }
            else
            {
                result.Append(c);
            }
        }
        return result.ToString();
    }
}
public class Game
{

    public event Action<Player, int> Poison;
    public event Action<Player, int> Heal;
    public event Func<Player,int> SomeFunc
    public void DoPoison(Player player, int damage)
    {
        Poison?.Invoke(player, damage);
    }

    public void DoHeal(Player player, int amount)
    {
        Heal?.Invoke(player, amount);
    }
}

public class Player
{
    private int health;
    public string Name { get; private set; }
    public int Health
    {
        get => health;
        private set
        {
            health = value;
            Console.WriteLine($"{Name}'s health is now {health}");
        }
    }
    public bool Undead;

    public Player(string name, int initialHealth, bool undead)
    {
        Name = name;
        Health = initialHealth;
        Undead = undead;
    }

    public void SubscribeToEvents(Game game)
    {
        game.Poison += (player,damage) =>
        {  
                if (Undead != false) { 
                    Health -= damage;
                    Console.WriteLine($"{Name} was Poisoned  for {damage} damage.");
                }
                else
                {
                    Health += damage;
                    Console.WriteLine($"{Name} was Healed for {damage} damage.");
                }
        };

        game.Heal += (healer, amount) =>
        {
                if (Undead != false)
                {
                    Health += amount;
                    Console.WriteLine($"{Name} was healed for {amount} health.");
                }
                else { 
                    health -= amount; 
                    Console.WriteLine($"{Name} was Poisoned for {amount} damage.");
                }
        };
    }

}

class Program
{
    static void Main()
    {
        Game game = new Game();
        string temp = Console.ReadLine();
        Player player1 = new Player(temp, 100,true);
        temp = Console.ReadLine();
        Player player2 = new Player(temp, 120,false); 
        player1.SubscribeToEvents(game);
        player2.SubscribeToEvents(game);
        
        game.DoPoison(player1, 20);
        game.DoHeal(player2, 10);
        string initialString = "My name is Pavel and I love C#";

        Func<string, string> removePunc = StringProcessor.RemovePunc;
        Func<string, string, string> addSymbols = StringProcessor.AddSymbols;
        Func<string, string> toUpperCase = StringProcessor.ToUpperCase;
        Func<string, string> removeSpaces = StringProcessor.RemoveSpaces;
        Func<string, Predicate<char>, Func<char, char>, string> customProcessing = StringProcessor.CustomProcessing;

        string result = removePunc(initialString);
        result = addSymbols(result, "!!!");
        result = toUpperCase(result);
        result = removeSpaces(result);

        Predicate<char> isVowel = c => "SOIDEncA".IndexOf(c) != -1;
        Func<char, char> toLower = char.ToLower;

        result = customProcessing(result, isVowel, toLower);

        // Вывод результата
        Console.WriteLine(result);
    }
}
