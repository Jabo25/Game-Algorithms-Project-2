//this file is responsible for swaping Paralax scrolling backgrounds by layer. 
//the point of this file is that each background needs a backGround ID and a inner/outerBackground status
//swapBacground gives each layer an inner/outer background status and dissables the current background swapping it for a new one. 
//the inner and outer status tells the program if it needs to swap the inner or outer background. 

//goals research how the hierarchy works in unity. also start the project for applied NLP if possible. 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swapBackground : MonoBehaviour
{
    [Tooltip("inside or outdide paralax layer")]
    public bool innerBackground;
    
    [Tooltip("set backgroundID")]
    public int backgroundID;

    // SpriteRenderer.forceRenderingOff = True;//turn off spriterenderer
// SpriteRenderer.forceRenderingOff = False;//turn off spriterenderer

}