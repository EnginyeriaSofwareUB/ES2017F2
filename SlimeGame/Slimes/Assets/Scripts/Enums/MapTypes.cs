public enum MapTypes{
	Small,
	Medium,
	Big
}

static class MapTypesCtrl{
	
	public static string GetPath(MapTypes type){
		string root = "Maps/";

		switch (type) {

		case MapTypes.Small:
			return root + "small";

		case MapTypes.Medium:
			return root + "medium";

		case MapTypes.Big:
			return root + "big";

		default:
			return root + "medium";
		}
	}
}