using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace PropTweaks.Mods;

public class PlayerPatcher : IScriptMod
{
	public bool ShouldRun(string path) => path == "res://Scenes/Entities/Player/player.gdc";

	public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens)
	{
		Mod.instance.logger.Information("found file or something idfk if thats how it works lol");
		MultiTokenWaiter createPropWaiterBegin = new MultiTokenWaiter([
			t=>t is IdentifierToken {Name: "prop"},
			t=>t is IdentifierToken {Name: "prop"},
			t=>t.Type is TokenType.CfIf
		], allowPartialMatch: true);
		MultiTokenWaiter createPropWaiterEnd = new MultiTokenWaiter([
			t=>t is IdentifierToken {Name: "prop"},
			t=>t is IdentifierToken {Name: "prop"},
			t=>t is IdentifierToken {Name: "ref"},
			t=>t.Type is TokenType.Colon,
			t=>t.Type is TokenType.Colon
		], allowPartialMatch: true);

		foreach (Token token in tokens)
		{
			if (createPropWaiterBegin.Check(token))
			{
				Mod.instance.logger.Information("found begin");
				yield return token;

				yield return new ConstantToken(new BoolVariant(false));
				yield return new Token(TokenType.OpAnd);
				yield return new Token(TokenType.ParenthesisOpen);
			}
			else if (createPropWaiterEnd.Check(token))
			{
				Mod.instance.logger.Information("found end");
				yield return new Token(TokenType.ParenthesisClose);
				yield return token;
			}
			else yield return token;
		}
	}
}