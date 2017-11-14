public enum SlimeCoreTypes{
	Agile,
    Aggressive
}

static class SlimeCoreTypesCtrl{
	
	public static string GetPath(SlimeCoreTypes type){
		string root = "Slimes/";

		switch (type) {

		case SlimeCoreTypes.Agile:
			return root + "agile";

		case SlimeCoreTypes.Aggressive:
			return root + "aggressive";

		default:
			return root + "agile";
		}
	}

	public static string GetSprite(SlimeCoreTypes type){
		string root = "Test/";

		switch (type) {

		case SlimeCoreTypes.Agile:
			return root + "slime2";

		case SlimeCoreTypes.Aggressive:
			return root + "slime";

		default:
			return root + "slime";
		}
	}
}