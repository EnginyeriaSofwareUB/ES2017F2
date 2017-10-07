public enum MapTypes{
	Small,
	Medium,
	Big
}

static class MapTypesCtrl{
	
	public static string GetPath(MapTypes type){
		string root = "Assets/Resources/Maps/";

		switch (type) {

		case MapTypes.Small:
			return root + "small.txt";

		case MapTypes.Big:
			return root + "big.txt";

		default:
			return root + "medium.txt";
		}
	}
}