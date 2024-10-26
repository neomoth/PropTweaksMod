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
		MultiTokenWaiter createPropWaiter = new MultiTokenWaiter([
			t=>t is IdentifierToken {Name: "prop"},
			t=>t is IdentifierToken {Name: "prop"},
			t=>t.Type is TokenType.CfIf
		], allowPartialMatch: true);
		MultiTokenWaiter createPropWaiter1 = new MultiTokenWaiter([
			t=>t is IdentifierToken {Name: "prop"},
			t=>t is IdentifierToken {Name: "prop"},
			t=>t is IdentifierToken {Name: "ref"},
			t=>t.Type is TokenType.Colon,
			t=>t.Type is TokenType.Colon
		], allowPartialMatch: true);
		MultiTokenWaiter createPropWaiter2 = new MultiTokenWaiter([
			t=>t is IdentifierToken {Name: "prop"},
			t=>t is IdentifierToken {Name: "prop"},
			t=>t is IdentifierToken {Name: "prop_ids"},
			t=>t is IdentifierToken {Name: "size"},
			t=>t.Type is TokenType.Colon
		], allowPartialMatch: true);
		MultiTokenWaiter createPropWaiter3 = new MultiTokenWaiter([
			t=>t is IdentifierToken {Name: "prop"},
			t=>t is IdentifierToken {Name: "prop"},
			t=>t is IdentifierToken {Name: "prop_ids"},
			t=>t is IdentifierToken {Name: "size"},
			t=>t.Type is TokenType.Colon,
			t=>t.Type is TokenType.CfReturn
		], allowPartialMatch: true);

		foreach (Token token in tokens)
		{
			if (createPropWaiter.Check(token))
			{
				Mod.instance.logger.Information("found begin");
				yield return token;

				yield return new ConstantToken(new BoolVariant(false));
				yield return new Token(TokenType.OpAnd);
				yield return new Token(TokenType.ParenthesisOpen);
			}
			else if (createPropWaiter1.Check(token))
			{
				Mod.instance.logger.Information("found end");
				yield return new Token(TokenType.ParenthesisClose);
				yield return token;
			}
			else if (createPropWaiter2.Check(token))
			{
				yield return new Token(TokenType.OpAnd);
				yield return new ConstantToken(new BoolVariant(false));
				yield return token;
			}
			else if (createPropWaiter3.Check(token))
			{
				yield return token;
				yield return new Token(TokenType.Newline, 1);
				yield return new Token(TokenType.CfIf);
				yield return new Token(TokenType.OpNot);
				yield return new IdentifierToken("Network");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("GAME_MASTER");
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.Newline, 2);
				yield return new Token(TokenType.CfIf);
				yield return new IdentifierToken("prop_ids");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("size");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.OpGreater);
				yield return new ConstantToken(new IntVariant(4));
				yield return new Token(TokenType.Colon);
				yield return new Token(TokenType.Newline, 3);
				yield return new IdentifierToken("PlayerData");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_send_notification");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("prop limit reached"));
				yield return new Token(TokenType.Comma);
				yield return new ConstantToken(new IntVariant(1));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 3);
				yield return new IdentifierToken("PlayerData");
				yield return new Token(TokenType.Period);
				yield return new IdentifierToken("_send_notification");
				yield return new Token(TokenType.ParenthesisOpen);
				yield return new ConstantToken(new StringVariant("only lobby host can exceed prop limit"));
				yield return new Token(TokenType.Comma);
				yield return new ConstantToken(new IntVariant(1));
				yield return new Token(TokenType.ParenthesisClose);
				yield return new Token(TokenType.Newline, 3);
				yield return new Token(TokenType.CfReturn);
				yield return new Token(TokenType.Newline, 1);
			}
			else yield return token;
		}
	}
}













