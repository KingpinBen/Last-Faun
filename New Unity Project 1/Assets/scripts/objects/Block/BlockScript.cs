using UnityEngine;

public class BlockScript : MonoBehaviour
{
	public BlockTrackTarget currentlyOccupying
	{
		get {
			return _currentlyOccupying;	
		}
		set {
			_currentlyOccupying =value;	
		}
	}
	
    private BlockTrackTarget _currentlyOccupying;
}
