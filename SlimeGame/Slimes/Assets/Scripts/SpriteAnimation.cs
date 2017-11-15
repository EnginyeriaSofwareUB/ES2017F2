using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimation
{

	private List<Sprite> sprites;
	private SpriteAnimationMode mode;
	private SpriteAnimationStatus status;
	private int cursor;
	private int currentImg;
	private SpriteRenderer spriteRenderer;

	//Images to increment
	private float dtPerImg;

	private float timeCounter;


	public SpriteAnimation (SpriteRenderer sp)
	{
		spriteRenderer = sp;
		status = SpriteAnimationStatus.PAUSED;
		mode = SpriteAnimationMode.BOUNCE;
		dtPerImg = 0.10f;
	}

	/* Getters and initializers
	 * 
	 */
	public void LoadSprites(string name,int imgQuantity){
		
		sprites = new List<Sprite> ();
		for (int i = 0; i < imgQuantity; i++) {
			sprites.Add (Resources.Load<Sprite>(name+i));
		}
	}

	public void setSprites(List<Sprite> list){
		sprites = list;
	}

	public void setSpriteRenderer(SpriteRenderer spr){
		this.spriteRenderer = spr;
	}

	public void playAnimation(){
		spriteRenderer.sprite = sprites[0];
		cursor = 1;
		status = SpriteAnimationStatus.PLAYING;
	}

	public void resetAnimation(){
		spriteRenderer.sprite = sprites[0];
		currentImg = 0;
		cursor = 1;
		status = SpriteAnimationStatus.PAUSED;
	}

	public void update(){
		float dt = Time.deltaTime;
		timeCounter += dt;
		if (status==SpriteAnimationStatus.PLAYING) {
			switch (mode) {
			case SpriteAnimationMode.BOUNCE:
				if (timeCounter > dtPerImg) {
					timeCounter -= dtPerImg;
					currentImg += cursor;
					if (currentImg >= sprites.Count) {
						cursor = -1;
						currentImg = sprites.Count - 2;
					}
					if (currentImg < 0) {
						cursor = 1;
						currentImg = 1;
					}
					spriteRenderer.sprite = sprites [currentImg];
				}
				break;
			case SpriteAnimationMode.LOOP:
				if (timeCounter > dtPerImg) {
					timeCounter -= dtPerImg;
					currentImg += cursor;
					currentImg %= sprites.Count;
					spriteRenderer.sprite = sprites [currentImg];
				}
				break;
			case SpriteAnimationMode.ONESHOT:
				break;
			case SpriteAnimationMode.REVERSE:
				break;
			}
		}
	}

	//NOTIFIER TO PARENT OBJECT
	public void notifyParent(){
	} 

}

