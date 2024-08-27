namespace KarnelTravel.API;

public static class IdentityClients
{
	public const string PORTAL = "c437faac-c42d-42fc-84cb-e4faabc0ab43";
	public const string MOBILE = "e343ae58-24c4-42ca-a986-487613662cc5";

	public readonly static IReadOnlyDictionary<string, string> CLIENT_SECRET =
		new Dictionary<string, string>
		{
			{ PORTAL, "D869DCC7401043879239A534148042A6" },
			{ MOBILE, "090DD3837E4E47428BD2C12293D27A34" },
		};
}