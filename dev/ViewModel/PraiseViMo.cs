using FateExplorer.GameData;
using FateExplorer.Shared;
using System;
using System.Linq;

namespace FateExplorer.ViewModel;

public class PraiseViMo
{
    public const int Praise = 0;
    public const int Insult = 1;

    protected PraiseOrInsultDB Data;
    protected Random Rnd;

    public PraiseViMo(IGameDataService gameData)
    {
        this.Data = gameData.PraiseOrInsult ?? throw new ArgumentNullException(nameof(gameData));
        Rnd = new();
    }

    public string GetFateQuote()
    {
        int i = Rnd.Next(Data.FateQuote.Length);
        return Data.FateQuote[i];
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="tone"></param>
    /// <param name="check"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public string Give(int tone, Check check) 
    {
        int Scope = check.IsCombat ? 1 : 255;
        if (tone == Praise) 
        {
            if (Rnd.Next(2) > 0) // predefined phrase or one created from adjective
            {   // Create a sentence using a praise adjective
                int i = Rnd.Next(Data.PraisePhrase.Length);
                string Sentence = Data.PraisePhrase.ElementAt(i);
                i = Rnd.Next(Data.PositiveAdjective.Length);
                string Adjective = Data.PositiveAdjective[i];
                return string.Format(Sentence, Adjective);
            }
            else
            {   // Create a predefined sentence
                var Sub = Data.Praise.Where(t => (t.Scope | Scope) > 0);
                int i = Rnd.Next(Sub.Count());
                return Sub.ElementAt(i).Text;
            }
        }
        if (tone == Insult)
        {
            var Sub = Data.Insult.Where(t => (t.Scope | Scope) > 0);
            int i = Rnd.Next(Sub.Count());
            return Data.Insult.ElementAt(i).Text;
        }
        throw new ArgumentException("Unknown form of praise or insult");
    }
}
