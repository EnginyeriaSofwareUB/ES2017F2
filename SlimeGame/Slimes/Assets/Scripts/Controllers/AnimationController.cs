using System;
using UnityEngine;
using System.Collections;

/**
 * Class for controlling all abilities movement and all this stuff of the game entities
 * 
 */
public class AnimationController
{
	/*
	private Hashtable walkingAnimationHashTable;
	private WalkingDirection.Direction walkingDirection;

	private SpriteAnimation currentAnimation;

	//TODO to be moved to other site
	private int walkingAnimationImageCounter=8;

	public AnimationController (SpriteRenderer sp,EntityCode entityCode)
	{

		walkingAnimationHashTable = new Hashtable ();

		foreach (WalkingDirection.Direction w in Enum.GetValues(typeof(WalkingDirection.Direction))) {
			
			string imgName = entityCode.ToString () + "/WalkingAnimation/Walking" + WalkingDirection.getDirectionAsCode (w);
			SpriteAnimation spAnimation = new SpriteAnimation(sp);
			spAnimation.LoadSprites(imgName,walkingAnimationImageCounter);
			walkingAnimationHashTable.Add (WalkingDirection.getDirectionAsCode(w),spAnimation);

		}

		currentAnimation = (SpriteAnimation) walkingAnimationHashTable [WalkingDirection.getDirectionAsCode (WalkingDirection.Direction.NONE)];
		walkingDirection = WalkingDirection.Direction.NORTH;
		notifyNewDirection (WalkingDirection.Direction.NONE);
	}


	public void update(){
		currentAnimation.update();
	}

	public void notifyNewDirection(WalkingDirection.Direction newDirection){
		if (walkingDirection != newDirection) {
			currentAnimation.resetAnimation ();
			currentAnimation = (SpriteAnimation) walkingAnimationHashTable [WalkingDirection.getDirectionAsCode (newDirection)];
			currentAnimation.playAnimation ();
			walkingDirection = newDirection;
		}
	}
	*/
}
